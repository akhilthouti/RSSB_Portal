using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceProvider1.Models
{
    public class OfficerDocUploade
    {
        [Required(ErrorMessage = "File Is Required")]
        public IFormFile DocFile { get; set; }//HttpPostedFile

        [Required(ErrorMessage = "Please Enter First Name")]
        public string CustomerFirstName { get; set; }
        [Required(ErrorMessage = "Please Enter Middle Name")]
        public string CustomerMiddleName { get; set; }
        [Required(ErrorMessage = "Please Enter Last Name")]
        public string CustomerLastName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]
        [Required(ErrorMessage = "Please Enter Mobile Number")]
        public string CustomerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "Please Enter Email Id")]
        public string CustomerMailId { get; set; }

        [Display(Name = "First Signer First Name ")]
        public string FirstFSignerName { get; set; }
        [Display(Name = "First Signer Middle Name ")]
       
        public string FirstMSignerName { get; set; }
        [Display(Name = "First Signer Last Name ")]
        public string FirstLSignerName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]
        public string FirstSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string FirstSignerMailId { get; set; }

        [Display(Name = "Second Signer First Name ")]
        public string SecondSignerFName { get; set; }
        [Display(Name = "Second Signer Middle Name ")]
        public string SecondSignerMName { get; set; }
        [Display(Name = "Second Signer Last Name ")]
        public string SecondSignerLName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]
        public string SecondSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]
        public string SecondSignerMailId { get; set; }

        [Display(Name = "Third Signer First Name ")]
        public string ThirdSignerFName { get; set; }
        [Display(Name = "Third Signer Middle Name ")]
        public string ThirdSignerMName { get; set; }
        [Display(Name = "Third Signer Last Name ")]
        public string ThirdSignerLName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]

        public string ThirdSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]

        public string ThirdSignerMailId { get; set; }

        [Display(Name = "Fourth Signer First Name ")]
        public string FourthSignerFName { get; set; }
        [Display(Name = "Fourth Signer Middle Name ")]
        public string FourthSignerMName { get; set; }
        [Display(Name = "Fourth Signer Last Name ")]
        public string FourthSignerLName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]

        public string FourthSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]

        public string FourthSignerMailId { get; set; }

        [Display(Name = "Fifth Signer First Name ")]
        public string FifthSignerFName { get; set; }
        [Display(Name = "Fifth Signer Middle Name ")]
        public string FifthSignerMName { get; set; }
        [Display(Name = "Fifth Signer Last Name ")]
        public string FifthSignerLName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]

        public string FifthSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]

        public string FifthSignerMailId { get; set; }

        [Display(Name = "Six Signer First Name ")]
        public string SixSignerFName { get; set; }
        [Display(Name = "Six Signer Middle Name ")]
        public string SixSignerMName { get; set; }
        [Display(Name = "Six Signer Last Name ")]
        public string SixSignerLName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]

        public string SixSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]

        public string SixSignerMailId { get; set; }

        [Display(Name = "Seventh Signer First Name ")]
        public string SeventhSignerFName { get; set; }
        [Display(Name = "Seventh Signer Middle Name ")]
        public string SeventhSignerMName { get; set; }
        [Display(Name = "Seventh Signer Last Name ")]
        public string SeventhSignerLName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]

        public string SeventhSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]

        public string SeventhSignerMailId { get; set; }

        [Display(Name = "Eighth Signer First Name ")]
        public string EighthSignerFName { get; set; }
        [Display(Name = "Eighth Signer Middle Name ")]
        public string EighthSignerMName { get; set; }
        [Display(Name = "Eighth Signer Last Name ")]
        public string EighthSignerLName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]

        public string EighthSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]

        public string EighthSignerMailId { get; set; }

        [Display(Name = "Ninth Signer First Name ")]
        public string NinthSignerFName { get; set; }
        [Display(Name = "Ninth Signer Middle Name ")]
        public string NinthSignerMName { get; set; }
        [Display(Name = "Ninth Signer Last Name ")]
        public string NinthSignerLName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter valid mobile number.")]

        public string NinthSignerMobileNo { get; set; }
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Invalid Email Address")]

        public string NinthSignerMailId { get; set; }
    }
}