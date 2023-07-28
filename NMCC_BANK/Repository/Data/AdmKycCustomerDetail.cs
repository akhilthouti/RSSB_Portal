using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_KYC_CustomerDetails")]
    public partial class AdmKycCustomerDetail
    {
        [Key]
        [Column("UID")]
        public long Uid { get; set; }
        public long CustomerDetailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string FatherName { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [Column("DOB")]
        [StringLength(50)]
        public string Dob { get; set; }

        public string CasteCd { get; set; }
        public string SubTitle { get; set; }
        public string Religion { get; set; }
        public string maritalstatus { get; set; }
        public string Residence { get; set; }
        public string ResidenceDocument { get; set; }
        public string residenceYN { get; set; }
        public string ResidentialStatus { get; set; }

       





        public string PhoneBanking { get; set; }
        public string MobileDetails { get; set; }
        public string AMLRating { get; set; }
        [StringLength(10)]
        public string PanNo { get; set; }
        [StringLength(15)]
        public string AadharNo { get; set; }
        [StringLength(15)]
        public string VoterId { get; set; }
        [StringLength(15)]
        public string PassportNo { get; set; }
        [StringLength(20)]
        public string DrivingLicenceNo { get; set; }
        [StringLength(15)]
        public string MobileNo { get; set; }
        [StringLength(50)]
        public string EmailId { get; set; }
        [Column("CLIENT_ADDRESS_1")]
        [StringLength(100)]
        public string ClientAddress1 { get; set; }
        [Column("CLIENT_ADDRESS_2")]
        [StringLength(50)]
        public string ClientAddress2 { get; set; }
        [Column("CLIENT_ADDRESS_3")]
        [StringLength(50)]
        public string ClientAddress3 { get; set; }
        [StringLength(50)]
        public string CountryId { get; set; }
        [StringLength(50)]
        public string StateId { get; set; }
        [StringLength(50)]
        public string CityId { get; set; }

        [StringLength(10)]
        public string PinCode { get; set; }
        [Column("Corresponence_Permanent_Address_same_flag")]
        [StringLength(1)]
        public string CorresponencePermanentAddressSameFlag { get; set; }
        [Column("CLIENT_PERM_ADDRESS_1")]
        [StringLength(100)]
        public string ClientPermAddress1 { get; set; }
        [Column("CLIENT_PERM_ADDRESS_2")]
        [StringLength(50)]
        public string ClientPermAddress2 { get; set; }
        [Column("CLIENT_PERM_ADDRESS_3")]
        [StringLength(50)]
        public string ClientPermAddress3 { get; set; }
        [Column("CLIENT_PERM_CITY")]
        [StringLength(20)]
        public string ClientPermCity { get; set; }
        [Column("CLIENT_PERM_PIN")]
        [StringLength(7)]
        public string ClientPermPin { get; set; }
        [Column("CLIENT_PERM_STATE")]
        [StringLength(25)]
        public string ClientPermState { get; set; }
        [Column("CLIENT_PERM_COUNTRY")]
        [StringLength(25)]
        public string ClientPermCountry { get; set; }
        [Column("UCMID")]
        [StringLength(20)]
        public string Ucmid { get; set; }
        [Column("CKYCNO")]
        [StringLength(15)]
        public string Ckycno { get; set; }
        [StringLength(20)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public byte[] Photo { get; set; }
        public byte[] LivePhoto { get; set; }
        [Column("MobileDetails_Code")]
        [StringLength(50)]
        public string MobileDetailsCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public bool? CustomerMobileVerify { get; set; }
        public bool? UserMobileVerfiy { get; set; }
        [StringLength(50)]
        public string Vtc { get; set; }
        [StringLength(50)]
        public string Street { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [Column("Post_Office")]
        [StringLength(50)]
        public string PostOffice { get; set; }
        [StringLength(50)]
        public string Locality { get; set; }
        [StringLength(50)]
        public string House { get; set; }
        [StringLength(50)]
        public string District { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(50)]
        public string VerificationType { get; set; }
        [StringLength(50)]
        public string ReferenceNumber { get; set; }
        [Column("Latitude_Longitude")]
        [StringLength(200)]
        public string LatitudeLongitude { get; set; }
        public long? CustomerId { get; set; }
        public string Account_No { get; set; }
        public string CbsId { get; set; }
    }
    [Table("AdmApproveddata")]
    public partial class AdmApproveddata
    {
        [Key]

        public long CustomerDetailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public string Account_no { get; set; }
        public string customer_No { get; set; }
        public DateTime ApproveDate { get; set; }
        public string CreatedBy { get; set; }

        //public long ApprovedBy { get; set; }


    }


    public partial class AdmKycCustomerDetail1
    {
        [Key]
        [Column("UID")]
        public long Uid { get; set; }
        public long CustomerDetailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string FatherName { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        [Column("DOB")]
        [StringLength(50)]
        public string Dob { get; set; }

        public string CasteCd { get; set; }
        public string SubTitle { get; set; }
        public string Religion { get; set; }
        public string maritalstatus { get; set; }
        public string Residence { get; set; }
        public string ResidenceDocument { get; set; }
        public string residenceYN { get; set; }
        public string ResidentialStatus { get; set; }

        public string CbsId { get; set; }







        public string PhoneBanking { get; set; }
        public string MobileDetails { get; set; }
        public string AMLRating { get; set; }
        [StringLength(10)]
        public string PanNo { get; set; }
        [StringLength(15)]
        public string AadharNo { get; set; }
        [StringLength(15)]
        public string VoterId { get; set; }
        [StringLength(15)]
        public string PassportNo { get; set; }
        [StringLength(20)]
        public string DrivingLicenceNo { get; set; }
        [StringLength(15)]
        public string MobileNo { get; set; }
        [StringLength(50)]
        public string EmailId { get; set; }
        [Column("CLIENT_ADDRESS_1")]
        [StringLength(100)]
        public string ClientAddress1 { get; set; }
        [Column("CLIENT_ADDRESS_2")]
        [StringLength(50)]
        public string ClientAddress2 { get; set; }
        [Column("CLIENT_ADDRESS_3")]
        [StringLength(50)]
        public string ClientAddress3 { get; set; }
        [StringLength(50)]
        public string CountryId { get; set; }
        [StringLength(50)]
        public string StateId { get; set; }
        [StringLength(50)]
        public string CityId { get; set; }

        [StringLength(10)]
        public string PinCode { get; set; }
        [Column("Corresponence_Permanent_Address_same_flag")]
        [StringLength(1)]
        public string CorresponencePermanentAddressSameFlag { get; set; }
        [Column("CLIENT_PERM_ADDRESS_1")]
        [StringLength(100)]
        public string ClientPermAddress1 { get; set; }
        [Column("CLIENT_PERM_ADDRESS_2")]
        [StringLength(50)]
        public string ClientPermAddress2 { get; set; }
        [Column("CLIENT_PERM_ADDRESS_3")]
        [StringLength(50)]
        public string ClientPermAddress3 { get; set; }
        [Column("CLIENT_PERM_CITY")]
        [StringLength(20)]
        public string ClientPermCity { get; set; }
        [Column("CLIENT_PERM_PIN")]
        [StringLength(7)]
        public string ClientPermPin { get; set; }
        [Column("CLIENT_PERM_STATE")]
        [StringLength(25)]
        public string ClientPermState { get; set; }
        [Column("CLIENT_PERM_COUNTRY")]
        [StringLength(25)]
        public string ClientPermCountry { get; set; }
        [Column("UCMID")]
        [StringLength(20)]
        public string Ucmid { get; set; }
        [Column("CKYCNO")]
        [StringLength(15)]
        public string Ckycno { get; set; }
        [StringLength(20)]
        public string CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public byte[] Photo { get; set; }
        public byte[] LivePhoto { get; set; }
        [Column("MobileDetails_Code")]
        [StringLength(50)]
        public string MobileDetailsCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public bool? CustomerMobileVerify { get; set; }
        public bool? UserMobileVerfiy { get; set; }
        [StringLength(50)]
        public string Vtc { get; set; }
        [StringLength(50)]
        public string Street { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [Column("Post_Office")]
        [StringLength(50)]
        public string PostOffice { get; set; }
        [StringLength(50)]
        public string Locality { get; set; }
        [StringLength(50)]
        public string House { get; set; }
        [StringLength(50)]
        public string District { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(50)]
        public string VerificationType { get; set; }
        [StringLength(50)]
        public string ReferenceNumber { get; set; }
        [Column("Latitude_Longitude")]
        [StringLength(200)]
        public string LatitudeLongitude { get; set; }
        public long? CustomerId { get; set; }
    }


    [Table("Adm_JointSilResponseDetails")]
    public partial class AdmJointApproveddata
    {
        [Key]

        public long Customer_ID { get; set; }
        [StringLength(50)]
        public string PrimaryCustName { get; set; }
        [StringLength(50)]
        public string Account_no { get; set; }
        public string PrimaryCust_No { get; set; }
        public string JointCustName1 { get; set; }
        public string JointCustName2 { get; set; }
        public string JoinCust_No1 { get; set; }
        public string JoinCust_No2 { get; set; }
        public string ApproveDate { get; set; }
        //public long ApprovedBy { get; set; }


    }

    [Table("AdmRejectdata")]
    public partial class AdmRejectdata
    {
        [Key]

        public long CustomerDetailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }


        public string RejectedReason { get; set; }

        public DateTime RejectedDate { get; set; }
    }

    public partial class AdmCustGrid
    {
        [Key]

        public long CustomerDetailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        //public long ApprovedBy { get; set; }


    }
    public partial class AdmpanlogDetailsGrid
    {
        [Key]
        public string first_name { get; set; }
        [StringLength(50)]
        public string middle_name { get; set; }
        [StringLength(50)]
        public string last_name { get; set; }
        public DateTime date { get; set; }
        public string createdBy { get; set; }
    }
    public partial class AdmDigiAadharlogDetailsGrid
    {
        [Key]
        public string firstname { get; set; }
        [StringLength(50)]
        public string middlename { get; set; }
        [StringLength(50)]
        public string lastname { get; set; }
        public string CreatedBy { get; set; }
        public DateTime AadharverifiedOn { get; set; }
       

        //public long ApprovedBy { get; set; }


    }


    public partial class AdmRejectrekycdata
    {
        [Key]

        public long CustomerDetailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }


        public string ReKycRejectReason { get; set; }

        public DateTime RejectedDate { get; set; }
    }

    public partial class AdmCosmosjointDetails
    {
        [Key]

        public long CustomerID { get; set; }
        [StringLength(50)]
        public string CustomerFName { get; set; }
        [StringLength(50)]
        public string CustomerMName { get; set; }
        [StringLength(50)]
        public string CustomerLName { get; set; }
        public DateTime CreatedDate { get; set; }

        //public long ApprovedBy { get; set; }


    }

    public partial class adm_CustRkycstatus
    {
        [Key]
        public string Cid { get; set; }
        [StringLength(50)]
        public string CustFirstnm { get; set; }
        [StringLength(50)]
        public string CustMiddlenm { get; set; }
        [StringLength(50)]
        public string CustLastnm { get; set; }


        public string Status { get; set; }

        public DateTime Date { get; set; }

        public string CustAccNo { get; set; }
        public string CustId { get; set; }

    }

    public partial class AdmSilResponseDetail
    {
        [Key]
        public long? Customer_ID { get; set; }
        [StringLength(50)]
        public string Account_no { get; set; }
        [StringLength(50)]
        public string customer_No { get; set; }
        [StringLength(50)]
        public string errorMessage { get; set; }


        public string response { get; set; }

        public string Success { get; set; }

        public string CustomerName { get; set; }
        public string Branch { get; set; }
        public DateTime ApproveDate { get; set; }

    }
}
