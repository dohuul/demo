using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.Utilities
{
    public class ProcessUtilities
    {
        public static string RunProcess(string processName, string arguments)
        {
            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = processName;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.Arguments = $" {arguments}";
                p.Start();
                p.WaitForExit();
                string output = p.StandardOutput.ReadToEnd();               
                return output;
            }
        }
    }
}
