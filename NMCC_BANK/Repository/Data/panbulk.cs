using System.ComponentModel.DataAnnotations;
using System;

namespace INDO_FIN_NET.Repository.Data
{
    public partial class bulkup
    {
        [Key]

        public int sr_no { get; set; }
        [StringLength(50)]
        public string pan_no { get; set; }
        public string first_name { get; set; }
        [StringLength(50)]
        public string middle_name { get; set; }
        [StringLength(50)]
        public string last_name { get; set; }
        public DateTime date { get; set; }

        public string status { get; set; }


    }
    public partial class AgentPendingExcel
    {
        [Key]

        public long CustomerDetailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        


    }
    public partial class AgentRejectedExcel
    {
        [Key]

        public long CustomerDetailId { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string RejectedReason { get; set; }
        public DateTime RejectedDate { get; set; }


    }
}
