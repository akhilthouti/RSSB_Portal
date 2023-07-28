using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CKYC_Customer")]
    public partial class AdmCkycCustomer
    {
        [Key]
        [Column("customerId")]
        public long CustomerId { get; set; }
        [Column("customerPrefix")]
        [StringLength(10)]
        public string CustomerPrefix { get; set; }
        [Column("customerFirstName")]
        [StringLength(50)]
        public string CustomerFirstName { get; set; }
        [Column("customerMiddleName")]
        [StringLength(50)]
        public string CustomerMiddleName { get; set; }
        [Column("customerLastName")]
        [StringLength(50)]
        public string CustomerLastName { get; set; }
        [Column("customerMaidenNamePrefix")]
        [StringLength(10)]
        public string CustomerMaidenNamePrefix { get; set; }
        [Column("customerMaidenFirstName")]
        [StringLength(50)]
        public string CustomerMaidenFirstName { get; set; }
        [Column("customerMaidenMiddleName")]
        [StringLength(50)]
        public string CustomerMaidenMiddleName { get; set; }
        [Column("customerMaidenLastName")]
        [StringLength(50)]
        public string CustomerMaidenLastName { get; set; }
        [Column("flagForFatherorSpouseName")]
        [StringLength(5)]
        public string FlagForFatherorSpouseName { get; set; }
        [Column("flagSpouse")]
        [StringLength(5)]
        public string FlagSpouse { get; set; }
        [Column("customerFatherNamePrefix")]
        [StringLength(10)]
        public string CustomerFatherNamePrefix { get; set; }
        [Column("customerFatherFirstName")]
        [StringLength(50)]
        public string CustomerFatherFirstName { get; set; }
        [Column("customerFatherMiddleName")]
        [StringLength(50)]
        public string CustomerFatherMiddleName { get; set; }
        [Column("customerFatherLastName")]
        [StringLength(50)]
        public string CustomerFatherLastName { get; set; }
        [Column("customerMotherNamePrefix")]
        [StringLength(10)]
        public string CustomerMotherNamePrefix { get; set; }
        [Column("customerMotherFirstName")]
        [StringLength(50)]
        public string CustomerMotherFirstName { get; set; }
        [Column("customerMotherMiddleName")]
        [StringLength(50)]
        public string CustomerMotherMiddleName { get; set; }
        [Column("customerMotherLastName")]
        [StringLength(50)]
        public string CustomerMotherLastName { get; set; }
        [Column("customerDOB")]
        [StringLength(10)]
        public string CustomerDob { get; set; }
        [Column("customerGender")]
        [StringLength(5)]
        public string CustomerGender { get; set; }
        [Column("customerMaritalStatus")]
        [StringLength(5)]
        public string CustomerMaritalStatus { get; set; }
        [Column("customerCitizenship")]
        [StringLength(5)]
        public string CustomerCitizenship { get; set; }
        [Column("customerResidentialStatus")]
        [StringLength(50)]
        public string CustomerResidentialStatus { get; set; }
        [Column("customerOccupation")]
        [StringLength(5)]
        public string CustomerOccupation { get; set; }
        public bool? TaxPurposeJurisdictionOutsideIndia { get; set; }
        [Column("ISOContryCodeOfJurisdictionResidence")]
        [StringLength(20)]
        public string IsocontryCodeOfJurisdictionResidence { get; set; }
        [StringLength(50)]
        public string TaxIdentificationNumber { get; set; }
        [StringLength(50)]
        public string PlaceOfBirth { get; set; }
        [Column("ISOCountryCodeOfBirth")]
        [StringLength(20)]
        public string IsocountryCodeOfBirth { get; set; }
        [StringLength(5)]
        public string Form60 { get; set; }
        public bool? PassportSubmitted { get; set; }
        [StringLength(50)]
        public string Passportnumber { get; set; }
        [StringLength(10)]
        public string PassportExpiryDate { get; set; }
        public bool? VoterIdSubmitted { get; set; }
        [StringLength(50)]
        public string VoterIdNumber { get; set; }
        public bool? PanCardSubmitted { get; set; }
        [StringLength(20)]
        public string PanCardNumber { get; set; }
        public bool? DrivingLicenceSubmitted { get; set; }
        [StringLength(50)]
        public string DrivingLicenceNumber { get; set; }
        [StringLength(10)]
        public string DrivingLicenceExpiryDate { get; set; }
        public bool? AadhaarSubmitted { get; set; }
        [StringLength(50)]
        public string ReferenceKey { get; set; }
        [StringLength(20)]
        public string AadhaarNumber { get; set; }
        [Column("NREGAsubmitted")]
        public bool? Nregasubmitted { get; set; }
        [Column("NREGAnumber")]
        [StringLength(50)]
        public string Nreganumber { get; set; }
        [Column("otherDocumentSubmitted")]
        public bool? OtherDocumentSubmitted { get; set; }
        [Column("otherDocumentType")]
        [StringLength(50)]
        public string OtherDocumentType { get; set; }
        [Column("otherDocumentNumber")]
        [StringLength(50)]
        public string OtherDocumentNumber { get; set; }
        [Column("simplifiedPOIDocument1")]
        public bool? SimplifiedPoidocument1 { get; set; }
        [Column("simplifiedPOIDocumentType1")]
        [StringLength(50)]
        public string SimplifiedPoidocumentType1 { get; set; }
        [Column("simplifiedPOIDocumentNumber1")]
        [StringLength(50)]
        public string SimplifiedPoidocumentNumber1 { get; set; }
        [Column("simplifiedPOIDocument2")]
        public bool? SimplifiedPoidocument2 { get; set; }
        [Column("simplifiedPOIDocumentType2")]
        [StringLength(50)]
        public string SimplifiedPoidocumentType2 { get; set; }
        [Column("simplifiedPOIDocumentNumber2")]
        [StringLength(50)]
        public string SimplifiedPoidocumentNumber2 { get; set; }
        [Column("NPRLDocNumber")]
        [StringLength(50)]
        public string NprldocNumber { get; set; }
        [StringLength(50)]
        public string EkycAuthDocNumber { get; set; }
        [StringLength(50)]
        public string OfflineAadhaarDocNumber { get; set; }
        [Column("CPOaddressType")]
        [StringLength(20)]
        public string CpoaddressType { get; set; }
        [Column("CPOline1")]
        [StringLength(60)]
        public string Cpoline1 { get; set; }
        [Column("CPOline2")]
        [StringLength(60)]
        public string Cpoline2 { get; set; }
        [Column("CPOline3")]
        [StringLength(60)]
        public string Cpoline3 { get; set; }
        [Column("CPOcity")]
        [StringLength(50)]
        public string Cpocity { get; set; }
        [Column("CPOdistrict")]
        [StringLength(50)]
        public string Cpodistrict { get; set; }
        [Column("CPOpinCode")]
        [StringLength(10)]
        public string CpopinCode { get; set; }
        [Column("CPOstate")]
        [StringLength(5)]
        public string Cpostate { get; set; }
        [Column("CPOcountry")]
        [StringLength(5)]
        public string Cpocountry { get; set; }
        [Column("CPOSubmitted")]
        [StringLength(5)]
        public string Cposubmitted { get; set; }
        [Column("CPOSpecification")]
        [StringLength(75)]
        public string Cpospecification { get; set; }
        [Column("LCsameAsCPO")]
        public bool? LcsameAsCpo { get; set; }
        [Column("LCaddressType")]
        [StringLength(20)]
        public string LcaddressType { get; set; }
        [Column("LCline1")]
        [StringLength(60)]
        public string Lcline1 { get; set; }
        [Column("LCline2")]
        [StringLength(60)]
        public string Lcline2 { get; set; }
        [Column("LCline3")]
        [StringLength(60)]
        public string Lcline3 { get; set; }
        [Column("LCcity")]
        [StringLength(50)]
        public string Lccity { get; set; }
        [Column("LCdistrict")]
        [StringLength(50)]
        public string Lcdistrict { get; set; }
        [Column("LCpinCode")]
        [StringLength(10)]
        public string LcpinCode { get; set; }
        [Column("LCstate")]
        [StringLength(5)]
        public string Lcstate { get; set; }
        [Column("LCcountry")]
        [StringLength(5)]
        public string Lccountry { get; set; }
        [Column("POSubmitted")]
        [StringLength(50)]
        public string Posubmitted { get; set; }
        [StringLength(10)]
        public string FlagForAddressinJurisdiction { get; set; }
        [Column("AJaddressType")]
        [StringLength(20)]
        public string AjaddressType { get; set; }
        [Column("AJline1")]
        [StringLength(60)]
        public string Ajline1 { get; set; }
        [Column("AJline2")]
        [StringLength(60)]
        public string Ajline2 { get; set; }
        [Column("AJline3")]
        [StringLength(60)]
        public string Ajline3 { get; set; }
        [Column("AJcity")]
        [StringLength(50)]
        public string Ajcity { get; set; }
        [Column("AJstate")]
        [StringLength(5)]
        public string Ajstate { get; set; }
        [Column("AJcountry")]
        [StringLength(5)]
        public string Ajcountry { get; set; }
        [Column("AJpinCode")]
        [StringLength(10)]
        public string AjpinCode { get; set; }
        [Column("ResidenceTelephoneSTD")]
        [StringLength(10)]
        public string ResidenceTelephoneStd { get; set; }
        [StringLength(10)]
        public string ResidenceTelephoneNumber { get; set; }
        [Column("OfficeTelephoneSTD")]
        [StringLength(10)]
        public string OfficeTelephoneStd { get; set; }
        [StringLength(10)]
        public string OfficeTelephoneNumber { get; set; }
        [Column("MobileNumberISD")]
        [StringLength(10)]
        public string MobileNumberIsd { get; set; }
        [StringLength(10)]
        public string MobileNumber { get; set; }
        [Column("FaxNoSTD")]
        [StringLength(10)]
        public string FaxNoStd { get; set; }
        [StringLength(10)]
        public string FaxNo { get; set; }
        [Column("customerEmailId")]
        [StringLength(100)]
        public string CustomerEmailId { get; set; }
        public bool? RelatedPersonRequired { get; set; }
        [StringLength(5)]
        public string AdditionOrDeletionOfRelatedPerson1 { get; set; }
        [Column("KYCNumberOfRelatedPerson1")]
        [StringLength(20)]
        public string KycnumberOfRelatedPerson1 { get; set; }
        [Column("relatedPersonType1")]
        [StringLength(10)]
        public string RelatedPersonType1 { get; set; }
        [Column("relatedPersonPrefix1")]
        [StringLength(10)]
        public string RelatedPersonPrefix1 { get; set; }
        [Column("relatedPersonFName1")]
        [StringLength(50)]
        public string RelatedPersonFname1 { get; set; }
        [Column("relatedPersonMName1")]
        [StringLength(50)]
        public string RelatedPersonMname1 { get; set; }
        [Column("relatedPersonLName1")]
        [StringLength(50)]
        public string RelatedPersonLname1 { get; set; }
        [Column("relatedPersonMaidenNamePrefix1")]
        [StringLength(10)]
        public string RelatedPersonMaidenNamePrefix1 { get; set; }
        [Column("relatedPersonMaidenFirstName1")]
        [StringLength(50)]
        public string RelatedPersonMaidenFirstName1 { get; set; }
        [Column("relatedPersonMaidenMiddleName1")]
        [StringLength(50)]
        public string RelatedPersonMaidenMiddleName1 { get; set; }
        [Column("relatedPersonMaidenLastName1")]
        [StringLength(50)]
        public string RelatedPersonMaidenLastName1 { get; set; }
        [Column("relatedPersonFlagForFatherorSpouseName1")]
        [StringLength(5)]
        public string RelatedPersonFlagForFatherorSpouseName1 { get; set; }
        [Column("relatedPersonFatherNamePrefix1")]
        [StringLength(10)]
        public string RelatedPersonFatherNamePrefix1 { get; set; }
        [Column("relatedPersonFatherFirstName1")]
        [StringLength(50)]
        public string RelatedPersonFatherFirstName1 { get; set; }
        [Column("relatedPersonFatherMiddleName1")]
        [StringLength(50)]
        public string RelatedPersonFatherMiddleName1 { get; set; }
        [Column("relatedPersonFatherLastName1")]
        [StringLength(50)]
        public string RelatedPersonFatherLastName1 { get; set; }
        [Column("relatedPersonMotherNamePrefix1")]
        [StringLength(10)]
        public string RelatedPersonMotherNamePrefix1 { get; set; }
        [Column("relatedPersonMotherFirstName1")]
        [StringLength(50)]
        public string RelatedPersonMotherFirstName1 { get; set; }
        [Column("relatedPersonMotherMiddleName1")]
        [StringLength(50)]
        public string RelatedPersonMotherMiddleName1 { get; set; }
        [Column("relatedPersonMotherLastName1")]
        [StringLength(50)]
        public string RelatedPersonMotherLastName1 { get; set; }
        [Column("relatedPersonDOB")]
        [StringLength(10)]
        public string RelatedPersonDob { get; set; }
        [Column("relatedPersonGender")]
        [StringLength(5)]
        public string RelatedPersonGender { get; set; }
        [Column("relatedPersonMaritalStatus")]
        [StringLength(5)]
        public string RelatedPersonMaritalStatus { get; set; }
        [Column("relatedPersonCitizenship")]
        [StringLength(5)]
        public string RelatedPersonCitizenship { get; set; }
        [Column("relatedPersonResidentialStatus")]
        [StringLength(50)]
        public string RelatedPersonResidentialStatus { get; set; }
        [Column("relatedPersonOccupation")]
        [StringLength(5)]
        public string RelatedPersonOccupation { get; set; }
        [Column("relatedPersonTaxPurposeJurisdictionOutsideIndia")]
        public bool? RelatedPersonTaxPurposeJurisdictionOutsideIndia { get; set; }
        [Column("relatedPersonISOContryCodeOfJurisdictionResidence")]
        [StringLength(20)]
        public string RelatedPersonIsocontryCodeOfJurisdictionResidence { get; set; }
        [Column("relatedPersonTaxIdentificationNumber")]
        [StringLength(50)]
        public string RelatedPersonTaxIdentificationNumber { get; set; }
        [Column("relatedPersonPlaceOfBirth")]
        [StringLength(50)]
        public string RelatedPersonPlaceOfBirth { get; set; }
        [Column("relatedPersonISOCountryCodeOfBirth")]
        [StringLength(20)]
        public string RelatedPersonIsocountryCodeOfBirth { get; set; }
        [Column("relatedPersonCPOaddressType")]
        [StringLength(20)]
        public string RelatedPersonCpoaddressType { get; set; }
        [Column("relatedPersonCPOline1")]
        [StringLength(60)]
        public string RelatedPersonCpoline1 { get; set; }
        [Column("relatedPersonCPOline2")]
        [StringLength(60)]
        public string RelatedPersonCpoline2 { get; set; }
        [Column("relatedPersonCPOline3")]
        [StringLength(60)]
        public string RelatedPersonCpoline3 { get; set; }
        [Column("relatedPersonCPOcity")]
        [StringLength(50)]
        public string RelatedPersonCpocity { get; set; }
        [Column("relatedPersonCPOdistrict")]
        [StringLength(50)]
        public string RelatedPersonCpodistrict { get; set; }
        [Column("relatedPersonCPOpinCode")]
        [StringLength(10)]
        public string RelatedPersonCpopinCode { get; set; }
        [Column("relatedPersonCPOstate")]
        [StringLength(5)]
        public string RelatedPersonCpostate { get; set; }
        [Column("relatedPersonCPOcountry")]
        [StringLength(5)]
        public string RelatedPersonCpocountry { get; set; }
        [Column("relatedPersonPassportSubmitted1")]
        public bool? RelatedPersonPassportSubmitted1 { get; set; }
        [Column("relatedPersonPassportnumber1")]
        [StringLength(50)]
        public string RelatedPersonPassportnumber1 { get; set; }
        [Column("relatedPersonPassportExpiryDate1")]
        [StringLength(10)]
        public string RelatedPersonPassportExpiryDate1 { get; set; }
        [Column("relatedPersonVoterIdSubmitted1")]
        public bool? RelatedPersonVoterIdSubmitted1 { get; set; }
        [Column("relatedPersonVoterIdNumber1")]
        [StringLength(50)]
        public string RelatedPersonVoterIdNumber1 { get; set; }
        [Column("relatedPersonPanCardSubmitted1")]
        public bool? RelatedPersonPanCardSubmitted1 { get; set; }
        [Column("relatedPersonPanCardNumber1")]
        [StringLength(20)]
        public string RelatedPersonPanCardNumber1 { get; set; }
        [Column("relatedPersonDrivingLicenceSubmitted1")]
        public bool? RelatedPersonDrivingLicenceSubmitted1 { get; set; }
        [Column("relatedPersonDrivingLicenceNumber1")]
        [StringLength(50)]
        public string RelatedPersonDrivingLicenceNumber1 { get; set; }
        [Column("relatedPersonDrivingLicenceExpiryDate1")]
        [StringLength(10)]
        public string RelatedPersonDrivingLicenceExpiryDate1 { get; set; }
        [Column("relatedPersonAadhaarSubmitted1")]
        public bool? RelatedPersonAadhaarSubmitted1 { get; set; }
        [Column("relatedPersonAadhaarNumber1")]
        [StringLength(20)]
        public string RelatedPersonAadhaarNumber1 { get; set; }
        [Column("relatedPersonNREGAsubmitted1")]
        public bool? RelatedPersonNregasubmitted1 { get; set; }
        [Column("relatedPersonNREGAnumber1")]
        [StringLength(50)]
        public string RelatedPersonNreganumber1 { get; set; }
        [Column("relatedPersonotherDocumentSubmitted1")]
        public bool? RelatedPersonotherDocumentSubmitted1 { get; set; }
        [Column("relatedPersonotherDocumentType1")]
        [StringLength(50)]
        public string RelatedPersonotherDocumentType1 { get; set; }
        [Column("relatedPersonotherDocumentNumber1")]
        [StringLength(50)]
        public string RelatedPersonotherDocumentNumber1 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocument11")]
        public bool? RelatedPersonsimplifiedPoidocument11 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentType11")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentType11 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentNumber11")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentNumber11 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocument21")]
        public bool? RelatedPersonsimplifiedPoidocument21 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentType21")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentType21 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentNumber21")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentNumber21 { get; set; }
        public bool? RelatedPersonRequired2 { get; set; }
        [StringLength(5)]
        public string AdditionOrDeletionOfRelatedPerson2 { get; set; }
        [Column("KYCNumberOfRelatedPerson2")]
        [StringLength(20)]
        public string KycnumberOfRelatedPerson2 { get; set; }
        [Column("relatedPersonType2")]
        [StringLength(10)]
        public string RelatedPersonType2 { get; set; }
        [Column("relatedPersonPrefix2")]
        [StringLength(10)]
        public string RelatedPersonPrefix2 { get; set; }
        [Column("relatedPersonFName2")]
        [StringLength(50)]
        public string RelatedPersonFname2 { get; set; }
        [Column("relatedPersonMName2")]
        [StringLength(50)]
        public string RelatedPersonMname2 { get; set; }
        [Column("relatedPersonLName2")]
        [StringLength(50)]
        public string RelatedPersonLname2 { get; set; }
        [Column("relatedPersonMaidenNamePrefix2")]
        [StringLength(10)]
        public string RelatedPersonMaidenNamePrefix2 { get; set; }
        [Column("relatedPersonMaidenFirstName2")]
        [StringLength(50)]
        public string RelatedPersonMaidenFirstName2 { get; set; }
        [Column("relatedPersonMaidenMiddleName2")]
        [StringLength(50)]
        public string RelatedPersonMaidenMiddleName2 { get; set; }
        [Column("relatedPersonMaidenLastName2")]
        [StringLength(50)]
        public string RelatedPersonMaidenLastName2 { get; set; }
        [Column("relatedPersonFlagForFatherorSpouseName2")]
        [StringLength(5)]
        public string RelatedPersonFlagForFatherorSpouseName2 { get; set; }
        [Column("relatedPersonFatherNamePrefix2")]
        [StringLength(10)]
        public string RelatedPersonFatherNamePrefix2 { get; set; }
        [Column("relatedPersonFatherFirstName2")]
        [StringLength(50)]
        public string RelatedPersonFatherFirstName2 { get; set; }
        [Column("relatedPersonFatherMiddleName2")]
        [StringLength(50)]
        public string RelatedPersonFatherMiddleName2 { get; set; }
        [Column("relatedPersonFatherLastName2")]
        [StringLength(50)]
        public string RelatedPersonFatherLastName2 { get; set; }
        [Column("relatedPersonMotherNamePrefix2")]
        [StringLength(10)]
        public string RelatedPersonMotherNamePrefix2 { get; set; }
        [Column("relatedPersonMotherFirstName2")]
        [StringLength(50)]
        public string RelatedPersonMotherFirstName2 { get; set; }
        [Column("relatedPersonMotherMiddleName2")]
        [StringLength(50)]
        public string RelatedPersonMotherMiddleName2 { get; set; }
        [Column("relatedPersonMotherLastName2")]
        [StringLength(50)]
        public string RelatedPersonMotherLastName2 { get; set; }
        [Column("relatedPersonDOB2")]
        [StringLength(10)]
        public string RelatedPersonDob2 { get; set; }
        [Column("relatedPersonGender2")]
        [StringLength(5)]
        public string RelatedPersonGender2 { get; set; }
        [Column("relatedPersonMaritalStatus2")]
        [StringLength(5)]
        public string RelatedPersonMaritalStatus2 { get; set; }
        [Column("relatedPersonCitizenship2")]
        [StringLength(5)]
        public string RelatedPersonCitizenship2 { get; set; }
        [Column("relatedPersonResidentialStatus2")]
        [StringLength(50)]
        public string RelatedPersonResidentialStatus2 { get; set; }
        [Column("relatedPersonOccupation2")]
        [StringLength(5)]
        public string RelatedPersonOccupation2 { get; set; }
        [Column("relatedPersonTaxPurposeJurisdictionOutsideIndia2")]
        public bool? RelatedPersonTaxPurposeJurisdictionOutsideIndia2 { get; set; }
        [Column("relatedPersonISOContryCodeOfJurisdictionResidence2")]
        [StringLength(20)]
        public string RelatedPersonIsocontryCodeOfJurisdictionResidence2 { get; set; }
        [Column("relatedPersonTaxIdentificationNumber2")]
        [StringLength(50)]
        public string RelatedPersonTaxIdentificationNumber2 { get; set; }
        [Column("relatedPersonPlaceOfBirth2")]
        [StringLength(50)]
        public string RelatedPersonPlaceOfBirth2 { get; set; }
        [Column("relatedPersonISOCountryCodeOfBirth2")]
        [StringLength(20)]
        public string RelatedPersonIsocountryCodeOfBirth2 { get; set; }
        [Column("relatedPersonCPOaddressType2")]
        [StringLength(20)]
        public string RelatedPersonCpoaddressType2 { get; set; }
        [Column("relatedPersonCPOline12")]
        [StringLength(60)]
        public string RelatedPersonCpoline12 { get; set; }
        [Column("relatedPersonCPOline22")]
        [StringLength(60)]
        public string RelatedPersonCpoline22 { get; set; }
        [Column("relatedPersonCPOline32")]
        [StringLength(60)]
        public string RelatedPersonCpoline32 { get; set; }
        [Column("relatedPersonCPOcity2")]
        [StringLength(50)]
        public string RelatedPersonCpocity2 { get; set; }
        [Column("relatedPersonCPOdistrict2")]
        [StringLength(50)]
        public string RelatedPersonCpodistrict2 { get; set; }
        [Column("relatedPersonCPOpinCode2")]
        [StringLength(10)]
        public string RelatedPersonCpopinCode2 { get; set; }
        [Column("relatedPersonCPOstate2")]
        [StringLength(5)]
        public string RelatedPersonCpostate2 { get; set; }
        [Column("relatedPersonCPOcountry2")]
        [StringLength(5)]
        public string RelatedPersonCpocountry2 { get; set; }
        [Column("relatedPersonPassportSubmitted2")]
        public bool? RelatedPersonPassportSubmitted2 { get; set; }
        [Column("relatedPersonPassportnumber2")]
        [StringLength(50)]
        public string RelatedPersonPassportnumber2 { get; set; }
        [Column("relatedPersonPassportExpiryDate2")]
        [StringLength(10)]
        public string RelatedPersonPassportExpiryDate2 { get; set; }
        [Column("relatedPersonVoterIdSubmitted2")]
        public bool? RelatedPersonVoterIdSubmitted2 { get; set; }
        [Column("relatedPersonVoterIdNumber2")]
        [StringLength(50)]
        public string RelatedPersonVoterIdNumber2 { get; set; }
        [Column("relatedPersonPanCardSubmitted2")]
        public bool? RelatedPersonPanCardSubmitted2 { get; set; }
        [Column("relatedPersonPanCardNumber2")]
        [StringLength(20)]
        public string RelatedPersonPanCardNumber2 { get; set; }
        [Column("relatedPersonDrivingLicenceSubmitted2")]
        public bool? RelatedPersonDrivingLicenceSubmitted2 { get; set; }
        [Column("relatedPersonDrivingLicenceNumber2")]
        [StringLength(50)]
        public string RelatedPersonDrivingLicenceNumber2 { get; set; }
        [Column("relatedPersonDrivingLicenceExpiryDate2")]
        [StringLength(10)]
        public string RelatedPersonDrivingLicenceExpiryDate2 { get; set; }
        [Column("relatedPersonAadhaarSubmitted2")]
        public bool? RelatedPersonAadhaarSubmitted2 { get; set; }
        [Column("relatedPersonAadhaarNumber2")]
        [StringLength(20)]
        public string RelatedPersonAadhaarNumber2 { get; set; }
        [Column("relatedPersonNREGAsubmitted2")]
        public bool? RelatedPersonNregasubmitted2 { get; set; }
        [Column("relatedPersonNREGAnumber2")]
        [StringLength(50)]
        public string RelatedPersonNreganumber2 { get; set; }
        [Column("relatedPersonotherDocumentSubmitted2")]
        public bool? RelatedPersonotherDocumentSubmitted2 { get; set; }
        [Column("relatedPersonotherDocumentType2")]
        [StringLength(50)]
        public string RelatedPersonotherDocumentType2 { get; set; }
        [Column("relatedPersonotherDocumentNumber2")]
        [StringLength(50)]
        public string RelatedPersonotherDocumentNumber2 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocument12")]
        public bool? RelatedPersonsimplifiedPoidocument12 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentType12")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentType12 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentNumber12")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentNumber12 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocument22")]
        public bool? RelatedPersonsimplifiedPoidocument22 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentType22")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentType22 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentNumber22")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentNumber22 { get; set; }
        public bool? RelatedPersonRequired3 { get; set; }
        [StringLength(5)]
        public string AdditionOrDeletionOfRelatedPerson3 { get; set; }
        [Column("KYCNumberOfRelatedPerson3")]
        [StringLength(20)]
        public string KycnumberOfRelatedPerson3 { get; set; }
        [Column("relatedPersonType3")]
        [StringLength(10)]
        public string RelatedPersonType3 { get; set; }
        [Column("relatedPersonPrefix3")]
        [StringLength(10)]
        public string RelatedPersonPrefix3 { get; set; }
        [Column("relatedPersonFName3")]
        [StringLength(50)]
        public string RelatedPersonFname3 { get; set; }
        [Column("relatedPersonMName3")]
        [StringLength(50)]
        public string RelatedPersonMname3 { get; set; }
        [Column("relatedPersonLName3")]
        [StringLength(50)]
        public string RelatedPersonLname3 { get; set; }
        [Column("relatedPersonMaidenNamePrefix3")]
        [StringLength(10)]
        public string RelatedPersonMaidenNamePrefix3 { get; set; }
        [Column("relatedPersonMaidenFirstName3")]
        [StringLength(50)]
        public string RelatedPersonMaidenFirstName3 { get; set; }
        [Column("relatedPersonMaidenMiddleName3")]
        [StringLength(50)]
        public string RelatedPersonMaidenMiddleName3 { get; set; }
        [Column("relatedPersonMaidenLastName3")]
        [StringLength(50)]
        public string RelatedPersonMaidenLastName3 { get; set; }
        [Column("relatedPersonFlagForFatherorSpouseName3")]
        [StringLength(5)]
        public string RelatedPersonFlagForFatherorSpouseName3 { get; set; }
        [Column("relatedPersonFatherNamePrefix3")]
        [StringLength(10)]
        public string RelatedPersonFatherNamePrefix3 { get; set; }
        [Column("relatedPersonFatherFirstName3")]
        [StringLength(50)]
        public string RelatedPersonFatherFirstName3 { get; set; }
        [Column("relatedPersonFatherMiddleName3")]
        [StringLength(50)]
        public string RelatedPersonFatherMiddleName3 { get; set; }
        [Column("relatedPersonFatherLastName3")]
        [StringLength(50)]
        public string RelatedPersonFatherLastName3 { get; set; }
        [Column("relatedPersonMotherNamePrefix3")]
        [StringLength(10)]
        public string RelatedPersonMotherNamePrefix3 { get; set; }
        [Column("relatedPersonMotherFirstName3")]
        [StringLength(50)]
        public string RelatedPersonMotherFirstName3 { get; set; }
        [Column("relatedPersonMotherMiddleName3")]
        [StringLength(50)]
        public string RelatedPersonMotherMiddleName3 { get; set; }
        [Column("relatedPersonMotherLastName3")]
        [StringLength(50)]
        public string RelatedPersonMotherLastName3 { get; set; }
        [Column("relatedPersonDOB3")]
        [StringLength(10)]
        public string RelatedPersonDob3 { get; set; }
        [Column("relatedPersonGender3")]
        [StringLength(5)]
        public string RelatedPersonGender3 { get; set; }
        [Column("relatedPersonMaritalStatus3")]
        [StringLength(5)]
        public string RelatedPersonMaritalStatus3 { get; set; }
        [Column("relatedPersonCitizenship3")]
        [StringLength(5)]
        public string RelatedPersonCitizenship3 { get; set; }
        [Column("relatedPersonResidentialStatus3")]
        [StringLength(50)]
        public string RelatedPersonResidentialStatus3 { get; set; }
        [Column("relatedPersonOccupation3")]
        [StringLength(5)]
        public string RelatedPersonOccupation3 { get; set; }
        [Column("relatedPersonTaxPurposeJurisdictionOutsideIndia3")]
        public bool? RelatedPersonTaxPurposeJurisdictionOutsideIndia3 { get; set; }
        [Column("relatedPersonISOContryCodeOfJurisdictionResidence3")]
        [StringLength(20)]
        public string RelatedPersonIsocontryCodeOfJurisdictionResidence3 { get; set; }
        [Column("relatedPersonTaxIdentificationNumber3")]
        [StringLength(50)]
        public string RelatedPersonTaxIdentificationNumber3 { get; set; }
        [Column("relatedPersonPlaceOfBirth3")]
        [StringLength(50)]
        public string RelatedPersonPlaceOfBirth3 { get; set; }
        [Column("relatedPersonISOCountryCodeOfBirth3")]
        [StringLength(20)]
        public string RelatedPersonIsocountryCodeOfBirth3 { get; set; }
        [Column("relatedPersonCPOaddressType3")]
        [StringLength(20)]
        public string RelatedPersonCpoaddressType3 { get; set; }
        [Column("relatedPersonCPOline13")]
        [StringLength(60)]
        public string RelatedPersonCpoline13 { get; set; }
        [Column("relatedPersonCPOline23")]
        [StringLength(60)]
        public string RelatedPersonCpoline23 { get; set; }
        [Column("relatedPersonCPOline33")]
        [StringLength(60)]
        public string RelatedPersonCpoline33 { get; set; }
        [Column("relatedPersonCPOcity3")]
        [StringLength(50)]
        public string RelatedPersonCpocity3 { get; set; }
        [Column("relatedPersonCPOdistrict3")]
        [StringLength(50)]
        public string RelatedPersonCpodistrict3 { get; set; }
        [Column("relatedPersonCPOpinCode3")]
        [StringLength(10)]
        public string RelatedPersonCpopinCode3 { get; set; }
        [Column("relatedPersonCPOstate3")]
        [StringLength(5)]
        public string RelatedPersonCpostate3 { get; set; }
        [Column("relatedPersonCPOcountry3")]
        [StringLength(5)]
        public string RelatedPersonCpocountry3 { get; set; }
        [Column("relatedPersonPassportSubmitted3")]
        public bool? RelatedPersonPassportSubmitted3 { get; set; }
        [Column("relatedPersonPassportnumber3")]
        [StringLength(50)]
        public string RelatedPersonPassportnumber3 { get; set; }
        [Column("relatedPersonPassportExpiryDate3")]
        [StringLength(10)]
        public string RelatedPersonPassportExpiryDate3 { get; set; }
        [Column("relatedPersonVoterIdSubmitted3")]
        public bool? RelatedPersonVoterIdSubmitted3 { get; set; }
        [Column("relatedPersonVoterIdNumber3")]
        [StringLength(50)]
        public string RelatedPersonVoterIdNumber3 { get; set; }
        [Column("relatedPersonPanCardSubmitted3")]
        public bool? RelatedPersonPanCardSubmitted3 { get; set; }
        [Column("relatedPersonPanCardNumber3")]
        [StringLength(20)]
        public string RelatedPersonPanCardNumber3 { get; set; }
        [Column("relatedPersonDrivingLicenceSubmitted3")]
        public bool? RelatedPersonDrivingLicenceSubmitted3 { get; set; }
        [Column("relatedPersonDrivingLicenceNumber3")]
        [StringLength(50)]
        public string RelatedPersonDrivingLicenceNumber3 { get; set; }
        [Column("relatedPersonDrivingLicenceExpiryDate3")]
        [StringLength(10)]
        public string RelatedPersonDrivingLicenceExpiryDate3 { get; set; }
        [Column("relatedPersonAadhaarSubmitted3")]
        public bool? RelatedPersonAadhaarSubmitted3 { get; set; }
        [Column("relatedPersonAadhaarNumber3")]
        [StringLength(20)]
        public string RelatedPersonAadhaarNumber3 { get; set; }
        [Column("relatedPersonNREGAsubmitted3")]
        public bool? RelatedPersonNregasubmitted3 { get; set; }
        [Column("relatedPersonNREGAnumber3")]
        [StringLength(50)]
        public string RelatedPersonNreganumber3 { get; set; }
        [Column("relatedPersonotherDocumentSubmitted3")]
        public bool? RelatedPersonotherDocumentSubmitted3 { get; set; }
        [Column("relatedPersonotherDocumentType3")]
        [StringLength(50)]
        public string RelatedPersonotherDocumentType3 { get; set; }
        [Column("relatedPersonotherDocumentNumber3")]
        [StringLength(50)]
        public string RelatedPersonotherDocumentNumber3 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocument13")]
        public bool? RelatedPersonsimplifiedPoidocument13 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentType13")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentType13 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentNumber13")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentNumber13 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocument23")]
        public bool? RelatedPersonsimplifiedPoidocument23 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentType23")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentType23 { get; set; }
        [Column("relatedPersonsimplifiedPOIDocumentNumber23")]
        [StringLength(50)]
        public string RelatedPersonsimplifiedPoidocumentNumber23 { get; set; }
        [StringLength(300)]
        public string Ramarks { get; set; }
        [Column("applicationDate")]
        [StringLength(10)]
        public string ApplicationDate { get; set; }
        [StringLength(50)]
        public string PlaceOfApplication { get; set; }
        [Column("KYCVerificationCarriedDate")]
        [StringLength(10)]
        public string KycverificationCarriedDate { get; set; }
        [Column("KYCVerificationEmpName")]
        [StringLength(150)]
        public string KycverificationEmpName { get; set; }
        [Column("KYCVerificationEmpCode")]
        [StringLength(50)]
        public string KycverificationEmpCode { get; set; }
        [Column("KYCVerificationEmpDesignation")]
        [StringLength(50)]
        public string KycverificationEmpDesignation { get; set; }
        [Column("KYCVerificationEMPBranch")]
        [StringLength(50)]
        public string KycverificationEmpbranch { get; set; }
        [StringLength(150)]
        public string InstituteName { get; set; }
        [StringLength(50)]
        public string InstituteCode { get; set; }
        [Column("ckycApplicationType")]
        [StringLength(5)]
        public string CkycApplicationType { get; set; }
        [Column("ckycAccountType")]
        [StringLength(5)]
        public string CkycAccountType { get; set; }
        [Column("constitutionType")]
        [StringLength(5)]
        public string ConstitutionType { get; set; }
        [Column("customerRiskCategory")]
        [StringLength(50)]
        public string CustomerRiskCategory { get; set; }
        [Column("accountHolderTypeFlag")]
        [StringLength(5)]
        public string AccountHolderTypeFlag { get; set; }
        [Column("accountHolderType")]
        [StringLength(5)]
        public string AccountHolderType { get; set; }
        [Column("ckycNumber")]
        [StringLength(20)]
        public string CkycNumber { get; set; }
        [Column("customerDetailId")]
        public long? CustomerDetailId { get; set; }
    }
}
