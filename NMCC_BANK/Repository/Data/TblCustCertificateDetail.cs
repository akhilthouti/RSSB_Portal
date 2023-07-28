using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Tbl_CustCertificateDetails")]
    public partial class TblCustCertificateDetail
    {
        [Key]
        public long OrgCustCertId { get; set; }
        [StringLength(100)]
        public string OrgCustOrgName { get; set; }
        [StringLength(100)]
        public string OrgCustCertName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? OrgCustCertCreatedDate { get; set; }
        [StringLength(100)]
        public string OrgCustCertSerialNo { get; set; }
        public string OrgCustCertPrivateKey { get; set; }
        public long? OrgCustCertCreatedBy { get; set; }
        public long? OrgCustDetailsId { get; set; }
    }
}
