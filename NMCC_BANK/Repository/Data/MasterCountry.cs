using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("MASTER_COUNTRY")]
    public partial class MasterCountry
    {
        [Key]
        [Column("Country_id")]
        public int CountryId { get; set; }
        [Column("Country_Code")]
        [StringLength(15)]
        public string CountryCode { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
    }
}
