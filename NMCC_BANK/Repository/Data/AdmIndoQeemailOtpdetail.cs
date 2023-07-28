using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_Indo_QEEmailOTPDetails")]
    public partial class AdmIndoQeemailOtpdetail
    {
        [Key]
        public int UniqueId { get; set; }
        [Column("EmailOTP")]
        [StringLength(10)]
        public string EmailOtp { get; set; }
        [Column("IsEmailOTPSend")]
        public bool? IsEmailOtpsend { get; set; }
        [Column("EmailOTPSendTime", TypeName = "datetime")]
        public DateTime? EmailOtpsendTime { get; set; }
        public bool? IsEmailVerfiy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EmailVerfiyTime { get; set; }
    }
}
