using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_DigiAadharData")]
    public partial class AdmDigiAadharDatum
    {
        [Key]
        public int Id { get; set; }
        public long? CustomerId { get; set; }
        [Column("name")]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("photo")]
        public byte[] Photo { get; set; }
        [Column("DOB")]
        [StringLength(50)]
        public string Dob { get; set; }
        [Column("gender")]
        [StringLength(50)]
        public string Gender { get; set; }
        [StringLength(50)]
        public string Vtc { get; set; }
        [StringLength(50)]
        public string Street { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string Pc { get; set; }
        [StringLength(50)]
        public string Locality { get; set; }
        [StringLength(50)]
        public string House { get; set; }
        [Column("district")]
        [StringLength(50)]
        public string District { get; set; }
        [Column("country")]
        [StringLength(50)]
        public string Country { get; set; }
        [Column("uid")]
        [StringLength(50)]
        public string Uid { get; set; }
        [Column("firstname")]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Column("middlename")]
        [StringLength(50)]
        public string Middlename { get; set; }
        [Column("lastname")]
        [StringLength(50)]
        public string Lastname { get; set; }
        [StringLength(250)]
        public string Address { get; set; }
    }
}
