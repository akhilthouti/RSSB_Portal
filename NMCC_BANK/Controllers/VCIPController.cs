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

namespace INDO_FIN_NET.Controllers
{
    public class VCIPController : Controller
    {
        TripleDESImplementation objtriple = new TripleDESImplementation();
        ClsCustIPVDetails objcustIpv = new ClsCustIPVDetails();
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        // GET: VCIP
        public ActionResult Index()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception"); 
            }
        }
        public ActionResult Admin()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        [HttpGet]
        public ActionResult PreviewConcall()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult AdminVCIP()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        public ActionResult LastPage()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        //Sending SMS
        public string SendSms(string Mobile, string Msg)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ResponseSMS objsms = new ResponseSMS();
                var client = new RestClient("https://api.pinnacle.in/index.php/sms/send");
                client.Timeout = -1;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request = new RestRequest(Method.POST);
                request.AddHeader("apikey", "936e06-1a6cfb-c3f05f-638ddc-2b99ae");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddHeader("Cookie", "PHPSESSID=jj06ajo9d7b7u5nvthnofj3qvb; DO-LB=node-156923666|X2Rfh|X2Re8");
                request.AddParameter("sender", "ALPHAF");
                request.AddParameter("numbers", Mobile);
                request.AddParameter("message", Msg);
                request.AddParameter("messagetype", "TXT");
                IRestResponse response = client.Execute(request);

                string Response_Status = response.Content;


                if (response.ErrorMessage != null)
                {
                    return response.ErrorMessage /*+ errorline*/ + response.Content + response.ErrorException + response.Request + response.ResponseUri + response.Server + response.StatusCode;
                }
                else
                {
                    string Result = null;
                    objsms = JsonConvert.DeserializeObject<ResponseSMS>(Response_Status);
                    Result = objsms.status + "," + objsms.code;
                    return Result;
                }

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return ex.Message;
            }
        }

    }
}