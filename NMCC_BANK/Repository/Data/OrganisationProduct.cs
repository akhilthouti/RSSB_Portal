using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Organisation_Product")]
    public partial class OrganisationProduct
    {
        [Key]
        public long OrgProdId { get; set; }
        [Column("OrganisationID")]
        public long? OrganisationId { get; set; }
        public long? ProductId { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
