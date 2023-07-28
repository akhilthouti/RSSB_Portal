using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("Adm_UploadedDocument")]
    public partial class AdmUploadedDocument
    {
        [Column("Doc_Id")]
        public long DocId { get; set; }
        [Column("Doc_Name")]
        [StringLength(500)]
        public string DocName { get; set; }
        [Column("Doc_Size")]
        public long? DocSize { get; set; }
        [Column("Customer_Id")]
        [StringLength(150)]
        public string CustomerId { get; set; }
        [Column("Document_History")]
        public byte[] DocumentHistory { get; set; }
        [Column("Doc_Path")]
        [StringLength(500)]
        public string DocPath { get; set; }
        [Column("Created_Date", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
