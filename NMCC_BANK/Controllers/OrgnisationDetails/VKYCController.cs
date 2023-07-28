using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using INDO_FIN_NET.Repository;
using ServiceProvider1.Models;
using Newtonsoft.Json;
using ServiceProvider1.Models.OrgExceptionLogs;
using INDO_FIN_NET.Models.VKYC;
using Microsoft.AspNetCore.Http;
using INDO_FIN_NET.Models.Organisation;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;
using RestSharp;
using static INDO_FIN_NET.Models.VKYC.vkyc;
using Microsoft.Extensions.Configuration;
using INDO_FIN_NET.Models;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ServiceProvider1.Controllers.OrgnisationDetails
{
    public class VKYCController : Controller
    {
        private readonly RSSBPRODDbCotext ObjIndo;
        private readonly INDO_FinNetDbCotext objData;
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        CPO_model CM = new CPO_model();
        ClsCustIPVDetails objcustIpv = new ClsCustIPVDetails();
        private readonly IConfiguration _config;
        private readonly IConfiguration configuration;
        private Appsettings _settings;
        private readonly string _connectionString;
        public VKYCController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            ObjIndo = Context;
            objData = iNDO_;
            _settings = new Appsettings();
            configuration.GetSection("Appsettings").Bind(_settings);
            _connectionString = configuration.GetConnectionString("DefaultConnection2");


        }

        public HttpResponseMessage Post(HttpRequestMessage body)
        {

            string ts = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");//2016-01-11T14:44:48+05:30

            //Request Parameter
            string Source = "";
            string meetingID = "";
            string Password = "";
            string TxnID = "";
            string CustName = "";
            string MobileNo = "";
            string AgentMobileNo = "";
            string EmailID = "";
            string Userid = "";
            string ApplicationID = "";
            string VkycType = "";
            string VkycMode = "";
            string MeetingTime = "";
            string Ref1 = "";
            string Ref2 = "";
            string Ref3 = "";

            string SMS_RESP = "";
            string SMS_RESP1 = "";

            //  string ReqTs = "";
            string startdatetime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            string Enddatetime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");



            Dictionary<string, string> repdict = new Dictionary<string, string>();
            Dictionary<string, string> objgenmet = new Dictionary<string, string>();
            XDocument VKYCReq = new XDocument();

            string pstatus = "";
            string pToken = "";
            string pHost_url = "";
            string pcust_url = "";
            string Respxml = "";
            string responce = string.Empty;
            string ErrorCode = "";
            string ErrorMsg = "";
            string TransactionStatus = "";

            string CUST_URL = "";
            string MobileStatus = "";
            string EmailStatus = "";
            string EmaildError = "";
            string MobileError = "";
            string exception = "";
            string CPO_URL = "";
            string refID = "";
            string MeetingID = "";
            string Passcode = "";
            string msg = "";
            string msg1 = "";



            string EmailTxnID = "";

            try
            {
                ClsCustQuickEnrollment obj = new ClsCustQuickEnrollment();
                //string MoNo2 = Convert.ToString(TempData["CustMob"]);
                string MoNo = Convert.ToString(HttpContext.Session.GetString("CustMob1"));
                //var qdata = ObjIndo.USP_GetQEDetailsByMobNo(MoNo);
                var qdata = ObjIndo.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsByCustID {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                // var qdata = ObjIndo.USP_GetQEDetailsByCustID(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                obj.QEFirstName = qdata.CustFirstName;
                obj.QELastName = qdata.CustLastName;
                obj.QEEmailId = qdata.CustEmailId;
                obj.QEMobileNo = qdata.CustMobileNo;
                refID = GetRandomString();
                TempData["refID1"] = refID;
                /*  Session["refID1"]= */
                HttpContext.Session.SetString("refID1", refID);
                Passcode = GetRandomString1();
                var cutomerid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_InsertPasscode", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Passcode", Passcode);
                    cmd.Parameters.AddWithValue("@CustomerDetailId", cutomerid);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    connection.Close();
                }

                string conn1 = _connectionString;
                using (SqlConnection connection1 = new SqlConnection(conn1))
                {
                    SqlCommand cmd = new SqlCommand("USP_InsertCallingKey", connection1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Key", refID);
                    cmd.Parameters.AddWithValue("@CustomerDetailId", cutomerid);

                    connection1.Open();
                    int result = cmd.ExecuteNonQuery();
                    connection1.Close();
                }
                var cust_url_new = "https://kyc.indofinnet.com//CustomerOTP/CustomerOTP?";
                //var cust_url_new = "https://localhost:5003/CustomerOTP/CustomerOTP?";
                var cpo_url_new = "https://rssbactopen.silsaas.co.in//CPO/CPO?";
                CUST_URL = cust_url_new + "RefID=" + refID;
                CPO_URL = cpo_url_new + "RefID=" + refID;
                
              




                try
                {
                    responce = "\r\n<VkycRequest>\r\n<Source>CP1</Source>\r\n<Password>" + Passcode + "</Password>\r\n<TxnId>123</TxnId>\r\n<Userid>123</Userid>\r\n<ApplicationID>123</ApplicationID>\r\n<MobileNo>7021270023</MobileNo>\r\n<AgentMobileNo>7021270023</AgentMobileNo>\r\n<EmailID>dubeyjogendra@gmail.com</EmailID>\r\n<CustomerName>jogendra</CustomerName>\r\n<VkycType>Assisted</VkycType>\r\n<VkycMode>immediate</VkycMode>\r\n<MeetingTime></MeetingTime>\r\n<Ref1></Ref1>\r\n<Ref2></Ref2>\r\n<Ref3></Ref3>\r\n</VkycRequest>";
                }
                catch (Exception ex)
                {
                    TransactionStatus = "UNSUCCESSFUL";
                    ErrorCode = "001";
                    ErrorMsg = "Error During Receving Request";
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);



                    exception = ex.Message + ex.InnerException + ex.Source + ex.HelpLink + ex.HResult + ex.TargetSite + ex.StackTrace;
                    string conn10 = _connectionString;
                    using (SqlConnection connection10 = new SqlConnection(conn10))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection10);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                        cmd.Parameters.AddWithValue("@PToken", pToken);
                        cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@latitude", "latitude");
                        cmd.Parameters.AddWithValue("@longitude", "longitude");
                        cmd.Parameters.AddWithValue("@location", "location");
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));
                        connection10.Open();
                        int result = cmd.ExecuteNonQuery();
                        connection10.Close();
                    }
                }
                #region ParseRequestXml

                try
                {


                    VKYCReq = XDocument.Parse(responce);

                    Source = VKYCReq.Root.Element("Source").Value;
                    Password = VKYCReq.Root.Element("Password").Value;
                    TxnID = VKYCReq.Root.Element("TxnId").Value;
                    Userid = VKYCReq.Root.Element("Userid").Value;
                    ApplicationID = VKYCReq.Root.Element("ApplicationID").Value;
                    //ReqTs = VKYCReq.Root.Element("DateTime").Value;
                    MobileNo = VKYCReq.Root.Element("MobileNo").Value;
                    AgentMobileNo = VKYCReq.Root.Element("AgentMobileNo").Value;
                    EmailID = VKYCReq.Root.Element("EmailID").Value;
                    CustName = VKYCReq.Root.Element("CustomerName").Value;
                    VkycType = VKYCReq.Root.Element("VkycType").Value;
                    VkycMode = VKYCReq.Root.Element("VkycMode").Value;
                    MeetingTime = VKYCReq.Root.Element("MeetingTime").Value;
                    Ref1 = VKYCReq.Root.Element("Ref1").Value;
                    Ref2 = VKYCReq.Root.Element("Ref2").Value;
                    Ref3 = VKYCReq.Root.Element("Ref3").Value;

                }
                catch (Exception ex)
                {

                    TransactionStatus = "UNSUCCESSFUL";

                    ErrorCode = "002";
                    ErrorMsg = "Error During Parsing RequestXml";
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);


                    exception = ex.Message + ex.InnerException + ex.Source + ex.HelpLink + ex.HResult + ex.TargetSite + ex.StackTrace;


                    //objSUD_VideoKycEntities.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ReqTs, ts, MobileNo, EmailID, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, doneby, VkycUrl, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml);

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };

                }

                #endregion ParseRequestXml

                #region Validation

                #region onlyMobile

                if (MobileNo == null || MobileNo == "")
                {
                    TransactionStatus = "UNSUCCESSFUL";
                    ErrorCode = "008";
                    MobileError = "Mobile Number Required";
                    MobileStatus = "UNSUCCESSFUL";
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);

                    // objSUD_VideoKycEntities.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts,CustName, MobileNo, EmailID, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid,VkycMode, CUST_URL,pstatus, pToken,pHost_url, pcust_url,TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml);
                    //ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                    string conn9 = _connectionString;
                    using (SqlConnection connection9 = new SqlConnection(conn9))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection9);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                        cmd.Parameters.AddWithValue("@PToken", pToken);
                        cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@latitude", "latitude");
                        cmd.Parameters.AddWithValue("@longitude", "longitude");
                        cmd.Parameters.AddWithValue("@location", "location");
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));
                        //cmd.Parameters.AddWithValue("@AuthorizedPerson", AuthorizedPerson);
                        //cmd.Parameters.AddWithValue("@AutoAssignDate", AutoAssignDate);
                        //cmd.Parameters.AddWithValue("@VcipStatus", VcipStatus);










                        connection9.Open();
                        int result = cmd.ExecuteNonQuery();
                        connection9.Close();
                    }
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };


                }


                else if ((MobileNo != "" || MobileNo != null) && (EmailID == "" || EmailID == null))
                {
                    Regex regexM = new Regex(@"^[0-9]{10}$");
                    Match matchM = regexM.Match(MobileNo);
                    if (matchM.Success)
                    {



                        if (_settings.SMS != "CosmosBank")
                        {
                            // SMSService.OTPClient objsms1 = new SMSService.OTPClient();
                            // SMS_RESP = objsms1.SendMessage(MobileNo, msg);
                        }
                        else if (_settings.SMS == "CosmosBank")
                        {
                            // CosmosSMSservice.Service1Client objSMS = new CosmosSMSservice.Service1Client();
                            Dictionary<string, string> objDictionarySMS = new Dictionary<string, string>();
                            // objDictionarySMS = objSMS.SMS_API(MobileNo, msg);
                            if (objDictionarySMS.ContainsKey("Success"))
                            {
                                SMS_RESP = objDictionarySMS["Success"];
                                SMS_RESP = "E001";
                            }
                            else
                            {
                                SMS_RESP = objDictionarySMS["Error"];
                            }
                        }


                        if (SMS_RESP.Contains("E001"))
                        {

                            #region meeting region

                            try
                            {


                                objgenmet = create_meeting(CustName, Userid, startdatetime, Enddatetime, MobileNo, EmailID, Password);

                                if (objgenmet["Status"] == "SUCCESS")
                                {
                                    MeetingID = objgenmet["MeetingID"];
                                }
                            }
                            catch
                            {
                            }

                            //objgenmet = GetToken(CustName, MobileNo, EmailID);

                            //if (objgenmet["Status"] == "SUCCESS")
                            //{
                            //    pcust_url = objgenmet["CustLink"];
                            //    pHost_url = objgenmet["HostLink"];
                            //    pToken = objgenmet["Token"];
                            //}

                            #endregion meeting region

                            TransactionStatus = "PENDING";
                            MobileStatus = "SUCCESSFUL";

                        }
                        else
                        {
                            TransactionStatus = "UNSUCCESSFUL";
                            ErrorCode = "010";
                            MobileStatus = "UNSUCCESSFUL";
                            ErrorMsg = SMS_RESP;
                        }

                    }

                    else
                    {
                        TransactionStatus = "UNSUCCESSFUL";
                        MobileStatus = "UNSUCCESSFUL";
                        ErrorCode = "006";
                        MobileError = "Invalid Mobile Number";

                    }
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, MobileStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);
                    //ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                    string conn3 = _connectionString;
                    using (SqlConnection connection3 = new SqlConnection(conn3))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection3);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                        cmd.Parameters.AddWithValue("@PToken", pToken);
                        cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@latitude", "latitude");
                        cmd.Parameters.AddWithValue("@longitude", "longitude");
                        cmd.Parameters.AddWithValue("@location", "location");
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));
                        //cmd.Parameters.AddWithValue("@AuthorizedPerson", AuthorizedPerson);
                        //cmd.Parameters.AddWithValue("@AutoAssignDate", AutoAssignDate);
                        //cmd.Parameters.AddWithValue("@VcipStatus", VcipStatus);










                        connection3.Open();
                        int result = cmd.ExecuteNonQuery();
                        connection3.Close();
                    }
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };



                }


                #endregion onlyMobile


                #region MobileWithEmail
                else if ((MobileNo != "" || MobileNo != null) && (EmailID != "" || EmailID != null))
                {
                    Regex regexM = new Regex(@"^[0-9]{10}$");
                    Match matchM = regexM.Match(MobileNo);
                    Match matchM1 = regexM.Match(AgentMobileNo);

                    Regex regexE = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match matchE = regexE.Match(EmailID);

                    if (matchM.Success && matchE.Success)
                    {
                        TransactionStatus = "PENDING";

                        if (_settings.SMS != "CosmosBank")
                        {
                            string OrgID = "IndoFin007";
                            //string LinkTocustomer = "asd";
                            // string LinkTocustomer = "https://Kyc.indofinnet.com"+ CUST_URL;
                            //string LinkTocustomer = CUST_URL;
                            string LinkTocustomer = CUST_URL;
                            string CustName1 = obj.QEFirstName + " " + obj.QELastName;

                            //string LinkTocustomer = "https://kyc.indofinnet.com//CustomerOTP/CustomerOTP?RefID=qJS3TqONPq";
                            var result = VKYCSMS(obj.QEMobileNo, OrgID, LinkTocustomer, Passcode, CustName1);
                            SMS_RESP = "SUCCESSFUL";
                            MobileStatus = "SUCCESSFUL";
                        }
                        else if (_settings.SMS == "CosmosBank")
                        {
                            // CosmosSMSservice.Service1Client objSMS = new CosmosSMSservice.Service1Client();
                            Dictionary<string, string> objDictionarySMS = new Dictionary<string, string>();
                            // objDictionarySMS = objSMS.SMS_API(MobileNo, msg);
                            if (objDictionarySMS.ContainsKey("Success"))
                            {
                                SMS_RESP = objDictionarySMS["Success"];
                                SMS_RESP = "E001";
                            }
                            else
                            {
                                SMS_RESP = objDictionarySMS["Error"];
                            }
                        }
                        //if (SMS_RESP.Contains("E001"))
                        //{

                        //    MobileStatus = "SUCCESSFUL";



                        //}
                        //else
                        //{

                        //    ErrorCode = "010";
                        //    MobileError = SMS_RESP;
                        //    MobileStatus = "UNSUCCESSFUL";
                        //}



                        #region SendEmail


                        string respemail = SendEmail(EmailID, msg);
                        if (respemail == "SUCCESS")
                        {
                            EmailStatus = "SUCCESSFUL";


                        }
                        else
                        {

                            ErrorCode = "010";
                            EmaildError = "Unable to Send Email";
                            EmailStatus = "UNSUCCESSFUL";
                        }

                        #endregion  SendEmail

                        if (EmailStatus == "SUCCESSFUL" && MobileStatus == "SUCCESSFUL")
                        {


                            #region meeting 
                            try
                            {


                                objgenmet = create_meeting(CustName, Userid, startdatetime, Enddatetime, MobileNo, EmailID, Password);

                                if (objgenmet["Status"] == "SUCCESS")
                                {
                                    MeetingID = objgenmet["MeetingID"];
                                    // MeetingID = "ae067ab9-be96-4b20-2fb7-08dabe274b93";
                                }
                            }
                            catch
                            {
                            }
                            //objgenmet = GetToken(CustName, MobileNo, EmailID);

                            //if (objgenmet["Status"] == "SUCCESS")
                            //{
                            //    pcust_url = objgenmet["CustLink"];
                            //    pHost_url = objgenmet["HostLink"];
                            //    pToken = objgenmet["Token"];
                            //}

                            #endregion meeting
                            TransactionStatus = "PENDING";

                        }
                        else
                        {
                            TransactionStatus = "UNSUCCESSFUL";

                        }
                    }

                    else if (matchM.Success)
                    {
                        TransactionStatus = "UNSUCCESSFUL";

                        EmailStatus = "UNSUCCESSFUL";
                        EmaildError = "Invalid Email ID";

                        if (_settings.SMS != "CosmosBank")
                        {
                            // SMSService.OTPClient objsms1 = new SMSService.OTPClient();
                            //SMS_RESP = objsms1.SendMessage(MobileNo, msg);
                        }
                        else if (_settings.SMS == "CosmosBank")
                        {
                            // CosmosSMSservice.Service1Client objSMS = new CosmosSMSservice.Service1Client();
                            Dictionary<string, string> objDictionarySMS = new Dictionary<string, string>();
                            //objDictionarySMS = objSMS.SMS_API(MobileNo, msg);
                            if (objDictionarySMS.ContainsKey("Success"))
                            {
                                SMS_RESP = objDictionarySMS["Success"];
                                SMS_RESP = "E001";
                            }
                            else
                            {
                                SMS_RESP = objDictionarySMS["Error"];
                            }
                        }
                        if (SMS_RESP.Contains("E001"))
                        {

                            MobileStatus = "SUCCESSFUL";



                            #region meeeting

                            try
                            {

                                objgenmet = create_meeting(CustName, Userid, startdatetime, Enddatetime, MobileNo, EmailID, Password);

                                if (objgenmet["Status"] == "SUCCESS")
                                {
                                    MeetingID = objgenmet["MeetingID"];
                                }
                            }
                            catch
                            {
                            }

                            //objgenmet = GetToken(CustName, MobileNo, EmailID);

                            //if (objgenmet["Status"] == "SUCCESS")
                            //{
                            //    pcust_url = objgenmet["CustLink"];
                            //    pHost_url = objgenmet["HostLink"];
                            //    pToken = objgenmet["Token"];
                            //}
                            #endregion meeeting
                        }
                        else
                        {

                            ErrorCode = "010";
                            MobileError = SMS_RESP;
                            MobileStatus = "UNSUCCESSFUL";
                        }



                    }

                    else if (matchE.Success)
                    {
                        TransactionStatus = "UNSUCCESSFUL";
                        MobileStatus = "UNSUCCESSFUL";

                        MobileError = "Invalid Mobile No";
                        #region SendEmail


                        string respemail = SendEmail(EmailID, msg);
                        if (respemail == "SUCCESS")
                        {

                            EmailStatus = "SUCCESSFUL";
                            #region  meeting


                            try
                            {

                                objgenmet = create_meeting(CustName, Userid, startdatetime, Enddatetime, MobileNo, EmailID, Password);

                                if (objgenmet["Status"] == "SUCCESS")
                                {
                                    MeetingID = objgenmet["MeetingID"];
                                }
                            }
                            catch
                            {
                            }
                            //objgenmet = GetToken(CustName, MobileNo, EmailID);

                            //if (objgenmet["Status"] == "SUCCESS")
                            //{
                            //    pcust_url = objgenmet["CustLink"];
                            //    pHost_url = objgenmet["HostLink"];
                            //    pToken = objgenmet["Token"];
                            //}
                            #endregion meeting
                        }
                        else
                        {

                            ErrorCode = "010";
                            EmaildError = "Unable to Send Email";
                            EmailStatus = "UNSUCCESSFUL";
                        }

                        #endregion  SendEmail

                    }


                    else
                    {
                        TransactionStatus = "UNSUCCESSFUL";
                        MobileStatus = "UNSUCCESSFUL";
                        EmailStatus = "UNSUCCESSFUL";
                        MobileError = "Invalid Mobile No";
                        EmaildError = "Invalid Email ID";
                        CUST_URL = "";
                    }

                    Respxml = VkycResponseXmlBoth(SMS_RESP, TransactionStatus, ts, TxnID, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, CUST_URL, EmailTxnID, Ref1, Ref2, Ref3);

                    //ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                    string conn4 = _connectionString;
                    using (SqlConnection connection4 = new SqlConnection(conn4))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection4);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        //cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        // cmd.Parameters.AddWithValue("@Pstatus", "pstatus");
                        //cmd.Parameters.AddWithValue("@PToken", pToken);
                        //cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        //cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "PENDING");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "PENDING");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        //cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        //cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        //cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        //cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));

                        //cmd.Parameters.AddWithValue("@latitude", "latitude");
                        // cmd.Parameters.AddWithValue("@longitude", "longitude");
                        //cmd.Parameters.AddWithValue("@location", "location");
                        //cmd.Parameters.AddWithValue("@AuthorizedPerson", AuthorizedPerson);
                        //cmd.Parameters.AddWithValue("@AutoAssignDate", AutoAssignDate);
                        //cmd.Parameters.AddWithValue("@VcipStatus", VcipStatus);










                        connection4.Open();
                        int result = cmd.ExecuteNonQuery();
                        //    SqlDataReader reader2 = cmd.ExecuteReader();
                        //    if (reader2.Read())
                        //    {

                        //        var Result1 = reader2["RESULT"].ToString();
                        //    }
                        connection4.Close();
                    }

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };


                }


                else
                {
                    TransactionStatus = "UNSUCCESSFUL";
                    ErrorMsg = "Invalid Input";
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);

                    // ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                    string conn5 = _connectionString;
                    using (SqlConnection connection5 = new SqlConnection(conn5))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection5);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                        cmd.Parameters.AddWithValue("@PToken", pToken);
                        cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@latitude", "latitude");
                        cmd.Parameters.AddWithValue("@longitude", "longitude");
                        cmd.Parameters.AddWithValue("@location", "location");
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));
                        //cmd.Parameters.AddWithValue("@AuthorizedPerson", AuthorizedPerson);
                        //cmd.Parameters.AddWithValue("@AutoAssignDate", AutoAssignDate);
                        //cmd.Parameters.AddWithValue("@VcipStatus", VcipStatus);










                        connection5.Open();
                        int result = cmd.ExecuteNonQuery();
                        connection5.Close();
                    }
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };
                }


                #endregion MobileWithEmail


                #endregion Validation


            }
            catch (Exception ex)
            {
                TransactionStatus = "UNSUCCESSFUL";
                ErrorCode = "004";
                ErrorMsg = "Some Error Occured";
                Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, "", "", ErrorMsg);


                exception = ex.Message + ex.InnerException + ex.Source + ex.HelpLink + ex.HResult + ex.TargetSite + ex.StackTrace;
                // ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                string conn6 = _connectionString;
                using (SqlConnection connection6 = new SqlConnection(conn6))
                {
                    SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection6);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Source", Source);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@refId", refID);
                    cmd.Parameters.AddWithValue("@TxnId", TxnID);
                    cmd.Parameters.AddWithValue("@Ts", ts);
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cmd.Parameters.AddWithValue("@CustomerName", CustName);
                    cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                    cmd.Parameters.AddWithValue("@EmailID", EmailID);
                    cmd.Parameters.AddWithValue("@Ref1", Ref1);
                    cmd.Parameters.AddWithValue("@Ref2", Ref2);
                    cmd.Parameters.AddWithValue("@Ref3", Ref3);
                    cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                    cmd.Parameters.AddWithValue("@MobileError", MobileError);
                    cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                    cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                    cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                    cmd.Parameters.AddWithValue("@userid", Userid);
                    cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                    cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                    cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                    cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                    cmd.Parameters.AddWithValue("@PToken", pToken);
                    cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                    cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                    cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                    cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                    cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                    cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                    cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                    cmd.Parameters.AddWithValue("@Exception", exception);
                    cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                    cmd.Parameters.AddWithValue("@RespXml", Respxml);
                    //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                    cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                    cmd.Parameters.AddWithValue("@latitude", "latitude");
                    cmd.Parameters.AddWithValue("@longitude", "longitude");
                    cmd.Parameters.AddWithValue("@location", "location");
                    cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));
                    connection6.Open();
                    int result = cmd.ExecuteNonQuery();
                    connection6.Close();
                }
                return new HttpResponseMessage()
                {
                    Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                };
            }
        }

        public HttpResponseMessage SELFVKYC(HttpRequestMessage body)
        {

            string ts = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");//2016-01-11T14:44:48+05:30

            //Request Parameter
            string Source = "";
            string meetingID = "";
            string Password = "";
            string TxnID = "";
            string CustName = "";
            string MobileNo = "";
            string AgentMobileNo = "";
            string EmailID = "";
            string Userid = "";
            string ApplicationID = "";
            string VkycType = "";
            string VkycMode = "";
            string MeetingTime = "";
            string Ref1 = "";
            string Ref2 = "";
            string Ref3 = "";

            string SMS_RESP = "";
            string SMS_RESP1 = "";

            //  string ReqTs = "";
            string startdatetime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            string Enddatetime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");



            Dictionary<string, string> repdict = new Dictionary<string, string>();
            Dictionary<string, string> objgenmet = new Dictionary<string, string>();
            XDocument VKYCReq = new XDocument();

            string pstatus = "";
            string pToken = "";
            string pHost_url = "";
            string pcust_url = "";
            string Respxml = "";
            string responce = string.Empty;
            string ErrorCode = "";
            string ErrorMsg = "";
            string TransactionStatus = "";

            string CUST_URL = "";
            string MobileStatus = "";
            string EmailStatus = "";
            string EmaildError = "";
            string MobileError = "";
            string exception = "";
            string CPO_URL = "";
            string refID = "";
            string MeetingID = "";
            string Passcode = "";
            string msg = "";
            string msg1 = "";



            string EmailTxnID = "";

            try
            {
                ClsCustQuickEnrollment obj = new ClsCustQuickEnrollment();
                //string MoNo2 = Convert.ToString(TempData["CustMob"]);
                string MoNo = Convert.ToString(HttpContext.Session.GetString("CustMob1"));
                //var qdata = ObjIndo.USP_GetQEDetailsByMobNo(MoNo);
                var qdata = ObjIndo.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsByCustID {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                // var qdata = ObjIndo.USP_GetQEDetailsByCustID(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                obj.QEFirstName = qdata.CustFirstName;
                obj.QELastName = qdata.CustLastName;
                obj.QEEmailId = qdata.CustEmailId;
                obj.QEMobileNo = qdata.CustMobileNo;
                var name = string.Concat(qdata.CustFirstName, " ", qdata.CustLastName);
                HttpContext.Session.SetString("CustomerName", name);
                refID = GetRandomString();
                TempData["refID1"] = refID;
                /*  Session["refID1"]= */
                HttpContext.Session.SetString("refID1", refID);
                Passcode = GetRandomString1();
                var cutomerid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_InsertPasscode1", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Passcode", Passcode);
                    cmd.Parameters.AddWithValue("@CustomerDetailId", cutomerid);

                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    connection.Close();
                }
                string conn11 = _connectionString;
                using (SqlConnection connection11 = new SqlConnection(conn11))
                {
                    SqlCommand cmd11 = new SqlCommand("USP_InsertRequest", connection11);
                    cmd11.CommandType = CommandType.StoredProcedure;
                    cmd11.Parameters.AddWithValue("@CustId", cutomerid);
                    cmd11.Parameters.AddWithValue("@CustRequestFlag", "true");
                    cmd11.Parameters.AddWithValue("@CustReqTime", DateTime.Now);
                    cmd11.Parameters.AddWithValue("@LinkSendFlag", "false");
                    cmd11.Parameters.AddWithValue("@LinkSendTime", DateTime.Now);
                    cmd11.Parameters.AddWithValue("@ConnectionFlag", "false");
                    cmd11.Parameters.AddWithValue("@CustOtp", "null");
                    cmd11.Parameters.AddWithValue("@CustOtpVerifyFlag", "true");
                    cmd11.Parameters.AddWithValue("@VcipStatus", "");
                    cmd11.Parameters.AddWithValue("@LinkSendBy", "null");
                    cmd11.Parameters.AddWithValue("@AuthorizedPerson", "null");
                    cmd11.Parameters.AddWithValue("@Manualassignflag", "false");

                    connection11.Open();
                    int result = cmd11.ExecuteNonQuery();
                    connection11.Close();
                }

                string conn1 = _connectionString;
                using (SqlConnection connection1 = new SqlConnection(conn1))
                {
                    SqlCommand cmd = new SqlCommand("USP_InsertCallingKey", connection1);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Key", refID);
                    cmd.Parameters.AddWithValue("@CustomerDetailId", cutomerid);

                    connection1.Open();
                    int result = cmd.ExecuteNonQuery();
                    connection1.Close();
                }
                
                try
                {
                    responce = "\r\n<VkycRequest>\r\n<Source>CP1</Source>\r\n<Password>" + Passcode + "</Password>\r\n<TxnId>123</TxnId>\r\n<Userid>123</Userid>\r\n<ApplicationID>123</ApplicationID>\r\n<MobileNo>"+ qdata.CustMobileNo + "</MobileNo>\r\n<AgentMobileNo> "+ qdata.CustMobileNo + "</AgentMobileNo>\r\n<EmailID>"+ qdata.CustEmailId + "</EmailID>\r\n<CustomerName>"+ name + "</CustomerName>\r\n<VkycType>Assisted</VkycType>\r\n<VkycMode>self</VkycMode>\r\n<MeetingTime></MeetingTime>\r\n<Ref1></Ref1>\r\n<Ref2></Ref2>\r\n<Ref3></Ref3>\r\n</VkycRequest>";
                }
                catch (Exception ex)
                {
                    TransactionStatus = "UNSUCCESSFUL";
                    ErrorCode = "001";
                    ErrorMsg = "Error During Receving Request";
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);

                    exception = ex.Message + ex.InnerException + ex.Source + ex.HelpLink + ex.HResult + ex.TargetSite + ex.StackTrace;
                    string conn10 = _connectionString;
                    using (SqlConnection connection10 = new SqlConnection(conn10))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection10);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                        cmd.Parameters.AddWithValue("@PToken", pToken);
                        cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@latitude", "latitude");
                        cmd.Parameters.AddWithValue("@longitude", "longitude");
                        cmd.Parameters.AddWithValue("@location", "location");
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));

                        connection10.Open();
                        int result = cmd.ExecuteNonQuery();
                        connection10.Close();
                    }
                }

                #region ParseRequestXml

                try
                {


                    VKYCReq = XDocument.Parse(responce);

                    Source = VKYCReq.Root.Element("Source").Value;
                    Password = VKYCReq.Root.Element("Password").Value;
                    TxnID = VKYCReq.Root.Element("TxnId").Value;
                    Userid = VKYCReq.Root.Element("Userid").Value;
                    ApplicationID = VKYCReq.Root.Element("ApplicationID").Value;
                    //ReqTs = VKYCReq.Root.Element("DateTime").Value;
                    MobileNo = VKYCReq.Root.Element("MobileNo").Value;
                    AgentMobileNo = VKYCReq.Root.Element("AgentMobileNo").Value;
                    EmailID = VKYCReq.Root.Element("EmailID").Value;
                    CustName = VKYCReq.Root.Element("CustomerName").Value;
                    VkycType = VKYCReq.Root.Element("VkycType").Value;
                    VkycMode = VKYCReq.Root.Element("VkycMode").Value;
                    MeetingTime = VKYCReq.Root.Element("MeetingTime").Value;
                    Ref1 = VKYCReq.Root.Element("Ref1").Value;
                    Ref2 = VKYCReq.Root.Element("Ref2").Value;
                    Ref3 = VKYCReq.Root.Element("Ref3").Value;

                }
                catch (Exception ex)
                {

                    TransactionStatus = "UNSUCCESSFUL";

                    ErrorCode = "002";
                    ErrorMsg = "Error During Parsing RequestXml";
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);


                    exception = ex.Message + ex.InnerException + ex.Source + ex.HelpLink + ex.HResult + ex.TargetSite + ex.StackTrace;


                    //objSUD_VideoKycEntities.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ReqTs, ts, MobileNo, EmailID, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, doneby, VkycUrl, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml);

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };

                }

                #endregion ParseRequestXml

                #region Validation

                #region onlyMobile

                if (MobileNo == null || MobileNo == "")
                {
                    TransactionStatus = "UNSUCCESSFUL";
                    ErrorCode = "008";
                    MobileError = "Mobile Number Required";
                    MobileStatus = "UNSUCCESSFUL";
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);

                    // objSUD_VideoKycEntities.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts,CustName, MobileNo, EmailID, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid,VkycMode, CUST_URL,pstatus, pToken,pHost_url, pcust_url,TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml);
                    //ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                    string conn9 = _connectionString;
                    using (SqlConnection connection9 = new SqlConnection(conn9))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection9);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                        cmd.Parameters.AddWithValue("@PToken", pToken);
                        cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@latitude", "latitude");
                        cmd.Parameters.AddWithValue("@longitude", "longitude");
                        cmd.Parameters.AddWithValue("@location", "location");
                        //cmd.Parameters.AddWithValue("@AuthorizedPerson", AuthorizedPerson);
                        //cmd.Parameters.AddWithValue("@AutoAssignDate", AutoAssignDate);
                        //cmd.Parameters.AddWithValue("@VcipStatus", VcipStatus);










                        connection9.Open();
                        int result = cmd.ExecuteNonQuery();
                        connection9.Close();
                    }
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };


                }


                else if ((MobileNo != "" || MobileNo != null) && (EmailID == "" || EmailID == null))
                {
                    Regex regexM = new Regex(@"^[0-9]{10}$");
                    Match matchM = regexM.Match(MobileNo);
                    if (matchM.Success)
                    {



                        if (_settings.SMS != "CosmosBank")
                        {
                            // SMSService.OTPClient objsms1 = new SMSService.OTPClient();
                            // SMS_RESP = objsms1.SendMessage(MobileNo, msg);
                        }
                        else if (_settings.SMS == "CosmosBank")
                        {
                            // CosmosSMSservice.Service1Client objSMS = new CosmosSMSservice.Service1Client();
                            Dictionary<string, string> objDictionarySMS = new Dictionary<string, string>();
                            // objDictionarySMS = objSMS.SMS_API(MobileNo, msg);
                            if (objDictionarySMS.ContainsKey("Success"))
                            {
                                SMS_RESP = objDictionarySMS["Success"];
                                SMS_RESP = "E001";
                            }
                            else
                            {
                                SMS_RESP = objDictionarySMS["Error"];
                            }
                        }


                        if (SMS_RESP.Contains("E001"))
                        {

                            #region meeting region

                            try
                            {


                                objgenmet = create_meeting1(CustName, Userid, startdatetime, Enddatetime, MobileNo, EmailID, Password);

                                if (objgenmet["Status"] == "SUCCESS")
                                {
                                    MeetingID = objgenmet["MeetingID"];
                                    //MeetingID = "4b9012ee - f7a0 - 4395 - 7e35 - 08dace09ff5e";
                                }
                            }
                            catch
                            {
                            }

                            //objgenmet = GetToken(CustName, MobileNo, EmailID);

                            //if (objgenmet["Status"] == "SUCCESS")
                            //{
                            //    pcust_url = objgenmet["CustLink"];
                            //    pHost_url = objgenmet["HostLink"];
                            //    pToken = objgenmet["Token"];
                            //}

                            #endregion meeting region

                            TransactionStatus = "PENDING";
                            MobileStatus = "SUCCESSFUL";

                        }
                        else
                        {
                            TransactionStatus = "UNSUCCESSFUL";
                            ErrorCode = "010";
                            MobileStatus = "UNSUCCESSFUL";
                            ErrorMsg = SMS_RESP;
                        }

                    }

                    else
                    {
                        TransactionStatus = "UNSUCCESSFUL";
                        MobileStatus = "UNSUCCESSFUL";
                        ErrorCode = "006";
                        MobileError = "Invalid Mobile Number";

                    }
                    Respxml = SelfVkycResponseXmlBoth(SMS_RESP, TransactionStatus, ts, TxnID, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, CUST_URL, EmailTxnID, Ref1, Ref2, Ref3);

                    //ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                    string conn3 = _connectionString;
                    using (SqlConnection connection3 = new SqlConnection(conn3))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection3);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                        cmd.Parameters.AddWithValue("@PToken", pToken);
                        cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@latitude", "latitude");
                        cmd.Parameters.AddWithValue("@longitude", "longitude");
                        cmd.Parameters.AddWithValue("@location", "location");
                        //cmd.Parameters.AddWithValue("@AuthorizedPerson", AuthorizedPerson);
                        //cmd.Parameters.AddWithValue("@AutoAssignDate", AutoAssignDate);
                        //cmd.Parameters.AddWithValue("@VcipStatus", VcipStatus);










                        connection3.Open();
                        int result = cmd.ExecuteNonQuery();
                        connection3.Close();
                    }
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };



                }


                #endregion onlyMobile


                #region MobileWithEmail
                else if ((MobileNo != "" || MobileNo != null) && (EmailID != "" || EmailID != null))
                {
                    Regex regexM = new Regex(@"^[0-9]{10}$");
                    Match matchM = regexM.Match(MobileNo);
                    Match matchM1 = regexM.Match(AgentMobileNo);

                    Regex regexE = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match matchE = regexE.Match(EmailID);

                    if (matchM.Success && matchE.Success)
                    {
                        TransactionStatus = "PENDING";

                        if (_settings.SMS != "CosmosBank")
                        {
                            // SMSService.OTPClient objsms1 = new SMSService.OTPClient();
                            //SMS_RESP = objsms1.SendMessage(MobileNo, msg);
                            //SMS_RESP1 = objsms1.SendMessage(AgentMobileNo, msg);
                            SMS_RESP = "SUCCESSFUL";
                            MobileStatus = "SUCCESSFUL";
                        }
                        else if (_settings.SMS == "CosmosBank")
                        {
                            // CosmosSMSservice.Service1Client objSMS = new CosmosSMSservice.Service1Client();
                            Dictionary<string, string> objDictionarySMS = new Dictionary<string, string>();
                            // objDictionarySMS = objSMS.SMS_API(MobileNo, msg);
                            if (objDictionarySMS.ContainsKey("Success"))
                            {
                                SMS_RESP = objDictionarySMS["Success"];
                                SMS_RESP = "E001";
                            }
                            else
                            {
                                SMS_RESP = objDictionarySMS["Error"];
                            }
                        }
                        //if (SMS_RESP.Contains("E001"))
                        //{

                        //    MobileStatus = "SUCCESSFUL";



                        //}
                        //else
                        //{

                        //    ErrorCode = "010";
                        //    MobileError = SMS_RESP;
                        //    MobileStatus = "UNSUCCESSFUL";
                        //}



                        #region SendEmail


                        string respemail = SendEmail(EmailID, msg);
                        if (respemail == "SUCCESS")
                        {
                            EmailStatus = "SUCCESSFUL";


                        }
                        else
                        {

                            ErrorCode = "010";
                            EmaildError = "Unable to Send Email";
                            EmailStatus = "UNSUCCESSFUL";
                        }

                        #endregion  SendEmail

                        if (EmailStatus == "SUCCESSFUL" && MobileStatus == "SUCCESSFUL")
                        {


                            #region meeting 
                            try
                            {


                                objgenmet = create_meeting1(CustName, Userid, startdatetime, Enddatetime, MobileNo, EmailID, Password);

                                if (objgenmet["Status"] == "SUCCESS")
                                {
                                    MeetingID = objgenmet["MeetingID"];
                                    // MeetingID = "ae067ab9-be96-4b20-2fb7-08dabe274b93";
                                }
                            }
                            catch
                            {
                            }
                            //objgenmet = GetToken(CustName, MobileNo, EmailID);

                            //if (objgenmet["Status"] == "SUCCESS")
                            //{
                            //    pcust_url = objgenmet["CustLink"];
                            //    pHost_url = objgenmet["HostLink"];
                            //    pToken = objgenmet["Token"];
                            //}

                            #endregion meeting
                            TransactionStatus = "PENDING";

                        }
                        else
                        {
                            TransactionStatus = "UNSUCCESSFUL";

                        }
                    }

                    else if (matchM.Success)
                    {
                        TransactionStatus = "UNSUCCESSFUL";

                        EmailStatus = "UNSUCCESSFUL";
                        EmaildError = "Invalid Email ID";

                        if (_settings.SMS != "CosmosBank")
                        {
                            // SMSService.OTPClient objsms1 = new SMSService.OTPClient();
                            //SMS_RESP = objsms1.SendMessage(MobileNo, msg);
                        }
                        else if (_settings.SMS == "CosmosBank")
                        {
                            // CosmosSMSservice.Service1Client objSMS = new CosmosSMSservice.Service1Client();
                            Dictionary<string, string> objDictionarySMS = new Dictionary<string, string>();
                            //objDictionarySMS = objSMS.SMS_API(MobileNo, msg);
                            if (objDictionarySMS.ContainsKey("Success"))
                            {
                                SMS_RESP = objDictionarySMS["Success"];
                                SMS_RESP = "E001";
                            }
                            else
                            {
                                SMS_RESP = objDictionarySMS["Error"];
                            }
                        }
                        if (SMS_RESP.Contains("E001"))
                        {

                            MobileStatus = "SUCCESSFUL";



                            #region meeeting

                            try
                            {

                                objgenmet = create_meeting(CustName, Userid, startdatetime, Enddatetime, MobileNo, EmailID, Password);

                                if (objgenmet["Status"] == "SUCCESS")
                                {
                                    MeetingID = objgenmet["MeetingID"];
                                }
                            }
                            catch
                            {
                            }

                            //objgenmet = GetToken(CustName, MobileNo, EmailID);

                            //if (objgenmet["Status"] == "SUCCESS")
                            //{
                            //    pcust_url = objgenmet["CustLink"];
                            //    pHost_url = objgenmet["HostLink"];
                            //    pToken = objgenmet["Token"];
                            //}
                            #endregion meeeting
                        }
                        else
                        {

                            ErrorCode = "010";
                            MobileError = SMS_RESP;
                            MobileStatus = "UNSUCCESSFUL";
                        }



                    }

                    else if (matchE.Success)
                    {
                        TransactionStatus = "UNSUCCESSFUL";
                        MobileStatus = "UNSUCCESSFUL";

                        MobileError = "Invalid Mobile No";
                        #region SendEmail


                        string respemail = SendEmail(EmailID, msg);
                        if (respemail == "SUCCESS")
                        {

                            EmailStatus = "SUCCESSFUL";
                            #region  meeting


                            try
                            {

                                objgenmet = create_meeting(CustName, Userid, startdatetime, Enddatetime, MobileNo, EmailID, Password);

                                if (objgenmet["Status"] == "SUCCESS")
                                {
                                    MeetingID = objgenmet["MeetingID"];
                                }
                            }
                            catch
                            {
                            }
                            //objgenmet = GetToken(CustName, MobileNo, EmailID);

                            //if (objgenmet["Status"] == "SUCCESS")
                            //{
                            //    pcust_url = objgenmet["CustLink"];
                            //    pHost_url = objgenmet["HostLink"];
                            //    pToken = objgenmet["Token"];
                            //}
                            #endregion meeting
                        }
                        else
                        {

                            ErrorCode = "010";
                            EmaildError = "Unable to Send Email";
                            EmailStatus = "UNSUCCESSFUL";
                        }

                        #endregion  SendEmail

                    }


                    else
                    {
                        TransactionStatus = "UNSUCCESSFUL";
                        MobileStatus = "UNSUCCESSFUL";
                        EmailStatus = "UNSUCCESSFUL";
                        MobileError = "Invalid Mobile No";
                        EmaildError = "Invalid Email ID";
                        CUST_URL = "";
                    }

                    Respxml = SelfVkycResponseXmlBoth(SMS_RESP, TransactionStatus, ts, TxnID, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, CUST_URL, EmailTxnID, Ref1, Ref2, Ref3);

                    //ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                    string conn4 = _connectionString;
                    using (SqlConnection connection4 = new SqlConnection(conn4))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection4);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        //cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        // cmd.Parameters.AddWithValue("@Pstatus", "pstatus");
                        //cmd.Parameters.AddWithValue("@PToken", pToken);
                        //cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        //cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "PENDING");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "PENDING");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        //cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        //cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        //cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        //cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));

                        //cmd.Parameters.AddWithValue("@latitude", "latitude");
                        // cmd.Parameters.AddWithValue("@longitude", "longitude");
                        //cmd.Parameters.AddWithValue("@location", "location");
                        //cmd.Parameters.AddWithValue("@AuthorizedPerson", AuthorizedPerson);
                        //cmd.Parameters.AddWithValue("@AutoAssignDate", AutoAssignDate);
                        //cmd.Parameters.AddWithValue("@VcipStatus", VcipStatus);










                        connection4.Open();
                        int result = cmd.ExecuteNonQuery();
                        //    SqlDataReader reader2 = cmd.ExecuteReader();
                        //    if (reader2.Read())
                        //    {

                        //        var Result1 = reader2["RESULT"].ToString();
                        //    }
                        connection4.Close();
                    }

                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };


                }


                else
                {
                    TransactionStatus = "UNSUCCESSFUL";
                    ErrorMsg = "Invalid Input";
                    Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, CPO_URL, CUST_URL, ErrorMsg);

                    // ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                    string conn5 = _connectionString;
                    using (SqlConnection connection5 = new SqlConnection(conn5))
                    {
                        SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection5);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Source", Source);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@TxnId", TxnID);
                        cmd.Parameters.AddWithValue("@Ts", ts);
                        cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        cmd.Parameters.AddWithValue("@CustomerName", CustName);
                        cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                        cmd.Parameters.AddWithValue("@EmailID", EmailID);
                        cmd.Parameters.AddWithValue("@Ref1", Ref1);
                        cmd.Parameters.AddWithValue("@Ref2", Ref2);
                        cmd.Parameters.AddWithValue("@Ref3", Ref3);
                        cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                        cmd.Parameters.AddWithValue("@MobileError", MobileError);
                        cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                        cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                        cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                        cmd.Parameters.AddWithValue("@userid", Userid);
                        cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                        cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                        cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                        cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                        cmd.Parameters.AddWithValue("@PToken", pToken);
                        cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                        cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                        cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                        cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                        cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                        cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                        cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                        cmd.Parameters.AddWithValue("@Exception", exception);
                        cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                        cmd.Parameters.AddWithValue("@RespXml", Respxml);
                        //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                        cmd.Parameters.AddWithValue("@latitude", "latitude");
                        cmd.Parameters.AddWithValue("@longitude", "longitude");
                        cmd.Parameters.AddWithValue("@location", "location");
                        //cmd.Parameters.AddWithValue("@AuthorizedPerson", AuthorizedPerson);
                        //cmd.Parameters.AddWithValue("@AutoAssignDate", AutoAssignDate);
                        //cmd.Parameters.AddWithValue("@VcipStatus", VcipStatus);










                        connection5.Open();
                        int result = cmd.ExecuteNonQuery();
                        connection5.Close();
                    }
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                    };
                }


                #endregion MobileWithEmail


                #endregion Validation


            }
            catch (Exception ex)
            {
                TransactionStatus = "UNSUCCESSFUL";
                ErrorCode = "004";
                ErrorMsg = "Some Error Occured";
                Respxml = VkycResponseXmlOnlyMobile(SMS_RESP, TransactionStatus, ts, TxnID, Ref1, Ref2, Ref3, "", "", ErrorMsg);


                exception = ex.Message + ex.InnerException + ex.Source + ex.HelpLink + ex.HResult + ex.TargetSite + ex.StackTrace;
                // ObjIndo.USP_INSERT_CustLinkReq(Source, Password, refID, TxnID, ts, ApplicationID, CustName, MobileNo, EmailID, Ref1, Ref2, Ref3, MobileStatus, MobileError, EmailStatus, EmaildError, CPO_URL, Userid, VkycMode, CUST_URL, pstatus, pToken, pHost_url, pcust_url, TransactionStatus, "PENDING", "PENDING", ErrorCode, ErrorMsg, exception, responce, Respxml, MeetingID);
                string conn6 = _connectionString;
                using (SqlConnection connection6 = new SqlConnection(conn6))
                {
                    SqlCommand cmd = new SqlCommand("USP_INSERT_CustLinkReq", connection6);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Source", Source);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@refId", refID);
                    cmd.Parameters.AddWithValue("@TxnId", TxnID);
                    cmd.Parameters.AddWithValue("@Ts", ts);
                    cmd.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                    cmd.Parameters.AddWithValue("@CustomerName", CustName);
                    cmd.Parameters.AddWithValue("@MobileNo", MobileNo);
                    cmd.Parameters.AddWithValue("@EmailID", EmailID);
                    cmd.Parameters.AddWithValue("@Ref1", Ref1);
                    cmd.Parameters.AddWithValue("@Ref2", Ref2);
                    cmd.Parameters.AddWithValue("@Ref3", Ref3);
                    cmd.Parameters.AddWithValue("@MobileStatus", MobileStatus);
                    cmd.Parameters.AddWithValue("@MobileError", MobileError);
                    cmd.Parameters.AddWithValue("@EmailStatus", EmailStatus);
                    cmd.Parameters.AddWithValue("@EmailError", EmaildError);
                    cmd.Parameters.AddWithValue("@CPO_URL", CPO_URL);
                    cmd.Parameters.AddWithValue("@userid", Userid);
                    cmd.Parameters.AddWithValue("@VkycMode", VkycMode);
                    cmd.Parameters.AddWithValue("@CUST_URL", CUST_URL);
                    cmd.Parameters.AddWithValue("@URLStatus", "URLStatus");
                    cmd.Parameters.AddWithValue("@Pstatus", pstatus);
                    cmd.Parameters.AddWithValue("@PToken", pToken);
                    cmd.Parameters.AddWithValue("@PHost_Url", pHost_url);
                    cmd.Parameters.AddWithValue("@PCust_Url", pcust_url);
                    cmd.Parameters.AddWithValue("@VkycStatus", "VkycStatus");
                    cmd.Parameters.AddWithValue("@OtpVerfication", "OtpVerfication");
                    cmd.Parameters.AddWithValue("@TransactionStatus", TransactionStatus);
                    cmd.Parameters.AddWithValue("@ErrorCode", ErrorCode);
                    cmd.Parameters.AddWithValue("@ErrorMsg", ErrorMsg);
                    cmd.Parameters.AddWithValue("@Exception", exception);
                    cmd.Parameters.AddWithValue("@RequestXml", "RequestXml");
                    cmd.Parameters.AddWithValue("@RespXml", Respxml);
                    //cmd.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                    cmd.Parameters.AddWithValue("@MeetingID", MeetingID);
                    cmd.Parameters.AddWithValue("@latitude", "latitude");
                    cmd.Parameters.AddWithValue("@longitude", "longitude");
                    cmd.Parameters.AddWithValue("@location", "location");
                    cmd.Parameters.AddWithValue("@CustomerID", HttpContext.Session.GetString("PersonalId"));
                    connection6.Open();
                    int result = cmd.ExecuteNonQuery();
                    connection6.Close();
                }
                return new HttpResponseMessage()
                {
                    Content = new StringContent(Respxml, Encoding.UTF8, "application/xml")
                };
            }
        }
        public string VkycResponseXmlOnlyMobile(string SMS_RESP, string status, string ts, string TxnID, string Ref1, string Ref2, string Ref3, string CPO_URL, string Cust_URL, string ErrorMessage)
        {
            XDocument VkycResponseXml = new XDocument();
            if (status == "UNSUCEESSFUL")
            {
                VkycResponseXml = new XDocument(
                    new XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("VKYCResponse",


                          new XElement("ts", ts),
                            new XElement("TxnID", TxnID),
                             new XElement("Ref1", Ref1),
                            new XElement("Ref2", Ref2),
                              new XElement("Ref3", Ref3),
                           new XElement("CPO_URL", CPO_URL),
                          new XElement("Cust_URL", Cust_URL),
                          new XElement("MobileStatus", status),

                          new XElement("MobileError", ErrorMessage),
                           new XElement("SMS_RESP", SMS_RESP)
                                ));
            }
            else
            {
                VkycResponseXml = new XDocument(
                     new XDeclaration("1.0", "UTF-8", "yes"),
                         new XElement("VKYCResponse",


                            new XElement("ts", ts),
                            new XElement("TxnID", TxnID),
                             new XElement("Ref1", Ref1),
                            new XElement("Ref2", Ref2),
                              new XElement("Ref3", Ref3),
                             new XElement("CPO_URL", CPO_URL),
                               new XElement("Cust_URL", Cust_URL),
                               new XElement("SMS_RESP", SMS_RESP),
                           new XElement("MobileStatus", status)


                                 ));

            }

            string Respxml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" + VkycResponseXml.ToString();
            return Respxml;

        }


        public string VkycResponseXmlBoth(string SMS_RESP, string status, string ts, string TxnID, string MobileStatus, string MobileError, string EmailStatus, string EmailError, string CPO_URL, string Cust_URL, string EmailTxnID, string Ref1, string Ref2, string Ref3)
        {
            XDocument VkycResponseXml = new XDocument();
            if (status == "UNSUCCESSFUL")
            {
                VkycResponseXml = new XDocument(
                    new XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("VKYCResponse",


                           new XElement("ts", ts),
                           new XElement("TxnID", TxnID),
                            new XElement("Ref1", Ref1),
                            new XElement("Ref2", Ref2),
                              new XElement("Ref3", Ref3),

                           new XElement("CPO_URL", CPO_URL),
                           new XElement("Cust_URL", Cust_URL),
                            //  new XElement("EmailTxnID", EmailTxnID),

                            new XElement("MobileStatus", MobileStatus),
                           new XElement("MobileError", MobileError),
                            new XElement("SMS_RESP", SMS_RESP),
                                 new XElement("EmailStatus", EmailStatus),
                                 new XElement("EmailError", EmailError)

                                ));
            }
            else
            {
                VkycResponseXml = new XDocument(
                     new XDeclaration("1.0", "UTF-8", "yes"),
                         new XElement("VKYCResponse",
                         new XElement("ts", ts),
                            new XElement("TxnID", TxnID),
                             new XElement("Ref1", Ref1),
                            new XElement("Ref2", Ref2),
                              new XElement("Ref3", Ref3),
                          new XElement("CPO_URL", CPO_URL),
                            new XElement("Cust_URL", Cust_URL),
                              // new XElement("EmailTxnID", EmailTxnID),


                              new XElement("MobileStatus", MobileStatus),
                               new XElement("SMS_RESP", SMS_RESP),
                             new XElement("EmailStatus", EmailStatus)



                                 ));

            }

            string Respxml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" + VkycResponseXml.ToString();
            return Respxml;

        }




        public string SelfVkycResponseXmlBoth(string SMS_RESP, string status, string ts, string TxnID, string MobileStatus, string MobileError, string EmailStatus, string EmailError, string CPO_URL, string Cust_URL, string EmailTxnID, string Ref1, string Ref2, string Ref3)
        {
            XDocument VkycResponseXml = new XDocument();
            if (status == "UNSUCCESSFUL")
            {
                VkycResponseXml = new XDocument(
                    new XDeclaration("1.0", "UTF-8", "yes"),
                        new XElement("VKYCResponse",


                           new XElement("ts", ts),
                           new XElement("TxnID", TxnID),
                            new XElement("Ref1", Ref1),
                            new XElement("Ref2", Ref2),
                              new XElement("Ref3", Ref3),
                            new XElement("MobileStatus", MobileStatus),
                           new XElement("MobileError", MobileError),
                                 // new XElement("SMS_RESP", SMS_RESP),
                                 new XElement("EmailStatus", EmailStatus),
                                 new XElement("EmailError", EmailError)

                                ));
            }
            else
            {
                VkycResponseXml = new XDocument(
                     new XDeclaration("1.0", "UTF-8", "yes"),
                         new XElement("VKYCResponse",
                         new XElement("ts", ts),
                            new XElement("TxnID", TxnID),
                             new XElement("Ref1", Ref1),
                            new XElement("Ref2", Ref2),
                              new XElement("Ref3", Ref3),
                              new XElement("MobileStatus", MobileStatus),
                             // new XElement("SMS_RESP", SMS_RESP),
                             new XElement("EmailStatus", EmailStatus)



                                 ));

            }

            string Respxml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>\r\n" + VkycResponseXml.ToString();
            return Respxml;

        }


        static string GetRandomString()
        {
            int length = 10;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

            string s = "";

            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                while (s.Length != length)
                {
                    byte[] oneByte = new byte[1];
                    provider.GetBytes(oneByte);
                    char character = (char)oneByte[0];
                    if (valid.Contains(character))
                    {
                        s += character;
                    }
                }
            }
            return s;
        }

        static string GetRandomString1()
        {
            int length = 4;
            const string valid = "1234567890";

            string s = "";
            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                while (s.Length != length)
                {
                    byte[] oneByte = new byte[1];
                    provider.GetBytes(oneByte);
                    char character = (char)oneByte[0];
                    if (valid.Contains(character))
                    {
                        s += character;
                    }
                }
            }
            return s;
        }

        public ActionResult Randomstring()
        {
            string CustName = HttpContext.Session.GetString("CustomerName");
            int randomNumber = new Random().Next(1000, 9999);
            //string English = Convert.ToString(randomNumber);
            // string lang = "English";
            //ViewBag.Random = randomNumber;
            //ViewBag.Random

            string conn20 = _connectionString;
            using (SqlConnection connection = new SqlConnection(conn20))
            {
                SqlCommand cmd = new SqlCommand("USP_InsertRandomNumberAndName", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RandomNumber", randomNumber);
                cmd.Parameters.AddWithValue("@CustName", CustName);
                cmd.Parameters.AddWithValue("@CustomerId", HttpContext.Session.GetString("PersonalId"));

                connection.Open();
                int result = cmd.ExecuteNonQuery();
                connection.Close();
            }
            return Json(randomNumber + "," + CustName);
        }


        public string SendEmail(string EmailId, string msg)
        {
            return "SUCCESS";
        }
        //  [HttpGet]
        // [Route("api/VKYC/GetToken")]

        #region Pichain
        public Dictionary<string, string> GetToken(string name, string MobileNo, string EmailID)
        {

            string Token = "";
            Dictionary<string, string> genmetResp = new Dictionary<string, string>();

            var client = new RestClient("https://backend-videopd.pichainlabs.com/api/auth/get_user_auth_token/");
            var request = new RestRequest(Method.POST);

            var GetToken = new
            {
                username = "alpha_finsoft",
                password = "xAQyqMQH+afvY7R}",
                fcm_token = "token"
            };

            string GetTokenJson = JsonConvert.SerializeObject(GetToken);

            request.AddHeader("postman-token", "8fb53c34-dd71-c8fe-0fcb-7dddacc3426b");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", GetTokenJson, ParameterType.RequestBody);

            //  request.AddParameter("application/json", "{\r\n    \"username\":\"alpha_finsoft\",\r\n    \"password\":\"xAQyqMQH+afvY7R}\",\r\n    \"fcm_token\":\"token\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            var PResParam = JsonConvert.DeserializeObject<clsGetToken>(response.Content);
            if (PResParam.resp_code == "200")
            {
                Token = PResParam.resp_msg.rest_token;
                genmetResp = generate_meeting(name, MobileNo, EmailID, Token);


            }
            else
            {
                Token = "";
                genmetResp.Add("Token", "");
                genmetResp.Add("Status", "ERROR");


            }

            return genmetResp;
        }


        public Dictionary<string, string> generate_meeting(string name, string MobileNo, string EmailID, string Token)
        {
            Dictionary<string, string> objDictionary = new Dictionary<string, string>();
            string CustLink = "";

            string HostLink = "";

            try
            {

                var client = new RestClient("https://backend-videopd.pichainlabs.com/api/conferences/generate_meeting/");
                var request = new RestRequest(Method.POST);

                Generatemetting clsgeneratemetting = new Generatemetting();

                clsgeneratemetting.title = "Meeting with " + name;
                clsgeneratemetting.is_instant = true;
                clsgeneratemetting.send_notification = false;
                clsgeneratemetting.consent_message = "hi";
                clsgeneratemetting.category = 2;
                clsgeneratemetting.notes = "Video Verification";

                users_list objuse = new users_list();
                objuse.name = name;
                objuse.email = EmailID;
                objuse.mobile = MobileNo;

                clsgeneratemetting.users_list.Add(objuse);



                string generatemeetingJson = JsonConvert.SerializeObject(clsgeneratemetting);


                request.AddHeader("authorization", "JWT " + Token);
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", generatemeetingJson, ParameterType.RequestBody);

                Meeting_Participants MpObj = new Meeting_Participants();
                MeetingResponse Mobj = new MeetingResponse();
                IRestResponse response = client.Execute(request);


                JObject MeetRes = JObject.Parse(response.Content);
                Mobj = JsonConvert.DeserializeObject<MeetingResponse>(MeetRes.ToString());
                var getMeet = MeetRes["meeting_participants"][MobileNo];
                MpObj = JsonConvert.DeserializeObject<Meeting_Participants>(getMeet.ToString());
                Mobj.UserDetails = MpObj;
                CustLink = Mobj.UserDetails.url;

                HostLink = Mobj.host_meeting_link + Token;

                if (CustLink != null && HostLink != null)
                {
                    objDictionary.Add("Status", "SUCCESS");
                    objDictionary.Add("Token", Token);
                    objDictionary.Add("HostLink", HostLink);
                    objDictionary.Add("CustLink", CustLink);

                }
                else
                {
                    objDictionary.Add("Status", "ERROR");

                }
                return objDictionary;
            }
            catch (Exception ex)
            {
                string exception = ex.Message + ex.InnerException + ex.InnerException + ex.HelpLink;
                objDictionary.Add("Status", "ERROR");
                return objDictionary;
            }

        }

        #endregion Pichain


        #region VKYC API

        public Dictionary<string, string> create_meeting(string attendees, string host, string StartDateTime, string EndDateTime, string MobileNo, string EmailID, string Password)
        {
            string filename1 = "";
            Dictionary<string, string> objDictionary = new Dictionary<string, string>();
            string meetingTitle = "VKYC for SUD Customer";
            string meetingId = string.Empty;
            string meetingDescription = "Video Verification";
            var new_videocall_back = "https://rssbactopen.silsaas.co.in//CPO/GetFile?";
            var new_snapshotcall_back = "https://rssbactopen.silsaas.co.in//CPO/GetSnapshot?";
            //string VideoCallbackURL = ConfigurationManager.AppSettings["VideoCallbackURL"];
            //string SnapshotCallbackURL = ConfigurationManager.AppSettings["SnapshotCallbackURL"];

            string VideoCallbackURL = new_videocall_back;
            string SnapshotCallbackURL = new_snapshotcall_back;

            try
            {
                filename1 = _settings.Document + "error.txt";
                System.IO.File.AppendAllText(filename1, "GetFile called");
                var client = new RestClient("https://api.indofinnet.com/api/meeting");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                //client.Proxy = new WebProxy("http://proxy.sud.in:8080",true);

                var meeting = new
                {
                    attendees = attendees,
                    host = host,
                    StartDateTime = StartDateTime,
                    EndDateTime = EndDateTime,
                    meetingTitle = meetingTitle,
                    meetingDescription = meetingDescription,
                    IsPrivate = true,
                    PassCode = Password,
                    hostPassCode = Password,
                    Questions = "[\"Speak Your Name?\", \"Show Original ID Proof?\", \"Show Original Address Proof?\"]",

                    VideoCallbackURL = VideoCallbackURL,
                    SnapshotCallbackURL = SnapshotCallbackURL,
                    isOneWay = false
                };


                System.IO.File.AppendAllText(filename1, "callback url call");
                //string req="{"+
                //                    "attendees: "+attendees +,
                //    //    "host": "mansi.mehta@hirdhav.com",
                //    //    "StartDateTime": "2021-07-15T11:46:00",
                //    //    "EndDateTime": "2021-07-15T11:46:00",
                //    //    "meetingTitle": "VKYC for ABC Customer",
                //    //    "meetingDescription": "",
                //    //    "IsPrivate": true,
                //    //    "PassCode": "123456",
                //    //    "hostPassCode": "999999"
                //    //}




                var Meetingjson = JsonConvert.SerializeObject(meeting);

                //request.AddHeader("postman-token", "8fb53c34-dd71-c8fe-0fcb-7dddacc3426b");
                ///request.AddHeader("cache-control", "no-cache");
                request.AddParameter("application/json", Meetingjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                try
                {
                    var resp = JsonConvert.DeserializeObject<MeetingRes>(response.Content.ToString());
                    meetingId = resp.meetingId;
                    //// meetingId = "ae067ab9-be96-4b20-2fb7-08dabe274b93";
                    objDictionary.Add("Status", "SUCCESS");
                    objDictionary.Add("MeetingID", meetingId);
                }
                catch
                {
                    objDictionary.Add("Status", "ERROR");

                }

                return objDictionary;
            }

            catch (Exception ex)
            {
                string exception = ex.Message + ex.InnerException + ex.InnerException + ex.HelpLink;
                objDictionary.Add("Status", "ERROR");
                System.IO.File.AppendAllText(filename1, "EXCEPTION callback url call");
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                System.IO.File.AppendAllText(filename1, errormsg);
                //System.IO.File.WriteAllText("",exce)
                return objDictionary;
            }

        }


        public Dictionary<string, string> create_meeting1(string attendees, string host, string StartDateTime, string EndDateTime, string MobileNo, string EmailID, string Password)
        {
            string filename1 = "";
            Dictionary<string, string> objDictionary = new Dictionary<string, string>();
            string meetingTitle = "VKYC for SUD Customer";
            string meetingId = string.Empty;
            string meetingDescription = "Video Verification";
            var selfnew_videocall_back = "https:\\indoesign.in\\CPO\\SelfGetFile";
            // var new_snapshotcall_back = "https:\\indoesign.in\\CPO\\GetSnapshot";
            //string VideoCallbackURL = ConfigurationManager.AppSettings["VideoCallbackURL"];
            //string SnapshotCallbackURL = ConfigurationManager.AppSettings["SnapshotCallbackURL"];

            string VideoCallbackURL = selfnew_videocall_back;
            // string SnapshotCallbackURL = new_snapshotcall_back;

            try
            {
                filename1 = _settings.Document + "error.txt";
                System.IO.File.AppendAllText(filename1, "GetFile called");
                var client = new RestClient("https://api.indofinnet.com/api/meeting");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                //client.Proxy = new WebProxy("http://proxy.sud.in:8080",true);

                var meeting = new
                {
                    attendees = attendees,
                    host = host,
                    StartDateTime = StartDateTime,
                    EndDateTime = EndDateTime,
                    meetingTitle = meetingTitle,
                    meetingDescription = meetingDescription,
                    IsPrivate = false,
                    PassCode = Password,
                    hostPassCode = Password,
                    Questions = "[\"Speak Your Name?\", \"Show Original ID Proof?\", \"Show Original Address Proof?\"]",

                    VideoCallbackURL = VideoCallbackURL,
                    // SnapshotCallbackURL = SnapshotCallbackURL,
                    isOneWay = true
                };


                System.IO.File.AppendAllText(filename1, "callback url call");
                //string req="{"+
                //                    "attendees: "+attendees +,
                //    //    "host": "mansi.mehta@hirdhav.com",
                //    //    "StartDateTime": "2021-07-15T11:46:00",
                //    //    "EndDateTime": "2021-07-15T11:46:00",
                //    //    "meetingTitle": "VKYC for ABC Customer",
                //    //    "meetingDescription": "",
                //    //    "IsPrivate": true,
                //    //    "PassCode": "123456",
                //    //    "hostPassCode": "999999"
                //    //}




                var Meetingjson = JsonConvert.SerializeObject(meeting);

                //request.AddHeader("postman-token", "8fb53c34-dd71-c8fe-0fcb-7dddacc3426b");
                ///request.AddHeader("cache-control", "no-cache");
                request.AddParameter("application/json", Meetingjson, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);


                try
                {
                    var resp = JsonConvert.DeserializeObject<MeetingRes>(response.Content.ToString());
                    meetingId = resp.meetingId;
                    //// meetingId = "ae067ab9-be96-4b20-2fb7-08dabe274b93";
                    objDictionary.Add("Status", "SUCCESS");
                    objDictionary.Add("MeetingID", meetingId);
                }
                catch
                {
                    objDictionary.Add("Status", "ERROR");

                }

                return objDictionary;
            }

            catch (Exception ex)
            {
                string exception = ex.Message + ex.InnerException + ex.InnerException + ex.HelpLink;
                objDictionary.Add("Status", "ERROR");
                System.IO.File.AppendAllText(filename1, "EXCEPTION callback url call");
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                System.IO.File.AppendAllText(filename1, errormsg);
                //System.IO.File.WriteAllText("",exce)
                return objDictionary;
            }

        }


        public ActionResult Custom()
        {

            if (HttpContext.Session.GetString("CustomerType") != "CustomerLogin")
            {
                return RedirectToAction("CPOVKYC", "NEWVCIP");


            }
            else
            {
                return RedirectToAction("CustVKYC1", "CUSTOMEROTP");
            }
        }




        #endregion VKYC API

        public ActionResult SelfCustom()
        {

            return RedirectToAction("ONEWAYCPOVKYC", "NEWVCIP");
        }

        public ActionResult VKYCSMS(string MobileNumber, string OrgID, string LinkTocustomer, string MeetingCode, string CustomerName)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {




                //var client = new RestClient("https://cbs.indofinnet.com/api/SMSVKYC?Tomobile=" + MobileNumber + "&OrgID=IndoFin007&LinkTocustomer=" + LinkTocustomer + "&MeetingCode=" + MeetingCode + "&customerName=" + CustomerName + "&bankname=RSSB");


                ////var client = new RestClient("https://apigateway.indofinnet.com/api/SMSOTP?OrgID=IndoFin007&Tomobile=" + MobileNumber);
                //client.Timeout = -1;
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                //IRestResponse response = client.Execute(request);
                //string res = response.Content;

                //res = res.Replace(@"\", "");

                //string s = res.Split('"')[4];
                //return Json(s);

                string LinkToCust = "https://nmccactopen.silsaas.co.in"; 

                var client = new RestClient("https://cbs.indofinnet.com/api/SMSVKYC?Tomobile=" + MobileNumber + "&OrgID=IndoFin007&LinkTocustomer=" + LinkTocustomer + "&MeetingCode=" + MeetingCode + "&customerName=" + CustomerName + "&bankname=NMCC");

             
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;
           
                

                if (res.Contains("Successfully Sent"))
                {
                    return Json("Successfully Sent");
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
        }


    }
}
