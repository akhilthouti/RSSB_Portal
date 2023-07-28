using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_Digilockererrorlog")]
    public partial class AdmDigilockererrorlog
    {
        [Key]
        public int Identity { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public long? CustomerId { get; set; }
        [StringLength(300)]
        public string Status { get; set; }
        [StringLength(50)]
        public string Digilockertype { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Createddate { get; set; }
    }
}
