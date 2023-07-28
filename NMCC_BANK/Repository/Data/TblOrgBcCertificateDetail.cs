using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Tbl_OrgBcCertificateDetails")]
    public partial class TblOrgBcCertificateDetail
    {
        [Key]
        public long BcCertid { get; set; }
        [StringLength(100)]
        public string BcCertOrgName { get; set; }
        [StringLength(100)]
        public string BcCertName { get; set; }
        [StringLength(100)]
        public string BcCertSerialNo { get; set; }
        public string BcCertPrivateKey { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? BcCertCreationDate { get; set; }
        public long? BcCertCreatedBy { get; set; }
        public long? BcCustomerId { get; set; }
    }
}
