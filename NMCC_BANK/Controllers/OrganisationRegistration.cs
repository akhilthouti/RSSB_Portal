using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;
using ServiceProvider1.Models.OrgExceptionLogs;
using System.Configuration;
using System.Text;

using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;

using System.Threading.Tasks;
using System.ComponentModel;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs.Specialized;
using Microsoft.Extensions.Configuration;

namespace INDO_FIN_NET.Controllers
{
    public class OrganisationRegistration : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        private readonly string _connectionString;

        public OrganisationRegistration(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData1 = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
                    }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult OrgRegDetails()
        {
            var verificationtype = (from details in objDetails.Adm_OrganizationNames.FromSqlRaw($"USP_ToGetOrganizationName").ToList()
                                    select new SelectListItem()
                                    {
                                        Text = details.Organization_Name.ToString(),
                                        Value = details.Organization_ID.ToString(),

                                    }).ToList();
            verificationtype.Insert(0, new SelectListItem()
            {
                Text = "----Select----",
                Value = string.Empty
            });
            ViewBag.OrgName = verificationtype;
            return View();
        }
        [HttpPost]
        public IActionResult OrgRegDetails(OrganisationReg orgreg)
        {
            string orgUseri = HttpContext.Session.GetString("ImageByte");
            
            byte[] OrganizationLogo = Convert.FromBase64String(orgUseri);
            orgreg.OrganizationLogo = OrganizationLogo;
            string conn = _connectionString;
            using (SqlConnection connection2 = new SqlConnection(conn))
            {
                SqlCommand cmd = new SqlCommand("USP_OrganisationDetails", connection2);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@OrganizationName", orgreg.OrganizationName);
                cmd.Parameters.AddWithValue("@OrganizationLogo", orgreg.OrganizationLogo);
                cmd.Parameters.AddWithValue("@OrganizationDescription", orgreg.OrgDescription);
                cmd.Parameters.AddWithValue("@AdminUsername", orgreg.AdminUserName);
                cmd.Parameters.AddWithValue("@ContactPerMobNo  ", orgreg.ContactPerMobNo);
                cmd.Parameters.AddWithValue("@ContactPerEmailId", orgreg.ContactPerEmailId);
                cmd.Parameters.AddWithValue("@HOadddress", orgreg.HOadddress);
                cmd.Parameters.AddWithValue("@FaxNo ", orgreg.FaxNo);
                cmd.Parameters.AddWithValue("@OrgPassword", orgreg.ConfirmPassword);
                cmd.Parameters.AddWithValue("@Fname  ", orgreg.Fname);
                cmd.Parameters.AddWithValue("@mname ", orgreg.mname);
                cmd.Parameters.AddWithValue("@Lname  ", orgreg.Lname);
                cmd.Parameters.AddWithValue("@ConfirmPassword", orgreg.ConfirmPassword);

                connection2.Open();
                cmd.ExecuteNonQuery();
                connection2.Close();
            }
            return Json("Organization register successfully");
        }

        public IActionResult BrowseLogoImage()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                IFormFile files = Request.Form.Files[0];
                byte[] OrgLogoImg = null;
                BinaryReader reader = new BinaryReader(files.OpenReadStream());
                OrgLogoImg = reader.ReadBytes((int)files.Length);
                string base64String = Convert.ToBase64String(OrgLogoImg, 0, OrgLogoImg.Length);
                TempData["LogoImg"] = base64String;
                HttpContext.Session.SetString("ImageByte", (base64String));
                return Json(TempData["LogoImg"]);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
    }
}
