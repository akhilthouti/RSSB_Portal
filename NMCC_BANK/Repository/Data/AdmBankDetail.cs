using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_BankDetails")]
    public partial class AdmBankDetail
    {
        [Key]
        [Column("BankDetailID")]
        public long BankDetailId { get; set; }
        [StringLength(50)]
        public string BankName { get; set; }
        [Column("Bank_IFSCNumber")]
        [StringLength(50)]
        public string BankIfscnumber { get; set; }
        [Column("Bank_MICR")]
        [StringLength(50)]
        public string BankMicr { get; set; }
        [Column("Bank_Branch")]
        [StringLength(50)]
        public string BankBranch { get; set; }
        [Column("Bank_HO_Address")]
        [StringLength(50)]
        public string BankHoAddress { get; set; }
        [Column("Bank_ContactNumber")]
        [StringLength(50)]
        public string BankContactNumber { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string District { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
