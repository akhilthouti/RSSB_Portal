using INDO_FIN_NET.Models;
using INDO_FIN_NET.Models.CurrentModels;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Linq;

namespace INDO_FIN_NET.Controllers.CurrectDetails
{
    public class CurrentCAFController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        private readonly string _connectionString;
        public CurrentCAFController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData1 = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
        }
        [HttpGet]
        public IActionResult CurrentCAF()
        {
            Current_Verification obj = new Current_Verification();
            var CustomerId = HttpContext.Session.GetString("PersonalId");
            ViewBag.PersonalId = CustomerId;
            var dataforPan = objDetails.PAN_Verifications.FromSqlRaw($"USP_GetPanDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault();
            if (dataforPan != null)
            {
                ViewBag.dataforPan = "true";
                obj.CONSTITUTIONOFBUSINESS = dataforPan.CONSTITUTIONOFBUSINESS;
                obj.GSTIN = dataforPan.GSTIN;
                obj.LEGALNAMEOFBUSINESS = dataforPan.LEGALNAMEOFBUSINESS;
                obj.State = dataforPan.State;
                obj.Status = dataforPan.Status;
                obj.created_at = dataforPan.created_at;
                obj.ref_idforPAN = dataforPan.ref_id;
                obj.statusMessageforPAN = dataforPan.statusMessage;
                obj.GSTIN = dataforPan.GSTIN;


                //if (dataforPan.State != null)
                //{
                //    string Scode = dataforPan.State;
                //    string conn1 = _connectionString;
                //    using (SqlConnection connection12 = new SqlConnection(conn1))
                //    {
                //        SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection12);
                //        cmd12.CommandType = CommandType.StoredProcedure;

                //        cmd12.Parameters.AddWithValue("@State_Code", Scode);
                //        connection12.Open();
                //        SqlDataReader reader1 = cmd12.ExecuteReader();
                //        if (reader1.Read())
                //        {


                //            var state = reader1[2].ToString();
                //            obj.State = state;
                //        }
                //    }
                //}

            }
            var dataforCIN = objDetails.CIN_Verifications.FromSqlRaw($"USP_GetCINDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault();
            if (dataforCIN != null)
            {
                ViewBag.dataforCIN = "true";
                obj.ActiveCompliance = dataforCIN.ActiveCompliance;
                obj.AddressotherthanRegisteredoffice = dataforCIN.AddressotherthanRegisteredoffice;
                obj.AuthorizedCapital = dataforCIN.AuthorizedCapital;
                obj.BalanceSheetDate = dataforCIN.BalanceSheetDate;
                obj.CIN = dataforCIN.CIN;
                obj.CategoryforCIN = dataforCIN.Category;
                obj.Class = dataforCIN.Class;
                obj.CompanyName = dataforCIN.CompanyName;
                obj.CompanyType = dataforCIN.CompanyType;
                obj.DateofIncorporation = dataforCIN.DateofIncorporation;
                obj.LastAnnualGeneralMeetingDate = dataforCIN.LastAnnualGeneralMeetingDate;
                obj.ListedorUnlisted = dataforCIN.ListedorUnlisted;
                obj.NumberofDirectors = dataforCIN.NumberofDirectors;
                obj.NumberofMembers = dataforCIN.NumberofMembers.ToString();
                obj.PaidUpCapital = dataforCIN.PaidUpCapital;
                obj.ROCOffice = dataforCIN.ROCOffice;
                obj.RegisteredAddress = dataforCIN.RegisteredAddress;
                obj.RegisteredEmailId = dataforCIN.RegisteredEmailId;
                obj.RegistrationNumber = dataforCIN.RegistrationNumber;
                obj.StatusForEfiling = dataforCIN.StatusForEfiling;
                obj.SubCategory = dataforCIN.SubCategory;
                obj.Suspendedatstockexchange = dataforCIN.Suspendedatstockexchange;
                obj.DIN1 = dataforCIN.DIN;
                obj.DateofAppointment1 = dataforCIN.DateofAppointment;
                obj.Name1 = dataforCIN.Name;
                obj.SurrenderedDIN1 = dataforCIN.SurrenderedDIN;
                obj.DIN2 = dataforCIN.DIN2;
                obj.DateofAppointment2 = dataforCIN.DateofAppointment2;
                obj.Name2 = dataforCIN.Name2;
                obj.SurrenderedDIN2 = dataforCIN.SurrenderedDIN2;

            }
            var dataforGSTIN = objDetails.GSTIN_Verifications.FromSqlRaw($"USP_GetGSTINDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault();
            if (dataforGSTIN != null)
            {
                ViewBag.dataforGSTIN = "true";
                obj.PAN = dataforGSTIN.PAN;
                obj.CreatedByforGSTIN = dataforGSTIN.created_at;
                obj.ref_idforGSTIN = dataforGSTIN.ref_id;
                obj.statusMessageforGSTIN = dataforGSTIN.statusMessage;
            }
            var dataforMSME = objDetails.MSME_Verifications.FromSqlRaw($"USP_GetMSMEDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault();
            if (dataforMSME != null)
            {
                ViewBag.dataforMSME = "true";
                obj.CategoryforMSME = dataforMSME.Category;
                obj.DateofCommencement = dataforMSME.DateofCommencement;
                obj.District = dataforMSME.District;
                obj.company_name = dataforMSME.company_name;
                obj.StateforMSME = dataforMSME.State;
                obj.messageforMSME = dataforMSME.message;
                obj.status = dataforMSME.status;
                obj.created_atforMSME = dataforMSME.created_at;
            }


            return View(obj);
        }

        [HttpPost]
        public ActionResult CurrentCAF(Current_Verification ObjCurrentCaf)
        {
            ErrorLog error_log = new ErrorLog();
            //Current_Verification ObjCurrentCaf = new Current_Verification();
            var CustomerId = HttpContext.Session.GetString("PersonalId");

            //manipulate the caf textbox values to current page
            TempData["CompanyName"] = ObjCurrentCaf.company_name;
            TempData["IndustryType"] = ObjCurrentCaf.Industrytype;
            TempData["BusinessType"] = ObjCurrentCaf.BusinessTL;
            TempData["DateESTD"] = ObjCurrentCaf.DOE;
            TempData["PlaceESTD"] = ObjCurrentCaf.POE;
            TempData["NoOfBranches"] = ObjCurrentCaf.Branches;
            TempData["NoOfEmp"] = ObjCurrentCaf.NOE;
            TempData["Turnover"] = ObjCurrentCaf.Turnover;
            TempData["Address1"] = ObjCurrentCaf.PAddress1;
            TempData["Address2"] = ObjCurrentCaf.PAddress2;
            TempData["City"] = ObjCurrentCaf.PCity;
            TempData["Pincode"] = ObjCurrentCaf.PPincode;
            TempData["State"] = ObjCurrentCaf.PState;
            TempData["Country"] = ObjCurrentCaf.PCountry;
            TempData["Email"] = ObjCurrentCaf.Email;
            TempData["MobileNo"] = ObjCurrentCaf.Mobile;
            TempData["LandlineNo"] = ObjCurrentCaf.Landline;
            TempData["CinNo"] = ObjCurrentCaf.CINCAF;
            TempData["CompanyPan"] = ObjCurrentCaf.DINCAF;
            TempData["PanNo"] = ObjCurrentCaf.PanCAF;
            TempData["GSTNo"] = ObjCurrentCaf.GSTNCAF;
            TempData["UdyogAdhaarNo"] = ObjCurrentCaf.UdyogAadhaarCAF;
            //return RedirectToAction("GOTOCURRENTACCOUNT", "CurrentDataVerify");




            try
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_UpsertADM_CAFCustomerDetails", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerId);
                    cmd.Parameters.AddWithValue("@Comapnyname", ObjCurrentCaf.company_name);
                    cmd.Parameters.AddWithValue("@Industrytype", ObjCurrentCaf.Industrytype);
                    cmd.Parameters.AddWithValue("@BusinessTL", ObjCurrentCaf.BusinessTL);
                    cmd.Parameters.AddWithValue("@DOE", ObjCurrentCaf.DOE);
                    cmd.Parameters.AddWithValue("@POE", ObjCurrentCaf.POE);
                    cmd.Parameters.AddWithValue("@Branches", ObjCurrentCaf.Branches);
                    cmd.Parameters.AddWithValue("@NOE", ObjCurrentCaf.NOE);
                    cmd.Parameters.AddWithValue("@Turnover", ObjCurrentCaf.Turnover);
                    cmd.Parameters.AddWithValue("@Email", ObjCurrentCaf.Email);
                    cmd.Parameters.AddWithValue("@Mobile", ObjCurrentCaf.Mobile);
                    cmd.Parameters.AddWithValue("@Landline", ObjCurrentCaf.Landline);
                    cmd.Parameters.AddWithValue("@PAddress1", ObjCurrentCaf.PAddress1);
                    cmd.Parameters.AddWithValue("@PAddress2", ObjCurrentCaf.PAddress2);
                    cmd.Parameters.AddWithValue("@PAddress3", ObjCurrentCaf.PAddress3);
                    cmd.Parameters.AddWithValue("@PCity", ObjCurrentCaf.PCity);
                    cmd.Parameters.AddWithValue("@PPincode", ObjCurrentCaf.PPincode);
                    cmd.Parameters.AddWithValue("@PState", ObjCurrentCaf.PState);
                    cmd.Parameters.AddWithValue("@PCountry", ObjCurrentCaf.PCountry);
                    cmd.Parameters.AddWithValue("@CAddress1", ObjCurrentCaf.CAddress1);
                    cmd.Parameters.AddWithValue("@CAddress2", ObjCurrentCaf.CAddress2);
                    cmd.Parameters.AddWithValue("@CAddress3", ObjCurrentCaf.CAddress3);
                    cmd.Parameters.AddWithValue("@CCity", ObjCurrentCaf.CCity);
                    cmd.Parameters.AddWithValue("@CPincode", ObjCurrentCaf.CPincode);
                    cmd.Parameters.AddWithValue("@CState", ObjCurrentCaf.CState);
                    cmd.Parameters.AddWithValue("@CCountry", ObjCurrentCaf.CCountry);
                    cmd.Parameters.AddWithValue("@CINCAF", ObjCurrentCaf.CINCAF);
                    cmd.Parameters.AddWithValue("@DINCAF", ObjCurrentCaf.DINCAF);
                    cmd.Parameters.AddWithValue("@PanCAF", ObjCurrentCaf.PanCAF);
                    cmd.Parameters.AddWithValue("@GSTNCAF", ObjCurrentCaf.GSTNCAF);
                    cmd.Parameters.AddWithValue("@UdyogAadhaarCAF", ObjCurrentCaf.UdyogAadhaarCAF);
                    cmd.Parameters.AddWithValue("@PB", ObjCurrentCaf.PB);
                    cmd.Parameters.AddWithValue("@AML", ObjCurrentCaf.AML);
                    cmd.Parameters.AddWithValue("@PHOTO", ObjCurrentCaf.DigiKYCPhoto);
                    connection.Open();
                    var abc = cmd.ExecuteNonQuery();
                    return Json("Success");
                }
                return View();
            }
            catch (System.Exception ex)
            {
                _ = error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        [HttpPost]
        public IActionResult GetPincodeData(string pincode)
        {
            // Validate the pincode (you can add your validation logic here)
            if (!IsValidPincode(pincode))
            {
                return Json(new { error = "Invalid pincode" });
            }

            // Execute the stored procedure to retrieve values
            var district = ""; // Placeholder for the district value
            var state = ""; // Placeholder for the state value

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("USP_GetPincodeData", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Pincode", pincode);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Retrieve the district and state values from the reader
                        district = reader["District"].ToString();
                        state = reader["State_Name"].ToString();
                    }
                }
            }

            // Return the data as JSON
            return Json(new { district, state });
        }

        private bool IsValidPincode(string pincode)
        {


            bool isValid = true;

            if (string.IsNullOrEmpty(pincode) || pincode.Length != 6 || !pincode.All(char.IsDigit))
            {
                isValid = false;
            }

            return isValid;
        }



    }
}
