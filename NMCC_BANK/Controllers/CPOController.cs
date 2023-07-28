using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using RestSharp;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using INDO_FIN_NET.Models.Organisation;
using INDO_FIN_NET.Repository;
using System.Configuration;
using System.Text;
using System.IO;
using System;
using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs.Specialized;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Models.VKYC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;

namespace INDO_FIN_NET.Controllers
{
    public class CPOController : Controller
    {
        CPO_model CM = new CPO_model();
        //databaseservice.Service1Client objdb = new databaseservice.Service1Client();
        //SMSService.OTPClient objsms = new SMSService.OTPClient();
        ClsCustIPVDetails objcustIpv = new ClsCustIPVDetails(); private readonly RSSBPRODDbCotext ObjIndo;
        private readonly INDO_FinNetDbCotext objData;
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        private readonly IConfiguration _config;
        private readonly IConfiguration configuration;
        private Appsettings _settings;
        private readonly string _connectionString;
        public CPOController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            ObjIndo = Context;
            objData = iNDO_;
            _settings = new Appsettings();
            configuration.GetSection("Appsettings").Bind(_settings);
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
        }

        string REFID = "";



        // GET: CPO
        public ActionResult Index()
        {
            return View();
        }

        // http://localhost:49970/CPO/CPO?RefID=nyzRmLlLLS

        //public ActionResult CPO(string keystring)
        //{
        //    string filename1 = "";
        //    try
        //    {
        //        //// filename1 = WebConfigurationManager.AppSettings["Document"] + "VKYCError.txt";

        //        //ViewBag.CUSTOMERLOGIN = Session["CUSTOMERLOGIN"].ToString();
        //        //ViewBag.CUSTOMERLOGIN = "CUSTOMERLOGIN";
        //        //// var result = ObjIndo.USP_GetCustDetails(RefID).FirstOrDefault();
        //        //DateTime URLCreatedDate = Convert.ToDateTime(result.CreatedDate);
        //        //DateTime CurrDate = DateTime.Now;
        //        //TimeSpan timeSub = CurrDate - URLCreatedDate;
        //        //var minuteget = timeSub.TotalHours;

        //        //if (minuteget < 24)
        //        //{
        //        Session["RefID"] = keystring;
        //        REFID = keystring;
        //        //CM.customerName = result.CustomerName;
        //        //CM.mobile = result.MobileNo;
        //        //CM.email_Id = result.EmailId;
        //        return View();

        //        // return RedirectToAction("CPOVKYC", "CPO");
        //        //}
        //        //else
        //        //{
        //        //    return View("Error");
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
        //        System.IO.File.AppendAllText(filename1, errormsg);
        //        PortalException.InsertPortalException(ex);
        //        return Json("Exception", JsonRequestBehavior.AllowGet);
        //    }
        //}




        [HttpGet]
        //public ActionResult GetOTP(string mobilenum, string emailidno, string AppName, string FlagMailOTP, string VerificationStatus, string OTPcount)
        //{

        //    string msg = "";

        //    string generatedotp;
        //    string refid = Convert.ToString(@Session["RefID"]);
        //    var result = objdb.USP_GetCustDetails(refid).FirstOrDefault();
        //    var DB_OTP = objdb.USP_GetOTPDetails(refid).FirstOrDefault();
        //    var otpbtnflag = Convert.ToInt16(OTPcount);
        //    Random objRandomOtp = new Random();
        //    if (otpbtnflag > 1)
        //    {
        //        DateTime OtpcrtDate = Convert.ToDateTime(DB_OTP.CreatedDate);
        //        DateTime CurrDate = DateTime.Now;
        //        TimeSpan timeSub = CurrDate - OtpcrtDate;
        //        var minuteget = timeSub.TotalMinutes;
        //        if (minuteget > 3)
        //        {
        //            generatedotp = objRandomOtp.Next(1000, 9999).ToString();
        //        }
        //        else
        //        {
        //            generatedotp = DB_OTP.OTP;
        //        }
        //        string TimeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
        //        var insertotp = objdb.USP_InsertUpdateOTP(mobilenum, emailidno, generatedotp, AppName, FlagMailOTP, VerificationStatus, TimeStamp, OTPcount, refid);
        //    }
        //    else
        //    {
        //        generatedotp = objRandomOtp.Next(1000, 9999).ToString();
        //        string TimeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");
        //        var insertotp = objdb.USP_InsertUpdateOTP(mobilenum, emailidno, generatedotp, AppName, FlagMailOTP, VerificationStatus, TimeStamp, OTPcount, refid);
        //    }

        //    string Appid = result.ApplicationID;

        //    try
        //    {
        //        msg = "Dear Customer, your OTP to submit online consent for application no " + " " + Appid + " " + "is " + " " + generatedotp + " , Regards SUD Life.";
        //        string smsresp = objsms.SendMessage(mobilenum, msg);
        //    }
        //    catch
        //    {

        //    }
        //    var insertotp = objdb.USP_InsertUpdateOTP(mobilenum, emailidno, generatedotp, AppName, FlagMailOTP, VerificationStatus, TimeStamp, OTPcount, refid);

        //    return View("CPO");
        //}





        //  [ValidateInput(false)]
        public ActionResult CPOVKYCP()
        {
            string RefID = TempData["RefID"].ToString();

            // var result = ObjIndo.USP_GetCustDetails(RefID).FirstOrDefault();
            //// if (result.OtpVerfication == "SUCCESS")   //For testing commented
            ////{
            ////    Check OTP Status first from Table ADM_CustLinkReq  if it is Success then only show video CustVKYC  view
            ////                                                  for this  used fetch able ADM_CustLinkReq and then Fetch  PCust_Url from table
            ////                                           if otpverification status is Failed then shhow Message on screen as OTP verifcation Required

            ////       CM.CustVkycLink = "https://videopd.pichainlabs.com/confrences/join_meeting_attendee/eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJjb25mcmVuY2VfaWQiOjc4NywicGFydGljaXBhbnRfaWQiOjEwMzgsInVzZXJfaWQiOjE4Mn0.2-SGYLMmoDle6_ZX1VKO4fzQxXG2xrxuPZqZULAh2sc/";

            ////    CM.CPOVkycLink = result.PHost_Url;
            ////}

            return View(CM);
        }


        // [ValidateInput(false)]
        //[HttpPost]
        //public ActionResult CPOVKYC()
        //{
        //    string filename1 = "";
        //    try
        //    {
        //        // filename1 = WebConfigurationManager.AppSettings["Document"] + "VKYCError.txt";

        //        string defg = "";
        //        //ViewBag.CUSTOMERLOGIN = Session["CUSTOMERLOGIN"].ToString();
        //        ////ViewBag.CUSTOMERLOGIN = "CUSTOMERLOGIN";
        //        ////ViewBag.CUSTOMERLOGIN = Session["CUSTOMERLOGIN"].ToString();
        //        ////ViewBag.CUSTOMERLOGIN = "CUSTOMERLOGIN";
        //        //if (ViewBag.CUSTOMERLOGIN == "CUSTOMERLOGIN")
        //        //{
        //        //    string RefID = Session["refID1"].ToString();

        //        //    defg = RefID;
        //        //}
        //        //else
        //        //{
        //        //    string RefID = Convert.ToString(@Session["RefID"]);
        //        //    defg = RefID;
        //        //    Session["VKYCVIEW"] = ViewBag.CUSTOMERLOGIN;

        //        //}
        //        string RefID = Session["RefID"].ToString();

        //        var result = ObjIndo.USP_GetCustDetails(RefID).FirstOrDefault();

        //        string meetId = result.MeetingID;
        //        if (meetId != null || meetId != string.Empty)
        //        {
        //            // meetId = "e41ca714-5260-4b3f-0192-08d94b65b5d5";//Request.QueryString["meetId"];
        //            ViewBag.meetId = meetId;
        //        }
        //        //var result = objdb.GetLocationDetails(RefID).FirstOrDefault();

        //        //CPO1(RefID);
        //        return View();
        //    }

        //    catch (Exception ex)
        //    {
        //        string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
        //        System.IO.File.AppendAllText(filename1, errormsg);

        //        PortalException.InsertPortalException(ex);
        //        return Json("Exception", JsonRequestBehavior.AllowGet);
        //    }


        //}

        //public ActionResult ShowAddress(string latitude, string Longitude)

        //{
        //    ViewBag.Customerlivelocation = TempData["Customerlivelocation"];
        //    var currency = "";
        //    ViewBag.Title = "CustVKYC";

        //    string APIKEY = "AIzaSyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE";
        //    string URL = @"https://maps.googleapis.com/maps/api/geocode/json?address=" + latitude + "," + Longitude + "&key=" + APIKEY;
        //    string OUTPUT = string.Empty;
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);

        //    using (HttpWebResponse response1 = (HttpWebResponse)request.GetResponse())
        //    using (Stream stream = response1.GetResponseStream())
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        OUTPUT = reader.ReadToEnd();
        //    }

        //    string bfb = "{\n   \"results\" : [\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Mansarovar Station Road\",\n               \"short_name\" : \"Mansarovar Station Rd\",\n               \"types\" : [ \"route\" ]\n            },\n            {\n               \"long_name\" : \"Sector 11\",\n               \"short_name\" : \"Sector 11\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_3\" ]\n            },\n            {\n               \"long_name\" : \"Kamothe\",\n               \"short_name\" : \"Kamothe\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_2\" ]\n            },\n            {\n               \"long_name\" : \"Panvel\",\n               \"short_name\" : \"Panvel\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Navi Mumbai\",\n               \"short_name\" : \"Navi Mumbai\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Raigad\",\n               \"short_name\" : \"Raigad\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"410209\",\n               \"short_name\" : \"410209\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"Shop No. 6, Plot No.6, Main Road, Mansarovar Station Rd, Sector 11, Kamothe, Panvel, Navi Mumbai, Maharashtra 410209, India\",\n         \"geometry\" : {\n            \"location\" : {\n               \"lat\" : 19.0203907,\n               \"lng\" : 73.08939599999999\n            },\n            \"location_type\" : \"GEOMETRIC_CENTER\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0217396802915,\n                  \"lng\" : 73.0907449802915\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0190417197085,\n                  \"lng\" : 73.0880470197085\n               }\n            }\n         },\n         \"place_id\" : \"ChIJdepdQtbp5zsRkRNguNWFWjc\",\n         \"types\" : [ \"establishment\", \"point_of_interest\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"7\",\n               \"short_name\" : \"7\",\n               \"types\" : [ \"premise\" ]\n            },\n            {\n               \"long_name\" : \"Sector 11\",\n               \"short_name\" : \"Sector 11\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_3\" ]\n            },\n            {\n               \"long_name\" : \"Kamothe\",\n               \"short_name\" : \"Kamothe\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_2\" ]\n            },\n            {\n               \"long_name\" : \"Panvel\",\n               \"short_name\" : \"Panvel\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Navi Mumbai\",\n               \"short_name\" : \"Navi Mumbai\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Raigad\",\n               \"short_name\" : \"Raigad\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"410206\",\n               \"short_name\" : \"410206\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"7, Sector 11, Kamothe, Panvel, Navi Mumbai, Maharashtra 410206, India\",\n         \"geometry\" : {\n            \"location\" : {\n               \"lat\" : 19.0202383,\n               \"lng\" : 73.0892673\n            },\n            \"location_type\" : \"ROOFTOP\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0215872802915,\n                  \"lng\" : 73.09061628029151\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0188893197085,\n                  \"lng\" : 73.08791831970849\n               }\n            }\n         },\n         \"place_id\" : \"ChIJmWKoatbp5zsRroWi56dQ3lU\",\n         \"types\" : [ \"street_address\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Main Road\",\n               \"short_name\" : \"Main Rd\",\n               \"types\" : [ \"route\" ]\n            },\n            {\n               \"long_name\" : \"Sector 35\",\n               \"short_name\" : \"Sector 35\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_3\" ]\n            },\n            {\n               \"long_name\" : \"Kamothe\",\n               \"short_name\" : \"Kamothe\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_2\" ]\n            },\n            {\n               \"long_name\" : \"Panvel\",\n               \"short_name\" : \"Panvel\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Navi Mumbai\",\n               \"short_name\" : \"Navi Mumbai\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Raigad\",\n               \"short_name\" : \"Raigad\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"410206\",\n               \"short_name\" : \"410206\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"Main Rd, Sector 35, Kamothe, Panvel, Navi Mumbai, Maharashtra 410206, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0211482,\n                  \"lng\" : 73.08974239999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0198168,\n                  \"lng\" : 73.0887589\n               }\n            },\n            \"location\" : {\n               \"lat\" : 19.0204975,\n               \"lng\" : 73.0892279\n            },\n            \"location_type\" : \"GEOMETRIC_CENTER\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0218314802915,\n                  \"lng\" : 73.09059963029151\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0191335197085,\n                  \"lng\" : 73.0879016697085\n               }\n            }\n         },\n         \"place_id\" : \"ChIJ_49lP9bp5zsRnmTILshjHEs\",\n         \"types\" : [ \"route\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Sector 11\",\n               \"short_name\" : \"Sector 11\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_3\" ]\n            },\n            {\n               \"long_name\" : \"Kamothe\",\n               \"short_name\" : \"Kamothe\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_2\" ]\n            },\n            {\n               \"long_name\" : \"Panvel\",\n               \"short_name\" : \"Panvel\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Navi Mumbai\",\n               \"short_name\" : \"Navi Mumbai\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Raigad\",\n               \"short_name\" : \"Raigad\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"410209\",\n               \"short_name\" : \"410209\",\n               \"types\" : [ \"postal_code\" ]\n            }\n         ],\n         \"formatted_address\" : \"Sector 11, Kamothe, Panvel, Navi Mumbai, Maharashtra 410209, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0215114,\n                  \"lng\" : 73.09312009999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0189731,\n                  \"lng\" : 73.088786\n               }\n            },\n            \"location\" : {\n               \"lat\" : 19.020262,\n               \"lng\" : 73.0915747\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0215912302915,\n                  \"lng\" : 73.09312009999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0188932697085,\n                  \"lng\" : 73.088786\n               }\n            }\n         },\n         \"place_id\" : \"ChIJ46Y7i9bp5zsRKqxEiGZJow4\",\n         \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_3\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"410209\",\n               \"short_name\" : \"410209\",\n               \"types\" : [ \"postal_code\" ]\n            },\n            {\n               \"long_name\" : \"Navi Mumbai\",\n               \"short_name\" : \"Navi Mumbai\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Raigad\",\n               \"short_name\" : \"Raigad\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Navi Mumbai, Maharashtra 410209, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.028895,\n                  \"lng\" : 73.10455349999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0111634,\n                  \"lng\" : 73.0833603\n               }\n            },\n            \"location\" : {\n               \"lat\" : 19.0218215,\n               \"lng\" : 73.0907\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.028895,\n                  \"lng\" : 73.10455349999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0111634,\n                  \"lng\" : 73.0833603\n               }\n            }\n         },\n         \"place_id\" : \"ChIJMbzdt9bp5zsRkBXKtELfMO4\",\n         \"types\" : [ \"postal_code\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Kamothe\",\n               \"short_name\" : \"Kamothe\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_2\" ]\n            },\n            {\n               \"long_name\" : \"Panvel\",\n               \"short_name\" : \"Panvel\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Navi Mumbai\",\n               \"short_name\" : \"Navi Mumbai\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Raigad\",\n               \"short_name\" : \"Raigad\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Kamothe, Panvel, Navi Mumbai, Maharashtra, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0266828,\n                  \"lng\" : 73.1063669\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0091499,\n                  \"lng\" : 73.0832169\n               }\n            },\n            \"location\" : {\n               \"lat\" : 19.0166223,\n               \"lng\" : 73.0966019\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0266828,\n                  \"lng\" : 73.1063669\n               },\n               \"southwest\" : {\n                  \"lat\" : 19.0091499,\n                  \"lng\" : 73.0832169\n               }\n            }\n         },\n         \"place_id\" : \"ChIJrW_BkNPp5zsR_XHBtpZgokU\",\n         \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_2\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Panvel\",\n               \"short_name\" : \"Panvel\",\n               \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n            },\n            {\n               \"long_name\" : \"Navi Mumbai\",\n               \"short_name\" : \"Navi Mumbai\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Raigad\",\n               \"short_name\" : \"Raigad\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Panvel, Navi Mumbai, Maharashtra, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0471059,\n                  \"lng\" : 73.145771\n               },\n               \"southwest\" : {\n                  \"lat\" : 18.9308072,\n                  \"lng\" : 73.082943\n               }\n            },\n            \"location\" : {\n               \"lat\" : 18.9894007,\n               \"lng\" : 73.1175162\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.0471059,\n                  \"lng\" : 73.145771\n               },\n               \"southwest\" : {\n                  \"lat\" : 18.9308072,\n                  \"lng\" : 73.082943\n               }\n            }\n         },\n         \"place_id\" : \"ChIJPfIjHz7o5zsRCv3jMcQGoeM\",\n         \"types\" : [ \"political\", \"sublocality\", \"sublocality_level_1\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Navi Mumbai\",\n               \"short_name\" : \"Navi Mumbai\",\n               \"types\" : [ \"locality\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Thane\",\n               \"short_name\" : \"Thane\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Navi Mumbai, Maharashtra, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.1861914,\n                  \"lng\" : 73.1620789\n               },\n               \"southwest\" : {\n                  \"lat\" : 18.8465126,\n                  \"lng\" : 72.9042434\n               }\n            },\n            \"location\" : {\n               \"lat\" : 19.0330488,\n               \"lng\" : 73.0296625\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.1861914,\n                  \"lng\" : 73.1620789\n               },\n               \"southwest\" : {\n                  \"lat\" : 18.8465126,\n                  \"lng\" : 72.9042434\n               }\n            }\n         },\n         \"place_id\" : \"ChIJrRMfuPC55zsRafiFEWj3Ejw\",\n         \"types\" : [ \"locality\", \"political\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Thane taluka\",\n               \"short_name\" : \"Thane taluka\",\n               \"types\" : [ \"administrative_area_level_3\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Thane\",\n               \"short_name\" : \"Thane\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Thane taluka, Maharashtra, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.3191198,\n                  \"lng\" : 73.1620789\n               },\n               \"southwest\" : {\n                  \"lat\" : 18.8465126,\n                  \"lng\" : 72.77794349999999\n               }\n            },\n            \"location\" : {\n               \"lat\" : 19.0752757,\n               \"lng\" : 73.0169135\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.3191198,\n                  \"lng\" : 73.1620789\n               },\n               \"southwest\" : {\n                  \"lat\" : 18.8465126,\n                  \"lng\" : 72.77794349999999\n               }\n            }\n         },\n         \"place_id\" : \"ChIJY1cwm2q55zsRStnQ_eXSKtw\",\n         \"types\" : [ \"administrative_area_level_3\", \"political\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Raigad\",\n               \"short_name\" : \"Raigad\",\n               \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Raigad, Maharashtra, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.13368,\n                  \"lng\" : 73.66013989999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 17.84528,\n                  \"lng\" : 72.85393999999999\n               }\n            },\n            \"location\" : {\n               \"lat\" : 18.5157519,\n               \"lng\" : 73.1821623\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 19.13368,\n                  \"lng\" : 73.66013989999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 17.84528,\n                  \"lng\" : 72.85393999999999\n               }\n            }\n         },\n         \"place_id\" : \"ChIJjfO02zt66DsRn3LsnNeC5nk\",\n         \"types\" : [ \"administrative_area_level_2\", \"political\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"Maharashtra\",\n               \"short_name\" : \"MH\",\n               \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n            },\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"Maharashtra, India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 22.028441,\n                  \"lng\" : 80.890924\n               },\n               \"southwest\" : {\n                  \"lat\" : 15.6024121,\n                  \"lng\" : 72.659363\n               }\n            },\n            \"location\" : {\n               \"lat\" : 19.7514798,\n               \"lng\" : 75.7138884\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 22.028441,\n                  \"lng\" : 80.890924\n               },\n               \"southwest\" : {\n                  \"lat\" : 15.6024121,\n                  \"lng\" : 72.659363\n               }\n            }\n         },\n         \"place_id\" : \"ChIJ-dacnB7EzzsRtk_gS5IiLxs\",\n         \"types\" : [ \"administrative_area_level_1\", \"political\" ]\n      },\n      {\n         \"access_points\" : [],\n         \"address_components\" : [\n            {\n               \"long_name\" : \"India\",\n               \"short_name\" : \"IN\",\n               \"types\" : [ \"country\", \"political\" ]\n            }\n         ],\n         \"formatted_address\" : \"India\",\n         \"geometry\" : {\n            \"bounds\" : {\n               \"northeast\" : {\n                  \"lat\" : 35.513327,\n                  \"lng\" : 97.39535869999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 6.4626999,\n                  \"lng\" : 68.1097\n               }\n            },\n            \"location\" : {\n               \"lat\" : 20.593684,\n               \"lng\" : 78.96288\n            },\n            \"location_type\" : \"APPROXIMATE\",\n            \"viewport\" : {\n               \"northeast\" : {\n                  \"lat\" : 35.513327,\n                  \"lng\" : 97.39535869999999\n               },\n               \"southwest\" : {\n                  \"lat\" : 6.4626999,\n                  \"lng\" : 68.1097\n               }\n            }\n         },\n         \"place_id\" : \"ChIJkbeSa_BfYzARphNChaFPjNc\",\n         \"types\" : [ \"country\", \"political\" ]\n      }\n   ],\n   \"status\" : \"OK\"\n}\n";


        //    Dictionary<string, object> dict = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(bfb);
        //    JObject currencies = JObject.Parse(OUTPUT);
        //    var szc = dict["results"];
        //    var model = new CPO_model();
        //    currency = currencies.SelectToken("results")[0].SelectToken("formatted_address").ToString();

        //    ViewBag.c = currency;
        //    model.Customerlivelocation = currency;
        //    TempData["Customerlivelocation"] = currency;
        //    ///return currency;
        //    return Json(currency, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult EndCP()
        {
            string filename1 = "";
            try
            {
                //filename1 = WebConfigurationManager.AppSettings["Document"] + "VKYCError.txt";
                string defg = "";
                // string CUSTOMERLOGIN = TempData["CUSTOMERLOGIN"].ToString();
                //ViewBag.CUSTOMERLOGIN = Session["CUSTOMERLOGIN"].ToString();

                //if (ViewBag.CUSTOMERLOGIN == "CUSTOMERLOGIN")
                //{
                //    string RefID = Session["refID1"].ToString();
                //    defg = RefID;
                //}
                //else
                //{
                //    string RefID = Session["refID1"].ToString();
                //    defg = RefID;

                //}

                string RefID = HttpContext.Session.GetString("PersonalId");
                //// var result = ObjIndo.USP_GetMeetingDetails(RefID).FirstOrDefault();


                string meetId = "ae067ab9-be96-4b20-2fb7-08dabe274b93";
                ////ViewBag.meetId = result.MeetingID;
                //// ViewBag.link = "https://api.indofinnet.com/api/meeting/" + result.MeetingID + "/download";
                ViewBag.link = "https://api.indofinnet.com/api/meeting/" + meetId + "/download";
                WebClient wc = new WebClient();
                //  long amc = Convert.ToInt32(Session["CustId"]);
                long amc = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                ///var res1 = ObjIndo.USP_updateflag(amc);
                TempData["msg"] = "Video Saved Successfully";

                //var d = wc.DownloadData("https://api.indofinnet.com/api/meeting/" + result.MeetingID + "/download");
                //System.IO.File.WriteAllBytes("D:\\Video\\" + result.MeetingID + ".webm", d);
                ////  return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                return RedirectToAction("SummerySheetDetails", "DataVerify");
            }
            catch (Exception ex)
            {
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                System.IO.File.AppendAllText(filename1, errormsg);
                PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        //public ActionResult EndCP1()
        //{
        //    string filename1 = "";
        //    try
        //    {
        //        filename1 = WebConfigurationManager.AppSettings["Document"] + "VKYCError.txt";
        //        string defg = "";
        //        // string CUSTOMERLOGIN = TempData["CUSTOMERLOGIN"].ToString();
        //        //ViewBag.CUSTOMERLOGIN = Session["CUSTOMERLOGIN"].ToString();

        //        //if (ViewBag.CUSTOMERLOGIN == "CUSTOMERLOGIN")
        //        //{
        //        //    string RefID = Session["refID1"].ToString();
        //        //    defg = RefID;
        //        //}
        //        //else
        //        //{
        //        //    string RefID = Session["refID1"].ToString();
        //        //    defg = RefID;

        //        //}
        //        string RefID = Session["RefID"].ToString();
        //        var result = ObjIndo.USP_GetCustDetails(RefID).FirstOrDefault();


        //        ViewBag.meetId = result.MeetingID;
        //        ViewBag.link = "https://api.indofinnet.com/api/meeting/" + result.MeetingID + "/download";
        //        WebClient wc = new WebClient();
        //        long amc = Convert.ToInt32(Session["PersonalId"]);
        //        var res1 = ObjIndo.USP_updateflag(amc);
        //        TempData["msg"] = "Video Saved Successfully";

        //        //var d = wc.DownloadData("https://api.indofinnet.com/api/meeting/" + result.MeetingID + "/download");
        //        //System.IO.File.WriteAllBytes("D:\\Video\\" + result.MeetingID + ".webm", d);
        //        return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
        //    }
        //    catch (Exception ex)
        //    {
        //        string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
        //        System.IO.File.AppendAllText(filename1, errormsg);
        //        PortalException.InsertPortalException(ex);
        //        return Json("Exception", JsonRequestBehavior.AllowGet);
        //    }
        //}

        // [HttpPost]
        //public string VideoCallBack(Guid id, List<IFormFile> files)
        //{
        //    try
        //    {
        //        PrintLog("CallBack", "hi");

        //        foreach (var formFile in files)
        //        {
        //            if (formFile.Length > 0)
        //            {
        //                var filePath = @"D:\Video\" + id + ".wmv";

        //                using (var stream = System.IO.File.Create(filePath))
        //                {
        //                    formFile.CopyTo(stream);
        //                }
        //            }
        //        }





        //        return "SUCCESS";
        //    }
        //    catch (Exception ex)
        //    {
        //        PrintLog("CallBack", ex.Message + ex.InnerException);
        //        return "ERROR";
        //    }
        //}

        //public bool SnapshotCallbackURL(Guid id, List<IFormFile> files)
        //{
        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            var filePath = @"D:\Video\" + id + ".jpeg";

        //            using (var stream = System.IO.File.Create(filePath))
        //            {
        //                formFile.CopyTo(stream);
        //            }
        //        }
        //    }
        //    return true;
        //}

        //public static void PrintLog(string RequestHeader, string data)
        //{
        //    string filepath = ConfigurationManager.AppSettings["VkycLog"];// @"D:\" ; //Text File Path

        //    if (!Directory.Exists(filepath))
        //    {
        //        Directory.CreateDirectory(filepath);

        //    }
        //    filepath = filepath + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name
        //    if (!System.IO.File.Exists(filepath))
        //    {


        //        System.IO.File.Create(filepath).Dispose();

        //    }
        //    using (StreamWriter sw = System.IO.File.AppendText(filepath))
        //    {


        //        sw.WriteLine("-------------------------------" + RequestHeader + "------------" + DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss"));
        //        sw.WriteLine(data);

        //        sw.WriteLine("--------------------------------End-------------------------------------------------");
        //        sw.Flush();
        //        sw.Close();

        //    }
        //}

        public async Task<bool> GetFile(Guid id, List<IFormFile> files)
        {
            ErrorLog error_log = new ErrorLog();
            string filename1 = "";
            //var VKYCDATA = @"D:\Videocallback";
            // var VKYCDATA = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\video");

            try
            {

                //filename1 = WebConfigurationManager.AppSettings["Document"] + "error1.txt";
                //System.IO.File.AppendAllText(filename1, "GetFile called");
                //string cde = Convert.ToString((Session["PersonalId"]));
                string avc = Convert.ToString(id);
                var data = ObjIndo.AdmCustLinkReq.FromSqlRaw($"USP_GetVKYCCustomerID {avc}").AsEnumerable().FirstOrDefault();
                int CVH = Convert.ToInt32(data.CustomerID);
                //var datetime = data.CreatedDate;
                //var qdata = ObjIndo.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsByCustID {CVH}").AsEnumerable().FirstOrDefault();
                //string FirstName = qdata.CustFirstName;// Cust_FirstName;
                //string LastName = qdata.CustLastName;// Cust_LastName;




                //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\video", (Convert.ToString(files)));//ConfigurationManager.AppSettings["VKYCDATA"] + "\\" + CVH + "_" + FirstName + "_" + LastName + ".webm";
                //var filePath = VKYCDATA + "\\" + CVH + ".webm";

                //var stream = System.IO.File.Create(filePath);

                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in files)
                {
                    var connectionstring1 = "DefaultEndpointsProtocol=https;AccountName=alphafileupload;AccountKey=gKAw4e0SU9dk0vmWYghlEmI/tFbUL1RdYKMJghhKWaj+0tEZ0KXnKN3GSjTWsB9QDDIuwVtvaCr7+AStGgmbIQ==;EndpointSuffix=core.windows.net";

                    string fileName = Path.GetFileName(postedFile.FileName);
                    var containerName = "fileupload";
                    var container = new BlobContainerClient(connectionstring1, containerName);
                    var blobClient = container.GetBlockBlobClient("VKYC.webm");
                    using (FileStream stream = new FileStream(Path.Combine(fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        await blobClient.UploadAsync(stream);

                        //ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }
                //foreach (IFormFile fileName in files)
                //{
                //    string fileName1 = Path.GetFileName(fileName.FileName);

                //    byte[] readText = System.IO.File.ReadAllBytes(fileName1);

                //    var connectionstring = "DefaultEndpointsProtocol=https;AccountName=alphafileupload;AccountKey=gKAw4e0SU9dk0vmWYghlEmI/tFbUL1RdYKMJghhKWaj+0tEZ0KXnKN3GSjTWsB9QDDIuwVtvaCr7+AStGgmbIQ==;EndpointSuffix=core.windows.net";

                //    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);


                //    var containerName = "fileupload";
                //    var container = new BlobContainerClient(connectionstring, containerName);
                //    var blobClient = container.GetBlockBlobClient("VKYC.webm");
                //    using (var ms = new MemoryStream(readText))
                //    {
                //        StreamWriter writer = new StreamWriter(ms);

                //        writer.Flush();
                //        ms.Position = 0;
                //        await blobClient.UploadAsync(ms);
                //    }

                //}
                //var file = files;


                //foreach (var formFile in files)
                //{
                //    if (formFile.Length > 0)
                //    {
                //        // filePath = Path.GetTempFileName();

                //        using (var stream = System.IO.File.Create(filePath))
                //        {
                //            await formFile.CopyToAsync(stream);
                //        }
                //    }
                //}


                return true;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString() + files);


                return false;
            }
        }

        public bool GetSnapshot(Guid id, List<IFormFile> files)
        {
            string filename = "";
            try
            {
                //filename = WebConfigurationManager.AppSettings["Document"] + "error2.txt";
                //System.IO.File.AppendAllText(filename, "GetSnapshot called");

                //// PrintLog("snapshot", "hi");
                ////string cde = Convert.ToString((Session["PersonalId"]));
                //string avc = Convert.ToString(id);
                //var data = ObjIndo.USP_GetVKYCCustomerID(avc);
                //string CVH = data.CustomerID;
                ////var datetime = data.CreatedDate;
                //var qdata = ObjIndo.USP_GetQEDetailsByCustID(Convert.ToInt64(CVH));
                //string FirstName = qdata.Cust_FirstName;
                //string LastName = qdata.Cust_LastName;
                //var filePath = ConfigurationManager.AppSettings["VKYCDATA1"] + "\\" + CVH + "_" + FirstName + "_" + LastName + ".jpg";
                ////var filePath = ConfigurationManager.AppSettings["VKYCDATA1"] + "\\" + cde + ".jpg";



                ////var stream = System.IO.File.Create(filePath);
                //foreach (string fileName in Request.Files)
                //{
                //    HttpPostedFileBase file = Request.Files[fileName];
                //    file.SaveAs(filePath);


                //}


                return true;

            }
            catch (Exception ex)
            {
                //PrintLog("snapshot", ex.Message + ex.InnerException);
                //return false;
                //string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                //System.IO.File.AppendAllText(filename, errormsg);
                //PrintLog("snapshot", ex.Message + ex.InnerException);
                return false;
            }
        }


        [HttpPost]
        public async Task<ActionResult> UploadVideo(IFormFile file)
        {
            var CustomerId= HttpContext.Session.GetString("PersonalId");
            byte[] eibytes1 = null;
            BinaryReader reader = new BinaryReader(file.OpenReadStream());
            eibytes1 = reader.ReadBytes((int)file.Length);
            string m = Convert.ToBase64String(eibytes1);
            if (eibytes1 != null)
            {
                string connectionString = $"DefaultEndpointsProtocol=https;AccountName=alphafileupload;AccountKey=gKAw4e0SU9dk0vmWYghlEmI/tFbUL1RdYKMJghhKWaj+0tEZ0KXnKN3GSjTWsB9QDDIuwVtvaCr7+AStGgmbIQ==;EndpointSuffix=core.windows.net";
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);


                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("fileupload");

                await containerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = containerClient.GetBlobClient(CustomerId+".webm");

                using (var stream = new MemoryStream(eibytes1))
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

            }
            string defg = "";
            string filename1 = "";
            string RefID = HttpContext.Session.GetString("RefID");
            string conn2 = _connectionString;
            using (SqlConnection connection2 = new SqlConnection(conn2))
            {
                SqlCommand cmd2 = new SqlCommand("USP_GetMeetingDetails", connection2);
                cmd2.CommandType = CommandType.StoredProcedure;

                cmd2.Parameters.AddWithValue("@refID", RefID);


                connection2.Open();


                SqlDataReader reader2 = cmd2.ExecuteReader();
                if (reader2.Read())
                {

                    string RefID1 = reader2[5].ToString();
                    defg = RefID1;
                }
            }
            string conn3 = _connectionString;
            using (SqlConnection connection3 = new SqlConnection(conn3))
            {
                SqlCommand cmd3 = new SqlCommand("USP_updateflag", connection3);
                cmd3.CommandType = CommandType.StoredProcedure;

                cmd3.Parameters.AddWithValue("@CustomerDetailId", CustomerId);
                connection3.Open();
            }
            return Json("Success");


        }


        //public bool SelfGetFile(Guid id, List<IFormFile> files)
        //{
        //    string filename1 = "";

        //    try
        //    {

        //        filename1 = WebConfigurationManager.AppSettings["Document"] + "error1.txt";
        //        System.IO.File.AppendAllText(filename1, "GetFile called");
        //        //string cde = Convert.ToString((Session["PersonalId"]));
        //        string avc = Convert.ToString(id);
        //        var data = ObjIndo.USP_GetVKYCCustomerID(avc);
        //        string CVH = data.CustomerID;

        //        var qdata = ObjIndo.USP_GetQEDetailsByCustID(Convert.ToInt64(CVH));
        //        string FirstName = qdata.Cust_FirstName;
        //        string LastName = qdata.Cust_LastName;

        //        var filePath = ConfigurationManager.AppSettings["VKYCDATA2"] + "\\" + CVH + "_" + FirstName + "_" + LastName + ".webm";
        //        // var filePath = ConfigurationManager.AppSettings["VKYCDATA"] + "\\" + cde + ".webm";

        //        //var stream = System.IO.File.Create(filePath);
        //        foreach (string fileName in Request.Files)
        //        {
        //            HttpPostedFileBase file = Request.Files[fileName];
        //            file.SaveAs(filePath);


        //        }


        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //PrintLog("callback", ex.Message + ex.InnerException);
        //        //return false;
        //        string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
        //        System.IO.File.AppendAllText(filename1, errormsg);
        //        PrintLog("callback", ex.Message + ex.InnerException);
        //        return false;
        //    }
        //}


    }
}

