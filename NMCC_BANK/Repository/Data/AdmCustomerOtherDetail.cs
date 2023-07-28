using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CustomerOtherDetail")]
    public partial class AdmCustomerOtherDetail
    {
        [Key]
        [Column("CustDetailID")]
        public long CustDetailId { get; set; }
        [StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string Gender { get; set; }
        [Column("DOB")]
        [StringLength(50)]
        public string Dob { get; set; }
        [Column("Address_Line1")]
        [StringLength(100)]
        public string AddressLine1 { get; set; }
        [Column("Address_Line2")]
        [StringLength(100)]
        public string AddressLine2 { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string Pincode { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string Country { get; set; }
        [StringLength(20)]
        public string RefNumber { get; set; }
        [StringLength(10)]
        public string DocumentType { get; set; }
        [StringLength(30)]
        public string DocumentNo { get; set; }
        public long? CustomerId { get; set; }
    }
}
