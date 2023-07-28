using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("Indofin_ErrorLogs")]
    public partial class IndofinErrorLog
    {
        [Column("ID")]
        public long Id { get; set; }
        public long PersonalDetailId { get; set; }
        public string ErrorMsg { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(50)]
        public string ControllerName { get; set; }
        [StringLength(50)]
        public string ActionName { get; set; }
    }
}
