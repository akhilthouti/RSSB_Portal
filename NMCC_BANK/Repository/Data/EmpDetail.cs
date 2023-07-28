using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    public partial class EmpDetail
    {
        public long Id { get; set; }
        public long? EmpId { get; set; }
        [StringLength(50)]
        public string Fname { get; set; }
        [StringLength(50)]
        public string Mname { get; set; }
        [StringLength(50)]
        public string Lname { get; set; }
        [StringLength(20)]
        public string Gender { get; set; }
        [StringLength(50)]
        public string MobileNo { get; set; }
        [StringLength(100)]
        public string Location { get; set; }
        [StringLength(100)]
        public string Designation { get; set; }
        [StringLength(100)]
        public string Skills { get; set; }
    }
}
