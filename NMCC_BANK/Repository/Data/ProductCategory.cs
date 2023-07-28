using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Product_Category")]
    public partial class ProductCategory
    {
        [Key]
        public long CatProdId { get; set; }
        public long? ProductId { get; set; }
        public long? CategoryId { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
