using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Tbl_CustCertificateLogsDetails")]
    public partial class TblCustCertificateLogsDetail
    {
        [Key]
        public long OrgCustLogId { get; set; }
        [StringLength(100)]
        public string OrgCustCertSerialNo { get; set; }
        [StringLength(100)]
        public string OrgCustCertOrgName { get; set; }
        [StringLength(100)]
        public string OrgCustCertName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OrgCustCertCreatedDate { get; set; }
        public long? OrgCustCertCreatedBy { get; set; }
        public long? OrgCustCertCustId { get; set; }
    }
}
