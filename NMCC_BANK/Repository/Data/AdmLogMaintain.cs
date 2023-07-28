using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_LogMaintain")]
    public partial class AdmLogMaintain
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        [StringLength(50)]
        public string Loginway { get; set; }
        [StringLength(50)]
        public string BankName { get; set; }
        [StringLength(50)]
        public string ServiceName { get; set; }
        [StringLength(30)]
        public string ServiceProvider { get; set; }
        [StringLength(50)]
        public string CategoryName { get; set; }
        [StringLength(50)]
        public string ProductName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LoginTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogoutTime { get; set; }
    }
}
