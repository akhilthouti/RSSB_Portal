using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_Categories")]
    public partial class AdmCategory
    {
        [Key]
        public long CategoryId { get; set; }
        [StringLength(200)]
        public string CategoryName { get; set; }
        [StringLength(500)]
        public string CategoryDescription { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
