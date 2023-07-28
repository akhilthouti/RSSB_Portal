using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceProvider1.Models.Admin
{
    public class ClsDataVerification
    {
        #region QEDetails
        [Display(Name = "First Name")]
        public string QEFirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string QEMiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string QELastName { get; set; }

        [Display(Name = "DOB")]
        public string QEDOB { get; set; }
        [Display(Name = "Gender")]
        public string QEGender { get; set; }
        [Display(Name = "Pan Number")]
        public string QEPanNo { get; set; }
        [Display(Name = "Aadhar Number")]
        public string QEAadhaarNo { get; set; }

        //
        [Display(Name = "Father Name")]
        public string QEFatherName { get; set; }

        [Display(Name = "PassportNo")]
        public string QEPassportNo { get; set; }

        [Display(Name = "DrivingLicenceNo")]
        public string QEDrivingLicenceNo { get; set; }
        [Display(Name = "MobileNo")]
        public string QEMobileNo { get; set; }

        [Display(Name = "EmailId")]
        public string QEEmailId { get; set; }

        [Display(Name = "Address1")]
        public string QECLIENT_ADDRESS_1 { get; set; }

        [Display(Name = "Address2")]
        public string QECLIENT_ADDRESS_2 { get; set; }
        [Display(Name = "Address3")]
        public string QECLIENT_ADDRESS_3 { get; set; }

        [Display(Name = "Country")]
        public string QECountry { get; set; }

        [Display(Name = "City")]
        public string QECity { get; set; }

        [Display(Name = "State")]
        public string QEState { get; set; }
        [Display(Name = "Pincode")]
        public string QEPin { get; set; }

        [Display(Name = "PERM ADDRESS_1")]
        public string QECLIENT_PERM_ADDRESS_1 { get; set; }

        [Display(Name = "PERM ADDRESS_2")]
        public string QECLIENT_PERM_ADDRESS_2 { get; set; }


        [Display(Name = "PERM ADDRESS_3")]
        public string QECLIENT_PERM_ADDRESS_3 { get; set; }


        [Display(Name = "PERM CITY")]
        public string QECLIENT_PERM_CITY { get; set; }

        [Display(Name = "PERM COUNTRY")]
        public string QECLIENT_PERM_COUNTRY { get; set; }

        [Display(Name = "Live Photo")]
        public string QELive_Photo { get; set; }



        #endregion

        #region PanDetailsForCompare in dashboard 
        [Display(Name = "Pan No")]
        public string PanNo { get; set; }

        [Display(Name = "Pan Doc")]
        public string PanDocumentType { get; set; }

        [Display(Name = "First Name")]
        public string PanComFirstName { get; set; }

        [Display(Name = "Middle Name")]
        public string PanComMiddleName { get; set; }

        [Display(Name = "Last Name")]
        public string PanComLastName { get; set; }

        [Display(Name = "Pan Number")]
        public string PanComNo { get; set; }

        #endregion
        #region status
        public string FirstNameStatus { get; set; }
        public string MiddleNameStatus { get; set; }
        public string LastNameStatus { get; set; }
        public string DOBStatus { get; set; }
        public string Genderstatus { get; set; }
        public string PanNoStatus { get; set; }
        public string AadhaarNoStatus { get; set; }
        public string facematching { get; set; }
        public string score { get; set; }


        #endregion

        #region AadharData

        [Display(Name = "Aadhar Number")]
        public string AadharNumberDV { get; set; }


        [Display(Name = "Photo")]
        public string AadharPhotoDV { get; set; }

        [Display(Name = "First Name")]
        public string AadharFirstNameDV { get; set; }

        [Display(Name = "Middle Name")]
        public string AadharMiddleNameDV { get; set; }

        [Display(Name = "Last Name")]
        public string AadharLastNameDV { get; set; }

        [Display(Name = "DOB")]
        public string AadharDOBDV { get; set; }

        [Display(Name = "Gender")]
        public string AadharGenderDV { get; set; }

        [Display(Name = "AadharAddress")]
        public string AadharAddress { get; set; }

        [Display(Name = "House")]
        public string House { get; set; }

        [Display(Name = "Street")]
        public string Street { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Post_Office")]
        public string Post_Office { get; set; }

        [Display(Name = "Pin_Code")]
        public string Pin_Code { get; set; }

        [Display(Name = "Locality")]
        public string Locality{get;set;}
        #endregion

        public string isPanApproveOrReject { get; set; }

        public string isAadhaarApproveOrReject { get; set; }
    }
}
