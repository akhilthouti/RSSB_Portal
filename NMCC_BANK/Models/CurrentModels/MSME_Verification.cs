using System;
using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models.CurrentModels
{
    public class MSME_Verification
    {
        [Key]
        public long CustomerId { get; set; }
        public string code { get; set; }
        public string Category { get; set; }
        public string DateofCommencement { get; set; }
        public string District { get; set; }
        public string company_name { get; set; }
        public string State { get; set; }
        public string message { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
        public string ref_id { get; set; }
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
