using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceProvider1.Models.UserDetails
{
    public class ClsSummeryDetails
    {
        public string CustomerId { get; set; }
        public long? CustomerDetailId { get; set; }
        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string CFirstName { get; set; }

        public string CMiddleName { get; set; }

        public string CLastName { get; set; }

        public string BCPhoto { get; set; }
        public string DocName { get; set; }
        public int? DocType { get; set; }
        public string DocCategoryCode { get; set; }
        public string DocMainType { get; set; }
        public byte[] DocDetails { get; set; }
        public string BC_Latitude_Longitude { get; set; }
        public string CPhoto { get; set; }

        public DateTime? CDateOfBirth { get; set; }

        public string CGender { get; set; }

        public string CAddress { get; set; }

        public string CEmailId { get; set; }

        public string CMobileNo { get; set; }
        public string MobileOtp1 { get; set; }

        public string AadharNumber { get; set; }

        public string AadharPhoto { get; set; }

        public string LivePhoto { get; set; }
        public string AadharName { get; set; }
        public string AadharGender { get; set; }


        public string AadharDOB { get; set; }

        public string AadharHouse { get; set; }

        public string AadharVtc { get; set; }
        public string AadharStreet { get; set; }
        public string AadharCountry { get; set; }
        public string AadharState { get; set; }

        public string AadharPin_Code { get; set; }
        public string DOB { get; set; }
        public string DOB_DD { get; set; }
        public string Pin_Code { get; set; }
        public string DOB_MM { get; set; }
        public string DOB_yyyy { get; set; }

        public string User_MobileNo { get; set; }
        public string imgtypePhoto { get; set; }

        public string PhotoDocument { get; set; }

        public string imgtypePOI { get; set; }
        public string StatusCode { get; set; }
        public string IsMatching { get; set; }
        public string SimilarityScore { get; set; }


        public string POI_Document { get; set; }
        public string POI_Document1 { get; set; }
        public string POI_Document2 { get; set; }

        public string POA_Document_Name { get; set; }

        public string POI_Document_Name { get; set; }

        public string SI_Document { get; set; }
        public string imgtypeSI { get; set; }

        public string imgtypeCA { get; set; }

        public string CA_Document { get; set; }

        public string ManualPanNo { get; set; }

        public string ManualPassportNo { get; set; }
        public string ManualDrivingLicenceNo { get; set; }
        public string ManualVoterId { get; set; }

        public string NSDL_PANNumber { get; set; }
        public string NSDL_DOB { get; set; }
        public string NSDL_FirstName { get; set; }
        public string NSDL_MiddleName { get; set; }
        public string NSDL_LastName { get; set; }
        public string NSDL_PanTitle { get; set; }
        public string NSDL_NamePrintedOnPan { get; set; }

        public bool? IsPanVerify { get; set; }

        public bool? IsCkycVerify { get; set; }

        public bool? IsAadharVerify { get; set; }

        public string Referencenumber { get; set; }


        //public string Aadhar_MiddleName { get; set; }
        //public string Aadhar_LastName { get; set; }
        public string Aadhar_DateOfBirth { get; set; }

        public string Aadhar_Gender { get; set; }

        public string Aadhar_Address { get; set; }
        //public string Country { get; set; }
        public string Aadhar_Locality { get; set; }
        //public string State { get; set; }
        //public string House { get; set; }
        public string Aadhar_District { get; set; }
        // public string Street { get; set; }
        public string Aadhar_Pincode { get; set; }

      public string Latitude_Longitude { get; set; }
        // }
        #region QEDetails
        [Display(Name = "First Name")]
        public string QEFirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string QEMiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string QELastName { get; set; }

        [Display(Name = "DOB")]
        public string QEDOB { get; set; }
        [Display(Name = "Gender")]
        public string QEGender { get; set; }
        [Display(Name = "Pan Number")]
        public string QEPanNo { get; set; }
        [Display(Name = "Aadhar Number")]
        public string QEAadhaarNo { get; set; }

        //
        [Display(Name = "Father Name")]
        public string QEFatherName { get; set; }

        [Display(Name = "PassportNo")]
        public string QEPassportNo { get; set; }

        [Display(Name = "DrivingLicenceNo")]
        public string QEDrivingLicenceNo { get; set; }
        [Display(Name = "MobileNo")]
        public string QEMobileNo { get; set; }

        [Display(Name = "EmailId")]
        public string QEEmailId { get; set; }

        [Display(Name = "Address1")]
        public string QECLIENT_ADDRESS_1 { get; set; }

        [Display(Name = "Address2")]
        public string QECLIENT_ADDRESS_2 { get; set; }
        [Display(Name = "Address3")]
        public string QECLIENT_ADDRESS_3 { get; set; }

        [Display(Name = "Country")]
        public string QECountry { get; set; }

        [Display(Name = "City")]
        public string QECity { get; set; }

        [Display(Name = "State")]
        public string QEState { get; set; }
        [Display(Name = "Pincode")]
        public string QEPin { get; set; }

        [Display(Name = "CLIENT_PERM_ADDRESS_1")]
        public string QECLIENT_PERM_ADDRESS_1 { get; set; }

        [Display(Name = "CLIENT_PERM_ADDRESS_2")]
        public string QECLIENT_PERM_ADDRESS_2 { get; set; }


        [Display(Name = "CLIENT_PERM_ADDRESS_3")]
        public string QECLIENT_PERM_ADDRESS_3 { get; set; }


        [Display(Name = "CLIENT_PERM_CITY")]
        public string QECLIENT_PERM_CITY { get; set; }

        [Display(Name = "CLIENT_PERM_COUNTRY")]
        public string QECLIENT_PERM_COUNTRY { get; set; }

        [Display(Name = "Live Photo")]
        public string QELive_Photo { get; set; }

        public string ll1 { get; set; }


        #endregion

        #region PanDetailsForCompare in dashboard 
        [Display(Name = "Pan No")]
        public string PanNo { get; set; }

        [Display(Name = "Pan Doc")]
        public string PanDocumentType { get; set; }

        [Display(Name = "First Name")]
        public string PanComFirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string PanComMiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string PanComLastName { get; set; }

        [Display(Name = "Pan Number")]
        public string PanComNo { get; set; }

        #endregion
        #region status
        public string FirstNameStatus { get; set; }
        public string MiddleNameStatus { get; set; }
        public string LastNameStatus { get; set; }
        public string DOBStatus { get; set; }
        public string Genderstatus { get; set; }
        public string PanNoStatus { get; set; }
        public string AadhaarNoStatus { get; set; }
        #endregion

        #region AadharData

        [Display(Name = "Aadhar Number")]
        public string AadharNumberDV { get; set; }


        [Display(Name = "Photo")]
        public string AadharPhotoDV { get; set; }

        [Display(Name = "First Name")]
        public string AadharFirstNameDV { get; set; }

        [Display(Name = "Middle Name")]
        public string AadharMiddleNameDV { get; set; }

        [Display(Name = "Last Name")]
        public string AadharLastNameDV { get; set; }

        [Display(Name = "DOB")]
        public string AadharDOBDV { get; set; }

        [Display(Name = "Gender")]
        public string AadharGenderDV { get; set; }

        #endregion

        public bool IsQuickEnrollDetailsRejected { get; set; } 

        public string QuickEnrollDetailsRejectedReason { get; set; }

        public bool IsCAFDetailsRejected { get; set; }

        public string CAFDetailsRejectedReason { get; set; }

        public bool IsDocumentDetailsRejected { get; set; }

        public string DocumentDetailsRejectedReason { get; set; } 

        public bool CAFPDFReject { get; set; }

        public string CAFPDFRejectedReason { get; set; }

        public bool IsIPVRejected { get; set; }

        public string IPVRejectedReason { get; set; }


        public string isCAFPDFApproveOrReject { get; set; }

        #region Account Form

        public string Branch { get; set; }

        public string BranchSolID { get; set; }

        public string AccountNo { get; set; }

        public string AccOpenDate { get; set; }
        public bool Regular { get; set; }
        public bool CosmoPremium { get; set; }
        public bool CosmoSalary { get; set; }
        public bool CosmoRoyale { get; set; }
        public bool CosmoPremiumPlus { get; set; }
        public bool BSBDA { get; set; }

        public bool CosmoYouth { get; set; }

        public bool CosmoPremiumSalary { get; set; }

        public bool other { get; set; }

        public bool CosmoRupayCard { get; set; }

        public bool CosmoVisaDebitCard { get; set; }
        public bool UPI { get; set; }
        public bool InternetBanking { get; set; }
        public bool IMPS { get; set; }
        public bool YouthCard { get; set; }
        public bool CosmoKidzCard { get; set; }

        public bool CosmoJansanchayCard { get; set; }

        public bool CosmoNet { get; set; }

        public bool IMbanking { get; set; }

        public bool ViewOnly { get; set; }

        public bool Transaction { get; set; }
        public bool CosmoEstatement { get; set; }
        public bool Frequency { get; set; }
        public bool Monthly { get; set; }

        public bool Quarterly { get; set; }
        public bool SixMonthly { get; set; }

        public bool Yearly { get; set; }
        public bool chequeBook { get; set; }
        public bool FlexiFixedDepositScheme { get; set; }
        public bool CarLoan { get; set; }
        public bool ConsumerLoan { get; set; }
        public bool HomeLoan { get; set; }
        public bool BusinessLoan { get; set; }
        public bool EducationLoan { get; set; }
        public bool NewsPaper { get; set; }
        public bool Staff { get; set; }
        public bool RelativeFriend { get; set; }
        public bool Advertise { get; set; }
        public bool OtherCreditFacility { get; set; }

        public string PassportNumber { get; set; }
        public string PassportExpiryDate { get; set; }
        public string VoterIdCardNumber { get; set; }
        public string PANCardNumber { get; set; }
        public bool Form60 { get; set; }
        public string UIDNumber { get; set; }
        public string NregaJobCard { get; set; }
        public string Others { get; set; }
        public string IdentificationNumber { get; set; }

        public string VerifyCustData { get; set; }
        public string SearchType { get; set; }

        public string ckycIdentifier { get; set; }

        public string MaidenPrefixName { get; set; }
        public string MaidenFName { get; set; }
        public string MaidenMName { get; set; }
        public string MaidenLName { get; set; }
        public string FatherPrefixName { get; set; }
        public string FatherFName { get; set; }
        public string FatherMName { get; set; }
        public string FatherLName { get; set; }

        public string MotherPrefixName { get; set; }
        public string MotherFName { get; set; }
        public string MotherMName { get; set; }
        public string MotherLName { get; set; }

        public string BirthPlaceCity { get; set; }
        public string BirthPlaceCountry { get; set; }

        public string MaritalStatus { get; set; }

        public string Nationality { get; set; }

        public string ResidentialStatus { get; set; }

        public string Religion { get; set; }

        public string Caste { get; set; }

        public string OccupationType { get; set; }

        public string BusinessFirm { get; set; }

        public string SalaryEmployed { get; set; }

        public string Designation { get; set; }

        public string EducationQualification { get; set; }

        public string AnnualIncome { get; set; }

        public string ThresholdLimit { get; set; }

        public string NatureOfOrganization { get; set; }
        public string NatureOfOrganizationRemark { get; set; }
        public string DepositNumberOfTxnPerMonth { get; set; }
        public string DepositValueOfTxnPerMonth { get; set; }
        public string DepositTotalFundsDepositedinThreeMonth { get; set; }
        public string DepositTxnPerMonthChequeOrTransfer { get; set; }
        public string WithdrawNumberOfTxnPerMonth { get; set; }
        public string WithdrawValueOfTxnPerMonth { get; set; }
        public string WithdrawTotalFundsDepositedinThreeMonth { get; set; }
        public string WithdrawTxnPerMonthChequeOrTransfer { get; set; }

        public string NomineePrefix { get; set; }
        public string NomineeFName { get; set; }
        public string NomineeMName { get; set; }
        public string NomineeLName { get; set; }
        public string NomineeAge { get; set; }
        public string NomineeRelation { get; set; }

        [Display(Name = "Address1")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string Nominee_ADDRESS_1 { get; set; }

        [Display(Name = "Address2")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string Nominee_ADDRESS_2 { get; set; }

        [Display(Name = "Address3")]
        [Required(ErrorMessage = "Please Enter Your Address")]
        public string Nominee_ADDRESS_3 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "Please Enter City")]
        public string Nominee_CITY { get; set; }

        [Display(Name = "State* :")]
        [Required(ErrorMessage = "Select State.", AllowEmptyStrings = false)]
        public string Nominee_STATE { get; set; }

        [Display(Name = "Country*")]
        [Required(ErrorMessage = "Select Country.", AllowEmptyStrings = false)]
        public string Nominee_COUNTRY { get; set; }

        [Display(Name = "Pincode")]
        [Required(ErrorMessage = "Enter Pin Code.", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]{0,6}$", ErrorMessage = "Enter Only Number and Length must be 6 Number")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Pin Code must be 6 Number.")]
        public string Nominee_Pincode { get; set; }

        public string OfficeUseInitialAmount { get; set; }

        public string OfficeUseDC_NO { get; set; }

        public string OfficeUseDate { get; set; }

        public string OfficeUseWF_No { get; set; }

        public string OfficeUseTicket_No { get; set; }

        public string OfficeUseNameAndSign { get; set; }

        public bool AOC_VISAorATM { get; set; }

        public bool AOC_SMSBanking { get; set; }

        public bool AOC_ACAndKYCComplied { get; set; }

        public bool AOC_IBComformationRequest { get; set; }

        public bool AOC_ChequeBookRequest { get; set; }
        public bool AOC_CosmoNetComformationRequest { get; set; }
        public bool AOC_AutoSweep { get; set; }
        public string AOC_OfficerName { get; set; }
        public string AOC_TicketNo { get; set; }

        public string ABCcell_OfficerName { get; set; }
        public string ABCcell_TicketNo { get; set; }

        public bool NomineeForAccountYes { get; set; }

        public bool NomineeForAccountNo { get; set; }

        public bool initialDepositCash { get; set; }

        public bool initialDepositCheque { get; set; }

        public string initialDepositChequeNo { get; set; }

        public string initialDepositChequeDate { get; set; }
        public string initialDepositAmount { get; set; }

        public bool JointForAccountYes { get; set; }

        public bool JointForAccountNo { get; set; }

        public string Joint1stimgtypePhoto { get; set; }

        public string Joint1stPhotoDocument { get; set; }

        public string Joint1stimgtypePOI { get; set; }

        public string Joint1stPOI_Document { get; set; }

        public string Joint1stimgtypeCA { get; set; }

        public string Joint1stCA_Document { get; set; }
        #endregion

        #region Joint1st
        public string CFirstName1st { get; set; }

        public string CMiddleName1st { get; set; }

        public string CLastName1st { get; set; }

        public string CGender1st { get; set; }

        public string CEmailId1st { get; set; }

        public string CustomerId1st { get; set; }

        public string CMobileNo1st { get; set; }

        public string DOB1st { get; set; }

        public string ManualPanNo1st { get; set; }

        public string Pin_Code1st { get; set; }

        public string Referencenumber1st { get; set; }

        public DateTime? CDateOfBirth1st { get; set; }

        public string AadharPhoto1st { get; set; }

        public string LivePhoto1st { get; set; }

        public string CAddress1st { get; set; }

        public string DOB_MM1st { get; set; }
        public string DOB_yyyy1st { get; set; }

        public string DOB_DD1st { get; set; }



        public string AadharName1st { get; set; }
        public string AadharGender1st { get; set; }


        public string AadharDOB1st { get; set; }

        public string AadharHouse1st { get; set; }

        public string AadharVtc1st { get; set; }
        public string AadharStreet1st { get; set; }
        public string AadharCountry1st { get; set; }
        public string AadharState1st { get; set; }

        public string AadharPin_Code1st { get; set; }

        public string Aadhar_DateOfBirth1st { get; set; }

        public string Aadhar_Gender1st { get; set; }

        public string Aadhar_Address1st { get; set; }
        //public string Country { get; set; }
        public string Aadhar_Locality1st { get; set; }
        //public string State { get; set; }
        //public string House { get; set; }
        public string Aadhar_District1st { get; set; }
        // public string Street { get; set; }
        public string Aadhar_Pincode1st { get; set; }
        #endregion Joint1st 
    }
}