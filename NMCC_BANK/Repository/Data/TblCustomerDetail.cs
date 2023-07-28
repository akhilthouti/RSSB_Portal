using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    public partial class TblCustomerDetail
    {
        [Key]
        public long CustDetailsId { get; set; }
        [Column("Cust_FirstName")]
        [StringLength(50)]
        public string CustFirstName { get; set; }
        [Column("Cust_MiddleName")]
        [StringLength(50)]
        public string CustMiddleName { get; set; }
        [Column("Cust_LastName")]
        [StringLength(50)]
        public string CustLastName { get; set; }
        [Column("Cust_EmailId")]
        [StringLength(50)]
        public string CustEmailId { get; set; }
        [Column("Cust_MobileNo")]
        [StringLength(50)]
        public string CustMobileNo { get; set; }
        [Column("Cust_SessionToken")]
        [StringLength(2000)]
        public string CustSessionToken { get; set; }
        [Column("Cust_CreatedDate", TypeName = "datetime")]
        public DateTime? CustCreatedDate { get; set; }
        [Column("Cust_CreatedBy")]
        public long? CustCreatedBy { get; set; }
        [StringLength(50)]
        public string FacilitatorCode { get; set; }
        [Column("isEmailOTPSend")]
        public bool? IsEmailOtpsend { get; set; }
        [Column("EmailSendOTPTime", TypeName = "datetime")]
        public DateTime? EmailSendOtptime { get; set; }
        [Column("isEmailOTPVerify")]
        public bool? IsEmailOtpverify { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EmailVerifyDate { get; set; }
        [Column("IsMobileOTPSend")]
        public bool? IsMobileOtpsend { get; set; }
        [Column("MOBileOTP")]
        [StringLength(10)]
        public string MobileOtp { get; set; }
        [Column("MobileOTPTime", TypeName = "datetime")]
        public DateTime? MobileOtptime { get; set; }
        [Column("IsMobileOTPVerify")]
        public bool? IsMobileOtpverify { get; set; }
        [Column("MobileOTPVerifyDate", TypeName = "datetime")]
        public DateTime? MobileOtpverifyDate { get; set; }
        [StringLength(10)]
        public string PanNo { get; set; }
        [StringLength(15)]
        public string AadhaarNo { get; set; }
        [StringLength(15)]
        public string VoterId { get; set; }
        [StringLength(15)]
        public string PassportNo { get; set; }
        [StringLength(20)]
        public string DrivingLicenceNo { get; set; }
        [Column("Cust_VerificationType")]
        [StringLength(50)]
        public string CustVerificationType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LoginDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogOutDateTime { get; set; }
    }

    public partial class admname
    {
        public string id { get; set; }
        public string NAME_code { get; set; }
        public string NAME_description { get; set; }
    }
    public partial class admBranchDetails
    {
        public long id { get; set; }
        public long branch_code { get; set; }
        public string branch_description { get; set; }
    }

    public partial class admCASTDetail
    {
        public string id { get; set; }
        public string CAST_code { get; set; }
        public string CAST_description { get; set; }
    }

    public partial class adm_ReligionDetail
    {
        public string id { get; set; }
        public string Religion_code { get; set; }
        public string Religion_description { get; set; }
    }

    public partial class adm_maritalstatus
    {
        public string id { get; set; }
        public string maritalstatus_code { get; set; }
        public string maritalstatus_description { get; set; }
    }

    public partial class adm_residence
    {
        public string id { get; set; }
        public string residence_code { get; set; }
        public string residence_description { get; set; }
    }

    public partial class adm_residencedocument
    {
        public string id { get; set; }
        public string document_code { get; set; }
        public string document_description { get; set; }
    }

    public partial class adm_residenceYN
    {
        public string id { get; set; }
        public string residenceYN_code { get; set; }
        public string residenceYN_description { get; set; }
    }

    public partial class adm_phonebanking
    {
        public string id { get; set; }
        public string PB_code { get; set; }
        public string PB_description { get; set; }
    }
    public partial class adm_OccupationDetails
    {
        public string id { get; set; }
        public string Occupation_Code { get; set; }
        public string Occupation_desc { get; set; }

    }
    public partial class adm_PhysicallyChallenged
    {
        public string id { get; set; }
        public string PhysicalCha_Code { get; set; }
        public string PhysicalChalDesc { get; set; }
    }
    public partial class adm_TDSReasonCode
    {
        public string id { get; set; }
        public string TDSReason_Code { get; set; }
        public string TDSReason_Desc { get; set; }
    }
    public partial class adm_ProfessionDetails
    {
        public string id { get; set; }
        public string Pro_Code { get; set; }
        public string Pro_Value { get; set; }

    }
    public partial class adm_OrganizationName
    {
        public string id { get; set; }
        public string Organization_ID { get; set; }
        public string Organization_Name { get; set; }       
    }

    public partial class adm_OraganisationRegistration
    {
        public string id { get; set; }
        public string Organization_ID { get; set; }
        public string Organization_Name { get; set; }

        public string OrganizationDescription { get; set; }

        public string OrganizationLogo { get; set; }
        public string AdminUsername { get; set; }

        public string ContactPerMobNo { get; set; }
        public string ContactPerEmailId { get; set; }
        public string HOadddress { get; set; }
        public string FaxNo { get; set; }

        public string OrgPassword { get; set; }
        public string Fname { get; set; }

        public string mname { get; set; }
        public string Lname { get; set; }
        public string ConfirmPassword { get; set; }


    }
}
