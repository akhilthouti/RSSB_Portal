using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CustomerOCR_Data")]
    public partial class AdmCustomerOcrDatum
    {
        [Key]
        [Column("OCR_Id")]
        public int OcrId { get; set; }
        [Column("Pan_Name")]
        [StringLength(50)]
        public string PanName { get; set; }
        [Column("Pan_FatherName")]
        [StringLength(50)]
        public string PanFatherName { get; set; }
        [StringLength(20)]
        public string PanNumber { get; set; }
        [Column("Pan_DateOfIssue")]
        [StringLength(20)]
        public string PanDateOfIssue { get; set; }
        [Column("Pan_DOB")]
        [StringLength(20)]
        public string PanDob { get; set; }
        [Column("Aadhar_Name")]
        [StringLength(50)]
        public string AadharName { get; set; }
        [Column("Aadhar_DOB")]
        [StringLength(20)]
        public string AadharDob { get; set; }
        [Column("Aadhar_Gender")]
        [StringLength(10)]
        public string AadharGender { get; set; }
        [Column("Aadhar_FatherName")]
        [StringLength(50)]
        public string AadharFatherName { get; set; }
        [Column("Aadhar_YOB")]
        [StringLength(20)]
        public string AadharYob { get; set; }
        [Column("Aadhar_Number")]
        [StringLength(50)]
        public string AadharNumber { get; set; }
        [Column("Aadhar_MotherName")]
        [StringLength(50)]
        public string AadharMotherName { get; set; }
        [Column("Aadhar_Phone")]
        [StringLength(15)]
        public string AadharPhone { get; set; }
        [Column("Aadhar_Husband")]
        [StringLength(25)]
        public string AadharHusband { get; set; }
        [Column("Aadhar_City")]
        [StringLength(25)]
        public string AadharCity { get; set; }
        [Column("Aadhar_Pin")]
        [StringLength(10)]
        public string AadharPin { get; set; }
        [Column("Aadhar_District")]
        [StringLength(20)]
        public string AadharDistrict { get; set; }
        [Column("Aadhar_State")]
        [StringLength(20)]
        public string AadharState { get; set; }
        [Column("Aadhar_Locality")]
        [StringLength(20)]
        public string AadharLocality { get; set; }
        [Column("Aadhar_HouseNo")]
        [StringLength(20)]
        public string AadharHouseNo { get; set; }
        [Column("Aadhar_Care_Of")]
        [StringLength(25)]
        public string AadharCareOf { get; set; }
        [Column("Aadhar_Street")]
        [StringLength(25)]
        public string AadharStreet { get; set; }
        [Column("Aadhar_Landmark")]
        [StringLength(50)]
        public string AadharLandmark { get; set; }
        [Column("Aadhar_Addressline1")]
        [StringLength(150)]
        public string AadharAddressline1 { get; set; }
        [Column("Aadhar_Addressline2")]
        [StringLength(150)]
        public string AadharAddressline2 { get; set; }
        [Column("Aadhar_Address")]
        [StringLength(250)]
        public string AadharAddress { get; set; }
        [Column("Voter_Name")]
        [StringLength(50)]
        public string VoterName { get; set; }
        [Column("Voter_DOB")]
        [StringLength(20)]
        public string VoterDob { get; set; }
        [Column("Voter_Gender")]
        [StringLength(10)]
        public string VoterGender { get; set; }
        [Column("Voter_Age")]
        [StringLength(10)]
        public string VoterAge { get; set; }
        [StringLength(20)]
        public string VoterNumber { get; set; }
        [Column("Voter_Relation")]
        [StringLength(20)]
        public string VoterRelation { get; set; }
        [Column("Voter_City")]
        [StringLength(25)]
        public string VoterCity { get; set; }
        [Column("Voter_Pin")]
        [StringLength(10)]
        public string VoterPin { get; set; }
        [Column("Voter_District")]
        [StringLength(20)]
        public string VoterDistrict { get; set; }
        [Column("Voter_State")]
        [StringLength(25)]
        public string VoterState { get; set; }
        [Column("Voter_Locality")]
        [StringLength(25)]
        public string VoterLocality { get; set; }
        [Column("Voter_HouseNo")]
        [StringLength(20)]
        public string VoterHouseNo { get; set; }
        [Column("Voter_Street")]
        [StringLength(25)]
        public string VoterStreet { get; set; }
        [Column("Voter_Landmark")]
        [StringLength(50)]
        public string VoterLandmark { get; set; }
        [Column("Voter_Addressline1")]
        [StringLength(150)]
        public string VoterAddressline1 { get; set; }
        [Column("Voter_Addressline2")]
        [StringLength(150)]
        public string VoterAddressline2 { get; set; }
        [Column("Voter_Address")]
        [StringLength(250)]
        public string VoterAddress { get; set; }
        [Column("Passport_Name")]
        [StringLength(50)]
        public string PassportName { get; set; }
        [Column("Passport_FatherName")]
        [StringLength(50)]
        public string PassportFatherName { get; set; }
        [Column("Passport_MotherName")]
        [StringLength(50)]
        public string PassportMotherName { get; set; }
        [Column("Passport_Spouse")]
        [StringLength(50)]
        public string PassportSpouse { get; set; }
        [StringLength(20)]
        public string PassportNumber { get; set; }
        [Column("Passport_OldPassportNumber")]
        [StringLength(20)]
        public string PassportOldPassportNumber { get; set; }
        [Column("Passport_Surname")]
        [StringLength(25)]
        public string PassportSurname { get; set; }
        [Column("Passport_Mrz1")]
        [StringLength(150)]
        public string PassportMrz1 { get; set; }
        [Column("Passport_Mrz2")]
        [StringLength(150)]
        public string PassportMrz2 { get; set; }
        [Column("Passport_DOB")]
        [StringLength(20)]
        public string PassportDob { get; set; }
        [Column("Passport_Gender")]
        [StringLength(10)]
        public string PassportGender { get; set; }
        [Column("Passport_DOE")]
        [StringLength(20)]
        public string PassportDoe { get; set; }
        [Column("Passport_DOI")]
        [StringLength(20)]
        public string PassportDoi { get; set; }
        [Column("Passport_OldDOI")]
        [StringLength(20)]
        public string PassportOldDoi { get; set; }
        [Column("Passport_CountryCode")]
        [StringLength(10)]
        public string PassportCountryCode { get; set; }
        [Column("Passport_PlaceOfBirth")]
        [StringLength(50)]
        public string PassportPlaceOfBirth { get; set; }
        [Column("Passport_PlaceOfIssue")]
        [StringLength(50)]
        public string PassportPlaceOfIssue { get; set; }
        [Column("Passport_Nationality")]
        [StringLength(25)]
        public string PassportNationality { get; set; }
        [Column("Passport_FileNumber")]
        [StringLength(50)]
        public string PassportFileNumber { get; set; }
        [Column("Passport_City")]
        [StringLength(25)]
        public string PassportCity { get; set; }
        [Column("Passport_Pin")]
        [StringLength(10)]
        public string PassportPin { get; set; }
        [Column("Passport_District")]
        [StringLength(25)]
        public string PassportDistrict { get; set; }
        [Column("Passport_State")]
        [StringLength(25)]
        public string PassportState { get; set; }
        [Column("Passport_Locality")]
        [StringLength(50)]
        public string PassportLocality { get; set; }
        [Column("Passport_HouseNo")]
        [StringLength(20)]
        public string PassportHouseNo { get; set; }
        [Column("Passport_Street")]
        [StringLength(50)]
        public string PassportStreet { get; set; }
        [Column("Passport_Landmark")]
        [StringLength(50)]
        public string PassportLandmark { get; set; }
        [Column("Passport_Addressline1")]
        [StringLength(150)]
        public string PassportAddressline1 { get; set; }
        [Column("Passport_Addressline2")]
        [StringLength(150)]
        public string PassportAddressline2 { get; set; }
        [Column("Passport_Address")]
        [StringLength(250)]
        public string PassportAddress { get; set; }
    }
}
