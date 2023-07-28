using Amazon.Auth.AccessControlPolicy;
using System.Transactions;
using System;
using VKYCWebAPI;
using System.ComponentModel.DataAnnotations;

namespace INDO_FIN_NET.Models
{
    public class AdmCustLinkReq
    {
        [Key]
        public string Source { get; set; }


        public string Password { get; set; }

        public string refId { get; set; }



        public string TxnId { get; set; }


        public string Ts { get; set; }


        public string ApplicationID { get; set; }

        public string CustomerName { get; set; }

        public long? MobileNo { get; set; }

        public string EmailID { get; set; }

        public string Ref1 { get; set; }

        public string Ref2 { get; set; }

        public string Ref3 { get; set; }
        public string MobileStatus { get; set; }
        public string MobileError { get; set; }
        public string EmailStatus { get; set; }
        public string EmailError { get; set; }
        public string CPO_URL { get; set; }
        public string userid { get; set; }
        public string VkycMode { get; set; }
        public string CUST_URL { get; set; }
        public string TransactionStatus { get; set; }
        public string OtpVerfication { get; set; }
        public string RequestXml { get; set; }
        public string RespXml { get; set; }
        public string MeetingID { get; set; }
        public string CustomerID { get; set; }  


    }
}
