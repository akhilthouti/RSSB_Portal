using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Models
{
    public class ClsDocumentDetails
    {
        public long? CustomerDetailId { get; set; }


        [Display(Name = "Proof Of Identity")]
        [Required(ErrorMessage = "Required")]
        public IFormFile UploadprfOfId { get; set; }

        [Display(Name = "Proof Of Correspondence Address")]
        [Required(ErrorMessage = "Required")]
        public IFormFile UploadprfOfCorrAdd { get; set; }

        [Display(Name = "Proof Of Permanant Address")]
        [Required(ErrorMessage = "Required")]
        public IFormFile UploadprfOfPerAdd { get; set; }

        [Display(Name = "Photo")]
        [Required(ErrorMessage = "Required")]
        //[ValidateFile(ErrorMessage = "Please select a PNG image smaller than 1MB")]
        public IFormFile UploadPhoto { get; set; }

        [Display(Name = "Proof Of Identity")]
        //[Required(ErrorMessage = "Required")]
        public string ProofOfIdCode { get; set; }

        [Display(Name = "Proof Of Correspondence Address")]
        //[Required(ErrorMessage = "Required")]
        public string ProofOfCorrAddCode { get; set; }

        public string Signature { get; set; }
        public string photocode { get; set; }

        [Display(Name = "Proof Of Permanant Address")]
        [Required(ErrorMessage = "Required")]
        public string ProofOfPerAddCode { get; set; }

        public string SameAdd { get; set; }

        public string isDigiApproveOrReject { get; set; }
        public string POCflag { get; set; }
        public string DocName { get; set; }
        public int? DocType { get; set; }
        //public byte[] DocDetails { get; set; }
        public string DocCategoryCode { get; set; }
        public string DocMainType { get; set; }

        public string PersonalId { get; set; }



        //POI and POA Ids


        [Display(Name = "Proof Of Identity Details")]
        [Required(ErrorMessage = "Required")]
        public string DocumentIdPOI { get; set; }



        [Display(Name = "POA Expiry Date")]
        [Required(ErrorMessage = "Please select Date")]
        public string DocumentIdDatePOI { get; set; }

        public Nullable<System.DateTime> DocumentIdDatePOI1 { get; set; }


        [Display(Name = "Proof Of Address Details")]
        [Required(ErrorMessage = "Required")]
        public string DocumentIdPOA { get; set; }


        [Display(Name = "POA Expiry Date")]
        [Required(ErrorMessage = "Please select Date")]
        public string DocumentIdDatePOA { get; set; }

        public Nullable<System.DateTime> DocumentIdDatePOA1 { get; set; }
        public string proceedwithOCR { get; set; }

        public string shareAadharNumber { get; set; }


        public byte[] custPhotoData { get; set; }

        public byte[] custPOADocumnet { get; set; }
        public byte[] custPOIDocumnet { get; set; }

        public string isDocApproveOrReject { get; set; }
    }

    public class ClsDocDetails
    {
        public long? CustomerDetailId { get; set; }

        public string ProofOfIdCode { get; set; }
        public string ProofOfCorrAddCode { get; set; }
        public string ProofOfPerAddCode { get; set; }
        public string SameAdd { get; set; }
        public string POCflag { get; set; }
        public string DocName { get; set; }
        public int? DocType { get; set; }
        public string DocCategoryCode { get; set; }
        public string DocMainType { get; set; }
        public byte[] DocDetails { get; set; }
        public long? PersonalDetailId { get; set; }

        public string Latitude_Longitude { get; set; }

        public string documentCategory { get; set; }
        //POI and POA Ids

        public string DocumentId { get; set; }

        public string DocumentIdDate { get; set; }

        public string documentCategoryCode { get; set; }

        public Nullable<System.DateTime> DocumentIdDate1 { get; set; }


        public string Source { get; set; }
        public string prediction { get; set; }

        public string documentTypeId { get; set; }


        public byte[] Faceext { get; set; }

        public byte[] Signature { get; set; }
        public string Prediction { get; set; }

        public string DocType1 { get; set; }
        public string DocumentType { get; set; }





    }
}
