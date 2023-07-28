using INDO_FIN_NET.Models;
using INDO_FIN_NET.Models.CurrentModels;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace INDO_FIN_NET.Controllers.CurrectDetails
{
    public class CurrentServicesController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        private readonly string _connectionString;
        public CurrentServicesController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData1 = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
        }
        [HttpGet]
        public ActionResult CurrentService()
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
        public ActionResult CurrentServices()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult IDVerifyforPan(string IdType, string Idvalue)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                Current_Verification obj = new Current_Verification();
                var CustomerId = HttpContext.Session.GetString("PersonalId");
                var userid = HttpContext.Session.GetString("UseID");
                var client = new RestClient("https://apigateway.indofinnet.com/api/GSTINfromPan?OrgID=IndoFin007&PAN_number=" + Idvalue);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;
                string res1 = res.Replace(@"\", "");
                string res12 = res1.Replace(@"\", "");
                string res2 = res12.Replace("{", ",");
                string res3 = res2.Replace("{", "");
                string res4 = res3.Replace(":", "");
                string res5 = res4.Replace(",", "");
                string res6 = res5.Replace(".", "");
                string[] ress = res6.Split('"');
                string code = ress[3];
                string CONSTITUTIONOFBUSINESS = ress[8];
                string GSTIN = ress[12];
                string LEGALNAMEOFBUSINESS = ress[16];
                string State = ress[20];
                string Status = ress[24];
                string created_at = ress[34];
                string ref_id = ress[38];
                string statusMessageforPAN = ress[44];
                if (code == "200")
                {



                    using (SqlConnection connection2 = new SqlConnection(_connectionString))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_InsertPanfromGST", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@CustomerId", CustomerId);
                        cmd2.Parameters.AddWithValue("@CONSTITUTIONOFBUSINESS", CONSTITUTIONOFBUSINESS);
                        cmd2.Parameters.AddWithValue("@GSTIN", ress[12]);
                        cmd2.Parameters.AddWithValue("@LEGALNAMEOFBUSINESS", ress[16]);
                        cmd2.Parameters.AddWithValue("@State", ress[20]);
                        cmd2.Parameters.AddWithValue("@Status", ress[24]);
                        cmd2.Parameters.AddWithValue("@created_at", ress[34]);
                        cmd2.Parameters.AddWithValue("@ref_id", ress[38]);
                        cmd2.Parameters.AddWithValue("@statusCode", ress[41]);
                        cmd2.Parameters.AddWithValue("@statusMessage", ress[44]);
                        cmd2.Parameters.AddWithValue("@CreatedBy", userid);
                        connection2.Open();
                        cmd2.ExecuteNonQuery();
                        connection2.Close();
                    }


                    return Json(new { CONSTITUTIONOFBUSINESS, GSTIN, LEGALNAMEOFBUSINESS, State, Status, created_at, ref_id, statusMessageforPAN });
                }
                else
                {
                    return Json("Failed");
                }
            }
            catch (Exception ex)
            {
                _ = error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        public ActionResult IDVerifyforCIN(CIN_Verification obj, string IdType, string Idvalue)
        {


            ErrorLog error_log = new ErrorLog();
            try
            {
                var CustomerId = HttpContext.Session.GetString("PersonalId");
                var userid = HttpContext.Session.GetString("UseID");
                var client = new RestClient("https://apigateway.indofinnet.com/api/CINSearch?OrgID=IndoFin007&Cin=" + Idvalue);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;
                string res1 = res.Replace(@"\", "");
                string res12 = res1.Replace(@"\", "");
                string res2 = res12.Replace("{", ",");
                string res3 = res2.Replace("{", "");
                string res4 = res2.Replace(",", "");
                string res5 = res2.Replace(":", "");
                string res6 = res5.Replace(".", "");
                string res7 = res6.Replace(",", "");
                string[] ress = res7.Split('"');
                string objName = res3.Split(',')[2];
                //string codeString = Regex.Replace(ress[03], "[^0-9]", "");
                obj.code = ress[03].ToString();
                if (obj.code == "200")
                {

                    obj.ActiveCompliance = ress[08].ToString().Trim();
                    obj.AddressotherthanRegisteredoffice = ress[12].ToString().Trim();
                    obj.AuthorizedCapital = ress[16].ToString().Trim();
                    string balancesheetStr = ress[20].ToString().Trim();
                    //DateTime balancesheet = DateTime.ParseExact(balancesheetStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    obj.BalanceSheetDate = balancesheetStr;
                    obj.CIN = ress[24].ToString().Trim();
                    obj.Category = ress[28].ToString().Trim();
                    obj.Class = ress[32].ToString().Trim();
                    obj.CompanyName = ress[36].ToString().Trim();
                    obj.CompanyType = ress[40].ToString().Trim();
                    string DateofIncorporationStr = ress[44].ToString().Trim();
                    //DateTime DateofIncorporation = DateTime.ParseExact(DateofIncorporationStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    obj.DateofIncorporation = DateofIncorporationStr;
                    string LastAnnualGeneralMeetingDateStr = ress[44].ToString().Trim();
                    // DateTime LastAnnualGeneralMeetingDate = DateTime.ParseExact(LastAnnualGeneralMeetingDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    obj.LastAnnualGeneralMeetingDate = LastAnnualGeneralMeetingDateStr;

                    obj.ListedorUnlisted = ress[52].ToString().Trim();
                    string directorsString = ress[55].Trim();
                    string directorsOnlyNumeric = Regex.Replace(directorsString, "[^0-9]", "");
                    string NumberofDirectorstring = ress[58].Trim();
                    string NumberofDirectorsonlyNumeric = Regex.Replace(NumberofDirectorstring, "[^0-9]", "");

                    if (int.TryParse(directorsOnlyNumeric, out int numberOfDirectors))
                    {
                        obj.NumberofDirectors = numberOfDirectors;
                    }
                    if (int.TryParse(NumberofDirectorsonlyNumeric, out int NumberofMembers))
                    {
                        obj.NumberofMembers = NumberofMembers;
                    }

                    //obj.NumberofMembers = ress[58].Trim();
                    obj.PaidUpCapital = ress[62].ToString().Trim();
                    obj.ROCOffice = ress[66].ToString().Trim();
                    obj.RegisteredAddress = ress[70].ToString().Trim();
                    obj.RegisteredEmailId = ress[74].ToString().Trim();
                    obj.RegistrationNumber = ress[78].ToString().Trim();
                    obj.StatusForEfiling = ress[82].ToString().Trim();
                    obj.SubCategory = ress[86].ToString().Trim();
                    obj.Suspendedatstockexchange = ress[90].ToString().Trim();
                    obj.DIN = ress[96].ToString().Trim();


                    string DateofAppointmentStr = ress[100].ToString().Trim();

                    obj.DateofAppointment = DateofAppointmentStr;
                    string enddateStr = ress[104].ToString().Trim();
                    obj.Enddate = enddateStr;
                    obj.Name = ress[108].ToString().Trim();
                    obj.SurrenderedDIN = ress[112].ToString().Trim();
                    obj.DIN2 = ress[116].ToString().Trim();
                    obj.DateofAppointment2 = ress[120].ToString().Trim();
                    obj.Enddate2 = ress[124].ToString().Trim();
                    obj.Name2 = ress[128].ToString().Trim();
                    obj.SurrenderedDIN2 = ress[132].ToString().Trim();


                    // remove all non-numeric characters

                    obj.message = ress[136].ToString().Trim();
                    string created_atStr = ress[142].ToString().Trim();
                    obj.created_at = created_atStr;

                    obj.ref_id = ress[146].ToString().Trim();
                    obj.statusCode = ress[149].ToString();
                    obj.statusMessage = ress[152].ToString().Trim();

                    using (SqlConnection cn = new SqlConnection(_connectionString))
                    {
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("USP_Insert_CinDetails", cn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                        cmd.Parameters.AddWithValue("@ActiveCompliance", obj.ActiveCompliance);
                        cmd.Parameters.AddWithValue("@AddressotherthanRegisteredoffice", obj.AddressotherthanRegisteredoffice);
                        cmd.Parameters.AddWithValue("@AuthorizedCapital", obj.AuthorizedCapital);

                        cmd.Parameters.AddWithValue("@BalanceSheetDate", obj.BalanceSheetDate);

                        cmd.Parameters.AddWithValue("@CIN", obj.CIN);
                        cmd.Parameters.AddWithValue("@Category", obj.Category);
                        cmd.Parameters.AddWithValue("@Class", obj.Class);
                        cmd.Parameters.AddWithValue("@CompanyName", obj.CompanyName);
                        cmd.Parameters.AddWithValue("@CompanyType", obj.CompanyType);
                        cmd.Parameters.AddWithValue("@DateofIncorporation", obj.DateofIncorporation);
                        cmd.Parameters.AddWithValue("@LastAnnualGeneralMeetingDate", obj.LastAnnualGeneralMeetingDate);

                        cmd.Parameters.AddWithValue("@ListedorUnlisted", obj.ListedorUnlisted);
                        cmd.Parameters.AddWithValue("@NumberofDirectors", obj.NumberofDirectors);
                        cmd.Parameters.AddWithValue("@NumberofMembers", obj.NumberofMembers);
                        cmd.Parameters.AddWithValue("@PaidUpCapital", obj.PaidUpCapital);
                        cmd.Parameters.AddWithValue("@ROCOffice", obj.ROCOffice);
                        cmd.Parameters.AddWithValue("@RegisteredAddress", obj.RegisteredAddress);
                        cmd.Parameters.AddWithValue("@RegisteredEmailId", obj.RegisteredEmailId);
                        cmd.Parameters.AddWithValue("@RegistrationNumber", obj.RegistrationNumber);
                        cmd.Parameters.AddWithValue("@StatusForEfiling", obj.StatusForEfiling);
                        cmd.Parameters.AddWithValue("@SubCategory", obj.SubCategory);
                        cmd.Parameters.AddWithValue("@Suspendedatstockexchange", obj.Suspendedatstockexchange);
                        cmd.Parameters.AddWithValue("@DIN", obj.DIN);
                        cmd.Parameters.AddWithValue("@DateofAppointment", obj.DateofAppointment);
                        cmd.Parameters.AddWithValue("@Enddate", obj.Enddate);
                        cmd.Parameters.AddWithValue("@Name", obj.Name);
                        cmd.Parameters.AddWithValue("@SurrenderedDIN", obj.SurrenderedDIN);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@message", obj.message);
                        cmd.Parameters.AddWithValue("@created_at", obj.created_at);
                        cmd.Parameters.AddWithValue("@ref_id", obj.ref_id);
                        cmd.Parameters.AddWithValue("@statusCode", obj.statusCode);
                        cmd.Parameters.AddWithValue("@statusMessage", obj.statusMessage);
                        cmd.Parameters.AddWithValue("@CreatedBy", userid);
                        cmd.Parameters.AddWithValue("@DIN2", obj.DIN2);
                        cmd.Parameters.AddWithValue("@DateofAppointment2", obj.DateofAppointment2);
                        cmd.Parameters.AddWithValue("@Enddate2", obj.Enddate2);
                        cmd.Parameters.AddWithValue("@Name2", obj.Name2);
                        cmd.Parameters.AddWithValue("@SurrenderedDIN2", obj.SurrenderedDIN2);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();


                    }

                    return Json(obj);
                }
                else
                {
                    return Json("Failed");
                }
            }
            catch (Exception ex)
            {
                _ = error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        public ActionResult IDVerifyforMSME(string IdType, string Idvalue)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var CustomerId = HttpContext.Session.GetString("PersonalId");
                var userid = HttpContext.Session.GetString("UseID");

                var client = new RestClient("https://apigateway.indofinnet.com/api/UdyogAadharBasic?OrgID=IndoFin007&Uam=" + Idvalue);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;
                string res1 = res.Replace(@"\", "");
                string res12 = res1.Replace(@"\", "");
                string res2 = res12.Replace("{", ",");
                string res3 = res2.Replace("{", "");
                string res27 = res2.Replace(":", "");
                string res28 = res27.Replace(",", "");
                string res29 = res28.Replace(".", "");
                string[] ress = res29.Split('"');
                // string objName = res3.Split(',')[2];
                // res30 = res29.Split(",")[44];
                string code = ress[3].ToString().Trim();
                if (code == "200")
                {
                    string Category = ress[8].ToString().Trim();
                    string DateofCommencement = ress[12].ToString().Trim();
                    string District = ress[16].ToString().Trim();
                    string company_name = ress[20].ToString().Trim();
                    string State = ress[24].ToString().Trim();
                    string message = ress[28].ToString().Trim();
                    string status = ress[31].ToString().Trim();
                    string created_at = ress[34].ToString().Trim();
                    string ref_id = ress[38].ToString().Trim();
                    string statusCode = ress[41].ToString().Trim();
                    string statusMessage = ress[44].ToString().Trim();
                    using (SqlConnection cn = new SqlConnection(_connectionString))
                    {
                        string s = (HttpContext.Session.GetString("PersonalId"));
                        cn.Open();
                        SqlCommand cmd = new SqlCommand("USP_InsertUdyogAadhaar", cn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
                        cmd.Parameters.AddWithValue("@code", ress[3].ToString().Trim());
                        cmd.Parameters.AddWithValue("@Category", ress[8].ToString().Trim());
                        cmd.Parameters.AddWithValue("@DateofCommencement", ress[12].ToString().Trim());
                        cmd.Parameters.AddWithValue("@District ", ress[16].ToString().Trim());
                        cmd.Parameters.AddWithValue("@company_name", ress[20].ToString().Trim());
                        cmd.Parameters.AddWithValue("@State", ress[24].ToString().Trim());
                        cmd.Parameters.AddWithValue("@message", ress[28].ToString().Trim());
                        cmd.Parameters.AddWithValue("@status", ress[31].ToString().Trim());
                        cmd.Parameters.AddWithValue("@created_at", ress[34].ToString().Trim());
                        cmd.Parameters.AddWithValue("@ref_id", ress[38].ToString().Trim());
                        cmd.Parameters.AddWithValue("@statusCode", ress[41].ToString().Trim());
                        cmd.Parameters.AddWithValue("@statusMessage", ress[44].Trim());
                        cmd.Parameters.AddWithValue("@CreatedDate", "");
                        cmd.Parameters.AddWithValue("@CreatedBy", userid);
                        cmd.ExecuteNonQuery();
                        //string[] result = { (Category), (DateofCommencement), (District), (company_name), (State), (message), (status), (created_at) };
                        return Json(new { Category, DateofCommencement, District, company_name, State, message, status, created_at });

                    }
                }
                else
                {
                    return Json("Failed");
                }
            }
            catch (Exception ex)
            {
                _ = error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult IDVerifyforDIN(string IdType, string Idvalue)
        {
            var CustomerId = HttpContext.Session.GetString("PersonalId");
            var userid = HttpContext.Session.GetString("UseID");
            var client = new RestClient("https://apigateway.indofinnet.com/api/DINSearch?OrgID=IndoFin007&din=" + Idvalue);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
            IRestResponse response = client.Execute(request);
            string res = response.Content;
            string res1 = res.Replace(@"\", "");
            string res12 = res1.Replace(@"\", "");
            string res2 = res12.Replace("{", ",");
            string res3 = res2.Replace("{", "");
            string res5 = res3.Replace(":", "");
            string res6 = res5.Replace(",", "");
            string[] ress = res6.Split('"');
            string code = res3.Split(',')[1];
            string code1 = code.Split(':')[1];
            if (code1 == "200")
            {
                string DINno = ress[8].ToString().Trim();
                string DOB = ress[12].ToString().Trim();
                string FathrName = ress[16].ToString().Trim();
                string Name = ress[20].ToString().Trim();
                string Nationality = ress[24].ToString().Trim();
                string PanNo = ress[24].ToString().Trim();
                string RegAdd = ress[30].ToString().Trim();
                string RegEmail = ress[34].ToString().Trim();

                string CINNo = ress[48].ToString().Trim();
                string Message = ress[56].ToString().Trim();
                string CompanyName = ress[52].ToString().Trim();
                string createdat = ress[62].ToString().Trim();
                string RefId = ress[66].ToString().Trim();
                string Statuscd = ress[69].ToString().Trim();
                string statusMessage = ress[72].ToString().Trim();

                using (SqlConnection connection3 = new SqlConnection(_connectionString))
                {
                    connection3.Open();
                    SqlCommand cmd2 = new SqlCommand("USP_InsertDinData", connection3);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.AddWithValue("@CustomerId", CustomerId);
                    cmd2.Parameters.AddWithValue("@code", code1);
                    cmd2.Parameters.AddWithValue("@cin_number", CINNo);
                    cmd2.Parameters.AddWithValue("@company_name", CompanyName);
                    cmd2.Parameters.AddWithValue("@DIN", DINno);
                    cmd2.Parameters.AddWithValue("@DateofBirth", DOB);
                    cmd2.Parameters.AddWithValue("@FathersName", FathrName);
                    cmd2.Parameters.AddWithValue("@Name", Name);
                    cmd2.Parameters.AddWithValue("@Nationality", Nationality);
                    cmd2.Parameters.AddWithValue("@PanNumber", PanNo);
                    cmd2.Parameters.AddWithValue("@RegisteredAddress", RegAdd);
                    cmd2.Parameters.AddWithValue("@RegisteredEmailID", RegEmail);
                    cmd2.Parameters.AddWithValue("@message", Message);
                    cmd2.Parameters.AddWithValue("@created_at", createdat);
                    cmd2.Parameters.AddWithValue("@ref_id", RefId);
                    cmd2.Parameters.AddWithValue("@statusCode", Statuscd);
                    cmd2.Parameters.AddWithValue("@statusMessage", statusMessage);
                    cmd2.Parameters.AddWithValue("@CreatedBy", userid);
                    cmd2.Parameters.AddWithValue("@CreatedDate", DateTime.Now);


                    cmd2.ExecuteReader();
                    connection3.Close();

                }
                return Json("Success");
            }
            else
            {
                return Json("Failed");
            }
        }

        public ActionResult IDVerifyforGSTIN(string IdType, string Idvalue)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var CustomerId = HttpContext.Session.GetString("PersonalId");
                var userid = HttpContext.Session.GetString("UseID");
                var client = new RestClient("https://apigateway.indofinnet.com/api/PanfromGSTIN?OrgID=IndoFin007&GSTIN=" + Idvalue);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;
                string res1 = res.Replace(@"\", "");
                string res12 = res1.Replace(@"\", "");
                string res2 = res12.Replace("{", ",");
                string res3 = res2.Replace("{", "");
                string res5 = res3.Replace(":", "");
                string res6 = res5.Replace(",", "");
                string[] ress = res6.Split('"');
                string code = ress[3];
                string PAN = ress[8];
                string created_at = ress[18];
                string ref_id = ress[22];
                string statuscode = ress[25];
                string statusMessage = ress[28];

                if (code == "200")
                {
                    using (SqlConnection connection3 = new SqlConnection(_connectionString))
                    {
                        connection3.Open();
                        SqlCommand cmd2 = new SqlCommand("USP_InsertPanfromGSTIN", connection3);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@CustomerId", CustomerId);
                        cmd2.Parameters.AddWithValue("@PAN", PAN);
                        cmd2.Parameters.AddWithValue("@created_at", created_at);
                        cmd2.Parameters.AddWithValue("@ref_id", ref_id);
                        cmd2.Parameters.AddWithValue("@statuscode", statuscode);
                        cmd2.Parameters.AddWithValue("@statusMessage", statusMessage);
                        cmd2.Parameters.AddWithValue("@CreatedBy", userid);


                        cmd2.ExecuteReader();
                        connection3.Close();
                    }
                    return Json(new { PAN, created_at, ref_id, statuscode, statusMessage });

                }
                else
                {
                    return Json("Failed");
                }
            }
            catch (Exception ex)
            {
                _ = error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
    }
}
