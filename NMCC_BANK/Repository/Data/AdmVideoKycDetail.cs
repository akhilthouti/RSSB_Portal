using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("Adm_VideoKYC_Details")]
    public partial class AdmVideoKycDetail
    {
        [Column("IPV_ID")]
        public long IpvId { get; set; }
        public long? CustomerDetailId { get; set; }
        [Column("IPV_Video")]
        public byte[] IpvVideo { get; set; }
        [StringLength(50)]
        public string VideoFileName { get; set; }
        public long? VideoFileSize { get; set; }
        [StringLength(100)]
        public string VideoFilePath { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [StringLength(100)]
        public string RefNumber { get; set; }
    }
}
