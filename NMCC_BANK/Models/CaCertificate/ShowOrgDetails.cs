using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET_CORE.Models.CaCertificate
{
    public class ShowOrgDetails
    {
        public long OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationDescription { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail_ID { get; set; }
    }
}
