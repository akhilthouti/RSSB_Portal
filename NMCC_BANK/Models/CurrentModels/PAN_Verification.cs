using System;
using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models.CurrentModels
{
    public class PAN_Verification
    {
        [Key]
        public long CustomerId { get; set; }
        public string CONSTITUTIONOFBUSINESS { get; set; }
        public string GSTIN { get; set; }
        public string LEGALNAMEOFBUSINESS { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string created_at { get; set; }
        public string ref_id { get; set; }
        public long statusCode { get; set; }
        public string statusMessage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
