using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Repository.Data
{
    public class AdmRekycCustomerDetail
    {
        [Key]
        public long id { get; set; }
        public string CustomerNo { get; set; }

        public string CustomerFirstname { get; set; }
        public string CustomerMiddlename { get; set; }
        public string CustomerLastname { get; set; }
        public string Customer_Mobno { get; set; }
        public string CustomerEmailID { get; set; }
        public string customerDOB { get; set; }
        public string CustomerGender { get; set; }
        public string CustomerAdd1 { get; set; }
        public string CustomerAdd2 { get; set; }
        public string CustomerAdd3 { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerPincode { get; set; }
        public string CustomerState { get; set; }
        public string CustomerCountryID { get; set; }
        public string CustomerAnualincome { get; set; }
        public string CustomerOccupation { get; set; }



        

    }
}
