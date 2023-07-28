using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Models
{
    public class ClsUser
    {
        internal string mobno;

        [Display(Name = "User Id")]
        [Required(ErrorMessage = "Please Enter UserId")]
        public string UserId { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        public string error { get; set; } = null;

        public string BankName { get; set; }
    }
}
