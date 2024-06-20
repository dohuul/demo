using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Common.WebService.JamesTestService.Model
{
    public class JamesTestServiceResponse
    {
        public List<JamesServiceResponseData> Responses { get; set; }
    }

    public class JamesServiceResponseData
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
