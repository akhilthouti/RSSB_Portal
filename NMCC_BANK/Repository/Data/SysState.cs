using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("sys_States")]
    public partial class SysState
    {
        public SysState()
        {
            AdmCustomerManagements = new HashSet<AdmCustomerManagement>();
        }

        [Key]
        [Column("stateId")]
        public long StateId { get; set; }
        [Required]
        [Column("stateCode")]
        [StringLength(15)]
        public string StateCode { get; set; }
        [StringLength(15)]
        public string CkycstateCode { get; set; }
        [Required]
        [Column("state")]
        [StringLength(255)]
        public string State { get; set; }
        [Column("countryId")]
        public int CountryId { get; set; }
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
        [Column("CBSStateId")]
        public long CbsstateId { get; set; }
        [Column("statename")]
        [StringLength(30)]
        public string Statename { get; set; }
        [Column("CBSstatecode")]
        [StringLength(10)]
        public string Cbsstatecode { get; set; }
        [Column("APYStateCode")]
        [StringLength(50)]
        public string ApystateCode { get; set; }

        [ForeignKey(nameof(CountryId))]
        [InverseProperty(nameof(SysCountry.SysStates))]
        public virtual SysCountry Country { get; set; }
        [InverseProperty(nameof(AdmCustomerManagement.State))]
        public virtual ICollection<AdmCustomerManagement> AdmCustomerManagements { get; set; }
    }
}
