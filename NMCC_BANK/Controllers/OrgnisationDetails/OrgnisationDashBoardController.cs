using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using INDO_FIN_NET.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Controllers.OrgnisationDetails
{
    public class OrgnisationDashBoardController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        private readonly string _connectionString;
        public OrgnisationDashBoardController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        public ActionResult IndoFinNetOrgDashBoard()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }
        }

        public ActionResult MainDashboard([FromServices] IActiveLogin objLogin)
        {
            ErrorLog error_log = new ErrorLog();
            var agentid = HttpContext.Session.GetString("UseID");
            var CustomerFlow = HttpContext.Session.GetString("CustMobileNo");
            if (CustomerFlow != null)
            {
                ViewBag.CustomerFlow = true;
            }
            else if (agentid != null)
            {
                ViewBag.CustomerFlow = false;
            }

            var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            if (LoginStatus == "Active")
            {
                if (ViewBag.CustomerFlow == false)
                {
                    string message = "Session Expired";
                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                }
                else if (ViewBag.CustomerFlow == true)
                {
                    return RedirectToAction("CustomerRegistration", "CustomerRegistration");
                }
                else
                {
                    return RedirectToAction("MainHomePage", "OrganisationLogin");
                }
            }
            else if (LoginStatus == null)
            {
                if (ViewBag.CustomerFlow == false)
                {
                    string message = "Session Expired";
                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                }
                else if (ViewBag.CustomerFlow == true)
                {
                    return RedirectToAction("CustomerRegistration", "CustomerRegistration");
                }
                else
                {
                    return RedirectToAction("MainHomePage", "OrganisationLogin");
                }
            }
            //var userid = HttpContext.Session.GetString("UseID");
            //string conn50 = _connectionString;
            //using (SqlConnection connection50 = new SqlConnection(conn50))
            //{
            //    SqlCommand cmd50 = new SqlCommand("USP_GetApproveCustByAgentCount", connection50);
            //    cmd50.CommandType = CommandType.StoredProcedure;
            //    cmd50.Parameters.AddWithValue("@UserId", userid);
            //    connection50.Open();
            //    SqlDataReader reader50 = cmd50.ExecuteReader();
            //    if (reader50.Read())
            //    {
            //        var ApproveCount = reader50[0].ToString();
            //    }
            //    ViewBag.approveCustForagentCnt = reader50[0].ToString();
            //}
            //string conn51 = _connectionString;
            //using (SqlConnection connection51 = new SqlConnection(conn51))
            //{
            //    SqlCommand cmd50 = new SqlCommand("USP_GetRejectCustByAgentCount", connection51);
            //    cmd50.CommandType = CommandType.StoredProcedure;
            //    cmd50.Parameters.AddWithValue("@UserId", userid);
            //    connection51.Open();
            //    SqlDataReader reader51 = cmd50.ExecuteReader();
            //    if (reader51.Read())
            //    {
            //        var RejectCount = reader51[0].ToString();
            //    }
            //    ViewBag.rejectCustForagentCnt = reader51[0].ToString();
            //}
            //string conn52 = _connectionString;
            //using (SqlConnection connection52 = new SqlConnection(conn51))
            //{

            //    SqlCommand cmd52 = new SqlCommand("USP_GetCompletedCustomerByAgentCount", connection52);
            //    cmd52.CommandType = CommandType.StoredProcedure;
            //    cmd52.Parameters.AddWithValue("@UserId", userid);
            //    connection52.Open();
            //    SqlDataReader reader52 = cmd52.ExecuteReader();
            //    if (reader52.Read())
            //    {
            //        var PendingCount = reader52[0].ToString();
            //    }
            //    ViewBag.PendingCustForagentCnt = reader52[0].ToString();
            //}
            //string conn53 = _connectionString;
            //using (SqlConnection connection53 = new SqlConnection(conn53))
            //{

            //    SqlCommand cmd53 = new SqlCommand("USP_TotalCustomerByAgentCount", connection53);
            //    cmd53.CommandType = CommandType.StoredProcedure;
            //    cmd53.Parameters.AddWithValue("@UserId", userid);
            //    connection53.Open();
            //    SqlDataReader reader53 = cmd53.ExecuteReader();
            //    if (reader53.Read())
            //    {
            //        var TotalCount = reader53[0].ToString();
            //    }
            //    ViewBag.TotalCount = reader53[0].ToString();
            //}
            
            try
            {
                var ABC = HttpContext.Session.GetString("UserName");//, result.UserName);
                ViewBag.agnet = ABC;
                ViewBag.Time = DateTime.Now.ToString("HH:mm:ss");

                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }
        [HttpGet]
        public ActionResult LoanDetails()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }
        [HttpGet]
        public ActionResult RejectedCustomer()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }
        [HttpGet]
        public ActionResult RejectedCustomerDashboard()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ViewBag.Time = DateTime.Now.ToString("HH:mm:ss");
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }
        [HttpGet]
        public ActionResult ReportsDownload()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ViewBag.Time = DateTime.Now.ToString("HH:mm:ss");
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }
        [HttpPost]
        public string RejectCustomerGRID()
        {
            ErrorLog error_log = new ErrorLog();
            AdmRejectdata objmain = new AdmRejectdata();
            var userid = HttpContext.Session.GetString("UseID");
            try
            {
                var list = objDetails.AdmRejectdatas.FromSqlRaw($"USP_GetRejectCustomerData {(userid)}").ToList().AsEnumerable().ToList();
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
        public ActionResult ReKycRejectedCustomer()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }
        [HttpPost]
        public string ReKycRejectCustomerGRID()
        {
            ErrorLog error_log = new ErrorLog();
            //AdmRejectrekycdatas objmain = new AdmRejectrekycdatas();
            var userid = HttpContext.Session.GetString("UseID");
            try
            {
                var list = objDetails.AdmRejectrekycdatas.FromSqlRaw($"USP_GetReKycRejectCustomerData {(userid)}").ToList().AsEnumerable().ToList();
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

    }
}
