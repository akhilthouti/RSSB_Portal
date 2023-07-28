using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CustomerDocumentDetailsMapping")]
    public partial class AdmCustomerDocumentDetailsMapping
    {
        [Key]
        [Column("docDetailId")]
        public long DocDetailId { get; set; }
        public long? CustomerDetailId { get; set; }
        [Column("documentId")]
        public long DocumentId { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column("createdBy")]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
