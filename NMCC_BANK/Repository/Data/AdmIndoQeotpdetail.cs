using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_Indo_QEOTPDetails")]
    public partial class AdmIndoQeotpdetail
    {
        [Key]
        public int UniqueId { get; set; }
        [Column("MobileOTP")]
        [StringLength(10)]
        public string MobileOtp { get; set; }
        [Column("IsMobileOTPSend")]
        public bool? IsMobileOtpsend { get; set; }
        [Column("MobileOTPSendTime", TypeName = "datetime")]
        public DateTime? MobileOtpsendTime { get; set; }
        public bool? IsMobileVerfiy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? MobileVerfiyTime { get; set; }
    }
}
