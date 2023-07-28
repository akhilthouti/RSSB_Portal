using System.Collections.Generic;

namespace INDO_FIN_NET
{
    public class Dedupeaccno
    {
        public string DedupeAccFlags { get; set; }
        public string AccountNo { get; set; }
        public class Root1
        {
            public List<string> AccountNo { get; set; }
        }
    }

    public class dedupegridlists
    {
        public string AccountNo { get; set; }
        public string DedupeAccFlags { get; set; }
    }

    public class DEDUPE_GRID_MAIN1
    {
        public List<dedupegridlists> dedupegridlistss { get; set; }

    }

    public class Rekycaccno
    {
        public string RekycAccFlags { get; set; }
        public string AccountNo { get; set; }
        public class Root1
        {
            public List<string> AccountNo { get; set; }
        }
    }

    public class Rekycgridlists
    {
        public string AccountNo { get; set; }
        public string RekycAccFlags { get; set; }
    }

    public class Rekyc_GRID_MAIN1
    {
        public List<Rekycgridlists> Rekycgridlistss { get; set; }

    }
}
