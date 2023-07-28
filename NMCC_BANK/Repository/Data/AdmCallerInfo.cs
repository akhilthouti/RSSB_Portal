using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("Adm_CallerInfo")]
    public partial class AdmCallerInfo
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("Caller_Name")]
        [StringLength(50)]
        public string CallerName { get; set; }
        [Column("Caller_Id")]
        [StringLength(50)]
        public string CallerId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public long? CustomerDetailId { get; set; }
    }
}
