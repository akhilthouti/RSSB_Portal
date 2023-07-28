using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CustomerDocumentCategories")]
    public partial class AdmCustomerDocumentCategory
    {
        [Key]
        [Column("documentCategoryId")]
        public long DocumentCategoryId { get; set; }
        [Column("POI_Code")]
        [StringLength(3)]
        public string PoiCode { get; set; }
        [Column("POA_Code")]
        [StringLength(3)]
        public string PoaCode { get; set; }
        [Column("Ckyc_CategoryId")]
        [StringLength(50)]
        public string CkycCategoryId { get; set; }
        [Required]
        [Column("documentCategory")]
        [StringLength(100)]
        public string DocumentCategory { get; set; }
        [Column("documentCategoryDescription")]
        [StringLength(255)]
        public string DocumentCategoryDescription { get; set; }
        [Column("POA_Flag")]
        [StringLength(1)]
        public string PoaFlag { get; set; }
        [Column("POI_Flag")]
        [StringLength(1)]
        public string PoiFlag { get; set; }
        [Column("organisationDetailId")]
        public long? OrganisationDetailId { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
        [Column("createdBy")]
        public long? CreatedBy { get; set; }
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
        [Column("isDisplay")]
        public bool? IsDisplay { get; set; }
        [Column("Nominee_Code")]
        [StringLength(3)]
        public string NomineeCode { get; set; }
        [Column("Nominee_Flag")]
        [StringLength(1)]
        public string NomineeFlag { get; set; }
        [Column("NPS_POA_Flag")]
        [StringLength(1)]
        public string NpsPoaFlag { get; set; }
        [Column("NPS_POI_Flag")]
        [StringLength(1)]
        public string NpsPoiFlag { get; set; }
        [Column("NPS_POI_Code")]
        [StringLength(3)]
        public string NpsPoiCode { get; set; }
        [Column("NPS_POA_Code")]
        [StringLength(3)]
        public string NpsPoaCode { get; set; }
    }
}
