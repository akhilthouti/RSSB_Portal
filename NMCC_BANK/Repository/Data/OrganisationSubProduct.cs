using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Organisation_SubProduct")]
    public partial class OrganisationSubProduct
    {
        [Key]
        public long OrgSubProdId { get; set; }
        [Column("OrganisationID")]
        public long? OrganisationId { get; set; }
        public long? SubProductId { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
