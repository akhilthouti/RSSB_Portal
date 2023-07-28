using System.Collections.Generic;

namespace INDO_FIN_NET
{
    public class RekycDedupepan
    {
        public int custNo { get; set; }
        public string name { get; set; }
        public string panNo { get; set; }
        public string PanRadioFlag { get; set; }
    }
    public class DEDUPE_GRID_PAN
    {
        public List<RekycDedupepan> RekycDedupepans { get; set; }

    }
    public class RekycJointgridlist
    {
        public long? custNo { get; set; }
        public string name { get; set; }
        public string RekycJointRadioFlag { get; set; }
    }
    public class RekycJoint_GRID_MAIN
    {
        public List<RekycJointgridlist> RekycJointgridlists { get; set; }


    }
}
