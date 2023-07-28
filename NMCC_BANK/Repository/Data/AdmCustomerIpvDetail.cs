using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CustomerIpvDetails")]
    public partial class AdmCustomerIpvDetail
    {
        [Key]
        [Column("IPV_ID")]
        public long IpvId { get; set; }
        public long? PersonalDetailId { get; set; }
        [StringLength(100)]
        public string ClientName { get; set; }
        [StringLength(10)]
        public string PanNo { get; set; }
        [Column("Ipv_Video")]
        public byte[] IpvVideo { get; set; }
        [StringLength(50)]
        public string VideoFileName { get; set; }
        public long? VideoFileSize { get; set; }
        [StringLength(200)]
        public string VideoFilePath { get; set; }
        public Guid? RowGuid { get; set; }
        [Column("isActiveIPV")]
        public bool? IsActiveIpv { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
