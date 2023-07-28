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
using Microsoft.Data.SqlClient;
using System.Data;

namespace INDO_FIN_NET.Controllers
{
    public class CustomerOTPController : Controller
    {

        CustomerModel CM = new CustomerModel();
        private readonly RSSBPRODDbCotext ObjIndo;
        private readonly INDO_FinNetDbCotext objData;
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        private readonly IConfiguration _config;
        private readonly IConfiguration configuration;
        private Appsettings _settings;
        private readonly string _connectionString;

        public CustomerOTPController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            ObjIndo = Context;
            objData = iNDO_;
            _settings = new Appsettings();
            configuration.GetSection("Appsettings").Bind(_settings);
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        //databaseservice.Service1Client objdb = new databaseservice.Service1Client();
        bool result;
        string nabc = "";
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CustomerOTP(string RefID)
        {
            string filename1 = "";
            try
            {
                HttpContext.Session.SetString("RefID", RefID);
                return View(CM);
            }
            catch (Exception ex)
            {
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                System.IO.File.AppendAllText(filename1, errormsg);
                PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }
        public ActionResult CustVKYC()
        {
            string filename1 = "";
            try

            {
                string defg = "";
                
                long abcf = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string RefID = HttpContext.Session.GetString("RefID");
                defg = RefID;

                string conn23 = _connectionString;
                using (SqlConnection connection23 = new SqlConnection(conn23))
                {
                    SqlCommand cmd23 = new SqlCommand("USP_GetMeetingDetails", connection23);
                    cmd23.CommandType = CommandType.StoredProcedure;

                    cmd23.Parameters.AddWithValue("@refID", defg);


                    connection23.Open();


                    SqlDataReader reader23 = cmd23.ExecuteReader();
                    if (reader23.Read())
                    {

                        string RefID1 = reader23[5].ToString();
                    }


                    string meetId = reader23[5].ToString();
                    if (meetId != null || meetId != string.Empty)
                    {
                        ViewBag.meetId = meetId;
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                System.IO.File.AppendAllText(filename1, errormsg);
                PortalException.InsertPortalException(ex);
                return Json("Exception");
            }

        }

        public ActionResult CustVKYC1()
        {
            string filename1 = "";
            try

            {
                string defg = "";
                long abcf = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                if (ViewBag.CUSTOMERLOGIN == "CustomerLogin")
                {
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
                        string conn3 = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn3))
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

                        }
                    }
                }
                else
                {
                    string RefID = HttpContext.Session.GetString("RefID");

                    defg = RefID;


                }
                string conn2 = _connectionString;
                using (SqlConnection connection2 = new SqlConnection(conn2))
                {
                    SqlCommand cmd2 = new SqlCommand("USP_GetMeetingDetails", connection2);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.AddWithValue("@refID", defg);


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
                        ViewBag.meetId = meetId;
                    }
                    return View();
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
            return View();

        }
    }
}
