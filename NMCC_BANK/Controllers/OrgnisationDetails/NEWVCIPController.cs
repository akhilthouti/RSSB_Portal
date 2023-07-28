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
using Microsoft.EntityFrameworkCore;
using Amazon.Auth.AccessControlPolicy;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System.Transactions;
using VKYCWebAPI;
using Spire.Pdf.OPC;
using Microsoft.Extensions.Configuration;

namespace INDO_FIN_NET.Controllers
{
    public class NEWVCIPController : Controller
    {
        TripleDESImplementation objtriple = new TripleDESImplementation();
        ClsCustIPVDetails objcustIpv = new ClsCustIPVDetails();
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        private readonly string _connectionString;

        // GET: VCIP
        public NEWVCIPController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        public ActionResult Index()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                return View();
                //return RedirectToAction("StartRequestCamera", "VCIP");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception"); //, );;
            }
        }       
        public ActionResult customer(long custid)
        {
            try
            {
                if (custid == 0)
                {
                    ViewBag.custid = custid;


                }
                else
                {
                    Int64 abcd = Convert.ToInt64(custid);

                    string conn4 = _connectionString;
                    using (SqlConnection connection4 = new SqlConnection(conn4))
                    {
                        SqlCommand cmd = new SqlCommand("USP_InsertRequest", connection4);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustId", abcd);
                        cmd.Parameters.AddWithValue("@CustRequestFlag", "true");
                        //cmd.Parameters.AddWithValue("@refId", refID);
                        cmd.Parameters.AddWithValue("@CustReqTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@LinkSendFlag", "false");
                        cmd.Parameters.AddWithValue("@LinkSendTime", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ConnectionFlag", "false");
                        cmd.Parameters.AddWithValue("@CustOtp", "null");
                        cmd.Parameters.AddWithValue("@CustOtpVerifyFlag", "true");
                        cmd.Parameters.AddWithValue("@VcipStatus", "");
                        cmd.Parameters.AddWithValue("@LinkSendBy", "null");
                        cmd.Parameters.AddWithValue("@AuthorizedPerson", "null");
                        cmd.Parameters.AddWithValue("@Manualassignflag", "false");
                        
                        connection4.Open();
                        int result = cmd.ExecuteNonQuery();

                        connection4.Close();
                    }
                    ViewBag.custid = custid;
                    ViewBag.MOB = HttpContext.Session.GetString("MobileNo");

                }

                return View();
            }
            catch (Exception ex)
            {
                string errormsg = ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                string str = errormsg;
                return Json(str); //, );;
            }
        }

        
        public ActionResult VerifyMobile(long custid, string mobileno)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var client = new RestClient("https://apigateway.indofinnet.com/api/SMSOTP?OrgID=IndoFin007&Tomobile=" + mobileno);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;

                res = res.Replace(@"\", "");

                string s = res.Split('"')[4];
                return Json(s);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
        }
        ////check OTP in db
        public ActionResult OTPVerify(long CustId, string OTP, string Mobileno)
        {
            
            ErrorLog error_log = new ErrorLog();
            try
            {
                var client = new RestClient("https://apigateway.indofinnet.com/api/VerifyOTP?OrgID=IndoFin007&mobileno=" + Mobileno + "&otp=" + OTP);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;

                res = res.Replace(@"\", "");
                string s = res.Split('"')[1];
                string result = s;

                return Json(result);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        
        public ActionResult SkipIpv()
        {


            try
            {
                long amc = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

                string conn3 = _connectionString;
                using (SqlConnection connection3 = new SqlConnection(conn3))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_SkipIPVFlag", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;

                    cmd3.Parameters.AddWithValue("@CustId ", amc);


                    connection3.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {
                        //var Result = reader2["RESULT"].ToString();
                    }
                }
                return RedirectToAction("CustomerDocumentDetails1", "DataVerify");
            }
            catch (Exception ex)
            {
                return Json(ex.ToString());
            }


        }
        
        public ActionResult Admin()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                return View(); // objDetails.USP_SelectCallDetails().ToList());
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
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
                //PortalException.InsertPortalException(ex);
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
                //PortalException.InsertPortalException(ex);
                return Json("Exception");//, );;
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
                //PortalException.InsertPortalException(ex);
                return Json("Exception"); //, );;
            }
        }

       
        public ActionResult CPOVKYC()
        {
            string filename1 = "";
            try
            {
                long abcf = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

                string conn23 = _connectionString;
                using (SqlConnection connection23 = new SqlConnection(conn23))
                {
                    SqlCommand cmd23 = new SqlCommand("USP_GetREFID", connection23);
                    cmd23.CommandType = CommandType.StoredProcedure;

                    cmd23.Parameters.AddWithValue("@Cust_CustId", abcf);


                    connection23.Open();


                    SqlDataReader reader23 = cmd23.ExecuteReader();
                    if (reader23.Read())
                    {

                        string RefID1 = reader23[15].ToString();
                    }
                    string RefID = reader23[15].ToString();
                    HttpContext.Session.SetString("RefID", RefID);
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
                        }

                        string meetId = reader2[5].ToString();
                        string conn = _connectionString;
                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand cmd = new SqlCommand("USP_InsertMeetingid", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MeetingID", meetId);
                            cmd.Parameters.AddWithValue("@CustomerDetailId", abcf);

                            connection.Open();
                            int result = cmd.ExecuteNonQuery();
                            connection.Close();
                        }

                        if (meetId != null || meetId != string.Empty)
                        {
                            // meetId = "e41ca714-5260-4b3f-0192-08d94b65b5d5";//Request.QueryString["meetId"];
                            ViewBag.meetId = meetId;
                        }
                        return View();

                    }

                }
            }
            catch (Exception ex)
            {
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                System.IO.File.AppendAllText(filename1, errormsg);
                PortalException.InsertPortalException(ex);
                // return Json("Exception", JsonRequestBehavior.AllowGet);
                return Json("Exception");
            }
        }

        public ActionResult ONEWAYCPOVKYC()
        {
            string filename1 = "";
            try
            {
                long abcf = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

                string conn23 = _connectionString;
                using (SqlConnection connection23 = new SqlConnection(conn23))
                {
                    SqlCommand cmd23 = new SqlCommand("USP_GetREFID", connection23);
                    cmd23.CommandType = CommandType.StoredProcedure;

                    cmd23.Parameters.AddWithValue("@Cust_CustId", abcf);


                    connection23.Open();


                    SqlDataReader reader23 = cmd23.ExecuteReader();
                    if (reader23.Read())
                    {

                        string RefID1 = reader23[15].ToString();
                    }
                    string RefID = reader23[15].ToString();
                    HttpContext.Session.SetString("RefID", RefID);
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
                        }

                        string meetId = reader2[5].ToString();
                        string conn = _connectionString;
                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand cmd = new SqlCommand("USP_InsertMeetingid", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MeetingID", meetId);
                            cmd.Parameters.AddWithValue("@CustomerDetailId", abcf);

                            connection.Open();
                            int result = cmd.ExecuteNonQuery();
                            connection.Close();
                        }

                        if (meetId != null || meetId != string.Empty)
                        {
                            // string meetId = "e41ca714-5260-4b3f-0192-08d94b65b5d5";//Request.QueryString["meetId"];
                            ViewBag.meetId = meetId;
                        }
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                System.IO.File.AppendAllText(filename1, errormsg);
                PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        public ActionResult EndCU()
        {
            return RedirectToAction("SummerySheetDetails", "DataVerify");

        }

        public async Task<IActionResult> showVideo()
        {
            try
            {
                var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");

                string connectionString = $"DefaultEndpointsProtocol=https;AccountName=alphafileupload;AccountKey=gKAw4e0SU9dk0vmWYghlEmI/tFbUL1RdYKMJghhKWaj+0tEZ0KXnKN3GSjTWsB9QDDIuwVtvaCr7+AStGgmbIQ==;EndpointSuffix=core.windows.net";

                //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);

                byte[] bytes = null;
                var containerName = "fileupload";
                var container = new BlobContainerClient(connectionString, containerName);
               // await foreach (BlobItem item in container.GetBlobsAsync())
                
                    var blobClient = container.GetBlobClient(CustomerId+".webm");
                    using Stream stream = await blobClient.OpenReadAsync();


                    using (MemoryStream ms = new MemoryStream())
                    {
                        await stream.CopyToAsync(ms);
                        bytes = ms.ToArray();
                    }
                
                //var mayur = "mmmmmmm";
                //byte[] dada = Convert.FromBase64String(mayur);
                string base64string = Convert.ToBase64String(bytes);
                string decodedString = Encoding.UTF8.GetString(bytes);
                byte[] byteArray = Encoding.UTF8.GetBytes(decodedString);

                ViewBag.filepath1 = base64string;


                return Json(base64string);

                // var VKYCDATA = @"D:\Videocallback";
                //var filePath = "";
                //var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                //var qdata1 = objDetails.TblCustomerDetails.FromSqlRaw($"USP_GetCustomerDetailsbyId {CustomerId}").AsEnumerable().FirstOrDefault();
                //string FirstName = qdata1.CustFirstName;
                //string LastName = qdata1.CustLastName;
                //var qdata = objDetails.AdmVciprequestDetails.FromSqlRaw($"USP_GetREFID {CustomerId}").AsEnumerable().FirstOrDefault();
                //string abcd = qdata.MeetingID;
                //var data = objDetails.AdmCustLinkReq.FromSqlRaw($"USP_GetVKYCCustomerID {CustomerId}").AsEnumerable().FirstOrDefault();
                ////string datetime = Convert.ToString(data.CreatedDate);
                //var mnp = objDetails.AdmCustLinkReq.FromSqlRaw($"USP_GetVkycMode {CustomerId}").AsEnumerable().FirstOrDefault();
                //string abcs = Convert.ToString(CustomerId);
                //string abd = mnp.VkycMode;

                //var VKYCDATA = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\xmlExtract");
                //if (abd == "immediate")
                //{
                //    filePath = VKYCDATA + "\\" + abcs + "_" + FirstName + "_" + LastName + ".webm";
                //}
                //if (!Directory.Exists(filePath))
                //{
                //    byte[] readText = System.IO.File.ReadAllBytes(filePath);
                //    string ImgData = Convert.ToBase64String(readText);
                //    ViewBag.filepath1 = ImgData;
                //    return Json(ImgData);
                //}
                //return Json("");
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return View();

            }
        }

        public async Task<ActionResult> SelfVkycSave(IFormFile file)
        {
            var CustomerId=Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
            byte[] eibytes1 = null;
            BinaryReader reader = new BinaryReader(file.OpenReadStream());
            eibytes1 = reader.ReadBytes((int)file.Length);
            //string m = Convert.ToBase64String(eibytes1);
            if (eibytes1 != null)
            {



                string connectionString = $"DefaultEndpointsProtocol=https;AccountName=alphafileupload;AccountKey=gKAw4e0SU9dk0vmWYghlEmI/tFbUL1RdYKMJghhKWaj+0tEZ0KXnKN3GSjTWsB9QDDIuwVtvaCr7+AStGgmbIQ==;EndpointSuffix=core.windows.net";
                BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);


                BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("fileupload");

                await containerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = containerClient.GetBlobClient(CustomerId + ".webm");

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
            long amc = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
            string conn3 = _connectionString;
            using (SqlConnection connection3 = new SqlConnection(conn3))
            {
                SqlCommand cmd3 = new SqlCommand("USP_updateflag", connection3);
                cmd3.CommandType = CommandType.StoredProcedure;

                cmd3.Parameters.AddWithValue("@CustomerDetailId", amc);
                connection3.Open();
            }
            return Json("Success");
        }
    }
}