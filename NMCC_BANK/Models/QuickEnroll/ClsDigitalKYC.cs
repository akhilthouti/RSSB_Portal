using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Models.QuickEnroll
{
    public class ClsDigitalKYC
    {



    }

    public class ClsDigitalKYC1
    {
        [Display(Name = "Customer No ")]
        public string RekycustomerNO { get; set; }

        [Display(Name = "Customer First name ")]
        public string RekycustomerFirstname { get; set; }
        [Display(Name = "Customer Middle name ")]
        public string RekycustomerMiddlename { get; set; }
        [Display(Name = "Customer Last name ")]

        public string RekycustomerLastname { get; set; }
        [Display(Name = "Customer Mobile No ")]
        public string RekycustomerMobileno { get; set; }
        [Display(Name = "Customer Email Id ")]
        public string RekycustomerEmailID { get; set; }
        [Display(Name = "Customer DOB ")]
        public string RekycustomerDOB { get; set; }
        [Display(Name = "Customer Gender ")]
        public string RekycustomerGEnder { get; set; }
        [Display(Name = "Customer Address 1 ")]
        public string RekycustomerAdd1 { get; set; }
        [Display(Name = "Customer Address 2 ")]
        public string RekycustomerAdd2 { get; set; }
        [Display(Name = "Customer Address 3")]
        public string RekycustomerAdd3 { get; set; }
        [Display(Name = "City")]
        public string RekycustomerCity { get; set; }
        [Display(Name = "PinCode")]
        public string RekycustomerPincode { get; set; }
        [Display(Name = "State")]
        public string RekycustomerState { get; set; }
        [Display(Name = "Country")]
        public string RekycustomerCountryID { get; set; }
        [Display(Name = "Anual income")]
        public string RekycustomerAnnualIncome { get; set; }
        [Display(Name = "Occupation")]
        public string RekycustomerOccupation { get; set; }



        [Display(Name = "Customer First name ")]
        public string DRekycustomerFirstname { get; set; }
        [Display(Name = "Customer Middle name ")]
        public string DRekycustomerMiddlename { get; set; }
        [Display(Name = "Customer Last name ")]

        public string DRekycustomerLastname { get; set; }
        [Display(Name = "Customer Mobile No ")]
        public string DRekycustomerMobileno { get; set; }
        [Display(Name = "Customer Email Id ")]
        public string DRekycustomerEmailID { get; set; }
        [Display(Name = "Customer DOB ")]
        public string DRekycustomerDOB { get; set; }
        [Display(Name = "Customer Gender ")]
        public string DRekycustomerGEnder { get; set; }
        [Display(Name = "Customer Address 1 ")]
        public string DRekycustomerAdd1 { get; set; }
        [Display(Name = "Customer Address 2 ")]
        public string DRekycustomerAdd2 { get; set; }
        [Display(Name = "Customer Address 3")]
        public string DRekycustomerAdd3 { get; set; }
        [Display(Name = "City")]
        public string DRekycustomerCity { get; set; }
        [Display(Name = "PinCode")]
        public string DRekycustomerPincode { get; set; }
        [Display(Name = "State")]
        public string DRekycustomerState { get; set; }
        [Display(Name = "Country")]
        public string DRekycustomerCountryID { get; set; }
        [Display(Name = "Anual income")]
        public string DRekycustomerAnnualIncome { get; set; }
        [Display(Name = "Occupation")]
        public string DRekycustomerOccupation { get; set; }


        public string FirstnameStatus { get; set; }
        public string MiddlenameStatus { get; set; }
        public string LastnameStatus { get; set; }
        public string ADDRESSStatus1 { get; set; }
        public string ADDRESSStatus2 { get; set; }
        public string ADDRESSStatus3 { get; set; }
        public string DobStatus { get; set; }
        public string GenderStatus { get; set; }
        public string CountryStatus { get; set; }
        public string CITYStatus { get; set; }
        public string PincodeStatus { get; set; }
        public string STATEStatus { get; set; }








        public long? PersonalId { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer First Name ")]
        [StringLength(50, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Middle Name ")]
        [StringLength(50, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_MiddleName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Last Name ")]
        [StringLength(50, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_LastName { get; set; }

        [Display(Name = "Barcode/Application No")]
        public string Digi_BarcodeOrAppNumber { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [Required(ErrorMessage = "Select DOB.", AllowEmptyStrings = false)]
        public string Digi_DOB { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Select Gender.", AllowEmptyStrings = false)]
        public string Digi_gender { get; set; }

        [Display(Name = "PAN No.")]
        [Required(ErrorMessage = "Enter Pan Number.", AllowEmptyStrings = false)]
        [RegularExpression("^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Enter Valid Pan Card Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Pan Number must be 10 char long.")]
        public string Digi_PAN { get; set; }

        [Display(Name = "Mobile No.")]
        [Required(ErrorMessage = "Enter Mobile Number.", AllowEmptyStrings = false)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Number must be 10 char long.")]
        [Range(5999999999, 9999999999999, ErrorMessage = "Mobile Number should Start with 6,7,8 or 9")]

        [RegularExpression("^[0-9]{0,10}$", ErrorMessage = "Invalid Mobile Number")]
        public string MobileNo { get; set; }

        [Display(Name = "Mobile Number belongs to*")]
        [Required(ErrorMessage = "Select MobileDetails")]
        public string MobileDetails { get; set; }



        [Required(ErrorMessage = "Enter your Aadharnumber")]
        [RegularExpression("^/d{4}/s/d{4}/s/d{4}$", ErrorMessage = "invalid")]
        //[RegularExpression(@"^\d{4}\s\d{4}\s\d{4}$", ErrorMessage = "enter Integers only")]//^\d{4}\s\d{4}\s\d{4}$


        [Display(Name = "Aadhar No")]
        public string Digi_Aadhar { get; set; }

        // [Required(ErrorMessage ="Enter a voter id")]  //^([a-zA-Z]){3}([0-9]){7}?$   //[a-zA-Z]{3}

        [RegularExpression("^([a-zA-Z]){3}([0-9]){7}?$", ErrorMessage = "Enter valid Voter Id Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Voter Id Number must be 10 char long.")]

        [Display(Name = "Voter Id")]
        public string Digi_Voter { get; set; }

        [Display(Name = "Passport")]
        public string Digi_passport { get; set; }

        [Display(Name = "Driving License")]
        public string Digi_Drivinglicense { get; set; }

        [Display(Name = "N-Rega Number")]
        public string Digi_nrega { get; set; }


        public string Flag { get; set; }
        public string DigiKYCPhoto { get; set; }
        public string livecameraphoto { get; set; }
        public byte[] BCPhoto { get; set; }

        public string LivePhoto { get; set; }
        public byte[] Photo { get; set; }
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string Address { get; set; }
        public string Latitude_Longitude { get; set; }

        public string Prediction { get; set; }

        public string SubTitle { get; set; }
        public string CasteCd { get; set; }
        public string Religion { get; set; }
        public string maritalstatus { get; set; }
        public string Residence { get; set; }
        public string ResidenceDocument { get; set; }
        public string ResidentialStatus { get; set; }

        public string residenceYN { get; set; }
        public string CreatedBy { get; set; }

        public string PhoneBanking { get; set; }
        public string AMLRating { get; set; }
        [Display(Name = "Address1")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string CLIENT_ADDRESS_1 { get; set; }

        [Display(Name = "Address2")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string CLIENT_ADDRESS_2 { get; set; }

        [Display(Name = "Address3")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string CLIENT_ADDRESS_3 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please Enter City")]
        public string CLIENT_CITY { get; set; }

        [Display(Name = "State* :")]
        [Required(ErrorMessage = "Select State.", AllowEmptyStrings = false)]
        public string CLIENT_STATE { get; set; }

        [Display(Name = "Country*")]
        [Required(ErrorMessage = "Select Country.", AllowEmptyStrings = false)]
        public string CLIENT_COUNTRY { get; set; }

        [Display(Name = "Address1")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string CLIENT_PERM_ADDRESS_1 { get; set; }

        [Display(Name = "Address2")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string CLIENT_PERM_ADDRESS_2 { get; set; }

        [Display(Name = "Address3")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string CLIENT_PERM_ADDRESS_3 { get; set; }

        [Display(Name = "City")]
        // [Required(ErrorMessage = "Please Enter City")]
        public string CLIENT_PERM_CITY { get; set; }

        [Display(Name = "State* :")]
        // [Required(ErrorMessage = "Select State.", AllowEmptyStrings = false)]
        public string CLIENT_PERM_STATE { get; set; }

        [Display(Name = "Country*")]
        //[Required(ErrorMessage = "Select Country.", AllowEmptyStrings = false)]
        public string CLIENT_PERM_COUNTRY { get; set; }

        [Display(Name = "Pincode")]
        //[Required(ErrorMessage = "Enter Pin Code.", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]{0,6}$", ErrorMessage = "Enter Only Number and Length must be 6 Number")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Pin Code must be 6 Number.")]
        public string CLIENT_PERM_Pincode { get; set; }
        [Display(Name = "Correspondence Permanent Address same flag:")]
        [Required(ErrorMessage = "Select Correspondence Permanent Address.", AllowEmptyStrings = false)]
        //[StringLength(1, MinimumLength = 1, ErrorMessage = "Corresponence Permanent Address must be 1 char long.")]
        public bool Corresponence_Permanent_Address_same_flag { get; set; }

        //[Required(ErrorMessage = "Enter Email ID.", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "Enter Valid Email ID")]
        [RegularExpression(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(?:com(?!.*com)|COM(?!.*COM)|co.in(?!.*co.in)|CO.IN(?!.*CO.IN)|net(?!.*net)|NET(?!.*NET)|in(?!.*in)|IN(?!.*IN))+$", ErrorMessage = "Enter a Valid Email Id")]
        public string EmailId { get; set; }

        [Display(Name = "Pincode")]
        [Required(ErrorMessage = "Enter Pin Code.", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]{0,6}$", ErrorMessage = "Enter Only Number and Length must be 6 Number")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Pin Code must be 6 Number.")]
        public string Pincode { get; set; }
        public string proceedwithOCR { get; set; }


        public string shareAadharNumber { get; set; }

        public string KYCverificationType { get; set; }
        public bool PanVerificationType { get; set; }

        public bool Otherverification { get; set; }

        public bool CKYCVerificationType { get; set; }

        public bool AadharVerificationType { get; set; }

        public string DigiReferencenumber { get; set; }

        public string isDigiApproveOrReject { get; set; }

        public string CustDetailsId { get; set; }


        public string Digilock_firstname { get; set; }

        public string Digilock_middlename { get; set; }

        public string Digilock_lastname { get; set; }

        public string Digilock_PANNo { get; set; }

        public string dob1lock { get; set; }

        public string genderlock { get; set; }

        public string Genderfemale { get; set; }

        public string Digi_ORGname { get; set; }


        public string Digi_Dfirstname { get; set; }

        public string Digi_Dswd { get; set; }

        public string Digi_Dlastname { get; set; }

        public string Digi_DAddress { get; set; }
        public string Digidob2 { get; set; }
        public string DigilockerType { get; set; }
        public bool? IsDigiPANSumbitted { get; set; }
        public bool? IsDigilDRLCSumbitted { get; set; }
        public bool? IsDigiAadharSumbitted { get; set; }

        public string Digilockgender { get; set; }

        public string Digilock_Afirstname { get; set; }
        public string Digilock_Amiddlename { get; set; }
        public string Digilock_Alastname { get; set; }


        public string FirstNameOther { get; set; }
        public string MiddleNameOther { get; set; }

        public string LastNameOther { get; set; }
        public string Oaddressline1 { get; set; }

        public string Oaddressline2 { get; set; }
        public string DateOfBirthOther { get; set; }
        public string Other_PinCode { get; set; }
        public string Other_State { get; set; }
        public string Other_City { get; set; }
        public string Other_CountryCode { get; set; }

        public string Othergender { get; set; }





    }



    public class ClsDigitalOCR
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer First Name ")]
        [StringLength(50, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string OcrFirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Middle Name ")]
        [StringLength(50, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string OcrMiddleName { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Customer Last Name ")]
        [StringLength(50, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string OcrLastName { get; set; }
        public string DigiOcrDropDown { get; set; }
        [Display(Name = "Select Image")]
        public IFormFile DigiPANOrAadharCard { get; set; }

        [Display(Name = "Date of Birth")]
        public string OcrDateOfBirth { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Gender")]
        public string OcrGender { get; set; }

        public string OcrAddress { get; set; }

        [Display(Name = "PAN No")]
        public string OcrPANNumber { get; set; }

        //[Required(ErrorMessage = "Required")]
        //[Display(Name = "Aadhar No")]

        public string OcrAadharNo { get; set; } = null;

        [Display(Name = "Pincode")]
        public string OcrPincode { get; set; }

        public string OcrKycPhoto { get; set; }



    }

}
