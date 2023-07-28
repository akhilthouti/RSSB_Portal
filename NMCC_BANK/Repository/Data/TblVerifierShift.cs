using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("tblVerifierShift")]
    public partial class TblVerifierShift
    {
        [Key]
        [Column("Shift_Id")]
        public long ShiftId { get; set; }
        [Required]
        [Column("Shift_Time")]
        [StringLength(20)]
        public string ShiftTime { get; set; }
        public bool? Isactive { get; set; }
        [Column("Lunch_Time")]
        [StringLength(20)]
        public string LunchTime { get; set; }
    }
    public partial class Mobile_result
    {
        [Key]
        [Required]
        [Column("Result")]
        [StringLength(20)]
        public string Result { get; set; }

    }
}
