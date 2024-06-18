using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Specflow.API.StepDefinitions
{
    public  class APIUserStepDefinitions
    {
        [Given("The name is test (.*)")]
        public void GivenTheNameIs(string test)
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

        [Then("The response HTTP status is 200 (.*)")]
        public void ThenTheResponseHttpStatusIs(int httpStatus)
        { 
        
        }

    }
}
