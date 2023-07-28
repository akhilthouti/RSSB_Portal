using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_HsmKeyDetails")]
    public partial class AdmHsmKeyDetail
    {
        [Key]
        public long KeyId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Date { get; set; }
        [Column("Key_LabelName")]
        [StringLength(20)]
        public string KeyLabelName { get; set; }
        [StringLength(50)]
        public string KeyVersion { get; set; }
        [StringLength(20)]
        public string KeyType { get; set; }
        public bool? IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExpirayDate { get; set; }
        [StringLength(10)]
        public string Validity { get; set; }
    }
}
