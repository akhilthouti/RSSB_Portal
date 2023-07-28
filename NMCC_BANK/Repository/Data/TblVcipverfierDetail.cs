using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("tblVCIPverfierDetails")]
    public partial class TblVcipverfierDetail
    {
        [Key]
        [Column("Verfier_id")]
        public long VerfierId { get; set; }
        [Required]
        [Column("First_Name")]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [Column("Last_Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        [Column("Empoyee_Id")]
        [StringLength(50)]
        public string EmpoyeeId { get; set; }
        [Required]
        [Column("Mobile_No")]
        [StringLength(20)]
        public string MobileNo { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string VerfierPass { get; set; }
        [Required]
        [Column("Shift_Time")]
        [StringLength(50)]
        public string ShiftTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Intime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Outtime { get; set; }
        public bool? Isactive { get; set; }
    }
}
