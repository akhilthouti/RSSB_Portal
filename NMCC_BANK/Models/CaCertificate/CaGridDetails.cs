using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET_CORE.Models.CaCertificate
{
    public class CaGridDetails
    {
        public long OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationReferenceNumber { get; set; }
        public byte[] OrganizationLogo { get; set; }
        public string OrganizationDescription { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonNumber { get; set; }
        public string ContactPersonEmail_ID { get; set; }
        public string HO_Address { get; set; }
        public string Fax_No { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
    }

}
