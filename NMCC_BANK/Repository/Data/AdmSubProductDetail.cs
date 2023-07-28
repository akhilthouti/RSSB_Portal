using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_SubProductDetails")]
    public partial class AdmSubProductDetail
    {
        [Key]
        [Column("SubProductID")]
        public long SubProductId { get; set; }
        [StringLength(50)]
        public string SubProductName { get; set; }
        [StringLength(50)]
        public string SubProductDescription { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
