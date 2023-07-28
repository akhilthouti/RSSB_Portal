using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("sys_DocumentTypes")]
    public partial class SysDocumentType
    {
        [Key]
        [Column("documentTypeId")]
        public int DocumentTypeId { get; set; }
        [Required]
        [Column("documentType")]
        [StringLength(50)]
        public string DocumentType { get; set; }
        [Required]
        [Column("documentMIMEType")]
        [StringLength(255)]
        public string DocumentMimetype { get; set; }
        [Required]
        [Column("documentImageURL")]
        public string DocumentImageUrl { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("createdBy")]
        public long CreatedBy { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [Column("updatedBy")]
        public long? UpdatedBy { get; set; }
        [Column("updatedDate", TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column("deletedBy")]
        public long? DeletedBy { get; set; }
        [Column("deletedDate", TypeName = "datetime")]
        public DateTime? DeletedDate { get; set; }
    }
}
