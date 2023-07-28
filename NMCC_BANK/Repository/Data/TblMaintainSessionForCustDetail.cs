using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Tbl_MaintainSessionForCustDetails")]
    public partial class TblMaintainSessionForCustDetail
    {
        [Key]
        public long CustSessionId { get; set; }
        [StringLength(20)]
        public string CustMobileId { get; set; }
        [Column("CustDetailID")]
        public long? CustDetailId { get; set; }
        [StringLength(200)]
        public string TransactionId { get; set; }
    }
}
