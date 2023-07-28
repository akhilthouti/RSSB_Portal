using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace INDO_FIN_NET.Repository.Data
{
    [Keyless]
    [Table("adm_VehicleInsuranceDetails")]
    public partial class AdmVehicleInsuranceDetail
    {
        public long InsuranceId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(10)]
        public string MobileNumber { get; set; }
        [StringLength(50)]
        public string EmailId { get; set; }
        [StringLength(200)]
        public string Address { get; set; }
        [StringLength(50)]
        public string City { get; set; }
        [StringLength(50)]
        public string State { get; set; }
        [StringLength(50)]
        public string PinCode { get; set; }
        [StringLength(50)]
        public string RegistrationNumber { get; set; }
        [StringLength(50)]
        public string EngineNumber { get; set; }
        [StringLength(50)]
        public string ChassisNumber { get; set; }
        [Column(TypeName = "date")]
        public DateTime? RegistrationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ManufacturingDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
    }
}
