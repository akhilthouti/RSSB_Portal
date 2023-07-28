using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace INDO_FIN_NET.Models.CurrentModels
{
    public class CustomIdValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string idType = validationContext.ObjectInstance.GetType().GetProperty("CType").GetValue(validationContext.ObjectInstance, null)?.ToString();

            if (idType == "Pan No")
            {
                if (!Regex.IsMatch((string)value, "^[A-Z]{5}[0-9]{4}[A-Z]{1}$"))
                {
                    return new ValidationResult("Please enter a valid PAN number.");
                }
            }
            else if (idType == "Aadhar")
            {
                if (!Regex.IsMatch((string)value, "^[2-9]\\d{11}$"))
                {
                    return new ValidationResult("Please enter a valid Aadhaar number.");
                }
            }
            else
            {
                return new ValidationResult("Please select a valid ID type.");
            }

            return ValidationResult.Success;
        }
    }


    //["GSTIN"]
    public class Current_Verification
    {
        [Key]
        public long CustomerId { get; set; }
        public string PAN { get; set; }
        public string created_at { get; set; }
        public string ref_idforGSTIN { get; set; }
        public string statusCodeforGSTIN { get; set; }
        public string statusMessageforGSTIN { get; set; }
        public string CreatedByforGSTIN { get; set; }
        public DateTime CreatedDateforGSTIN { get; set; }
        public string CONSTITUTIONOFBUSINESS { get; set; }
        public string GSTIN { get; set; }
        public string LEGALNAMEOFBUSINESS { get; set; }
        public string State { get; set; }
        public string Status { get; set; }
        public string ref_idforPAN { get; set; }
        public string statusCodeforPAN { get; set; }
        public string statusMessageforPAN { get; set; }
        public string CreatedByforPAN { get; set; }
        public DateTime CreatedDateforPAN { get; set; }

        //for CIN
        public string ActiveCompliance { get; set; }
        public string AddressotherthanRegisteredoffice { get; set; }
        public string AuthorizedCapital { get; set; }
        public string BalanceSheetDate { get; set; }
        public string CIN { get; set; }
        public string CategoryforCIN { get; set; }
        public string Class { get; set; }
        public string CompanyName { get; set; }
        public string CompanyType { get; set; }
        public string DateofIncorporation { get; set; }
        public string LastAnnualGeneralMeetingDate { get; set; }
        public string ListedorUnlisted { get; set; }
        public int NumberofDirectors { get; set; }
        public string NumberofMembers { get; set; }
        public string PaidUpCapital { get; set; }
        public string ROCOffice { get; set; }
        public string RegisteredAddress { get; set; }
        public string RegisteredEmailId { get; set; }
        public string RegistrationNumber { get; set; }
        public string StatusForEfiling { get; set; }
        public string SubCategory { get; set; }
        public string Suspendedatstockexchange { get; set; }
        public string DIN1 { get; set; }
        public string DIN2 { get; set; }
        public string DateofAppointment1 { get; set; }
        public string DateofAppointment2 { get; set; }
        public string? Enddate { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string SurrenderedDIN1 { get; set; }
        public string SurrenderedDIN2 { get; set; }
        public int code { get; set; }
        public string message { get; set; }

        //for MSME
        public string CategoryforMSME { get; set; }
        public string DateofCommencement { get; set; }
        public string District { get; set; }
        public string company_name { get; set; }
        public string StateforMSME { get; set; }
        public string messageforMSME { get; set; }
        public string status { get; set; }
        public string created_atforMSME { get; set; }

        //FOR INSERT CAF 
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
        public string? DINCAF { get; set; }
        public string PanCAF { get; set; }
        public string GSTNCAF { get; set; }
        public string UdyogAadhaarCAF { get; set; }
        public string PB { get; set; }
        public string AML { get; set; }
        public string capButton { get; set; }
        public string DigiKYCPhoto { get; set; }
        public string Latitude_Longitude { get; set; }


        public string Prediction { get; set; }

        // current quick enrollement 

        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Please enter a 10-digit numeric value")]
        public string CMobileNo { get; set; }


        public string CType { get; set; }

        [CustomIdValidation(ErrorMessage = "Please enter a valid ID number.")]
        public string CTypeTextbox { get; set; }

    }
    public class DataItem
    {
        public string org_type { get; set; }

    }

}
