using System.ComponentModel.DataAnnotations;
using System;

namespace INDO_FIN_NET.Models.CurrentModels
{
    public class CAFCustomerDetails
    {
        [Key]
        public long CustomerID { get; set; }
        public string Comapnyname { get; set; }
        public string Industrytype { get; set; }
        public string BusinessTL { get; set; }
        public string DOE { get; set; }
        public string POE { get; set; }
        public string Branches { get; set; }
        public string NOE { get; set; }
        public string Turnover { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Landline { get; set; }
        public string PAddress1 { get; set; }
        public string PAddress2 { get; set; }
        public string PAddress3 { get; set; }
        public string PCity { get; set; }
        public string PPincode { get; set; }
        public string PState { get; set; }
        public string PCountry { get; set; }
        public string CAddress1 { get; set; }
        public string CAddress2 { get; set; }
        public string CAddress3 { get; set; }
        public string CCity { get; set; }
        public string CPincode { get; set; }
        public string CState { get; set; }
        public string CCountry { get; set; }
        public string CINCAF { get; set; }
        public string DINCAF { get; set; }
        public string PanCAF { get; set; }
        public string GSTNCAF { get; set; }
        public string UdyogAadhaarCAF { get; set; }
        public string PB { get; set; }
        public string AML { get; set; }
        public string PHOTO { get; set; }

    }
}
