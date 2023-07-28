
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using INDO_FIN_NET.Repository.Data;
using System.Net;
using System.IO;
using Microsoft.Data.SqlClient;
using ServiceProvider1.Models.Admin;
using System.Configuration;
using System.Text;

using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;

using System.ComponentModel;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs.Specialized;
using RestSharp;
using INDO_FIN_NET.Controllers.Organisation;
using Microsoft.Extensions.Configuration;
using System.Net.Sockets;

namespace INDO_FIN_NET.Controllers
{

    public class AdminLoginController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        private readonly string _connectionString;

        public AdminLoginController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }

        TripleDESImplementation objtriple = new TripleDESImplementation();
        clsAddNewUser objAddUsers = new clsAddNewUser();
        ClsUser objuser = new ClsUser();
        string imgtypePhoto = "";
        string imgtype_POI = "";
        string imgtype_CA = "";
        byte[] dochistory_Photo = null;
        byte[] dochistory_POI = null;
        byte[] dochistory_CA = null;
        long? result;

        [HttpGet]
        public IActionResult UserDetails()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ClsUser objuser = new ClsUser();
                //ViewBag.msg = TempData["msg"];
                //ViewBag.ip = TempData["Ip"];
                //ViewBag.loginDateTime = TempData["LoginDateTime"];
                //if (ViewBag.loginDateTime != null)
                //{
                //    objuser.error = "Your previous session is already active, Please Logout.";
                //    return View(objuser);
                //}

                return View(objuser);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//JsonRequestBehavior.AllowGet
            }
        }

        [HttpPost]

        public ActionResult UserDetails([FromServices] IActiveLogin objLogin, ClsUser objuser)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ClsUser obj1 = new ClsUser();
                HttpWebRequest requestFund = null;
                WebResponse responsefund = null;
                var SessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("SessionId", SessionId);
                HttpContext.Session.SetString("UserID", JsonConvert.SerializeObject(objuser.UserId));
                var UserPass = objuser.Password;
                string strEncryptSessionkey;
                string conn12 = _connectionString;
                using (SqlConnection connection1 = new SqlConnection(conn12))
                {
                    SqlCommand cmd17 = new SqlCommand("USP_CheckUser", connection1);
                    cmd17.CommandType = CommandType.StoredProcedure;
                    cmd17.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                    connection1.Open();
                    SqlDataReader reader = cmd17.ExecuteReader();
                    reader.Read();
                    var responce1 = reader["FLAG"].ToString();
                    if (responce1 == "ADMIN")
                    {
                        var SystemName = "";
                        var result = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Get_AdminStatusById {(Convert.ToInt64(objuser.UserId))}").AsEnumerable().FirstOrDefault();
                        if (result.IsLogin == true)
                        {
                            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
                            if (LoginStatus == "Active")
                            {

                                //###################//
                                string ip = "";
                                
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
                        string conn145 = _connectionString;
                        using (SqlConnection connection = new SqlConnection(conn145))
                        {
                            SqlCommand cmd = new SqlCommand("USP_InsertCountOfLogins", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                            cmd.Parameters.AddWithValue("@SessionID", SessionId);
                            cmd.Parameters.AddWithValue("@LoginTime", Date);
                            connection.Open();
                            SqlDataReader reader1 = cmd.ExecuteReader();
                        }
                        obj1.mobno = result.MobileNo;
                        var MobileNo = result.MobileNo;
                        HttpContext.Session.SetString("MOBNO", MobileNo);
                        ViewBag.agentmobno = result.MobileNo;
                        if (result != null)
                        {
                            HttpContext.Session.SetString("UserRole", JsonConvert.SerializeObject(result.RoleId));
                            HttpContext.Session.SetString("UserName", JsonConvert.SerializeObject(result.UserName));
                            var ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            if (string.IsNullOrEmpty(ip))
                            {
                                ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                            }
                            var UserResponce = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Get_AdminDetailsById {(Convert.ToInt64(objuser.UserId))}").AsEnumerable().FirstOrDefault();
                            if (UserResponce.LogOutDateTime != null)
                            {
                                string conn = _connectionString;
                                using (SqlConnection connection2 = new SqlConnection(conn))
                                {
                                    SqlCommand cmd = new SqlCommand("USP_CheckAdminPasswordMatch", connection2);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                    cmd.Parameters.AddWithValue("@LoginPassword", UserPass);
                                    connection2.Open();
                                    SqlDataReader reader2 = cmd.ExecuteReader();
                                    reader2.Read();
                                    var responce = reader2["FLAG"].ToString();
                                    int resp = Convert.ToInt32(responce);
                                    if (resp == 1 && result.RoleId != 5 && result.RoleId != 3 && result.RoleId != 7)
                                    {
                                        using (SqlConnection connection5 = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                            cmd7.CommandType = CommandType.StoredProcedure;
                                            cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                            cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                            connection5.Open();
                                            var Response = cmd7.ExecuteReader();
                                            strEncryptSessionkey = objtriple.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + objuser.UserId.ToString() + objuser.Password.ToString()).Trim();
                                            objDetails.IndoAdminDetails.FromSqlRaw($"USP_IndoFiNet_UpdateAdminSessionKey {(Convert.ToInt64(objuser.UserId))},{strEncryptSessionkey}").AsEnumerable().ToString();
                                            string conn1 = _connectionString;
                                            using (SqlConnection connection = new SqlConnection(conn1))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_IndoFiNet_UpdateAdminSessionKey", connection);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                                cmd1.Parameters.AddWithValue("@sessionKey", strEncryptSessionkey);

                                                connection.Open();
                                                cmd1.ExecuteNonQuery();
                                                connection.Close();

                                                HttpContext.Session.SetString("SessionKey", JsonConvert.SerializeObject(strEncryptSessionkey));
                                                var utype = objDetails.Database.ExecuteSqlRaw($"USP_Get_RoleType {result.RoleId}");

                                                ViewBag.RoleType = utype;
                                                TempData["Ip"] = ip;
                                                TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                                return Json("Success" + "," + result.MobileNo + "," + result.IsLogin + "," + ip + "," + SystemName + "," + UserResponce.LoginDateTime);
                                            }
                                        }
                                    }
                                    else if (resp == 1 && result.RoleId == 5)                     ///VkycAgent Role
                                    {
                                        using (SqlConnection connection5 = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                            cmd7.CommandType = CommandType.StoredProcedure;
                                            cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                            cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                            connection5.Open();

                                            var Response1 = cmd7.ExecuteReader();
                                            strEncryptSessionkey = objtriple.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + objuser.UserId.ToString() + objuser.Password.ToString()).Trim();
                                            objDetails.IndoAdminDetails.FromSqlRaw($"USP_IndoFiNet_UpdateAdminSessionKey {(Convert.ToInt64(objuser.UserId))},{strEncryptSessionkey}").AsEnumerable().ToString();
                                            string conn1 = _connectionString;
                                            using (SqlConnection connection = new SqlConnection(conn1))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_IndoFiNet_UpdateAdminSessionKey", connection);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                                cmd1.Parameters.AddWithValue("@sessionKey", strEncryptSessionkey);

                                                connection.Open();
                                                cmd1.ExecuteNonQuery();
                                                connection.Close();

                                                HttpContext.Session.SetString("SessionKey", JsonConvert.SerializeObject(strEncryptSessionkey));

                                                long? Response = objDetails.IndoAdminDetails.FromSqlRaw($"usp_indo_UpdateLoginFlag{(Convert.ToInt64(objuser.UserId))}").LongCount();
                                                var utype = objDetails.Database.ExecuteSqlRaw($"USP_Get_RoleType {result.RoleId}");
                                                TempData["RoleType"] = utype;
                                                TempData["Ip"] = ip;
                                                TempData["LoginDateTime"] = UserResponce.LoginDateTime;

                                                return RedirectToAction("Admin", "VCIPAdmin");
                                            }
                                        }
                                    }
                                    else if (resp == 1 && result.RoleId == 3)                     //VkycAdmin Roles
                                    {
                                        using (SqlConnection connection5 = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                            cmd7.CommandType = CommandType.StoredProcedure;
                                            cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                            cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                            connection5.Open();

                                            var Response = cmd7.ExecuteReader();
                                            strEncryptSessionkey = objtriple.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + objuser.UserId.ToString() + objuser.Password.ToString()).Trim();
                                            objDetails.IndoAdminDetails.FromSqlRaw($"USP_IndoFiNet_UpdateAdminSessionKey {(Convert.ToInt64(objuser.UserId))},{strEncryptSessionkey}").AsEnumerable().ToString();
                                            string conn1 = _connectionString;
                                            using (SqlConnection connection = new SqlConnection(conn1))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_IndoFiNet_UpdateAdminSessionKey", connection);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                                cmd1.Parameters.AddWithValue("@sessionKey", strEncryptSessionkey);

                                                connection.Open();
                                                cmd1.ExecuteNonQuery();
                                                connection.Close();

                                                HttpContext.Session.SetString("SessionKey", JsonConvert.SerializeObject(strEncryptSessionkey));
                                                var utype = objDetails.Database.ExecuteSqlRaw($"USP_Get_RoleType {result.RoleId}");
                                                TempData["RoleType"] = utype;
                                                TempData["Ip"] = ip;
                                                TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                                return RedirectToAction("SuperAdminDashboard", "AdminLogin");
                                                //return RedirectToAction("OfficerPage", "VCIPAdmin");
                                            }
                                        }
                                    }
                                    else if (resp == 1 && result.RoleId == 7)                          //Loan Officer Role
                                    {
                                        using (SqlConnection connection5 = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                            cmd7.CommandType = CommandType.StoredProcedure;
                                            cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                            cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                            connection5.Open();

                                            var Response = cmd7.ExecuteReader();
                                            strEncryptSessionkey = objtriple.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + objuser.UserId.ToString() + objuser.Password.ToString()).Trim();
                                            objDetails.IndoAdminDetails.FromSqlRaw($"USP_IndoFiNet_UpdateAdminSessionKey {(Convert.ToInt64(objuser.UserId))},{strEncryptSessionkey}").AsEnumerable().ToString();
                                            string conn1 = _connectionString;
                                            using (SqlConnection connection = new SqlConnection(conn1))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_IndoFiNet_UpdateAdminSessionKey", connection);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                                cmd1.Parameters.AddWithValue("@sessionKey", strEncryptSessionkey);

                                                connection.Open();
                                                cmd1.ExecuteNonQuery();
                                                connection.Close();

                                                HttpContext.Session.SetString("SessionKey", JsonConvert.SerializeObject(strEncryptSessionkey));

                                                var utype = objDetails.Database.ExecuteSqlRaw($"USP_Get_RoleType {result.RoleId}");
                                                TempData["RoleType"] = utype;
                                                TempData["Ip"] = ip;
                                                TempData["LoginDateTime"] = UserResponce.LoginDateTime;

                                                return RedirectToAction("LoanOfficerLoginView", "NsdlEsign");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        using (SqlConnection connection15 = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd1 = new SqlCommand("USP_UserLogin", connection15);
                                            cmd1.CommandType = CommandType.StoredProcedure;
                                            cmd1.Parameters.AddWithValue("@UserName", (Convert.ToInt64(objuser.UserId)));
                                            cmd1.Parameters.AddWithValue("@Password", UserPass);
                                            connection15.Open();
                                            SqlDataReader reader1 = cmd1.ExecuteReader();
                                            if (reader1.Read())
                                            {
                                                var responce21 = reader1["Users"].ToString();
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
                                        TempData["msg"] = "Invalid UserId or Password";
                                        return RedirectToAction("UserDetails", "AdminLogin");
                                    }
                                }
                            }
                            else if (UserResponce.LoginDateTime != null)
                            {
                                var compareDate = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Indo_AdminCompareDateForSession {(Convert.ToInt64(objuser.UserId))}").ToString();

                                if (compareDate != "Allow")
                                {
                                    string conn = _connectionString;
                                    using (SqlConnection connection2 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd = new SqlCommand("USP_CheckAdminPasswordMatch", connection2);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                        cmd.Parameters.AddWithValue("@LoginPassword", UserPass);

                                        connection2.Open();
                                        SqlDataReader reader2 = cmd.ExecuteReader();
                                        reader2.Read();

                                        var resp = reader2["FLAG"].ToString();
                                        int responce = Convert.ToInt32(resp);
                                        if (responce == 1 && result.RoleId != 5 && result.RoleId != 3)
                                        {
                                            strEncryptSessionkey = objtriple.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + objuser.UserId.ToString() + objuser.Password.ToString()).Trim();
                                            objDetails.IndoAdminDetails.FromSqlRaw($"USP_IndoFiNet_UpdateAdminSessionKey {(Convert.ToInt64(objuser.UserId))},{strEncryptSessionkey}").AsEnumerable().ToString();
                                            string conn1 = _connectionString;
                                            using (SqlConnection connection = new SqlConnection(conn1))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_IndoFiNet_UpdateAdminSessionKey", connection);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                                cmd1.Parameters.AddWithValue("@sessionKey", strEncryptSessionkey);

                                                connection.Open();
                                                cmd1.ExecuteNonQuery();
                                                connection.Close();

                                                HttpContext.Session.SetString("SessionKey", JsonConvert.SerializeObject(strEncryptSessionkey));
                                                TempData["Ip"] = ip;
                                                TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                                return Json("Success" + "," + result.MobileNo + "," + result.IsLogin + "," + ip + "," + SystemName + "," + UserResponce.LoginDateTime);
                                            }
                                        }
                                        else if (responce == 1 && result.RoleId == 5)
                                        {
                                            using (SqlConnection connection5 = new SqlConnection(conn))
                                            {
                                                SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                                cmd7.CommandType = CommandType.StoredProcedure;
                                                cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                                cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                                connection5.Open();

                                                var Response = cmd7.ExecuteReader();
                                                return RedirectToAction("Admin", "VCIPAdmin");
                                            }
                                        }
                                        else if (responce == 1 && result.RoleId == 3)
                                        {
                                            using (SqlConnection connection5 = new SqlConnection(conn))
                                            {
                                                SqlCommand cmd7 = new SqlCommand("USP_UpdateBlockUser", connection5);
                                                cmd7.CommandType = CommandType.StoredProcedure;
                                                cmd7.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                                cmd7.Parameters.AddWithValue("@LoginPassword", UserPass);
                                                connection5.Open();

                                                var Response = cmd7.ExecuteReader();
                                                return RedirectToAction("OfficerPage", "VCIPAdmin");
                                            }
                                        }
                                        else
                                        {
                                            using (SqlConnection connection10 = new SqlConnection(conn))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_UserLogin", connection10);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserName", (Convert.ToInt64(objuser.UserId)));
                                                cmd1.Parameters.AddWithValue("@Password", UserPass);
                                                connection1.Open();

                                                SqlDataReader reader1 = cmd1.ExecuteReader();
                                                if (reader1.Read())
                                                {
                                                    var responce21 = reader1["Users"].ToString();
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
                                            TempData["msg"] = "Invalid UserId or Password";
                                            return RedirectToAction("UserDetails", "AdminLogin");
                                        }
                                    }
                                }
                                else
                                {
                                    TempData["Ip"] = ip;
                                    TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                    return RedirectToAction("UserDetails", "AdminLogin");
                                }
                            }
                            else
                            {
                                var compareDate = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Indo_AdminCompareDateForSession {(Convert.ToInt64(objuser.UserId))}").ToString();

                                if (compareDate != "Allow")
                                {
                                    string conn = _connectionString;
                                    using (SqlConnection connection2 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd = new SqlCommand("USP_CheckAdminPasswordMatch", connection2);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                        cmd.Parameters.AddWithValue("@LoginPassword", UserPass);

                                        connection2.Open();
                                        SqlDataReader reader2 = cmd.ExecuteReader();
                                        reader2.Read();

                                        var resp = reader2["FLAG"].ToString();
                                        int responce = Convert.ToInt32(resp);
                                        if (responce == 1 && result.RoleId != 5 && result.RoleId != 3)
                                        {
                                            strEncryptSessionkey = objtriple.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + objuser.UserId.ToString() + objuser.Password.ToString()).Trim();
                                            objDetails.IndoAdminDetails.FromSqlRaw($"USP_IndoFiNet_UpdateAdminSessionKey {(Convert.ToInt64(objuser.UserId))},{strEncryptSessionkey}").AsEnumerable().ToString();
                                            string conn1 = _connectionString;
                                            using (SqlConnection connection = new SqlConnection(conn1))
                                            {
                                                SqlCommand cmd1 = new SqlCommand("USP_IndoFiNet_UpdateAdminSessionKey", connection);
                                                cmd1.CommandType = CommandType.StoredProcedure;
                                                cmd1.Parameters.AddWithValue("@UserId", (Convert.ToInt64(objuser.UserId)));
                                                cmd1.Parameters.AddWithValue("@sessionKey", strEncryptSessionkey);

                                                connection.Open();
                                                cmd1.ExecuteNonQuery();
                                                connection.Close();

                                                HttpContext.Session.SetString("SessionKey", JsonConvert.SerializeObject(strEncryptSessionkey));
                                                TempData["Ip"] = ip;
                                                TempData["LoginDateTime"] = UserResponce.LoginDateTime;
                                                return Json("Success" + "," + result.MobileNo + "," + result.IsLogin + "," + ip + "," + SystemName + "," + UserResponce.LoginDateTime);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string conn = _connectionString;
                                    using (SqlConnection connection17 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd1 = new SqlCommand("USP_UserLogin", connection17);
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.AddWithValue("@UserName", (Convert.ToInt64(objuser.UserId)));
                                        cmd1.Parameters.AddWithValue("@Password", UserPass);
                                        connection1.Open();
                                        SqlDataReader reader1 = cmd1.ExecuteReader();
                                        if (reader1.Read())
                                        {
                                            var responce21 = reader1["Users"].ToString();

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
                                    TempData["msg"] = "Invalid UserId or Password";
                                    return RedirectToAction("UserDetails", "AdminLogin");
                                }
                            }
                        }
                    }
                    else
                    {
                        TempData["msg"] = "User Not Authenticate";
                        return RedirectToAction("UserDetails", "AdminLogin");
                    }
                    return RedirectToAction("UserDetails", "AdminLogin");
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                TempData["msg"] = "Some Error Occure";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
        }

        [HttpGet]
        public ActionResult AddNewUser([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                clsAddNewUser objAddUser = new clsAddNewUser();

                var verificationtype = (from details in objDetails.AdmRegiontbls.FromSqlRaw($"USP_Get_Region {objAddUser.RegionList}").ToList()
                                        select new SelectListItem()
                                        {
                                            Text = details.RegionName,
                                            Value = details.RegionId.ToString()
                                        }).ToList();
                verificationtype.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });

                ViewBag.Region = verificationtype;//new SelectList("RegionList","Name");//new SelectList("RegionList", "Name");// new SelectList(objDetails.AdmRegiontbls.FromSqlRaw($"USP_Get_Region").AsEnumerable().ToString(), "RegionId", "RegionName");
                var verificationtype1 = (from details in objDetails.admBranchDetailsS.FromSqlRaw($"USP_GetAllBranchDetailsRSSB").ToList()
                                        select new SelectListItem()
                                        {
                                            Text = details.branch_description.ToString(),
                                            Value = details.branch_code.ToString(),

                                        }).ToList();
                verificationtype.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.Branch = verificationtype1;
                
                var verificationtype2 = (from details in objDetails.AdmDepartmenttbls.FromSqlRaw($"USP_Get_Department {objAddUser.DepartmentList}").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.DeptName,
                                             Value = details.DeptId.ToString(),
                                         }).ToList();
                verificationtype2.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.Department = verificationtype2;// new SelectList("DepartmentList", "Name");// new SelectList(objDetails.AdmRoletbls.FromSqlRaw($"USP_Get_Role").AsEnumerable().ToString(), "RoleId", "Roletype", objAddUsers.DepartmentList);
                var verificationtype3 = (from details in objDetails.AdmRoletbls.FromSqlRaw($"USP_Get_Role {objAddUser.RoleList}").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Roletype,
                                             Value = details.RoleId.ToString(),
                                         }).ToList();
                verificationtype3.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.Role = verificationtype3;
                ViewBag.msg = TempData["msg"];
                return View(objAddUser);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                TempData["msg"] = ex.Message;
                ViewBag.msg = TempData["msg"];
                return View();
            }
        }
        public ActionResult ActiveUers([FromServices] IActiveLogin objLogin, string UserId)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_UnBlockUser", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", UserId);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                }
                return Json("Success");
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        public ActionResult BlockUsers([FromServices] IActiveLogin objLogin, string UserId)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_BlockUser", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userId", UserId);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                }
                return Json("Success");
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNewUser([FromServices] IActiveLogin objLogin, clsAddNewUser objAddUser)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                objAddUser.Password = objtriple.Encrypt(objAddUser.Password);
                objAddUser.ConfirmPassword = objtriple.Encrypt(objAddUser.ConfirmPassword);

                string AddUser = JsonConvert.SerializeObject(objAddUser);
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_UserExitsCheck", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", objAddUser.UserId);
                    cmd.Parameters.AddWithValue("@UserName", objAddUser.Username);
                    cmd.Parameters.AddWithValue("@MobileNo", objAddUser.MobileNo);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var result = reader["result"].ToString();
                        connection.Close();
                        if (result == "NOTEXISTS")
                        {
                            using (SqlConnection cn = new SqlConnection(_connectionString))
                            {
                                cn.Open();
                                SqlCommand cmd1 = new SqlCommand("USP_AddNewAdminDetails_New", cn);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@UserId", objAddUser.UserId);
                                cmd1.Parameters.AddWithValue("@UserName", objAddUser.Username);
                                cmd1.Parameters.AddWithValue("@Address", objAddUser.Address);
                                cmd1.Parameters.AddWithValue("@MobileNo", objAddUser.MobileNo);
                                cmd1.Parameters.AddWithValue("@EmailId", objAddUser.EmailId);
                                cmd1.Parameters.AddWithValue("@Region", objAddUser.RegionList);
                                cmd1.Parameters.AddWithValue("@Branch", objAddUser.BranchList);
                                cmd1.Parameters.AddWithValue("@DepartmentId", objAddUser.DepartmentList);
                                cmd1.Parameters.AddWithValue("@RoleId", objAddUser.RoleList);
                                cmd1.Parameters.AddWithValue("@Password", objAddUser.Password);
                                cmd1.Parameters.AddWithValue("@OrganizationID", "");
                                cmd1.Parameters.AddWithValue("@OrganizationName", "");
                                cmd1.Parameters.AddWithValue("@Roletype", "");
                                cmd1.ExecuteNonQuery();
                                TempData["msg"] = "User Added Successfully";
                                return RedirectToAction("AddNewUser", "AdminLogin");
                            };
                        }
                        else
                        {
                            TempData["msg"] = "User already exists with the same Mobile Number";
                            return RedirectToAction("AddNewUser", "AdminLogin");
                        }
                    }
                    else
                    {
                        TempData["msg"] = "User already exists with the same Mobile Number";
                        return RedirectToAction("AddNewUser", "AdminLogin");
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());

                TempData["msg"] = ex.Message;//"Some Error Occure";
                return RedirectToAction("AddNewUser", "AdminLogin");
            }
        }

        [HttpGet]
        public ActionResult CustDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        public ActionResult SubmitToFinacleCheck([FromServices] IActiveLogin objLogin, string AdminCustId)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                var CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId"));
                string conn22 = _connectionString;
                using (SqlConnection connection22 = new SqlConnection(conn22))
                {
                    SqlCommand cmd22 = new SqlCommand("USP_FlagMaintainData", connection22);
                    cmd22.CommandType = CommandType.StoredProcedure;
                    cmd22.Parameters.AddWithValue("@CustomerId", CustomerDetailId);
                    connection22.Open();
                    SqlDataReader reader22 = cmd22.ExecuteReader();
                    if (reader22.Read())
                    {
                        var QuickEnrollRejectReason = reader22[21].ToString();
                        var CAFRejectReason = reader22[23].ToString();
                        var DocumentRejectReason = reader22[25].ToString();
                        var IpvRejectReason = reader22[27].ToString();
                        var SavingRejectedReason = reader22[60].ToString();

                        if (QuickEnrollRejectReason == "Not reject" && CAFRejectReason == "Not reject" && DocumentRejectReason == "Not reject" && IpvRejectReason == "Not reject" && SavingRejectedReason == "Not reject")
                        {
                            string result = "1";
                            return Json(result);
                        }
                        else
                        {
                            string result = "0";
                            return Json(result);
                        }
                    }
                }
                return View();
            }
            catch (Exception Ex)
            {
                //PortalException.InsertPortalException(Ex);
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        [HttpGet]
        public ActionResult AddUserGrid([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                //PortalException.InsertPortalException(Ex);
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }

        [HttpPost]
        public string UserGrid()
        {
            ErrorLog error_log = new ErrorLog();
            IndoAdminDetail objmain2 = new IndoAdminDetail();
            try
            {
                var list = objDetails.IndoAdminDetails.FromSqlRaw($"USP_GetUserDetails").ToList().AsEnumerable().ToList();

                var p = JsonConvert.SerializeObject(list);

                var resp1 = "{\"data\":" + p + "}";

                return resp1;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return "";
            }
        }


        public ActionResult PanDataVerify([FromServices] IActiveLogin objLogin, long? CustomerDetailId)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            var result = HttpContext.Session.GetString("DAEditCustomerdetailId");
            if (result != null)
            {
                HttpContext.Session.SetString("PersonalId", result);
                CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId"));

                ViewBag.AdminFlag = "AdminFlag";
            }
            ClsDataVerification objPanDocumentDetails = new ClsDataVerification();

            var CustQE = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerQEData {CustomerDetailId}").AsEnumerable().FirstOrDefault();
            var AdmCustomerDocumentsD = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetCustomerDetails1 {CustomerDetailId}").AsEnumerable().FirstOrDefault();
            var panObj = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetdigipanForMatches {CustomerDetailId}").AsEnumerable().FirstOrDefault();
            try
            {
                if (panObj != null)
                {
                    if (CustQE.MiddleName != null)
                    {
                        objPanDocumentDetails.PanComFirstName = panObj.Firstname;
                        objPanDocumentDetails.PanComMiddleName = panObj.Middlename;
                        objPanDocumentDetails.PanComLastName = panObj.Lastname;
                        if (panObj.Panno.Length == 10)
                        {
                            objPanDocumentDetails.PanComNo = panObj.Panno;
                        }
                        else
                        {
                            objPanDocumentDetails.PanComNo = objtriple.Decrypt(panObj.Panno);
                        }
                        objPanDocumentDetails.QEFirstName = CustQE.FirstName;
                        objPanDocumentDetails.QEMiddleName = CustQE.MiddleName;
                        objPanDocumentDetails.QELastName = CustQE.LastName;
                        objPanDocumentDetails.QEPanNo = objtriple.Decrypt(CustQE.PanNo);
                        objPanDocumentDetails.QEFatherName = CustQE.FatherName;
                        objPanDocumentDetails.QEMobileNo = CustQE.MobileNo;
                        objPanDocumentDetails.QEEmailId = CustQE.EmailId;
                        objPanDocumentDetails.QECLIENT_ADDRESS_1 = CustQE.ClientAddress1;
                        objPanDocumentDetails.QECLIENT_ADDRESS_2 = CustQE.ClientAddress2;
                        objPanDocumentDetails.QECLIENT_ADDRESS_3 = CustQE.ClientAddress3;
                        if (CustQE.CountryId == "101")
                        {
                            objPanDocumentDetails.QECountry = "INDIA";
                        }
                        else
                        {
                            objPanDocumentDetails.QECountry = CustQE.ClientPermCountry;
                        }
                        objPanDocumentDetails.QECity = CustQE.CityId;
                        objPanDocumentDetails.QEPin = CustQE.PinCode;
                        objPanDocumentDetails.QECLIENT_PERM_ADDRESS_1 = CustQE.ClientPermAddress1;
                        objPanDocumentDetails.QECLIENT_PERM_ADDRESS_2 = CustQE.ClientPermAddress2;
                        objPanDocumentDetails.QECLIENT_PERM_ADDRESS_3 = CustQE.ClientPermAddress3;
                        objPanDocumentDetails.QECLIENT_PERM_CITY = CustQE.ClientPermCity;
                        objPanDocumentDetails.QECLIENT_PERM_COUNTRY = CustQE.ClientPermCountry;
                        objPanDocumentDetails.QELive_Photo = Convert.ToBase64String(AdmCustomerDocumentsD.DocumentHistory);
                        objPanDocumentDetails.FirstNameStatus = string.Equals(panObj.Firstname.ToLower(), CustQE.FirstName.ToLower()) ? "Match" : "Does Not Match";
                        objPanDocumentDetails.MiddleNameStatus = string.Equals(panObj.Middlename.ToLower(), CustQE.MiddleName.ToLower()) ? "Match" : "Does Not Match";
                        objPanDocumentDetails.LastNameStatus = string.Equals(panObj.Lastname.ToLower(), CustQE.LastName.ToLower()) ? "Match" : "Does Not Match";
                        objPanDocumentDetails.PanNoStatus = string.Equals(objPanDocumentDetails.PanComNo.ToLower(), objPanDocumentDetails.QEPanNo.ToLower()) ? "Match" : "Does Not Match";
                    }
                    else
                    {
                        objPanDocumentDetails.PanComFirstName = panObj.Firstname;
                        objPanDocumentDetails.PanComMiddleName = panObj.Middlename;
                        objPanDocumentDetails.PanComLastName = panObj.Lastname;
                        objPanDocumentDetails.PanComNo = panObj.Panno;
                        objPanDocumentDetails.QEFirstName = CustQE.FirstName;
                        objPanDocumentDetails.QEMiddleName = CustQE.MiddleName;
                        objPanDocumentDetails.QELastName = CustQE.LastName;
                        objPanDocumentDetails.QEPanNo = objtriple.Decrypt(CustQE.PanNo);
                        objPanDocumentDetails.QEFatherName = CustQE.FatherName;
                        objPanDocumentDetails.QEMobileNo = CustQE.MobileNo;
                        objPanDocumentDetails.QEEmailId = CustQE.EmailId;
                        objPanDocumentDetails.QECLIENT_ADDRESS_1 = CustQE.ClientAddress1;
                        objPanDocumentDetails.QECLIENT_ADDRESS_2 = CustQE.ClientAddress2;
                        objPanDocumentDetails.QECLIENT_ADDRESS_3 = CustQE.ClientAddress3;
                        if (CustQE.CountryId == "101")
                        {
                            objPanDocumentDetails.QECountry = "INDIA";
                        }
                        else
                        {
                            objPanDocumentDetails.QECountry = CustQE.ClientPermCountry;
                        }
                        objPanDocumentDetails.QECity = CustQE.CityId;
                        objPanDocumentDetails.QEPin = CustQE.PinCode;
                        objPanDocumentDetails.QECLIENT_PERM_ADDRESS_1 = CustQE.ClientPermAddress1;
                        objPanDocumentDetails.QECLIENT_PERM_ADDRESS_2 = CustQE.ClientPermAddress2;
                        objPanDocumentDetails.QECLIENT_PERM_ADDRESS_3 = CustQE.ClientPermAddress3;
                        objPanDocumentDetails.QECLIENT_PERM_CITY = CustQE.ClientPermCity;
                        objPanDocumentDetails.QECLIENT_PERM_COUNTRY = CustQE.ClientPermCountry;
                        if (CustQE.LivePhoto != null)
                        {
                            objPanDocumentDetails.QELive_Photo = Convert.ToBase64String(CustQE.LivePhoto);
                        }
                        objPanDocumentDetails.FirstNameStatus = string.Equals(panObj.Firstname.ToLower(), CustQE.FirstName.ToLower()) ? "Macth" : "Not Match";
                        objPanDocumentDetails.MiddleNameStatus = "N/A";
                        objPanDocumentDetails.LastNameStatus = string.Equals(panObj.Lastname.ToLower(), CustQE.LastName.ToLower()) ? "Macth" : "Not Match";
                        objPanDocumentDetails.PanNoStatus = "N/A";
                    }
                }
                else
                {
                    objPanDocumentDetails.QEFirstName = CustQE.FirstName;
                    objPanDocumentDetails.QEMiddleName = CustQE.MiddleName;
                    objPanDocumentDetails.QELastName = CustQE.LastName;
                    objPanDocumentDetails.QEPanNo = objtriple.Decrypt(CustQE.PanNo);
                    objPanDocumentDetails.QEFatherName = CustQE.FatherName;
                    objPanDocumentDetails.QEMobileNo = CustQE.MobileNo;
                    objPanDocumentDetails.QEEmailId = CustQE.EmailId;
                    objPanDocumentDetails.QECLIENT_ADDRESS_1 = CustQE.ClientAddress1;
                    objPanDocumentDetails.QECLIENT_ADDRESS_2 = CustQE.ClientAddress2;
                    objPanDocumentDetails.QECLIENT_ADDRESS_3 = CustQE.ClientAddress3;
                    objPanDocumentDetails.QECountry = CustQE.Country;
                    objPanDocumentDetails.QECity = CustQE.CityId;
                    objPanDocumentDetails.QEPin = CustQE.PinCode;
                    objPanDocumentDetails.QECLIENT_PERM_ADDRESS_1 = CustQE.ClientPermAddress1;
                    objPanDocumentDetails.QECLIENT_PERM_ADDRESS_2 = CustQE.ClientPermAddress2;
                    objPanDocumentDetails.QECLIENT_PERM_ADDRESS_3 = CustQE.ClientPermAddress3;
                    objPanDocumentDetails.QECLIENT_PERM_CITY = CustQE.ClientPermCity;
                    objPanDocumentDetails.QECLIENT_PERM_COUNTRY = CustQE.ClientPermCountry;
                    if (CustQE.LivePhoto != null)
                    {
                        objPanDocumentDetails.QELive_Photo = Convert.ToBase64String(CustQE.LivePhoto);
                    }
                }
                return View(objPanDocumentDetails);
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("Exception");
            }
        }

        public ActionResult AadharDataVerify([FromServices] IActiveLogin objLogin, long? CustomerDetailId)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            var OrgID1 = "IndoFin007";
            var ApiKey = "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE";
            var result = HttpContext.Session.GetString("DAEditCustomerdetailId");
            if ((result != null))
            {
                HttpContext.Session.SetString("PersonalId", result);
                CustomerDetailId = Convert.ToInt64((HttpContext.Session.GetString("DAEditCustomerdetailId")));
                ViewBag.AdminFlag = "AdminFlag";
            }
            ClsDataVerification objAadharDetails = new ClsDataVerification();
            var CustQE = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerQEData {CustomerDetailId}").AsEnumerable().FirstOrDefault();
            string ReferenceNo = CustQE.ReferenceNumber;
            var AdmCustomerDocumentsD = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetCustomerDetails1 {CustomerDetailId}").AsEnumerable().FirstOrDefault();
            var Aadharresponse = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharDataForMatches {CustomerDetailId}").AsEnumerable().FirstOrDefault();
            var aadhaarXML = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharXMLDetails {CustomerDetailId}").AsEnumerable().FirstOrDefault();
            try
            {
                if (Aadharresponse != null)
                {
                    if (CustQE.MiddleName != null)
                    {
                        string[] Name = (Aadharresponse.Name).Split(' ');
                        objAadharDetails.AadharFirstNameDV = Name[0];
                        objAadharDetails.AadharMiddleNameDV = Name[1];
                        objAadharDetails.AadharLastNameDV = Name[2];
                        objAadharDetails.AadharDOBDV = Aadharresponse.Dob;
                        objAadharDetails.AadharGenderDV = Aadharresponse.Gender;
                        objAadharDetails.AadharAddress = Aadharresponse.Address;
                        objAadharDetails.House = Aadharresponse.House;
                        objAadharDetails.Street = Aadharresponse.Street;
                        objAadharDetails.State = Aadharresponse.State;
                        objAadharDetails.Post_Office = Aadharresponse.Vtc;
                        objAadharDetails.Pin_Code = Aadharresponse.Pc;
                        objAadharDetails.Locality = Aadharresponse.Locality;
                        objAadharDetails.QEFirstName = CustQE.FirstName;
                        objAadharDetails.QEMiddleName = CustQE.MiddleName;
                        objAadharDetails.QELastName = CustQE.LastName;
                        objAadharDetails.QEAadhaarNo = CustQE.AadharNo;
                        objAadharDetails.QEDOB = CustQE.Dob.ToString();
                        objAadharDetails.QEGender = CustQE.Gender;
                        objAadharDetails.QEFatherName = CustQE.FatherName;
                        objAadharDetails.QEMobileNo = CustQE.MobileNo;
                        objAadharDetails.QEEmailId = CustQE.EmailId;
                        objAadharDetails.QECLIENT_ADDRESS_1 = CustQE.ClientAddress1;
                        objAadharDetails.QECLIENT_ADDRESS_2 = CustQE.ClientAddress2;
                        objAadharDetails.QECLIENT_ADDRESS_3 = CustQE.ClientAddress3;
                        objAadharDetails.QECountry = CustQE.Country;
                        objAadharDetails.QECity = CustQE.CityId;
                        objAadharDetails.QEPin = CustQE.PinCode;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_1 = CustQE.ClientPermAddress1;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_2 = CustQE.ClientPermAddress2;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_3 = CustQE.ClientPermAddress3;
                        objAadharDetails.QECLIENT_PERM_CITY = CustQE.ClientPermCity;
                        if (CustQE.CountryId == "101")
                        {
                            objAadharDetails.QECountry = "INDIA";
                        }
                        else
                        {
                            objAadharDetails.QECountry = CustQE.ClientPermCountry;
                        }
                        objAadharDetails.QELive_Photo = Convert.ToBase64String(AdmCustomerDocumentsD.DocumentHistory);
                        var xyz = Convert.ToBase64String(Aadharresponse.Photo);
                        objAadharDetails.AadharPhotoDV = (string.Format("data:image/jpg;base64,{0}", xyz));
                        objAadharDetails.FirstNameStatus = string.Equals(Name[0].ToLower(), CustQE.FirstName.ToLower()) ? "Match" : "Does Not Match";
                        objAadharDetails.MiddleNameStatus = string.Equals(Name[1].ToLower(), CustQE.MiddleName.ToLower()) ? "Match" : "Does Not Match";
                        objAadharDetails.LastNameStatus = string.Equals(Name[2].ToLower(), CustQE.LastName.ToLower()) ? "Match" : "Does Not Match";
                        objAadharDetails.Genderstatus = string.Equals(Aadharresponse.Gender.ToLower(), CustQE.Gender.ToLower()) ? "Match" : "Does Not Match";
                        objAadharDetails.DOBStatus = string.Equals(Aadharresponse.Dob.ToLower(), CustQE.Dob.ToLower()) ? "Match" : "Does Not Match";

                        byte[] image1 = Convert.FromBase64String(objAadharDetails.QELive_Photo);
                        byte[] image2 = Aadharresponse.Photo;

                        var client = new RestClient("https://apigateway.indofinnet.com/api/VerifyFaceSimilarity?OrgID=" + OrgID1);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("ApiKey", ApiKey);
                        request.AddFileBytes("image1", image1, "image1", "image/jpg");
                        request.AddFileBytes("image2", image2, "image2", "image/jpg");

                        IRestResponse response = client.Execute(request);
                        var aaa = response.Content;
                        dynamic output = JsonConvert.DeserializeObject(aaa);
                        dynamic output2 = JsonConvert.DeserializeObject(output);
                        var status = Convert.ToString(output2.StatusCode);
                        string[] Liveliresult = { (output2.IsMatching), (output2.SimilarityScore) };

                        if (status == "200")
                        {
                            if(Liveliresult[0]=="True")
                            {
                                objAadharDetails.facematching = "Match";
                                objAadharDetails.score = Liveliresult[1];
                            }
                        }
                        else
                        {
                            return Json("No image data found");
                        }
                        //===============================================================================================================================
                        if (CustQE.AadharNo != null)
                        {
                            objAadharDetails.AadhaarNoStatus = string.Equals(Aadharresponse.Uid.ToLower(), CustQE.AadharNo.ToLower()) ? "Match" : "Does Not Match";
                        }
                        else
                        {
                            objAadharDetails.AadhaarNoStatus = "N/A";
                        }
                    }
                    else
                    {
                        string[] Name = (Aadharresponse.Name).Split(' ');
                        objAadharDetails.AadharFirstNameDV = Name[0];
                        objAadharDetails.AadharMiddleNameDV = Name[1];
                        objAadharDetails.AadharLastNameDV = Name[2];
                        objAadharDetails.AadharDOBDV = Aadharresponse.Dob;
                        objAadharDetails.AadharGenderDV = Aadharresponse.Gender;
                        objAadharDetails.QEFirstName = CustQE.FirstName;
                        objAadharDetails.QEMiddleName = CustQE.MiddleName;
                        objAadharDetails.QELastName = CustQE.LastName;
                        objAadharDetails.QEFatherName = CustQE.FatherName;
                        objAadharDetails.QEMobileNo = CustQE.MobileNo;
                        objAadharDetails.QEEmailId = CustQE.EmailId;
                        objAadharDetails.QECLIENT_ADDRESS_1 = CustQE.ClientAddress1;
                        objAadharDetails.QECLIENT_ADDRESS_2 = CustQE.ClientAddress2;
                        objAadharDetails.QECLIENT_ADDRESS_3 = CustQE.ClientAddress3;
                        objAadharDetails.QECountry = CustQE.Country;
                        objAadharDetails.QECity = CustQE.CityId;
                        objAadharDetails.QEPin = CustQE.PinCode;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_1 = CustQE.ClientPermAddress1;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_2 = CustQE.ClientPermAddress2;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_3 = CustQE.ClientPermAddress3;
                        objAadharDetails.QECLIENT_PERM_CITY = CustQE.ClientPermCity;
                        objAadharDetails.QECLIENT_PERM_COUNTRY = CustQE.ClientPermCountry;
                        if (CustQE.LivePhoto != null)
                        {
                            objAadharDetails.QELive_Photo = Convert.ToBase64String(CustQE.LivePhoto);
                            objAadharDetails.AadharPhotoDV = (string.Format("data:image/jpg;base64,{0}", Aadharresponse.Photo));
                        }
                        objAadharDetails.FirstNameStatus = string.Equals(Name[0].ToLower(), CustQE.FirstName.ToLower()) ? "Macth" : "Does Not Match";
                        objAadharDetails.MiddleNameStatus = "N/A";
                        objAadharDetails.LastNameStatus = string.Equals(Name[2].ToLower(), CustQE.LastName.ToLower()) ? "Macth" : "Does Not Match";
                        objAadharDetails.Genderstatus = string.Equals(Aadharresponse.Gender.ToLower(), CustQE.Gender.ToLower()) ? "Match" : "Does Not Match";
                        if (CustQE.AadharNo != null)
                        {
                            objAadharDetails.AadhaarNoStatus = string.Equals(Aadharresponse.Uid.ToLower(), CustQE.AadharNo.ToLower()) ? "Match" : "Does Not Match";
                        }
                        else
                        {
                            objAadharDetails.AadhaarNoStatus = "N/A";
                        }
                    }
                }
                else if (aadhaarXML != null)
                {
                    if (CustQE.MiddleName != null)
                    {
                        string[] Name = (aadhaarXML.AadharName).Split(' ');
                        objAadharDetails.AadharFirstNameDV = Name[0];
                        objAadharDetails.AadharMiddleNameDV = Name[1];
                        objAadharDetails.AadharLastNameDV = Name[2];
                        objAadharDetails.AadharDOBDV = aadhaarXML.AadharDob;
                        objAadharDetails.AadharGenderDV = aadhaarXML.AadharGender;
                        objAadharDetails.AadharAddress = aadhaarXML.AadharAddress;
                        objAadharDetails.House = aadhaarXML.House;
                        objAadharDetails.Street = aadhaarXML.Street;
                        objAadharDetails.State = aadhaarXML.State;
                        objAadharDetails.Post_Office = aadhaarXML.Vtc;
                        objAadharDetails.Pin_Code = aadhaarXML.PinCode;
                        objAadharDetails.Locality = aadhaarXML.Locality;
                        objAadharDetails.QEFirstName = CustQE.FirstName;
                        objAadharDetails.QEMiddleName = CustQE.MiddleName;
                        objAadharDetails.QELastName = CustQE.LastName;
                        objAadharDetails.QEAadhaarNo = CustQE.AadharNo;
                        objAadharDetails.QEDOB = CustQE.Dob.ToString();
                        objAadharDetails.QEGender = CustQE.Gender;
                        objAadharDetails.QEFatherName = CustQE.FatherName;
                        objAadharDetails.QEMobileNo = CustQE.MobileNo;
                        objAadharDetails.QEEmailId = CustQE.EmailId;
                        objAadharDetails.QECLIENT_ADDRESS_1 = CustQE.ClientAddress1;
                        objAadharDetails.QECLIENT_ADDRESS_2 = CustQE.ClientAddress2;
                        objAadharDetails.QECLIENT_ADDRESS_3 = CustQE.ClientAddress3;
                        objAadharDetails.QECountry = CustQE.Country;
                        objAadharDetails.QECity = CustQE.CityId;
                        objAadharDetails.QEPin = CustQE.PinCode;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_1 = CustQE.ClientPermAddress1;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_2 = CustQE.ClientPermAddress2;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_3 = CustQE.ClientPermAddress3;
                        objAadharDetails.QECLIENT_PERM_CITY = CustQE.ClientPermCity;
                        if (CustQE.CountryId == "101")
                        {
                            objAadharDetails.QECountry = "INDIA";
                        }
                        else
                        {
                            objAadharDetails.QECountry = CustQE.ClientPermCountry;
                        }
                        objAadharDetails.QELive_Photo = Convert.ToBase64String(AdmCustomerDocumentsD.DocumentHistory);
                        objAadharDetails.AadharPhotoDV = aadhaarXML.AadharPhoto;
                        string resul = aadhaarXML.AadharPhoto.Remove(0, 22);
                        //var xyz = (string.Format("data:image/jpg;base64,{0}", aadhaarXML.AadharPhoto));
                        objAadharDetails.FirstNameStatus = string.Equals(Name[0].ToLower(), CustQE.FirstName.ToLower()) ? "Match" : "Does Not Match";
                        objAadharDetails.MiddleNameStatus = string.Equals(Name[1].ToLower(), CustQE.MiddleName.ToLower()) ? "Match" : "Does Not Match";
                        objAadharDetails.LastNameStatus = string.Equals(Name[2].ToLower(), CustQE.LastName.ToLower()) ? "Match" : "Does Not Match";
                        objAadharDetails.Genderstatus = string.Equals(aadhaarXML.AadharGender.ToLower(), CustQE.Gender.ToLower()) ? "Match" : "Does Not Match";
                        objAadharDetails.DOBStatus = string.Equals(aadhaarXML.AadharDob.ToLower(), CustQE.Dob.ToLower()) ? "Match" : "Does Not Match";

                        byte[] image1 = Convert.FromBase64String(objAadharDetails.QELive_Photo);
                        byte[] image2 = Convert.FromBase64String(resul);

                        var client = new RestClient("https://apigateway.indofinnet.com/api/VerifyFaceSimilarity?OrgID=" + OrgID1);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("ApiKey", ApiKey);

                        request.AddFileBytes("image1", image1, "image1", "image/jpg");
                        request.AddFileBytes("image2", image2, "image2", "image/jpg");

                        IRestResponse response = client.Execute(request);
                        var aaa = response.Content;
                        dynamic output = JsonConvert.DeserializeObject(aaa);
                        dynamic output2 = JsonConvert.DeserializeObject(output);
                        var status = Convert.ToString(output2.StatusCode);
                        string[] Liveliresult = { (output2.IsMatching), (output2.SimilarityScore) };

                        if (status == "200")
                        {
                            if (Liveliresult[0] == "True")
                            {
                                objAadharDetails.facematching = "Match";
                                objAadharDetails.score = Liveliresult[1];
                            }
                        }
                        else
                        {
                            return Json("No image data found");
                        }
                        //===============================================================================================================================
                        if (CustQE.AadharNo != null)
                        {
                            objAadharDetails.AadhaarNoStatus = string.Equals(aadhaarXML.AadharNumber.ToLower(), CustQE.AadharNo.ToLower()) ? "Match" : "Does Not Match";
                        }
                        else
                        {
                            objAadharDetails.AadhaarNoStatus = "N/A";
                        }
                    }
                    else
                    {
                        string[] Name = (aadhaarXML.AadharName).Split(' ');
                        objAadharDetails.AadharFirstNameDV = Name[0];
                        objAadharDetails.AadharMiddleNameDV = Name[1];
                        objAadharDetails.AadharLastNameDV = Name[2];
                        objAadharDetails.AadharDOBDV = aadhaarXML.AadharDob;
                        objAadharDetails.AadharGenderDV = aadhaarXML.AadharGender;
                        objAadharDetails.QEFirstName = CustQE.FirstName;
                        objAadharDetails.QEMiddleName = CustQE.MiddleName;
                        objAadharDetails.QELastName = CustQE.LastName;
                        objAadharDetails.QEFatherName = CustQE.FatherName;
                        objAadharDetails.QEMobileNo = CustQE.MobileNo;
                        objAadharDetails.QEEmailId = CustQE.EmailId;
                        objAadharDetails.QECLIENT_ADDRESS_1 = CustQE.ClientAddress1;
                        objAadharDetails.QECLIENT_ADDRESS_2 = CustQE.ClientAddress2;
                        objAadharDetails.QECLIENT_ADDRESS_3 = CustQE.ClientAddress3;
                        if (CustQE.CountryId == "101")
                        {
                            objAadharDetails.QECountry = "INDIA";
                        }
                        else
                        {
                            objAadharDetails.QECountry = CustQE.ClientPermCountry;
                        }
                        objAadharDetails.QECity = CustQE.CityId;
                        objAadharDetails.QEPin = CustQE.PinCode;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_1 = CustQE.ClientPermAddress1;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_2 = CustQE.ClientPermAddress2;
                        objAadharDetails.QECLIENT_PERM_ADDRESS_3 = CustQE.ClientPermAddress3;
                        objAadharDetails.QECLIENT_PERM_CITY = CustQE.ClientPermCity;
                        objAadharDetails.QECLIENT_PERM_COUNTRY = CustQE.ClientPermCountry;
                        if (CustQE.LivePhoto != null)
                        {
                            objAadharDetails.QELive_Photo = Convert.ToBase64String(CustQE.LivePhoto);

                        }
                        objAadharDetails.FirstNameStatus = string.Equals(Name[0].ToLower(), CustQE.FirstName.ToLower()) ? "Macth" : "Does Not Match";
                        objAadharDetails.MiddleNameStatus = "N/A";
                        objAadharDetails.LastNameStatus = string.Equals(Name[2].ToLower(), CustQE.LastName.ToLower()) ? "Macth" : "Does Not Match";
                        objAadharDetails.Genderstatus = string.Equals(aadhaarXML.AadharGender.ToLower(), CustQE.Gender.ToLower()) ? "Match" : "Does Not Match";
                        if (CustQE.AadharNo != null)
                        {
                            objAadharDetails.AadhaarNoStatus = string.Equals(Aadharresponse.Uid.ToLower(), CustQE.AadharNo.ToLower()) ? "Match" : "Does Not Match";
                        }
                        else
                        {
                            objAadharDetails.AadhaarNoStatus = "N/A";
                        }
                    }
                }
                else
                {
                    objAadharDetails.QEFirstName = CustQE.FirstName;
                    objAadharDetails.QEMiddleName = CustQE.MiddleName;
                    objAadharDetails.QELastName = CustQE.LastName;
                    objAadharDetails.QEFatherName = CustQE.FatherName;
                    objAadharDetails.QEMobileNo = CustQE.MobileNo;
                    objAadharDetails.QEEmailId = CustQE.EmailId;
                    objAadharDetails.QECLIENT_ADDRESS_1 = CustQE.ClientAddress1;
                    objAadharDetails.QECLIENT_ADDRESS_2 = CustQE.ClientAddress2;
                    objAadharDetails.QECLIENT_ADDRESS_3 = CustQE.ClientAddress3;
                    if (CustQE.CountryId == "101")
                    {
                        objAadharDetails.QECountry = "INDIA";
                    }
                    else
                    {
                        objAadharDetails.QECountry = CustQE.ClientPermCountry;
                    }
                    objAadharDetails.QECity = CustQE.CityId;
                    objAadharDetails.QEPin = CustQE.PinCode;
                    objAadharDetails.QECLIENT_PERM_ADDRESS_1 = CustQE.ClientPermAddress1;
                    objAadharDetails.QECLIENT_PERM_ADDRESS_2 = CustQE.ClientPermAddress2;
                    objAadharDetails.QECLIENT_PERM_ADDRESS_3 = CustQE.ClientPermAddress3;
                    objAadharDetails.QECLIENT_PERM_CITY = CustQE.ClientPermCity;
                    objAadharDetails.QECLIENT_PERM_COUNTRY = CustQE.ClientPermCountry;
                    if (CustQE.LivePhoto != null)
                    {
                        objAadharDetails.QELive_Photo = Convert.ToBase64String(AdmCustomerDocumentsD.DocumentHistory);
                    }
                }
                return View(objAadharDetails);
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }

        [HttpPost]
        public string CustGRID()
        {
            ErrorLog error_log = new ErrorLog();
            AdmCustGrid objmain = new AdmCustGrid();
            try
            {
                var list = objDetails.AdmCustGrids.FromSqlRaw($"USP_GetCustomerKycData").ToList().AsEnumerable().ToList();

                var p = JsonConvert.SerializeObject(list);

                var resp2 = "{\"data\":" + p + "}";

                return resp2;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return " ";
            }
        }
        public ActionResult adminrekyc([FromServices] IActiveLogin objLogin, long AdminCustId)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            var personalId = AdminCustId;
            string IsREkyctrue = Convert.ToString(personalId);
            HttpContext.Session.SetString("PersonalId", IsREkyctrue);
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult PendingREKYC([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("Exception");
            }
        }
        [HttpPost]
        public string ReKYCCustGRID()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var list = objDetails.AdmCustGrids.FromSqlRaw($"USP_GetCustomerReKycData").ToList().AsEnumerable().ToList();
                var p = JsonConvert.SerializeObject(list);
                var resp2 = "{\"data\":" + p + "}";
                return resp2;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return " ";
            }
        }

        public ActionResult CustomerStatus([FromServices] IActiveLogin objLogin, string CustomerDetailId)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
            {
                var DAEditCustomerdetailId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                HttpContext.Session.SetString("PersonalId", DAEditCustomerdetailId);
                ViewBag.AdminFlag = "AdminFlag";
            }
            string conn50 = _connectionString;
            using (SqlConnection connection50 = new SqlConnection(conn50))
            {
                SqlCommand cmd50 = new SqlCommand("USP_GetApproveCustomerCount", connection50);
                cmd50.CommandType = CommandType.StoredProcedure;
                connection50.Open();
                SqlDataReader reader50 = cmd50.ExecuteReader();
                if (reader50.Read())
                {
                    var ApproveCount = reader50[0].ToString();
                }
                ViewBag.approveCnt = reader50[0].ToString();
            }
            string conn51 = _connectionString;
            using (SqlConnection connection51 = new SqlConnection(conn51))
            {
                SqlCommand cmd50 = new SqlCommand("USP_GetRejectCustomerCount", connection51);
                cmd50.CommandType = CommandType.StoredProcedure;
                connection51.Open();
                SqlDataReader reader51 = cmd50.ExecuteReader();
                if (reader51.Read())
                {
                    var RejectCount = reader51[0].ToString();
                }
                ViewBag.rejectCnt = reader51[0].ToString();
            }
            string conn52 = _connectionString;
            using (SqlConnection connection52 = new SqlConnection(conn51))
            {
            https://localhost:5003/Content/imghome/blog1.png
                SqlCommand cmd52 = new SqlCommand("USP_GetPendingCustomerCount", connection52);
                cmd52.CommandType = CommandType.StoredProcedure;
                connection52.Open();
                SqlDataReader reader52 = cmd52.ExecuteReader();
                if (reader52.Read())
                {
                    var PendingCount = reader52[0].ToString();
                }
                ViewBag.PendingCnt = reader52[0].ToString();
            }
            string conn53 = _connectionString;
            using (SqlConnection connection53 = new SqlConnection(conn53))
            {
                //https://localhost:5003/Content/imghome/blog1.png
                SqlCommand cmd53 = new SqlCommand("USp_toGetApprovedjointcustomersCOUNT", connection53);
                cmd53.CommandType = CommandType.StoredProcedure;
                connection53.Open();
                SqlDataReader reader53 = cmd53.ExecuteReader();
                if (reader53.Read())
                {
                    var JointCount = reader53[0].ToString();
                }
                ViewBag.JointCount = reader53[0].ToString();
                
            }
            using (SqlConnection connection53 = new SqlConnection(conn53))
            {
                //https://localhost:5003/Content/imghome/blog1.png
                SqlCommand cmd53 = new SqlCommand("USP_GetPendingJointCustomerCount", connection53);
                cmd53.CommandType = CommandType.StoredProcedure;
                connection53.Open();
                SqlDataReader reader53 = cmd53.ExecuteReader();
                if (reader53.Read())
                {
                    var JointCount = reader53[0].ToString();
                }
                ViewBag.PendingJointCount = reader53[0].ToString();
                
            }
            try
            {
                TempData["custId"] = HttpContext.Session.GetString("PersonalId");
                TempData.Keep("custId");
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult ReKycStatus([FromServices] IActiveLogin objLogin, string CustomerDetailId)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
            {
                var DAEditCustomerdetailId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                HttpContext.Session.SetString("PersonalId", DAEditCustomerdetailId);
                ViewBag.AdminFlag = "AdminFlag";
            }
            string conn50 = _connectionString;
            using (SqlConnection connection50 = new SqlConnection(conn50))
            {
                SqlCommand cmd50 = new SqlCommand("USP_GetApproveReKycCustomerCount", connection50);
                cmd50.CommandType = CommandType.StoredProcedure;
                connection50.Open();
                SqlDataReader reader50 = cmd50.ExecuteReader();
                if (reader50.Read())
                {
                    var ApproveCount = reader50[0].ToString();
                }
                ViewBag.REkycapproveCnt = reader50[0].ToString();
            }
            string conn51 = _connectionString;
            using (SqlConnection connection51 = new SqlConnection(conn51))
            {
                SqlCommand cmd50 = new SqlCommand("USP_GetRejectReKycCustomerCount", connection51);
                cmd50.CommandType = CommandType.StoredProcedure;
                connection51.Open();
                SqlDataReader reader51 = cmd50.ExecuteReader();
                if (reader51.Read())
                {
                    var RejectCount = reader51[0].ToString();
                }
                ViewBag.REKYCrejectCnt = reader51[0].ToString();
            }
            string conn52 = _connectionString;
            using (SqlConnection connection52 = new SqlConnection(conn51))
            {
                SqlCommand cmd52 = new SqlCommand("USP_GetPendingReKycCustomerCount", connection52);
                cmd52.CommandType = CommandType.StoredProcedure;
                connection52.Open();
                SqlDataReader reader52 = cmd52.ExecuteReader();
                if (reader52.Read())
                {
                    var PendingCount = reader52[0].ToString();
                }
                ViewBag.PendingCnt = reader52[0].ToString();
            }
            try
            {
                TempData["custId"] = HttpContext.Session.GetString("PersonalId");
                TempData.Keep("custId");
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult getDigiAadhardetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        [HttpPost]
        public string getDigiAadhardetailsGrid()
        {
            ErrorLog error_log = new ErrorLog();
            AdmCustGrid objmain = new AdmCustGrid();
            try
            {
                var list = objDetails.AdmDigiAadharlogDetailsGrids.FromSqlRaw($"USP_GETAADHARLOGDATA").ToList().AsEnumerable().ToList();

                var p = JsonConvert.SerializeObject(list);

                var resp2 = "{\"data\":" + p + "}";

                return resp2;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return " ";
            }
        }
        public ActionResult getDidiPandetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        public string getDidiPandetailsGrid()
        {
            ErrorLog error_log = new ErrorLog();
            AdmCustGrid objmain = new AdmCustGrid();
            try
            {
                var list = objDetails.AdmCustGrids.FromSqlRaw($"USP_GetCustomerKycData").ToList().AsEnumerable().ToList();

                var p = JsonConvert.SerializeObject(list);

                var resp2 = "{\"data\":" + p + "}";

                return resp2;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return " ";
            }
        }
        public ActionResult getAadharXMLdetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        [HttpPost]
        public string getAadharXMLdetailsGrid()
        {
            ErrorLog error_log = new ErrorLog();
            AdmCustGrid objmain = new AdmCustGrid();
            try
            {
                var list = objDetails.AdmCustGrids.FromSqlRaw($"USP_GetCustomerKycData").ToList().AsEnumerable().ToList();

                var p = JsonConvert.SerializeObject(list);

                var resp2 = "{\"data\":" + p + "}";

                return resp2;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return " ";
            }
        }
        public ActionResult getPandetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        [HttpPost]
        public string getPandetailsGrid()
        {
            ErrorLog error_log = new ErrorLog();
            AdmCustGrid objmain = new AdmCustGrid();
            try
            {
                var list = objDetails.AdmpanlogDetailsGrids.FromSqlRaw($"USP_GETNSDLPANLOGDATA").ToList().AsEnumerable().ToList();
                //var list = objDetails.AdmpanlogDetailsGrids.FromSqlRaw($"USP_GETPANLOGDATA").ToList().AsEnumerable().ToList();

                var p = JsonConvert.SerializeObject(list);

                var resp2 = "{\"data\":" + p + "}";

                return resp2;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return " ";
            }
        }
        public ActionResult GetLogsData([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        [HttpGet]
        public ActionResult ApproveCustDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }

        [HttpPost]
        public string ApproveCustGRID()
        {
            ErrorLog error_log = new ErrorLog();
            AdmApproveddata objmain = new AdmApproveddata();
            try
            {
                var list = objDetails.AdmApproveddatas.FromSqlRaw($"USP_GetCustomerKycDataApprove").ToList().AsEnumerable().ToList();
                var p = JsonConvert.SerializeObject(list);
                var resp1 = "{\"data\":" + p + "}";
                return resp1;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return "";
            }
        }

        [HttpGet]
        public ActionResult ApproveCustAccDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }

        [HttpPost]
        public string ApproveCustAccGRID()
        {
            ErrorLog error_log = new ErrorLog();
            AdmApproveddata objmain = new AdmApproveddata();
            try
            {
                var list = objDetails.AdmApproveddatas.FromSqlRaw($"USP_AccHistory").ToList().AsEnumerable().ToList();
                var p = JsonConvert.SerializeObject(list);
                var resp1 = "{\"data\":" + p + "}";
                return resp1;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return "";
            }
        }

        [HttpGet]
        public ActionResult ApproveJointCustDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        [HttpPost]
        public string ApproveJointCustGRID()
        {
            ErrorLog error_log = new ErrorLog();
            AdmApproveddata objmain = new AdmApproveddata();
            try
            {
                var list = objDetails.AdmJointApproveddata.FromSqlRaw($"USp_toGetApprovedjointcustomers").ToList().AsEnumerable().ToList();
                var p = JsonConvert.SerializeObject(list);
                var resp1 = "{\"data\":" + p + "}";
                return resp1;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return "";
            }
        }

        [HttpGet]
        public ActionResult ApproveReKycDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        [HttpPost]
        public string ApproveReKycGRID()
        {
            ErrorLog error_log = new ErrorLog();
            AdmApproveddata objmain = new AdmApproveddata();
            try
            {
                var list = objDetails.adm_CustRkycstatuss.FromSqlRaw($"USP_RkycApproveReject").ToList().AsEnumerable().ToList();
                var p = JsonConvert.SerializeObject(list);
                var resp1 = "{\"data\":" + p + "}";
                return resp1;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return "";
            }
        }

        [HttpGet]
        public ActionResult PANCustGRIDCustDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }
        public string PANCustGRID()
        {
            ErrorLog error_log = new ErrorLog();
            bulkup objmain = new bulkup();
            try
            {
                var list = objDetails.bulkup.FromSqlRaw($"GetDataFrompanbulkupload").AsEnumerable().ToList();
                var p = JsonConvert.SerializeObject(list);
                var resp1 = "{\"data\":" + p + "}";
                return resp1;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return "";
            }
        }

        [HttpGet]
        public ActionResult RejectReKyctDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("Exception");
            }
        }

        [HttpPost]
        public string RejectReKycGRID()
        {
            ErrorLog error_log = new ErrorLog();
            AdmRejectdata objmain = new AdmRejectdata();
            try
            {
                var list = objDetails.AdmRejectrekycdatas.FromSqlRaw($"USP_GetCustomerReKycDataReject12").ToList().AsEnumerable().ToList();
                var p = JsonConvert.SerializeObject(list);
                var resp1 = "{\"data\":" + p + "}";
                return resp1;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return "";
            }
        }

        [HttpGet]
        public ActionResult RejectCustDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("Exception");
            }
        }
        [HttpPost]
        public string RejectCustGRID()
        {
            ErrorLog error_log = new ErrorLog();
            AdmRejectdata objmain = new AdmRejectdata();
            try
            {
                var list = objDetails.AdmRejectdatas.FromSqlRaw($"USP_GetCustomerKycDataReject").ToList().AsEnumerable().ToList();
                var p = JsonConvert.SerializeObject(list);
                var resp1 = "{\"data\":" + p + "}";
                return resp1;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return "";
            }
        }
        public ActionResult DataVerificationTabView([FromServices] IActiveLogin objLogin, string CustomerDetailId)
        {
            //if (!clsAdminAuthorize.IsAuthorizeCustDetail((objtriple.Decrypt(HttpContext.Session.GetString("UserId").ToString())), HttpContext.Session.GetString("SessionKey").ToString()))
            // return RedirectToAction("UserDetails", "AdminLogin");
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
            {
                var PersonalId = Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId"));
                //Session["PersonalId"] = Session["DAEditCustomerdetailId"];
                ViewBag.AdminFlag = "AdminFlag";
            }
            try
            {
                TempData["custId"] = HttpContext.Session.GetString("PersonalId");
                TempData.Keep("custId");
                //var Overview = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_Overview {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_Overview", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustId", (Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId"))));
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var IsPanVerify = reader[0].GetType();
                        if (IsPanVerify.Name == "DBNull")
                        {
                            var IsPanVerify1 = false;
                        }
                        else
                        {
                            var IsPanVerify1 = (bool)reader[0];
                            if (IsPanVerify1 == true)
                            {
                                ViewBag.PanVerify = true;
                            }
                            else
                            {
                                ViewBag.PanVerify = false;
                            }
                        }
                        var IsDigiPANSumbitted = reader[1].GetType();
                        if (IsDigiPANSumbitted.Name == "DBNull")
                        {
                            var IsDigiPANSumbitted1 = false;
                        }
                        else
                        {
                            var IsDigiPANSumbitted1 = (bool)reader[1];
                            if (IsDigiPANSumbitted1 == true)
                            {
                                ViewBag.IsDigiPANSumbitted = true;
                            }
                            else
                            {
                                ViewBag.IsDigiPANSumbitted = false;
                            }

                        }
                        var IsDigiAadharSumbitted = reader[2].GetType();
                        if (IsDigiAadharSumbitted.Name == "DBNull")
                        {
                            var IsDigiAadharSumbitted1 = false;
                        }
                        else
                        {
                            var IsDigiAadharSumbitted1 = (bool)reader[2];
                            if (IsDigiAadharSumbitted1 == true)
                            {
                                ViewBag.IsDigiAadharSumbitted = true;
                            }
                            else
                            {
                                ViewBag.IsDigiAadharSumbitted = false;
                            }
                        }
                        var AdharXML = reader[3].GetType();
                        if (AdharXML.Name == "DBNull")
                        {
                            var AdharXML1 = false;
                        }
                        else
                        {
                            var AdharXML1 = (bool)reader[3];
                            if (AdharXML1 == true)
                            {
                                ViewBag.AdharXML = true;
                            }
                            else
                            {
                                ViewBag.AdharXML = false;
                            }
                        }
                    }
                }
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult AdmOtpSend([FromServices] IActiveLogin objLogin, string MobileNumber)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                var mob=MobileNumber;
                if (MobileNumber != null)
                {
                    HttpContext.Session.SetString("MOBNO", MobileNumber);
                }
                //var mob = HttpContext.Session.GetString("MOBNO");
                
                var client = new RestClient("https://cbs.indofinnet.com/api/SMSOTPRSSB?ToMobileNo=" + mob);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                IRestResponse response = client.Execute(request);
                string res = response.Content;


                res = res.Replace(@"\", "");

                string s = res.Split('"')[1];

                return Json(s);

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
        }

        public ActionResult JointDetails([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                string message = "Session Expired";
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            else if (LoginStatus == null)
            {
                return RedirectToAction("UserDetails", "AdminLogin");
            }
            try
            {
                return View();
            }
            catch (Exception Ex)
            {
                error_log.WriteErrorLog(Ex.ToString());
                return Json("");
            }
        }

        [HttpPost]
        public string JointGRID()
        {
            ErrorLog error_log = new ErrorLog();

            try
            {

                var list = objDetails.AdmCosmosjointDetails.FromSqlRaw($"USP_GetJointHolderData").ToList().AsEnumerable().ToList();

                var p = JsonConvert.SerializeObject(list);
                
                var resp2 = "{\"data\":" + p + "}";
                HttpContext.Session.SetString("jointGrid", "true");
                HttpContext.Session.SetString("JointACCdata", "true");
                return resp2;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return " ";
            }
        }
    }

    public class ClsAdmogin
    {
    }
}
