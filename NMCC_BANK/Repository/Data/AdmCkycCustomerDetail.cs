using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Table("adm_Ckyc_CustomerDetails")]
    public partial class AdmCkycCustomerDetail
    {
        [Key]
        [Column("Ckyc_CustomerDetailsID")]
        public long CkycCustomerDetailsId { get; set; }
        [Column("CustomerDetailsID")]
        public long CustomerDetailsId { get; set; }
        [Column("CKYC_Number")]
        [StringLength(250)]
        public string CkycNumber { get; set; }
        [Column("customerDetails20")]
        public string CustomerDetails20 { get; set; }
        [Column("customerDetails40")]
        public string CustomerDetails40 { get; set; }
        [Column("customerDetails40_2")]
        public string CustomerDetails402 { get; set; }
        [Column("customerDetails40_3")]
        public string CustomerDetails403 { get; set; }
        [Column("customerDetails50")]
        public string CustomerDetails50 { get; set; }
        [Column("customerDetails60")]
        public string CustomerDetails60 { get; set; }
        [StringLength(250)]
        public string AadhaarNumber { get; set; }
        [Column("voterNumber")]
        [StringLength(250)]
        public string VoterNumber { get; set; }
        [Column("panNumber")]
        [StringLength(250)]
        public string PanNumber { get; set; }
        [Column("passportnumber")]
        [StringLength(250)]
        public string Passportnumber { get; set; }
        [Column("drivingLicen")]
        [StringLength(250)]
        public string DrivingLicen { get; set; }
        [Column("N_reganumber")]
        [StringLength(250)]
        public string NReganumber { get; set; }
        [Column("Other_Details")]
        [StringLength(250)]
        public string OtherDetails { get; set; }
        [Column("Identification_Number")]
        [StringLength(20)]
        public string IdentificationNumber { get; set; }
        public byte[] Empsignature { get; set; }
        [Column("instituteStamp")]
        public byte[] InstituteStamp { get; set; }
        public long Createdby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Createddate { get; set; }
        [StringLength(250)]
        public string Adharphoto70 { get; set; }
        [StringLength(250)]
        public string Pancardphoto70 { get; set; }
        [Column("voterphoto70")]
        [StringLength(250)]
        public string Voterphoto70 { get; set; }
        [Column("drivingLicenphoto70")]
        [StringLength(250)]
        public string DrivingLicenphoto70 { get; set; }
        [Column("passportphoto70")]
        [StringLength(250)]
        public string Passportphoto70 { get; set; }
        [StringLength(250)]
        public string Nregaphoto70 { get; set; }
        [StringLength(250)]
        public string Others70 { get; set; }
        [Column("Others1_70")]
        [StringLength(250)]
        public string Others170 { get; set; }
        [Column("Others2_70")]
        [StringLength(250)]
        public string Others270 { get; set; }
        [Column("customerphoto70")]
        [StringLength(250)]
        public string Customerphoto70 { get; set; }
        [Column("customersignature70")]
        [StringLength(250)]
        public string Customersignature70 { get; set; }
        public long? Updatedby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Updateddate { get; set; }
        [Column("isVerfy")]
        public bool? IsVerfy { get; set; }
        [Column("isApprove")]
        public bool? IsApprove { get; set; }
        [Column("isReject")]
        public bool? IsReject { get; set; }
        [Column("rejectReason")]
        public string RejectReason { get; set; }
        [Column("iszip")]
        public bool? Iszip { get; set; }
        [Column("isckycapproved")]
        public bool? Isckycapproved { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DownloadDate { get; set; }
        [Column("CKYCStatus")]
        public string Ckycstatus { get; set; }
        [Column("CKYCRefNO")]
        public string CkycrefNo { get; set; }
        [Column("status")]
        public int? Status { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UploadTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ResponseTime { get; set; }
        [Column("LOB")]
        [StringLength(20)]
        public string Lob { get; set; }
        [Column("SYSTEM")]
        [StringLength(20)]
        public string System { get; set; }
        [Column("CCID")]
        [StringLength(20)]
        public string Ccid { get; set; }
        [Column("LoanID")]
        [StringLength(20)]
        public string LoanId { get; set; }
        [Column("MLID")]
        [StringLength(50)]
        public string Mlid { get; set; }
        [StringLength(50)]
        public string Branch { get; set; }
        [Column("isUpdateApprove")]
        public bool? IsUpdateApprove { get; set; }
        [Column("isUpdateCkycApproved")]
        public bool? IsUpdateCkycApproved { get; set; }
        [Column("isUpdate")]
        public bool? IsUpdate { get; set; }
        [StringLength(10)]
        public string PoiStatus { get; set; }
        [Column("ReverseDMSFlag")]
        public bool? ReverseDmsflag { get; set; }
    }
}
