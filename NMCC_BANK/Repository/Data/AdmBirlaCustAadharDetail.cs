using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_BirlaCustAadharDetails")]
    public partial class AdmBirlaCustAadharDetail
    {
        public long? AadharId { get; set; }
        [StringLength(50)]
        public string AadharNumber { get; set; }
        [Column("XMLReferenceId")]
        [StringLength(50)]
        public string XmlreferenceId { get; set; }
        [StringLength(50)]
        public string RefrenceNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string VerificationType { get; set; }
        [Required]
        [StringLength(50)]
        public string AadharName { get; set; }
        [StringLength(50)]
        public string AadharGender { get; set; }
        [Column("AadharDOB")]
        [StringLength(50)]
        public string AadharDob { get; set; }
        [Required]
        [StringLength(100)]
        public string AadharMobile { get; set; }
        [StringLength(100)]
        public string AadharEmail { get; set; }
        public string AadharPhoto { get; set; }
        public string AadharAddress { get; set; }
        [StringLength(50)]
        public string Vtc { get; set; }
        [StringLength(50)]
        public string Subdist { get; set; }
        [StringLength(50)]
        public string Street { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [Column("Post_Office")]
        [StringLength(50)]
        public string PostOffice { get; set; }
        [Column("Pin_Code")]
        [StringLength(50)]
        public string PinCode { get; set; }
        [StringLength(50)]
        public string Locality { get; set; }
        [StringLength(50)]
        public string House { get; set; }
        [StringLength(50)]
        public string District { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        public long? CreatedBy { get; set; }
        [StringLength(50)]
        public string CretedDate { get; set; }
        public long? CustomerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column("TFForVerif")]
        public bool? TfforVerif { get; set; }
        [Column("OrganizationID")]
        public long? OrganizationId { get; set; }
    }
}
