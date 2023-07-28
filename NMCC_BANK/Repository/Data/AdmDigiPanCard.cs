using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_DigiPanCard")]
    public partial class AdmDigiPanCard
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [StringLength(50)]
        public string CustomerId { get; set; }
        [Column("PANNo")]
        [StringLength(150)]
        public string Panno { get; set; }
        [Column("name")]
        [StringLength(150)]
        public string Name { get; set; }
        [Column("firstname")]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Column("middlename")]
        [StringLength(50)]
        public string Middlename { get; set; }
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
        [StringLength(150)]
        public string Orgname { get; set; }
        [Column("PANverifiedOn")]
        [StringLength(100)]
        public string PanverifiedOn { get; set; }
        public string title { get; set; }
    }
}