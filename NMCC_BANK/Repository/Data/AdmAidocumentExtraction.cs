using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_AIDocumentExtraction")]
    public partial class AdmAidocumentExtraction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public long? Customerid { get; set; }
        [StringLength(100)]
        public string StatusCode { get; set; }
        [StringLength(100)]
        public string CardName { get; set; }
        [StringLength(100)]
        public string Documentid { get; set; }
        [Column("fullname")]
        [StringLength(100)]
        public string Fullname { get; set; }
        [Column("dob")]
        [StringLength(50)]
        public string Dob { get; set; }
        [Column("gender")]
        [StringLength(50)]
        public string Gender { get; set; }
        [Column("relationtype")]
        [StringLength(50)]
        public string Relationtype { get; set; }
        [Column("initialname")]
        [StringLength(50)]
        public string Initialname { get; set; }
        [Column("fname")]
        [StringLength(50)]
        public string Fname { get; set; }
        [Column("mname")]
        [StringLength(50)]
        public string Mname { get; set; }
        [Column("lname")]
        [StringLength(50)]
        public string Lname { get; set; }
    }
}
