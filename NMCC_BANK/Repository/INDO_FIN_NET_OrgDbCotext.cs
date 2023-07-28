using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using INDO_FIN_NET.Repository.Data;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Models.CurrentModels;

#nullable disable

namespace INDO_FIN_NET.Repository
{
    public partial class RSSBPRODDbCotext : DbContext
    {
        public RSSBPRODDbCotext()
        {
        }

        public RSSBPRODDbCotext(DbContextOptions<RSSBPRODDbCotext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdmAadharVaultErrorLog> AdmAadharVaultErrorLogs { get; set; }
        public virtual DbSet<AdmAdhaarVaultDatum> AdmAdhaarVaultData { get; set; }
        public virtual DbSet<AdmAidocumentExtraction> AdmAidocumentExtractions { get; set; }
        public virtual DbSet<AdmBirlaCustAadharDetail> AdmBirlaCustAadharDetails { get; set; }
        public virtual DbSet<AdmBranchtbl> AdmBranchtbls { get; set; }
        public virtual DbSet<AdmBankDetail> AdmBankDetails { get; set; }
        public virtual DbSet<AdmCallerInfo> AdmCallerInfos { get; set; }
        public virtual DbSet<adm_PhysicallyChallenged> adm_PhysicallyChallengeds { get; set; }
        public virtual DbSet<adm_TDSReasonCode> adm_TDSReasonCodes { get; set; }
        public virtual DbSet<adm_ProfessionDetails> adm_ProfessionDetailss { get; set; }
        public virtual DbSet<AdmCkycCustomer> AdmCkycCustomers { get; set; }
        public virtual DbSet<AdmCkycCustomerDetail> AdmCkycCustomerDetails { get; set; }
        public virtual DbSet<AdmCkycDownloadLog> AdmCkycDownloadLogs { get; set; }
        public virtual DbSet<AdmCkycSearchLog> AdmCkycSearchLogs { get; set; }
        public virtual DbSet<AdmCosmosCustomerDetail> AdmCosmosCustomerDetails { get; set; }
        public virtual DbSet<AdmCosmosCustomerJointAccountHolderDetail> AdmCosmosCustomerJointAccountHolderDetails { get; set; }
        public virtual DbSet<AdmCosmosSol> AdmCosmosSols { get; set; }
        public virtual DbSet<AdmCustomerAadharDetail> AdmCustomerAadharDetails { get; set; }
        public virtual DbSet<AdmCustomerDocument> AdmCustomerDocuments { get; set; }
        public virtual DbSet<AdmCustomerDocumentCategory> AdmCustomerDocumentCategories { get; set; }
        public virtual DbSet<AdmCustomerDocumentDetailsMapping> AdmCustomerDocumentDetailsMappings { get; set; }
        public virtual DbSet<AdmCustomerDocumentsCkyc> AdmCustomerDocumentsCkycs { get; set; }
        public virtual DbSet<AdmCustomerDocumentsDetail> AdmCustomerDocumentsDetails { get; set; }
        public virtual DbSet<AdmCustomerDocumentsDetail1> AdmCustomerDocumentsDetails1 { get; set; }
        public virtual DbSet<AdmCustomerIpvDetail> AdmCustomerIpvDetails { get; set; }
        public virtual DbSet<AdmCustomerManagement> AdmCustomerManagements { get; set; }
        public virtual DbSet<AdmCustomerManagementAdmCustomerDocument> AdmCustomerManagementAdmCustomerDocuments { get; set; }
        public virtual DbSet<AdmCustomerOtherDetail> AdmCustomerOtherDetails { get; set; }
        public virtual DbSet<AdmDepartmenttbl> AdmDepartmenttbls { get; set; }
        public virtual DbSet<AdmDigiAadharDatum> AdmDigiAadharData { get; set; }
        public virtual DbSet<AdmDigiDrivingLicence> AdmDigiDrivingLicences { get; set; }
        public virtual DbSet<AdmDigiPanCard> AdmDigiPanCards { get; set; }
        public virtual DbSet<AdmDigilockererrorlog> AdmDigilockererrorlogs { get; set; }
        public virtual DbSet<AdmErrorlogAidocumentType> AdmErrorlogAidocumentTypes { get; set; }
        public virtual DbSet<AdmErrorlogDocumentExtraction> AdmErrorlogDocumentExtractions { get; set; }
        public virtual DbSet<AdmFlagMainTain> AdmFlagMainTains { get; set; }
        public virtual DbSet<AdmFlagMainTain1> AdmFlagMainTains1 { get; set; }
        public virtual DbSet<AdmHsmKeyDetail> AdmHsmKeyDetails { get; set; }
        public virtual DbSet<AdmIndoFinNetOtpdetail> AdmIndoFinNetOtpdetails { get; set; }
        public virtual DbSet<AdmIndoQeemailOtpdetail> AdmIndoQeemailOtpdetails { get; set; }
        public virtual DbSet<AdmIndoQeotpdetail> AdmIndoQeotpdetails { get; set; }
        public virtual DbSet<AdmInsertAidocumentExtraction> AdmInsertAidocumentExtractions { get; set; }
        public virtual DbSet<AdmInsertCustomerDetail> AdmInsertCustomerDetails { get; set; }
        public virtual DbSet<AdmKycCustomerDetail> AdmKycCustomerDetails { get; set; }
        public virtual DbSet<AdmKycCustomerDetail1> AdmKycCustomerDetail1s { get; set; }
        public virtual DbSet<AdmSilResponseDetail> AdmSilResponseDetails { get; set; }

        public virtual DbSet<AdmLivenessServiceLog> AdmLivenessServiceLogs { get; set; }
        public virtual DbSet<AdmLogMaintain> AdmLogMaintains { get; set; }
        public virtual DbSet<AdmOtpdetail> AdmOtpdetails { get; set; }
        public virtual DbSet<AdmOtpdetailsForQuickEnroll> AdmOtpdetailsForQuickEnrolls { get; set; }
        public virtual DbSet<AdmPanVerificationDetail> AdmPanVerificationDetails { get; set; }
        public virtual DbSet<AdmPanVerificationServiceLog> AdmPanVerificationServiceLogs { get; set; }
        public virtual DbSet<AdmRegiontbl> AdmRegiontbls { get; set; }
        public virtual DbSet<AdmRoletbl> AdmRoletbls { get; set; }
        public virtual DbSet<AdmThirdPartyVendor> AdmThirdPartyVendors { get; set; }
        public virtual DbSet<AdmUploadedDocument> AdmUploadedDocuments { get; set; }
        public virtual DbSet<AdmVciprequestDetail> AdmVciprequestDetails { get; set; }
        public virtual DbSet<AdmVehicleInsuranceDetail> AdmVehicleInsuranceDetails { get; set; }
        public virtual DbSet<AdmVideoKycDetail> AdmVideoKycDetails { get; set; }
        public virtual DbSet<CkycServiceTransactionLog> CkycServiceTransactionLogs { get; set; }
        public virtual DbSet<EmpDetail> EmpDetails { get; set; }
        public virtual DbSet<IndoAdminDetail> IndoAdminDetails { get; set; }
        public virtual DbSet<IndoKycEmailMobileDetail> IndoKycEmailMobileDetails { get; set; }
        public virtual DbSet<IndoUserDetail> IndoUserDetails { get; set; }
        public virtual DbSet<IndofinErrorLog> IndofinErrorLogs { get; set; }
        public virtual DbSet<MasterCountry> MasterCountries { get; set; }
        public virtual DbSet<AdmRekycCustomerDetail> AdmRekycCustomerDetails { get; set; }
        public virtual DbSet<StateCode> StateCodes { get; set; }
        public virtual DbSet<SysCountry> SysCountries { get; set; }
        public virtual DbSet<SysDocumentType> SysDocumentTypes { get; set; }
        public virtual DbSet<SysState> SysStates { get; set; }
        public virtual DbSet<TblAadhaarBaseEsignLog> TblAadhaarBaseEsignLogs { get; set; }
        public virtual DbSet<TblBcCertificateLogsDetail> TblBcCertificateLogsDetails { get; set; }
        public virtual DbSet<TblCustCertificateDetail> TblCustCertificateDetails { get; set; }
        public virtual DbSet<TblCustCertificateLogsDetail> TblCustCertificateLogsDetails { get; set; }
        public virtual DbSet<TblCustomerDetail> TblCustomerDetails { get; set; }
        public virtual DbSet<admBranchDetails> admBranchDetailsS { get; set; }
        public virtual DbSet<admname> admnames { get; set; }
        public virtual DbSet<admCASTDetail> admCASTDetails { get; set; }
        public virtual DbSet<adm_ReligionDetail> adm_ReligionDetails { get; set; }
        public virtual DbSet<adm_maritalstatus> adm_maritalstatuss { get; set; }
        public virtual DbSet<adm_residence> adm_residences { get; set; }
        public virtual DbSet<adm_residencedocument> adm_residencedocuments { get; set; }
        public virtual DbSet<adm_residenceYN> adm_residenceYNs { get; set; }
        public virtual DbSet<adm_phonebanking> adm_phonebankings { get; set; }
        public virtual DbSet<adm_OccupationDetails> adm_OccupationDetailss { get; set; }
        public virtual DbSet<TblDesignation> TblDesignations { get; set; }
        public virtual DbSet<TblEsignCustomerDetail> TblEsignCustomerDetails { get; set; }
        public virtual DbSet<TblMaintainSessionForCustDetail> TblMaintainSessionForCustDetails { get; set; }
        public virtual DbSet<TblOrgBcCertificateDetail> TblOrgBcCertificateDetails { get; set; }
        public virtual DbSet<TblOrgCaCertificateLogDetail> TblOrgCaCertificateLogDetails { get; set; }
        public virtual DbSet<TblVcipverfierDetail> TblVcipverfierDetails { get; set; }
        public virtual DbSet<TblVerificationType> TblVerificationTypes { get; set; }
        public virtual DbSet<TblVerifierShift> TblVerifierShifts { get; set; }
        public virtual DbSet<Mobile_result> Mobile_result { get; set; }
        public virtual DbSet<AdmApproveddata> AdmApproveddatas { get; set; }
        public virtual DbSet<adm_CustRkycstatus> adm_CustRkycstatuss { get; set; }
        public virtual DbSet<AdmRejectdata> AdmRejectdatas { get; set; }
        public virtual DbSet<AdmCosmosjointDetails> AdmCosmosjointDetails { get; set; }
        public virtual DbSet<AdmRejectrekycdata> AdmRejectrekycdatas { get; set; }
        public virtual DbSet<AdmCustGrid> AdmCustGrids { get; set; }
        public virtual DbSet<AdmDigiAadharlogDetailsGrid> AdmDigiAadharlogDetailsGrids { get; set; }
        public virtual DbSet<AdmpanlogDetailsGrid> AdmpanlogDetailsGrids { get; set; }
        public virtual DbSet<bulkup> bulkup { get; set; }
        public virtual DbSet<AgentPendingExcel> AgentPendingExcels { get; set; }
        public virtual DbSet<AgentRejectedExcel> AgentRejectedExcels { get; set; }
        public virtual DbSet<AdmCosmosMasterData> AdmCosmosMasterData { get; set; }
        public virtual DbSet<adm_OrganizationName> Adm_OrganizationNames { get; set; }
        public virtual DbSet<adm_OraganisationRegistration> adm_OraganisationRegistrations { get; set; }

        public virtual DbSet<AdmJointApproveddata> AdmJointApproveddata { get; set; }
        public virtual DbSet<AdmCustLinkReq> AdmCustLinkReq { get; set; }
        public virtual DbSet<PAN_Verification> PAN_Verifications { get; set; }
        public virtual DbSet<GSTIN_Verification> GSTIN_Verifications { get; set; }
        public virtual DbSet<CIN_Verification> CIN_Verifications { get; set; }
        public virtual DbSet<CAFCustomerDetails> CAFCustomerDetails { get; set; }
        public virtual DbSet<MSME_Verification> MSME_Verifications { get; set; }
        public virtual DbSet<Current_DocumentDetails> Current_DocumentDetails { get; set; }
        public virtual DbSet<sysPinAndStateMasters> sysPinAndStateMasters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("DefaultConnection2");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdmAadharVaultErrorLog>(entity =>
            {
                entity.Property(e => e.KeyVersion).IsFixedLength(true);
            });

            modelBuilder.Entity<AdmAidocumentExtraction>(entity =>
            {
                entity.Property(e => e.CardName).IsUnicode(false);

                entity.Property(e => e.Dob).IsUnicode(false);

                entity.Property(e => e.Documentid).IsUnicode(false);

                entity.Property(e => e.Fname).IsUnicode(false);

                entity.Property(e => e.Fullname).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.Initialname).IsUnicode(false);

                entity.Property(e => e.Lname).IsUnicode(false);

                entity.Property(e => e.Mname).IsUnicode(false);

                entity.Property(e => e.Relationtype).IsUnicode(false);

                entity.Property(e => e.StatusCode).IsUnicode(false);
            });

            modelBuilder.Entity<AdmBranchtbl>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("BranchId_Pk");

                entity.Property(e => e.BranchId).ValueGeneratedNever();

                entity.Property(e => e.BranchName).IsUnicode(false);

                entity.Property(e => e.RegionId).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<AdmCkycCustomer>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK_adm_CosmosCustomer");

                entity.Property(e => e.AadhaarNumber).IsUnicode(false);

                entity.Property(e => e.AccountHolderType).IsUnicode(false);

                entity.Property(e => e.AccountHolderTypeFlag).IsUnicode(false);

                entity.Property(e => e.AdditionOrDeletionOfRelatedPerson1).IsUnicode(false);

                entity.Property(e => e.AdditionOrDeletionOfRelatedPerson2).IsUnicode(false);

                entity.Property(e => e.AdditionOrDeletionOfRelatedPerson3).IsUnicode(false);

                entity.Property(e => e.AjaddressType).IsUnicode(false);

                entity.Property(e => e.Ajcity).IsUnicode(false);

                entity.Property(e => e.Ajcountry).IsUnicode(false);

                entity.Property(e => e.Ajline1).IsUnicode(false);

                entity.Property(e => e.Ajline2).IsUnicode(false);

                entity.Property(e => e.Ajline3).IsUnicode(false);

                entity.Property(e => e.AjpinCode).IsUnicode(false);

                entity.Property(e => e.Ajstate).IsUnicode(false);

                entity.Property(e => e.ApplicationDate).IsUnicode(false);

                entity.Property(e => e.CkycAccountType).IsUnicode(false);

                entity.Property(e => e.CkycApplicationType).IsUnicode(false);

                entity.Property(e => e.CkycNumber).IsUnicode(false);

                entity.Property(e => e.ConstitutionType).IsUnicode(false);

                entity.Property(e => e.CpoaddressType).IsUnicode(false);

                entity.Property(e => e.Cpocity).IsUnicode(false);

                entity.Property(e => e.Cpocountry).IsUnicode(false);

                entity.Property(e => e.Cpodistrict).IsUnicode(false);

                entity.Property(e => e.Cpoline1).IsUnicode(false);

                entity.Property(e => e.Cpoline2).IsUnicode(false);

                entity.Property(e => e.Cpoline3).IsUnicode(false);

                entity.Property(e => e.CpopinCode).IsUnicode(false);

                entity.Property(e => e.Cpospecification).IsUnicode(false);

                entity.Property(e => e.Cpostate).IsUnicode(false);

                entity.Property(e => e.Cposubmitted).IsUnicode(false);

                entity.Property(e => e.CustomerCitizenship).IsUnicode(false);

                entity.Property(e => e.CustomerDob).IsUnicode(false);

                entity.Property(e => e.CustomerEmailId).IsUnicode(false);

                entity.Property(e => e.CustomerFatherFirstName).IsUnicode(false);

                entity.Property(e => e.CustomerFatherLastName).IsUnicode(false);

                entity.Property(e => e.CustomerFatherMiddleName).IsUnicode(false);

                entity.Property(e => e.CustomerFatherNamePrefix).IsUnicode(false);

                entity.Property(e => e.CustomerFirstName).IsUnicode(false);

                entity.Property(e => e.CustomerGender).IsUnicode(false);

                entity.Property(e => e.CustomerLastName).IsUnicode(false);

                entity.Property(e => e.CustomerMaidenFirstName).IsUnicode(false);

                entity.Property(e => e.CustomerMaidenLastName).IsUnicode(false);

                entity.Property(e => e.CustomerMaidenMiddleName).IsUnicode(false);

                entity.Property(e => e.CustomerMaidenNamePrefix).IsUnicode(false);

                entity.Property(e => e.CustomerMaritalStatus).IsUnicode(false);

                entity.Property(e => e.CustomerMiddleName).IsUnicode(false);

                entity.Property(e => e.CustomerMotherFirstName).IsUnicode(false);

                entity.Property(e => e.CustomerMotherLastName).IsUnicode(false);

                entity.Property(e => e.CustomerMotherMiddleName).IsUnicode(false);

                entity.Property(e => e.CustomerMotherNamePrefix).IsUnicode(false);

                entity.Property(e => e.CustomerOccupation).IsUnicode(false);

                entity.Property(e => e.CustomerPrefix).IsUnicode(false);

                entity.Property(e => e.CustomerResidentialStatus).IsUnicode(false);

                entity.Property(e => e.CustomerRiskCategory).IsUnicode(false);

                entity.Property(e => e.DrivingLicenceExpiryDate).IsUnicode(false);

                entity.Property(e => e.DrivingLicenceNumber).IsUnicode(false);

                entity.Property(e => e.EkycAuthDocNumber).IsUnicode(false);

                entity.Property(e => e.FaxNo).IsUnicode(false);

                entity.Property(e => e.FaxNoStd).IsUnicode(false);

                entity.Property(e => e.FlagForAddressinJurisdiction).IsUnicode(false);

                entity.Property(e => e.FlagForFatherorSpouseName).IsUnicode(false);

                entity.Property(e => e.FlagSpouse).IsUnicode(false);

                entity.Property(e => e.Form60).IsUnicode(false);

                entity.Property(e => e.InstituteCode).IsUnicode(false);

                entity.Property(e => e.InstituteName).IsUnicode(false);

                entity.Property(e => e.IsocontryCodeOfJurisdictionResidence).IsUnicode(false);

                entity.Property(e => e.IsocountryCodeOfBirth).IsUnicode(false);

                entity.Property(e => e.KycnumberOfRelatedPerson1).IsUnicode(false);

                entity.Property(e => e.KycnumberOfRelatedPerson2).IsUnicode(false);

                entity.Property(e => e.KycnumberOfRelatedPerson3).IsUnicode(false);

                entity.Property(e => e.KycverificationCarriedDate).IsUnicode(false);

                entity.Property(e => e.KycverificationEmpCode).IsUnicode(false);

                entity.Property(e => e.KycverificationEmpDesignation).IsUnicode(false);

                entity.Property(e => e.KycverificationEmpName).IsUnicode(false);

                entity.Property(e => e.KycverificationEmpbranch).IsUnicode(false);

                entity.Property(e => e.LcaddressType).IsUnicode(false);

                entity.Property(e => e.Lccity).IsUnicode(false);

                entity.Property(e => e.Lccountry).IsUnicode(false);

                entity.Property(e => e.Lcdistrict).IsUnicode(false);

                entity.Property(e => e.Lcline1).IsUnicode(false);

                entity.Property(e => e.Lcline2).IsUnicode(false);

                entity.Property(e => e.Lcline3).IsUnicode(false);

                entity.Property(e => e.LcpinCode).IsUnicode(false);

                entity.Property(e => e.Lcstate).IsUnicode(false);

                entity.Property(e => e.MobileNumber).IsUnicode(false);

                entity.Property(e => e.MobileNumberIsd).IsUnicode(false);

                entity.Property(e => e.NprldocNumber).IsUnicode(false);

                entity.Property(e => e.Nreganumber).IsUnicode(false);

                entity.Property(e => e.OfficeTelephoneNumber).IsUnicode(false);

                entity.Property(e => e.OfficeTelephoneStd).IsUnicode(false);

                entity.Property(e => e.OfflineAadhaarDocNumber).IsUnicode(false);

                entity.Property(e => e.OtherDocumentNumber).IsUnicode(false);

                entity.Property(e => e.OtherDocumentType).IsUnicode(false);

                entity.Property(e => e.PanCardNumber).IsUnicode(false);

                entity.Property(e => e.PassportExpiryDate).IsUnicode(false);

                entity.Property(e => e.Passportnumber).IsUnicode(false);

                entity.Property(e => e.PlaceOfApplication).IsUnicode(false);

                entity.Property(e => e.PlaceOfBirth).IsUnicode(false);

                entity.Property(e => e.Posubmitted).IsUnicode(false);

                entity.Property(e => e.Ramarks).IsUnicode(false);

                entity.Property(e => e.RelatedPersonAadhaarNumber1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonAadhaarNumber2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonAadhaarNumber3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCitizenship).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCitizenship2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCitizenship3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoaddressType).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoaddressType2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoaddressType3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpocity).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpocity2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpocity3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpocountry).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpocountry2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpocountry3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpodistrict).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpodistrict2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpodistrict3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline12).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline13).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline22).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline23).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline32).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpoline33).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpopinCode).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpopinCode2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpopinCode3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpostate).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpostate2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonCpostate3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDob).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDob2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDob3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDrivingLicenceExpiryDate1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDrivingLicenceExpiryDate2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDrivingLicenceExpiryDate3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDrivingLicenceNumber1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDrivingLicenceNumber2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonDrivingLicenceNumber3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherFirstName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherFirstName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherFirstName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherLastName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherLastName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherLastName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherMiddleName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherMiddleName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherMiddleName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherNamePrefix1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherNamePrefix2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFatherNamePrefix3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFlagForFatherorSpouseName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFlagForFatherorSpouseName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFlagForFatherorSpouseName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFname1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFname2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonFname3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonGender).IsUnicode(false);

                entity.Property(e => e.RelatedPersonGender2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonGender3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonIsocontryCodeOfJurisdictionResidence).IsUnicode(false);

                entity.Property(e => e.RelatedPersonIsocontryCodeOfJurisdictionResidence2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonIsocontryCodeOfJurisdictionResidence3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonIsocountryCodeOfBirth).IsUnicode(false);

                entity.Property(e => e.RelatedPersonIsocountryCodeOfBirth2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonIsocountryCodeOfBirth3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonLname1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonLname2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonLname3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenFirstName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenFirstName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenFirstName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenLastName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenLastName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenLastName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenMiddleName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenMiddleName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenMiddleName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenNamePrefix1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenNamePrefix2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaidenNamePrefix3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaritalStatus).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaritalStatus2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMaritalStatus3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMname1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMname2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMname3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherFirstName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherFirstName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherFirstName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherLastName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherLastName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherLastName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherMiddleName1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherMiddleName2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherMiddleName3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherNamePrefix1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherNamePrefix2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonMotherNamePrefix3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonNreganumber1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonNreganumber2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonNreganumber3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonOccupation).IsUnicode(false);

                entity.Property(e => e.RelatedPersonOccupation2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonOccupation3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPanCardNumber1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPanCardNumber2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPanCardNumber3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPassportExpiryDate1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPassportExpiryDate2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPassportExpiryDate3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPassportnumber1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPassportnumber2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPassportnumber3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPlaceOfBirth).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPlaceOfBirth2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPlaceOfBirth3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPrefix1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPrefix2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonPrefix3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonResidentialStatus).IsUnicode(false);

                entity.Property(e => e.RelatedPersonResidentialStatus2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonResidentialStatus3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonTaxIdentificationNumber).IsUnicode(false);

                entity.Property(e => e.RelatedPersonTaxIdentificationNumber2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonTaxIdentificationNumber3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonType1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonType2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonType3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonVoterIdNumber1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonVoterIdNumber2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonVoterIdNumber3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonotherDocumentNumber1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonotherDocumentNumber2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonotherDocumentNumber3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonotherDocumentType1).IsUnicode(false);

                entity.Property(e => e.RelatedPersonotherDocumentType2).IsUnicode(false);

                entity.Property(e => e.RelatedPersonotherDocumentType3).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentNumber11).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentNumber12).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentNumber13).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentNumber21).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentNumber22).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentNumber23).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentType11).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentType12).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentType13).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentType21).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentType22).IsUnicode(false);

                entity.Property(e => e.RelatedPersonsimplifiedPoidocumentType23).IsUnicode(false);

                entity.Property(e => e.ResidenceTelephoneNumber).IsUnicode(false);

                entity.Property(e => e.ResidenceTelephoneStd).IsUnicode(false);

                entity.Property(e => e.SimplifiedPoidocumentNumber1).IsUnicode(false);

                entity.Property(e => e.SimplifiedPoidocumentNumber2).IsUnicode(false);

                entity.Property(e => e.SimplifiedPoidocumentType1).IsUnicode(false);

                entity.Property(e => e.SimplifiedPoidocumentType2).IsUnicode(false);

                entity.Property(e => e.TaxIdentificationNumber).IsUnicode(false);

                entity.Property(e => e.VoterIdNumber).IsUnicode(false);
            });

            modelBuilder.Entity<AdmCkycCustomerDetail>(entity =>
            {
                entity.Property(e => e.Ccid).IsUnicode(false);

                entity.Property(e => e.CkycrefNo).IsUnicode(false);

                entity.Property(e => e.Ckycstatus).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.IdentificationNumber).IsUnicode(false);

                entity.Property(e => e.IsApprove).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsReject).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsUpdate).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsUpdateApprove).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsUpdateCkycApproved).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsVerfy).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.Isckycapproved).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.LoanId).IsUnicode(false);

                entity.Property(e => e.Lob).IsUnicode(false);

                entity.Property(e => e.PoiStatus).IsFixedLength(true);

                entity.Property(e => e.RejectReason).IsUnicode(false);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.System).IsUnicode(false);
            });

            modelBuilder.Entity<AdmCosmosCustomerDetail>(entity =>
            {
                entity.Property(e => e.AbctabOfficerName).IsUnicode(false);

                entity.Property(e => e.AbctabTicketNo).IsUnicode(false);

                entity.Property(e => e.AccountNumber).IsUnicode(false);

                entity.Property(e => e.AccountType).IsUnicode(false);

                entity.Property(e => e.AnnualIncome).IsUnicode(false);

                entity.Property(e => e.AoctabOfficerName).IsUnicode(false);

                entity.Property(e => e.AoctabTicketNo).IsUnicode(false);

                entity.Property(e => e.BirthPlaceCity).IsUnicode(false);

                entity.Property(e => e.BirthPlaceCountry).IsUnicode(false);

                entity.Property(e => e.Branch).IsUnicode(false);

                entity.Property(e => e.BusinessFirm).IsUnicode(false);

                entity.Property(e => e.Caste).IsUnicode(false);

                entity.Property(e => e.CkycNumber).IsUnicode(false);

                entity.Property(e => e.CreatedBy).IsUnicode(false);

                entity.Property(e => e.CustomerDob).IsUnicode(false);

                entity.Property(e => e.CustomerFname).IsUnicode(false);

                entity.Property(e => e.CustomerLname).IsUnicode(false);

                entity.Property(e => e.CustomerMname).IsUnicode(false);

                entity.Property(e => e.CustomerPrefix).IsUnicode(false);

                entity.Property(e => e.DepositNumberOfTxnPerMonth).IsUnicode(false);

                entity.Property(e => e.DepositTotalFundsDepositedinThreeMonth).IsUnicode(false);

                entity.Property(e => e.DepositTxnPerMonthChequeOrTransfer).IsUnicode(false);

                entity.Property(e => e.DepositValueOfTxnPerMonth).IsUnicode(false);

                entity.Property(e => e.Designation).IsUnicode(false);

                entity.Property(e => e.EducationQualification).IsUnicode(false);

                entity.Property(e => e.FatherSpouseFname).IsUnicode(false);

                entity.Property(e => e.FatherSpouseLname).IsUnicode(false);

                entity.Property(e => e.FatherSpouseMname).IsUnicode(false);

                entity.Property(e => e.FatherSpousePrefix).IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.InitialDepositAmount).IsUnicode(false);

                entity.Property(e => e.InitialDepositChequeNo).IsUnicode(false);

                entity.Property(e => e.InitialDepositType).IsUnicode(false);

                entity.Property(e => e.MaidenFname).IsUnicode(false);

                entity.Property(e => e.MaidenLname).IsUnicode(false);

                entity.Property(e => e.MaidenMname).IsUnicode(false);

                entity.Property(e => e.MaidenPrefix).IsUnicode(false);

                entity.Property(e => e.MaritalStatus).IsUnicode(false);

                entity.Property(e => e.MotherFname).IsUnicode(false);

                entity.Property(e => e.MotherLname).IsUnicode(false);

                entity.Property(e => e.MotherMname).IsUnicode(false);

                entity.Property(e => e.MotherPrefix).IsUnicode(false);

                entity.Property(e => e.Nationality).IsUnicode(false);

                entity.Property(e => e.NatureOrganisation).IsUnicode(false);

                entity.Property(e => e.NatureOrganisationOtherRemark).IsUnicode(false);

                entity.Property(e => e.NominationCountry).IsUnicode(false);

                entity.Property(e => e.NominationFname).IsUnicode(false);

                entity.Property(e => e.NominationLname).IsUnicode(false);

                entity.Property(e => e.NominationMname).IsUnicode(false);

                entity.Property(e => e.NominationPincode).IsUnicode(false);

                entity.Property(e => e.NominationPreName).IsUnicode(false);

                entity.Property(e => e.NominationState).IsUnicode(false);

                entity.Property(e => e.Nominationcity).IsUnicode(false);

                entity.Property(e => e.OccupationType).IsUnicode(false);

                entity.Property(e => e.OfficeUseTabDate).IsUnicode(false);

                entity.Property(e => e.OfficeUseTabDcno).IsUnicode(false);

                entity.Property(e => e.OfficeUseTabInitialAmount).IsUnicode(false);

                entity.Property(e => e.OfficeUseTabOfficerName).IsUnicode(false);

                entity.Property(e => e.OfficeUseTabTicketNo).IsUnicode(false);

                entity.Property(e => e.OfficeUseTabWfno).IsUnicode(false);

                entity.Property(e => e.OperationInstruction).IsUnicode(false);

                entity.Property(e => e.OtherRemark).IsUnicode(false);

                entity.Property(e => e.Religion).IsUnicode(false);

                entity.Property(e => e.ResidentialStatus).IsUnicode(false);

                entity.Property(e => e.SalariedEmployed).IsUnicode(false);

                entity.Property(e => e.ThresholdLimit).IsUnicode(false);

                entity.Property(e => e.UpdateBy).IsUnicode(false);

                entity.Property(e => e.WithdrawNumberOfTxnPerMonth).IsUnicode(false);

                entity.Property(e => e.WithdrawTotalFundsDepositedinThreeMonth).IsUnicode(false);

                entity.Property(e => e.WithdrawTxnPerMonthChequeOrTransfer).IsUnicode(false);

                entity.Property(e => e.WithdrawValueOfTxnPerMonth).IsUnicode(false);
            });

            modelBuilder.Entity<AdmCosmosCustomerJointAccountHolderDetail>(entity =>
            {
                entity.Property(e => e.JointHolderCountNumber).IsUnicode(false);
            });

            modelBuilder.Entity<AdmCosmosSol>(entity =>
            {
                entity.Property(e => e.SolDesc).IsUnicode(false);

                entity.Property(e => e.SolId).IsUnicode(false);
            });

            modelBuilder.Entity<AdmCustomerAadharDetail>(entity =>
            {
                entity.Property(e => e.CustEnrollId).IsUnicode(false);
            });

            modelBuilder.Entity<AdmCustomerDocument>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("('TRUE')");
            });

            modelBuilder.Entity<AdmCustomerDocumentCategory>(entity =>
            {
                entity.Property(e => e.CkycCategoryId).IsUnicode(false);

                entity.Property(e => e.NomineeCode).IsUnicode(false);

                entity.Property(e => e.NomineeFlag).IsUnicode(false);

                entity.Property(e => e.NpsPoaCode).IsUnicode(false);

                entity.Property(e => e.NpsPoaFlag).IsUnicode(false);

                entity.Property(e => e.NpsPoiCode).IsUnicode(false);

                entity.Property(e => e.NpsPoiFlag).IsUnicode(false);

                entity.Property(e => e.PoaCode).IsUnicode(false);

                entity.Property(e => e.PoaFlag).IsUnicode(false);

                entity.Property(e => e.PoiCode).IsUnicode(false);

                entity.Property(e => e.PoiFlag).IsUnicode(false);
            });

            modelBuilder.Entity<AdmCustomerDocumentsCkyc>(entity =>
            {
                entity.Property(e => e.CustomerDocumentId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AdmCustomerDocumentsDetail>(entity =>
            {
                entity.Property(e => e.CreatedBy).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DocumentCategoryCode).IsUnicode(false);

                entity.Property(e => e.DocumentId).IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("('TRUE')");

                entity.Property(e => e.RowGuid)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(newid())");
            });

            modelBuilder.Entity<AdmCustomerIpvDetail>(entity =>
            {
                entity.Property(e => e.ClientName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmCustomerManagement>(entity =>
            {
                entity.Property(e => e.ApplicantFlag).HasDefaultValueSql("('False')");

                entity.Property(e => e.ApprovedBy).IsUnicode(false);

                entity.Property(e => e.BillConsumerNo).IsUnicode(false);

                entity.Property(e => e.Ckycno).IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsAccountMasterDone).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsActive).HasDefaultValueSql("('TRUE')");

                entity.Property(e => e.IsBc).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsBillResource).IsUnicode(false);

                entity.Property(e => e.IsCbsbulk).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsCbscustomer).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsCustomer).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsCustomerMasterDone).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsDisplay).HasDefaultValueSql("('TRUE')");

                entity.Property(e => e.IsEkycdone).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsLocalRegistrationdone).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsNomineeMasterDone).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsPanVerified).HasDefaultValueSql("('false')");

                entity.Property(e => e.IsSystemUser).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.KycadharApprove).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.KycmobileApprove).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.KycpanApprove).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.PanDescription).IsUnicode(false);

                entity.Property(e => e.PerUidaiarea).IsUnicode(false);

                entity.Property(e => e.PerUidaicity).IsUnicode(false);

                entity.Property(e => e.PerUidaicountry).IsUnicode(false);

                entity.Property(e => e.PerUidaidistrict).IsUnicode(false);

                entity.Property(e => e.PerUidaistate).IsUnicode(false);

                entity.Property(e => e.PerUidaizipcode).IsUnicode(false);

                entity.Property(e => e.PerzipcodeId).IsUnicode(false);

                entity.Property(e => e.Uidaiarea).IsUnicode(false);

                entity.Property(e => e.Uidaicity).IsUnicode(false);

                entity.Property(e => e.Uidaicountry).IsUnicode(false);

                entity.Property(e => e.Uidaidistrict).IsUnicode(false);

                entity.Property(e => e.Uidaistate).IsUnicode(false);

                entity.Property(e => e.Uidaizipcode).IsUnicode(false);

                entity.Property(e => e.ZipCodeId).IsUnicode(false);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.AdmCustomerManagements)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_sys_States_adm_CustomerManagement");
            });

            modelBuilder.Entity<AdmCustomerManagementAdmCustomerDocument>(entity =>
            {
                entity.Property(e => e.ApprovedBy).IsUnicode(false);

                entity.HasOne(d => d.CustomerDocument)
                    .WithMany(p => p.AdmCustomerManagementAdmCustomerDocuments)
                    .HasForeignKey(d => d.CustomerDocumentId)
                    .HasConstraintName("FK__admCustom__custo__1F398B65");
            });

            modelBuilder.Entity<AdmDepartmenttbl>(entity =>
            {
                entity.HasKey(e => e.DeptId)
                    .HasName("DeptId_Pk");

                entity.Property(e => e.DeptId).ValueGeneratedNever();

                entity.Property(e => e.DeptName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmDigiAadharDatum>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.District).IsUnicode(false);

                entity.Property(e => e.Dob).IsUnicode(false);

                entity.Property(e => e.Firstname).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.House).IsUnicode(false);

                entity.Property(e => e.Lastname).IsUnicode(false);

                entity.Property(e => e.Locality).IsUnicode(false);

                entity.Property(e => e.Middlename).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Pc).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Street).IsUnicode(false);

                entity.Property(e => e.Uid).IsUnicode(false);

                entity.Property(e => e.Vtc).IsUnicode(false);
            });

            modelBuilder.Entity<AdmDigiDrivingLicence>(entity =>
            {
                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.Dob).IsUnicode(false);

                entity.Property(e => e.Drvlc).IsUnicode(false);

                entity.Property(e => e.Firstname).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.Lastname).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Orgname).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);

                entity.Property(e => e.Swd).IsUnicode(false);
            });

            modelBuilder.Entity<AdmDigiPanCard>(entity =>
            {
                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.CustomerId).IsUnicode(false);

                entity.Property(e => e.Dob).IsUnicode(false);

                entity.Property(e => e.Firstname).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.Lastname).IsUnicode(false);

                entity.Property(e => e.Middlename).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Orgname).IsUnicode(false);

                entity.Property(e => e.Panno).IsUnicode(false);

                entity.Property(e => e.PanverifiedOn).IsFixedLength(true);
            });

            modelBuilder.Entity<AdmDigilockererrorlog>(entity =>
            {
                entity.Property(e => e.Digilockertype).IsUnicode(false);

                entity.Property(e => e.Status).IsUnicode(false);
            });

            modelBuilder.Entity<AdmErrorlogAidocumentType>(entity =>
            {
                entity.Property(e => e.Status).IsUnicode(false);
            });

            modelBuilder.Entity<AdmErrorlogDocumentExtraction>(entity =>
            {
                entity.Property(e => e.Status).IsUnicode(false);
            });

            modelBuilder.Entity<AdmFlagMainTain>(entity =>
            {
                entity.Property(e => e.CustActivityDoneFlg).HasDefaultValueSql("((0))");

                entity.Property(e => e.CustomerCompleteStage).IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IsAssistedCustFlag).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsCafpdf).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDigiAadharSumbitted).HasDefaultValueSql("('0')");

                entity.Property(e => e.IsDigiPansumbitted).HasDefaultValueSql("('0')");

                entity.Property(e => e.IsDigilDrlcsumbitted).HasDefaultValueSql("('0')");

                entity.Property(e => e.IsDocumentSubmit).HasDefaultValueSql("('0')");

                entity.Property(e => e.IsIpvSkip).HasDefaultValueSql("('FALSE')");

                entity.Property(e => e.IsIpvrecord).HasDefaultValueSql("('0')");

                entity.Property(e => e.IsIpvsubmit).HasDefaultValueSql("('0')");

                entity.Property(e => e.IsQuickEnrollSubmit).HasDefaultValueSql("('0')");

                entity.Property(e => e.IsSavingAcc).HasDefaultValueSql("((0))");

                entity.Property(e => e.IssummarysheetSubmit).HasDefaultValueSql("('0')");

                entity.Property(e => e.ProceedwithOcr).IsUnicode(false);

                entity.Property(e => e.ShareAadharNumber).IsUnicode(false);
            });

            modelBuilder.Entity<AdmHsmKeyDetail>(entity =>
            {
                entity.Property(e => e.Validity).IsFixedLength(true);
            });

            modelBuilder.Entity<AdmIndoFinNetOtpdetail>(entity =>
            {
                entity.Property(e => e.EmailOtp).IsUnicode(false);

                entity.Property(e => e.VerficationType).IsUnicode(false);
            });

            modelBuilder.Entity<AdmIndoQeemailOtpdetail>(entity =>
            {
                entity.HasKey(e => e.UniqueId)
                    .HasName("PK__adm_Indo__A2A2A54AA6D126D6");
            });

            modelBuilder.Entity<AdmIndoQeotpdetail>(entity =>
            {
                entity.HasKey(e => e.UniqueId)
                    .HasName("PK__adm_Indo__A2A2A54A79360B1A");
            });

            modelBuilder.Entity<AdmInsertAidocumentExtraction>(entity =>
            {
                entity.Property(e => e.CardName).IsUnicode(false);

                entity.Property(e => e.Dob).IsUnicode(false);

                entity.Property(e => e.Documentid).IsUnicode(false);

                entity.Property(e => e.Fname).IsUnicode(false);

                entity.Property(e => e.Fullname).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.Initialname).IsUnicode(false);

                entity.Property(e => e.Lname).IsUnicode(false);

                entity.Property(e => e.Mname).IsUnicode(false);

                entity.Property(e => e.Relationtype).IsUnicode(false);

                entity.Property(e => e.StatusCode).IsUnicode(false);
            });

            modelBuilder.Entity<AdmInsertCustomerDetail>(entity =>
            {
                entity.Property(e => e.Adhaarhash).IsUnicode(false);

                entity.Property(e => e.KeyLabelName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmKycCustomerDetail>(entity =>
            {
                entity.Property(e => e.ClientPermCity).IsUnicode(false);

                entity.Property(e => e.ClientPermCountry).IsUnicode(false);

                entity.Property(e => e.ClientPermPin).IsUnicode(false);

                entity.Property(e => e.ClientPermState).IsUnicode(false);

                entity.Property(e => e.CorresponencePermanentAddressSameFlag).IsUnicode(false);

                entity.Property(e => e.Dob).IsUnicode(false);

                entity.Property(e => e.FatherName).IsUnicode(false);

                entity.Property(e => e.FirstName).IsUnicode(false);

                entity.Property(e => e.Gender).IsUnicode(false);

                entity.Property(e => e.LastName).IsUnicode(false);

                entity.Property(e => e.MiddleName).IsUnicode(false);

                entity.Property(e => e.MobileDetailsCode).IsUnicode(false);
            });

            modelBuilder.Entity<AdmLivenessServiceLog>(entity =>
            {
                entity.Property(e => e.Response).IsUnicode(false);
            });

            modelBuilder.Entity<AdmLogMaintain>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AdmOtpdetail>(entity =>
            {
                entity.Property(e => e.OtpId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AdmOtpdetailsForQuickEnroll>(entity =>
            {
                entity.Property(e => e.EmailOtp).IsUnicode(false);
            });

            modelBuilder.Entity<AdmPanVerificationDetail>(entity =>
            {
                entity.Property(e => e.NameprintedonPan).IsUnicode(false);
            });

            modelBuilder.Entity<AdmRegiontbl>(entity =>
            {
                entity.HasKey(e => e.RegionId)
                    .HasName("RegionId_Pk");

                entity.Property(e => e.RegionId).ValueGeneratedNever();

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.RegionName).IsUnicode(false);
            });

            modelBuilder.Entity<AdmRoletbl>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("RoleId_Pk");

                entity.Property(e => e.RoleId).ValueGeneratedNever();

                entity.Property(e => e.Roletype).IsUnicode(false);
            });

            modelBuilder.Entity<AdmUploadedDocument>(entity =>
            {
                entity.Property(e => e.CustomerId).IsUnicode(false);

                entity.Property(e => e.DocId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<AdmVehicleInsuranceDetail>(entity =>
            {
                entity.Property(e => e.City).IsUnicode(false);

                entity.Property(e => e.InsuranceId).ValueGeneratedOnAdd();

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.State).IsUnicode(false);
            });

            modelBuilder.Entity<AdmVideoKycDetail>(entity =>
            {
                entity.Property(e => e.IpvId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<EmpDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<IndoAdminDetail>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.IsLogin).HasDefaultValueSql("((0))");

                entity.Property(e => e.OrganizationName).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.RoleType).IsUnicode(false);

                entity.Property(e => e.SessionKey).IsUnicode(false);
            });

            modelBuilder.Entity<IndoKycEmailMobileDetail>(entity =>
            {
                entity.HasKey(e => e.MobileEmailId)
                    .HasName("PK_ABMS_EmailMobileDetails");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.EmailMobileCode).IsUnicode(false);
            });

            modelBuilder.Entity<IndoUserDetail>(entity =>
            {
                entity.Property(e => e.Branch).IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.Region).IsUnicode(false);

               // entity.Property(e => e.Roletype).IsUnicode(false);
            });

            modelBuilder.Entity<IndofinErrorLog>(entity =>
            {
                entity.Property(e => e.ActionName).IsUnicode(false);

                entity.Property(e => e.ControllerName).IsUnicode(false);

                entity.Property(e => e.ErrorMsg).IsUnicode(false);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MasterCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("PK__MASTER_C__8037C7D6B2811071");

                entity.Property(e => e.Country).IsUnicode(false);

                entity.Property(e => e.CountryCode).IsUnicode(false);
            });

            modelBuilder.Entity<StateCode>(entity =>
            {
                entity.HasKey(e => e.StateId)
                    .HasName("PK__State_Co__AF9338F75A82339E");

                entity.Property(e => e.CkycstateCode).IsUnicode(false);

                entity.Property(e => e.CountryId).IsUnicode(false);

                entity.Property(e => e.StateCode1).IsUnicode(false);

                entity.Property(e => e.StateName).IsUnicode(false);
            });

            modelBuilder.Entity<SysCountry>(entity =>
            {
                entity.Property(e => e.Code).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);
            });

            modelBuilder.Entity<SysState>(entity =>
            {
                entity.Property(e => e.Cbsstatecode).IsUnicode(false);

                entity.Property(e => e.Statename).IsUnicode(false);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.SysStates)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__sys_State__count__468862B0");
            });

            modelBuilder.Entity<TblDesignation>(entity =>
            {
                entity.HasKey(e => e.DesignationId)
                    .HasName("PK__TblDesig__BABD60DEE6805369");

                entity.Property(e => e.DesignationDesc).IsUnicode(false);
            });

            modelBuilder.Entity<TblEsignCustomerDetail>(entity =>
            {
                entity.Property(e => e.EcustFirstName).IsUnicode(false);

                entity.Property(e => e.EcustLastName).IsUnicode(false);

                entity.Property(e => e.EcustMailid).IsUnicode(false);

                entity.Property(e => e.EcustMiddleName).IsUnicode(false);

                entity.Property(e => e.EdocFileDummmy).IsUnicode(false);

                entity.Property(e => e.EeighthCustFirstName).IsUnicode(false);

                entity.Property(e => e.EeighthCustLastName).IsUnicode(false);

                entity.Property(e => e.EeighthCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EeighthMailid).IsUnicode(false);

                entity.Property(e => e.EfifthCustFirstName).IsUnicode(false);

                entity.Property(e => e.EfifthCustLastName).IsUnicode(false);

                entity.Property(e => e.EfifthCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EfirstCustFirstName).IsUnicode(false);

                entity.Property(e => e.EfirstCustLastName).IsUnicode(false);

                entity.Property(e => e.EfirstCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EfirstMailid).IsUnicode(false);

                entity.Property(e => e.EfourthCustFirstName).IsUnicode(false);

                entity.Property(e => e.EfourthCustLastName).IsUnicode(false);

                entity.Property(e => e.EfourthCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EfourthMailid).IsUnicode(false);

                entity.Property(e => e.EninthCustFirstName).IsUnicode(false);

                entity.Property(e => e.EninthCustLastName).IsUnicode(false);

                entity.Property(e => e.EninthCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EninthMailid).IsUnicode(false);

                entity.Property(e => e.EsecCustFirstName).IsUnicode(false);

                entity.Property(e => e.EsecCustLastName).IsUnicode(false);

                entity.Property(e => e.EsecCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EsecMailid).IsUnicode(false);

                entity.Property(e => e.EseventhCustFirstName).IsUnicode(false);

                entity.Property(e => e.EseventhCustLastName).IsUnicode(false);

                entity.Property(e => e.EseventhCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EseventhMailid).IsUnicode(false);

                entity.Property(e => e.EsixCustFirstName).IsUnicode(false);

                entity.Property(e => e.EsixCustLastName).IsUnicode(false);

                entity.Property(e => e.EsixCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EsixMailId).IsUnicode(false);

                entity.Property(e => e.EthirdCustFisrtName).IsUnicode(false);

                entity.Property(e => e.EthirdCustLastName).IsUnicode(false);

                entity.Property(e => e.EthirdCustMiddleName).IsUnicode(false);

                entity.Property(e => e.EthirdMailid).IsUnicode(false);

                entity.Property(e => e.SignDocDummy).IsUnicode(false);
            });

            modelBuilder.Entity<TblMaintainSessionForCustDetail>(entity =>
            {
                entity.HasKey(e => e.CustSessionId)
                    .HasName("PK__Tbl_Main__CE85154C343D9BC4");
            });

            modelBuilder.Entity<TblOrgCaCertificateLogDetail>(entity =>
            {
                entity.Property(e => e.OrgCaCertEmailId).IsUnicode(false);

                entity.Property(e => e.OrgCaCertOrgName).IsUnicode(false);

                entity.Property(e => e.OrgCaCertSerialNo).IsUnicode(false);

                entity.Property(e => e.OrgCaCertValidFrom).IsUnicode(false);

                entity.Property(e => e.OrgCaCertValidTo).IsUnicode(false);
            });

            modelBuilder.Entity<TblVcipverfierDetail>(entity =>
            {
                entity.HasKey(e => e.VerfierId)
                    .HasName("PK__tblVCIPv__01B571B17B0A7643");
            });

            modelBuilder.Entity<TblVerificationType>(entity =>
            {
                entity.Property(e => e.SrNo).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TblVerifierShift>(entity =>
            {
                entity.HasKey(e => e.ShiftId)
                    .HasName("PK__tblVerif__527AD697E7B4DFA6");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
