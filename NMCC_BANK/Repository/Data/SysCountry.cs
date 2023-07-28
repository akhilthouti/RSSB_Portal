using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("sys_Countries")]
    public partial class SysCountry
    {
        public SysCountry()
        {
            SysStates = new HashSet<SysState>();
        }

        [Key]
        [Column("countryId")]
        public int CountryId { get; set; }
        [Required]
        [Column("countryCode")]
        [StringLength(15)]
        public string CountryCode { get; set; }
        [StringLength(15)]
        public string CkyccountryCode { get; set; }
        [Required]
        [Column("country")]
        [StringLength(255)]
        public string Country { get; set; }
        [Column("continentId")]
        public int ContinentId { get; set; }
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
        [Column("CBSCountryId")]
        public long CbscountryId { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [Column("code")]
        [StringLength(10)]
        public string Code { get; set; }
        [Column("APYCode")]
        [StringLength(50)]
        public string Apycode { get; set; }

        [InverseProperty(nameof(SysState.Country))]
        public virtual ICollection<SysState> SysStates { get; set; }
    }
}
