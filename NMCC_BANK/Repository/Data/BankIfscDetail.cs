using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Bank_IFSC_Details")]
    public partial class BankIfscDetail
    {
        [Key]
        [Column("IFSC_ID")]
        public int IfscId { get; set; }
        [Column("BANK_NAME")]
        [StringLength(100)]
        public string BankName { get; set; }
        [Column("IFSC")]
        [StringLength(30)]
        public string Ifsc { get; set; }
        [Column("MICR")]
        [StringLength(30)]
        public string Micr { get; set; }
        [Column("BRANCH")]
        [StringLength(100)]
        public string Branch { get; set; }
        [Column("ADDRESS")]
        [StringLength(200)]
        public string Address { get; set; }
        [Column("CONTACT")]
        [StringLength(20)]
        public string Contact { get; set; }
        [Column("CITY")]
        [StringLength(50)]
        public string City { get; set; }
        [Column("DISTRICT")]
        [StringLength(50)]
        public string District { get; set; }
        [Column("STATE")]
        [StringLength(50)]
        public string State { get; set; }
    }
}
