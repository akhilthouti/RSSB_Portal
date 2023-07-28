using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_PanVerificationDetails")]
    public partial class AdmPanVerificationDetail
    {
        [Key]
        [Column("panVerificationId")]
        public long PanVerificationId { get; set; }
        [Column("panNo")]
        [StringLength(50)]
        public string PanNo { get; set; }
        [Column("panStatus")]
        [StringLength(50)]
        public string PanStatus { get; set; }
        [Column("firstName")]
        [StringLength(225)]
        public string FirstName { get; set; }
        [Column("middleName")]
        [StringLength(225)]
        public string MiddleName { get; set; }
        [Column("lastName")]
        [StringLength(225)]
        public string LastName { get; set; }
        [Column("lastUpdatedDate", TypeName = "datetime")]
        public DateTime? LastUpdatedDate { get; set; }
        [Column("title")]
        [StringLength(50)]
        public string Title { get; set; }
        [Column("customerDetailId")]
        public long? CustomerDetailId { get; set; }
        [Column("createdBy")]
        public long? CreatedBy { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column("updatedBy")]
        public long? UpdatedBy { get; set; }
        [Column("updatedDate", TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        [Column("appType")]
        public bool? AppType { get; set; }
        [Column("custCBSId")]
        public long? CustCbsid { get; set; }
        [Column("NameprintedonPAN")]
        [StringLength(100)]
        public string NameprintedonPan { get; set; }
    }
}
