using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    interface ISeleniumUtilities
    {
        void Init();
    }

    public class SeleniumUtilities
    {

        private IWebDriver _webDriver;
        private static string Javascript_Jquery = "if (!window.jQuery) { var jq = document.createElement('script'); jq.type = 'text/javascript'; + jq.src = '//code.jquery.com/jquery-3.5.0.js'; document.getElementsByTagName('head')[0].appendChild(jq);}";

        public SeleniumUtilities()
        {

        }

        //Insert JQuery so the test can utilize JQuery library
        //This can apply to other JS library that the test needs to use.
        public void InsertJQuery()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor) _webDriver;

            bool isDocumentReady = false;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int waitTimeInMiliSeconds = 30000;
            int sleepPerRetryInMiliSeconds = 500;
            while(!isDocumentReady &&  stopWatch.ElapsedMilliseconds < waitTimeInMiliSeconds)
            {
                try
                {
                    isDocumentReady = (bool)jsExecutor.ExecuteScript("return document.readyState == 'complete'");
                    if (isDocumentReady)
                    {
                        jsExecutor.ExecuteScript(Javascript_Jquery);
                        break;
                    }
                }
                catch { }
                Thread.Sleep(sleepPerRetryInMiliSeconds);
            }

            if (!isDocumentReady) throw new Exception($"Error: in InsertJquery function| Message: timed out while waiting for browser readyState| Total wait in miliseconds: {waitTimeInMiliSeconds}");
        }

    }
}
