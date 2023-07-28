using INDO_FIN_NET.Models.CurrentModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using RestSharp;
using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using RestSharp.Serialization.Json;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using System.Xml;
using INDO_FIN_NET.Models;
using System.Text.RegularExpressions;
using INDO_FIN_NET.Repository;
using Spire.Pdf.Widget;
using Org.BouncyCastle.Ocsp;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using System.Text;
using AutoMapper;
using NuGet.Packaging.Signing;
using System.Xml.Linq;
using Aspose.Pdf;
using Resolution2 = Aspose.Pdf.Devices.Resolution;
using static INDO_FIN_NET.dcumentdedupe;
using Aspose.Cells;
using Org.BouncyCastle.Tsp;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using Aspose.Pdf.Drawing;
using Point = System.Drawing.Point;
using Aspose.Pdf.Devices;
using Path = System.IO.Path;
using System.Net.Mail;
using System.IO.Compression;
using INDO_FIN_NET.Repository.Data;
using System.Configuration;
using Amazon.DynamoDBv2.Model;



namespace INDO_FIN_NET.Controllers.CurrectDetails
{
    public class CurrentQuickEnrollController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        private readonly string _connectionString;
        public CurrentQuickEnrollController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData1 = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
        }

        string extension = null;
        [HttpGet]
        public ActionResult CurrentQuickEnrollment(string NewCJourney)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                List<DataItem> dataList = new List<DataItem>();
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT org_type FROM adm_CurrentOrgtype", connection))
                {
                    SqlDataReader data = cmd.ExecuteReader();
                    while (data.Read())
                    {
                        DataItem item = new DataItem();
                        item.org_type = data["org_type"].ToString();

                        dataList.Add(item);
                    }
                }
                connection.Close();
                ViewBag.Data = dataList;
            }
            //bool toCheckPartner = (bool)TempData["addPartner"];
            bool? toCheckPartner = null; // Default value when TempData["addPartner"] is null

            if (TempData["addPartner"] != null && TempData["addPartner"] is bool)
            {
                toCheckPartner = (bool)TempData["addPartner"];
            }
            ViewData["toCheckPartner"] = toCheckPartner;
            var DigiDoc = HttpContext.Session.GetString("DigiDoc");
            ViewBag.DigiDoc = DigiDoc;
            if (DigiDoc == "PAN")
            {
                // digilocker pandata
                string PANNo = TempData["PANNo"] as string;
                string fname = TempData["PFName"] as string;
                string mname = TempData["PMName"] as string;
                string lname = TempData["PLName"] as string;
                string dob = TempData["PDOB"] as string;
                string gender = TempData["PGender"] as string;
                string country = TempData["PCoutry"] as string;
                string orgname = TempData["PORGName"] as string;

                ViewBag.PANNo = PANNo;
                ViewBag.FNAME = fname;
                ViewBag.MNAME = mname;
                ViewBag.LNAME = lname;
                ViewBag.DOB = dob;
                ViewBag.Gender = gender;
                ViewBag.Country = country;
                ViewBag.Orgname = orgname;

            }

            else if (DigiDoc == "PANwithxml")
            {
                string PANNo = TempData["PANNo"] as string;
                string fname = TempData["PFName"] as string;
                string mname = TempData["PMName"] as string;
                string lname = TempData["PLName"] as string;
                string dob = TempData["PDOB"] as string;
                string gender = TempData["PGender"] as string;
                string country = TempData["PCoutry"] as string;
                string orgname = TempData["PORGName"] as string;

                ViewBag.PANNo = PANNo;
                ViewBag.FNAME = fname;
                ViewBag.MNAME = mname;
                ViewBag.LNAME = lname;
                ViewBag.DOB = dob;
                ViewBag.Gender = gender;
                ViewBag.Country = country;
                ViewBag.Orgname = orgname;

                string AdhaarFname = TempData["AFName"] as string;
                string AdhaarMname = TempData["AMName"] as string;
                string AdhaarLname = TempData["ALName"] as string;
                string AdhaarPhoto = TempData["APhoto"] as string;
                string AdhaarDOB = TempData["ADOB"] as string;
                string AdhaarGender = TempData["AGender"] as string;
                string AdhaarCity = TempData["ACity"] as string;
                string AdhaarStreet = TempData["AStreet"] as string;
                string AdhaarState = TempData["AState"] as string;
                string AdhaarPincode = TempData["APincode"] as string;
                string AdhaarLocality = TempData["ALocality"] as string;
                string AdhaarHouse = TempData["AHouse"] as string;
                string AdhaarDistrict = TempData["ADistrict"] as string;
                string AdhaarCountry = TempData["ACountry"] as string;
                string AdhaarNo = TempData["AdhaarNo"] as string;

                ViewBag.AdhaarFname = AdhaarFname;
                ViewBag.AdhaarMname = AdhaarMname;
                ViewBag.AdhaarLname = AdhaarLname;
                ViewBag.AdhaarPhoto = AdhaarPhoto;
                ViewBag.AdhaarDOB = AdhaarDOB;
                ViewBag.AdhaarGender = AdhaarGender;
                ViewBag.AdhaarCity = AdhaarCity;
                ViewBag.AdhaarStreet = AdhaarStreet;
                ViewBag.AdhaarState = AdhaarState;
                ViewBag.AdhaarPincode = AdhaarPincode;
                ViewBag.AdhaarLocality = AdhaarLocality;
                ViewBag.AdhaarHouse = AdhaarHouse;
                ViewBag.AdhaarDistrict = AdhaarDistrict;
                ViewBag.AdhaarCountry = AdhaarCountry;
                ViewBag.AdhaarNo = AdhaarNo;
            }

            else if (DigiDoc == "AADHAAR_XML")
            {
                string AdhaarFname = TempData["AFName"] as string;
                string AdhaarMname = TempData["AMName"] as string;
                string AdhaarLname = TempData["ALName"] as string;
                string AdhaarPhoto = TempData["APhoto"] as string;
                string AdhaarDOB = TempData["ADOB"] as string;
                string AdhaarGender = TempData["AGender"] as string;
                string AdhaarCity = TempData["ACity"] as string;
                string AdhaarStreet = TempData["AStreet"] as string;
                string AdhaarState = TempData["AState"] as string;
                string AdhaarPincode = TempData["APincode"] as string;
                string AdhaarLocality = TempData["ALocality"] as string;
                string AdhaarHouse = TempData["AHouse"] as string;
                string AdhaarDistrict = TempData["ADistrict"] as string;
                string AdhaarCountry = TempData["ACountry"] as string;
                string AdhaarNo = TempData["AdhaarNo"] as string;

                ViewBag.AdhaarFname = AdhaarFname;
                ViewBag.AdhaarMname = AdhaarMname;
                ViewBag.AdhaarLname = AdhaarLname;
                ViewBag.AdhaarPhoto = AdhaarPhoto;
                ViewBag.AdhaarDOB = AdhaarDOB;
                ViewBag.AdhaarGender = AdhaarGender;
                ViewBag.AdhaarCity = AdhaarCity;
                ViewBag.AdhaarStreet = AdhaarStreet;
                ViewBag.AdhaarState = AdhaarState;
                ViewBag.AdhaarPincode = AdhaarPincode;
                ViewBag.AdhaarLocality = AdhaarLocality;
                ViewBag.AdhaarHouse = AdhaarHouse;
                ViewBag.AdhaarDistrict = AdhaarDistrict;
                ViewBag.AdhaarCountry = AdhaarCountry;
                ViewBag.AdhaarNo = AdhaarNo;
            }

            TempData.Clear();
            HttpContext.Session.Remove("DigiDoc");


            return View();
        }
        [HttpPost]
        public ActionResult CurrentQuickEnrollment(CQuickEnroll obj)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var userid = HttpContext.Session.GetString("UseID");
                var Ctype = HttpContext.Session.GetString("Ctype");
                if (Ctype == "PARTNERSHIP")
                {
                    var Pid = HttpContext.Session.GetString("PrimaryPartnerId");
                    using (SqlConnection connection2 = new SqlConnection(_connectionString))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_InsertCurrentAccQE", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@FirstName", obj.FirstName);
                        cmd2.Parameters.AddWithValue("@LastName", obj.LastName);
                        cmd2.Parameters.AddWithValue("@MobileNo", obj.MobileNO);
                        cmd2.Parameters.AddWithValue("@EmailId", obj.EmailID);
                        cmd2.Parameters.AddWithValue("@SessionToken", "");
                        cmd2.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd2.Parameters.AddWithValue("@CreatedBy", userid);
                        cmd2.Parameters.AddWithValue("@VerificationType", obj.VerificationType);
                        if (obj.VerificationType == "Pan No")
                        {
                            obj.PanNo = obj.VerificationTypeValue;
                        }
                        else
                        {
                            obj.AadhaarNo = obj.VerificationTypeValue;
                        }
                        cmd2.Parameters.AddWithValue("@PanNo", obj.PanNo);
                        cmd2.Parameters.AddWithValue("@AadhaarNo", obj.AadhaarNo);
                        cmd2.Parameters.AddWithValue("@Category", "PARTNERSHIP");
                        cmd2.Parameters.AddWithValue("@PrimaryPId", Pid);
                        HttpContext.Session.SetString("Ctype", "PARTNERSHIP");

                        connection2.Open();
                        int result = cmd2.ExecuteNonQuery();
                        connection2.Close();

                        var results = "";
                        if (result > 0)
                        {
                            SqlCommand cmd3 = new SqlCommand("USP_Currentgetcustomerdetailid", connection2);
                            cmd3.CommandType = CommandType.StoredProcedure;
                            DataTable dt = new DataTable();
                            SqlDataAdapter dp = new SqlDataAdapter(cmd3);
                            connection2.Open();
                            dp.Fill(dt);
                            SqlDataReader reader3 = cmd3.ExecuteReader();

                            if (reader3.Read())
                            {
                                results = reader3["CustomerID"].ToString();
                                if (results != null)
                                {
                                    HttpContext.Session.SetString("PersonalId", results);
                                }
                            }
                            connection2.Close();

                        }
                    }

                }
                else
                {
                    using (SqlConnection connection2 = new SqlConnection(_connectionString))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_InsertCurrentAccQE", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@FirstName", obj.FirstName);
                        cmd2.Parameters.AddWithValue("@LastName", obj.LastName);
                        cmd2.Parameters.AddWithValue("@MobileNo", obj.MobileNO);
                        cmd2.Parameters.AddWithValue("@EmailId", obj.EmailID);
                        cmd2.Parameters.AddWithValue("@SessionToken", "");
                        cmd2.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                        cmd2.Parameters.AddWithValue("@CreatedBy", userid);
                        cmd2.Parameters.AddWithValue("@VerificationType", obj.VerificationType);
                        if (obj.VerificationType == "Pan No")
                        {
                            obj.PanNo = obj.VerificationTypeValue;
                        }
                        else
                        {
                            obj.AadhaarNo = obj.VerificationTypeValue;
                        }
                        cmd2.Parameters.AddWithValue("@PanNo", obj.PanNo);
                        cmd2.Parameters.AddWithValue("@AadhaarNo", obj.AadhaarNo);
                        cmd2.Parameters.AddWithValue("@Category", obj.Category);
                        if (obj.Category == "PARTNERSHIP")
                        {
                            cmd2.Parameters.AddWithValue("@IsPartnership", 1);
                            cmd2.Parameters.AddWithValue("@PartnerCount", 0);
                            HttpContext.Session.SetString("Ctype", obj.Category);
                        }

                        connection2.Open();
                        int result = cmd2.ExecuteNonQuery();
                        connection2.Close();

                        var results = "";
                        if (result > 0)
                        {
                            SqlCommand cmd3 = new SqlCommand("USP_Currentgetcustomerdetailid", connection2);
                            cmd3.CommandType = CommandType.StoredProcedure;
                            DataTable dt = new DataTable();
                            SqlDataAdapter dp = new SqlDataAdapter(cmd3);
                            connection2.Open();
                            dp.Fill(dt);
                            SqlDataReader reader3 = cmd3.ExecuteReader();

                            if (reader3.Read())
                            {
                                results = reader3["CustomerID"].ToString();
                                if (results != null)
                                {
                                    HttpContext.Session.SetString("PersonalId", results);
                                }
                                if (obj.Category == "PARTNERSHIP")
                                {
                                    HttpContext.Session.SetString("PrimaryPartnerId", results);

                                }
                            }
                            connection2.Close();

                        }
                    }

                }
                return Json("Success");
            }
            catch (Exception ex)
            {
                _ = error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
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
                    //string CONSTITUTIONOFBUSINESS = null;
                    //string GSTIN = null;
                    //string LEGALNAMEOFBUSINESS = null;
                    //string CONSTITUTIONOFBUSINESS = null;
                    //string CONSTITUTIONOFBUSINESS = null;
                    //var dataforPan = objDetails.PAN_Verifications.FromSqlRaw($"USP_GetPanDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault(); if (dataforPan != null)
                    //{

                    //     CONSTITUTIONOFBUSINESS = dataforPan.CONSTITUTIONOFBUSINESS;
                    //    GSTIN = dataforPan.GSTIN;
                    //    obj.LEGALNAMEOFBUSINESS = dataforPan.LEGALNAMEOFBUSINESS;
                    //    obj.State = dataforPan.State;
                    //    obj.Status = dataforPan.Status;
                    //    obj.created_at = dataforPan.created_at;
                    //    obj.statusMessageforPAN = dataforPan.statusMessage;
                    //}


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


        public ActionResult FetchDoc(string Verificationtxt3, string Verificationtxt4, string Verificationtxt6, string Verificationtxt8, bool DrivingLic, bool PanCard, bool Aadharxml, bool Offline_AADHAAR_XML, bool IsPanVerify)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var panno = Verificationtxt3;
                var DrivingNo = Verificationtxt4;
                var PANFullName = Verificationtxt8;
                string DocType = null;
                string FlagDoc = null;

                if (Verificationtxt3 != null)
                {
                    HttpContext.Session.SetString("DocumentDetails", Verificationtxt3);
                }
                if (Verificationtxt4 != null)
                {
                    HttpContext.Session.SetString("DocumentDetails1", Verificationtxt4);
                }
                if (Verificationtxt8 != null)
                {
                    HttpContext.Session.SetString("DocumentDetails2", Verificationtxt8);
                }
                if (PanCard != null)
                {
                    HttpContext.Session.SetString("DoctypeSelect", Convert.ToString(PanCard));
                }

                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);                                                       // HttpContext.Session.SetString("DoctypeSelect1", DrivingLic);// = DrivingLic;

                if (PanCard == true && DrivingLic == true && Aadharxml == true)
                {
                    HttpContext.Session.SetString("DigiDoc", "Bothwithxml");
                    FlagDoc = "Bothwithxml";
                    HttpContext.Session.SetString("DigilockerType", FlagDoc);
                }
                else if (PanCard == true && DrivingLic == true && Aadharxml == false)
                {
                    HttpContext.Session.SetString("DigiDoc", "Both");
                    FlagDoc = "Both";
                    HttpContext.Session.SetString("DigilockerType", FlagDoc);
                }
                else if (PanCard == true && DrivingLic == false && Aadharxml == true)
                {
                    HttpContext.Session.SetString("DigiDoc", "PANwithxml");
                    FlagDoc = "PANwithxml";
                    HttpContext.Session.SetString("DigilockerType", FlagDoc);
                }
                else if (PanCard == false && DrivingLic == true && Aadharxml == true)
                {
                    HttpContext.Session.SetString("DigiDoc", "Drvlwithxml");
                    FlagDoc = "Drvlwithxml";
                    HttpContext.Session.SetString("DigilockerType", FlagDoc);
                }
                else if (PanCard == true)
                {
                    HttpContext.Session.SetString("DigiDoc", "PAN");
                    FlagDoc = "PAN";
                    DocType = "PAN";
                    HttpContext.Session.SetString("DigilockerType", DocType);
                }
                else if (DrivingLic == true)
                {
                    HttpContext.Session.SetString("DigiDoc", "DRIVINGLICENSE");
                    FlagDoc = "DRIVINGLICENSE";
                    DocType = "DRIVINGLICENSE";
                    HttpContext.Session.SetString("DigilockerType", DocType);
                }
                else if (Aadharxml == true)
                {
                    FlagDoc = "AADHAAR_XML";
                    DocType = "AADHAAR_XML";
                    HttpContext.Session.SetString("DigiDoc", DocType);
                    HttpContext.Session.SetString("DigilockerType", "AADHAAR_XML");
                }

                else if (Offline_AADHAAR_XML == true)
                {
                    FlagDoc = "Offline_AADHAAR_XML";
                    DocType = "Offline_AADHAAR_XML";
                    HttpContext.Session.SetString("DigiDoc", DocType);
                    HttpContext.Session.SetString("DigilockerType", "AADHAAR_XML");
                }
                else if (IsPanVerify == true)
                {
                    FlagDoc = "IsPanVerify";
                    DocType = "IsPanVerify";
                    HttpContext.Session.SetString("DigiDoc", DocType);
                    HttpContext.Session.SetString("DigilockerType", DocType);
                }
                var AadharNo = Verificationtxt6;

                if (FlagDoc != null)
                {
                    TempData["FlagDoc"] = FlagDoc;
                }
                else
                {
                    TempData["FlagDoc"] = "";
                }
                if (panno != null)
                {
                    TempData["PanNo"] = panno;
                }
                else
                {
                    TempData["PanNo"] = "";
                }
                if (PANFullName != null)
                {
                    TempData["PANFullName"] = PANFullName;
                }
                else
                {
                    TempData["PANFullName"] = "";
                }
                if (DrivingNo != null)
                {
                    TempData["DrivingNo"] = DrivingNo;
                }
                else
                {
                    TempData["DrivingNo"] = "";
                }

                if (DocType != null)
                {
                    HttpContext.Session.SetString("DocType", DocType);
                }
                //FOR LIVE //
                string url = "https://apigateway.indofinnet.com/api/Digilocker?RequestId=015&OrgID=IndoFin007&ApiKey=IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE";
                //FOR LOCAL //
                //string url = "https://apigateway.indofinnet.com/api/Digilocker?RequestId=123&OrgID=IndoLocal007&ApiKey=7b708c49-4c83-4831-9ffc-79e833f7a9d5";

                //string url = "https://indofinnet.co.in/api/Digilocker/Digilocker?RequestId=1";
                // https://api.digitallocker.gov.in/public/oauth2/1/consent?response_type=code&client_id=D51BA951&state=test&redirect_uri=https%3A%2F%2Findofinnet.co.in%2Fapi%2FDigilocker%2FGetToken%2F&orgid=003851&txn=623ae946c27a8oauth21648028035&hashkey=f0858ccba281f228dc1dc500261e9eb63b398a099b5d39f2b2f96e4b4cf6a41a&enc=j1vuvcfksSaWA-9d5ClZOCz9h1E-D2zcXqvt8T8ba11n1glfsCXLwJzvZmJlRPAfo7MLdp9EDqV84QUWqlMwx2mJNADM-v2OraZPme2-nY7kSNRfCZ0pEh2Aqoucd6N9pIpnn-P4tkXmRb2hOpqq-4u1xRJk2OqeeLxTWvcCRTvE-yPreWMO4WPizOfwLxci8y1SsE37HI04qGECKk2d2NsjpbahAhxGOzrVNHxDaPmX8CVelcesQTk5fVti3RCQFy7MDGoXoVboLHeMD1uWrIbOB5vEwllDni6uZtM8jx7-2FyQ6frJsJUi-T44KjQuPxQl7XmnNiTxrD2j-vTSAloogl0fHoNs5oUOOsg8UIDqhcmxqsDDbPsFEkgJcrYPQFYcgONNCRuN6VB9g00n0YAn9xxXdSns%2FeySVaa7rRTka-G5yciFkIZ6JfXSPGnhTf6rLbwyXu405dAoCYfzpEFyKTCMTF0kjwYsZy24eQwxZ4Wk9P7CupvJgGCsZRwShW6XodJxy6fNyNnXpDW-tGOHqHrllak0V6lrvJabBuLfKruAF-v5lDPSdziPVmvrMs1O-b0uq%2FuZMIEiNngElwwGv6uYORh3Wjwr64YFFVn7bL1NwvH1fFC6A6QUPtAUiWH7iPKUvnD3JBEgq9FH2pFknTq4s9zDFd9fsPM6K4KpMnvLnFQt3f3pJ3xFYfPnsj0NJ4Lm9ApcF0stnadKIWSg--4g787z0Vw%2F6lQPPlcbtxYdih8eRUXwoLm0fmaw4bujBetWrlZVQIwlxiyEWDEpcB3ggf3m%2FyZzRiJyHcDOsUj3N2mftYEUmRGE%2F7aNenDzm%2FF3x-Oz79VsW2mtNyNHH9qxNZQ26zs7Eo2dY8zmgHdw2RtFB3ZtaY-pGxBUHO6iKxLkJSsyP9BrXCStJOxHEKpxghidGtGFWZdh8nCpMv24glRCEeAk7vn1DEKcQ9Dfw59a4SG4%2FIq17bYE3qImbNLuk8kxYtV47ksxbaI%3D
                return Redirect(url);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }


        public ActionResult CurrentDigilocker(string Code)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                TempData["code"] = Code;

                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                // PortalException.InsertPortalException(ex);
                return Json("Exception");
            }


        }

        public async Task<ActionResult> GetDocument()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                string path = "";
                var DC = TempData["code"].ToString();


                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var client = new RestClient("https://apigateway.indofinnet.com/api/GetActualToken?OrgID=IndoFin007&code=" + DC);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);


                var res = response.Content;
                var serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(response.Content);
                dynamic jobject = serializer.Deserialize<dynamic>(jsonObject);

                string access_token = jobject["access_token"];

                string DocType = "";

                if (DocType == "AADHAAR_XML")
                {
                    //string myURLFund2 = "https://indofinnet.co.in/api/Digilocker/EAadhaarXml?Token=" + access_token;

                    //var clientEA = new RestClient("https://indofinnet.co.in/api/Digilocker/EAadhaarXml?Token=" + access_token);
                    //var requestEA = new RestRequest(Method.GET);
                    //requestEA.AddHeader("postman-token", "3b751a45-c622-c74b-6714-6315e1f06e69");
                    //requestEA.AddHeader("cache-control", "no-cache");
                    //IRestResponse responseEA = clientEA.Execute(requestEA);
                    var clientEA = new RestClient("https://apigateway.indofinnet.com/api/GetEAadhaarXmlData?OrgID=Alpha01" + "&Token=" + access_token);
                    clientEA.Timeout = -1;
                    var requestEA = new RestRequest(Method.POST);
                    requestEA.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                    IRestResponse responseEA = clientEA.Execute(requestEA);

                    string xmlbase64 = responseEA.Content.Trim('\"');
                    string xml = Encoding.UTF8.GetString(Convert.FromBase64String(xmlbase64));
                    TempData["xml"] = xml;

                    return Content("SUCCESS");

                }
                string URI = "";
                string PanNo = TempData["PanNo"].ToString();

                string PANFullName = TempData["PANFullName"].ToString();
                string DrivingNo = TempData["DrivingNo"].ToString();
                string panadata = TempData["FlagDoc"].ToString();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
               ((sender, certificate, chain, sslPolicyErrors) => true);
                if (panadata == "PAN")
                {
                    string PanDoc = GetPanDoc(PanNo, PANFullName, access_token);
                    if (PanDoc == "Please enter valid PAN no. [EF-WS-BVB-ERR-10004] Entered PAN does not exist")
                    {
                        return Json("No response from Diglocker as the Pan no is not valid");
                    }
                    else if (PanDoc == "Issuer service is currently unavailable. Please try again later.")
                    {
                        return Json("Issuer service is currently unavailable. Please try again later.");
                    }
                }
                #region Driving 
                //else if (panadata == "DRIVINGLICENSE")
                //{
                //    string DrivingDOc = GetDrivingLicenseDoc(DrivingNo, access_token);
                //    if (DrivingDOc == "Your Date of birth as per Aadhaar did not match with document details.Please contact concerned department for correction (if needed).")
                //    {
                //        return Json("Your Date of birth as per Aadhaar did not match with document details.Please contact concerned department for correction (if needed).");
                //    }
                //    TempData["path"] = path;
                //}
                #endregion Driving

                #region Aadhar 
                else if (panadata == "AADHAAR_XML")
                {
                    string xmlEa = GetAadharXml(access_token);
                    TempData["xml"] = xmlEa;
                }

                #endregion Aadhar
                #region Both 
                //else if (panadata == "Both")
                //{
                //    string PanDoc = GetPanDoc(PanNo, PANFullName, access_token);
                //    string DrivingDOc = GetDrivingLicenseDoc(DrivingNo, access_token);
                //    path = "C:\\Digilocker\\XmlData\\" + PanNo + ".pdf";
                //    TempData["path"] = path;
                //}

                #endregion Both
                #region Panwithxml
                else if (panadata == "PANwithxml")
                {
                    string PanDoc = GetPanDoc(PanNo, PANFullName, access_token);
                    string aadharxml = GetAadharXml(access_token);
                    //TempData["path"] = path;
                    //if(PanDoc!=null || aadharxml!=null)
                    //{
                    //    return Json("No Response from digilocker");

                    //}
                }
                #endregion Panwithxml
                #region Drivingwithxml
                //else if (panadata == "Drvlwithxml")
                //{
                //    string DrivingDOc = GetDrivingLicenseDoc(DrivingNo, access_token);
                //    string aadharxml = GetAadharXml(access_token);
                //    TempData["path"] = path;
                //}

                #endregion Drivingwithxml

                #region allthre
                //else if (panadata == "Bothwithxml")
                //{
                //    string PanDoc = GetPanDoc(PanNo, PANFullName, access_token);
                //    string DrivingDOc = GetDrivingLicenseDoc(DrivingNo, access_token);
                //    string aadharxml = GetAadharXml(access_token);
                //    TempData["path"] = path;
                //}
                return Json("PDF");
                #endregion allthre
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }

        }

        public string GetPanDoc(string PanNo, string PANFullName, string access_token)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                 ((sender, certificate, chain, sslPolicyErrors) => true);
                RestClient clientPD = new RestClient();
                string DocNm = "PAN";
                clientPD = new RestClient("https://apigateway.indofinnet.com/api/PullDocument?OrgID=IndoFin007&doctype=" + DocNm + "&IdentificationNO=" + PanNo + "&name=" + PANFullName + "&Token=" + access_token);
                clientPD.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse responsePD = clientPD.Execute(request);

                var res = responsePD.Content;
                var serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(responsePD.Content);
                dynamic jsonURI = serializer.Deserialize<dynamic>(jsonObject);
                string URI = jsonURI["uri"];
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_DigilockerErrorlog", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt64((HttpContext.Session.GetString("PersonalId"))));
                    cmd.Parameters.AddWithValue("@Request", Convert.ToString(URI));
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var responce2 = reader["result"].ToString();
                        string uriresp = URI;//Encoding.UTF8.GetString(dspd);
                        HttpContext.Session.SetString("checkPanstatus", uriresp);

                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_UpdateDigilockrlog", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@Identity", Convert.ToInt64(responce2));
                            cmd2.Parameters.AddWithValue("@Response", uriresp);
                            cmd2.Parameters.AddWithValue("@Status", Convert.ToString(responsePD.StatusCode));
                            cmd2.Parameters.AddWithValue("@Digilockertype", "PAN");

                            connection2.Open();
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            if (reader2.Read())
                            {
                                var ivar = reader["result"].ToString();
                            }
                        }

                        if (URI == string.Empty || (URI == null))
                        {

                            TempData["Error"] = uriresp;
                            HttpContext.Session.SetString("CheckPan", "PanFalse");
                        }
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender, certificate, chain, sslPolicyErrors) => true);
                        var clientT = new RestClient("https://apigateway.indofinnet.com/api/FileAPI?OrgID=IndoFin007&URI=" + URI + "&Token=" + access_token);

                        clientT.Timeout = -1;
                        var requestT = new RestRequest(Method.POST);
                        requestT.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        IRestResponse responseT = clientT.Execute(requestT);
                        var serializer1 = new JavaScriptSerializer();
                        dynamic jsonObject1 = serializer1.Deserialize<dynamic>(responseT.Content);
                        dynamic jsonURI1 = serializer1.Deserialize<dynamic>(jsonObject1);
                        string pdfresponse = jsonURI1["doc"];

                        string XMLdata = GetXml(URI, access_token, PanNo);

                        if (XMLdata != "FAIL")
                        {
                            XDocument AuthXMLResponse = XDocument.Parse(XMLdata);
                            string PANNo = AuthXMLResponse.Root.Attribute("number") != null ? AuthXMLResponse.Root.Attribute("number").Value : "";
                            string name = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("name") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("name").Value : "";
                            string dob = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("dob") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("dob").Value : "";
                            string gender = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("gender") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("gender").Value : "";
                            string country = AuthXMLResponse.Root.Element("IssuedBy").Element("Organization").Element("Address").Attribute("country") != null ? AuthXMLResponse.Root.Element("IssuedBy").Element("Organization").Element("Address").Attribute("country").Value : "";
                            string ORGname = AuthXMLResponse.Root.Element("IssuedBy").Element("Organization").Attribute("name") != null ? AuthXMLResponse.Root.Element("IssuedBy").Element("Organization").Attribute("name").Value : "";
                            string PANverifiedOn = AuthXMLResponse.Root.Element("CertificateData").Element("PAN").Attribute("verifiedOn") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("PAN").Attribute("verifiedOn").Value : "";

                            string[] Name = (name).Split(' ');

                            string fname = Name[0];
                            string mname = Name[1];
                            string lname = Name[2];
                            string UserId = Convert.ToString(HttpContext.Session.GetString("UseID"));
                            TempData["PANNo"] = PANNo;
                            TempData["PFName"] = fname;
                            TempData["PMName"] = mname;
                            TempData["PLName"] = lname;
                            TempData["PDOB"] = dob;
                            TempData["PGender"] = gender;
                            TempData["PCoutry"] = country;
                            TempData["PORGName"] = ORGname;




                            using (SqlConnection connection3 = new SqlConnection(_connectionString))
                            {
                                connection3.Open();
                                SqlCommand cmd3 = new SqlCommand("USP_CurrentDigiPanCard", connection3);
                                cmd3.CommandType = CommandType.StoredProcedure;
                                cmd3.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                //cmd3.Parameters.AddWithValue("@CustomerId", 45);
                                cmd3.Parameters.AddWithValue("@PANNo", PANNo);
                                cmd3.Parameters.AddWithValue("@name", name);
                                cmd3.Parameters.AddWithValue("@firstname", fname);
                                cmd3.Parameters.AddWithValue("@middlename", mname);
                                cmd3.Parameters.AddWithValue("@lastname", lname);
                                cmd3.Parameters.AddWithValue("@dob", dob);
                                cmd3.Parameters.AddWithValue("@gender", gender);
                                cmd3.Parameters.AddWithValue("@country", country);
                                cmd3.Parameters.AddWithValue("@ORGname", ORGname);
                                cmd3.Parameters.AddWithValue("@PANverifiedOn", PANverifiedOn);
                                cmd3.Parameters.AddWithValue("@createdBy", UserId);
                                cmd3.ExecuteReader();

                                //    SqlDataReader reader3 = cmd3.ExecuteReader();
                                //    //if (reader3.Read())
                                //    //{
                                //    //    using (SqlConnection connection4 = new SqlConnection(conn))
                                //    //    {
                                //    //        SqlCommand cmd4 = new SqlCommand("USP_DigilockerPANFlag", connection4);
                                //    //        cmd4.CommandType = CommandType.StoredProcedure;
                                //    //        cmd4.Parameters.AddWithValue("@CustId", Convert.ToString(HttpContext.Session.GetString("PersonalId")));
                                //    //        connection4.Open();
                                //    //        SqlDataReader reader4 = cmd4.ExecuteReader();
                                //    //        if (reader4.Read())
                                //    //        {
                                //    //            //var resp1 = reader["FLAG"].ToString();
                                //    //        }
                                //    //    }
                                //    //}

                            }
                            HttpContext.Session.SetString("CheckPan", "PanTrue");
                            string panstr = TempData["FlagDoc"].ToString();
                            #region Pan
                            if (panstr == "PAN")
                            {
                                ClsDocDetails objFinalDoc = new ClsDocDetails();
                                byte[] ds = Convert.FromBase64String(pdfresponse);

                                objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                objFinalDoc.DocDetails = ds;
                                objFinalDoc.DocName = "PanDigi.pdf";
                                extension = objFinalDoc.DocName.Split('.').LastOrDefault();

                                objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType {objFinalDoc.DocType}");
                                objFinalDoc.DocMainType = "I";
                                objFinalDoc.documentCategory = "PanCard";
                                objFinalDoc.Source = "FromDigiLocker";
                                objFinalDoc.documentTypeId = "Pan Card";
                                objFinalDoc.DocCategoryCode = "2";
                                objFinalDoc.DocumentType = "POI";

                                objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                                ///*************///
                                string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan", objFinalDoc.DocName);

                                byte[] bytes = Convert.FromBase64String(pdfresponse);
                                System.IO.File.WriteAllBytesAsync(FilePath, bytes);
                                Document pdfDocument = new Document(FilePath);

                                foreach (var page in pdfDocument.Pages)
                                {
                                    // Define Resolution
                                    Resolution2 resolution = new Resolution2(1600);

                                    // Create Jpeg device with specified attributes
                                    // Width, Height, Resolution
                                    JpegDevice JpegDevice = new JpegDevice(600, 750, resolution);

                                    // Convert a particular page and save the image to stream
                                    JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan\\Pan.jpg"));
                                }

                                byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan\\Pan.jpg"));

                                //**************//

                                var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                IMapper mapper = config.CreateMapper();
                                ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc);

                                string conn3 = _connectionString;

                                using (SqlConnection con = new SqlConnection(conn3))
                                {
                                    con.Open();

                                    SqlCommand cmd1 = new SqlCommand("USP_CurrentAddDocuments", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                    cmd1.Parameters.Add(new SqlParameter("@docMainCategory", objFinalDoc.DocMainType));
                                    cmd1.Parameters.Add(new SqlParameter("@doctype", objFinalDoc.DocumentType));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                    cmd1.Parameters.Add(new SqlParameter("@source", objFinalDoc.Source));

                                    cmd1.ExecuteNonQuery();


                                }


                            }
                            else if (panstr == "Both")
                            {

                                ClsDocDetails objFinalDoc = new ClsDocDetails();
                                byte[] ds = Convert.FromBase64String(pdfresponse);

                                objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                objFinalDoc.DocDetails = ds;
                                objFinalDoc.DocName = "PanDigi.pdf";
                                extension = objFinalDoc.DocName.Split('.').LastOrDefault();

                                objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType {objFinalDoc.DocType}");
                                objFinalDoc.DocMainType = "I";
                                objFinalDoc.documentCategory = "PanCard";
                                objFinalDoc.Source = "FromDigiLocker";
                                objFinalDoc.DocumentType = "POI";
                                objFinalDoc.DocCategoryCode = "2";

                                objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                                string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan", objFinalDoc.DocName);

                                byte[] bytes = Convert.FromBase64String(pdfresponse);
                                System.IO.File.WriteAllBytesAsync(FilePath, bytes);
                                Document pdfDocument = new Document(FilePath);

                                foreach (var page in pdfDocument.Pages)
                                {
                                    // Define Resolution
                                    Resolution2 resolution = new Resolution2(1600);

                                    // Create Jpeg device with specified attributes
                                    // Width, Height, Resolution
                                    JpegDevice JpegDevice = new JpegDevice(600, 750, resolution);

                                    // Convert a particular page and save the image to stream
                                    JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan\\Pan.jpg"));
                                }

                                byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan\\Pan.jpg"));
                                var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                IMapper mapper = config.CreateMapper();
                                ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc);
                                string conn3 = _connectionString;

                                using (SqlConnection con = new SqlConnection(conn3))
                                {
                                    con.Open();
                                    SqlCommand cmd1 = new SqlCommand("USP_CurrentAddDocuments", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                    cmd1.Parameters.Add(new SqlParameter("@docMainCategory", objFinalDoc.DocMainType));
                                    cmd1.Parameters.Add(new SqlParameter("@doctype", objFinalDoc.DocumentType));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                    cmd1.Parameters.Add(new SqlParameter("@source", objFinalDoc.Source));
                                    cmd1.ExecuteNonQuery();

                                }
                            }
                            else if (panstr == "Bothwithxml")
                            {

                                ClsDocDetails objFinalDoc = new ClsDocDetails();
                                byte[] ds = Convert.FromBase64String(pdfresponse);

                                objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                objFinalDoc.DocDetails = ds;
                                objFinalDoc.DocName = "PanDigi.pdf";
                                extension = objFinalDoc.DocName.Split('.').LastOrDefault();

                                objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType {objFinalDoc.DocType}");
                                objFinalDoc.DocMainType = "I";
                                objFinalDoc.documentCategory = "PanCard";
                                objFinalDoc.Source = "FromDigiLocker";
                                objFinalDoc.DocCategoryCode = "2";
                                objFinalDoc.DocumentType = "POI";

                                objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan", objFinalDoc.DocName);

                                byte[] bytes = Convert.FromBase64String(pdfresponse);
                                System.IO.File.WriteAllBytesAsync(FilePath, bytes);
                                Document pdfDocument = new Document(FilePath);

                                foreach (var page in pdfDocument.Pages)
                                {
                                    // Define Resolution
                                    Resolution2 resolution = new Resolution2(1600);

                                    // Create Jpeg device with specified attributes
                                    // Width, Height, Resolution
                                    JpegDevice JpegDevice = new JpegDevice(600, 750, resolution);

                                    // Convert a particular page and save the image to stream
                                    JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan\\Pan.jpg"));
                                }

                                byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan\\Pan.jpg"));
                                string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                                var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                IMapper mapper = config.CreateMapper();
                                ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc);
                                string conn3 = _connectionString;

                                using (SqlConnection con = new SqlConnection(conn3))
                                {
                                    con.Open();
                                    SqlCommand cmd1 = new SqlCommand("USP_CurrentAddDocuments", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                    cmd1.Parameters.Add(new SqlParameter("@docMainCategory", objFinalDoc.DocMainType));
                                    cmd1.Parameters.Add(new SqlParameter("@doctype", objFinalDoc.DocumentType));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                    cmd1.Parameters.Add(new SqlParameter("@source", objFinalDoc.Source));
                                    cmd1.ExecuteNonQuery();

                                }
                            }

                            else if (panstr == "PANwithxml")
                            {
                                ClsDocDetails objFinalDoc = new ClsDocDetails();
                                byte[] ds = Convert.FromBase64String(pdfresponse);

                                objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                objFinalDoc.DocDetails = ds;
                                objFinalDoc.DocName = "PanDigi.pdf";
                                extension = objFinalDoc.DocName.Split('.').LastOrDefault();

                                objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType {objFinalDoc.DocType}");
                                objFinalDoc.DocMainType = "I";
                                objFinalDoc.documentCategory = "PanCard";
                                objFinalDoc.Source = "FromDigiLocker";
                                objFinalDoc.DocCategoryCode = "2";
                                objFinalDoc.DocumentType = "POI";

                                objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                                string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan", objFinalDoc.DocName);

                                byte[] bytes = Convert.FromBase64String(pdfresponse);
                                System.IO.File.WriteAllBytesAsync(FilePath, bytes);
                                Document pdfDocument = new Document(FilePath);

                                foreach (var page in pdfDocument.Pages)
                                {
                                    // Define Resolution
                                    Resolution2 resolution = new Resolution2(1600);

                                    // Create Jpeg device with specified attributes
                                    // Width, Height, Resolution
                                    JpegDevice JpegDevice = new JpegDevice(600, 750, resolution);

                                    // Convert a particular page and save the image to stream
                                    JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan\\Pan.jpg"));
                                }

                                byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiPan\\Pan.jpg"));
                                var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                IMapper mapper = config.CreateMapper();
                                ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc);
                                string conn3 = _connectionString;

                                using (SqlConnection con = new SqlConnection(conn3))
                                {
                                    //SqlDataReader reader = null;
                                    con.Open();

                                    SqlCommand cmd1 = new SqlCommand("USP_CurrentAddDocuments", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                    cmd1.Parameters.Add(new SqlParameter("@docMainCategory", objFinalDoc.DocMainType));
                                    cmd1.Parameters.Add(new SqlParameter("@doctype", objFinalDoc.DocumentType));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                    cmd1.Parameters.Add(new SqlParameter("@source", objFinalDoc.Source));
                                    cmd1.ExecuteNonQuery();


                                }
                            }

                            #endregion Pan

                        }
                        else
                        {
                            HttpContext.Session.SetString("CheckPan", "PanFalse");
                        }
                        return pdfresponse;
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                // PortalException.InsertPortalException(ex);

                var result1 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();

                if (result1.IsDigiPansumbitted == false)
                {
                    ViewBag.PanFalse = "PanFalse";
                    ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                }
                string ss = "pdf not generate";
                return ss;
            }
        }


        public string GetXml(string URI, string access_token, string id)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var clientxml = new RestClient("https://apigateway.indofinnet.com/api/GetXmlData?OrgID=IndoFin007&URI=" + URI + "&Token=" + access_token);
                clientxml.Timeout = -1;
                var requestxml = new RestRequest(Method.POST);
                requestxml.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse responsexml = clientxml.Execute(requestxml);
                dynamic? output2 = JsonConvert.DeserializeObject(responsexml.Content);
                XDocument KycData = XDocument.Parse(output2);
                string XMLdata = KycData.ToString();
                //string XMLdata = Encoding.UTF8.GetString(Convert.FromBase64String(xmlresp));
                HttpContext.Session.SetString("checkdatastatus", XMLdata);// = XMLdata;
                string e = "error";
                if (XMLdata.Contains(e))
                {
                    XMLdata = "FAIL";

                }
                return XMLdata;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return "FAIL";
            }
            return URI;
        }


        public string GetAadharXml(string access_token)
        {
            ErrorLog error_log = new ErrorLog();
            string xml = null;
            try
            {
                var clientP = new RestClient("https://apigateway.indofinnet.com/api/GetEAadhaarXmlData?OrgID=Alpha01" + "&Token=" + access_token);
                clientP.Timeout = -1;
                var requestP = new RestRequest(Method.POST);
                requestP.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse responseEA = clientP.Execute(requestP);

                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_DigilockerErrorlog", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt64((HttpContext.Session.GetString("PersonalId"))));
                    cmd.Parameters.AddWithValue("@Request", Convert.ToString(responseEA));

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var responce2 = reader["result"].ToString();
                        var value = responseEA.Content;
                        dynamic? output2 = JsonConvert.DeserializeObject(value);
                        XDocument aKYC = XDocument.Parse(output2);
                        xml = aKYC.ToString();
                        //string xmlbase64 = responseEA.Content.Trim('\"');
                        //xml = Encoding.UTF8.GetString(Convert.FromBase64String(xmlbase64));
                        HttpContext.Session.SetString("checkAadharstatus", xml);
                        using (SqlConnection connection4 = new SqlConnection(conn))
                        {
                            SqlCommand cmd4 = new SqlCommand("USP_UpdateDigilockrlog", connection4);
                            cmd4.CommandType = CommandType.StoredProcedure;
                            cmd4.Parameters.AddWithValue("@Identity", Convert.ToInt32(responce2));
                            cmd4.Parameters.AddWithValue("@Response", xml);
                            cmd4.Parameters.AddWithValue("@Status", Convert.ToString(responseEA));
                            cmd4.Parameters.AddWithValue("@Digilockertype", "Aadhar");

                            connection4.Open();
                            SqlDataReader reader4 = cmd4.ExecuteReader();
                            if (reader4.Read())
                            {
                                //var resp1 = reader["FLAG"].ToString();

                            }
                        }

                    }
                }

                XDocument KycData = XDocument.Parse(xml);
                string name = KycData.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("name") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("name").Value : "";
                string photo = KycData.Root.Element("CertificateData").Element("UidData").Element("Pht") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Pht").Value : "";
                string DOB = KycData.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("dob") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("dob").Value : "";
                string gender = KycData.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("gender") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("gender").Value : "";
                string Vtc = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("vtc") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("vtc").Value : "";

                string Street = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("street") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("street").Value : "";
                string State = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("state") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("state").Value : "";

                string Pc = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("pc") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("pc").Value : "";
                string Locality = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("loc") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("loc").Value : "";
                string House = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("house") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("house").Value : "";
                string district = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("dist") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("dist").Value : "";
                string country = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("country") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("country").Value : "";
                string lm = KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("lm") != null ? KycData.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("lm").Value : "";

                string uid = KycData.Root.Element("CertificateData").Element("UidData").Attribute("uid") != null ? KycData.Root.Element("CertificateData").Element("UidData").Attribute("uid").Value : "";
                byte[] AadharPhoto = Encoding.ASCII.GetBytes(photo);
                //HttpContext.Session.SetString("AadharPhoto", photo);
                byte[] AadharPhoto1 = System.Convert.FromBase64String(photo);
                HttpContext.Session.SetString("AadharPhoto", photo);
                string Address = lm + "" + Locality;
                string[] Name = (name).Split(' ');
                string fname = Name[0];
                string mname = Name[1];
                string lname = Name[2];
                // For passing data to view
                TempData["AFName"] = fname;
                TempData["AMName"] = mname;
                TempData["ALName"] = lname;
                TempData["APhoto"] = photo;
                TempData["ADOB"] = DOB;
                TempData["AGender"] = gender;
                TempData["ACity"] = Vtc;
                TempData["AStreet"] = Street;
                TempData["AState"] = State;
                TempData["APincode"] = Pc;
                TempData["ALocality"] = Locality;
                TempData["AHouse"] = House;
                TempData["ADistrict"] = district;
                TempData["ACountry"] = country;
                TempData["AdhaarNo"] = uid;






                string UserId = Convert.ToString(HttpContext.Session.GetString("UseID"));
                using (SqlConnection connection3 = new SqlConnection(conn))
                {
                    connection3.Open();
                    SqlCommand cmd3 = new SqlCommand("USP_CurrentDigiAadharxml", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                    cmd3.Parameters.AddWithValue("@name", name);
                    cmd3.Parameters.AddWithValue("@photo", AadharPhoto1);
                    cmd3.Parameters.AddWithValue("@DOB", DOB);
                    cmd3.Parameters.AddWithValue("@gender", gender);

                    cmd3.Parameters.AddWithValue("@Vtc", Vtc);
                    cmd3.Parameters.AddWithValue("@Street", Street);
                    cmd3.Parameters.AddWithValue("@State", State);
                    cmd3.Parameters.AddWithValue("@Pc", Pc);
                    cmd3.Parameters.AddWithValue("@Locality", Locality);
                    cmd3.Parameters.AddWithValue("@House", House);
                    cmd3.Parameters.AddWithValue("@district", district);
                    cmd3.Parameters.AddWithValue("@country", country);
                    cmd3.Parameters.AddWithValue("@uid", uid);
                    cmd3.Parameters.AddWithValue("@firstname", fname);
                    cmd3.Parameters.AddWithValue("@middlename", mname);
                    cmd3.Parameters.AddWithValue("@lastname", lname);
                    cmd3.Parameters.AddWithValue("@Address", Address);
                    cmd3.ExecuteReader();
                    //cmd3.Parameters.AddWithValue("@createdBy", UserId);
                    //connection3.Open();
                    //SqlDataReader reader3 = cmd3.ExecuteReader();
                    //if (reader3.Read())
                    //{
                    //    // var  resp = reader["FLAG"].ToString();
                    //    using (SqlConnection connection4 = new SqlConnection(conn))
                    //    {
                    //        SqlCommand cmd4 = new SqlCommand("USP_DigilockerAdharFlag", connection4);
                    //        cmd4.CommandType = CommandType.StoredProcedure;
                    //        cmd4.Parameters.AddWithValue("@CustId", Convert.ToString(HttpContext.Session.GetString("PersonalId")));


                    //        connection4.Open();
                    //        SqlDataReader reader4 = cmd4.ExecuteReader();
                    //        if (reader4.Read())
                    //        {
                    //            //var resp1 = reader["FLAG"].ToString();

                    //        }
                    //    }
                    //}
                }
                HttpContext.Session.SetString("CheckAadhar", "AadharTrue");// = "AadharTrue";
                CurrentDigiAadharpdf();
                return xml;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                var result1 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {Convert.ToInt64(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();

                if (result1.IsDigiAadharSumbitted == false)
                {

                    ViewBag.AadharFalse = "AadharFalse";
                    HttpContext.Session.SetString("CheckAadhar", "AadharFalse");// = ViewBag.AadharFalse;
                }
                return xml;

            }
            return xml;

        }

        public async Task<ActionResult> CurrentDigiAadharpdf()
        {
            var ABC = HttpContext.Session.GetString("PersonalId");
            ErrorLog error_log = new ErrorLog();
            try
            {
                long personlids = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

                var CustDatadetails = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_CurrentGetDigiAadharxml {personlids}").AsEnumerable().FirstOrDefault();
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                clsCustAadharDataA objCustData = new clsCustAadharDataA();
                objCustData.FullName = CustDatadetails.Firstname;
                string[] Date = CustDatadetails.Dob.Split('-');
                objCustData.Day = Date[0];
                objCustData.Month = Date[1];
                objCustData.Year = Date[2];
                objCustData.gender = CustDatadetails.Gender;
                objCustData.address = CustDatadetails.Address;
                objCustData.Country = CustDatadetails.Country;
                objCustData.Locality = CustDatadetails.Locality;
                objCustData.state = CustDatadetails.State;
                objCustData.House = CustDatadetails.House;
                objCustData.AadharNumber = CustDatadetails.Uid;
                objCustData.City = CustDatadetails.Vtc;
                objCustData.Pincode = CustDatadetails.Pc;
                objCustData.Photo = Convert.ToString(HttpContext.Session.GetString("AadharPhoto"));
                objCustData.street = CustDatadetails.Street;

                objCustData.District = CustDatadetails.District;
                objCustData.QuickEnrollNumber = Convert.ToString(HttpContext.Session.GetString("QEPersonalId"));
                var pdf = new Rotativa.AspNetCore.ViewAsPdf("DigilockerAadharpdf", objCustData)
                {
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                };
                byte[] ds = await pdf.BuildFile(ControllerContext);
                string stringpdf = Convert.ToBase64String(ds);

                string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                string fileName = "doc.pdf";

                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\PDF", fileName);

                byte[] bytes = Convert.FromBase64String(stringpdf);
                await System.IO.File.WriteAllBytesAsync(FilePath, bytes);

                ClsDocDetails objFinalDoc = new ClsDocDetails();

                Document pdfDocument = new Document(FilePath);



                foreach (var page in pdfDocument.Pages)
                {
                    // Define Resolution
                    Resolution2 resolution = new Resolution2(1600);

                    // Create Jpeg device with specified attributes
                    // Width, Height, Resolution
                    JpegDevice JpegDevice = new JpegDevice(600, 750, resolution);

                    // Convert a particular page and save the image to stream
                    JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\PDF\\Output.jpg"));
                }

                byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\PDF\\Output.jpg"));

                objFinalDoc.CustomerDetailId = (Convert.ToInt64(ABC));
                objFinalDoc.DocDetails = imgData;
                objFinalDoc.DocName = "Aadharxml.jpg"; //(Convert.ToString(ABC));
                extension = objFinalDoc.DocName.Split('.').LastOrDefault();

                objFinalDoc.DocMainType = "CA";
                //objFinalDoc.documentCategory = "1";
                objFinalDoc.Source = "FromDigiLocker";
                objFinalDoc.DocCategoryCode = "1";
                objFinalDoc.documentTypeId = "Aadharxml";
                objFinalDoc.documentCategory = "AadharCard";
                objFinalDoc.DocumentType = "POA";
                objFinalDoc.DocumentId = (Convert.ToString(ABC));
                string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);


                string conn3 = _connectionString;

                using (SqlConnection con = new SqlConnection(conn3))
                {
                    SqlDataReader reader = null;
                    con.Open();

                    SqlCommand cmd1 = new SqlCommand("USP_CurrentAddDocuments", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                    cmd1.Parameters.Add(new SqlParameter("@docMainCategory", objFinalDoc.DocMainType));
                    cmd1.Parameters.Add(new SqlParameter("@doctype", objFinalDoc.DocumentType));
                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                    cmd1.Parameters.Add(new SqlParameter("@source", objFinalDoc.Source));
                    cmd1.ExecuteNonQuery();
                }

                return pdf;
            }
            catch (Exception ex)
            {
                await error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }

        }





    }
}
