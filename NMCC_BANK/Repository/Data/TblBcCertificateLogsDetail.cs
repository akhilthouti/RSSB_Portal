using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Tbl_BcCertificateLogsDetails")]
    public partial class TblBcCertificateLogsDetail
    {
        [Key]
        public long BcCertificateLogid { get; set; }
        [StringLength(100)]
        public string BcCertSerialNo { get; set; }
        [StringLength(200)]
        public string BcCertOrgName { get; set; }
        [StringLength(200)]
        public string BcCertOrgBcName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BcCertOrgCreatedDate { get; set; }
        public long? BcCertCreatedBy { get; set; }
        public long? BcCustId { get; set; }
    }
}
