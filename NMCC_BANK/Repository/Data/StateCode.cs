using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("State_Codes")]
    public partial class StateCode
    {
        [Key]
        [Column("State_Id")]
        public int StateId { get; set; }
        [Column("State_Code")]
        [StringLength(3)]
        public string StateCode1 { get; set; }
        [Column("State_Name")]
        [StringLength(100)]
        public string StateName { get; set; }
        [Column("isActive")]
        public bool? IsActive { get; set; }
        [Column("Country_Id")]
        [StringLength(3)]
        public string CountryId { get; set; }
        [StringLength(10)]
        public string CkycstateCode { get; set; }
    }
}
