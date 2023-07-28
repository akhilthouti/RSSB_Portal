namespace INDO_FIN_NET.Models.CurrentModels
{
    public class ClsCurrentDocDetails
    {
        public long? CustomerDetailId { get; set; }
        public string DocName { get; set; }
        public string DocType { get; set; }
        public string DocMainCategory { get; set; }
        public string DocCategory { get; set; }
        public byte[] DocDetails { get; set; }
        public string Source { get; set; }
    }
}
