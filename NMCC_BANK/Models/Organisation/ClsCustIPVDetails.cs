namespace INDO_FIN_NET.Models.Organisation
{
    public class ClsCustIPVDetails
    {
        public long PersonalId { get; set; }
        public string ClientName { get; set; }
        public string Pan { get; set; }
        public byte[] IpvVideo { get; set; }
        public string ipvFileName { get; set; }
        public long? VideoFileSize { get; set; }
        public string VideoFilePath { get; set; }

        public string isIPVApproveOrReject { get; set; }

    }
}
