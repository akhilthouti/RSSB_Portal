using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET_CORE.Models.CaCertificate
{
    public class CaCertificateDetails
    {
        public long OrganisationId { get; set; }
        [Display(Name = "Company Name")]
        public string OrgCaName { get; set; }
        [Display(Name = "Company Email")]
        public string OrgCaEmail { get; set; }
        [Display(Name = "Country")]
        public string OrgCaCountryName { get; set; }
        [Display(Name = "State")]
        public string OrgCaState { get; set; }
        [Display(Name = "Locality")]
        public string OrgCaLocality { get; set; }
        [Display(Name = "Organization Type")]
        public string OrgCaOrganization { get; set; }
        [Display(Name = "Organization Unit Name")]
        public string OrgCaOrganizationUnitName { get; set; }
        [Display(Name = "ContactPersonName")]
        public string OrgCaContactPersonName { get; set; }
        public string PostalCode { get; set; }
    }

}
