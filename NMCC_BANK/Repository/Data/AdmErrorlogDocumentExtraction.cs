using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_errorlogDocumentExtraction")]
    public partial class AdmErrorlogDocumentExtraction
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public long? CustomerId { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        public string Response { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Createddate { get; set; }
    }
}
