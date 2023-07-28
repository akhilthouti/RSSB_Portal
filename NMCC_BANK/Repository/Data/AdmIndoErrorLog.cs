using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_IndoErrorLogs")]
    public partial class AdmIndoErrorLog
    {
        [Key]
        public long Id { get; set; }
        public string ErrorMsg { get; set; }
        [StringLength(100)]
        public string ControllerName { get; set; }
        [StringLength(100)]
        public string MethodName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
