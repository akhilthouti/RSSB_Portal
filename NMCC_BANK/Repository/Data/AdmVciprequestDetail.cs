using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("Adm_VCIPRequest_Details")]
    public partial class AdmVciprequestDetail
    {
        public long? CustId { get; set; }
        public bool? CustRequestFlag { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CustReqTime { get; set; }
        public bool? LinkSendFlag { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LinkSendTime { get; set; }
        public bool? ConnectionFlag { get; set; }
        [StringLength(20)]
        public string CustOtp { get; set; }
        public bool? CustOtpVerifyFlag { get; set; }
        [StringLength(50)]
        public string VcipStatus { get; set; }
        [StringLength(50)]
        public string LinkSendBy { get; set; }
        [StringLength(50)]
        public string AuthorizedPerson { get; set; }
        public bool? Manualassignflag { get; set; }
        [StringLength(20)]
        public string CallingKey { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? AutoAssignDate { get; set; }
        public string MeetingID { get; set; }
    }
}
