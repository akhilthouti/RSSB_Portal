using System.Collections.Generic;
using static INDO_FIN_NET.dcumentdedupe;

namespace INDO_FIN_NET
{
    public class dcumentdedupe
    {
        public class Root
        {
            public string CustNo { get; set; }
            public string PanNo { get; set; }
            public string PinCode { get; set; }
            public string State { get; set; }
            public string District { get; set; }
            public string Add1 { get; set; }
            public string Add2 { get; set; }
            public string Mobile { get; set; }
            public string SexCode { get; set; }
            public string Name { get; set; }
            public string EmailID { get; set; }
            public string Nationality { get; set; }
            public string DOB { get; set; }
        }
        public class Root12
        {
            public int custNo { get; set; }
            public string name { get; set; }
            public string panNo { get; set; }
        }


    }
    public class Rekycgridlist
    {
        public int custNo { get; set; }
        public string name { get; set; }
        public string panNo { get; set; }
        public string RekycRadioFlag { get; set; }
    }
    public class Rekyc_GRID_MAIN
    {
        public List<Rekycgridlist> Rekycgridlists { get; set; }

    }
}
