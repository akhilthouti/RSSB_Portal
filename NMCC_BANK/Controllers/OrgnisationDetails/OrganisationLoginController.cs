using Amazon.SimpleSystemsManagement.Model;
using INDO_FIN_NET.Controllers.Organisation;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml;

namespace INDO_FIN_NET.Controllers.OrgnisationDetails
{
    public class OrganisationLoginController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        private readonly string _connectionString;

        public OrganisationLoginController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        [HttpGet]
        public async Task<ActionResult> OrganisationDetails(string userid, string BankName, string alphaservicename, string ServiceProvider, string CategoryName, string ProductName)
        {
            ErrorLog error_log = new ErrorLog();
            var errorline = "1";
            try
            {

                ClsOrgLogin objclsorg = new ClsOrgLogin();
                var UserId = ObjTripleDes.Decrypt(userid);
                HttpContext.Session.SetInt32("OrgUserId", Convert.ToInt32(UserId));
                var Bankname = ObjTripleDes.Decrypt(BankName);
                var AlphaService = ObjTripleDes.Decrypt(alphaservicename);
                var ServiceProvide = ObjTripleDes.Decrypt(ServiceProvider);
                var categoryName = ObjTripleDes.Decrypt(CategoryName);
                var product = ObjTripleDes.Decrypt(ProductName);
                if (UserId != null && Bankname != null && AlphaService != null && ServiceProvide != null && categoryName != null && product != null)
                {
                    try
                    {
                        errorline = "2";
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                        string myURLFund = "https://indodbservice.azurewebsites.net/api/OrganisationLogin/USP_GETIndoBankDetailsById?=" + UserId + "";
                        var client = new HttpClient();
                        var res = client.GetAsync(myURLFund);
                        string response = await res.Result.Content.ReadAsStringAsync();
                        var result = JsonConvert.SerializeObject(response);

                        errorline = "3";
                        if (result != null)
                        {
                            errorline = "4";
                            if (result == Bankname)
                            {
                                errorline = "5";
                                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                                string myURLFund1 = "https://indodbservice.azurewebsites.net/api/OrganisationLogin/USP_InsertLogDetails" + UserId + "" + "BNAK" + "" + Bankname + "" + AlphaService + "" + ServiceProvide + "" + categoryName + "" + product + "";
                                var client1 = new HttpClient();
                                var res1 = client.GetAsync(myURLFund1);
                                string response1 = await res1.Result.Content.ReadAsStringAsync();
                                var Insert = JsonConvert.SerializeObject(response);
                                return Json(Insert);
                                errorline = "6";
                                return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                            }
                            else
                            {
                                errorline = "7";
                                string error = "User Not Authenticate";
                                errorline = "8";
                                return new RedirectResult("https://organization.indofinnet.in/SelectedServicesByBank/SelectedServicesByBank/" + "/?" + "Error=" + error);
                            }
                        }
                        else
                        {
                            errorline = "9";
                            string error = "User Not available";
                            return new RedirectResult("https://organization.indofinnet.in/SelectedServicesByBank/SelectedServicesByBank/" + "/?" + "Error=" + error);
                        }
                    }
                    catch (Exception e)
                    {
                        error_log.WriteErrorLog(e.ToString());
                        string error = e.Message + "/" + errorline;
                        var result = objDetails.Database.ExecuteSqlRaw($"USP_IndoErrorLogs {e.Message},{"OrganisationLoginController"},{"OrganisationDetails"}");
                        return new RedirectResult("https://organization.indofinnet.in/SelectedServicesByBank/SelectedServicesByBank/" + "/?" + "Error=" + error);
                    }
                }
                else
                {
                    //ViewBag.msg = TempData["errormsg"];
                    //ViewBag.ip = TempData["Ip"];
                    //ViewBag.loginDateTime = TempData["LoginDateTime"];
                    //var s = HttpContext.Session.GetString("LStatus");
                    //if (s != null)
                    //{
                    //    if (ViewBag.loginDateTime != null && s != "True")
                    //    {
                    //        objclsorg.error = "Your previous session is already active, Please Logout.";
                    //        return View(objclsorg);
                    //    }

                    //}


                    return View(objclsorg);
                }
            }
            catch (Exception e)
            {
                error_log.WriteErrorLog(e.ToString());
                TempData["errormsg"] = e.Message + "/" + e.StackTrace;
                return RedirectToAction("OrganisationDetails", "OrganisationLogin");
            }

        }
        [HttpPost]
        public async Task<ActionResult> OrganisationDetails([FromServices] IActiveLogin objLogin, int userId, string Password, ClsOrgLogin obb1)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ClsOrgLogin obj1 = new ClsOrgLogin();

                HttpContext.Session.SetString("UseID", JsonConvert.SerializeObject(userId));
                var SessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("SessionId", SessionId);          
                string strEncryptSessionkey;
                string conn12 = _connectionString;
                //###################//
                var responce12 = "";
                using (SqlConnection connection12 = new SqlConnection(conn12))
                {
                    SqlCommand cmd = new SqlCommand("USP_OTPBasedUNBLOCK_USER", connection12);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));

                    connection12.Open();
                    SqlDataReader reader1 = cmd.ExecuteReader();
                    reader1.Read();

                    responce12 = reader1["RESULT"].ToString();
                }
                if(responce12 == "Invalid Credentials")
                {
                    return Json("Invalid Credentials");
                }
                else if (responce12 != "Unlocked")
                {
                    return Json("OTP Based Locked");
                }

                //#################//
                using (SqlConnection connection1 = new SqlConnection(conn12))
                {
                    SqlCommand cmd17 = new SqlCommand("USP_CheckUser", connection1);
                    cmd17.CommandType = CommandType.StoredProcedure;
                    cmd17.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));

                    connection1.Open();
                    SqlDataReader reader1 = cmd17.ExecuteReader();
                    reader1.Read();

                    var responce1 = reader1["FLAG"].ToString();

                    if (responce1 == "AGENT")
                    {

                        var result = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Get_AdminStatusById {(Convert.ToInt64(userId))}").AsEnumerable().FirstOrDefault();
                        if (result.IsLogin == true)
                        {
                            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
                            if (LoginStatus == "Active")
                            {
                                HttpContext.Session.SetString("UserName", result.UserName);
                                var UsrPAss = ObjTripleDes.Decrypt(Password);
                                var UserPass = ObjTripleDes.Encrypt(UsrPAss);
                                //###################//
                                string ip = "";
                                var SystemName = "";
                                if (result.IsLogin != true)
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

                                }
                                else
                                {
                                    ip = result.HostIP;
                                    SystemName = result.HostName;
                                    if (ip != null)
                                    {
                                        HttpContext.Session.SetString("ExistIP", ip);

                                    }
                                    // HttpContext.Session.SetString("ExistIP", ip);
                                }
                                //string message = "Session Expired";
                                //return RedirectToAction("Logout", "OrganisationLogin");
                            }

                        }

                        DateTime Date = DateTime.Now;
                        string conn = _connectionString;
                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand cmd = new SqlCommand("USP_InsertCountOfLogins", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                            cmd.Parameters.AddWithValue("@SessionID", SessionId);
                            cmd.Parameters.AddWithValue("@LoginTime", Date);
                            connection.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                        }
                        obj1.mobno = result.MobileNo;
                        var MobileNo = result.MobileNo;
                        HttpContext.Session.SetString("MOBNO", result.MobileNo);
                        ViewBag.agentmobno = result.MobileNo;
                        var res = result.IsLogin;
                        HttpContext.Session.SetString("LStatus", Convert.ToString(res));
                        var clearUserId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                        if (clearUserId != null)
                        {
                            //HttpContext.Session.SetString("DAEditCustomerdetailId", "");

                        }

                        //var j = HttpContext.Session.GetString("LStatus");
                        if (result != null)
                        {

                            HttpContext.Session.SetString("UserName", result.UserName);
                            var UsrPAss = ObjTripleDes.Decrypt(Password);
                            var UserPass = ObjTripleDes.Encrypt(UsrPAss);
                            //###################//
                            string ip = "";
                            var SystemName = "";
                            if (result.IsLogin != true)
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

                            }
                            else
                            {
                                ip = result.HostIP;
                                SystemName = result.HostName;
                                if (ip != null)
                                {
                                    HttpContext.Session.SetString("ExistIP", ip);

                                }
                                // HttpContext.Session.SetString("ExistIP", ip);
                            }

                            //#######################//

                            var UserResponce = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Get_AdminStatusById {(Convert.ToInt64(userId))}").AsEnumerable().FirstOrDefault();
                            if (UserResponce.LogOutDateTime != null)
                            {
                                string conn1 = _connectionString;
                                using (SqlConnection connection = new SqlConnection(conn1))
                                {
                                    SqlCommand cmd = new SqlCommand("USP_CheckAdminPasswordMatch", connection);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                                    cmd.Parameters.AddWithValue("@LoginPassword", UserPass);
                                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                    DataTable dt = new DataTable();
                                    connection.Open();
                                    adp.Fill(dt);

                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        var responce2 = reader["FLAG"].ToString();
                                        if (responce2 == "1")
                                        {
                                            using (SqlConnection connection5 = new SqlConnection(conn1))
                                            {
                                                SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                                cmd7.CommandType = CommandType.StoredProcedure;
                                                cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                                                cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                                cmd7.Parameters.AddWithValue("@HostNm", SystemName);
                                                cmd7.Parameters.AddWithValue("@HostIp", ip);
                                                connection5.Open();
                                                var Response = cmd7.ExecuteReader();
                                                strEncryptSessionkey = userId.ToString();
                                                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                                                //HttpWebRequest requestFund1 = null;
                                                //WebResponse responsefund1 = null;
                                                //string myURLFund1 = "https://indofinnetwebapi.azurewebsites.net/api//INdofinat/USP_IndoFiNet_UpdateAdminSessionKey?UserId=" + userId + "&sessionKey=" + strEncryptSessionkey + "";

                                                //requestFund1 = (HttpWebRequest)WebRequest.Create(myURLFund1);
                                                //requestFund1.Method = "POST";
                                                //requestFund1.ContentType = "application/json";
                                                //StreamWriter writerFund1 = new StreamWriter(requestFund1.GetRequestStream());
                                                //responsefund1 = requestFund1.GetResponse();
                                                //string Response_status1 = ((HttpWebResponse)responsefund1).StatusDescription.ToString(); // Display the status.            
                                                //Stream dataStreamFund1 = responsefund1.GetResponseStream(); // Get the stream containing content returned by the server.            
                                                //StreamReader incomingStreamReaderfund1 = new StreamReader(dataStreamFund1); // Open the stream using a StreamReader for easy access.            
                                                //string responseFromServerfund1 = incomingStreamReaderfund1.ReadToEnd();
                                                TempData["Ip"] = ip;
                                                TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                                //return Json("Success" + "," + result.MobileNo);
                                                return Json("Success" + "," + result.MobileNo + "," + result.IsLogin + "," + ip + "," + SystemName + "," + UserResponce.LoginDateTime);
                                            }
                                        }
                                        else
                                        {
                                            using (SqlConnection connection5 = new SqlConnection(conn1))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_UserLogin", connection5);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserName", (Convert.ToInt64(userId)));
                                                cmd1.Parameters.AddWithValue("@Password", UserPass);
                                                connection5.Open();
                                                SqlDataReader reader12 = cmd1.ExecuteReader();
                                                if (reader12.Read())
                                                {
                                                    var responce21 = reader12["Users"].ToString();
                                                    if (responce21 == "Attempt1")
                                                    {
                                                        return Json("Attempt1");
                                                    }
                                                    else if (responce21 == "Attempt2")
                                                    {
                                                        return Json("Attempt2");
                                                    }

                                                    else if (responce21 == "Locked")
                                                    {
                                                        return Json("Locked");
                                                    }
                                                    else
                                                    {
                                                        return Json("Locked");
                                                    }
                                                }
                                            }
                                            TempData["errormsg"] = "Invalid UserId or Password";
                                            return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                                        }
                                    }
                                }
                                if (true)
                                {
                                    return View();
                                }

                            }
                            else if (UserResponce.LoginDateTime != null)
                            {
                                var compareDate = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Indo_AdminCompareDateForSession {(Convert.ToInt64(userId))}").ToString();
                                if (compareDate != null)
                                {
                                    string conn2 = _connectionString;
                                    using (SqlConnection connection = new SqlConnection(conn2))
                                    {
                                        SqlCommand cmd = new SqlCommand("USP_CheckAdminPasswordMatch", connection);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                                        cmd.Parameters.AddWithValue("@LoginPassword", UserPass);
                                        connection.Open();
                                        SqlDataReader reader = cmd.ExecuteReader();
                                        if (reader.Read())
                                        {
                                            var responce2 = reader["FLAG"].ToString();
                                            if (responce2 == "1")
                                            {
                                                using (SqlConnection connection5 = new SqlConnection(conn2))
                                                {
                                                    SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                                    cmd7.CommandType = CommandType.StoredProcedure;
                                                    cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                                                    cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                                    cmd7.Parameters.AddWithValue("@HostNm", SystemName);
                                                    cmd7.Parameters.AddWithValue("@HostIp", ip);
                                                    connection5.Open();

                                                    var Response = cmd7.ExecuteReader();
                                                    strEncryptSessionkey = ObjTripleDes.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + userId.ToString() + Password.ToString()).Trim();
                                                    HttpContext.Session.SetString("SessionKey", JsonConvert.SerializeObject(strEncryptSessionkey));
                                                    string conn1 = _connectionString;
                                                    using (SqlConnection connection12 = new SqlConnection(conn1))
                                                    {
                                                        SqlCommand cmd1 = new SqlCommand("USP_IndoFiNet_UpdateAdminSessionKey", connection12);
                                                        cmd1.CommandType = CommandType.StoredProcedure;
                                                        cmd1.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                                                        cmd1.Parameters.AddWithValue("@sessionKey", strEncryptSessionkey);
                                                        connection12.Open();
                                                        cmd1.ExecuteNonQuery();
                                                        connection12.Close();
                                                        System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                                                        HttpWebRequest requestFund1 = null;
                                                        WebResponse responsefund1 = null;
                                                        string myURLFund1 = "https://indofinnetwebapi.azurewebsites.net/api//INdofinat/USP_IndoFiNet_UpdateAdminSessionKey?UserId=" + userId + "&sessionKey=" + strEncryptSessionkey + "";

                                                        requestFund1 = (HttpWebRequest)WebRequest.Create(myURLFund1);
                                                        requestFund1.Method = "POST";
                                                        requestFund1.ContentType = "application/json";
                                                        StreamWriter writerFund1 = new StreamWriter(requestFund1.GetRequestStream());
                                                        responsefund1 = requestFund1.GetResponse();
                                                        string Response_status1 = ((HttpWebResponse)responsefund1).StatusDescription.ToString(); // Display the status.            
                                                        Stream dataStreamFund1 = responsefund1.GetResponseStream();
                                                        StreamReader incomingStreamReaderfund1 = new StreamReader(dataStreamFund1); // Open the stream using a StreamReader for easy access.            
                                                        string responseFromServerfund1 = incomingStreamReaderfund1.ReadToEnd();
                                                        TempData["Ip"] = ip;
                                                        TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                                        return Json("Success" + "," + result.MobileNo + "," + result.IsLogin + "," + ip + "," + SystemName + "," + UserResponce.LoginDateTime);
                                                        //return Json("Success" + "," + result.MobileNo);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                using (SqlConnection connection12 = new SqlConnection(conn2))
                                                {
                                                    SqlCommand cmd1 = new SqlCommand("USP_UserLogin", connection12);
                                                    cmd1.CommandType = CommandType.StoredProcedure;
                                                    cmd1.Parameters.AddWithValue("@UserName", (Convert.ToInt64(userId)));
                                                    cmd1.Parameters.AddWithValue("@Password", UserPass);
                                                    connection12.Open();
                                                    //adp1.Fill(dt1);
                                                    SqlDataReader reader12 = cmd1.ExecuteReader();
                                                    if (reader12.Read())
                                                    {
                                                        var responce21 = reader12["Users"].ToString();
                                                        if (responce21 == "Attempt1")
                                                        {
                                                            return Json("Attempt1");
                                                        }
                                                        else if (responce21 == "Attempt2")
                                                        {
                                                            return Json("Attempt2");
                                                        }

                                                        else if (responce21 == "Locked")
                                                        {
                                                            return Json("Locked");
                                                        }
                                                        else
                                                        {
                                                            return Json("Locked");
                                                        }
                                                    }
                                                }
                                                TempData["Ip"] = ip;
                                                TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                                TempData["errormsg"] = "Invalid UserId or Password";
                                                return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                                            }
                                        }
                                    }
                                    if (true)
                                    {
                                        return View();
                                    }

                                }
                                else
                                {

                                    TempData["Ip"] = ip;
                                    TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                                }
                            }
                            else
                            {
                                string conn1256 = _connectionString;
                                using (SqlConnection connection = new SqlConnection(conn1256))
                                {
                                    SqlCommand cmd = new SqlCommand("USP_CheckAdminPasswordMatch", connection);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                                    cmd.Parameters.AddWithValue("@LoginPassword", UserPass);
                                    connection.Open();
                                    SqlDataReader reader = cmd.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        var responce2 = reader["FLAG"].ToString();
                                        if (responce2 == "1")
                                        {
                                            using (SqlConnection connection5 = new SqlConnection(conn1256))
                                            {
                                                SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                                cmd7.CommandType = CommandType.StoredProcedure;
                                                cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                                                cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                                cmd7.Parameters.AddWithValue("@HostNm", SystemName);
                                                cmd7.Parameters.AddWithValue("@HostIp", ip);
                                                connection5.Open();
                                                var Response = cmd7.ExecuteReader();
                                                strEncryptSessionkey = ObjTripleDes.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + userId.ToString() + Password.ToString()).Trim();
                                                HttpContext.Session.SetString("SessionKey", JsonConvert.SerializeObject(strEncryptSessionkey));
                                                string conn1 = _connectionString;
                                                using (SqlConnection connection12 = new SqlConnection(conn1))
                                                {
                                                    SqlCommand cmd1 = new SqlCommand("USP_IndoFiNet_UpdateAdminSessionKey", connection12);
                                                    cmd1.CommandType = CommandType.StoredProcedure;
                                                    cmd1.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userId)));
                                                    cmd1.Parameters.AddWithValue("@sessionKey", strEncryptSessionkey);
                                                    connection12.Open();
                                                    cmd1.ExecuteNonQuery();
                                                    connection12.Close();
                                                    System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                                                    HttpWebRequest requestFund1 = null;
                                                    WebResponse responsefund1 = null;
                                                    string myURLFund1 = "https://indofinnetwebapi.azurewebsites.net/api//INdofinat/USP_IndoFiNet_UpdateAdminSessionKey?UserId=" + userId + "&sessionKey=" + strEncryptSessionkey + "";

                                                    requestFund1 = (HttpWebRequest)WebRequest.Create(myURLFund1); // Create a request using a URL that can receive a post. 
                                                    requestFund1.Method = "POST";  // Set the Method property of the request to POST.                
                                                    requestFund1.ContentType = "application/json";
                                                    StreamWriter writerFund1 = new StreamWriter(requestFund1.GetRequestStream());
                                                    responsefund1 = requestFund1.GetResponse(); // Get the response.
                                                    string Response_status1 = ((HttpWebResponse)responsefund1).StatusDescription.ToString(); // Display the status.            
                                                    Stream dataStreamFund1 = responsefund1.GetResponseStream(); // Get the stream containing content returned by the server.            
                                                    StreamReader incomingStreamReaderfund1 = new StreamReader(dataStreamFund1); // Open the stream using a StreamReader for easy access.            
                                                    string responseFromServerfund1 = incomingStreamReaderfund1.ReadToEnd();
                                                    TempData["Ip"] = ip;
                                                    TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                                    return Json("Success" + "," + result.MobileNo + "," + result.IsLogin + "," + ip + "," + SystemName + "," + UserResponce.LoginDateTime);
                                                    //return Json("Success" + "," + result.MobileNo);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            using (SqlConnection connection12 = new SqlConnection(conn1256))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_UserLogin", connection12);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserName", (Convert.ToInt64(userId)));
                                                cmd1.Parameters.AddWithValue("@Password", UserPass);
                                                connection12.Open();
                                                SqlDataReader reader12 = cmd1.ExecuteReader();
                                                if (reader12.Read())
                                                {
                                                    var responce21 = reader12["Users"].ToString();
                                                    if (responce21 == "Attempt1")
                                                    {
                                                        return Json("Attempt1");
                                                    }
                                                    else if (responce21 == "Attempt2")
                                                    {
                                                        return Json("Attempt2");
                                                    }

                                                    else if (responce21 == "Locked")
                                                    {
                                                        return Json("Locked");
                                                    }
                                                    else
                                                    {
                                                        return Json("Locked");
                                                    }
                                                }
                                            }
                                            TempData["errormsg"] = "Invalid UserId or Password";
                                            return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                                        }
                                    }
                                }
                                if (true)
                                {
                                    return View();
                                }
                            }
                        }
                    }
                    else
                    {
                        TempData["errormsg"] = "User Not Authenticate";
                        return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                    }
                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                }


            }
            catch (Exception e)
            {
                error_log.WriteErrorLog(e.ToString());
                TempData["errormsg"] = e.Message + "/" + e.StackTrace;
                return RedirectToAction("OrganisationDetails", "OrganisationLogin");
            }
        }
        public ActionResult Logout()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ClsOrgLogin objclsorg = new ClsOrgLogin();
                var userid = HttpContext.Session.GetString("UseID");
                objclsorg.UserId = userid;
                string conn = _connectionString;
                using (SqlConnection connection3 = new SqlConnection(conn))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_UpdateAdminLogOutDateById", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@UserId", userid);
                    connection3.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {
                        //var Result = reader2["RESULT"].ToString();
                    }
                }
                //objDetails.IndoAdminDetails.FromSqlRaw($"USP_UpdateAdminLogOutDateById {(Convert.ToInt64(userid))}").AsEnumerable().ToString();
                HttpContext.Session.Clear();
                HttpContext.Session.Remove(objclsorg.UserId);
                HttpContext.Response.Cookies.Delete(objclsorg.UserId);
                HttpContext.SignOutAsync();
                return Json("Logout");

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                HttpContext.Session.Clear();
                HttpContext.Session.Remove("objuser");
                HttpContext.SignOutAsync();
                return Json("Logout");
            }

        }
        [HttpGet]
        public ActionResult ForgetPass()
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
        public ActionResult ChangePass(string Pass)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var Userid = HttpContext.Session.GetString("ForgetId");
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_UpdatePassword", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userid", Userid);
                    cmd.Parameters.AddWithValue("@Password", ObjTripleDes.Encrypt(Pass));
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    return Json("Success");
                }
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        [HttpPost]
        public ActionResult CheckUserId(string Userid)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                HttpContext.Session.SetString("ForgetId", Userid);
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_UserIdCheck", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", Userid);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var result = reader["result"].ToString();
                        connection.Close();
                        if (result == "USEREXISTS")
                        {
                            var result1 = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Get_AdminStatusById {(Convert.ToInt64(Userid))}").AsEnumerable().FirstOrDefault();

                            var MobileNo = result1.MobileNo;
                            return Json("Success" + "," + result1.MobileNo);
                        }
                        else
                        {
                            return Json("failed");
                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult MainHomePage()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                DateTime birthDate = new DateTime(1997, 5, 17);
                DateTime today = DateTime.Today;
                int age = today.Year - birthDate.Year;

                // Check if the birthday has not occurred yet this year

                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        [HttpGet]
        public ActionResult touploadFiletoDB()
        {
            return View();
        }



        [HttpGet]
        public ActionResult CustomerRegistration()
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
