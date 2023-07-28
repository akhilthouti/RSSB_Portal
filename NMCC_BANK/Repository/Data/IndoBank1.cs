using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("Indo_Bank1")]
    public partial class IndoBank1
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        [StringLength(50)]
        public string UserName { get; set; }
        [Column("BankID")]
        public long? BankId { get; set; }
        [StringLength(80)]
        public string BankName { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(50)]
        public string MobileNo { get; set; }
        [StringLength(100)]
        public string EmailId { get; set; }
        public long? Region { get; set; }
        public long? Branch { get; set; }
        public long? DepartmentId { get; set; }
        public long? RoleId { get; set; }
        [StringLength(50)]
        public string Password { get; set; }
        [StringLength(10)]
        public string Otp { get; set; }
        [StringLength(100)]
        public string CreateBy { get; set; }
        [StringLength(100)]
        public string UpdateBy { get; set; }
    }
}
