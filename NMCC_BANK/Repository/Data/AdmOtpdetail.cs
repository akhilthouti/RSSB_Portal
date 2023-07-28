using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("ADM_OTPDETAILS")]
    public partial class AdmOtpdetail
    {
        [Column("OTP_ID")]
        public long OtpId { get; set; }
        [Column("OTP_DESC")]
        public long? OtpDesc { get; set; }
        [Column("MOBILE_NO")]
        public long? MobileNo { get; set; }
        [Column("GEN_TIME", TypeName = "datetime")]
        public DateTime? GenTime { get; set; }
        [Column("IS_VALIDATE")]
        public bool? IsValidate { get; set; }
        [Column("VAL_TIME", TypeName = "datetime")]
        public DateTime? ValTime { get; set; }
        [Column("ISEXPIRE")]
        public bool? Isexpire { get; set; }
        [Column("OTPCHK_TIME", TypeName = "datetime")]
        public DateTime? OtpchkTime { get; set; }
        [Column("OTP_CHANNEL")]
        [StringLength(200)]
        public string OtpChannel { get; set; }
    }
}
