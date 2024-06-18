using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    interface IReporterUtilities 
    {
        void WriteTestStep(string stepName, string stepLabel, string stepDataDescription, string stepRemark, bool pass, IWebDriver webDriver);
    }

    public class ReporterUtilities : IReporterUtilities
    {
        private List<ReportTestStep> TestSteps;


        public void WriteTestStep(string stepName, string stepLabel, string stepDataDescription, string stepRemark, bool pass, IWebDriver webDriver)
        {
          
        }

        public void TakeScreenshotAndSave(bool isBuildMachine)
        {
            //if it's VM save to share location

            //if it's local machine save to local location
        }
    }

    public class ReportTestStep
    {
        public string StepInfo { get; set; }
        public string LabelInfo { get; set; }
        public string TestDataDescription { get; set; }
        public string Status { get; set; }
        public string ScreenshotFilePathOnTestMachine { get; set; }
        public string StepCounter { get; set; }
        public string Remark { get; set; }
        public string TimeStamp { get; set; }
       
    }
    
}
