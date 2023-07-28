using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_AlphaServices")]
    public partial class AdmAlphaService
    {
        [Key]
        public long AlphaServiceId { get; set; }
        [StringLength(200)]
        public string AlphaServiceName { get; set; }
        [StringLength(500)]
        public string AlphaServiceDescription { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
