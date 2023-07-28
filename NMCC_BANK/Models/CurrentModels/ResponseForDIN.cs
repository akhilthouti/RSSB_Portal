using Newtonsoft.Json;
using System.Collections.Generic;

namespace INDO_FIN_NET.Models.CurrentModels
{
    public class ResponseForDIN
    {

            [JsonProperty("CONSTITUTION OF BUSINESS")]
            public string CONSTITUTIONOFBUSINESS { get; set; }
            public string GSTIN { get; set; }

            [JsonProperty("LEGAL NAME OF BUSINESS")]
            public string LEGALNAMEOFBUSINESS { get; set; }
            public string State { get; set; }
            public string Status { get; set; }
    }

        public class Root
        {
            public int code { get; set; }
            public List<ResponseForDIN> data { get; set; }
            public string message { get; set; }
            public Status status { get; set; }
        }

        public class Status
        {
            public string created_at { get; set; }
            public string ref_id { get; set; }
            public int statusCode { get; set; }
            public string statusMessage { get; set; }
        }
}






    