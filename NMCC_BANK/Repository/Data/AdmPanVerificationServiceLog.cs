using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_PanVerificationServiceLog")]
    public partial class AdmPanVerificationServiceLog
    {
        [Key]
        [Column("responseId")]
        public long ResponseId { get; set; }
        [Column("response")]
        public string Response { get; set; }
        [Column("transactionId")]
        [StringLength(100)]
        public string TransactionId { get; set; }
        [Column("userId")]
        public long? UserId { get; set; }
        [Column("isSuccess")]
        [StringLength(50)]
        public string IsSuccess { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("appType")]
        public bool? AppType { get; set; }
    }
}
