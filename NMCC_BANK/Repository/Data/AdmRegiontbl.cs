using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_Regiontbl")]
    public partial class AdmRegiontbl
    {
        public long RegionId { get; set; }
        [StringLength(30)]
        public string RegionName { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
