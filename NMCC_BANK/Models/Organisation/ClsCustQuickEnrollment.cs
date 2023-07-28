using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models.Organisation
{
    public class ClsCustQuickEnrollment
    {
        #region QuickEnroll
        // [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "First Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string QEFirstName { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Last Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string QELastName { get; set; }

        [Display(Name = "Email Id")]
        [Required(ErrorMessage = "Enter Email ID", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "invalid Email Address")]
        public string QEEmailId { get; set; }


        [Display(Name = "Mobile No")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Number must be 10 char long.")]
        [Range(5999999999, 9999999999999, ErrorMessage = "Mobile Number should Start with 6,7,8 or 9")]
        [RegularExpression("^[0-9]{0,10}$", ErrorMessage = "Only Numeric Numbers Can be Enter")]
        public string QEMobileNo { get; set; }

        public long? QECustDetailsId { get; set; }

        public string QEVType { get; set; }
        public string QEVTypeTextbox { get; set; }
        public string QEpanNo { get; set; }
        public string QEvoterId { get; set; }
        public string QEpassportNo { get; set; }
        public string QEdrivingLicenceNo { get; set; }
        public string QEaadhaarNo { get; set; }
        public string Cust_VerificationType { get; set; }

        #endregion



        public long? PersonalId { get; set; }


        //[Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "First Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Middle Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Last Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        // [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }

        [Display(Name = "Email Id")]
        [Required(ErrorMessage = "Enter Email ID", AllowEmptyStrings = false)]
        [EmailAddress(ErrorMessage = "invalid Email Address")]
        public string EmailId { get; set; }


        [Required(ErrorMessage = "Enter Mobile Number.", AllowEmptyStrings = false)]
        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string MobileNo { get; set; }

        public string EnrollEmailId { get; set; }
        public string EnrollMobileNo { get; set; }


        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string Gender { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Select DOB.", AllowEmptyStrings = false)]
        public string DateOfBirth { get; set; }
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Father Name ")]
        [StringLength(50, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FatherName { get; set; }

        [Display(Name = "PAN No")]
        [Required(ErrorMessage = "Enter Pan Number.", AllowEmptyStrings = false)]
        [RegularExpression("^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Enter Valid Pan Card Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Pan Number must be 10 char long.")]
        public string PANNumber { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Aadhar No")]
        //[RegularExpression("^[0-9]{0,12}$", ErrorMessage = "Invalid Aadhaar Number")]
        //[StringLength(12, MinimumLength = 12, ErrorMessage = "Aadhaar Number must be 12 char long.")]
        public string AadharNo { get; set; } = null;

        [Display(Name = "Vote Id")]
        public string VoterId { get; set; }

        [Display(Name = "Passport")]
        public string Passport { get; set; }

        [Display(Name = "Driving Licence")]
        public string DrivingLicence { get; set; }


        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Display(Name = "Locality")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Locality { get; set; }

        [Display(Name = "Country")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Country { get; set; }

        [Display(Name = "State")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string State { get; set; }

        [Display(Name = "House")]
        //[RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Enter Correct Details")]
        public string House { get; set; }

        [Display(Name = "Street")]
        // [RegularExpression(@"^[0-9][a-zA-Z]+$", ErrorMessage = "Enter Correct Details")]
        public string Street { get; set; }
        [Display(Name = "District/City")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string District { get; set; }

        [Display(Name = "Pincode")]
        [Required(ErrorMessage = "Enter Pin Code.", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]{0,6}$", ErrorMessage = "Enter Only Number and Length must be 6 Number")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Pin Code must be 6 Number.")]
        public string Pincode { get; set; }

        public string XMLStatus { get; set; }

        public string XMLImageBase64 { get; set; }

        [Display(Name = "Customer Photo ")]
        public string CustomerPhoto { get; set; }

        public byte[] Photo { get; set; }

        public string OcrDropDown { get; set; }

        [Display(Name = "Select Image")]
        public IFormFile PANOrAadharCard { get; set; }

        public string OCRReferenceNo { get; set; }

        public string Error { get; set; }

        public string DigiKycPhoto { get; set; }

        public string LivePhoto { get; set; }

        public byte[] livecameraphoto { get; set; }
        public Boolean TFForVerif { get; set; }
        public string KYC_SubDistrict { get; set; }
        public string KYC_StreetName { get; set; }
        public string KYC_State { get; set; }
        public string KYC_MobileNo { get; set; }
        public string KYC_Locality { get; set; }
        public string KYC_HouseNo { get; set; }
        public string KYC_Address { get; set; }
        public string KYC_District { get; set; }
        public string KYC_City { get; set; }
        public string KYC_Country { get; set; }
        public string KYC_Pincode { get; set; }

        public string proceedwithOCR { get; set; }

        public string shareAadharNumber { get; set; }

        public string mobileDetail_Code { get; set; }
        public string KYCverificationType { get; set; }
        public bool PanVerificationType { get; set; }

        public bool CKYCVerificationType { get; set; }

        public bool AadharVerificationType { get; set; }

        public string DigilockerType { get; set; }



        //Other Option Fields
        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "First Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstNameOther { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Middle Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string MiddleNameOther { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Last Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastNameOther { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string GenderOther { get; set; }

        public string GenderForOthers { get; set; }

        //[Required(ErrorMessage = "Required")]
        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Select DOB.", AllowEmptyStrings = false)]
        public string DateOfBirthOther { get; set; }

        [Display(Name = "Address Line 1")]
        public string Other_AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string Other_AddressLine2 { get; set; }

        [Display(Name = "City/Town/Village")]
        public string Other_City { get; set; }

        [Display(Name = "Pin Code")]
        public string Other_PinCode { get; set; }

        [Display(Name = "State")]
        public string Other_State { get; set; }

        [Display(Name = "Country Code Of Resindence")]
        public string Other_CountryCode { get; set; }

        [Display(Name = "Document Type")]
        public string Other_DocType { get; set; }

        [Display(Name = "Document Number")]
        public string Other_DocNum { get; set; }
        public bool Otherverification { get; set; }

        public string OtherStatus { get; set; }


        //End Other Fields

        public string CKYCOption { get; set; }

        // Start CKYC
        [Display(Name = "Prefix")]
        [Required(ErrorMessage = "Enter Prefix.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Prefix { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Enter First Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_firstname { get; set; }

        [Display(Name = "Middle Name")]
        [Required(ErrorMessage = "Enter Middle Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_MiddelName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Enter Last Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_LastName { get; set; }

        [Display(Name = "Prefix")]
        [Required(ErrorMessage = "Enter Prefix.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Prefixmidden { get; set; }

        [Display(Name = "Maiden First Name")]
        [Required(ErrorMessage = "Enter Maiden First Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_firstnamemidden { get; set; }

        [Display(Name = "Maiden Middle Name")]
        [Required(ErrorMessage = "Enter Maiden Middle Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_MiddelNamemidden { get; set; }

        [Display(Name = "Maiden Last Name")]
        [Required(ErrorMessage = "Enter Maiden Last Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_LastNamemidden { get; set; }


        [Display(Name = "Kindly check for Father's Name")]
        [Required(ErrorMessage = "Enter Prefix.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public bool CKYC_Father_Prefixflag { get; set; }

        [Display(Name = "Prefix")]
        [Required(ErrorMessage = "Enter Prefix.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Father_Prefix { get; set; }


        [Display(Name = "Father First Name")]
        [Required(ErrorMessage = "Enter Father First Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Father_firstname { get; set; }


        [Display(Name = "Father Middle Name")]
        [Required(ErrorMessage = "Enter Father Middle Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Father_MiddelName { get; set; }


        [Display(Name = "Father Last Name")]
        [Required(ErrorMessage = "Enter Father Last Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Father_LastName { get; set; }


        [Display(Name = "Prefix")]
        [Required(ErrorMessage = "Enter Prefix.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Mother_Prefix { get; set; }


        [Display(Name = "Mother First Name")]
        [Required(ErrorMessage = "Enter Mother First Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Mother_firstname { get; set; }


        [Display(Name = "Mother Middle Name")]
        [Required(ErrorMessage = "Enter Mother Middle Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Mother_MiddelName { get; set; }


        [Display(Name = "Mother Last Name")]
        [Required(ErrorMessage = "Enter Mother Last Name.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Mother_LastName { get; set; }


        [Display(Name = "Marital Status")]
        [Required(ErrorMessage = "Enter Marital Status.", AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string CKYC_Marital_status { get; set; }


        [Display(Name = "Gender")]
        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string CKYC_gender { get; set; }

        [Display(Name = "Birth Date")]
        //   public DateTime birthDate { get; set; }
        [Required(ErrorMessage = "Select DOB.", AllowEmptyStrings = false)]
        public string CKYC_birthDate { get; set; }

        [Display(Name = "PAN No")]
        [Required(ErrorMessage = "Enter Pan Number.", AllowEmptyStrings = false)]
        [RegularExpression("^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Enter Valid Pan Card Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Pan Number must be 10 char long.")]
        public string CKYC_PANNumber { get; set; }

        [Display(Name = "Form 60 furnished")]
        public bool CKYC_Form60 { get; set; }

        // Proof of Identity & Address
        [Display(Name = "Line 1")]
        public string CKYC_AddressLine1 { get; set; }

        [Display(Name = "Line 2")]
        public string CKYC_AddressLine2 { get; set; }

        [Display(Name = "Line 3")]
        public string CKYC_AddressLine3 { get; set; }

        [Display(Name = "City/Town/Village")]
        public string CKYC_City { get; set; }

        [Display(Name = "Pin Code")]
        public string CKYC_PinCode { get; set; }

        [Display(Name = "State/U.T Code")]
        public string CKYC_State { get; set; }

        [Display(Name = "District")]
        public string CKYC_District { get; set; }

        [Display(Name = "ISO -3166 Country Code Of Resindence")]
        public string CKYC_CountryCode { get; set; }

        [Display(Name = "Proof of Indentity & Address")]
        public string CKYC_Indentity_Address { get; set; }

        [Display(Name = "ID Proof Number")]
        public string CKYC_IdProofNumber { get; set; }

        // END  Proof of Identity & Address

        //CKCY Current Address
        [Display(Name = "Line 1")]
        public string CKYC_CurrentAddressLine1 { get; set; }

        [Display(Name = "Line 2")]
        public string CKYC_CurrentAddressLine2 { get; set; }

        [Display(Name = "Line 3")]
        public string CKYC_CurrentAddressLine3 { get; set; }

        [Display(Name = "City/Town/Village")]
        public string CKYC_CurrentCity { get; set; }

        [Display(Name = "Pin Code")]
        public string CKYC_CurrentPinCode { get; set; }

        [Display(Name = "State/U.T Code")]
        public string CKYC_CurrentState { get; set; }

        [Display(Name = "District")]
        public string CKYC_CurrentDistrict { get; set; }

        [Display(Name = "ISO -3166 Country Code Of Resindence")]
        public string CKYC_CurrentCountryCode { get; set; }

        [Display(Name = "Proof of Current Address")]
        public string CKYC_CurrentIndentity_Address { get; set; }

        [Display(Name = "ID Proof Number")]
        public string CKYC_CurrentIdProofNumber { get; set; }

        [Display(Name = "Office Telephone No")]
        public string CKYC_OfficeNo { get; set; }

        [Display(Name = "Residence Telephone No")]
        public string CKYC_ResidenceNo { get; set; }

        [Display(Name = "Mobile No")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Enter Pin Code.", AllowEmptyStrings = false)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string CKYC_MobileNo { get; set; }

        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Enter Pin Code.", AllowEmptyStrings = false)]
        public string CKYC_EmailID { get; set; }


        //End CKCY Current Address
        //END CKYC

        //Start NSDL Pan
        [Display(Name = "PAN No")]
        [Required(ErrorMessage = "Enter Pan Number.", AllowEmptyStrings = false)]
        [RegularExpression("^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Enter Valid Pan Card Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Pan Number must be 10 char long.")]
        public string NSDL_PANNumber { get; set; }

        [Display(Name = "Date of Birth")]
        [Required(ErrorMessage = "Select DOB.", AllowEmptyStrings = false)]
        public string NSDL_DOB { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "First Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string NSDL_FirstName { get; set; }

        //[Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Middle Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string NSDL_MiddleName { get; set; }

        [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Last Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string NSDL_LastName { get; set; }

        [Display(Name = "Pan Title")]
        [StringLength(6, ErrorMessage = "Should not exceed 6 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string NSDL_PanTitle { get; set; }

        [Display(Name = "Name Printed On Pan")]
        [StringLength(60, ErrorMessage = "Should not exceed 60 characters")]
        [RegularExpression(@"^[a-zA-Z_ ]*$", ErrorMessage = "Use letters only please")]
        public string NSDL_NamePrintedOnPan { get; set; }

        public string NSDL_Result { get; set; }




        //End  NSDL Pan

        public string isQuickApproveOrReject { get; set; }

        public bool OnlineEKYCVerificationType { get; set; }

        public string AadhaarverificationType { get; set; }

        public string CKYC_age { get; set; }

        public string Ckyc_custPhoto { get; set; }
        public string Ckyc_custPANPhoto { get; set; }
        public string Ckyc_custAadhaarPhoto { get; set; }
        public string Ckyc_custPassportPhoto { get; set; }
        public string Ckyc_custDrivingPhoto { get; set; }
        public string Ckyc_custElectionPhoto { get; set; }

        public string Ckyc_custNregaPhoto { get; set; }
        public string Ckyc_custSignaturePhoto { get; set; }

        [RegularExpression("^[0-9]{0,12}$", ErrorMessage = "Only Numeric Numbers Can be Enter")]
        [StringLength(12, MinimumLength = 12, ErrorMessage = "Plz Enter only Numbers.")]
        public string Aadhaarxml { get; set; }
        //[RegularExpression("^[0-9]{0,12}$", ErrorMessage = "Invalid Aadhaar Number")]
        //[StringLength(12, MinimumLength = 12, ErrorMessage = "Plz Enter only Numbers.")]
        public string captchaimg { get; set; }

        public string CaptchaValue { get; set; }

        [StringLength(6, MinimumLength = 6, ErrorMessage = "Plz Enter only Numbers.")]
        public string xmlOTP { get; set; }
        //[StringLength(6, MinimumLength = 6, ErrorMessage = "Plz Enter only Numbers.")]

        public string SecurityXmlCode { get; set; }

        public bool ReKyc { get; set; }

        public bool DigiLocker { get; set; }

        public bool PanCard { get; set; }

        public bool DrivingLic { get; set; }

        public bool Aadharxml { get; set; }


        public string CBScustomerID { get; set; }


        //Existing Customer
        public string CustomerNo { get; set; }
        [Display(Name = "First Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string ExistFirstName { get; set; }

        [Display(Name = "Middle Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string ExistMiddleName { get; set; }

        [Display(Name = "Last Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string ExistLastName { get; set; }
        [Display(Name = "Mobile No")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile Number must be 10 char long.")]
        [Range(5999999999, 9999999999999, ErrorMessage = "Mobile Number should Start with 6,7,8 or 9")]
        [RegularExpression("^[0-9]{0,10}$", ErrorMessage = "Only Numeric Numbers Can be Enter")]
        public string ExistMobileNo { get; set; }
        [Display(Name = "Email Id")]
        public string ExistEmail { get; set; }
        [Display(Name = "Date of Birth")]

        public string ExistDOB { get; set; }
        [Display(Name = "Gender")]

        public string ExistGender { get; set; }
        [Display(Name = "Address1")]
        public string ExistAddress1 { get; set; }
        [Display(Name = "Address2")]
        public string ExistAddress2 { get; set; }
        [Display(Name = "Address3")]
        public string ExistAddress3 { get; set; }
        [Display(Name = "City")]
        public string ExistCity { get; set; }
        [Display(Name = "PinCode")]
        public string ExistPincode { get; set; }
        //public string State { get; set; }
        // public string Country { get; set; }
        public string ExistOccupation { get; set; }
        [Display(Name = "AnnualIncome")]
        public string ExistAnnalIncome { get; set; }

        //Digilocker pan 

        // [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "First Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_FirstName { get; set; }

        //[Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Middle Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        // [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_MiddleName { get; set; }

        //[Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Last Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        // [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_LastName { get; set; }

        [Display(Name = "Gender")]
        //[Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string Digi_gender { get; set; }

        [Display(Name = "Birth Date")]
        //   public DateTime birthDate { get; set; }
        // [Required(ErrorMessage = "Select DOB.", AllowEmptyStrings = false)]
        public string Digi_birthDate { get; set; }

        [Display(Name = "Pan Number")]
        public string Digi_PANNo { get; set; }

        [Display(Name = "Country")]
        public string Digi_country { get; set; }

        //[Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "ORG_Name ")]
        [StringLength(40, ErrorMessage = "Should not exceed 60 characters")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_ORGname { get; set; }

        public string Digi_PANverifiedOn { get; set; }

        //Digilocker Driving 

        // [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "First Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        // [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_DFirstName { get; set; }

        //[Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Middle Name")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_Dswd { get; set; }

        //[Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Last Name ")]
        [StringLength(20, ErrorMessage = "Should not exceed 50 characters")]
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string Digi_DLastName { get; set; }

        [Display(Name = "Gender")]
        // [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        public string Digigender { get; set; }

        [Display(Name = "Birth Date")]
        //   public DateTime birthDate { get; set; }
        // [Required(ErrorMessage = "Select DOB.", AllowEmptyStrings = false)]
        public string DigiDateOfBirth { get; set; }

        [Display(Name = "Driving Number")]
        public string Digi_DRVLC { get; set; }

        [Display(Name = "Country")]
        public string Digi_Dcountry { get; set; }

        // [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "ORG_Name ")]
        [StringLength(40, ErrorMessage = "Should not exceed 60 characters")]
        public string Digi_DORGname { get; set; }

        // [Required(ErrorMessage = "Required", AllowEmptyStrings = false)]
        [Display(Name = "Address ")]
        //[StringLength(100, ErrorMessage = "Should not exceed 100 characters")]
        public string Digi_DAddress { get; set; }

        //DigiAadhar 
        [Display(Name = "First Name ")]
        public string FirstName1 { get; set; }

        [Display(Name = "Middle  Name ")]
        public string MiddleName1 { get; set; }

        [Display(Name = "Last  Name ")]
        public string LastName1 { get; set; }

        [Display(Name = "Date of Birth ")]
        public string DateOfBirth1 { get; set; }

        [Display(Name = "Gender")]
        public string Gender2 { get; set; }

        [Display(Name = "Pincode")]
        public string Pincode1 { get; set; }

        //Digilocker pan 

        [Display(Name = "PAN No")]
        //  [Required(ErrorMessage = "Enter Pan Number.", AllowEmptyStrings = false)]
        //[RegularExpression("^([a-zA-Z]){5}([0-9]){4}([a-zA-Z]){1}?$", ErrorMessage = "Enter Valid Pan Card Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Pan Number must be 10 char long.")]
        public string digilockerpan { get; set; }
        public string digifullname { get; set; }



        //Digilocker pan 


        [Display(Name = "Driving Licence No")]
        //  [Required(ErrorMessage = "Enter Driving Licence Number.", AllowEmptyStrings = false)]
        //[RegularExpression("^[0-9a-zA-Z]{4,9}$", ErrorMessage = "Enter Valid Driving Licence Number")]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "Driving Licence Number must be 15 char long.")]
        public string digilockerdrlic { get; set; }



    }

}
