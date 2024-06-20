using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Common;
using QA.Common.Utilities;
using QA.Common.WebService.JamesTestService.Client;
using QA.Common.WebService.JamesTestService.Model;

namespace QA.Specflow.API.StepDefinitions
{
    [Binding]
    public  class APIUserStepDefinitions
    {
        JamesTestService jamesServiceClient;
        JamesTestServiceRequest jamesServiceRequest;
        JamesTestServiceResponse jamesServiceResponse;

        [BeforeScenario]
        public void BeforeScenario()
        {

            jamesServiceClient = new JamesTestService();
           
       
        }

        [Given("The name is test")]
        public void GivenTheNameIs()
        {
            jamesServiceRequest = new JamesTestServiceRequest
            {
                id = "1",
                name = "test"
            };
           
        }

        [When("Make a POST request to the API")]
        public void MakePOSTrequestToAPI()
        {
            //TODO: implement act (action) logic
            string responseString = jamesServiceClient.PostToService(jamesServiceRequest);
            Byte[] bytes = Encoding.UTF8.GetBytes(responseString);
            Stream responseStream = new MemoryStream(bytes);
            jamesServiceResponse = SerializationUtilities.DeserializeJson<JamesTestServiceResponse>(responseStream);
                   
        }

        [Then("The response is not null")]
        public void ThenTheResponseIsNotNul()
        {
            Assert.IsTrue(jamesServiceResponse != null);
        }

    }
}
