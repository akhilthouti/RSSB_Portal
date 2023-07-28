using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CKYC_Search_Logs")]
    public partial class AdmCkycSearchLog
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string TransactionId { get; set; }
        [Column("IDNumber")]
        [StringLength(100)]
        public string Idnumber { get; set; }
        [Column("IDType")]
        [StringLength(5)]
        public string Idtype { get; set; }
        [Column("CKYC_No")]
        [StringLength(25)]
        public string CkycNo { get; set; }
        [StringLength(500)]
        public string ErrorCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
