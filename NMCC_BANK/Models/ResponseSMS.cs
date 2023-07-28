using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProvider1.Models
{
    public class ResponseSMS
    {
        public string code { get; set; }
        public string status { get; set; }

        public class data
        {
            public string mobile { get; set; }
            public string uniqueid { get; set; }
        }
    }
}