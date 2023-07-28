using System;

namespace INDO_FIN_NET
{
    public class dedupe
    {
        public DateTime timestamp { get; set; }
        public string path { get; set; }
        public int status { get; set; }
        public string error { get; set; }
        public string requestId { get; set; }
        public string Errormessage { get; set; }
        public string Msg { get; set; }
    }
}
