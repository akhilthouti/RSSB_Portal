using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CustomerDocuments")]
    public partial class AdmCustomerDocument
    {
        public AdmCustomerDocument()
        {
            AdmCustomerManagementAdmCustomerDocuments = new HashSet<AdmCustomerManagementAdmCustomerDocument>();
        }

        [Key]
        [Column("customerDocumentId")]
        public long CustomerDocumentId { get; set; }
        [Column("documentHistory")]
        public byte[] DocumentHistory { get; set; }
        [Required]
        [Column("documentName")]
        [StringLength(500)]
        public string DocumentName { get; set; }
        [Column("documentTypeId")]
        public int DocumentTypeId { get; set; }
        [Column("documentCategoryId")]
        public long DocumentCategoryId { get; set; }
        [Required]
        [Column("isActive")]
        public bool? IsActive { get; set; }
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
        [Column("rowGuid")]
        public Guid RowGuid { get; set; }
        [Column("isActiveSecondDoc")]
        public bool? IsActiveSecondDoc { get; set; }
        public byte[] NewDocumentHistory { get; set; }
        [StringLength(500)]
        public string NewDocumentName { get; set; }
        [StringLength(500)]
        public string ThirdDocumentName { get; set; }
        [Column("updatedDate2", TypeName = "datetime")]
        public DateTime? UpdatedDate2 { get; set; }
        [Column("updatedBy2")]
        public long? UpdatedBy2 { get; set; }
        public byte[] ThirdDocumentHistory { get; set; }
        [Column("isActiveThirdDoc")]
        public bool? IsActiveThirdDoc { get; set; }
        [Column("isdocbackup")]
        public bool? Isdocbackup { get; set; }
        [Column("applicationNumber")]
        [StringLength(100)]
        public string ApplicationNumber { get; set; }
        [Column("isDMSdoc")]
        public bool? IsDmsdoc { get; set; }
        [Column("documentCategory")]
        [StringLength(100)]
        public string DocumentCategory { get; set; }

        [InverseProperty(nameof(AdmCustomerManagementAdmCustomerDocument.CustomerDocument))]
        public virtual ICollection<AdmCustomerManagementAdmCustomerDocument> AdmCustomerManagementAdmCustomerDocuments { get; set; }
    }
}
