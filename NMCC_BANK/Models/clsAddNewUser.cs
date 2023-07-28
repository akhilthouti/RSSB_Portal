using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Models
{
    public class clsAddNewUser
    {
        [Display(Name = "User Id")]
        [Required(ErrorMessage = "Enter User Id.", AllowEmptyStrings = false)]
        
        //[RegularExpression(@"[a-zA-Z0-9._%+-]+@", ErrorMessage = "Enter a Valid User Id")]
        public long? UserId { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Enter User Name", AllowEmptyStrings = false)]
        public string Username { get; set; }


        [Display(Name = "Address")]
        [Required(ErrorMessage = "Enter Address.", AllowEmptyStrings = false)]
        public string Address { get; set; }

        [Display(Name = "Mobile No.")]
        //[Required(ErrorMessage = "Enter Mobile No.", AllowEmptyStrings = false)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Number must be 10 char long.")]
        [Range(5999999999, 9999999999999, ErrorMessage = "Mobile Number should Start with 6,7,8 or 9")]
        [RegularExpression("^[0-9]{0,10}$", ErrorMessage = "Invalid Mobile Number")]
        public string MobileNo { get; set; }

        [Display(Name = "Email Id")]
        //[Required(ErrorMessage = "Enter Email Id.", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Enter Valid Email ID")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(?:com(?!.*com)|COM(?!.*COM)|co.in(?!.*co.in)|CO.IN(?!.*CO.IN)|net(?!.*net)|NET(?!.*NET)|in(?!.*in)|IN(?!.*IN))+$", ErrorMessage = "Enter a Valid Email Id")]
        public string EmailId { get; set; }

        [Display(Name = "Branch")]
        [Required(ErrorMessage = "Enter Branch.", AllowEmptyStrings = false)]
        public string Branch { get; set; }

        [Display(Name = "Region")]
        [Required(ErrorMessage = "Enter Region.", AllowEmptyStrings = false)]
        public string Region { get; set; }

        [Display(Name = "Department")]
        [Required(ErrorMessage = "Enter Department.", AllowEmptyStrings = false)]
        public string Department { get; set; }

        [Display(Name = "User Role")]
        [Required(ErrorMessage = "Enter User Role.", AllowEmptyStrings = false)]
        public string UserRole { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Password.", AllowEmptyStrings = false)]
        [RegularExpression("^(?=.*[A-Za-z0-9])(?=.*[@$!%*#?&])[A-Za-z0-9@$!%*#?&]{8,}$", ErrorMessage = "Password Should Contain Atleast One Alphabet ,One Number and One Symbol")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Enter Confirm Password.", AllowEmptyStrings = false)]
        [Compare("Password", ErrorMessage = "Password And ConfirmPassword should Not Match")]
        [RegularExpression("^(?=.*[A-Za-z0-9])(?=.*[@$!%*#?&])[A-Za-z0-9@$!%*#?&]{8,}$", ErrorMessage = "Password Should Contain Atleast One Alphabet ,One Number and One Symbol")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Select Region.", AllowEmptyStrings = false)]
        public string RegionList { get; set; }

        [Required(ErrorMessage = "Select Branch.", AllowEmptyStrings = false)]
        public string BranchList { get; set; }

        [Required(ErrorMessage = "Select Departmnet.", AllowEmptyStrings = false)]
        public string DepartmentList { get; set; }

        [Required(ErrorMessage = "Select Role.", AllowEmptyStrings = false)]
        public string RoleList { get; set; }


        [Required(ErrorMessage = "Select Organization.", AllowEmptyStrings = false)]
        public string OrganizationList { get; set; }

        [Display(Name = "RoleType")]
        [Required(ErrorMessage = "Enter Role Type ", AllowEmptyStrings = false)]
        public string RoleType { get; set; }

        public string Search { get; set; }
        public long BankID { get; set; }

        public string BankName { get; set; }

        public string isbankadmin { get; set; }

        public string Error { get; set; } = null;
    }

}
