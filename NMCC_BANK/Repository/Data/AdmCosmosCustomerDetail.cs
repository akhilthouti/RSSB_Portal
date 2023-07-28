using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_Cosmos_CustomerDetails")]
    public partial class AdmCosmosCustomerDetail
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("CustomerID")]
        public long? CustomerId { get; set; }
        [StringLength(255)]
        public string Branch { get; set; }
        [StringLength(255)]
        public string AccountNumber { get; set; }
        [StringLength(255)]
        public string AccountType { get; set; }
        [StringLength(255)]
        public string AccountopenDate { get; set; }
        [StringLength(255)]
        public string CkycNumber { get; set; }
        [StringLength(255)]
        public string CustomerPrefix { get; set; }
        [Column("CustomerFName")]
        [StringLength(255)]
        public string CustomerFname { get; set; }
        [Column("CustomerMName")]
        [StringLength(255)]
        public string CustomerMname { get; set; }
        [Column("CustomerLName")]
        [StringLength(255)]
        public string CustomerLname { get; set; }
        [Column("CustomerDOB")]
        [StringLength(255)]
        public string CustomerDob { get; set; }
        [StringLength(255)]
        public string MaidenPrefix { get; set; }
        [Column("MaidenFName")]
        [StringLength(255)]
        public string MaidenFname { get; set; }
        [Column("MaidenMName")]
        [StringLength(255)]
        public string MaidenMname { get; set; }
        [Column("MaidenLName")]
        [StringLength(255)]
        public string MaidenLname { get; set; }
        [StringLength(255)]
        public string FatherSpousePrefix { get; set; }
        [Column("FatherSpouseFName")]
        [StringLength(255)]
        public string FatherSpouseFname { get; set; }
        [Column("FatherSpouseMName")]
        [StringLength(255)]
        public string FatherSpouseMname { get; set; }
        [Column("FatherSpouseLName")]
        [StringLength(255)]
        public string FatherSpouseLname { get; set; }
        [StringLength(255)]
        public string MotherPrefix { get; set; }
        [Column("MotherFName")]
        [StringLength(255)]
        public string MotherFname { get; set; }
        [Column("MotherMName")]
        [StringLength(255)]
        public string MotherMname { get; set; }
        [Column("MotherLName")]
        [StringLength(255)]
        public string MotherLname { get; set; }
        [StringLength(255)]
        public string BirthPlaceCity { get; set; }
        [StringLength(255)]
        public string BirthPlaceCountry { get; set; }
        [StringLength(255)]
        public string MaritalStatus { get; set; }
        [StringLength(255)]
        public string Nationality { get; set; }
        [StringLength(255)]
        public string ResidentialStatus { get; set; }
        [StringLength(255)]
        public string Religion { get; set; }
        [StringLength(255)]
        public string Caste { get; set; }
        [StringLength(255)]
        public string OccupationType { get; set; }
        [StringLength(255)]
        public string BusinessFirm { get; set; }
        [StringLength(255)]
        public string SalariedEmployed { get; set; }
        [StringLength(255)]
        public string Designation { get; set; }
        [StringLength(255)]
        public string AnnualIncome { get; set; }
        [StringLength(255)]
        public string NatureOrganisation { get; set; }
        [StringLength(255)]
        public string NatureOrganisationOtherRemark { get; set; }
        [StringLength(255)]
        public string ThresholdLimit { get; set; }
        [Column("operationInstruction")]
        [StringLength(255)]
        public string OperationInstruction { get; set; }
        [StringLength(255)]
        public string InitialDepositType { get; set; }
        [StringLength(255)]
        public string InitialDepositChequeNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InitialDepositDate { get; set; }
        [StringLength(255)]
        public string InitialDepositAmount { get; set; }
        [StringLength(255)]
        public string EducationQualification { get; set; }
        public bool? CarLoan { get; set; }
        public bool? ConsumerLoan { get; set; }
        public bool? HomeLoan { get; set; }
        public bool? BusinessLoan { get; set; }
        public bool? EducationLoan { get; set; }
        [Column("otherRemark")]
        [StringLength(255)]
        public string OtherRemark { get; set; }
        public bool? Newspaper { get; set; }
        public bool? Staff { get; set; }
        [Column("RelativeORFriend")]
        public bool? RelativeOrfriend { get; set; }
        public bool? Advertise { get; set; }
        public bool? OtherCredit { get; set; }
        [Column("EBankCosmoRupay")]
        public bool? EbankCosmoRupay { get; set; }
        [Column("EBankCosmoVisa")]
        public bool? EbankCosmoVisa { get; set; }
        [Column("EUPI")]
        public bool? EUPI { get; set; }
        [Column("EInternetBanking")]
        public bool? EInternetBanking { get; set; }
        [Column("EIMPS")]
        public bool? EIMPS { get; set; }
        [StringLength(255)]
        public string DepositNumberOfTxnPerMonth { get; set; }
        [StringLength(255)]
        public string DepositValueOfTxnPerMonth { get; set; }
        [StringLength(255)]
        public string DepositTotalFundsDepositedinThreeMonth { get; set; }
        [StringLength(255)]
        public string DepositTxnPerMonthChequeOrTransfer { get; set; }
        [StringLength(255)]
        public string WithdrawNumberOfTxnPerMonth { get; set; }
        [StringLength(255)]
        public string WithdrawValueOfTxnPerMonth { get; set; }
        [StringLength(255)]
        public string WithdrawTotalFundsDepositedinThreeMonth { get; set; }
        [StringLength(255)]
        public string WithdrawTxnPerMonthChequeOrTransfer { get; set; }
        public bool? NominationYesOrNo { get; set; }
        [StringLength(255)]
        public string NominationPreName { get; set; }
        [Column("NominationFName")]
        [StringLength(255)]
        public string NominationFname { get; set; }
        [Column("NominationMName")]
        [StringLength(255)]
        public string NominationMname { get; set; }
        [Column("NominationLName")]
        [StringLength(255)]
        public string NominationLname { get; set; }
        [StringLength(255)]
        public string NominationAddress1 { get; set; }
        [StringLength(255)]
        public string NominationAddress2 { get; set; }
        [StringLength(255)]
        public string NominationAddress3 { get; set; }
        [StringLength(255)]
        public string NominationCountry { get; set; }
        [StringLength(255)]
        public string Nominationcity { get; set; }
        [StringLength(255)]
        public string NominationState { get; set; }
        [StringLength(255)]
        public string NominationPincode { get; set; }
        [StringLength(255)]
        public string OfficeUseTabInitialAmount { get; set; }
        [Column("OfficeUseTabDCNo")]
        [StringLength(255)]
        public string OfficeUseTabDcno { get; set; }
        [StringLength(255)]
        public string OfficeUseTabDate { get; set; }
        [Column("OfficeUseTabWFNO")]
        [StringLength(255)]
        public string OfficeUseTabWfno { get; set; }
        [StringLength(255)]
        public string OfficeUseTabOfficerName { get; set; }
        [StringLength(255)]
        public string OfficeUseTabTicketNo { get; set; }
        [Column("AOCTabVISAORATM")]
        public bool? AoctabVisaoratm { get; set; }
        [Column("AOCTabSMS")]
        public bool? AoctabSms { get; set; }
        [Column("AOCTabACandKYC")]
        public bool? AoctabAcandKyc { get; set; }
        [Column("AOCTabIBComformationRequest")]
        public bool? AoctabIbcomformationRequest { get; set; }
        [Column("AOCTabChequeBookRequest")]
        public bool? AoctabChequeBookRequest { get; set; }
        [Column("AOCTabCosmoNetComformationRequest")]
        public bool? AoctabCosmoNetComformationRequest { get; set; }
        [Column("AOCTabAutoSweep")]
        public bool? AoctabAutoSweep { get; set; }
        [Column("AOCTabOfficerName")]
        [StringLength(255)]
        public string AoctabOfficerName { get; set; }
        [Column("AOCTabTicketNo")]
        [StringLength(255)]
        public string AoctabTicketNo { get; set; }
        [Column("ABCTabOfficerName")]
        [StringLength(255)]
        public string AbctabOfficerName { get; set; }
        [Column("ABCTabTicketNo")]
        [StringLength(255)]
        public string AbctabTicketNo { get; set; }
        [StringLength(255)]
        public string CreatedBy { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column("updateBy")]
        [StringLength(255)]
        public string UpdateBy { get; set; }
        public bool? IsJointHolder { get; set; }
        public int JointHolderCount { get; set; }
        [Column("CBSCustomerID")]
        [StringLength(50)]
        public string CbscustomerId { get; set; }
        public string PhysicallyChall { get; set; }
        public string PhysicallyChall_YN { get; set; }
        public string TDSReason { get; set; }
        public string VerifyDOC { get; set; }
        public DateTime NomineeAge { get; set; }
        public string NomineeRelation { get; set; }
        public string Profession { get; set; }
        public string Minor { get; set; }
    }
}
