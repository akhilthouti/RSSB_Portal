using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiceProvider1.Models
{
    public class VehicleInsuranceDetails
    {
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string RegistrationNumber { get; set; }
        public string EngineNumber { get; set; }
        public string ChassisNumber { get; set; }
        //public DateTime RegistrationDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> RegistrationDate { get; set; }

        //  public DateTime ManufacturingDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ManufacturingDate { get; set; }
    }
}