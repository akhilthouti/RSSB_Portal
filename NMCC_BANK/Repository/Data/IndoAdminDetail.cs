using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("Indo_AdminDetails")]
    public partial class IndoAdminDetail
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
        [StringLength(100)]
        public string Branch { get; set; }
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
        [Column(TypeName = "datetime")]
        public DateTime? LoginDateTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LogOutDateTime { get; set; }
        [StringLength(500)]
        public string SessionKey { get; set; }
        [Column("isLogin")]
        public bool? IsLogin { get; set; }
        [Column("OrganizationID")]
        public long? OrganizationId { get; set; }
        [StringLength(80)]
        public string OrganizationName { get; set; }
        [StringLength(50)]
        public string RoleType { get; set; }
        [Column("Is_Locked")]
        public bool? Is_Locked { get; set; }
        [Column("IsActive")]
        public bool? IsActive { get; set; }
        public string HostName { get; set; }
        public string HostIP { get; set; }

    }
}
