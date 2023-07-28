using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Tbl_AadhaarBaseEsignLogs")]
    public partial class TblAadhaarBaseEsignLog
    {
        [Key]
        public long EsignId { get; set; }
        [StringLength(100)]
        public string TransactionNo { get; set; }
        [StringLength(1000)]
        public string FilePath { get; set; }
        public string Requeststring { get; set; }
        public string ResponseString { get; set; }
    }
}
