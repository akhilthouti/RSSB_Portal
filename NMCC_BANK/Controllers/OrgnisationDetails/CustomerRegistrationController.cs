using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using INDO_FIN_NET.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel;
using Newtonsoft.Json;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Controllers.OrgnisationDetails
{
    public class CustomerRegistrationController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        private readonly string _connectionString;

        public CustomerRegistrationController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        [HttpGet]
        public ActionResult CustomerRegistration()
        {
            ErrorLog error_log = new ErrorLog();

            try
            {
                //if (TempData["Fcode"] != null)
                //{
                //    ViewBag.Fcode = TempData["Fcode"];
                //}
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());

                // PortalException.InsertPortalException(ex);
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }
        }
        [HttpPost]
        public async Task<ActionResult> CustomerRegistration(clsCustRegistration objCustRegister)
        {
            ErrorLog error_log = new ErrorLog();
            long? id = 0;
            try
            {
                HttpContext.Session.SetString("CustMobileNo", objCustRegister.MobileNo);
                var SessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("SessionId", SessionId);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                HttpWebRequest requestFund = null;
                WebResponse responsefund = null;
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_CustomerExistsCheckNew", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@MobileNo", objCustRegister.MobileNo);
                    connection.Open();
                    SqlDataReader reader1 = cmd.ExecuteReader();
                    if (reader1.Read())
                    {
                        var MobResp = reader1["RESULT"].ToString();
                        if (MobResp == "NOTEXISTS")
                        {
                            DateTime Date = DateTime.Now;

                            using (SqlConnection connection1 = new SqlConnection(conn))
                            {
                                SqlCommand cmd1 = new SqlCommand("USP_InsertCountOfCustLogins", connection1);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@MobileNO", objCustRegister.MobileNo);
                                cmd1.Parameters.AddWithValue("@SessionID", SessionId);
                                cmd1.Parameters.AddWithValue("@LoginTime", Date);
                                cmd1.Parameters.AddWithValue("@IsLogin", true);
                                connection1.Open();
                                SqlDataReader reader = cmd1.ExecuteReader();
                            }
                            using (SqlConnection cn1 = new SqlConnection(_connectionString))
                            {
                                cn1.Open();
                                SqlCommand cmd1 = new SqlCommand("USP_NewCustomer", cn1);
                                cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@FirstName", objCustRegister.FirstName);
                                cmd1.Parameters.AddWithValue("@LastName", objCustRegister.LastName);
                                cmd1.Parameters.AddWithValue("@Email", objCustRegister.Emailid);
                                cmd1.Parameters.AddWithValue("@MobileNo", objCustRegister.MobileNo);
                                cmd1.ExecuteNonQuery();
                                return Json("Success");

                            }
                        }
                        else
                        {
                            return Json("Already Exist");
                        }
                    }
                }
                return View();
            }
            catch (Exception e)
            {
                error_log.WriteErrorLog(e.ToString());

                return Json("Exception");
            }
        }

        [HttpPost]
        public async Task<ActionResult> LoginWithOTP([FromServices] IActiveLogin objLogin, string MobileNo, string OTP, string FCode)
        {
            ErrorLog error_log = new ErrorLog();
            long? id = 0;
            HttpContext.Session.SetString("CustMobileNo", MobileNo);
            var SessionId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("NewSessionId", SessionId);
            if (FCode == "0" || FCode == "NaN")
            {
                FCode = null;
            }
            try
            {
                string OTPVerifyResponse = "";
                OTPVerifyResponse = OTPAuthenticateSignIn(OTP);
                if (OTPVerifyResponse != "Authinticate SuccessFully")
                {
                    //var result = objDetails.IndoAdminDetails.FromSqlRaw($"USP_GetCustomerSessionDetails {(MobileNo)}").AsEnumerable().FirstOrDefault();
                    string conn = _connectionString;
                    using (SqlConnection connection = new SqlConnection(conn))
                    {
                        SqlCommand cmd1 = new SqlCommand("USP_GetCustomerSessionDetails", connection);
                        cmd1.CommandType = CommandType.StoredProcedure;
                        cmd1.Parameters.AddWithValue("@MobileNo", MobileNo);

                        connection.Open();
                        SqlDataReader reader1 = cmd1.ExecuteReader();
                        if (reader1.Read())
                        {
                            bool Result = Convert.ToBoolean(reader1[0]);
                            var LoginTime = reader1[1];
                            if (Result == true)
                            {
                                
                                    var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
                                    if (LoginStatus == "Active")
                                    {

                                        //###################//
                                        string ip = "";
                                        var SystemName = "";
                                        if (Result != true)
                                        {

                                            SystemName = Dns.GetHostName();
                                            IPHostEntry HostDetails = Dns.GetHostEntry(SystemName);
                                            IPAddress ip1 = null;
                                            foreach (IPAddress info in HostDetails.AddressList)
                                            {
                                                if (info.AddressFamily == AddressFamily.InterNetwork)
                                                {
                                                    ip1 = info;
                                                }
                                            }
                                            ip = ip1.ToString();
                                            HttpContext.Session.SetString("ExistIP", ip);
                                            DateTime Date = DateTime.Now;

                                            using (SqlConnection connection1 = new SqlConnection(conn))
                                            {
                                                SqlCommand cmd = new SqlCommand("USP_InsertCountOfCustLogins", connection1);
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@MobileNO", MobileNo);
                                                cmd.Parameters.AddWithValue("@SessionID", SessionId);
                                                cmd.Parameters.AddWithValue("@LoginTime", Date);
                                                cmd.Parameters.AddWithValue("@IsLogin", true);
                                                connection1.Open();
                                                SqlDataReader reader = cmd.ExecuteReader();
                                            }
                                        }
                                        else
                                        {
                                            SystemName = Dns.GetHostName();
                                            IPHostEntry HostDetails = Dns.GetHostEntry(SystemName);
                                            IPAddress ip1 = null;
                                            foreach (IPAddress info in HostDetails.AddressList)
                                            {
                                                if (info.AddressFamily == AddressFamily.InterNetwork)
                                                {
                                                    ip1 = info;
                                                }
                                            }
                                            ip = ip1.ToString();
                                            HttpContext.Session.SetString("ExistIP", ip);
                                            DateTime Date = DateTime.Now;

                                            using (SqlConnection connection1 = new SqlConnection(conn))
                                            {
                                                SqlCommand cmd = new SqlCommand("USP_InsertCountOfCustLogins", connection1);
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@MobileNO", MobileNo);
                                                cmd.Parameters.AddWithValue("@SessionID", SessionId);
                                                cmd.Parameters.AddWithValue("@LoginTime", Date);
                                                cmd.Parameters.AddWithValue("@IsLogin", true);
                                                connection1.Open();
                                                SqlDataReader reader = cmd.ExecuteReader();
                                            }
                                        }
                                        return Json("Success" + "," + MobileNo + "," + Result + "," + ip + "," + SystemName + "," + LoginTime);
                                    }
                                    else
                                    {
                                        DateTime Date = DateTime.Now;

                                        using (SqlConnection connection1 = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd = new SqlCommand("USP_InsertCountOfCustLogins", connection1);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@MobileNO", MobileNo);
                                            cmd.Parameters.AddWithValue("@SessionID", SessionId);
                                            cmd.Parameters.AddWithValue("@LoginTime", Date);
                                            cmd.Parameters.AddWithValue("@IsLogin", true);
                                            connection1.Open();
                                            SqlDataReader reader = cmd.ExecuteReader();
                                        }
                                        var mobchk = objDetails.Mobile_result.FromSqlRaw($"USP_CheckExistMobNo {MobileNo}").AsEnumerable().FirstOrDefault();
                                        var chk = mobchk.Result.ToString();
                                        if (chk == "USEREXISTS")
                                        {
                                            TempData["msg"] = "success";
                                            return Json("Success");
                                        }
                                        else
                                        {
                                            TempData["msg"] = "Unsuccess";
                                            return Json("UnSuccess");
                                        }
                                    }
                                
                            }
                            else
                            {
                                DateTime Date = DateTime.Now;

                                using (SqlConnection connection1 = new SqlConnection(conn))
                                {
                                    SqlCommand cmd = new SqlCommand("USP_InsertCountOfCustLogins", connection1);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@MobileNO", MobileNo);
                                    cmd.Parameters.AddWithValue("@SessionID", SessionId);
                                    cmd.Parameters.AddWithValue("@LoginTime", Date);
                                    cmd.Parameters.AddWithValue("@IsLogin", true);
                                    connection1.Open();
                                    SqlDataReader reader = cmd.ExecuteReader();
                                }
                                var mobchk = objDetails.Mobile_result.FromSqlRaw($"USP_CheckExistMobNo {MobileNo}").AsEnumerable().FirstOrDefault();
                                var chk = mobchk.Result.ToString();
                                if (chk == "USEREXISTS")
                                {
                                    TempData["msg"] = "success";
                                    return Json("Success");
                                }
                                else
                                {
                                    TempData["msg"] = "Unsuccess";
                                    return Json("UnSuccess");
                                }
                            }
                        }
                    }
                }
                else
                {
                    TempData["msg"] = "OTP Not Matched";
                }
                return Json("");
            }
            catch (Exception e)
            {
                error_log.WriteErrorLog(e.ToString());

                return Json("Login Failed");
            }
        }
        public string OTPAuthenticateSignIn(string OTP)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string msg = null;
                var OTPDeatails = "OTP";
                if (OTPDeatails == "OTP")
                {
                    msg = "Authenticate Successfully... ";
                }
                else
                {
                    msg = "Otp does not match";
                }
                return (msg);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());

                return ("Exception");
            }
        }
        public async Task<ActionResult> CheckMobileNo(string MobileNo)
        {
            ErrorLog error_log = new ErrorLog();
            long? id = 0;
            var encryptedMobileNo = ObjTripleDes.Decrypt(MobileNo);

            try
            {

                var mobchk = objDetails.Mobile_result.FromSqlRaw($"USP_CheckExistMobNo {encryptedMobileNo}").AsEnumerable().FirstOrDefault();
                var chk = mobchk.Result.ToString();
                if (chk == "USEREXISTS")
                {

                    return Json("Success");
                }
                else
                {

                    return Json("UnSuccess");
                }

                return Json("");
            }
            catch (Exception e)
            {
                error_log.WriteErrorLog(e.ToString());

                return Json("Login Failed");
            }
        }
        public ActionResult CustomerFlowLogout()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                var MobNo = HttpContext.Session.GetString("CustMobileNo");

                string conn = _connectionString;
                using (SqlConnection connection3 = new SqlConnection(conn))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_UpdateCustLogOut", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@MobileNo", MobNo);
                    connection3.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {
                        //var Result = reader2["RESULT"].ToString();
                    }
                }
                //objDetails.IndoAdminDetails.FromSqlRaw($"USP_UpdateAdminLogOutDateById {(Convert.ToInt64(userid))}").AsEnumerable().ToString();
                HttpContext.Session.Clear();
                HttpContext.Session.Remove(MobNo);
                HttpContext.Response.Cookies.Delete(MobNo);

                return Json("Logout");

            }
            catch (Exception ex)
            {

                error_log.WriteErrorLog(ex.ToString());

                return Json("Logout");
            }

        }
        public ActionResult SignIn()

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
    }
}
