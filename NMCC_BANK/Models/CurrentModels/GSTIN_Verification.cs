using System;
using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models.CurrentModels
{
    public class GSTIN_Verification
    {
        [Key]
        public long CustomerId { get; set; }
        public string PAN { get; set; }
        public string created_at { get; set; }
        public string ref_id { get; set; }
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
