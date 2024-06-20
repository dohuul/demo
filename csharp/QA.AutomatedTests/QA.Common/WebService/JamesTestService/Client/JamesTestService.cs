using Org.BouncyCastle.Crypto.Paddings;
using QA.Common.Utilities;
using QA.Common.WebService.JamesTestService.Model;
using System.Text;


namespace QA.Common.WebService.JamesTestService.Client
{
    public class JamesTestService
    {
        public string URL { get; set; }

        public const string ServiceName = "JamesTestService";

        public JamesTestService()
        {
            URL = ConfigurationUtilities.GetProductUrl("JamesTestService");
        }

        public string PostToService(JamesTestServiceRequest serviceRequest)
        {
            MemoryStream stream = new MemoryStream();
            string stringContent = SerializationUtilities.SerializeContent("json", serviceRequest);
            byte[] bytes = Encoding.UTF8.GetBytes(stringContent);
            stream = new MemoryStream(bytes);
            Dictionary<string, string> httpHeaders = new Dictionary<string, string>();
            httpHeaders.Add("content-type", "application/json");
            httpHeaders.Add("px-portalCode", "API");
            using (HttpResponseMessage responseMessage = RESTUtilities.ExecuteWebRequest("POST", URL, stream, httpHeaders))
            {
                Stream responseStream = responseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string responseString = streamReader.ReadToEnd();
                return responseString;
            }
            
        }


    }
}
