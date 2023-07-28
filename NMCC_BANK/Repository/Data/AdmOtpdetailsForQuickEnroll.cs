using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_OTPDetailsForQuickEnroll")]
    public partial class AdmOtpdetailsForQuickEnroll
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string EmailId { get; set; }
        [Column("isEmailOTPSend")]
        public bool? IsEmailOtpsend { get; set; }
        [Column("EmailOTP")]
        [StringLength(20)]
        public string EmailOtp { get; set; }
        [Column("EmailSendOTPTime", TypeName = "datetime")]
        public DateTime? EmailSendOtptime { get; set; }
        [Column("isEmailOTPVerify")]
        public bool? IsEmailOtpverify { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? EmailVerifyDate { get; set; }
        [Column("IsMobileOTPSend")]
        public bool? IsMobileOtpsend { get; set; }
        [Column("MOBileOTP")]
        [StringLength(10)]
        public string MobileOtp { get; set; }
        [Column("MobileOTPTime", TypeName = "datetime")]
        public DateTime? MobileOtptime { get; set; }
        [Column("IsMobileOTPVerify")]
        public bool? IsMobileOtpverify { get; set; }
        [Column("MobileOTPVerifyDate", TypeName = "datetime")]
        public DateTime? MobileOtpverifyDate { get; set; }
    }
}
