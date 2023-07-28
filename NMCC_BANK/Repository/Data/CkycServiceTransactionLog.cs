using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("CkycServiceTransactionLog")]
    public partial class CkycServiceTransactionLog
    {
        [Key]
        public long CkycTransactionId { get; set; }
        [StringLength(500)]
        public string RequestId { get; set; }
        [Column("ID_Type")]
        [StringLength(20)]
        public string IdType { get; set; }
        [Column("ID_number")]
        [StringLength(100)]
        public string IdNumber { get; set; }
        [StringLength(50)]
        public string TransactionName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? TransactionDate { get; set; }
        [StringLength(20)]
        public string TransactionStatus { get; set; }
        [StringLength(500)]
        public string Error { get; set; }
        [StringLength(500)]
        public string Exception { get; set; }
        public long? CreatedBy { get; set; }
        public string Response { get; set; }
    }
}
