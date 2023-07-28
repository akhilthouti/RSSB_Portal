using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_LivenessServiceLog")]
    public partial class AdmLivenessServiceLog
    {
        [Column("responseId")]
        public long? ResponseId { get; set; }
        [Column("response")]
        [StringLength(1)]
        public string Response { get; set; }
        [Column("transactionId")]
        public string TransactionId { get; set; }
        [Column("userid")]
        public long? Userid { get; set; }
        [Column("isSuccess")]
        [StringLength(100)]
        public string IsSuccess { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("predictionaccuracy")]
        public double? Predictionaccuracy { get; set; }
        [Column("prediction")]
        public float? Prediction { get; set; }
        [Column("success")]
        [StringLength(50)]
        public string Success { get; set; }
    }
}
