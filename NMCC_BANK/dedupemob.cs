using System.Collections.Generic;

namespace INDO_FIN_NET
{
    public class dedupemob
    {
        public string DedupeRadioFlag { get; set; }
        public string CustNo { get; set; }

        public class Root
        {
            //public List<string> CustNo { get; set; }
            public int custNo { get; set; }
            public string name { get; set; }
            public string panNo { get; set; }
        }


    }

    public class dedupegridlist
    {
        public int custNo { get; set; }
        public string name { get; set; }
        public string panNo { get; set; }
        public string DedupeRadioFlag { get; set; }
    }

    public class DEDUPE_GRID_MAIN
    {
        public List<dedupegridlist> dedupegridlists { get; set; }

    }
}
