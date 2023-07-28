using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_Cosmos_CustomerJointAccountHolderDetails")]
    public partial class AdmCosmosCustomerJointAccountHolderDetail
    {
        [Key]
        [Column("ID")]
        public long Id { get; set; }
        [Column("MainholderID")]
        public long? MainholderId { get; set; }
        [StringLength(50)]
        public string MainholderFirstName { get; set; }
        [StringLength(50)]
        public string MainholderMiddleName { get; set; }
        [StringLength(50)]
        public string MainholderLastName { get; set; }
        [Column("JointholderID")]
        [StringLength(50)]
        public string JointholderId { get; set; }
        [StringLength(50)]
        public string JointholderSearchNumber { get; set; }
        [StringLength(50)]
        public string JointholderFirstName { get; set; }
        [StringLength(50)]
        public string JointholderMiddleName { get; set; }
        [StringLength(50)]
        public string JointholderLastName { get; set; }
        [StringLength(50)]
        public string JointHolderCountNumber { get; set; }
        public bool? IsActive { get; set; }
        [Column("createdby")]
        public long? Createdby { get; set; }
        [Column("createdDate", TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
