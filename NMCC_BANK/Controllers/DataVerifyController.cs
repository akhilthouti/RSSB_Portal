using AutoMapper;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using ServiceProvider1.Models.OrgExceptionLogs;
using ServiceProvider1.Models.UserDetails;
using System;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceProvider1.Models;
using INDO_FIN_NET.Models.QuickEnroll;
using OfflineKYC;
using Grpc.Core;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Models.QuickEnroll;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nancy.Json;
using Newtonsoft.Json;
using OfflineKYC;
using Org.BouncyCastle.Crypto.Tls;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Ionic.Zip;
using AutoMapper;
using RestSharp;
using System.Net.Security;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
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
using Document = iTextSharp.text.Document;
using System.Text.RegularExpressions;
using tessnet2;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace INDO_FIN_NET.Controllers
{
    public class DataVerifyController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        private object isInserted;
        private readonly string _connectionString;

        public DataVerifyController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        string extension = null;
        public long? isInserted1 = null;
        public long? isInserted3 = null;
        string UserId = null;
        string VendorPassword = "iC2pOBYQi6w5hK38DWDyPw==";
        string ts = DateTime.Now.ToString("dd-MM-yy  hh-mm-ss");
        static BetterWebClient client = new BetterWebClient();
        clsEncryptAndDecrypt objData1 = new clsEncryptAndDecrypt();
        ClsDigitalKYC1 objDigitalKYCdata = new ClsDigitalKYC1();
        TripleDESImplementation objtriple = new TripleDESImplementation();
        private readonly object city;
        string imgtypePhoto = "";
        string imgtype_POI = "";
        string imgtype_CA = "";
        byte[] dochistory_Photo = null;
        byte[] dochistory_POI = null;
        byte[] dochistory_CA = null;
        byte[] byteDoc = null;
        byte[] byteimg;
        public ActionResult SummerySheet()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    if (!clsCustAuthorize.IsAuthCustomerDetails(Convert.ToInt64(ObjTripleDes.Decrypt(HttpContext.Session.GetString("EncryPersonalId").ToString())), HttpContext.Session.GetString("SessionOrgKey").ToString()))
                    {
                        return RedirectToAction("CustomerRegistration", "SignIn");
                    }
                }
                long CustomerId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                ViewBag.msg = TempData["msg"];
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        [HttpGet]
        public ActionResult SummarySheetData()
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
        public ActionResult SummerySheetDetails([FromServices] IActiveLogin objLogin, ClsDocumentDetails objDocResult)

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
            try
            {
                ClsSummeryDetails objSummery = new ClsSummeryDetails();

                var jointData = HttpContext.Session.GetString("JointACCdata");
                if (jointData == "true")
                {
                    ViewBag.jointChanges = "true";
                }
                var rekycview = HttpContext.Session.GetString("rekycVIEW");
                if (rekycview != null)
                {
                    ViewBag.rekycview = rekycview;
                }
                var progressbar = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                if (progressbar.IsQuickEnrollSubmit == true && progressbar.IsCAFSubmit == true && progressbar.IsDocumentSubmit == true)
                {
                    ViewBag.progressbarqcdv = 1;
                }
                else { }

                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    ViewBag.AdminFlag = "AdminFlag";
                    string AdminId = Convert.ToString(HttpContext.Session.GetString("UserID"));
                    HttpContext.Session.SetString("OrgUserId", AdminId);
                }

                long CustomerId = Convert.ToInt64(HttpContext.Session.GetString(("PersonalId")));
                if (HttpContext.Session.GetString("RekycImg") != "true")
                {
                    ViewBag.liveimage = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("liveimage")));

                }
                var returnres = objDetails.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsById {CustomerId}").AsEnumerable().FirstOrDefault();
                if (returnres != null)                      ///for Agent
                {
                    string UserId = Convert.ToString(HttpContext.Session.GetString("UseID"));
                    string UserId1 = Convert.ToString(HttpContext.Session.GetString("UserID"));
                    if (UserId != null)
                    {

                        var userresult = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Get_AdminStatusById {UserId}").AsEnumerable().FirstOrDefault();

                        if (userresult.MobileNo.Length == 12)
                        {
                            var mm = userresult.MobileNo.Substring(2);
                            objDigitalKYCdata.MobileNo = mm;
                        }
                        else
                        {
                            objSummery.User_MobileNo = userresult.MobileNo;
                        }
                        objSummery.UserName = userresult.UserName;
                        ViewBag.AgentAndCustPDFDown = true;
                    }
                    else if (CustomerId != null)
                    {
                        var Custresult = objDetails.TblCustomerDetails.FromSqlRaw($"USP_GetCustomerDetailsbyId {CustomerId}").AsEnumerable().FirstOrDefault();

                        if (Custresult.CustMobileNo.Length == 12)
                        {
                            var mm = Custresult.CustMobileNo.Substring(2);
                            objDigitalKYCdata.MobileNo = mm;
                        }
                        else
                        {
                            objSummery.User_MobileNo = Custresult.CustMobileNo;
                        }
                        objSummery.UserName = Custresult.CustFirstName;
                        ViewBag.CustPDFDown = true;
                    }
                    else
                    {
                        var userresult = objDetails.IndoAdminDetails.FromSqlRaw($"USP_Get_AdminStatusById {UserId1}").AsEnumerable().FirstOrDefault();
                        objSummery.User_MobileNo = userresult.MobileNo;
                        objSummery.UserName = userresult.UserName;
                        ViewBag.AgentAndCustPDFDown = true;

                    }
                }
                else                                                                         //for Customer 
                {
                    string UserId = Convert.ToString(HttpContext.Session.GetString("UseID"));
                    ViewBag.AgentAndCustPDFDown = false;
                }
                var resul1 = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetCustomerDetails1 {CustomerId}").AsEnumerable().FirstOrDefault();
                var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerDetails {CustomerId}").AsEnumerable().FirstOrDefault();

                objSummery.CustomerId = Convert.ToString(CustomerId);
                objSummery.CFirstName = result.FirstName;

                objSummery.CMiddleName = result.MiddleName;
                objSummery.CLastName = result.LastName;
                objSummery.CustomerName = result.FirstName + " " + result.MiddleName + " " + result.LastName;
                objSummery.CGender = result.Gender;
                if (result.MobileNo != null)
                {

                    if (result.MobileNo.Length == 12)
                    {
                        var mm = result.MobileNo.Substring(2);
                        objSummery.CMobileNo = mm;
                    }
                    else
                    {
                        objSummery.CMobileNo = result.MobileNo;
                    }

                }
                else
                {
                    objSummery.CMobileNo = result.MobileNo;
                }
                objSummery.CEmailId = result.EmailId;
                objSummery.ManualPanNo = ObjTripleDes.Decrypt(result.PanNo);
                objSummery.Pin_Code = result.PinCode;
                objSummery.CAddress = result.ClientAddress1;
                ViewBag.Rekyc = HttpContext.Session.GetString("RekycImg");

                if (HttpContext.Session.GetString("RekycImg") == "true")
                {
                    string conn1 = _connectionString;
                    using (SqlConnection connection3 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd3 = new SqlCommand("USP_RKYCSummarySubmitFlag", connection3);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        connection3.Open();
                        SqlDataReader reader3 = cmd3.ExecuteReader();
                        if (reader3.Read())
                        {
                            //var Result = reader2["RESULT"].ToString();
                        }
                    }
                    long CustId = Convert.ToInt64(HttpContext.Session.GetString(("PersonalId")));
                    var result4 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_RekycAimage {CustId}").AsEnumerable().FirstOrDefault();
                    var result12 = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_RekycAxmlimage {CustId}").AsEnumerable().FirstOrDefault();

                    if (result4 != null)
                    {
                        string imgs1 = Convert.ToBase64String(result4.Photo);
                        ViewBag.liveimage = (string.Format("data:image/jpg;base64,{0}", imgs1));
                    }
                    else if (result12 != null)
                    {
                        string imgs2 = (result12.AadharPhoto);
                        ViewBag.liveimage = imgs2;// (string.Format("data:image/jpg;base64,{0}", imgs2));

                        ViewBag.rekycrekyc = "DocumentFlag";

                    }
                }
                if (resul1 != null)
                {

                    string imgs = Convert.ToBase64String(resul1.DocumentHistory);
                    ViewBag.liveimage = (string.Format("data:image/jpg;base64,{0}", imgs));

                    objSummery.LivePhoto = imgs;
                }

                string conn = _connectionString;

                using (SqlConnection connection3 = new SqlConnection(conn))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_AdminRKYCDetails", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@CustId", CustomerId);
                    connection3.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {
                        var Result = reader3[56].ToString();
                        HttpContext.Session.SetString("rekycforSS", Result);
                    }
                    var Rekyc = HttpContext.Session.GetString("rekycforSS");


                    if (Rekyc == "True")
                    {

                        var result4 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_RekycAimage {CustomerId}").AsEnumerable().FirstOrDefault();
                        var result12 = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_RekycAxmlimage {CustomerId}").AsEnumerable().FirstOrDefault();

                        if (result4 != null)
                        {
                            string imgs1 = Convert.ToBase64String(result4.Photo);
                            ViewBag.liveimage = (string.Format("data:image/jpg;base64,{0}", imgs1));
                            ViewBag.rekycrekyc = "DocumentFlag";
                        }
                        else if (result12 != null)
                        {
                            string imgs2 = (result12.AadharPhoto);
                            ViewBag.liveimage = imgs2;// (string.Format("data:image/jpg;base64,{0}", imgs2));
                            ViewBag.rekycrekyc = "DocumentFlag";

                        }
                        var adminId = HttpContext.Session.GetString("UserID");
                        if (adminId != null)
                        {
                            ViewBag.AdminFlag = "AdminFlag";
                        }
                    }
                }

                if (result.CountryId != null)
                {
                    objSummery.QECountry = result.CountryId;
                    string Ccode = result.CountryId;
                    string conn1 = _connectionString;
                    using (SqlConnection connection12 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection12);
                        cmd12.CommandType = CommandType.StoredProcedure;

                        cmd12.Parameters.AddWithValue("@CountryCode", Ccode);
                        connection12.Open();
                        SqlDataReader reader1 = cmd12.ExecuteReader();
                        if (reader1.Read())
                        {


                            var Country = reader1[2].ToString();
                            objSummery.QECountry = Country;
                        }
                    }
                }

                if (result.StateId != null)
                {
                    string Scode = result.ClientPermState;
                    string conn1 = _connectionString;
                    using (SqlConnection connection12 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection12);
                        cmd12.CommandType = CommandType.StoredProcedure;

                        cmd12.Parameters.AddWithValue("@State_Code", Scode);
                        connection12.Open();
                        SqlDataReader reader1 = cmd12.ExecuteReader();
                        if (reader1.Read())
                        {


                            var state = reader1[2].ToString();
                            objSummery.QEState = state;
                        }
                    }
                }

                objSummery.QECity = result.CityId;

                objSummery.DOB = result.Dob;
                if (objSummery.DOB.Contains('-'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('-');

                    objSummery.DOB_DD = DateOfBirth[0];
                    objSummery.DOB_MM = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];

                }
                else if (objSummery.DOB.Contains('/'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('/');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];

                }

                if (result.LivePhoto != null)
                {
                    objSummery.LivePhoto = Convert.ToBase64String(result.LivePhoto);
                }

                string conn4 = _connectionString;
                using (SqlConnection connection1 = new SqlConnection(conn4))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_GetDocumentById", connection1);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("CustomerId")));

                    connection1.Open();

                    int response = cmd3.ExecuteNonQuery();
                    connection1.Close();

                    var result1 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {CustomerId}").AsEnumerable().FirstOrDefault();
                    if (result1 != null)
                    {

                        if (result1.IsAadharVerify == true)
                        {
                            objSummery.IsAadharVerify = result1.IsAadharVerify;
                            if ((HttpContext.Session.GetString("PersonalId") != null))
                            {
                                string conn1 = _connectionString;
                                using (SqlConnection connection2 = new SqlConnection(conn1))
                                {
                                    SqlCommand cmd4 = new SqlCommand("USP_GetCustomerAdharDetailsByCustId", connection2);
                                    cmd4.CommandType = CommandType.StoredProcedure;
                                    cmd4.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("CustomerId")));

                                    connection2.Open();

                                    int resp = cmd4.ExecuteNonQuery();
                                    //connection2.Close();

                                    var AadharData = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetCustomerAdharDetailsByCustId {Convert.ToInt64(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();
                                    if (AadharData != null)
                                    {
                                        objSummery.CustomerId = Convert.ToString(CustomerId);
                                        objSummery.AadharName = AadharData.AadharName;
                                        objSummery.Aadhar_Gender = AadharData.AadharGender;
                                        objSummery.Aadhar_DateOfBirth = AadharData.AadharDob;
                                        objSummery.Aadhar_Address = AadharData.AadharAddress;
                                        objSummery.Aadhar_Pincode = AadharData.PinCode;
                                        objSummery.Aadhar_Locality = AadharData.Locality;
                                        objSummery.AadharState = AadharData.State;
                                        objSummery.AadharHouse = AadharData.House;
                                        objSummery.AadharStreet = AadharData.Street;
                                        objSummery.Aadhar_District = AadharData.District;
                                        objSummery.AadharCountry = AadharData.Country;
                                        if (AadharData.AadharPhoto != null)
                                        {
                                            objSummery.AadharPhoto = AadharData.AadharPhoto;
                                        }
                                    }
                                }
                            }
                        }
                        return View(objSummery);
                    }
                    return View(objDocResult);
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult SummerySheetDetailsFORDesign()

        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    HttpContext.Session.GetString("PersonalId");
                    HttpContext.Session.GetString("DAEditCustomerdetailId");
                    ViewBag.AdminFlag = "AdminFlag";
                }
                long CustomerId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string UserId = "1221";
                ClsSummeryDetails objSummery = new ClsSummeryDetails();
                var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerDetails {CustomerId},{""}").AsEnumerable().FirstOrDefault();
                var userresult = objDetails.IndoAdminDetails.FromSqlRaw($"USP_GetIndo_UserStatusById {UserId}").AsEnumerable().FirstOrDefault();
                objSummery.User_MobileNo = userresult.MobileNo;
                objSummery.UserName = userresult.UserName;
                HttpContext.Session.SetString("CustId", "CustomerId");
                objSummery.CustomerId = Convert.ToString(CustomerId);
                objSummery.CFirstName = result.FirstName;
                objSummery.CMiddleName = result.MiddleName;
                objSummery.CLastName = result.LastName;
                objSummery.CustomerName = result.FirstName + " " + result.MiddleName + " " + result.LastName;
                objSummery.CGender = result.Gender;
                
                if (result.MobileNo.Length == 12)
                {
                    var mm = userresult.MobileNo.Substring(2);
                    objSummery.CMobileNo = mm;
                }
                else
                {
                    objSummery.CMobileNo = result.MobileNo;
                }
                objSummery.CEmailId = result.EmailId;
                objSummery.ManualPanNo = result.PanNo;
                objSummery.Pin_Code = result.PinCode;
                if (result.CountryId != null)
                {
                    string Ccode = result.CountryId;
                    var ob1 = objDetails.MasterCountries.FromSqlRaw($"USP_GetCountryId {Ccode}").AsEnumerable().FirstOrDefault();
                }

                if (result.StateId != null)
                {
                    string Scode = result.StateId;
                    var ob1 = objDetails.StateCodes.FromSqlRaw($"USP_GetStateName {Scode}").AsEnumerable().FirstOrDefault();
                }

                objSummery.QECity = result.CityId;

                objSummery.DOB = result.Dob;
                if (objSummery.DOB.Contains('-'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('-');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];
                }
                else if (objSummery.DOB.Contains('/'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('/');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];
                }
                if (result.LivePhoto != null)
                {
                    objSummery.LivePhoto = Convert.ToBase64String(result.LivePhoto);
                }
                var objDocResult = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetDocumentById {CustomerId}").AsEnumerable().ToList();

                foreach (var listDoc in objDocResult)
                {
                    switch (listDoc.DocMainCategory)
                    {
                        case "P":
                            var base64Photo = Convert.ToBase64String(listDoc.DocumentHistory);
                            imgtypePhoto = listDoc.DocumentName.Split('.').LastOrDefault();
                            objSummery.imgtypePhoto = imgtypePhoto;
                            objSummery.PhotoDocument = String.Format("data:image/" + imgtypePhoto + ";base64,{0}", base64Photo);
                            objSummery.Latitude_Longitude = listDoc.LatitudeLongitude;

                            break;
                        case "I":
                            dochistory_POI = listDoc.DocumentHistory;
                            ViewBag.Cust_POI = listDoc.CustomerDocumentId;
                            var base64_POI = Convert.ToBase64String(dochistory_POI);
                            imgtype_POI = listDoc.DocumentName.Split('.').LastOrDefault();
                            objSummery.POI_Document_Name = listDoc.DocumentName.Split('.').FirstOrDefault();
                            objSummery.imgtypePOI = imgtype_POI;
                            objSummery.POI_Document = base64_POI;
                            break;
                        case "CA":
                            dochistory_CA = listDoc.DocumentHistory;
                            var base64_CA = Convert.ToBase64String(listDoc.DocumentHistory);
                            imgtype_CA = listDoc.DocumentName.Split('.').LastOrDefault();
                            objSummery.POA_Document_Name = listDoc.DocumentName.Split('.').FirstOrDefault();
                            objSummery.imgtypeCA = imgtype_CA;
                            objSummery.CA_Document = base64_CA;
                            break;
                    }
                }


                objSummery.CAddress = result.ClientAddress1;
                var result1 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {Convert.ToInt64(HttpContext.Session.GetString("CustomerId"))}").AsEnumerable().FirstOrDefault();
                if (result1 != null)
                {

                    if (result1.IsAadharVerify == true)
                    {
                        objSummery.IsAadharVerify = result1.IsAadharVerify;
                        var AadharData = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetCustomerAdharDetails {result.ReferenceNumber}").AsEnumerable().FirstOrDefault();
                        if (AadharData != null)
                        {
                            objSummery.AadharName = AadharData.AadharName;
                            objSummery.Aadhar_DateOfBirth = AadharData.AadharDob;
                            objSummery.Aadhar_Gender = AadharData.AadharGender;
                            objSummery.Aadhar_Address = AadharData.AadharAddress;
                            objSummery.AadharCountry = AadharData.Country;
                            objSummery.Aadhar_Locality = AadharData.Locality;
                            objSummery.AadharState = AadharData.State;
                            objSummery.AadharHouse = AadharData.House;
                            objSummery.AadharStreet = AadharData.Street;
                            objSummery.Aadhar_District = AadharData.District;
                            objSummery.Aadhar_Pincode = AadharData.PinCode;
                            if (AadharData.AadharPhoto != null)
                            {
                                objSummery.AadharPhoto = AadharData.AadharPhoto;
                            }
                        }
                    }

                }
                return View(objSummery);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        [HttpPost]
        public ActionResult SummerySheetDetails(ClsSummeryDetails obj)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                HttpContext.Session.SetString("AccountForm", "Account");
                return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }       
        [AllowAnonymous]
        [HttpGet]
        [Obsolete]
        public async void Generatepdf()
        {
            ClsCustQuickEnrollment dataobj = new ClsCustQuickEnrollment();
            ClsDocumentDetails docobj = new ClsDocumentDetails();

            forpdfeneration genpdf = new forpdfeneration();
            string IsPanVerifyData = HttpContext.Session.GetString("VerifyPan");



            long CustomerId = Convert.ToInt64(HttpContext.Session.GetString(("PersonalId")));
            ViewBag.refId = CustomerId.ToString();

            //-------Aadhaar Data-----
            var aadhaarresult1 = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetPanDetails {CustomerId}").AsEnumerable().FirstOrDefault();
            var aadhaarresult = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetAadharDetails {CustomerId}").AsEnumerable().FirstOrDefault();
            var aadhaarXML = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharXMLDetails {CustomerId}").AsEnumerable().FirstOrDefault();
            var xysss = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigipanDetails {CustomerId}").AsEnumerable().FirstOrDefault();
            var DL = objDetails.AdmDigiDrivingLicences.FromSqlRaw($"USP_GetDigiDLDetails {CustomerId}").AsEnumerable().FirstOrDefault();
            if (aadhaarresult != null && xysss != null && aadhaarresult1 != null)
            {
                genpdf.digia = "digia";
                genpdf.Aname = aadhaarresult.Name;
                genpdf.Aaddress = aadhaarresult.House + aadhaarresult.Street + aadhaarresult.Address;
                genpdf.Apincode = aadhaarresult.Pc;
                genpdf.Aadhaar = aadhaarresult.Uid;
                genpdf.Adob = aadhaarresult.Dob;
                genpdf.Agender = aadhaarresult.Gender;
                genpdf.Adistrict = aadhaarresult.District;
                genpdf.Astate = aadhaarresult.State;
                genpdf.Acountry = aadhaarresult.Country;
                string adrpht = Convert.ToString(HttpContext.Session.GetString("AadharPhoto"));
                genpdf.AadhaarPhoto = (string.Format("data:image/jpg;base64,{0}", adrpht));
            }
            else if (aadhaarresult1 != null)
            {
                genpdf.Aname = aadhaarresult1.Firstname + "_" + aadhaarresult1.Middlename + "_" + aadhaarresult1.Lastname;
                genpdf.pan = "pan";
                if (aadhaarresult1.title == "Shri")
                {
                    genpdf.Agender = "MALE";
                }
                else
                {
                    genpdf.Agender = "FEMALE";
                }
            }
            else if (aadhaarresult != null)
            {
                genpdf.digia = "digia";
                genpdf.Aname = aadhaarresult.Name;
                genpdf.Aaddress = aadhaarresult.House + aadhaarresult.Street + aadhaarresult.Address;
                genpdf.Apincode = aadhaarresult.Pc;
                genpdf.Aadhaar = aadhaarresult.Uid;
                genpdf.Adob = aadhaarresult.Dob;
                genpdf.Agender = aadhaarresult.Gender;
                genpdf.Adistrict = aadhaarresult.District;
                genpdf.Astate = aadhaarresult.State;
                genpdf.Acountry = aadhaarresult.Country;
                string adrpht = Convert.ToString(HttpContext.Session.GetString("AadharPhoto"));
                genpdf.AadhaarPhoto = (string.Format("data:image/jpg;base64,{0}", adrpht));
            }
            else if (aadhaarXML != null)
            {
                genpdf.xml = "xml";
                genpdf.Aname = aadhaarXML.AadharName;
                genpdf.Aaddress = aadhaarXML.AadharAddress;
                genpdf.Apincode = aadhaarXML.PinCode;
                genpdf.Aadhaar = aadhaarXML.AadharNumber;
                genpdf.Adob = aadhaarXML.AadharDob;
                genpdf.Agender = aadhaarXML.AadharGender;
                genpdf.Adistrict = aadhaarXML.District;
                genpdf.Astate = aadhaarXML.State;
                genpdf.Acountry = aadhaarXML.Country;
                genpdf.AadhaarPhoto = aadhaarXML.AadharPhoto;
            }

            else if (xysss != null)
            {
                genpdf.digip = "digip";
                genpdf.Aname = xysss.Name;

                genpdf.Adob = xysss.Dob;
                genpdf.Agender = xysss.Gender;
                if (xysss.Country == "IN")
                {
                    genpdf.Acountry = "INDIA";
                }
                else
                {
                    genpdf.Acountry = "INDIA";
                }
            }

            else if (DL != null)
            {
                genpdf.DL = "DL";
                genpdf.Aname = DL.Name;
                genpdf.Aaddress = DL.Address;
                genpdf.Agender = DL.Gender;
                genpdf.Adob = DL.Dob;
                if (DL.Country == "IN")
                {
                    genpdf.Acountry = "INDIA";
                }
                else
                {
                    genpdf.Acountry = "INDIA";
                }
                genpdf.AadhaarPhoto = (string.Format("data:image/jpg;base64,{0}", DL.Photo));
            }

            else
            {

            }



            //-------for Docs--------
            string[] val = { "IAPVD", "CADL", "SI" };
            foreach (var dmt in val)
            {
                var Res = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(CustomerId)},{dmt}").AsEnumerable().FirstOrDefault();
                string b64 = Convert.ToBase64String(Res.DocumentHistory);
                if (dmt == "IAPVD")
                {
                    genpdf.POI = (string.Format("data:image/jpg;base64,{0}", b64));
                }
                else if (dmt == "CADL")
                {
                    genpdf.POA = (string.Format("data:image/jpg;base64,{0}", b64));
                }
                else if (dmt == "SI")
                {
                    genpdf.Signature = (string.Format("data:image/jpg;base64,{0}", b64));
                }
            }

            //-------for Cust data------
            var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerDetails {CustomerId}").AsEnumerable().FirstOrDefault();

            string custidd = Convert.ToString(CustomerId);

            genpdf.CDmobile = result.MobileNo;
            genpdf.CDfirstname = result.FirstName;
            ViewBag.firstname = result.FirstName;
            genpdf.CDemail = result.EmailId;
            genpdf.CDmiddlename = result.MiddleName;
            genpdf.CDlastname = result.LastName;
            genpdf.CDgender = result.Gender;
            genpdf.CDpincode = result.PinCode;
            genpdf.CDPan = ObjTripleDes.Decrypt(result.PanNo);
            genpdf.CDaddress = result.ClientAddress1;
            genpdf.CDdob = result.Dob;
            genpdf.LivePhoto = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("liveimage")));

            if (result.LivePhoto != null)
            {
                ViewBag.livephoto = (string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(result.LivePhoto)));
            }

            string conn = _connectionString;

            using (SqlConnection connection3 = new SqlConnection(conn))
            {
                SqlCommand cmd3 = new SqlCommand("summarysheetFlag", connection3);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                connection3.Open();
                SqlDataReader reader3 = cmd3.ExecuteReader();
                if (reader3.Read())
                {
                }
            }

            //=================================

            var pdf = new Rotativa.AspNetCore.ViewAsPdf("Generatepdf", genpdf)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };
            byte[] b = await pdf.BuildFile(ControllerContext);
            string stringpdf = Convert.ToBase64String(b);
            string refId = ViewBag.refId;// "a12";
            string Cname = ViewBag.firstname;//= result.FirstName;
            NameValueCollection collections = new NameValueCollection();
            collections.Add("custname", Cname);
            collections.Add("APIKey", "");
            //collections.Add("OrgID", "Indofin01");//Local
            collections.Add("OrgID", "RSSBUAT01");//Publish
            collections.Add("Doc", stringpdf);


            string remoteUrl = "https://uatesignapi.indofinnet.com/api/AadharEsign/AadhaarEsign?refID=" + refId;

            string html = "<html><head>";
            html += "</head><body onload='document.forms[0].submit()'>";
            html += string.Format("<form name='PostForm' method='POST' action='{0}'>", remoteUrl, refId);
            foreach (string key in collections.Keys)
            {
                html += string.Format("<input name='{0}' type='hidden' value='{1}'>", key, collections[key]);
            }
            html += "</form></body></html>";

            byte[] htmldata1 = System.Text.Encoding.UTF8.GetBytes(html);

            Response.Headers.ContentEncoding = "ISO-8859-1";
            Response.Headers.AcceptCharset = "ISO-8859-1";
            Response.Body.WriteAsync(htmldata1);
            Response.Body.Close();

        }
        [HttpPost]
        public ActionResult eSignResponce(String refID)
        {

            long s = Convert.ToInt64(refID);
            string doc = Request.Form["resp"].ToString();

            byte[] data = Convert.FromBase64String(doc);
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("USP_AddEsignpdf", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustId", s);
                cmd.Parameters.AddWithValue("@data", data);
                cmd.ExecuteNonQuery();

                string conn = _connectionString;

                using (SqlConnection connection3 = new SqlConnection(conn))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_EsignFlag", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@CustId", s);// Convert.ToInt64(HttpContext.Session.GetString("Ecid")));
                    connection3.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {
                        //var Result = reader2["RESULT"].ToString();
                    }
                }
            }
            ViewBag.DocPdf = "data:application/pdf;base64," + Convert.ToBase64String(data, 0, data.Length);

            return View();
        }

        public string GenerateOTP(string MbileNo, string channel)
        {
            ErrorLog error_log = new ErrorLog();
            // public string GenerateOTP(string MbileNo)
            //{
            try
            {
                channel = "INDOFINNET";
                string resultmsg = null;
                string URL = System.Configuration.ConfigurationSettings.AppSettings["MobileOTPURL"] + "?mobileno=" + MbileNo + "&channel=" + channel;
                var client = new RestClient(URL);
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                IRestResponse resp = client.Execute(request);
                string aotp;
                if (resp.StatusCode == 0)
                {
                    aotp = "Some Error Occured";
                    var result = objData.Database.ExecuteSqlRaw($"USP_IndoErrorLogs {(resp.ErrorException)},{(resp.ErrorMessage)},{(resp.StatusCode)},{(resp.StatusDescription)}, {("GenerateOTP")}, {"dataverify"}");
                }
                else
                {
                    aotp = resp.Content.ToString();
                    if (aotp == "\"OTP Generated Successfully!!!!!\"")
                    {
                        resultmsg = "OTP Generated Successfully...!";
                    }
                    var result1 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs {("GenerateOTP")}, {"dataverify"}");
                }
                return resultmsg;
            }

            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                var result = objData.Database.ExecuteSqlRaw($"USP_IndoErrorLogs{(ex.Message)}, {(ex.StackTrace)}, {"GenerateOTP"}, {"dataverify"}");
                TempData["msg"] = ex.Message;
                return ex.Message;

            }
        }
        public string SendOtp(string MbileNo)
        {
            ErrorLog error_log = new ErrorLog();
            var encryptedMobileNo = objtriple.Decrypt(MbileNo);
            string Resultmsg = null;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                Resultmsg = "OTP Generated Successfully...!";
                var client = new RestClient("https://cbs.indofinnet.com/api/SMSOTPRSSB?ToMobileNo=" + encryptedMobileNo);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                return Resultmsg;
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                var result = objData.Database.ExecuteSqlRaw($"USP_IndoErrorLogs{(ex.Message)}, {(ex.InnerException)}, {"SendOtp"}, {"dataverify"}");
                return Resultmsg;
            }

        }


        public string VerifyGenerateOTP(string MbileNo, string OTP, string Verifyname)
        {
            ErrorLog error_log = new ErrorLog();
            string Result;
            long CustomerId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
            try
            {
                long? res;
                string OtpVerify = null;
                string currentime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");

                if (MbileNo.Length == 10)
                {
                    if (OTP.ToString().Length == 6)
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                        var client = new RestClient("https://cbs.indofinnet.com/api/VerifyOTPForRSSB?mobileno=" + MbileNo + "&otp=" + OTP);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        IRestResponse response = client.Execute(request);
                        Result = "OTP Verified Successfully";

                        return Result;

                    }
                    else
                    {
                        Result = "Please enter valid 6 digit OTP";
                        return Result;
                    }
                }
                else
                {
                    Result = "Please enter 10 Digit Valid Mobile Number";
                    return Result;
                }

            }

            catch (Exception e)
            {
                error_log.WriteErrorLog(e.ToString());
                var result = objData.Database.ExecuteSqlRaw($"USP_IndoErrorLogs{(e.Message)}, {(e.InnerException)}, {"VerifyGenerateOTP"}, {"dataverify"}");
                return Result = e.Message;
            }

        }
        public ActionResult Opencamera(string Cust)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return View();
            }
            catch (Exception e)
            {
                error_log.WriteErrorLog(e.ToString());
                return Json(e.Message);
            }
        }
        public ActionResult Opencamera1(string Cust)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return View();
            }
            catch (Exception e)
            {
                error_log.WriteErrorLog(e.ToString());
                return Json(e.Message);
            }
        }


        [HttpPost]
        public string SaveImage(string base64image, long customerDetailID, string KYCImage)
        {
            if (string.IsNullOrEmpty(base64image))
                return "Image is null";
            byte[] CameraSnapbytes;
            byte[] KYCbytes;

            if (base64image != null && base64image != "" && base64image != "undefined")
            {
                CameraSnapbytes = Convert.FromBase64String(base64image);
            }
            else
            {
                CameraSnapbytes = null;
            }
            if (KYCImage != null && KYCImage != "" && KYCImage != "undefined")
            {
                KYCbytes = Convert.FromBase64String(KYCImage);
            }
            else
            {
                KYCbytes = null;
            }
            string Inserted = "";
            return Inserted;
        }

        public ActionResult DigitalKYCPdf()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                long CustomerId = 1;
                ClsSummeryDetails objSummery = new ClsSummeryDetails();
                var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerDetails {CustomerId}").AsEnumerable().FirstOrDefault();
                result.CustomerId = Convert.ToInt64(HttpContext.Session.GetString("CustomerId"));
                objSummery.CustomerId = Convert.ToString(CustomerId);
                objSummery.CFirstName = result.FirstName;
                objSummery.CMiddleName = result.MiddleName;
                objSummery.CLastName = result.LastName;
                objSummery.CGender = result.Gender;
                objSummery.CMobileNo = result.MobileNo;
                objSummery.CEmailId = result.EmailId;
                objSummery.ManualPanNo = result.PanNo;
                objSummery.Pin_Code = result.PinCode;
                objSummery.DOB = result.Dob;
                if (objSummery.DOB.Contains('-'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('-');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];
                }
                else if (objSummery.DOB.Contains('/'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('/');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];
                }
                if (result.LivePhoto != null)
                {
                    objSummery.LivePhoto = Convert.ToBase64String(result.LivePhoto);
                }
                objSummery.CAddress = result.ClientAddress1;
                return View(objSummery);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public ActionResult DigitalKYCPdfForCustOnly()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                long CustomerId = 1;
                ClsSummeryDetails objSummery = new ClsSummeryDetails();
                var result = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerDetails {CustomerId}").AsEnumerable().FirstOrDefault();
                result.CustomerId = Convert.ToInt64(HttpContext.Session.GetString("CustomerId"));
                objSummery.CustomerId = Convert.ToString(CustomerId);
                objSummery.CFirstName = result.FirstName;
                objSummery.CMiddleName = result.MiddleName;
                objSummery.CLastName = result.LastName;
                objSummery.CGender = result.Gender;
                objSummery.CMobileNo = result.MobileNo;
                objSummery.CEmailId = result.EmailId;
                objSummery.ManualPanNo = result.PanNo;
                objSummery.Pin_Code = result.PinCode;
                objSummery.DOB = result.Dob;
                if (objSummery.DOB.Contains('-'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('-');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];
                }
                else if (objSummery.DOB.Contains('/'))
                {
                    string[] DateOfBirth = objSummery.DOB.Split('/');
                    objSummery.DOB_MM = DateOfBirth[0];
                    objSummery.DOB_DD = DateOfBirth[1];
                    string[] year = DateOfBirth[2].Split(' ');
                    objSummery.DOB_yyyy = year[0];
                }
                if (result.LivePhoto != null)
                {
                    objSummery.LivePhoto = Convert.ToBase64String(result.LivePhoto);
                }
                objSummery.CAddress = result.ClientAddress1;
                return View(objSummery);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        
        [HttpGet]
        public ActionResult testmethoddemo()
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

        [HttpPost]
        public JsonResult SaveBCPhoto(string datatestdemo, string BCLatitude_Longitude)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ClsSummeryDetails objSummery = new ClsSummeryDetails();
                objSummery.BCPhoto = datatestdemo;
                ClsDocDetails objFinalDoc = new ClsDocDetails();

                long CustomerId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                if (objSummery.BCPhoto != null && objSummery.BCPhoto != "" && objSummery.BCPhoto != "undefined")
                {
                    objSummery.BCPhoto = objSummery.BCPhoto;
                }
                if (objSummery.BCPhoto != null)
                {
                    string extension = null;
                    objFinalDoc.DocName = "BC_livePhoto.jpg";
                    extension = "jpg";
                    objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetDocumentType {objFinalDoc.DocType}");
                    objFinalDoc.DocMainType = "BC_P";
                    objFinalDoc.DocCategoryCode = "0";
                    objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                    objFinalDoc.Latitude_Longitude = BCLatitude_Longitude;
                    string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                    objFinalDoc.DocDetails = Convert.FromBase64String(objSummery.BCPhoto);
                    objFinalDoc.DocumentId = Convert.ToString(objFinalDoc.CustomerDetailId);
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                    IMapper mapper = config.CreateMapper();
                    ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc);
                    isInserted = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments{(objResult)}");
                    HttpContext.Session.SetString("ll1", objFinalDoc.Latitude_Longitude);
                    if (Convert.ToInt32(isInserted) > 0)
                    {
                        //isSuccess = true;
                    }
                    objSummery.LivePhoto = null;
                }
                return Json("Photo Saved Successfully");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public JsonResult ShowAddress(string latitude, string Longitude, string OrgID)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                string resultmsg = null;
                var latcode = latitude.Substring(0, 7);
                var loncode = Longitude.Substring(0, 7);
                string URL = "https://apigateway.indofinnet.com/api/GeoLocation";
                var client = new RestClient(URL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                request.AddParameter("latitude", latcode);
                request.AddParameter("Longitude", loncode);
                request.AddParameter("OrgID", "Alpha01");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                client = new RestClient("https://apigateway.indofinnet.com/api/GeoLocation?OrgID=Alpha01&Latitude=" + latcode + "&Longitude=" + loncode);
                client.Timeout = -1;
                request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                response = client.Execute(request);
                Console.WriteLine(response.Content);
                return Json(response.Content);

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("NEW Errors");

            }
        }

        public static byte[] mergePdf6(byte[] pdf1, byte[] pdf2/*, byte[] pdf3*/)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                MemoryStream outStream = new MemoryStream();
                using (Document document = new Document())
                using (PdfCopy copy = new PdfCopy(document, outStream))
                {

                    document.Open();
                    PdfReader.unethicalreading = true;
                    copy.AddDocument(new PdfReader(pdf1));
                    copy.AddDocument(new PdfReader(pdf2));
                    document.Close();
                }
                return outStream.ToArray();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return null;
            }
        }

        public static byte[] mergePdf(byte[] pdf1, byte[] pdf2)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                MemoryStream outStream = new MemoryStream();
                using (Document document = new Document())
                using (PdfCopy copy = new PdfCopy(document, outStream))
                {

                    document.Open();

                    PdfReader.unethicalreading = true;
                    copy.AddDocument(new PdfReader(pdf1));
                    copy.AddDocument(new PdfReader(pdf2));
                    // copy.AddDocument(new PdfReader(pdf3));
                    document.Close();
                    //PdfReader.unethicalreading = true;
                }
                return outStream.ToArray();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return null;
            }
        }

        public static byte[] Append(byte[] inputPdf, params System.Drawing.Image[] images)
        {

            using (var ms = new MemoryStream())
            {
                var pdf = new PdfReader(inputPdf);
                var doc = new Document(pdf.GetPageSizeWithRotation(1));
                using (var writer = PdfWriter.GetInstance(doc, ms))
                {
                    doc.Open();

                    for (int page = 0; page < pdf.NumberOfPages; page++)
                    {
                        doc.SetPageSize(pdf.GetPageSizeWithRotation(page + 1));
                        doc.NewPage();
                        var pg = writer.GetImportedPage(pdf, page + 1);
                        int rotation = pdf.GetPageRotation(page + 1);
                        if (rotation == 90 || rotation == 270)
                            writer.DirectContent.AddTemplate(
                                pg, 0, -1f, 1f, 0, 0, pdf.GetPageSizeWithRotation(page).Height);
                        else
                            writer.DirectContent.AddTemplate(pg, 1f, 0, 0, 1f, 0, 0);
                    }
                    foreach (var image in images)
                    {
                        doc.NewPage();

                        ImageFormat format = image.PixelFormat == PixelFormat.Format1bppIndexed
                                             || image.PixelFormat == PixelFormat.Format4bppIndexed
                                             || image.PixelFormat == PixelFormat.Format8bppIndexed
                                                 ? ImageFormat.Tiff
                                                 : ImageFormat.Jpeg;
                        var pdfImage = iTextSharp.text.Image.GetInstance(image, format);
                        pdfImage.Alignment = Element.ALIGN_CENTER;
                        pdfImage.ScaleToFit(doc.PageSize.Width - 10, doc.PageSize.Height - 10);
                        doc.Add(pdfImage);
                    }
                    doc.Close();
                    writer.Close();
                }
                ms.Flush();
                return ms.GetBuffer();
            }
        }




        public byte[] removePagesFromPdf(byte[] sourceFile, int TotalPage)
        {

            //Used to pull individual pages from our source
            PdfReader r = new PdfReader(sourceFile);
            byte[] fsbyte;
            //Create our destination file
            using (MemoryStream fs = new MemoryStream())
            {
                using (Document doc = new Document())
                {
                    using (PdfWriter w = PdfWriter.GetInstance(doc, fs))
                    {
                        doc.Open();
                        for (int page = 2; page <= TotalPage; page++)
                        {
                            doc.NewPage();
                            w.DirectContent.AddTemplate(w.GetImportedPage(r, page), 0, 0);
                        }
                        doc.Close();
                        fsbyte = fs.GetBuffer();
                    }
                }
            }

            return fsbyte;
        }

        [HttpPost]
        public IActionResult Liveliness(IFormCollection abc)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string img = abc["base64img"];// frm["base64img"];

                var Photo = img.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                byte[] Image = Convert.FromBase64String(Photo);
                WebClient client1 = new WebClient();

                string url = "https://apigateway.indofinnet.com/api/CheckLivelinessFactor?OrgID=Alpha01";
                var client = new RestClient(url);
                //client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                request.AddParameter("application/octet-stream", Image, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                string responser = response.Content;

                string Response1 = response.Content.Replace("{", "");
                string Response2 = Response1.Replace("}", "");
                string Response3 = Response2.Replace("/", "");

                var jsonSendResponse = Response3.Split(',');
                var SendResponse = jsonSendResponse[2] + "  " + jsonSendResponse[3];

                return Json(SendResponse);

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Errors");

            }
        }
       
        public ActionResult EndCP()
        {
            string filename1 = "";
            try
            {
                string defg = "";
               
                string RefID = HttpContext.Session.GetString("RefID");
                string conn2 = _connectionString;
                using (SqlConnection connection2 = new SqlConnection(conn2))
                {
                    SqlCommand cmd2 = new SqlCommand("USP_GetMeetingDetails", connection2);
                    cmd2.CommandType = CommandType.StoredProcedure;

                    cmd2.Parameters.AddWithValue("@refID", RefID);


                    connection2.Open();


                    SqlDataReader reader2 = cmd2.ExecuteReader();
                    if (reader2.Read())
                    {

                        string RefID1 = reader2[5].ToString();
                        defg = RefID1;
                    }
                }

                string meetId = defg;
                ViewBag.meetId = meetId;
                //// ViewBag.link = "https://api.indofinnet.com/api/meeting/" + result.MeetingID + "/download";
                ViewBag.link = "https://api.indofinnet.com/api/meeting/" + meetId + "/download";
                WebClient wc = new WebClient();
                long amc = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string conn3 = _connectionString;
                using (SqlConnection connection3 = new SqlConnection(conn3))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_updateflag", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;

                    cmd3.Parameters.AddWithValue("@CustomerDetailId", amc);
                    connection3.Open();
                }

                TempData["msg"] = "Video Saved Successfully";
                return RedirectToAction("CustomerDocumentDetails1", "DataVerify");
            }
            catch (Exception ex)
            {
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;
                System.IO.File.AppendAllText(filename1, errormsg);
                PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        public ActionResult CustomerDocumentDetails1(ClsDocumentDetails objdoc)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                long? isInserted;
                ClsDocDetails objFinalDoc = new ClsDocDetails();
                // HttpContext.Session.SetString("DAEditCustomerdetailId", JsonConvert.SerializeObject(objdoc.CustomerDetailId));
                string proceedwithOCR = Convert.ToString(HttpContext.Session.GetString("proceedwithOCR"));
                string shareAadharNumber = Convert.ToString(HttpContext.Session.GetString("shareAadharNumber"));
                if (objdoc.UploadPhoto != null)
                {
                    var binaryReader = new BinaryReader((Stream)objdoc.UploadPhoto.Headers);
                    objFinalDoc.DocName = objdoc.UploadPhoto.FileName;
                    extension = objdoc.UploadPhoto.FileName.Split('.').LastOrDefault();
                    objFinalDoc.DocDetails = binaryReader.ReadBytes((int)objdoc.UploadPhoto.Length);
                    string fileContentType = objdoc.UploadPhoto.ContentType;
                    byte[] tempbytefile = new byte[objdoc.UploadPhoto.Length];
                    binaryReader.Close();
                    /*objFinalDoc.DocType =*/
                    objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType{objFinalDoc.DocType}");
                    objFinalDoc.DocMainType = "P";
                    objFinalDoc.DocCategoryCode = "0";
                    objFinalDoc.Source = "Upload";
                    objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                    objFinalDoc.DocumentId = (Convert.ToString(HttpContext.Session.GetString("PersonalId")));
                    string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                    IMapper mapper = config.CreateMapper();
                    INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                    isInserted = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments{(objResult)}");
                    if (isInserted > 0)
                    {
                        //isSuccess = true;
                    }
                    objdoc.UploadPhoto = null;
                }
                if (objdoc.ProofOfIdCode != null && objdoc.UploadprfOfId != null)
                {
                    if (objdoc.ProofOfIdCode == "67")
                    {
                        string ProofOfIdCode = objdoc.ProofOfIdCode;
                        var data = blackening(objdoc, ProofOfIdCode);
                        if (data == null)
                        {
                            TempData["msg"] = "Document";
                            return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                        }
                        else
                        {
                            var result1 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs({"ProofOfIdCode"}, {"CustomerDocumentDetails"}, {"DocumentDetails"}");
                        }
                    }
                    else
                    {
                        objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        string path = "/Uploads/" + objFinalDoc.CustomerDetailId;
                        string ImagePath = path;
                        string[] filenames = System.IO.Directory.GetFiles(ImagePath);
                        var binaryReader = new BinaryReader((Stream)objdoc.UploadprfOfId.Headers);
                        objFinalDoc.DocName = objdoc.UploadprfOfId.FileName;
                        extension = objdoc.UploadprfOfId.FileName.Split('.').LastOrDefault();
                        objFinalDoc.DocDetails = binaryReader.ReadBytes((int)objdoc.UploadprfOfId.Length);
                        string fileContentType = objdoc.UploadprfOfId.ContentType;
                        byte[] tempbytefile = new byte[objdoc.UploadprfOfId.Length];
                        binaryReader.Close();

                        /*objFinalDoc.DocType =*/
                        objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetPOIDocumentByCategory {objFinalDoc.DocType}");
                        objFinalDoc.DocCategoryCode = objdoc.ProofOfIdCode;
                        objFinalDoc.DocumentId = objdoc.DocumentIdPOI;
                        objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));

                        if (objdoc.DocumentIdDatePOI != null)
                        {
                            objFinalDoc.DocumentIdDate1 = DateTime.ParseExact(objdoc.DocumentIdDatePOI, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            objFinalDoc.DocumentIdDate1 = null;
                        }
                        string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                        IMapper mapper = config.CreateMapper();
                        INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                        isInserted = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments {(objResult)}");
                        if (isInserted > 0)
                        {
                            //isSuccess = true;
                        }
                    }
                }
                if (objdoc.ProofOfCorrAddCode != null && objdoc.UploadprfOfCorrAdd != null)
                {
                    if (objdoc.ProofOfCorrAddCode == "68")
                    {
                        string ProofOfCorrAddCode = objdoc.ProofOfCorrAddCode;
                        var data = blackening(objdoc, ProofOfCorrAddCode);
                        if (data != null)
                        {
                            var result1 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs {"ProofOfCorrAddCode"}, {"CustomerDocumentDetails"}, {"DocumentDetails"}");
                            TempData["msg"] = "Document Saved Successfully";
                            return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                        }
                        else
                        {
                            TempData["msg"] = "Document";
                            return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                        }
                    }
                    else
                    {
                        var binaryReader = new BinaryReader((Stream)objdoc.UploadprfOfCorrAdd.Headers);
                        objFinalDoc.DocName = objdoc.UploadprfOfCorrAdd.FileName;
                        extension = objdoc.UploadprfOfCorrAdd.FileName.Split('.').LastOrDefault();
                        objFinalDoc.DocDetails = binaryReader.ReadBytes((int)objdoc.UploadprfOfCorrAdd.Length);
                        string fileContentType = objdoc.UploadprfOfCorrAdd.ContentType;
                        byte[] tempbytefile = new byte[objdoc.UploadprfOfCorrAdd.Length];
                        binaryReader.Close();
                        /*objFinalDoc.DocType =*/
                        objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType {objFinalDoc.DocType}").AsEnumerable().FirstOrDefault();
                        objFinalDoc.DocMainType = "CA";
                        objFinalDoc.Source = "Upload";
                        objFinalDoc.DocCategoryCode = objdoc.ProofOfCorrAddCode;
                        objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                        objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        if (objdoc.DocumentIdDatePOA != null)
                        {
                            objFinalDoc.DocumentIdDate1 = DateTime.ParseExact(objdoc.DocumentIdDatePOA, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            objFinalDoc.DocumentIdDate1 = null;
                        }
                        string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                        IMapper mapper = config.CreateMapper();
                        INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                        isInserted = objData.Database.ExecuteSqlRaw($"USP_AddDocuments {objResult}");
                        if (isInserted > 0)
                        {
                            objDetails.AdmFlagMainTains.FromSqlRaw($"USP_CustomerUpdateFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}, {proceedwithOCR}, {shareAadharNumber}, {false}, {false}, {false}");
                            TempData["msg"] = "Document Saved Successfully";
                            return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                        }
                    }
                    TempData["msg"] = "Document Saved Successfully";
                    return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                }
                string conn = _connectionString;
                {
                    var Res = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))},{("I")}", conn).ToList();
                    ViewBag.POI = Res;
                }
                conn = _connectionString;

                var Ress = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))},{("CA")}", conn).ToList();
                ViewBag.POA = Ress;

                if (ViewBag.POI.Count == 0)
                {
                    ViewBag.msg = "Please Upload POI";
                    //return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome", new { @ShareAadharOrNot = ViewBag.msg });
                }
                else if (ViewBag.POA.Count == 0)
                {
                    //return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                }
                else
                {
                    using (SqlConnection connection3 = new SqlConnection(conn))
                    {
                        SqlCommand cmd3 = new SqlCommand("USP_CustomerUpdateFlag", connection3);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        cmd3.Parameters.AddWithValue("@proceedwithOCR", proceedwithOCR);
                        cmd3.Parameters.AddWithValue("@shareAadharNumber", shareAadharNumber);
                        cmd3.Parameters.AddWithValue("@isPanVerify", false);
                        cmd3.Parameters.AddWithValue("@isCKYCVerify", false);
                        cmd3.Parameters.AddWithValue("@isAadharVerify", false);

                        connection3.Open();
                        SqlDataReader reader3 = cmd3.ExecuteReader();
                        if (reader3.Read())
                        {
                            //var Result = reader2["RESULT"].ToString();
                        }
                    }
                }
                return RedirectToAction("SummerySheetDetails", "DataVerify");
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        public ActionResult blackening(ClsDocumentDetails upobj, string IdCode)
        {
            ErrorLog error_log = new ErrorLog();
            byte[] bytes = null;
            //string DateOfBirth1 = "";
            ClsDocDetails objFinalDoc = new ClsDocDetails();
            int i;
            try
            {
                byte[] imagebytearay = null;
                switch (IdCode)
                {
                    case "67":
                        using (BinaryReader br = new BinaryReader((Stream)upobj.UploadprfOfId.Headers))    ////convert string to binnery
                        {
                            bytes = br.ReadBytes((int)upobj.UploadprfOfId.Length);
                            imagebytearay = br.ReadBytes((int)upobj.UploadprfOfId.Length);
                        }
                        break;
                    case "68":
                        using (BinaryReader br = new BinaryReader((Stream)upobj.UploadprfOfCorrAdd.Headers))    ////convert string to binnery
                        {
                            bytes = br.ReadBytes((int)upobj.UploadprfOfCorrAdd.Length);
                            imagebytearay = br.ReadBytes((int)upobj.UploadprfOfCorrAdd.Length);
                        }
                        break;
                }
                var Number = @"^[a-zA-Z]+$";
                var DOBRegex = @"[0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{4}";
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(Bitmap));
                Bitmap image = (Bitmap)tc.ConvertFrom(bytes);                                             // var imageFile = File.ReadAllBytes(@"E:\ocrimg\vickysir.jpg");
                                                                                                          //var image = new Bitmap(@"C: \Users\SUNIL\Desktop\ComapnyPanCardImages\P17.jpg"); 
                var ocr = new Tesseract();
                ocr.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz/"); // If digit only
                //@"C:\OCRTest\tessdata" contains the language package, without this the method crash and app breaks
                ocr.Init(@"D:\OCRTest\tessdata", "eng", false);
                var result = ocr.DoOCR(image, System.Drawing.Rectangle.Empty);
                var solutionDirectory = @"D:\samples";//Directory.GetParent(Directory.GetCurrentDirectory()).FullName;// @"C:\inetpub\wwwroot\AlphaLoaclOCRPublishFolder\bin";//
                var tesseractPath = solutionDirectory + @"\tesseract-master.1153";
                var testFiles = Directory.EnumerateFiles(solutionDirectory + @"\samples");
                var text = ParseText(tesseractPath, bytes, "eng", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                string[] custDetails1 = (text.Split(',', '\n'));

                string strAadharNo = "";
                string DateOfBirth = "";

                if (text != null && text.Contains(" ") && (text.Contains("Address") || text.Contains("ddress") || text.Contains("dress")
                            || text.Contains("ress")) && !text.Contains("Father") && !text.Contains("INDIA") && !text.Contains("Year")
                                    && !text.Contains("Birth") && !text.Contains("DOB") && !text.Contains("Female")
                                    && !text.Contains("Male"))
                {
                    string[] details = (text.Split('\n'));
                    for (i = 0; i < details.Length; i++)
                    {
                        if (details[i] != "" && details[i] != null && details[i] != " ")
                        {
                            if (details[i].Contains("Address:"))
                            {
                                details[i] = details[i].Substring(details[i].LastIndexOf("Address:"));//+ details[i].Length
                            }
                            else
                            {
                                string aadharvalue = details[i];
                                foreach (Word word in result)
                                {
                                    var vals = "";
                                    var foursetRegex1 = @"\d{4}";
                                    Match Matchfourset1 = Regex.Match(Convert.ToString(word), foursetRegex1, RegexOptions.IgnoreCase);
                                    var onlynumregex1 = @"^[0-9 ]+$";
                                    Match MatchOnlydigits1 = Regex.Match(Convert.ToString(word), onlynumregex1, RegexOptions.IgnoreCase);
                                    if (Matchfourset1.Success == true || MatchOnlydigits1.Success == true)
                                    {
                                        string AadharNo1 = Convert.ToString(word);
                                        string dob4 = AadharNo1.Split(' ')[0];
                                        var pincoderegex = "^[1-9][0-9]{5}$";
                                        Match Matchpincode = Regex.Match(dob4, pincoderegex, RegexOptions.IgnoreCase);
                                        if (Matchpincode.Success == true || AadharNo1.Length != 9)
                                        {
                                            if (AadharNo1.Contains("/") || AadharNo1.Contains("-"))
                                            {
                                                vals = "";
                                            }
                                            vals = "";
                                        }
                                        else
                                        {
                                            vals = AadharNo1.Split(' ')[0];
                                            if (vals != null || vals != "")
                                            {
                                                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                                                {
                                                    Bitmap bmp = new Bitmap(ms);
                                                    Graphics gr = Graphics.FromImage(bmp);
                                                    int top = word.Top;
                                                    int left = word.Left;
                                                    int height = word.Bottom - word.Top;
                                                    int width = word.Right - word.Left;
                                                    int count = (width * 2) + (width / 4);
                                                    System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, count, height);
                                                    gr.DrawImage(Bitmap.FromFile(@"C:\inetpub\wwwroot\KYCSmartBankerPlus\imagesforaadhar\photo_2019-07-08_15-28-51.jpg"), rect);
                                                    bmp.Clone();
                                                    bmp.Save(@"C:\inetpub\wwwroot\KYCSmartBankerPlus\imagesforaadhar\test.jpg");
                                                    string msg = "Image Masked successfully";
                                                    return Json(msg);//, JsonRequestBehavior.AllowGet
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    string[] custDetails = (text.Split(',', '\n'));
                    for (i = 0; i < custDetails.Length; i++)
                    {
                        if (custDetails[i] != null && (custDetails[i].Contains("Year") || custDetails[i].Contains("of") || custDetails[i].Contains("Birth") || custDetails[i].Contains("DOB") || custDetails[i].Contains("Dob")))
                        {
                            if (custDetails[i].Contains("/"))
                            {
                                DateOfBirth = custDetails[i].Substring(custDetails[i].Length - 4);
                                break;
                            }
                            else if (custDetails[i].Contains("-"))
                            {
                                DateOfBirth = custDetails[i].Substring(custDetails[i].Length - 10);
                                break;
                            }
                            else
                            {
                                DateOfBirth = custDetails[i].Substring(custDetails[i].Length - 4);
                                break;
                            }
                        }
                    }
                    for (i = 0; i < custDetails.Length; i++)
                    {
                        string element = custDetails[i];
                        var numberRegex = @"\d";
                        Match MatchNumber = Regex.Match(custDetails[i], numberRegex, RegexOptions.IgnoreCase);
                        var aadharRegex = @"^\d{4}\s\d{4}\s\d{4}$";
                        Match MatchAadhar = Regex.Match(custDetails[i], aadharRegex, RegexOptions.IgnoreCase);
                        var foursetRegex = @"\d{4}";
                        Match Matchfourset = Regex.Match(custDetails[i], foursetRegex, RegexOptions.IgnoreCase);
                        var onlynumregex = @"^[0-9 ]+$";
                        Match MatchOnlydigits = Regex.Match(custDetails[i], onlynumregex, RegexOptions.IgnoreCase);
                        // var DOBRegex = @"[0-9]{2}[-|\/]{1}[0-9]{2}[-|\/]{1}[0-9]{4}";
                        Match DOB = Regex.Match(custDetails[i], DOBRegex, RegexOptions.IgnoreCase);
                        if (DOB.Success == true)
                        {
                            DateOfBirth = DOB.ToString();
                        }
                        if (custDetails[i] != null && custDetails[i].Length > 12 && MatchAadhar.Success == true)
                        {
                            string AadharNo = custDetails[i];
                            strAadharNo = AadharNo.Replace(" ", "");
                            foreach (Word word in result)
                            {
                                var vals = "";
                                var foursetRegex1 = @"\d{4}";
                                Match Matchfourset1 = Regex.Match(Convert.ToString(word), foursetRegex1, RegexOptions.IgnoreCase);
                                var onlynumregex1 = @"^[0-9 ]+$";
                                Match MatchOnlydigits1 = Regex.Match(Convert.ToString(word), onlynumregex1, RegexOptions.IgnoreCase);
                                if (Matchfourset1.Success == true || MatchOnlydigits1.Success == true)
                                {
                                    string AadharNo1 = Convert.ToString(word);
                                    if (AadharNo1.Contains("/") || AadharNo1.Contains("-"))
                                    {
                                        vals = "";
                                    }
                                    else
                                    {
                                        vals = AadharNo1.Split(' ')[0];
                                        if (AadharNo.Contains(vals))
                                        {
                                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                                            {
                                                Bitmap bmp = new Bitmap(ms);
                                                Graphics gr = Graphics.FromImage(bmp);
                                                int top = word.Top;
                                                int left = word.Left;
                                                int height = word.Bottom - word.Top;
                                                int width = word.Right - word.Left;
                                                int count = (width * 2) + (width / 4);
                                                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, count, height);
                                                gr.DrawImage(Bitmap.FromFile(@"C:\inetpub\wwwroot\KYCSmartBankerPlus\imagesforaadhar\photo_2019-07-08_15-28-51.jpg"), rect);
                                                bmp.Clone();
                                                bmp.Save(@"C:\inetpub\wwwroot\KYCSmartBankerPlus\imagesforaadhar\test.jpg");
                                                string msg = "Image Masked successfully";
                                                return Json(msg);//, JsonRequestBehavior.AllowGet
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                                else
                                {
                                }
                            }
                        }
                        else if (custDetails[i] != null && custDetails[i].Length >= 12 && MatchNumber.Success == true && MatchOnlydigits.Success == true)
                        {
                            string AadharNo = custDetails[i];
                            strAadharNo = AadharNo.Replace(" ", "");
                            foreach (Word word in result)
                            {
                                var foursetRegex1 = @"\d{4}";
                                Match Matchfourset1 = Regex.Match(Convert.ToString(word), foursetRegex1, RegexOptions.IgnoreCase);
                                var onlynumregex1 = @"^[0-9 ]+$";
                                Match MatchOnlydigits1 = Regex.Match(Convert.ToString(word), onlynumregex1, RegexOptions.IgnoreCase);
                                if (Matchfourset1.Success == true || MatchOnlydigits1.Success == true)
                                {
                                    string AadharNo1 = Convert.ToString(word);
                                    var vals = AadharNo1.Split(' ')[0];
                                    if (AadharNo.Contains(vals))
                                    {
                                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                                        {
                                            Bitmap bmp = new Bitmap(ms);
                                            Graphics gr = Graphics.FromImage(bmp);
                                            int top = word.Top;
                                            int left = word.Left;
                                            int height = word.Bottom - word.Top;
                                            int width = word.Right - word.Left;
                                            int count = (width * 2) + (width / 4);
                                            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, count, height);
                                            gr.DrawImage(Bitmap.FromFile(@"C:\inetpub\wwwroot\KYCSmartBankerPlus\imagesforaadhar\photo_2019-07-08_15-28-51.jpg"), rect);
                                            bmp.Clone();
                                            bmp.Save(@"C:\inetpub\wwwroot\KYCSmartBankerPlus\imagesforaadhar\test.jpg");
                                            string msg = "Image Masked successfully";
                                            return Json(msg);//, JsonRequestBehavior.AllowGet
                                        }
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                        }
                        else if (custDetails[i] != null && custDetails[i].Length > 12 && custDetails[i].Contains("lllll") && MatchNumber.Success == true)
                        {
                            strAadharNo = custDetails[i].Substring(0, 14);
                            foreach (Word word in result)
                            {
                                var foursetRegex1 = @"\d{4}";
                                Match Matchfourset1 = Regex.Match(Convert.ToString(word), foursetRegex1, RegexOptions.IgnoreCase);
                                var onlynumregex1 = @"^[0-9 ]+$";
                                Match MatchOnlydigits1 = Regex.Match(Convert.ToString(word), onlynumregex1, RegexOptions.IgnoreCase);
                                if (Matchfourset1.Success == true || MatchOnlydigits1.Success == true)
                                {
                                    string AadharNo1 = Convert.ToString(word);
                                    var vals = AadharNo1.Split(' ')[0];
                                    if (AadharNo1.Contains(vals))
                                    {
                                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                                        {
                                            Bitmap bmp = new Bitmap(ms);
                                            Graphics gr = Graphics.FromImage(bmp);
                                            int top = word.Top;
                                            int left = word.Left;
                                            int height = word.Bottom - word.Top;
                                            int width = word.Right - word.Left;
                                            int count = (width * 2) + (width / 4);
                                            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, count, height);
                                            gr.DrawImage(Bitmap.FromFile(@"D:\imagesforaadhar\photo_2019-07-08_15-28-51.jpg"), rect);
                                            bmp.Clone();
                                            bmp.Save(@"D:\imagesforaadhar\test.jpg");
                                            string msg = "Image Masked successfully";
                                            return Json(msg);//, JsonRequestBehavior.AllowGet
                                        }
                                    }
                                    else
                                    {
                                    }
                                }
                            }
                        }
                        else if (custDetails[i] != null && custDetails[i].Length >= 14 && MatchNumber.Success == true && !custDetails[i].Contains("Father")
                                && !custDetails[i].Contains("INDIA") && !custDetails[i].Contains("Year") && !custDetails[i].Contains("of")
                                && !custDetails[i].Contains("Birth") && !custDetails[i].Contains("DOB") && !custDetails[i].Contains("Female")
                                && !custDetails[i].Contains("Male") && Matchfourset.Success == true)
                        {
                            string test = custDetails[i].Substring(0, 14);
                            Match MatchNumberforaadhar = Regex.Match(test, onlynumregex, RegexOptions.IgnoreCase);
                            if (MatchNumberforaadhar.Success == true)
                            {
                                strAadharNo = custDetails[i].Substring(0, 14);
                                foreach (Word word in result)
                                {
                                    var foursetRegex1 = @"\d{4}";
                                    Match Matchfourset1 = Regex.Match(Convert.ToString(word), foursetRegex1, RegexOptions.IgnoreCase);
                                    var onlynumregex1 = @"^[0-9 ]+$";
                                    Match MatchOnlydigits1 = Regex.Match(Convert.ToString(word), onlynumregex1, RegexOptions.IgnoreCase);
                                    if (Matchfourset1.Success == true || MatchOnlydigits1.Success == true)
                                    {
                                        string AadharNo1 = Convert.ToString(word);
                                        var vals = AadharNo1.Split(' ')[0];
                                        if (strAadharNo.Contains(vals))
                                        {
                                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                                            {
                                                Bitmap bmp = new Bitmap(ms);
                                                Graphics gr = Graphics.FromImage(bmp);
                                                int top = word.Top;
                                                int left = word.Left;
                                                int height = word.Bottom - word.Top;
                                                int width = word.Right - word.Left;
                                                int count = (width * 2) + (width / 4);
                                                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, count, height);
                                                gr.DrawImage(Bitmap.FromFile(@"C:\inetpub\wwwroot\KYCSmartBankerPlus\imagesforaadhar\photo_2019-07-08_15-28-51.jpg"), rect);
                                                bmp.Clone();
                                                bmp.Save(@"C:\inetpub\wwwroot\KYCSmartBankerPlus\imagesforaadhar\test.jpg");
                                                string msg = "Image Masked successfully";
                                                return Json(msg);//, JsonRequestBehavior.AllowGet
                                            }
                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                            }
                            else if (!custDetails[i].Contains("/") && !custDetails[i].Contains(":") && !custDetails[i].Contains(".") &&
                                !custDetails[i].Contains(",") && !custDetails[i].Contains("-") && !custDetails[i].Contains("'"))
                            {
                                string[] array = custDetails[i].Split(' ');
                                string str1 = "";
                                string str2 = "";
                                string str3 = "";
                                string str4 = "";
                                string str5 = "";
                                string Check1 = array[0];
                                int testlen1 = array[0].Length;
                                if (testlen1 == 4)
                                {
                                    str1 = array[0];
                                }
                                else if (testlen1 > 4)
                                {
                                    str1 = (Check1.Substring(Check1.Length - 4));
                                }
                                string Check2 = array[1];
                                int testlen2 = array[1].Length;
                                if (testlen2 == 4)
                                {
                                    str2 = array[1];
                                }
                                else if (testlen2 > 4)
                                {
                                    str2 = (Check2.Substring(Check2.Length - 4));
                                }
                                string Check3 = array[2];
                                int testlen3 = array[2].Length;
                                if (testlen3 == 4)
                                {
                                    str3 = array[2];
                                }
                                else if (testlen3 > 4)
                                {
                                    str3 = (Check3.Substring(Check3.Length - 4));
                                }
                                string Check4 = array[3];
                                int testlen4 = array[3].Length;
                                if (testlen4 == 4)
                                {
                                    str4 = array[3];
                                }
                                else if (testlen4 > 4)
                                {
                                    str4 = (Check4.Substring(Check4.Length - 4));
                                }
                                string Check5 = array[4];
                                int testlen5 = array[4].Length;
                                if (testlen5 == 4)
                                {
                                    str5 = array[4];
                                }
                                else if (testlen5 > 4)
                                {
                                    str5 = (Check5.Substring(Check5.Length - 4));
                                }
                                string allStr = str1 + str2 + str3 + str4 + str5;
                                strAadharNo = String.Join(", ", allStr);
                                Console.WriteLine(strAadharNo);
                            }
                        }
                        else
                        {
                            foreach (Word word in result)
                            {
                                var vals = "";
                                var foursetRegex1 = @"\d{4}";
                                Match Matchfourset1 = Regex.Match(Convert.ToString(word), foursetRegex1, RegexOptions.IgnoreCase);
                                var onlynumregex1 = @"^[0-9 ]+$";
                                Match MatchOnlydigits1 = Regex.Match(Convert.ToString(word), onlynumregex1, RegexOptions.IgnoreCase);
                                if (Matchfourset1.Success == true || MatchOnlydigits1.Success == true)
                                {
                                    string AadharNo1 = Convert.ToString(word);
                                    string dob4 = AadharNo1.Split(' ')[0];
                                    var pincoderegex = "^[1-9][0-9]{5}$";
                                    Match Matchpincode = Regex.Match(dob4, pincoderegex, RegexOptions.IgnoreCase);
                                    if (Matchpincode.Success == true || dob4.Contains(DateOfBirth) || AadharNo1.Length != 9)
                                    {
                                        if (AadharNo1.Contains("/") || AadharNo1.Contains("-"))
                                        {
                                            vals = "";
                                        }
                                        vals = "";
                                    }
                                    else
                                    {
                                        vals = AadharNo1.Split(' ')[0];
                                        if (vals != null || vals != "")
                                        {
                                            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
                                            {
                                                var result11 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs {"MemoryStream"}, {"blackening_POI"}, {"DocumentDetails"}");
                                                Bitmap bmp = new Bitmap(ms);
                                                Graphics gr = Graphics.FromImage(bmp);
                                                int top = word.Top;
                                                int left = word.Left;
                                                int height = word.Bottom - word.Top;
                                                int width = word.Right - word.Left;
                                                int count = (width * 2) + (width / 4);
                                                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(left, top, count, height);
                                                var result12 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs({"D:imagesforaadhar"}, {"blackening_POI"}, {"DocumentDetails"}");
                                                gr.DrawImage(Bitmap.FromFile(@"D:\imagesforaadhar\photo_2019-07-08_15-28-51.jpg"), rect);
                                                bmp.Clone();
                                                var result13 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs {"D:imagesforaadhar_test.jpg"}, {"blackening_POI"}, {"DocumentDetails"}");
                                                bmp.Save(@"D:\imagesforaadhar\test.jpg");
                                                var result14 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs {"path"}, {"blackening_POI"}, {"DocumentDetails"}");
                                                string path = @"D:\imagesforaadhar\test.jpg";
                                                byte[] photo = System.IO.File.ReadAllBytes(path);

                                                string msg = "Image Masked successfully";
                                                if (photo != null)
                                                {
                                                    switch (IdCode)
                                                    {
                                                        case "67":
                                                            objFinalDoc.DocName = upobj.UploadprfOfId.FileName;
                                                            extension = upobj.UploadprfOfId.FileName.Split('.').LastOrDefault();
                                                            objFinalDoc.DocDetails = photo;
                                                            string fileContentType = upobj.UploadprfOfId.ContentType;
                                                            byte[] tempbytefile = new byte[upobj.UploadprfOfId.Length];

                                                            objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType {objFinalDoc.DocType}");

                                                            objFinalDoc.DocMainType = "I";
                                                            objFinalDoc.DocCategoryCode = upobj.ProofOfIdCode;

                                                            objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                                            objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));

                                                            if (upobj.DocumentIdDatePOI != null)
                                                            {
                                                                objFinalDoc.DocumentIdDate1 = DateTime.ParseExact(upobj.DocumentIdDatePOI, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                            }
                                                            else
                                                            {
                                                                objFinalDoc.DocumentIdDate1 = null;
                                                            }

                                                            string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                                                            var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                                                            IMapper mapper = config.CreateMapper();
                                                            INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                                                            isInserted3 = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments{(objResult)}");


                                                            if (isInserted3 > 0)
                                                            {
                                                                var result1 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs {("67")}, {"blackening_POI"}, {"DocumentDetails"}");
                                                            }
                                                            break;
                                                        case "68":
                                                            objFinalDoc.DocName = upobj.UploadprfOfCorrAdd.FileName;
                                                            extension = upobj.UploadprfOfCorrAdd.FileName.Split('.').LastOrDefault();
                                                            objFinalDoc.DocDetails = photo;
                                                            string fileContentType1 = upobj.UploadprfOfCorrAdd.ContentType;
                                                            byte[] tempbytefile1 = new byte[upobj.UploadprfOfCorrAdd.Length];

                                                            objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType {(objFinalDoc.DocType)}");
                                                            objFinalDoc.DocMainType = "CA";
                                                            objFinalDoc.DocCategoryCode = upobj.ProofOfCorrAddCode;

                                                            objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                                            objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));

                                                            if (upobj.DocumentIdDatePOA != null)
                                                            {
                                                                objFinalDoc.DocumentIdDate1 = DateTime.ParseExact(upobj.DocumentIdDatePOA, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                                            }
                                                            else
                                                            {
                                                                objFinalDoc.DocumentIdDate1 = null;
                                                            }
                                                            string DocumentsDetails1 = JsonConvert.SerializeObject(objFinalDoc);
                                                            var config1 = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                                                            IMapper mapper1 = config1.CreateMapper();
                                                            INDO_FIN_NET.Models.ClsDocumentDetails objResult1 = mapper1.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                                                            isInserted3 = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments{(objResult1)}");
                                                            if (isInserted3 > 0)
                                                            {
                                                                var result1 = objData.AdmIndoErrorLogs.FromSqlRaw($"USP_IndoErrorLogs ({"68"}, {"blackening_POA"}, {"DocumentDetails"}");
                                                            }
                                                            break;
                                                    }
                                                }

                                                return Json(msg);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
            }
            return Json("Image quality too low! Provide with another image");
        }

        private static string ParseText(string tesseractPath, byte[] imageFile, params string[] lang)
        {
            ErrorLog error_log = new ErrorLog();
            string output = string.Empty;
            var tempOutputFile = System.IO.Path.GetTempPath() + Guid.NewGuid();
            var tempImageFile = System.IO.Path.GetTempFileName();
            try
            {
                System.IO.File.WriteAllBytes(tempImageFile, imageFile);
                ProcessStartInfo info = new ProcessStartInfo();
                info.WorkingDirectory = tesseractPath;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.UseShellExecute = false;
                info.FileName = "cmd.exe";
                info.Arguments =
                    "/c tesseract.exe " +
                    // Image file.
                    tempImageFile + " " +
                    // Output file (tesseract add '.txt' at the end)
                    tempOutputFile +
                    // Languages.
                    " -l " + string.Join("+", lang);
                // Start tesseract.
                Process process = Process.Start(info);
                process.WaitForExit();
                if (process.ExitCode == 0)
                {
                    // Exit code: success.
                    output = System.IO.File.ReadAllText(tempOutputFile + ".txt");
                }
                else
                {
                    throw new Exception("Error. Tesseract stopped with an error code = " + process.ExitCode);
                }
            }
            finally
            {
                System.IO.File.Delete(tempImageFile);
                System.IO.File.Delete(tempOutputFile + ".txt");
            }
            return output;
        }
    }
}
