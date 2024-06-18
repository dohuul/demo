﻿using iText.Commons.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    interface IRESTUtilities
    {
        abstract static HttpResponseMessage ExecuteHTTPPostRequest(string url, string requestBody, Dictionary<string,string> httpHeadersDictionary);
        abstract static HttpResponseMessage ExecuteHTTPGetRequest(string url, string requestBody, Dictionary<string, string> httpHeadersDictionary);
        abstract static int ExtractHttpStatusFromResponse(HttpResponseMessage webResponse);
        abstract static string ExtractStringBodyFromResponse(HttpResponseMessage webResponse);
    }

    internal class RESTUtilities
    {

        private static HttpResponseMessage ExecuteWebRequest(string httpMethod, string url, Stream requestBody, Dictionary<string, string> httpHeadersDictionary)
        {
            HttpClient httpClient = new HttpClient();
            HttpRequestMessage webRequest = new HttpRequestMessage(new HttpMethod(httpMethod), url);
            HttpResponseMessage webResponse = null;            

            if(requestBody != null)
            {
                using (HttpContent httpContent = new StreamContent(requestBody))
                {
                    webRequest.Content = httpContent;
                }                   
            }
        
            if(httpHeadersDictionary != null)
            {               
                foreach (string httpHeaderName in httpHeadersDictionary.Keys)
                {
                    webRequest.Headers.Add(httpHeaderName, httpHeadersDictionary[httpHeaderName]);
                }
            }
         
            webResponse = httpClient.Send(webRequest);        

            return webResponse;
        }

        
    }
}
