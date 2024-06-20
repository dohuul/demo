using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using Amazon.S3;
using Amazon.S3.Model;
using MathNet.Numerics.LinearAlgebra.Complex.Solvers;
using MathNet.Numerics.Statistics;
using QA.Common.Models;
using System.Net;
using System.Text;


namespace QA.Common.Utilities
{
    public static class AWSUtilities
    {
       
    }

    public static class AWSS3Utilities
    {
        private static string GetLifeSpan(LifeSpan lifeSpan)
        {
            return lifeSpan switch
            {
                LifeSpan.OneDay => "1d",
                LifeSpan.NinetyDays => "90d",
                LifeSpan.SevenYears => "7y",
                LifeSpan.OneYear => "1y",
                _ => "7y"
            };
        }

        public static async Task<PutObjectResponse> SaveToS3<T>(IAmazonS3 S3Client, T document, string bucket, string key, string kmsKeyId,
            int cacheDays = 0, Dictionary<string, string> metaData = null, LifeSpan lifeSpan = LifeSpan.SevenYears, bool isAES256Encryption = false)
        {
            PutObjectResponse result = null;
            string value = string.Empty;
            metaData?.TryGetValue("Content-Type", out value);

            using (MemoryStream ms = new MemoryStream())
            {
                if (key.EndsWith("json") || value.EndsWith("json"))
                {
                    SerializationUtilities.SerializeJson(document, ms);
                    value = "application/json";
                }
                else if (key.EndsWith("xml") || value.EndsWith("xml"))
                {
                    SerializationUtilities.SerializeXml(document, ms);
                    value = "application/xml";
                }
                else
                {
                    string s = document.ToString();
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(s)))
                    {
                        memoryStream.CopyTo(ms);
                    }
                    value = (string.IsNullOrEmpty(value) ? "text/plain" : value);
                }

                ms.Position = 0L;
                PutObjectRequest putObjectRequest = new PutObjectRequest
                {
                    BucketName = bucket,
                    Key = key,
                    InputStream = ms,
                    ServerSideEncryptionMethod = ((!string.IsNullOrEmpty(kmsKeyId)) ? ServerSideEncryptionMethod.AWSKMS : (isAES256Encryption ? ServerSideEncryptionMethod.AES256 : null)),
                    ServerSideEncryptionKeyManagementServiceKeyId = ((!string.IsNullOrEmpty(kmsKeyId)) ? kmsKeyId : null),
                    ContentType = value

                };

                if (cacheDays > 0)
                {
                    putObjectRequest.Headers.ExpiresUtc = DateTime.UtcNow.AddDays(cacheDays);
                }

                if (metaData != null)
                {
                    foreach (KeyValuePair<string, string> metaDatum in metaData)
                    {
                        putObjectRequest.Metadata.Add(metaDatum.Key, metaDatum.Value);
                    }
                }

                putObjectRequest.TagSet = new List<Amazon.S3.Model.Tag>();
                Amazon.S3.Model.Tag item = new Amazon.S3.Model.Tag
                {
                    Key = "LifeSpan",
                    Value = GetLifeSpan(lifeSpan)
                };

                putObjectRequest.TagSet.Add(item);
                result = await S3Client.PutObjectAsync(putObjectRequest);
            }
            return result;
        }


        public static async Task<T> SearchS3ByKey<T>(IAmazonS3 S3Client, string bucket, string key, bool log404Error = false, int? cacheLimit = null)
        {
            return await SearchS3ByKeyFilterByMetaData<T>(S3Client, bucket, key, null, log404Error, cacheLimit);
        }

        public static async Task<T> SearchS3ByKeyFilterByMetaData<T>(IAmazonS3 S3Client, string bucket, string key,
             Dictionary<string, string> metaData, bool log404Error = false, int? cacheLimit = null)
        {
            T document = default(T);
            GetObjectResponse getObjectResponse;
            try
            {
                getObjectResponse = await S3Client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = key
                });
            }
            catch (AmazonS3Exception ex)
            {
                if (!log404Error && ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return document;
                }

                throw;
            }
            if (cacheLimit.HasValue)
            {
                int value = cacheLimit.Value;
                if (getObjectResponse.LastModified.CompareTo(DateTime.Now.AddDays(-value)) < 0)
                {
                    return document;
                }
            }

            if (metaData != null)
            {
                foreach (KeyValuePair<string, string> metaDatum in metaData)
                {
                    if (!getObjectResponse.Metadata[metaDatum.Key].Equals(metaData[metaDatum.Key], StringComparison.OrdinalIgnoreCase))
                    {
                        return document;
                    }
                }
            }

            using (StreamReader sr = new StreamReader(getObjectResponse.ResponseStream))
            {
                string text = getObjectResponse?.Metadata?["x-amz-meta-content-type"] ?? getObjectResponse?.Headers?.ContentType ?? string.Empty;
                if (key.EndsWith("json") || text.EndsWith("json"))
                {
                    document = SerializationUtilities.DeserializeJson<T>(sr);
                }
                else if (key.EndsWith("xml") || text.EndsWith("xml"))
                {
                    document = SerializationUtilities.DeserializeXml<T>(sr);
                }
                else
                {
                    if (!(typeof(T) == typeof(string)))
                    {
                        throw new Exception($"Unable to deserialize document to object as no file extension is specified. Bucker={bucket}, Key={key}");
                    }

                    document = (T)(object)(await sr.ReadToEndAsync());
                }
            }

            return document;
        }


        public static async Task<CopyObjectResponse> CopyS3Object(IAmazonS3 S3Client, string sourceBucket, string destinationBucket, string sourceKey,
            string destinationKey, string contentType, string kmsKeyId, int cacheDays = 0, Dictionary<string, string> metaData = null, LifeSpan lifeSpan = LifeSpan.SevenYears)
        {
            CopyObjectRequest copyObjectRequest = new CopyObjectRequest
            {
                SourceBucket = sourceBucket,
                DestinationBucket = destinationBucket,
                SourceKey = sourceKey,
                DestinationKey = destinationKey,
                ServerSideEncryptionMethod = ((!string.IsNullOrEmpty(kmsKeyId)) ? kmsKeyId : null),
                ContentType = contentType
            };
            if (cacheDays > 0)
            {
                copyObjectRequest.Headers.ExpiresUtc = DateTime.UtcNow.AddDays(cacheDays);
            }

            if (metaData != null)
            {
                foreach (KeyValuePair<string, string> metaDatum in metaData)
                {
                    copyObjectRequest.Metadata.Add(metaDatum.Key, metaDatum.Value);
                }
            }

            copyObjectRequest.TagSet = new List<Amazon.S3.Model.Tag>();
            Amazon.S3.Model.Tag item = new Amazon.S3.Model.Tag
            {
                Key = "LifeSpan",
                Value = GetLifeSpan(lifeSpan)
            };
            copyObjectRequest.TagSet.Add(item);
            return await S3Client.CopyObjectAsync(copyObjectRequest);
        }

        public static async Task<DeleteObjectResponse> DeleteS3Object(IAmazonS3 S3Client, string bucket, string key)
        {
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = bucket,
                Key = key
            };

            return await S3Client.DeleteObjectAsync(request);
        }

        public static async Task<bool> S3BucketExits(IAmazonS3 S3Client, string bucket)
        {
            return await S3Client.DoesS3BucketExistAsync(bucket);
        }
    }

    public static class AWSDynamoUtilities
    {
        public static async Task SaveToDynamoDB<T>(IDynamoDBContext context, string table, string hashKey)
        {
            DynamoDBOperationConfig operationConfig = new DynamoDBOperationConfig
            {
                OverrideTableName = table
            };
            await context.SaveAsync(table, operationConfig);
        }

        public static async Task<T> GetFromDynamoDB<T>(IDynamoDBContext context, string table, string hashKey)
        {
            DynamoDBOperationConfig operationConfig = new DynamoDBOperationConfig
            {
                OverrideTableName = table
            };

            return await context.LoadAsync<T>(table, operationConfig);
        }

        public static async Task<UpdateItemResponse> UpdateDynamoDB(IAmazonDynamoDB dbClient, string table, string keyName, string keyValue, string updateExpression, 
            Dictionary<string, AttributeValue> expresionAttributeValues, ReturnValue returnValue)
        {
            UpdateItemRequest request = new UpdateItemRequest
            {
                TableName = table,
                UpdateExpression = updateExpression,
                ExpressionAttributeValues = expresionAttributeValues,
                ReturnValues = returnValue,
                Key = new Dictionary<string, AttributeValue> {
                {
                    keyName,
                    new AttributeValue
                    {
                        S = keyValue
                    }
                }}
            
            };
            return await dbClient.UpdateItemAsync(request);
        }

        public static async Task<QueryResponse> QueryDynamoDB(IAmazonDynamoDB dbClient, string table, string keyExpression,
            Dictionary<string,AttributeValue> expressionAttributeValues, int limit = 0, string returnConsumeCapacity="NONE",string index=null)
        {
            QueryRequest queryRequest = new QueryRequest
            {
                TableName = table,
                KeyConditionExpression = keyExpression,
                ExpressionAttributeValues = expressionAttributeValues,
                ReturnConsumedCapacity = returnConsumeCapacity
            };

            if(index != null)
            {
                queryRequest.IndexName = index;
            }
            if(limit > 0)
            {
                queryRequest.Limit = limit;
            }
            return await dbClient.QueryAsync(queryRequest);
        }

        public static async Task<bool> DynamoTableExists(IAmazonDynamoDB dbClient, string table)
        {
            bool exists = false;
            DescribeTableResponse describeTableResponse = await dbClient.DescribeTableAsync(table);
            if(describeTableResponse?.Table != null && describeTableResponse.Table.TableStatus == TableStatus.ACTIVE)
            {
                exists = true;
            }
            return exists;
        }
    }

    public static class AWSLambdaUtilities
    {
        public static async Task<InvokeResponse> InvokeLambda<T>(IAmazonLambda client, string lambdaARN, T request, string requestType)
        {
            InvokeRequest invokeRequest = new InvokeRequest
            {
                FunctionName = lambdaARN,
                Payload = SerializationUtilities.SerializeContent(requestType, request)
            };
            return await client.InvokeAsync(invokeRequest);
        }

        public static async Task<HttpStatusCode> CheckFunction(IAmazonLambda client, string lambdaARN)
        {
            HttpStatusCode result;
            try
            {
                await client.GetFunctionAsync(lambdaARN);
                result = HttpStatusCode.Found;
            }
            catch (Amazon.Lambda.Model.ResourceNotFoundException)
            {
                result = HttpStatusCode.NotFound;
            }
            return result;
        }
    }
}
