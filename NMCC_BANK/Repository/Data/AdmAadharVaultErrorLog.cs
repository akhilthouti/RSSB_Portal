using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_AadharVault_ErrorLog")]
    public partial class AdmAadharVaultErrorLog
    {
        [Key]
        public int Logid { get; set; }
        [StringLength(500)]
        public string EncryptedAdhaarNo { get; set; }
        public string Exception { get; set; }
        [StringLength(20)]
        public string ReferenceKey { get; set; }
        [StringLength(75)]
        public string UidToken { get; set; }
        public string DemoAuth { get; set; }
        [StringLength(250)]
        public string Adhaarhash { get; set; }
        [StringLength(25)]
        public string TransactionDate { get; set; }
        [Column("IV")]
        [StringLength(10)]
        public string Iv { get; set; }
        [Column("Key_LabelName")]
        [StringLength(50)]
        public string KeyLabelName { get; set; }
        [Column("Cust_Id")]
        [StringLength(50)]
        public string CustId { get; set; }
        [StringLength(10)]
        public string KeyVersion { get; set; }
    }
}
