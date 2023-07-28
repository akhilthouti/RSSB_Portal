using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_Departmenttbl")]
    public partial class AdmDepartmenttbl
    {
        public long DeptId { get; set; }
        [StringLength(50)]
        public string DeptName { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        public long? BranchId { get; set; }
    }
}
