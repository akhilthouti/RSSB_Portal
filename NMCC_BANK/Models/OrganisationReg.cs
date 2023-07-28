using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models
{
    public class OrganisationReg
    {
        public long? OrganizationID { get; set; }

        public string OrgReferenceNumber { get; set; }

        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; }
        [Display(Name = "Organization Logo")]
        public byte[] OrganizationLogo { get; set; }

        [ReadOnly(true)]
        [Display(Name = "Admin User Name")]
        public string AdminUserName { get; set; }

        //public DateTime createdDate { get; set; }
        public string createdDate { get; set; }
        [Display(Name = "Organization Description")]
        public string OrgDescription { get; set; }



        [Display(Name = "Mobile Number")]
        public string ContactPerMobNo { get; set; }

        [Display(Name = "Contact Person Email ID")]
        public string ContactPerEmailId { get; set; }
        [Display(Name = "HO Address")]
        public string HOadddress { get; set; }

        [Display(Name = "Fax No")]
        public string FaxNo { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Password.", AllowEmptyStrings = false)]
        public string Password { get; set; }

        [Display(Name = "Contact Person First Name")]
        [Required(ErrorMessage = "Enter Confirm Password.", AllowEmptyStrings = false)]
        public string Fname { get; set; }


        [Display(Name = "Contact Person Middle Name")]
        public string mname { get; set; }


        [Display(Name = "Contact Person Last Name")]
        [Required(ErrorMessage = "Enter Confirm Password.", AllowEmptyStrings = false)]
        public string Lname { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Enter Confirm Password.", AllowEmptyStrings = false)]
        [Compare("Password", ErrorMessage = "Password And ConfirmPassword should Not Match")]
        public string ConfirmPassword { get; set; }

    }
}
