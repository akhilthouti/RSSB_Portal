using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Tbl_OrgCaCertificateLogDetails")]
    public partial class TblOrgCaCertificateLogDetail
    {
        [Key]
        public long OrgCaCertDetailsId { get; set; }
        [StringLength(200)]
        public string OrgCaCertSerialNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OrgCaCertCreatedDate { get; set; }
        public long? OrgCaCertCreatedBy { get; set; }
        [StringLength(50)]
        public string OrgCaCertValidFrom { get; set; }
        [StringLength(50)]
        public string OrgCaCertValidTo { get; set; }
        [StringLength(200)]
        public string OrgCaCertOrgName { get; set; }
        [StringLength(50)]
        public string OrgCaCertEmailId { get; set; }
        public long? OrganisationId { get; set; }
    }
}
