using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Tbl_EsignCustomerDetails")]
    public partial class TblEsignCustomerDetail
    {
        [Key]
        [Column("ECustDetailsId")]
        public long EcustDetailsId { get; set; }
        [Column("ECustFirstName")]
        [StringLength(50)]
        public string EcustFirstName { get; set; }
        [Column("ECustMiddleName")]
        [StringLength(50)]
        public string EcustMiddleName { get; set; }
        [Column("ECustLastName")]
        [StringLength(50)]
        public string EcustLastName { get; set; }
        [Column("ECustMobId")]
        [StringLength(20)]
        public string EcustMobId { get; set; }
        [Column("ECustMailid")]
        [StringLength(50)]
        public string EcustMailid { get; set; }
        [Column("EFirstCustFirstName")]
        [StringLength(50)]
        public string EfirstCustFirstName { get; set; }
        [Column("EFirstCustMiddleName")]
        [StringLength(50)]
        public string EfirstCustMiddleName { get; set; }
        [Column("EFirstCustLastName")]
        [StringLength(50)]
        public string EfirstCustLastName { get; set; }
        [Column("EFirstCustMobId")]
        [StringLength(20)]
        public string EfirstCustMobId { get; set; }
        [Column("EFirstMailid")]
        [StringLength(50)]
        public string EfirstMailid { get; set; }
        [Column("ESecCustFIrstName")]
        [StringLength(50)]
        public string EsecCustFirstName { get; set; }
        [Column("ESecCustMiddleName")]
        [StringLength(50)]
        public string EsecCustMiddleName { get; set; }
        [Column("ESecCustLastName")]
        [StringLength(50)]
        public string EsecCustLastName { get; set; }
        [Column("ESecCustMobId")]
        [StringLength(20)]
        public string EsecCustMobId { get; set; }
        [Column("ESecMailid")]
        [StringLength(50)]
        public string EsecMailid { get; set; }
        [Column("EThirdCustFisrtName")]
        [StringLength(50)]
        public string EthirdCustFisrtName { get; set; }
        [Column("EThirdCustMiddleName")]
        [StringLength(50)]
        public string EthirdCustMiddleName { get; set; }
        [Column("EThirdCustLastName")]
        [StringLength(50)]
        public string EthirdCustLastName { get; set; }
        [Column("EThirdCustMobId")]
        [StringLength(20)]
        public string EthirdCustMobId { get; set; }
        [Column("EThirdMailid")]
        [StringLength(50)]
        public string EthirdMailid { get; set; }
        [Column("EFourthCustFirstName")]
        [StringLength(50)]
        public string EfourthCustFirstName { get; set; }
        [Column("EFourthCustMiddleName")]
        [StringLength(50)]
        public string EfourthCustMiddleName { get; set; }
        [Column("EFourthCustLastName")]
        [StringLength(50)]
        public string EfourthCustLastName { get; set; }
        [Column("EFourthCustMobId")]
        [StringLength(20)]
        public string EfourthCustMobId { get; set; }
        [Column("EFourthMailid")]
        [StringLength(50)]
        public string EfourthMailid { get; set; }
        [Column("EFifthCustFirstName")]
        [StringLength(50)]
        public string EfifthCustFirstName { get; set; }
        [Column("EFifthCustMiddleName")]
        [StringLength(50)]
        public string EfifthCustMiddleName { get; set; }
        [Column("EFifthCustLastName")]
        [StringLength(50)]
        public string EfifthCustLastName { get; set; }
        [Column("EFifthCustMobId")]
        [StringLength(20)]
        public string EfifthCustMobId { get; set; }
        [Column("EFifthMailid")]
        [StringLength(50)]
        public string EfifthMailid { get; set; }
        [Column("ESixCustFirstName")]
        [StringLength(50)]
        public string EsixCustFirstName { get; set; }
        [Column("ESixCustMiddleName")]
        [StringLength(50)]
        public string EsixCustMiddleName { get; set; }
        [Column("ESixCustLastName")]
        [StringLength(50)]
        public string EsixCustLastName { get; set; }
        [Column("ESixCustMobId")]
        [StringLength(20)]
        public string EsixCustMobId { get; set; }
        [Column("ESixMailId")]
        [StringLength(50)]
        public string EsixMailId { get; set; }
        [Column("ESeventhCustFirstName")]
        [StringLength(50)]
        public string EseventhCustFirstName { get; set; }
        [Column("ESeventhCustMiddleName")]
        [StringLength(50)]
        public string EseventhCustMiddleName { get; set; }
        [Column("ESeventhCustLastName")]
        [StringLength(50)]
        public string EseventhCustLastName { get; set; }
        [Column("ESeventhCustMobId")]
        [StringLength(20)]
        public string EseventhCustMobId { get; set; }
        [Column("ESeventhMailid")]
        [StringLength(50)]
        public string EseventhMailid { get; set; }
        [Column("EEighthCustFirstName")]
        [StringLength(50)]
        public string EeighthCustFirstName { get; set; }
        [Column("EEighthCustMiddleName")]
        [StringLength(50)]
        public string EeighthCustMiddleName { get; set; }
        [Column("EEighthCustLastName")]
        [StringLength(50)]
        public string EeighthCustLastName { get; set; }
        [Column("EEighthCustMobId")]
        [StringLength(20)]
        public string EeighthCustMobId { get; set; }
        [Column("EEighthMailid")]
        [StringLength(50)]
        public string EeighthMailid { get; set; }
        [Column("ENinthCustFirstName")]
        [StringLength(50)]
        public string EninthCustFirstName { get; set; }
        [Column("ENinthCustMiddleName")]
        [StringLength(50)]
        public string EninthCustMiddleName { get; set; }
        [Column("ENinthCustLastName")]
        [StringLength(50)]
        public string EninthCustLastName { get; set; }
        [Column("ENinthCustMobId")]
        [StringLength(20)]
        public string EninthCustMobId { get; set; }
        [Column("ENinthMailid")]
        [StringLength(50)]
        public string EninthMailid { get; set; }
        [Column("EDocId")]
        public long? EdocId { get; set; }
        [Column("EDocType")]
        [StringLength(50)]
        public string EdocType { get; set; }
        [Column("EDocImage")]
        public byte[] EdocImage { get; set; }
        [Column("EMailSendFlag")]
        [StringLength(20)]
        public string EmailSendFlag { get; set; }
        [Column("ECustCount")]
        public int? EcustCount { get; set; }
        [Column("EIsMailSendFlag")]
        [StringLength(10)]
        public string EisMailSendFlag { get; set; }
        [Column("EIsSmsSendFlag")]
        [StringLength(10)]
        public string EisSmsSendFlag { get; set; }
        public byte[] SignDocument { get; set; }
        [Column("EDocFileDummmy")]
        public string EdocFileDummmy { get; set; }
        [StringLength(200)]
        public string TransactionId { get; set; }
        public string SignDocDummy { get; set; }
        public long? OrganisationId { get; set; }
    }
}
