using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Text;
using System.IO;

using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;

using System.ComponentModel;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs.Specialized;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace INDO_FIN_NET.Controllers
{
    public class AdminDashboardNewController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        private readonly string _connectionString;
        public AdminDashboardNewController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
        }

        TripleDESImplementation objtriple = new TripleDESImplementation();
        ClsUser objuser = new ClsUser();
        public ActionResult IndoFinAdminDashBoard([FromServices] IActiveLogin objLogin)
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
                ViewBag.msg = TempData["msg"];
                var approvecount =(objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetApproveCustomerCount "));
                var rejectcount = (objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetRejectCustomerCount "));
                ViewBag.approveCnt = approvecount;
                ViewBag.rejectCnt = rejectcount;
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        public ActionResult Logout()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var userid = HttpContext.Session.GetString("UserID");
                var userid1 = userid.Split('"');
                var UseriD = userid1[1];
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_UpdateAdminLogOutDateById", connection);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@UserId", UseriD);
                    connection.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {

                    }
                }
                HttpContext.Session.Clear();
                HttpContext.Session.Remove(UseriD);
                HttpContext.Response.Cookies.Delete(UseriD);
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
            return View();
        }
    }
}
