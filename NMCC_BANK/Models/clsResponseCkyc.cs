using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ServiceProvider1.Models
{
    public class clsResponseCkyc
    {
        [Display(Name = "Upload File :")]
        public IFormFile uploadBatch_file { get; set; }
        public string response { get; set; }
        public string firstName { get; set; } //= "supriya";

        public string lastName { get; set; }// = "Deshmukh";

        public string fatherName { get; set; }// = "Rajaram";

        public string ckycNumber { get; set; } //= "12";

        public string DOB { get; set; } //= "16/12/1993";

        public string custPhoto { get; set; }

        public string dateofBirth { get; set; }

        public string CkycNum1 { get; set; }
        public string Gender { get; set; }
    }
}
