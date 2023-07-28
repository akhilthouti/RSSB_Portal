using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_AdhaarVaultData")]
    public partial class AdmAdhaarVaultDatum
    {
        [Key]
        public int AdhaarVaultId { get; set; }
        public string EncryptedAdhaarNo { get; set; }
        [StringLength(20)]
        public string ReferenceKey { get; set; }
        [StringLength(250)]
        public string Adhaarhash { get; set; }
        [StringLength(25)]
        public string TransactionDate { get; set; }
        [Column("Key_LabelName")]
        [StringLength(50)]
        public string KeyLabelName { get; set; }
        [Column("Cust_Id")]
        [StringLength(50)]
        public string CustId { get; set; }
    }
}
