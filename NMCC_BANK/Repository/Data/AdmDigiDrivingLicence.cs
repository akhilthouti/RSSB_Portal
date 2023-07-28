using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_DigiDrivingLicence")]
    public partial class AdmDigiDrivingLicence
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }
        public long? CustomerId { get; set; }
        [Column("name")]
        [StringLength(150)]
        public string Name { get; set; }
        [Column("firstname")]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Column("swd")]
        [StringLength(50)]
        public string Swd { get; set; }
        [Column("lastname")]
        [StringLength(50)]
        public string Lastname { get; set; }
        [Column("dob")]
        [StringLength(50)]
        public string Dob { get; set; }
        [Column("gender")]
        [StringLength(50)]
        public string Gender { get; set; }
        [Column("country")]
        [StringLength(50)]
        public string Country { get; set; }
        [Column("ORGname")]
        [StringLength(50)]
        public string Orgname { get; set; }
        [StringLength(300)]
        public string Address { get; set; }
        [Column("photo")]
        public byte[] Photo { get; set; }
        [Column("DRVLC")]
        [StringLength(100)]
        public string Drvlc { get; set; }
        [StringLength(50)]
        public string State { get; set; }
    }
}
