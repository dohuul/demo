using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA.Common;
using QA.Common.Utilities;

namespace QA.Specflow.API.StepDefinitions
{
    [Binding]
    public  class APIUserStepDefinitions
    {
        [BeforeScenario]
        public void BeforeScenario()
        {
           
            ConfigurationStore_XlsxImplementation configuration = new ConfigurationStore_XlsxImplementation();
            string url = configuration.GetProductUrl("JamesMockService");
        }

        [Given("The name is test")]
        public void GivenTheNameIs()
        {
            //TODO: implement arrange (precondition) logic
            // For storing and retrieving scenario-specific data see https://go.specflow.org/doc-sharingdata
            // To use the multiline text or the table argument of the scenario,
            // additional string/Table parameters can be defined on the step definition
            // method. 

            throw new PendingStepException();
        }

        [When("Make a POST request to the API")]
        public void MakePOSTrequestToAPI()
        {
            //TODO: implement act (action) logic

            throw new PendingStepException();
        }

        [Then("The response HTTP status is (.*)")]
        public void ThenTheResponseHttpStatusIs(int httpStatus)
        { 
        
        }

    }
}
