using MathNet.Numerics.Statistics;
using OpenQA.Selenium.DevTools.V123.Browser;
using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QA.Common.Utilities
{
    public class ConfigurationUtilities
    {
       

     
    }

    public class ConfigurationStore_XlsxImplementation
    {

        private DataTable _ProductUrls = null;
        private DataTable _ZoneMembers = null;
        private DataTable _Credentials = null;
        private DataTable _Vips = null;
        private DataTable _AppConfig = null;
        private string _CurrentEnvironment = string.Empty;

        public ConfigurationStore_XlsxImplementation()
        {
            Stream xlsxStore = typeof(ConfigurationUtilities).Assembly.GetManifestResourceStream("QA.Common.Artifacts.ConfigurationStore.xlsx");
            //_CurrentEnvironment = ConfigurationManager.AppSettings["targetEnvironment"].ToUpper();
            _CurrentEnvironment = "RC";

            using (MemoryStream msA = new MemoryStream())
            using (MemoryStream msB = new MemoryStream())
            using (MemoryStream msC = new MemoryStream())
            using (MemoryStream msD = new MemoryStream())
            {
                xlsxStore.Seek(0, SeekOrigin.Begin);
                xlsxStore.CopyTo(msA);
                msA.Seek(0, SeekOrigin.Begin);
                _ProductUrls = ExcelUtilities.XlsxToDataTable(msA, "ProductUrls");

                xlsxStore.Seek(0, SeekOrigin.Begin);
                xlsxStore.CopyTo(msB);
                msB.Seek(0, SeekOrigin.Begin);
                _AppConfig = ExcelUtilities.XlsxToDataTable(msB, "AppConfig");
            }
        }

        public string GetProductUrl(string productName)
        {
            string filterExpression = $"Environment Like 'RC' and Product Like '{productName}'";        
            var rows = _ProductUrls.Select(filterExpression);
            return rows.Count() > 0 ? rows[0]["Url"].ToString() : null;
        }

        public IDictionary<string,string> GetAppConfigByEnvironment(string key)
        {
            string filterExpression = $"Environment like '{_CurrentEnvironment}' and Key like '{key}'";
            var row = _AppConfig.Select(filterExpression).FirstOrDefault();

            if (row == null) return null;

            string values = row["Values"].ToString();
            IDictionary<string, string> returnValue = new Dictionary<string, string>();
            if(values.IndexOf('=') > 0)
            {
                var nvs = HttpUtility.ParseQueryString(values);
                foreach(string k in nvs.Keys)
                {
                    returnValue[k] = nvs[k];
                }
            }
            else
            {
                returnValue[key] = values;
            }

            return returnValue;
        }
    }

}
