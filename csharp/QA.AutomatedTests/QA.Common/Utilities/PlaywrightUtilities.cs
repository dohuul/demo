using Microsoft.Playwright;

namespace QA.Common.Utilities
{
    interface IPlaywrightUtilities
    {

    }

    public class PlaywrightUtilities
    {
        public IPlaywright PlayWright { get; set; }

        
        public async Task Init(string browserName)
        {


            PlayWright = await Playwright.CreateAsync();          
        }

        public async Task GoToUrl()
        {
            
        }

      
        
    }
}
