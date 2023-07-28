using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_SelectedOrganizationByBank")]
    public partial class AdmSelectedOrganizationByBank
    {
        [Key]
        public long Id { get; set; }
        [Column("BankDetailID")]
        public long? BankDetailId { get; set; }
        [Column("ServiceProviderID")]
        public long? ServiceProviderId { get; set; }
        [Column("PorductID")]
        public long? PorductId { get; set; }
        [Column("CategoryID")]
        public long? CategoryId { get; set; }
        [Column("SubProductID")]
        public long? SubProductId { get; set; }
        [Column("AlphaServiceID")]
        public long? AlphaServiceId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
