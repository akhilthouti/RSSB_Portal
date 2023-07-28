using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_CKYC_Download_Logs")]
    public partial class AdmCkycDownloadLog
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string TransactionId { get; set; }
        [Column("UserID")]
        [StringLength(100)]
        public string UserId { get; set; }
        [Column("CustomerID")]
        [StringLength(50)]
        public string CustomerId { get; set; }
        [Column("CKYC_No")]
        [StringLength(25)]
        public string CkycNo { get; set; }
        [Column("DOB")]
        [StringLength(100)]
        public string Dob { get; set; }
        [StringLength(500)]
        public string ErrorCode { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [StringLength(50)]
        public string VerificationDate { get; set; }
        [Column("Emp_Code")]
        [StringLength(100)]
        public string EmpCode { get; set; }
        [Column("Emp_Name")]
        [StringLength(100)]
        public string EmpName { get; set; }
        [Column("Emp_Designation")]
        [StringLength(100)]
        public string EmpDesignation { get; set; }
        [Column("Emp_Branch")]
        [StringLength(100)]
        public string EmpBranch { get; set; }
    }
}
