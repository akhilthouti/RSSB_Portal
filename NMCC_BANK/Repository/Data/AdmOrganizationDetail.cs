using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_OrganizationDetails")]
    public partial class AdmOrganizationDetail
    {
        [Key]
        [Column("OrganizationID")]
        public long OrganizationId { get; set; }
        [StringLength(80)]
        public string OrganizationName { get; set; }
        [StringLength(50)]
        public string OrganizationReferenceNumber { get; set; }
        public byte[] OrganizationLogo { get; set; }
        [StringLength(50)]
        public string OrganizationDescription { get; set; }
        [StringLength(80)]
        public string ContactPersonName { get; set; }
        [StringLength(10)]
        public string ContactPersonNumber { get; set; }
        [Column("ContactPersonEmail_ID")]
        [StringLength(80)]
        public string ContactPersonEmailId { get; set; }
        [Column("HO_Address")]
        [StringLength(50)]
        public string HoAddress { get; set; }
        [Column("Fax_No")]
        [StringLength(15)]
        public string FaxNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedDate { get; set; }
        [StringLength(50)]
        public string OrgCaCertSerialNo { get; set; }
        public string OrgCaCertPrivateKey { get; set; }
    }
}
