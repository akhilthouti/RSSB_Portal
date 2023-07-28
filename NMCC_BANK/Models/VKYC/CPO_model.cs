using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models.VKYC
{
    public class CPO_model
    {
        public bool otpServiceE { get; set; }
        public bool otpServiceM { get; set; }
        public string mobile { get; set; }
        public string email_Id { get; set; }
        public string customerName { get; set; }
        public string CPOVkycLink { get; set; }
        public string RefId { get; set; }
        [Display(Name = "Customer livelocation")]
        public string Customerlivelocation { get; set; }

        public string latitude { get; set; }
        public string longitude { get; set; }
        public string location { get; set; }

    }
}
