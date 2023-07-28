using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_ThirdPartyVendor")]
    public partial class AdmThirdPartyVendor
    {
        [Key]
        [Column("VenderID")]
        public long VenderId { get; set; }
        [StringLength(200)]
        public string VenderName { get; set; }
        [StringLength(200)]
        public string OrganizationName { get; set; }
        [Column("VenderUserID")]
        [StringLength(200)]
        public string VenderUserId { get; set; }
        [StringLength(200)]
        public string Password { get; set; }
        [StringLength(200)]
        public string EncryptionKey { get; set; }
        public long? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
