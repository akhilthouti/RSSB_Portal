using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_OTPsave")]
    public partial class AdmOtpsave
    {
        [Key]
        public long Id { get; set; }
        public long? UserId { get; set; }
        [Column("otp")]
        [StringLength(50)]
        public string Otp { get; set; }
    }
}
