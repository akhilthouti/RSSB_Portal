using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_Roletbl")]
    public partial class AdmRoletbl
    {
        [Key]
        public long RoleId { get; set; }
        [StringLength(50)]
        public string Roletype { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        public long? DeptId { get; set; }
    }
}
