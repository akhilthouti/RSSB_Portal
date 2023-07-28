using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INDO_FIN_NET.Models.CurrentModels
{
    public class Current_DocumentDetails
    {
        [Key]
        public long DocumentId { get; set; }
        [Column("customerDocumentId")]
        public long CustomerDocumentId { get; set; }
        [Column("documentHistory")]
        public byte[] DocumentHistory { get; set; }
        [Column("documentName")]
        [StringLength(500)]
        public string DocumentName { get; set; }
        [Column("documentType")]
        public string DocumentType { get; set; }
        [Column("documentCategoryCode")]
        [StringLength(50)]
        public string DocumentCategoryCode { get; set; }
        [Column("docMainCategory")]
        [StringLength(50)]
        public string DocMainCategory { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column("createdBy")]
        [StringLength(50)]
        public string CreatedBy { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("updatedBy")]
        public long? UpdatedBy { get; set; }
        [Column("updatedDate", TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column("deletedBy")]
        public long? DeletedBy { get; set; }
        [Column("deletedDate", TypeName = "datetime")]
        public DateTime? DeletedDate { get; set; }
        [Column("rowGuid")]
        [StringLength(50)]
        public string RowGuid { get; set; }
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

        [Column(TypeName = "datetime")]
        public DateTime? DocumentIdDate { get; set; }
        [Column("Latitude_Longitude")]
        [StringLength(200)]
        public string LatitudeLongitude { get; set; }
        [Column("documentCategory")]
        [StringLength(200)]
        public string DocumentCategory { get; set; }
        [StringLength(200)]
        public string Source { get; set; }
        public byte[] Faceext { get; set; }
        public byte[] Signature { get; set; }
        [StringLength(200)]
        public string Prediction { get; set; }
    }
}


