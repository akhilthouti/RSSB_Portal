using System;
using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models.Organisation
{
    public class ClsCustAccountFormDetails
    {
        public string MainHolderapplicantFNAME { get; set; }
        public string MainHolderapplicantMNAME { get; set; }


        public string MainHolderapplicantLNAME { get; set; }

        public string MainHolderapplicantCustID { get; set; }
        public string firstapplicantFNAME { get; set; }
        public string firstapplicantMNAME { get; set; }

        public string firstapplicantLNAME { get; set; }

        public string firstapplicantCustID { get; set; }

        public string SecondapplicantFNAME { get; set; }
        public string SecondapplicantMNAME { get; set; }

        public string SecondapplicantLNAME { get; set; }

        public string SecondapplicantCustID { get; set; }

        public string ThirdapplicantFNAME { get; set; }
        public string ThirdapplicantMNAME { get; set; }

        public string ThirdapplicantLNAME { get; set; }

        public string ThirdapplicantCustID { get; set; }

        public string FourthapplicantFNAME { get; set; }
        public string FourthapplicantMNAME { get; set; }

        public string FourthapplicantLNAME { get; set; }

        public string FourthapplicantCustID { get; set; }

        public bool initialDepositCash { get; set; }

        public bool initialDepositCheque { get; set; }

        public string initialDepositChequeNo { get; set; }

        public string initialDepositChequeDate { get; set; }
        public string initialDepositAmount { get; set; }
        
        public string PrimaryCustomerID { get; set; }

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

        //first jointHolder


        public string FirstHolderVerifyCustData1 { get; set; }
        public string FirstHolderSearchType1 { get; set; }
        public string firstapplicantFNAME1 { get; set; }
        public string firstapplicantMNAME1 { get; set; }

        public string firstapplicantLNAME1 { get; set; }

        public string firstapplicantCustID1 { get; set; }
        public string MainHolderapplicantFNAME1 { get; set; }
        public string MainHolderapplicantMNAME1 { get; set; }


        public string MainHolderapplicantLNAME1 { get; set; }
        public string FSubTitle { get; set; }
        public string FJointAdd1 { get; set; }
        public string FJointAdd2 { get; set; }
        public string FJointAdd3 { get; set; }
        public string FCity { get; set; }
        public string FPin { get; set; }
        public string FState { get; set; }
        public string FCountry { get; set; }
        public string FDob { get; set; }
        public string FEmailId { get; set; }
        public string FMobileNo { get; set; }
        public string FPanNo { get; set; }
        public string FGender { get; set; }
        public string FResidence { get; set; }
        public string FDocument { get; set; }
        public string FPassportNo { get; set; }
        public string FResidenceYN { get; set; }
        public string FPhoneBAnking { get; set; }




        public string MaidenPrefixName1 { get; set; }
        public string MaidenFName1 { get; set; }
        public string MaidenMName1 { get; set; }
        public string MaidenLName1 { get; set; }
        public string FatherPrefixName1 { get; set; }
        public string FatherFName1 { get; set; }
        public string FatherMName1 { get; set; }
        public string FatherLName1 { get; set; }

        public string MotherPrefixName1 { get; set; }
        public string MotherFName1 { get; set; }
        public string MotherMName1 { get; set; }
        public string MotherLName1 { get; set; }

        public string BirthPlaceCity1 { get; set; }
        public string BirthPlaceCountry1 { get; set; }
        public string Branch1 { get; set; }

        public string ckycIdentifier1 { get; set; }

        public string MaritalStatus1 { get; set; }
        public string ResidentialStatus1 { get; set; }
        public string Religion1 { get; set; }

        public string Caste1 { get; set; }

        public string OccupationType1 { get; set; }

        public string BusinessFirm1 { get; set; }

        public string SalaryEmployed1 { get; set; }

        public string Designation1 { get; set; }

        public string EducationQualification1 { get; set; }

        public string AnnualIncome1 { get; set; }

        public string ThresholdLimit1 { get; set; }
        public string NatureOfOrganization1 { get; set; }
        public string NatureOfOrganizationRemark1 { get; set; }

        public string Nationality1 { get; set; }
        public bool initialDepositCash1 { get; set; }
        public bool initialDepositCheque1 { get; set; }

        public string initialDepositChequeNo1 { get; set; }

        public string initialDepositChequeDate1 { get; set; }
        public string initialDepositAmount1 { get; set; }

        //SECOND JOIND HOLDER
        public string SecondHolderVerifyCustData2 { get; set; }

        public string SecondapplicantFNAME2 { get; set; }
        public string SecondapplicantMNAME2 { get; set; }
        public string SecondapplicantLNAME2 { get; set; }
        public string SecondHolderSearchType2 { get; set; }
        public string SecondapplicantCustID2 { get; set; }
        public string SSubTitle { get; set; }
        public string SJointAdd1 { get; set; }
        public string SJointAdd2 { get; set; }
        public string SJointAdd3 { get; set; }
        public string SCity { get; set; }
        public string SPin { get; set; }
        public string SState { get; set; }
        public string SCountry { get; set; }
        public string SDob { get; set; }
        public string SEmailId { get; set; }
        public string SMobileNo { get; set; }
        public string SPanNo { get; set; }
        public string SGender { get; set; }
        public string SResidence { get; set; }
        public string SDocument { get; set; }
        public string SPassportNo { get; set; }
        public string SResidenceYN { get; set; }
        public string SPhoneBAnking { get; set; }
        public string MaidenPrefixName2 { get; set; }
        public string MaidenFName2 { get; set; }
        public string MaidenMName2 { get; set; }
        public string MaidenLName2 { get; set; }
        public string FatherPrefixName2 { get; set; }
        public string FatherFName2 { get; set; }
        public string FatherMName2 { get; set; }
        public string FatherLName2 { get; set; }

        public string MotherPrefixName2 { get; set; }
        public string MotherFName2 { get; set; }
        public string MotherMName2 { get; set; }
        public string MotherLName2 { get; set; }

        public string BirthPlaceCity2 { get; set; }
        public string BirthPlaceCountry2 { get; set; }
        public string Branch2 { get; set; }

        public string ckycIdentifier2 { get; set; }

        public string MaritalStatus2 { get; set; }
        public string ResidentialStatus2 { get; set; }
        public string Religion2 { get; set; }

        public string Caste2 { get; set; }

        public string OccupationType2 { get; set; }

        public string BusinessFirm2 { get; set; }

        public string SalaryEmployed2 { get; set; }

        public string Designation2 { get; set; }

        public string EducationQualification2 { get; set; }

        public string AnnualIncome2 { get; set; }

        public string ThresholdLimit2 { get; set; }
        public string NatureOfOrganization2 { get; set; }
        public string NatureOfOrganizationRemark2 { get; set; }

        public string Nationality2 { get; set; }
        public bool initialDepositCash2 { get; set; }
        public bool initialDepositCheque2 { get; set; }

        public string initialDepositChequeNo2 { get; set; }

        public string initialDepositChequeDate2 { get; set; }
        public string initialDepositAmount2 { get; set; }

        //THIRD JOINT HOLDER
        public string ThirdHolderVerifyCustData3 { get; set; }
        public string ThirdHolderSearchType3 { get; set; }

        public string ThirdapplicantFNAME3 { get; set; }
        public string ThirdapplicantMNAME3 { get; set; }
        public string ThirdapplicantLNAME3 { get; set; }

        public string ThirdapplicantCustID3 { get; set; }
        public string TSubTitle { get; set; }
        public string TJointAdd1 { get; set; }
        public string TJointAdd2 { get; set; }
        public string TJointAdd3 { get; set; }
        public string TCity { get; set; }
        public string TPin { get; set; }
        public string TState { get; set; }
        public string TCountry { get; set; }
        public string TDob { get; set; }
        public string TEmailId { get; set; }
        public string TMobileNo { get; set; }
        public string TPanNo { get; set; }
        public string TGender { get; set; }
        public string TResidence { get; set; }
        public string TDocument { get; set; }
        public string TPassportNo { get; set; }
        public string TResidenceYN { get; set; }
        public string TPhoneBAnking { get; set; }

        public string MaidenPrefixName3 { get; set; }
        public string MaidenFName3 { get; set; }
        public string MaidenMName3 { get; set; }
        public string MaidenLName3 { get; set; }
        public string FatherPrefixName3 { get; set; }
        public string FatherFName3 { get; set; }
        public string FatherMName3 { get; set; }
        public string FatherLName3 { get; set; }

        public string MotherPrefixName3 { get; set; }
        public string MotherFName3 { get; set; }
        public string MotherMName3 { get; set; }
        public string MotherLName3 { get; set; }

        public string BirthPlaceCity3 { get; set; }
        public string BirthPlaceCountry3 { get; set; }
        public string Branch3 { get; set; }

        public string ckycIdentifier3 { get; set; }

        public string MaritalStatus3 { get; set; }
        public string ResidentialStatus3 { get; set; }
        public string Religion3 { get; set; }

        public string Caste3 { get; set; }

        public string OccupationType3 { get; set; }

        public string BusinessFirm3 { get; set; }

        public string SalaryEmployed3 { get; set; }

        public string Designation3 { get; set; }

        public string EducationQualification3 { get; set; }

        public string AnnualIncome3 { get; set; }

        public string ThresholdLimit3 { get; set; }
        public string NatureOfOrganization3 { get; set; }
        public string NatureOfOrganizationRemark3 { get; set; }

        public string Nationality3 { get; set; }
        public bool initialDepositCash3 { get; set; }
        public bool initialDepositCheque3 { get; set; }

        public bool initialDepositAmount3 { get; set; }

        //Fourth joind Holder 
        public string FourthHolderVerifyCustData4 { get; set; }
        public string FourthHolderSearchType4 { get; set; }
        public string FourthapplicantFNAME4 { get; set; }
        public string FourthapplicantMNAME4 { get; set; }
        public string FourthapplicantLNAME4 { get; set; }
        public string FourthapplicantCustID4 { get; set; }


        public string MaidenPrefixName4 { get; set; }
        public string MaidenFName4 { get; set; }
        public string MaidenMName4 { get; set; }
        public string MaidenLName4 { get; set; }
        public string FatherPrefixName4 { get; set; }
        public string FatherFName4 { get; set; }
        public string FatherMName4 { get; set; }
        public string FatherLName4 { get; set; }

        public string MotherPrefixName4 { get; set; }
        public string MotherFName4 { get; set; }
        public string MotherMName4 { get; set; }
        public string MotherLName4 { get; set; }

        public string BirthPlaceCity4 { get; set; }
        public string BirthPlaceCountry4 { get; set; }
        public string Branch4 { get; set; }

        public string ckycIdentifier4 { get; set; }

        public string MaritalStatus4 { get; set; }
        public string ResidentialStatus4 { get; set; }
        public string Religion4 { get; set; }

        public string Caste4 { get; set; }

        public string OccupationType4 { get; set; }

        public string BusinessFirm4 { get; set; }

        public string SalaryEmployed4 { get; set; }

        public string Designation4 { get; set; }

        public string EducationQualification4 { get; set; }

        public string AnnualIncome4 { get; set; }

        public string ThresholdLimit4 { get; set; }
        public string NatureOfOrganization4 { get; set; }
        public string NatureOfOrganizationRemark4 { get; set; }

        public string Nationality4 { get; set; }
        public bool initialDepositCash4 { get; set; }
        public bool initialDepositCheque4 { get; set; }

        public string initialDepositChequeNo4 { get; set; }

        public string initialDepositChequeDate4 { get; set; }
        public string initialDepositAmount4 { get; set; }



        //public bool CosmoRupayCard { get; set; }

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



        public string Branch { get; set; }

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
        public string PhysicallyChall { get; set; }
        public string PhysicallyChall_YN { get; set; }
        public string TDSReason { get; set; }
        public string VerifyDOC { get; set; }
        public string CustDOB { get; set; }
        public string Minor { get; set; }
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

        public DateTime OfficeUseDate { get; set; }

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

        public bool FormAgree { get; set; }

        public string MainHolderVerifyCustData { get; set; }
        public string MainHolderSearchType { get; set; }

        public string FirstHolderVerifyCustData { get; set; }
        public string FirstHolderSearchType { get; set; }

        public string SecondHolderVerifyCustData { get; set; }
        public string SecondHolderSearchType { get; set; }

        public string ThirdHolderVerifyCustData { get; set; }
        public string ThirdHolderSearchType { get; set; }

        public string FourthHolderVerifyCustData { get; set; }
        public string FourthHolderSearchType { get; set; }

        public bool JointForAccountYes { get; set; }

        public bool JointForAccountNo { get; set; }

        public string isAccountApproveOrReject { get; set; }
    }



}

