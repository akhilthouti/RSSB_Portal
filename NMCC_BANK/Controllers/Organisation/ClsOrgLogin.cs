using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Controllers.Organisation
{
    public class ClsOrgLogin
    {
        [Display(Name = "User Id")]
        [Required(ErrorMessage = "Please Enter UserId")]
        public string UserId { get; set; }
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        
        [RegularExpression("^(?=.*[A-Za-z0-9])(?=.*[@$!%*#?&])[A-Za-z0-9@$!%*#?&]{8,}$", ErrorMessage = "Enter a Password Should Contain One Upper Case ,One Number and One Symbol")]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public string error { get; set; } = null;

        public string Bank_Account_no { get; set; }
        public string mobno { get; set; }

    }

}
