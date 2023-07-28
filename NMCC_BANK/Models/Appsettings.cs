namespace INDO_FIN_NET.Models
{

    public class Rootobject
    {
        public string AllowedHosts { get; set; }
        public Appsettings AppSettings { get; set; }
        public Connectionstrings ConnectionStrings { get; set; }
        public Logging Logging { get; set; }
    }

    public class Appsettings
    {
        public string ApiUrl { get; set; }
        public string aspnet { get; set; }
        public string PDFLink { get; set; }
        public string webpagesVersion { get; set; }
        public string webpagesEnabled { get; set; }
        public string ClientValidationEnabled { get; set; }
        public string UnobtrusiveJavaScriptEnabled { get; set; }
        public string ExitForLink { get; set; }
        public string Exit { get; set; }
        public string Preview { get; set; }
        public string GOBack { get; set; }
        public string MobileOTPURL { get; set; }
        public string MobileOTPVerifyURL { get; set; }
        public string From_MailAddress { get; set; }
        public string PortalExceptionLogPath { get; set; }
        public string birlalocation { get; set; }
        public string XMLandORcertificates { get; set; }
        public string errorfile { get; set; }
        public string PdfFolder { get; set; }
        public string FinalPDF { get; set; }
        public string SMS { get; set; }
        public string esign { get; set; }
        public string errorfiles { get; set; }
        public string CUST_URL { get; set; }
        public string CPO_URL { get; set; }
        public string Document { get; set; }
        public string VideoCallbackURL { get; set; }
        public string SnapshotCallbackURL { get; set; }
        public string VKYCDATA { get; set; }
        public string VKYCDATA1 { get; set; }
    }

    public class Connectionstrings
    {
        public string DefaultConnection { get; set; }
        public string DefaultConnection2 { get; set; }
    }

    public class Logging
    {
        public Loglevel LogLevel { get; set; }
    }

    public class Loglevel
    {
        public string Default { get; set; }
        public string Microsoft { get; set; }
        public string MicrosoftHostingLifetime { get; set; }
    }

}
