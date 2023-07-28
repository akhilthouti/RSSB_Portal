using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Organisation_Categories")]
    public partial class OrganisationCategory
    {
        [Key]
        public long OrgCatId { get; set; }
        [Column("OrganisationID")]
        public long? OrganisationId { get; set; }
        public long? CategaryId { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
