using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("admCustomerManagement_admCustomerDocuments")]
    public partial class AdmCustomerManagementAdmCustomerDocument
    {
        [Key]
        [Column("admCustomerManagementadmCustomerDocumentsId")]
        public long AdmCustomerManagementadmCustomerDocumentsId { get; set; }
        [Column("customerDetailId")]
        public long? CustomerDetailId { get; set; }
        [Column("customerDocumentId")]
        public long? CustomerDocumentId { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        public long? Documentsfilter { get; set; }
        [Column("isApproved")]
        public bool? IsApproved { get; set; }
        [StringLength(40)]
        public string ApprovedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedDate { get; set; }
        [Column("applicationNumber")]
        [StringLength(100)]
        public string ApplicationNumber { get; set; }

        [ForeignKey(nameof(CustomerDocumentId))]
        [InverseProperty(nameof(AdmCustomerDocument.AdmCustomerManagementAdmCustomerDocuments))]
        public virtual AdmCustomerDocument CustomerDocument { get; set; }
    }
}
