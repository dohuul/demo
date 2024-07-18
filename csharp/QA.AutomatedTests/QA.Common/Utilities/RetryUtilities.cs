using NPOI.HSSF.Record;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    public class RetryUtilities
    {
        public static void RetryAction(int retryCount, int sleepPerRetryMs, Action a)
        {
            string errorMessage = "";
            int internalRetryCount = retryCount;
            while(internalRetryCount > 0)
            {
                try
                {
                    a();
                    errorMessage = "";
                    break;
                }
                catch(Exception e)
                {
                    System.Threading.Thread.Sleep(sleepPerRetryMs);
                    errorMessage = e.Message;
                    if(e.InnerException != null)
                    {
                        errorMessage = $"{errorMessage}|InnerException:{e.InnerException.Message}";
                    }
                }
                finally
                {
                    internalRetryCount--;
                }
            }
        }
    }
}
