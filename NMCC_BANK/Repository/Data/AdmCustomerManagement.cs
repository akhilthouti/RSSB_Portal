using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CustomerManagement")]
    public partial class AdmCustomerManagement
    {
        [Key]
        [Column("customerDetailId")]
        public long CustomerDetailId { get; set; }
        [Column("customerReferenceNumber")]
        [StringLength(100)]
        public string CustomerReferenceNumber { get; set; }
        [Column("firstName")]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Column("middleName")]
        [StringLength(255)]
        public string MiddleName { get; set; }
        [Column("lastName")]
        [StringLength(255)]
        public string LastName { get; set; }
        [Column("mobileNo")]
        [StringLength(15)]
        public string MobileNo { get; set; }
        [Column("directTelephoneNo")]
        [StringLength(15)]
        public string DirectTelephoneNo { get; set; }
        [Column("companyTelephoneNo")]
        [StringLength(15)]
        public string CompanyTelephoneNo { get; set; }
        [Column("emailId")]
        [StringLength(255)]
        public string EmailId { get; set; }
        [Column("gender")]
        [StringLength(15)]
        public string Gender { get; set; }
        [Column("address")]
        [StringLength(500)]
        public string Address { get; set; }
        [Column("areaId")]
        public long? AreaId { get; set; }
        [Column("maritalstatus")]
        [StringLength(15)]
        public string Maritalstatus { get; set; }
        [Column("nationality")]
        [StringLength(15)]
        public string Nationality { get; set; }
        [Column("birthDate", TypeName = "datetime")]
        public DateTime? BirthDate { get; set; }
        [Column("birthPlace")]
        [StringLength(255)]
        public string BirthPlace { get; set; }
        [Column("qualification")]
        [StringLength(500)]
        public string Qualification { get; set; }
        [Column("schoolcollegename")]
        [StringLength(500)]
        public string Schoolcollegename { get; set; }
        [Column("university")]
        [StringLength(500)]
        public string University { get; set; }
        [Column("professionalDetails")]
        [StringLength(500)]
        public string ProfessionalDetails { get; set; }
        [Column("companyName")]
        [StringLength(500)]
        public string CompanyName { get; set; }
        [Column("companyContactNo")]
        [StringLength(15)]
        public string CompanyContactNo { get; set; }
        [Column("department")]
        [StringLength(255)]
        public string Department { get; set; }
        [Column("lastnameManager")]
        [StringLength(255)]
        public string LastnameManager { get; set; }
        [Column("firstnameManager")]
        [StringLength(255)]
        public string FirstnameManager { get; set; }
        [Column("designationManager")]
        [StringLength(255)]
        public string DesignationManager { get; set; }
        [Column("mobileNoManager")]
        [StringLength(255)]
        public string MobileNoManager { get; set; }
        [Column("landlineManager")]
        [StringLength(255)]
        public string LandlineManager { get; set; }
        [Column("emailIdManager")]
        [StringLength(255)]
        public string EmailIdManager { get; set; }
        [Column("subordinateLastName")]
        [StringLength(255)]
        public string SubordinateLastName { get; set; }
        [Column("subordinateFirstName")]
        [StringLength(255)]
        public string SubordinateFirstName { get; set; }
        [Column("subordinateDesignation")]
        [StringLength(255)]
        public string SubordinateDesignation { get; set; }
        [Column("subordinateMobile")]
        [StringLength(255)]
        public string SubordinateMobile { get; set; }
        [Column("subordinateLandline")]
        [StringLength(255)]
        public string SubordinateLandline { get; set; }
        [Column("subordinateEmailId")]
        [StringLength(255)]
        public string SubordinateEmailId { get; set; }
        [Column("passportNo")]
        [StringLength(255)]
        public string PassportNo { get; set; }
        [Column("adharcardNo")]
        [StringLength(400)]
        public string AdharcardNo { get; set; }
        [Column("pancardNo")]
        [StringLength(255)]
        public string PancardNo { get; set; }
        [Column("rationcardNo")]
        [StringLength(255)]
        public string RationcardNo { get; set; }
        [Column("drivinglicenseNo")]
        [StringLength(255)]
        public string DrivinglicenseNo { get; set; }
        [Column("electioncardNo")]
        [StringLength(255)]
        public string ElectioncardNo { get; set; }
        [Column("nregaNo")]
        [StringLength(255)]
        public string NregaNo { get; set; }
        [Column("religionId")]
        public int? ReligionId { get; set; }
        [Column("languageId")]
        public int? LanguageId { get; set; }
        [Column("casteId")]
        public int? CasteId { get; set; }
        [Column("totalFamilyMembers")]
        public int? TotalFamilyMembers { get; set; }
        [Column("spouseName")]
        [StringLength(255)]
        public string SpouseName { get; set; }
        [Column("fathersName")]
        [StringLength(255)]
        public string FathersName { get; set; }
        [Column("mothersName")]
        [StringLength(255)]
        public string MothersName { get; set; }
        [Column("noOfChildren")]
        public int? NoOfChildren { get; set; }
        [Column("primaryBankAccountNo")]
        [StringLength(400)]
        public string PrimaryBankAccountNo { get; set; }
        [Column("bankName")]
        [StringLength(255)]
        public string BankName { get; set; }
        [Column("branchName")]
        [StringLength(255)]
        public string BranchName { get; set; }
        [Column("annualIncome", TypeName = "money")]
        public decimal? AnnualIncome { get; set; }
        [Column("organisationDetailId")]
        public long OrganisationDetailId { get; set; }
        [Required]
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column("isSystemUser")]
        public bool? IsSystemUser { get; set; }
        [Column("createdBy")]
        public long CreatedBy { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column("updatedBy")]
        public long? UpdatedBy { get; set; }
        [Column("updatedDate", TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column("deletedBy")]
        public long? DeletedBy { get; set; }
        [Column("deletedDate", TypeName = "datetime")]
        public DateTime? DeletedDate { get; set; }
        [Column("customerTypeId")]
        public int? CustomerTypeId { get; set; }
        [Required]
        [Column("isCustomer")]
        public bool? IsCustomer { get; set; }
        [Column("faxNo")]
        [StringLength(15)]
        public string FaxNo { get; set; }
        [Column("permanentAddress")]
        [StringLength(255)]
        public string PermanentAddress { get; set; }
        [Column("permanentareaId")]
        public long? PermanentareaId { get; set; }
        [Column("occupation")]
        [StringLength(75)]
        public string Occupation { get; set; }
        [Column("mailingDistrict")]
        [StringLength(100)]
        public string MailingDistrict { get; set; }
        [Column("permanentDistrict")]
        [StringLength(100)]
        public string PermanentDistrict { get; set; }
        [Column("companyAddress")]
        public string CompanyAddress { get; set; }
        [Column("title")]
        [StringLength(10)]
        public string Title { get; set; }
        [Required]
        [Column("isEKYCDone")]
        public bool? IsEkycdone { get; set; }
        [Column("ekycDoneBy")]
        public long? EkycDoneBy { get; set; }
        [Column("lastEKYCDate", TypeName = "datetime")]
        public DateTime? LastEkycdate { get; set; }
        [Column("applicationNumber")]
        [StringLength(100)]
        public string ApplicationNumber { get; set; }
        [Required]
        [Column("isDisplay")]
        public bool? IsDisplay { get; set; }
        [Column("address2")]
        [StringLength(500)]
        public string Address2 { get; set; }
        [Column("address3")]
        [StringLength(500)]
        public string Address3 { get; set; }
        [Column("mailingSubDistrict")]
        [StringLength(100)]
        public string MailingSubDistrict { get; set; }
        [Column("mailingstreetName")]
        [StringLength(100)]
        public string MailingstreetName { get; set; }
        [Column("mailingLandmark")]
        [StringLength(100)]
        public string MailingLandmark { get; set; }
        [Column("mailingLocality")]
        [StringLength(100)]
        public string MailingLocality { get; set; }
        [Column("permanentAddress2")]
        [StringLength(500)]
        public string PermanentAddress2 { get; set; }
        [Column("permanentAddress3")]
        [StringLength(500)]
        public string PermanentAddress3 { get; set; }
        [Column("permanentSubDistrict")]
        [StringLength(100)]
        public string PermanentSubDistrict { get; set; }
        [Column("permanentstreetName")]
        [StringLength(100)]
        public string PermanentstreetName { get; set; }
        [Column("permanentLandmark")]
        [StringLength(100)]
        public string PermanentLandmark { get; set; }
        [Column("permanentLocality")]
        [StringLength(100)]
        public string PermanentLocality { get; set; }
        [Column("userDeviceDetailId")]
        public long? UserDeviceDetailId { get; set; }
        [StringLength(100)]
        public string DocumentCatId { get; set; }
        [Column("permenentCustomerId")]
        [StringLength(500)]
        public string PermenentCustomerId { get; set; }
        public long? LocalRegistrationdoneby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LocalRegistrationlastdate { get; set; }
        [Column("isLocalRegistrationdone")]
        public bool? IsLocalRegistrationdone { get; set; }
        [Column("isLocalRegistrationApproved")]
        public bool? IsLocalRegistrationApproved { get; set; }
        [Column("isAuthenticated")]
        public bool? IsAuthenticated { get; set; }
        [Column("shortName")]
        [StringLength(50)]
        public string ShortName { get; set; }
        [Column("panDescription")]
        [StringLength(50)]
        public string PanDescription { get; set; }
        [Column("UIDAIcountry")]
        [StringLength(10)]
        public string Uidaicountry { get; set; }
        [Column("UIDAIstate")]
        [StringLength(20)]
        public string Uidaistate { get; set; }
        [Column("UIDAIcity")]
        [StringLength(20)]
        public string Uidaicity { get; set; }
        [Column("UIDAIzipcode")]
        [StringLength(20)]
        public string Uidaizipcode { get; set; }
        [Column("UIDAIarea")]
        [StringLength(20)]
        public string Uidaiarea { get; set; }
        [Column("UIDAIdistrict")]
        [StringLength(20)]
        public string Uidaidistrict { get; set; }
        [Column("countryId")]
        public int? CountryId { get; set; }
        [Column("stateId")]
        public long? StateId { get; set; }
        [Column("cityId")]
        public long? CityId { get; set; }
        [Column("zipCodeId")]
        [StringLength(10)]
        public string ZipCodeId { get; set; }
        [Column("sysareaId")]
        public long? SysareaId { get; set; }
        [Column("percountryId")]
        public long? PercountryId { get; set; }
        [Column("perstateId")]
        public long? PerstateId { get; set; }
        [Column("percityId")]
        public long? PercityId { get; set; }
        [Column("perzipcodeId")]
        [StringLength(10)]
        public string PerzipcodeId { get; set; }
        [Column("perUIDAIcountry")]
        [StringLength(10)]
        public string PerUidaicountry { get; set; }
        [Column("perUIDAIstate")]
        [StringLength(20)]
        public string PerUidaistate { get; set; }
        [Column("perUIDAIcity")]
        [StringLength(20)]
        public string PerUidaicity { get; set; }
        [Column("perUIDAIzipcode")]
        [StringLength(20)]
        public string PerUidaizipcode { get; set; }
        [Column("perUIDAIarea")]
        [StringLength(20)]
        public string PerUidaiarea { get; set; }
        [Column("perUIDAIdistrict")]
        [StringLength(20)]
        public string PerUidaidistrict { get; set; }
        [Column("isApproved")]
        public bool? IsApproved { get; set; }
        [Column("isReject")]
        public bool? IsReject { get; set; }
        public string RejectReason { get; set; }
        [StringLength(20)]
        public string ApprovedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedDate { get; set; }
        public bool? IsCustomerMasterDone { get; set; }
        public bool? IsAccountMasterDone { get; set; }
        public bool? IsNomineeMasterDone { get; set; }
        [StringLength(50)]
        public string PanNo { get; set; }
        [StringLength(50)]
        public string AmlRating { get; set; }
        [StringLength(50)]
        public string Residence { get; set; }
        [StringLength(50)]
        public string ResidenceStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? MarriageDate { get; set; }
        public bool? IsResidence { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? VisaValidUpto { get; set; }
        [Column("PasswordassignYN")]
        public bool? PasswordassignYn { get; set; }
        [Column("PhoneBankingYN")]
        public bool? PhoneBankingYn { get; set; }
        public string DocumentChecklist { get; set; }
        public string OtherDocumentslist { get; set; }
        [Column("UIDAIAddress")]
        public string Uidaiaddress { get; set; }
        [Column("UIDAICustName")]
        [StringLength(100)]
        public string UidaicustName { get; set; }
        [Column("UIDAIEmailId")]
        [StringLength(50)]
        public string UidaiemailId { get; set; }
        [Column("UIDAIPhoneNo")]
        [StringLength(15)]
        public string UidaiphoneNo { get; set; }
        [Column("UIDAIBirthDate", TypeName = "datetime")]
        public DateTime? UidaibirthDate { get; set; }
        [Column("UIDAIGender")]
        [StringLength(10)]
        public string Uidaigender { get; set; }
        [StringLength(200)]
        public string KycMobileno { get; set; }
        [Column("isCBScustomer")]
        public bool? IsCbscustomer { get; set; }
        [Column("isBC")]
        public bool? IsBc { get; set; }
        [Column("isCBSBulk")]
        public bool? IsCbsbulk { get; set; }
        [Column("kycmobileApprove")]
        public bool? KycmobileApprove { get; set; }
        [Column("kycadharApprove")]
        public bool? KycadharApprove { get; set; }
        [Column("kycpanApprove")]
        public bool? KycpanApprove { get; set; }
        [Column("isPANEnrollment")]
        public bool? IsPanenrollment { get; set; }
        [Column("isPanVerification")]
        public bool? IsPanVerification { get; set; }
        [Column("lastUpdatedPanDate", TypeName = "datetime")]
        public DateTime? LastUpdatedPanDate { get; set; }
        [Column("panStatus")]
        [StringLength(50)]
        public string PanStatus { get; set; }
        [Column("enrollmentKitNumber")]
        [StringLength(500)]
        public string EnrollmentKitNumber { get; set; }
        [Column(TypeName = "money")]
        public decimal? InitialAmount { get; set; }
        [Column("BCForInitial")]
        public long? BcforInitial { get; set; }
        [Column("isbcpaid")]
        public bool? Isbcpaid { get; set; }
        [Column("isInitialSync")]
        public bool? IsInitialSync { get; set; }
        [StringLength(50)]
        public string BillConsumerNo { get; set; }
        [StringLength(50)]
        public string IsBillResource { get; set; }
        [Column("isCASReject")]
        public bool? IsCasreject { get; set; }
        [Column("CASRejectedBy")]
        public long? CasrejectedBy { get; set; }
        [Column("KYCFlag")]
        public bool? Kycflag { get; set; }
        [Required]
        public bool? ApplicantFlag { get; set; }
        [Column("isAadharVerify")]
        public bool? IsAadharVerify { get; set; }
        [Column("isPanVerified")]
        public bool? IsPanVerified { get; set; }
        [Column("isAdharVerifiedPOA")]
        public bool? IsAdharVerifiedPoa { get; set; }
        [Column("CKYCNO")]
        [StringLength(15)]
        public string Ckycno { get; set; }

        [ForeignKey(nameof(StateId))]
        [InverseProperty(nameof(SysState.AdmCustomerManagements))]
        public virtual SysState State { get; set; }
    }
}
