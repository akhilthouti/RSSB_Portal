using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiceProvider1.Models
{
    public class EKYC
    {
        public string sha { get; set; }
        public string dt { get; set; }

        public string referencekey { get; set; }
        public string aadhaarNumber { get; set; }
        public string name { get; set; }
        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string careOfPerson { get; set; }
        public string landmark { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string subDistrict { get; set; }
        public string house { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string postOfficeName { get; set; }
        public string error { get; set; }
        public string mobile { get; set; }
    }
}