using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("IndoFin_OtpMsgFormat")]
    public partial class IndoFinOtpMsgFormat
    {
        [Column("msgId")]
        public long MsgId { get; set; }
        [Column("startMsg")]
        [StringLength(1000)]
        public string StartMsg { get; set; }
        [Column("endMsg")]
        [StringLength(1000)]
        public string EndMsg { get; set; }
        [Column("otpFor")]
        [StringLength(50)]
        public string OtpFor { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column("createdBy")]
        public long? CreatedBy { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
