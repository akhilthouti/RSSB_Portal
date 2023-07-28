using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("TblDesignation")]
    public partial class TblDesignation
    {
        [Key]
        public int DesignationId { get; set; }
        [StringLength(100)]
        public string DesignationDesc { get; set; }
    }
}
