using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_Cosmos_Sol")]
    public partial class AdmCosmosSol
    {
        [Key]
        [Column("cosmosBranch")]
        public long CosmosBranch { get; set; }
        [Column("solId")]
        [StringLength(50)]
        public string SolId { get; set; }
        [Column("sol_Desc")]
        [StringLength(100)]
        public string SolDesc { get; set; }
        public bool? IsActive { get; set; }
    }
}
