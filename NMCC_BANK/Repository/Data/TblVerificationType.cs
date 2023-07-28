using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("Tbl_VerificationType")]
    public partial class TblVerificationType
    {
        [Column("Sr.No")]
        public long SrNo { get; set; }
        [Column("VTypeID")]
        [StringLength(10)]
        public string VtypeId { get; set; }
        [Column("Cust_VerificationType")]
        [StringLength(50)]
        public string CustVerificationType { get; set; }
    }
}
