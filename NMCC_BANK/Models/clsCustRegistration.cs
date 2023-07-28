using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Models
{
    public class clsCustRegistration
    {
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email Id")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "Enter Email Id")]
        public string Emailid { get; set; }

        [Display(Name = "Mobile No")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]
        [Required(ErrorMessage = "Enter Mobile Number")]
        public string MobileNo { get; set; }

        [Display(Name = "Enter Captcha")]
        [Required(ErrorMessage = "Enter captcha")]
        public string EnterCaptcha { get; set; }

        [Display(Name = "Facilitator Code")]
        public string FacilitatorCode { get; set; }

        [Display(Name = "Mobile OTP")]
        [Required(ErrorMessage = "Enter OTP")]
        public string MobileOtp { get; set; }

    }

}
