using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("INDO_KYC_EmailMobileDetails")]
    public partial class IndoKycEmailMobileDetail
    {
        [Key]
        [Column("MobileEmail_Id")]
        public long MobileEmailId { get; set; }
        [Column("EmailMobile_code")]
        [StringLength(2)]
        public string EmailMobileCode { get; set; }
        [StringLength(50)]
        public string Description { get; set; }
    }
}
