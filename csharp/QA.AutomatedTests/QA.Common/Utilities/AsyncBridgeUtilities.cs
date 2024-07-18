using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    public static  class AsyncBridgeUtilities
    {
        public static T Run<T>(Task<T> t)
        {
            t.Wait();
            return t.Result;
        }

        public static void Run(Task t)
        {
            t.Wait();
        }
    }
}
