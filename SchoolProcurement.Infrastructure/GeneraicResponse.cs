using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProcurement.Infrastructure
{
    public class GeneraicResponse
    {
        public GeneraicResponse()
        {
            this.status = "success";
            this.message = string.Empty;
            this.error_message = string.Empty;
        }

        public string status { get; set; }
        public string message { get; set; }
        public string error_message { get; set; }
        public object data { get; set; }
    }
}
