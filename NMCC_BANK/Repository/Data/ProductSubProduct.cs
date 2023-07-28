using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Product_SubProduct")]
    public partial class ProductSubProduct
    {
        [Key]
        public long ProdSubProdId { get; set; }
        public long? ProductId { get; set; }
        public long? SubProductId { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
    }
}
