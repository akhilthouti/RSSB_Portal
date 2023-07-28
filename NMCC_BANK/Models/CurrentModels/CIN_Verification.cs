using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models.CurrentModels
{
    public class CIN_Verification
    {
        [Key]
        public long CustomerId { get; set; }
        public string ActiveCompliance { get; set; }
        public string AddressotherthanRegisteredoffice { get; set; }
        public string AuthorizedCapital { get; set; }
        public string BalanceSheetDate { get; set; }
        public string CIN { get; set; }
        public string Category { get; set; }
        public string Class { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string DateofIncorporation { get; set; }
        public string LastAnnualGeneralMeetingDate { get; set; }
        public string ListedorUnlisted { get; set; }
        public int NumberofDirectors { get; set; }
        public int NumberofMembers { get; set; }
        public string PaidUpCapital { get; set; }
        public string ROCOffice { get; set; }
        public string RegisteredAddress { get; set; }
        public string RegisteredEmailId { get; set; }
        public string RegistrationNumber { get; set; }
        public string StatusForEfiling { get; set; }
        public string SubCategory { get; set; }
        public string Suspendedatstockexchange { get; set; }
        public string DIN { get; set; }
        public string DateofAppointment { get; set; }
        public string? Enddate { get; set; }
        public string Name { get; set; }
        public string SurrenderedDIN { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public string created_at { get; set; }
        public string ref_id { get; set; }
        public string statusCode { get; set; }
        public string statusMessage { get; set; }
        public string DIN2 { get; set; }
        public string DateofAppointment2 { get; set; }
        public string? Enddate2 { get; set; }
        public string Name2 { get; set; }
        public string SurrenderedDIN2 { get; set; }
    }
}
