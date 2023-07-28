using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_Branchtbl")]
    public partial class AdmBranchtbl
    {
        public long BranchId { get; set; }
        [StringLength(50)]
        public string BranchName { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        public long? RegionId { get; set; }
    }
}
