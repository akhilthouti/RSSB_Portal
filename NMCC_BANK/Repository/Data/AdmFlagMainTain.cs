using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_FlagMainTain")]
    public partial class AdmFlagMainTain
    {
        public long Id { get; set; }
        public long? CustomerId { get; set; }
        public bool? IsQuickEnrollSubmit { get; set; }
        public bool? IsCAFSubmit { get; set; }
        public bool? IsDocumentSubmit { get; set; }
        [Column("IsIPVSubmit")]
        public bool? IsIpvsubmit { get; set; }
        public bool? IssummarysheetSubmit { get; set; }
        [Column("IsIPVRecord")]
        public bool? IsIpvrecord { get; set; }
        [Column("proceedwithOCR")]
        [StringLength(5)]
        public string ProceedwithOcr { get; set; }
        [Column("shareAadharNumber")]
        [StringLength(5)]
        public string ShareAadharNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public bool? IsPanVerify { get; set; }
        public bool? IsCkycVerify { get; set; }
        public bool? IsAadharVerify { get; set; }
        public bool? IsAadharXmlDone { get; set; }

        public bool? IsCustOtpVerify { get; set; }
        [StringLength(100)]
        public string CustOtpTimeStamp { get; set; }
        public bool? IsBcOtpVerify { get; set; }
        [StringLength(100)]
        public string BcOtpTimeStamp { get; set; }
        public bool? IsApproved { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedDate { get; set; }
        public long? ApprovedBy { get; set; }
        public bool? IsQuickEnrollRejected { get; set; }
        [StringLength(100)]
        public string QuickEnrollRejectReason { get; set; }
        [Column("IsCAFRejected")]
        public bool? IsCafrejected { get; set; }
        [Column("CAFRejectReason")]
        [StringLength(100)]
        public string CafrejectReason { get; set; }
        public bool? IsDocumentRejected { get; set; }
        [StringLength(100)]
        public string DocumentRejectReason { get; set; }
        public bool? IsIpvRejected { get; set; }
        [StringLength(100)]
        public string IpvRejectReason { get; set; }
        [Column("IsCAFPDFRejected")]
        public bool? IsCafpdfrejected { get; set; }
        [Column("CAFPDFRejectReason")]
        [StringLength(100)]
        public string CafpdfrejectReason { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RejectedDate { get; set; }
        public long? RejectedBy { get; set; }
        public bool? IsMailSent { get; set; }
        public bool? IsAssistedCustFlag { get; set; }
        public bool? IsSignUpDone { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? SignUpDoneDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CustActiviteStartDate { get; set; }
        public bool? CustActivityDoneFlg { get; set; }
        public bool? IsIpvSkip { get; set; }
        public bool? IsView { get; set; }
        public long? IsViewBy { get; set; }
        public bool? IsAccountSectionSubmit { get; set; }
        public bool? IsAccountSectionRejected { get; set; }
        [StringLength(100)]
        public string IsAccountSectionRejectReason { get; set; }
        [StringLength(50)]
        public string CustomerCompleteStage { get; set; }
        [Column("isCAFPDF")]
        public bool? IsCafpdf { get; set; }
        [Column("isSavingAcc")]
        public bool? IsSavingAcc { get; set; }
        [Column("isLinkSend")]
        public bool? IsLinkSend { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LinksendTime { get; set; }
        [Column("isLinkIPVComplete")]
        public bool? IsLinkIpvcomplete { get; set; }
        [Column("isFinalIPVComplete")]
        public bool? IsFinalIpvcomplete { get; set; }
        [Column("IsCustomerPDFDownload")]
        public bool IsCustomerPdfdownload { get; set; }
        [Column("CustDownloadedPDF")]
        public byte[] CustDownloadedPdf { get; set; }
        [Column("IsDigiPANSumbitted")]
        public bool? IsDigiPansumbitted { get; set; }
        [Column("IsDigilDRLCSumbitted")]
        public bool? IsDigilDrlcsumbitted { get; set; }
        public bool? IsDigiAadharSumbitted { get; set; }
        public bool? IsPanDone { get; set; }
        public bool? IsDrivingLicenceDone { get; set; }
    }

    [Keyless]
    [Table("adm_FlagMainTain1")]
    public partial class AdmFlagMainTain1
    {
        public long? CustomerId { get; set; }
        public long? IsViewBy { get; set; }

    }
}
