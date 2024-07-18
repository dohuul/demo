using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace QA.Common.Utilities
{
    public interface IBrowserUtilities
    {
        void Init(string browserType);
        void Refresh();
        void GoToUrl(string url);
        void Close();
        IWebElementUtilities FindElementById(string id);
    }

    public class BrowserUtilitiesFactory
    {
        public static IBrowserUtilities Create(string browserType)
        {
            //switch to a differnt implementation here
            //doing this way there is a laywer on top of the underling automation library like Selenium or Playwright
            IBrowserUtilities browserUtiliites = new BrowserUtilitiesPlayWright();
            //IBrowserUtilities browserUtiliites = new BrowserUtilitiesSelenium();
            browserUtiliites.Init(browserType);
            return browserUtiliites;
        }
    }

    public class BrowserUtilitiesPlayWright : IBrowserUtilities
    {
        private IPlaywright PlayWrightDriver;
        private IPage PlayWrightPage;
        private IBrowser PlayWrightBrowser;

        public  void Init(string browserType)
        {
            BrowserTypeLaunchOptions playwrightLaunchOption = new BrowserTypeLaunchOptions()
            {
                Headless = false
            };

            PlayWrightDriver = AsyncBridgeUtilities.Run(Playwright.CreateAsync());
            PlayWrightBrowser = AsyncBridgeUtilities.Run(PlayWrightDriver.Chromium.LaunchAsync(playwrightLaunchOption));
            PlayWrightPage = AsyncBridgeUtilities.Run(PlayWrightBrowser.NewPageAsync());
                      
        }

        public void Refresh()
        {
            AsyncBridgeUtilities.Run(PlayWrightPage.ReloadAsync());
         
        }

        public void GoToUrl(string url)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            AsyncBridgeUtilities.Run(PlayWrightPage.GotoAsync(url));
            stopwatch.Stop();
            Console.WriteLine("Playwright:" + stopwatch.ElapsedMilliseconds.ToString());

        }

        public void Close()
        {
            AsyncBridgeUtilities.Run(PlayWrightPage.CloseAsync());
          
        }

         
        public IWebElementUtilities FindElementById(string id)
        {
            ILocator playWrightLocator = PlayWrightPage.Locator($"#{id}");     

            IWebElementUtilities result = new WebElementPlayWright(playWrightLocator);
            return result;
        }

       

     
    }

    public class BrowserUtilitiesSelenium : IBrowserUtilities
    {        
        private IWebDriver WebDriverBrowser;

        public void Init(string browserType)
        {
       

            new DriverManager().SetUpDriver(new ChromeConfig());
            WebDriverBrowser = new ChromeDriver();

        }

        public void Refresh()
        {
       
            WebDriverBrowser.Navigate().Refresh();
        }

        public void GoToUrl(string url)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            WebDriverBrowser.Navigate().GoToUrl(url);
            stopwatch.Stop();
            Console.WriteLine("Selenium:" + stopwatch.ElapsedMilliseconds.ToString());
        }

        public void Close()
        {
         
            WebDriverBrowser.Close();
        }


        public IWebElementUtilities FindElementById(string id)
        {          
            IWebElement webElement = WebDriverBrowser.FindElement(By.Id(id));
            IWebElementUtilities result = new WebElementSelenium(webElement);
            return result;
        }




    }

    public interface IWebElementUtilities
    {
        void Click();
        void SendKeys(string value);

    }

    public class WebElementPlayWright : IWebElementUtilities
    {       
        private ILocator webElementPlaywright;
              
        public WebElementPlayWright(ILocator webElement)
        {
            webElementPlaywright = webElement;
        }

        public void Click()
        {
            AsyncBridgeUtilities.Run(webElementPlaywright.ClickAsync());
        }

        public void SendKeys(string value)
        {
            AsyncBridgeUtilities.Run(webElementPlaywright.FillAsync(value));      
        }
    }

    public class WebElementSelenium : IWebElementUtilities
    {
        private IWebElement webElementSelenium;    

        public WebElementSelenium(IWebElement webElement)
        {
            webElementSelenium = webElement;
        }

        public void Click()
        {           
            webElementSelenium.Click();
        }

        public void SendKeys(string value)
        {
            webElementSelenium.SendKeys(value);
        }
    }

    public interface IWebElementLocator
    {

    }
    public class WebElementLocator
    {

    }
}
