using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_InsertCustomerDetails")]
    public partial class AdmInsertCustomerDetail
    {
        [Key]
        [Column("Cust_Id")]
        public long CustId { get; set; }
        public long? EncryptedAdhaarNo { get; set; }
        public int? ReferenceKey { get; set; }
        [StringLength(50)]
        public string Adhaarhash { get; set; }
        [Column(TypeName = "date")]
        public DateTime? TransactionDate { get; set; }
        [Column("Key_LabelName")]
        [StringLength(50)]
        public string KeyLabelName { get; set; }
    }
}
