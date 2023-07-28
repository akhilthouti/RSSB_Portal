using Grpc.Core;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Models.QuickEnroll;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
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

using Microsoft.IdentityModel;

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
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.KeyVault.Models;
using static INDO_FIN_NET.dcumentdedupe;
using Aspose.Cells;
using Org.BouncyCastle.Tsp;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using ImageFormat = System.Drawing.Imaging.ImageFormat;
using Aspose.Pdf;
using Aspose.Pdf.Drawing;
using Point = System.Drawing.Point;
using Aspose.Pdf.Devices;
using Path = System.IO.Path;
using Resolution2 = Aspose.Pdf.Devices.Resolution;
using System.Net.Mail;
using System.IO.Compression;
using INDO_FIN_NET.Repository.Data;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json.Linq;
using ParameterType = RestSharp.ParameterType;

namespace INDO_FIN_NET.Controllers.OrgnisationDetails
{

    public class KYCQuickEnrollController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        private readonly string _connectionString;
        public KYCQuickEnrollController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData1 = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
        }
     
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        string extension = null;
        public long? isInserted = null;
        string UserId = null;
        string VendorPassword = "iC2pOBYQi6w5hK38DWDyPw==";
        string ts = DateTime.Now.ToString("dd-MM-yy  hh-mm-ss");
        static BetterWebClient client = new BetterWebClient();
        clsEncryptAndDecrypt objData = new clsEncryptAndDecrypt();
        ClsDigitalKYC1 objDigitalKYCdata = new ClsDigitalKYC1();
        TripleDESImplementation objtriple = new TripleDESImplementation();
        private readonly object city;

        [HttpGet]
        public ActionResult CheckAdharByRadio(string userid, string BankName, string alphaservicename, string ServiceProvider, string CategoryName, string ProductName)
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

        [HttpGet]
        public async Task<ActionResult> CustomerQuickEnrollment([FromServices] IActiveLogin objLogin, string proceedwithOCR, string shareAadharNumber, string sharePan, string checkCKYC, string msg)
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
            if (HttpContext.Session.GetString("DACustMOBNo") != null)
            {
                if (!clsCustAuthorize.IsAuthCustomerDetails(Convert.ToInt64(ObjTripleDes.Decrypt(HttpContext.Session.GetString("EncryPersonalId").ToString())), HttpContext.Session.GetString("SessionOrgKey").ToString()))
                {
                    return RedirectToAction("CustomerRegistration", "SignIn");
                }
            }
            else if (HttpContext.Session.GetString("OrgUserId") != null)
            {
              
            }
            ClsCustQuickEnrollment obj = new ClsCustQuickEnrollment();
            //ViewBag.login = "1";
            string XMLReferenceID = null;
            string NSDL_PANNumber = null;
            string AadharUid = null;
            try
            {
                ViewBag.QuickEnrollment = true;
                var custid = 1;
                if (custid == null)
                {
                    ViewBag.RejectId = 1;
                    var qdata = objDetails.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsByRejectId {(custid)}").AsEnumerable().FirstOrDefault();
                    obj.QEFirstName = qdata.CustFirstName;
                    obj.QELastName = qdata.CustLastName;
                    obj.QEEmailId = qdata.CustEmailId;
                    obj.QEMobileNo = qdata.CustMobileNo;
                    HttpContext.Session.SetString("OrgUserId", ObjTripleDes.Encrypt("456"));
                    var orgUseri = HttpContext.Session.GetString("OrgUserId");
                    string newTokenOrSessionKey = ObjTripleDes.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + qdata.CustMobileNo.Trim());
                    string OrgId = ObjTripleDes.Decrypt(Convert.ToString(orgUseri));
                    HttpContext.Session.SetString("SessionOrgKey", newTokenOrSessionKey);
                    HttpContext.Session.GetString("PersonalId");
                    ViewBag.QECustId = HttpContext.Session.GetString("PersonalId");
                    var verificationtype1 = (from details in objDetails.TblVerificationTypes.FromSqlRaw($"USP_Get_VerificationDetails {(obj.QEVType)}").ToList()
                                             select new SelectListItem()
                                             {
                                                 Text = details.CustVerificationType,
                                                 Value = details.VtypeId.ToString(),
                                             }).ToList();
                    verificationtype1.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.VTypeData = verificationtype1;
                    ViewBag.UpdateQE = TempData["UpdateQE"];
                }
                if (obj.QEVType == null)
                {
                    ViewBag.VTypeData = new SelectList(new[] { /*new { ID = "P", Value = "Passport" },*/ /*new { ID = "V", Value = "Voter ID" },*/
                      new { ID = "P1", Value = "Pan" },/* new { ID = "DL", Value = "Driving Licence" },*/ new { ID = "A", Value = "Aadhaar" } }, "ID", "Value");
                }
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    var result1 = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    HttpContext.Session.SetString("PersonalId", result1);
                    //HttpContext.Session.SetString("PersonalId", "1");
                    HttpContext.Session.GetString("PersonalId");
                    HttpContext.Session.GetString("DAEditCustomerdetailId");
                    ViewBag.AdminFlag = "AdminFlag";
                    if (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")) > 0)
                    {
                        var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                        if (result != null)
                        {
                            var CustQE = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerQEData {HttpContext.Session.GetString("PersonalId")}").AsEnumerable().FirstOrDefault();
                            if ((result.IsPanVerify) == true)
                            {
                                string PanNo = CustQE.PanNo;
                                ViewBag.NSDL_PanNumber = PanNo;
                                HttpContext.Session.SetString("NSDL_PANNumber", PanNo);
                                ViewBag.IsPanVerify = true;
                            }
                            if (result.IsAadharVerify == true)
                            {
                                long refNo = CustQE.CustomerDetailId;
                                var Aadharresponse = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharDataForMatches {(refNo)}").AsEnumerable().FirstOrDefault();
                                string refNo1 = refNo.ToString();
                                HttpContext.Session.SetString("XMLReferenceID",refNo1);
                            }
                            if (result.IsPanVerify == true)
                            {
                                string refNo = CustQE.CustomerDetailId.ToString();
                                var PanResponse = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetPanData {Convert.ToInt64(refNo)}").AsEnumerable().FirstOrDefault();
                                string refNo1 = refNo.ToString();
                                HttpContext.Session.SetString("PanReferenceID", refNo1);
                            }
                            if (CustQE == null && HttpContext.Session.GetString("UserRole") == "1")
                            {
                                obj.QEFirstName = CustQE.FirstName;
                                obj.QELastName = CustQE.LastName;
                                obj.QEMobileNo = CustQE.MobileNo;
                                obj.QEpanNo = CustQE.PanNo;
                                obj.QEaadhaarNo = CustQE.AadharNo;
                                obj.QEEmailId = CustQE.EmailId;
                                if (!string.IsNullOrEmpty(obj.QEpanNo))
                                {
                                    obj.QEVTypeTextbox = obj.QEpanNo;
                                }
                            }
                        }
                    }
                }
                if (TempData["msg"] != null)
                {
                    if (msg == "1")
                    {
                        ViewBag.msg = "";
                    }
                    else
                    {
                    }
                }
                try
                {
                    string CheckDigiPanStatus = "";
                    string CheckDigiAadharStatus = "";
                    string CheckDigiDriveStatus = "";
                    string CheckDataPanStatus = "";
                    string PanFalse = "";
                    string DRLCFalse = "";
                    string AadharFalse = "";
                    if (!CheckDigiPanStatus.Contains("error") || !CheckDigiAadharStatus.Contains("error") || !CheckDigiDriveStatus.Contains("error") || !CheckDataPanStatus.Contains("error"))
                    {
                        var DigiDoc = HttpContext.Session.GetString("DigiDoc");                                           
                        if (HttpContext.Session.GetString("UserRole") == "1")
                        {
                            var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (result.IsDigiAadharSumbitted == true)
                            {
                                if (result.IsDigiPansumbitted == true && result.IsDigilDrlcsumbitted == true || result.IsDigiPansumbitted == true && result.IsDigilDrlcsumbitted == false || result.IsDigiPansumbitted == false && result.IsDigilDrlcsumbitted == false)
                                {
                                    DigiDoc = "AADHAAR_XML";
                                }
                                
                            }
                            else
                            if (result.IsPanDone == true)
                            {
                                DigiDoc = "PAN";
                            }
                            else if (result.IsDrivingLicenceDone == true)
                            {
                                DigiDoc = "Drvlwithxml";
                                HttpContext.Session.SetString("drvl", "DRIVINGLICENSE");
                            }
                            else if (HttpContext.Session.GetString("XMLReferenceID") != null)
                            {
                                DigiDoc = "Offline_AADHAAR_XML";
                            }
                            else if (HttpContext.Session.GetString("PanReferenceID") != null)
                            {
                                DigiDoc = "IsPanVerify";
                            }
                        }
                        ViewBag.Digidoc = DigiDoc;
                        if (DigiDoc == "PAN")
                        {
                            var Response = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigiPan {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (Response != null)
                            {
                                ViewBag.PANNo = ObjTripleDes.Decrypt(Response.Panno);
                                ViewBag.FName = Response.Firstname;
                                ViewBag.mName = Response.Middlename;
                                ViewBag.lName = Response.Lastname;
                                ViewBag.dob = Response.Dob;
                                ViewBag.gender = Response.Gender;
                                ViewBag.country = Response.Country;
                                ViewBag.ORGname = Response.Orgname;
                                ViewBag.PANverifiedOn = Response.PanverifiedOn;
                                ViewBag.Digidoc = DigiDoc;
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect");
                                ViewBag.DocDetails = HttpContext.Session.GetString("DocumentDetails");
                                ViewBag.DocDetails2 = HttpContext.Session.GetString("DocumentDetails2");
                                ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                ViewBag.PanPan = "True";
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];
                                    ViewBag.Digidoc = Description;
                                    ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkPanstatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.Digidoc = Description;
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                        ViewBag.PanPan = "False";
                                    }
                                    else
                                    {
                                        ViewBag.Digidoc = "Digilocker Through Error Occured  PAN";
                                        //TempData["msg"] = "Digilocker Through Error Occured  PAN";
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                }
                            }
                        }
                        else if (DigiDoc == "Both")
                        {
                            var Response9 = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigiPan {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            //string myURLFund9 = "https://indodbservice.azurewebsites.net/api/KYCQuickEnroll/USP_GetDigiPan?CustomerId=" + UserId + "";
                            if (Response9 != null)
                            {
                                ViewBag.PANNo = ObjTripleDes.Decrypt(Response9.Panno);
                                ViewBag.FName = Response9.Firstname;
                                ViewBag.mName = Response9.Middlename;
                                ViewBag.lName = Response9.Lastname;
                                ViewBag.dob = Response9.Dob;
                                ViewBag.gender = Response9.Gender;
                                ViewBag.country = Response9.Country;
                                ViewBag.ORGname = Response9.Orgname;
                                ViewBag.PANverifiedOn = Response9.PanverifiedOn;
                                ViewBag.Digidoc = DigiDoc;
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect");
                                ViewBag.DocDetails = HttpContext.Session.GetString("DocumentDetails");
                                ViewBag.DocDetails2 = HttpContext.Session.GetString("DocumentDetails2");
                                ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];
                                    ViewBag.CheckStatus = Description;
                                    ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkPanstatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.CheckStatus = Description;
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                    else
                                    {
                                        ViewBag.CheckStatus = "Digilocker Through error Occured in PAN";
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                }
                            }
                            var Response1 = objDetails.AdmDigiDrivingLicences.FromSqlRaw($"USP_GetDigiDriving {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            //string myURLFund10 = "https://indodbservice.azurewebsites.net/api/KYCQuickEnroll/USP_GetDigiDriving?CustomerId=" + UserId + "";
                            if (Response1 != null)
                            {
                                ViewBag.CustDigilockerVerify = "Hide";
                                ViewBag.Digi_DRVLC = Response1.Drvlc;
                                ViewBag.Digi_Dfirstname = Response1.Firstname;
                                ViewBag.Digi_Dswd = Response1.Swd;
                                ViewBag.Digi_Dlastname = Response1.Lastname;
                                ViewBag.Digidob2 = Response1.Dob;
                                ViewBag.Digi_Dcountry = Response1.Country;
                                ViewBag.Digi_DORGname = Response1.Orgname;
                                ViewBag.Digi_DAddress = Response1.Address;
                                ViewBag.DocDetails1 = HttpContext.Session.GetString("DocumentDetails1");
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect1");
                                var prefix = "data:image /gif;base64,";
                                string imgs = Convert.ToBase64String(Response1.Photo);
                                //ViewBag.Drvphoto = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("DrivePhoto")));
                               ViewBag.DAphoto = (string.Format("data:image/jpg;base64,{0}", imgs)); //HttpContext.Session.GetString("DrivePhoto")));
                                ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];
                                    ViewBag.CheckStatus = Description;
                                    ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkDrivestatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.CheckStatus = Description;
                                        ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    }
                                    else
                                    {
                                        ViewBag.CheckStatus = "Digilocker Through error Occured in DrivingLIC"; 
                                        ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    }
                                }
                            }
                        }
                        else if (DigiDoc == "AADHAAR_XML")
                        {
                            var Response2 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (Response2 != null)
                            {
                                ViewBag.firstname = Response2.Firstname;
                                ViewBag.middlename = Response2.Middlename;
                                obj.MiddleName = Response2.Middlename;
                                ViewBag.lastname = Response2.Lastname;
                                obj.LastName = Response2.Lastname;
                                ViewBag.dob1 = Response2.Dob;
                                ViewBag.gender1 = Response2.Gender;
                                obj.Gender = Response2.Gender;
                                ViewBag.aadharno = Response2.Uid;
                                obj.AadharNo = Response2.Uid;
                                ViewBag.Country = Response2.Country;
                                obj.Country = Response2.Country;
                                ViewBag.locality = Response2.Locality;
                                obj.Locality = Response2.Locality;
                                ViewBag.house = Response2.House;
                                obj.House = Response2.House;
                                ViewBag.street = Response2.Street;
                                obj.Street = Response2.Street;
                                ViewBag.district = Response2.District;
                                obj.District = Response2.District;
                                ViewBag.state = Response2.State;
                                obj.State = Response2.State;
                                ViewBag.Vtc = Response2.Vtc;
                                ViewBag.Pc = Response2.Pc;
                                ViewBag.imgid = Response2.Photo;
                                ViewBag.photo = Response2.Photo;
                                obj.Photo = Response2.Photo;
                                ViewBag.Address = Response2.Address;
                                var prefix = "data:image /gif;base64,";
                                string imgs = Convert.ToBase64String(Response2.Photo);
                                ViewBag.photo = (string.Format("data:image/jpg;base64,{0}", imgs));
                                ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                if (Response2 != null && HttpContext.Session.GetString("UserRole") == "1")
                                {
                                    obj.FirstName = Response2.Firstname;
                                    obj.MiddleName = Response2.Middlename;
                                    obj.LastName = Response2.Lastname;
                                    obj.Gender = Response2.Gender;
                                    obj.AadharNo = Response2.Uid;
                                    obj.Country = Response2.Country;
                                    obj.Locality = Response2.Locality;
                                    obj.House = Response2.House;
                                    obj.Street = Response2.Street;
                                    obj.District = Response2.District;
                                    obj.State = Response2.State;
                                    obj.Photo = Response2.Photo;
                                }
                            }
                            else
                            {
                                string error = Convert.ToString(HttpContext.Session.GetString("checkAadharstatus"));
                                if (error != null && error != "")
                                {
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.Digidoc = Description;
                                        ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                    }
                                    else
                                    {
                                        ViewBag.Digidoc = "Digilocker Through Error Occured  Aadharxml";
                                        ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                    }
                                }
                                else
                                {
                                    ViewBag.Digidoc = "Digilocker Through Error Occured  Aadharxml";
                                    ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                }
                            }
                            var Response9 = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigiPan {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (Response9 != null)
                            {
                                ViewBag.PANNo = ObjTripleDes.Decrypt(Response9.Panno);
                                obj.digilockerpan = Response9.Panno;
                                ViewBag.FName = Response9.Firstname;
                                obj.Digi_FirstName = Response9.Firstname;
                                ViewBag.mName = Response9.Middlename;
                                obj.Digi_MiddleName = Response9.Middlename;
                                ViewBag.lName = Response9.Lastname;
                                obj.Digi_LastName = Response9.Lastname;
                                obj.digifullname = Response9.Firstname + " " + Response9.Middlename + " " + Response9.Lastname;
                                ViewBag.dob = Response9.Dob;
                                obj.DateOfBirth = Response9.Dob;
                                ViewBag.gender = Response9.Gender;
                                obj.Digigender = Response9.Gender;
                                ViewBag.country = Response9.Country;
                                obj.Digi_country = Response9.Country;
                                ViewBag.ORGname = Response9.Orgname;
                                obj.Digi_ORGname = Response9.Orgname;
                                obj.Digi_PANverifiedOn = Response9.PanverifiedOn;
                                ViewBag.PANverifiedOn = Response9.PanverifiedOn;
                                ViewBag.Digidoc = DigiDoc;
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect");
                                ViewBag.DocDetails = HttpContext.Session.GetString("DocumentDetails");
                                ViewBag.DocDetails2 = HttpContext.Session.GetString("DocumentDetails2");
                                ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];

                                    ViewBag.CheckStatus = Description;
                                    ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkPanstatus"));
                                    if (error != null && error != "")
                                    {
                                        if (error.Contains("error_description"))
                                        {
                                            var serializer = new JavaScriptSerializer();
                                            dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                            var Description = jsonObject["error_description"];
                                            ViewBag.CheckStatus = Description;
                                            ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                        }
                                        else
                                        {
                                            ViewBag.CheckStatus = "Digilocker Through error Occured in PAN";
                                            ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.CheckStatus = "Digilocker Through error Occured in PAN";
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                }
                            }
                        }
                        else if (DigiDoc == "Offline_AADHAAR_XML")
                        {
                            var AadharData = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetCustomerAdharDetailsByCustId {Convert.ToInt64(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();
                            if (AadharData != null)
                            {
                                string s = AadharData.AadharName;
                                string[] Name = s.Split(' ');
                                ViewBag.firstname = Name[0];
                                obj.FirstName = Name[0];
                                ViewBag.middlename = Name[1];
                                obj.MiddleName = Name[1];
                                ViewBag.lastname = Name[2];
                                obj.LastName = Name[2];
                                ViewBag.dob1 = AadharData.AadharDob;
                                obj.DateOfBirth = AadharData.AadharDob;
                                ViewBag.gender1 = AadharData.AadharGender;
                                obj.Gender = AadharData.AadharGender;                             
                                ViewBag.aadharno = AadharData.AadharNumber;
                                obj.AadharNo = AadharData.AadharNumber;
                                ViewBag.Country = AadharData.Country;
                                obj.Country = AadharData.Country;
                                ViewBag.locality = AadharData.Locality;
                                obj.Locality = AadharData.Locality;
                                ViewBag.house = AadharData.House;
                                obj.House = AadharData.House;
                                ViewBag.street = AadharData.Street;
                                obj.Street = AadharData.Street;
                                ViewBag.district = AadharData.District;
                                obj.District = AadharData.District;
                                ViewBag.state = AadharData.State;
                                obj.State = AadharData.State;
                                ViewBag.Vtc = AadharData.Vtc;
                                ViewBag.Pc = AadharData.PinCode;
                                obj.Pincode = AadharData.PinCode;
                                ViewBag.photo = AadharData.AadharPhoto;
                                obj.CustomerPhoto = AadharData.AadharPhoto;
                                ViewBag.Address = AadharData.AadharAddress;
                                obj.Address = AadharData.AadharAddress;
                                var prefix = "data:image /gif;base64,";
                            }
                            else
                            {    
                            }
                        }
                        else if (DigiDoc == "IsPanVerify")
                        {
                            string s = HttpContext.Session.GetString("PersonalId");
                            var Response = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigiPan {s}").AsEnumerable().FirstOrDefault();
                            if (Response != null)
                            {
                                ViewBag.PANNo = ObjTripleDes.Decrypt(Response.Panno);
                                ViewBag.FName = Response.Firstname;
                                ViewBag.mName = Response.Middlename;
                                ViewBag.lName = Response.Lastname;
                                ViewBag.title = Response.title;
                                ViewBag.Digidoc = DigiDoc;
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect");
                                ViewBag.DocDetails = HttpContext.Session.GetString("DocumentDetails");
                                ViewBag.DocDetails2 = HttpContext.Session.GetString("DocumentDetails2");
                                ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];
                                    ViewBag.Digidoc = Description; //HttpContext.Session.GetString("checkdatastatus");//"PAN Documents able to fetch properly Error occurred while fetching xml data";
                                    TempData["msg"] = Description;//HttpContext.Session.GetString("checkdatastatus");//"PAN Documents able to fetch properly Error occurred while fetching xml data";
                                    ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkPanstatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.Digidoc = Description;//Session["checkPanstatus"];//"Digilocker Through Error Occured  PAN";
                                        TempData["msg"] = Description;//Session["checkPanstatus"];//"Digilocker Through Error Occured  PAN";
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                    else
                                    {
                                        ViewBag.Digidoc = "Digilocker Through Error Occured  PAN";
                                        TempData["msg"] = "Digilocker Through Error Occured  PAN";
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                }
                            }
                        }
                        else if (DigiDoc == "PANwithxml")
                        {
                            var Response = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigiPan {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            //string myURLFund9 = "https://indodbservice.azurewebsites.net/api/KYCQuickEnroll/USP_GetDigiPan?CustomerId=" + UserId + "";
                            if (Response != null)
                            {
                                ViewBag.PANNo = Response.Panno;
                                ViewBag.FName = Response.Firstname;
                                ViewBag.mName = Response.Middlename;
                                ViewBag.lName = Response.Lastname;
                                ViewBag.dob = Response.Dob;
                                ViewBag.gender = Response.Gender;
                                ViewBag.country = Response.Country;
                                ViewBag.ORGname = Response.Orgname;
                                ViewBag.PANverifiedOn = Response.PanverifiedOn;
                                ViewBag.Digidoc = DigiDoc;
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect");
                                ViewBag.DocDetails = HttpContext.Session.GetString("DocumentDetails");
                                ViewBag.DocDetails2 = HttpContext.Session.GetString("DocumentDetails2");
                                ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                ViewBag.PanFlag = "True";
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];
                                    ViewBag.CheckStatus = Description;
                                    ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkPanstatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.CheckStatus = Description;
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                        ViewBag.PanFlag = "False";
                                    }
                                    else
                                    {
                                        ViewBag.CheckStatus = "Digilocker Through Error Occured in Pan";
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                }
                            }
                            var Response6 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            //string myURLFund6 = "https://indodbservice.azurewebsites.net/api/KYCQuickEnroll/USP_GetDigiAadharxml?CustomerId=" + UserId + "";
                            if (Response6 != null)
                            {
                                ViewBag.firstname = Response6.Firstname;
                                ViewBag.middlename = Response6.Middlename;
                                ViewBag.lastname = Response6.Lastname;
                                ViewBag.dob1 = Response6.Dob;
                                ViewBag.gender1 = Response6.Gender;
                                ViewBag.aadharno = Response6.Uid;
                                ViewBag.Country = Response6.Country;
                                ViewBag.locality = Response6.Locality;
                                ViewBag.house = Response6.House;
                                ViewBag.street = Response6.Street;
                                ViewBag.district = Response6.District;
                                ViewBag.state = Response6.State;
                                ViewBag.Vtc = Response6.Vtc;
                                ViewBag.Pc = Response6.Pc;
                                ViewBag.imgid = Response6.Photo;
                                ViewBag.Address = Response6.Address;
                                var prefix = "data:image /gif;base64,";
                                string imgs = Convert.ToBase64String(Response6.Photo);
                                ViewBag.photo = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("AadharPhoto")));
                                ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                            }
                            else
                            {
                                string error = Convert.ToString(HttpContext.Session.GetString("checkAadharstatus"));
                                if (error.Contains("error_description"))
                                {
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];
                                    ViewBag.CheckStatus = Description;
                                    ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                }
                                else
                                {
                                    ViewBag.CheckStatus = "Digilocker Through Error Occured in aadharxml";
                                    ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                }
                            }
                        }
                        else if (DigiDoc == "Drvlwithxml")
                        {
                            var Response1 = objDetails.AdmDigiDrivingLicences.FromSqlRaw($"USP_GetDigiDriving {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            //string myURLFund7 = "https://indodbservice.azurewebsites.net/api/KYCQuickEnroll/USP_GetDigiDriving?CustomerId=" + UserId + "";
                            if (Response1 != null)
                            {
                                ViewBag.CustDigilockerVerify = "Hide";
                                ViewBag.Digi_DRVLC = Response1.Drvlc;
                                ViewBag.Digi_Dfirstname = Response1.Firstname;
                                ViewBag.Digi_Dswd = Response1.Swd;
                                ViewBag.Digi_Dlastname = Response1.Lastname;
                                ViewBag.Digidob2 = Response1.Dob;
                                ViewBag.Digi_Dcountry = Response1.Country;
                                ViewBag.Digi_DORGname = Response1.Orgname;
                                ViewBag.Digi_DAddress = Response1.Address;
                                ViewBag.Digidoc = HttpContext.Session.GetString("drvl");
                                ViewBag.Digidoc = "Drvlwithxml";
                                ViewBag.DocDetails1 = HttpContext.Session.GetString("DocumentDetails1");
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect1");
                                var prefix = "data:image /gif;base64,";
                                string imgs = Convert.ToBase64String(Response1.Photo);
                                //ViewBag.Drvphoto = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("DrivePhoto")));
                                ViewBag.DAphoto = (string.Format("data:image/jpg;base64,{0}",imgs));
                                ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];
                                    ViewBag.CheckStatus = Description;
                                    ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkDrivestatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.CheckStatus = Description;
                                        ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    }
                                    else
                                    {
                                        ViewBag.CheckStatus = "digilocker Through Error Occured in drivingLIC";
                                        ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    }
                                }
                            }
                            var Response5 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            //string myURLFund = "https://indofinnetwebapi.azurewebsites.net/ap/KYCQuickEnroll/USP_GetDigiAadharxml?CustomerId=?" + UserId + "";
                            if (Response5 != null)
                            {
                                ViewBag.firstname = Response5.Firstname;
                                ViewBag.middlename = Response5.Middlename;
                                ViewBag.lastname = Response5.Lastname;
                                ViewBag.dob1 = Response5.Dob;
                                ViewBag.gender1 = Response5.Gender;
                                ViewBag.aadharno = Response5.Uid;
                                ViewBag.Country = Response5.Country;
                                ViewBag.locality = Response5.Locality;
                                ViewBag.house = Response5.House;
                                ViewBag.street = Response5.Street;
                                ViewBag.district = Response5.District;
                                ViewBag.state = Response5.State;                           
                                ViewBag.Vtc = Response5.Vtc;
                                ViewBag.Pc = Response5.Pc;
                                ViewBag.imgid = Response5.Photo;
                                ViewBag.Address = Response5.Address;
                                var prefix = "data:image /gif;base64,";
                                string imgs = Convert.ToBase64String(Response5.Photo);
                                ViewBag.photo = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("AadharPhoto")));
                                ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                            }
                            else
                            {
                                string error = Convert.ToString(HttpContext.Session.GetString("checkAadharstatus"));
                                if (error!= null)
                                {
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.CheckStatus = Description;
                                        ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                    }
                                    else
                                    {
                                        ViewBag.CheckStatus = "digilocker Through Error Occured In Aadharxml";
                                        ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                    }
                                }
                            }
                        }
                        else if (DigiDoc == "Bothwithxml")
                        {
                            var Response = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigiPan {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (Response != null)
                            {
                                ViewBag.PANNo = Response.Panno;
                                ViewBag.FName = Response.Firstname;
                                ViewBag.mName = Response.Middlename;
                                ViewBag.lName = Response.Lastname;
                                ViewBag.dob = Response.Dob;
                                ViewBag.gender = Response.Gender;
                                ViewBag.country = Response.Country;
                                ViewBag.ORGname = Response.Orgname;
                                ViewBag.PANverifiedOn = Response.PanverifiedOn;
                                ViewBag.Digidoc = DigiDoc;
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect");
                                ViewBag.DocDetails = HttpContext.Session.GetString("DocumentDetails");
                                ViewBag.DocDetails2 = HttpContext.Session.GetString("DocumentDetails2");
                                ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];
                                    ViewBag.CheckStatus = Description;
                                    ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkPanstatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];
                                        ViewBag.CheckStatus = Description;
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                    else
                                    {
                                        ViewBag.CheckStatus = "Digilocker  Error Occured in PAN";
                                        //TempData["msg"] = "Digilocker  Error Occured in PAN";
                                        ViewBag.PanFalse = HttpContext.Session.GetString("CheckPan");
                                    }
                                }
                            }

                            var Response1 = objDetails.AdmDigiDrivingLicences.FromSqlRaw($"USP_GetDigiDriving {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (Response1 != null)
                            {
                                ViewBag.CustDigilockerVerify = "Hide";
                                ViewBag.Digi_DRVLC = Response1.Drvlc;
                                ViewBag.Digi_Dfirstname = Response1.Firstname;
                                ViewBag.Digi_Dswd = Response1.Swd;
                                ViewBag.Digi_Dlastname = Response1.Lastname;
                                ViewBag.Digidob2 = Response1.Dob;
                                ViewBag.Digi_Dcountry = Response1.Country;
                                ViewBag.Digi_DORGname = Response1.Orgname;
                                ViewBag.Digi_DAddress = Response1.Address;
                                ViewBag.DocDetails1 = HttpContext.Session.GetString("DocumentDetails1");
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect1");
                                var prefix = "data:image /gif;base64,";
                                string imgs = Convert.ToBase64String(Response1.Photo);
                               
                                ViewBag.DAphoto = (string.Format("data:image/jpg;base64,{0}",HttpContext.Session.GetString("DrivePhoto")));
                                //ViewBag.photo = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("DrivePhoto"))); 
                                ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");

                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    var Response5 = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigiPan {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}");
                                    ViewBag.Count = Response5.Count();
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];

                                    ViewBag.CheckStatus = Description;
                                    //TempData["msg"] = Description;
                                    ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkDrivestatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];

                                        ViewBag.CheckStatus = Description;
                                        //TempData["msg"] = Description;
                                        ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    }
                                    else
                                    {
                                        ViewBag.CheckStatus = "Digilocker  Error Occured in DrivingLIC";
                                        //TempData["msg"] = "Digilocker  Error Occured in DrivingLIC";
                                        ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    }
                                }
                            }
                            var Response2 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (Response2 != null)
                            {
                                ViewBag.firstname = Response2.Firstname;
                                ViewBag.middlename = Response2.Middlename;
                                ViewBag.lastname = Response2.Lastname;
                                ViewBag.dob1 = Response2.Dob;
                                ViewBag.gender1 = Response2.Gender;
                                ViewBag.aadharno = Response2.Uid;
                                ViewBag.Country = Response2.Country;
                                ViewBag.locality = Response2.Country;
                                ViewBag.house = Response2.House;
                                ViewBag.street = Response2.Street;
                                ViewBag.district = Response2.District;
                                ViewBag.state = Response2.State;
                                ViewBag.Vtc = Response2.Vtc;
                                ViewBag.Pc = Response2.Pc;
                                ViewBag.imgid = Response2.Photo;
                                ViewBag.Address = Response2.Address;
                                var prefix = "data:image /gif;base64,";
                                string imgs = Convert.ToBase64String(Response2.Photo);
                                ViewBag.photo = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("AadharPhoto")));
                                ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");

                            }
                            else
                            {
                                var ResponseDrive = objDetails.AdmDigiDrivingLicences.FromSqlRaw($"USP_GetDigiDriving {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                                string error = Convert.ToString(HttpContext.Session.GetString("checkAadharstatus"));
                                if (error.Contains("error_description"))
                                {
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];

                                    ViewBag.CheckStatus = Description;
                                    TempData["msg"] = Description;
                                    ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                }
                                else
                                {
                                    ViewBag.CheckStatus = "Digilocker  Error Occured in Aadharxml";
                                    TempData["msg"] = "Digilocker  Error Occured in Aadharxml";
                                    ViewBag.AadharFalse = HttpContext.Session.GetString("CheckAadhar");
                                }
                            }
                        }
                        else if (DigiDoc == "DRIVINGLICENSE")
                        {
                            var Response = objDetails.AdmDigiDrivingLicences.FromSqlRaw($"USP_GetDigiDriving {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (Response != null)
                            {
                                ViewBag.CustDigilockerVerify = "Hide";
                                ViewBag.Digi_DRVLC = Response.Drvlc;
                                ViewBag.Digi_Dfirstname = Response.Firstname;
                                ViewBag.Digi_Dswd = Response.Swd;
                                ViewBag.Digi_Dlastname = Response.Lastname;
                                ViewBag.Digidob2 = Response.Dob;
                                ViewBag.Digi_Dcountry = Response.Country;
                                ViewBag.Digi_DORGname = Response.Orgname;
                                ViewBag.Digi_DAddress = Response.Address;
                                ViewBag.DocDetails1 = HttpContext.Session.GetString("DocumentDetails1");
                                ViewBag.DocTypeSelect = HttpContext.Session.GetString("DoctypeSelect1");
                                var prefix = "data:image /gif;base64,";
                                string imgs = Convert.ToBase64String(Response.Photo);
                                ViewBag.Drvphoto = (string.Format("data:image/jpg;base64,{0}", HttpContext.Session.GetString("DrivePhoto"))); 
                                ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                ViewBag.Driving = "True";
                            }
                            else
                            {
                                if (CheckDataPanStatus.Contains("error_description"))
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkdatastatus"));
                                    var serializer = new JavaScriptSerializer();
                                    dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                    var Description = jsonObject["error_description"];

                                    ViewBag.Digidoc = Description;
                                   // TempData["msg"] = Description;
                                    ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    
                                }
                                else
                                {
                                    string error = Convert.ToString(HttpContext.Session.GetString("checkDrivestatus"));
                                    if (error.Contains("error_description"))
                                    {
                                        var serializer = new JavaScriptSerializer();
                                        dynamic jsonObject = serializer.Deserialize<dynamic>(error);
                                        var Description = jsonObject["error_description"];

                                        ViewBag.Digidoc = Description;
                                        //TempData["msg"] = Description;
                                        ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    }
                                    else
                                    {
                                        ViewBag.Digidoc = "Digilocker Through Error Occured  DrivingLIC";
                                        //TempData["msg"] = "Digilocker Through Error Occured  DrivingLIC";
                                        ViewBag.DRLCFalse = HttpContext.Session.GetString("CheckDRLC");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.CheckStatus = "Error";
                        TempData["msg"] = "Some Error Occured Through Digilocker";
                    }
                }
                catch (Exception ex)
                {
                    error_log.WriteErrorLog(ex.ToString());
                    // PortalException.InsertPortalException(ex);
                    return Json("Exception");
                }
                if (HttpContext.Session.GetString("XMLReferenceID") != null)
                {
                    XMLReferenceID = Convert.ToString(HttpContext.Session.GetString("XMLReferenceID"));
                    ViewBag.XMLRefID = XMLReferenceID;
                    ViewBag.shareAadharNumber = HttpContext.Session.GetString("shareAadharNumber");
                    obj.AadharVerificationType = true;
                }

                if (HttpContext.Session.GetString("XMLReferenceID") != null && (HttpContext.Session.GetString("PersonalId") != null))
                {
                    XMLReferenceID = Convert.ToString(HttpContext.Session.GetString("XMLReferenceID"));
                    ViewBag.XMLRefID = XMLReferenceID;
                    ViewBag.shareAadharNumber = HttpContext.Session.GetString("shareAadharNumber");
                    obj.AadharVerificationType = true;
                    obj.AadhaarverificationType = "Offline";

                    objDetails.AdmFlagMainTains.FromSqlRaw($"USP_UpdateCustAadharFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}, {1}");
                    objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_UpdateCustIdInAdhar {(XMLReferenceID, Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}");
                }
                if (HttpContext.Session.GetString("AadharUid") != null)
                {
                    AadharUid = Convert.ToString(HttpContext.Session.GetString("AadharUid"));
                    ViewBag.AadharUid = AadharUid;
                    obj.AadharVerificationType = true;
                }
                if (proceedwithOCR == null && shareAadharNumber == null)
                {
                    proceedwithOCR = Convert.ToString(HttpContext.Session.GetString("proceedwithOCR"));
                    shareAadharNumber = Convert.ToString(HttpContext.Session.GetString("shareAadharNumber"));
                }
                else if (sharePan != null || checkCKYC == null)
                {
                    ViewBag.sharePan = sharePan;
                    ViewBag.checkCKYC = checkCKYC;
                }
                else
                {
                    HttpContext.Session.SetString("proceedwithOCR", proceedwithOCR);

                    HttpContext.Session.SetString("shareAadharNumber", shareAadharNumber);

                    ViewBag.shareAadharNumber = shareAadharNumber;
                }

                if (HttpContext.Session.GetString("KYCverificationType") != null)
                {
                    obj.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("KYCverificationType"));
                }
                if (HttpContext.Session.GetString("Otherverification") != null)
                {
                    obj.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("Otherverification"));
                }

                ViewBag.Gender_status = new SelectList(new[] { new { ID = "M", Value = "Male" }, new { ID = "F", Value = "Female" } }, "ID", "Value");
                ViewBag.CKYC_POA = new SelectList(new[] { new { ID = "01", Value = "Aadhaar" },
                                                          new { ID = "02", Value = "Passport" },
                                                           new { ID = "03", Value = "Driving License" },
                                                           new { ID = "04", Value = "Voters Identity Card" },
                                                           new { ID = "05", Value = "NREGA Job Card" },
                                                           new { ID = "08", Value = "National Population Register Letter" },
                                                           new { ID = "09", Value = "E-KYC Authentication" } }, "ID", "Value");
                ViewBag.marital_status = new SelectList(new[] { new { ID = "01", Value = "Married" }, new { ID = "02", Value = "UnMarried" } }, "ID", "Value");

                ViewBag.CKYCoption = new SelectList(new[] { new { ID = "P", Value = "Passport" }, new { ID = "V", Value = "Voter ID" },
                    new { ID = "P1", Value = "Pan" }, new { ID = "DL", Value = "Driving Licence" }, new { ID = "A", Value = "Aadhar" } }, "ID", "Value");

                var verificationtype = (from details in objDetails.SysCountries.FromSqlRaw($"USP_GetallCountry_CKYC {(obj.QEVType)}").ToList()
                                        select new SelectListItem()
                                        {
                                            Text = details.Country,
                                            Value = details.CountryId.ToString(),
                                        }).ToList();
                verificationtype.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.CKYCCountry = verificationtype;

                if (HttpContext.Session.GetString("PersonalId") == null || TempData["CustMob"] != null)
                {
                    string guid = Guid.NewGuid().ToString();
                    // https://resident.uidai.gov.in/offline-kyc
                    try
                    {
                        client.DownloadString("https://resident.uidai.gov.in/offline-kyc");
                    }
                    catch (Exception ex)
                    {
                        return View(obj);
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }
       
        public ActionResult LogStatus([FromServices] IActiveLogin objLogin,string Lstatus)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string result = "";
                var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
                if (LoginStatus == "Active")
                {
                    string message = "Session Expired";
                    result = message;

                }
                return Json(result);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }

        }

        [HttpPost]
        public async Task<ActionResult> CustomerQuickEnrollment([FromServices] IActiveLogin objLogin,string QEFirstName, string QELastName, string QEMobileNo, string QEEmailId, string QEVType, string QEpanNo, string QEaadhaarNo, string QEvoterId, string QEpassportNo, string QEdrivingLicenceNo, string QEVTypeTextbox)
        {
            
            ErrorLog error_log = new ErrorLog();
            ClsCustQuickEnrollment objQuickEnroll = new ClsCustQuickEnrollment();
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
            FormCollection data;
            string check = "";
            //var LoginStatus = objLogin.OrganisationDetails(HttpContext, _connectionString);
            //if (LoginStatus == "Active")
            //{
            //    string message = "Session Expired";
            //    return RedirectToAction("LogStatus", "KYCQuickEnroll", new { Lstatus = message });
            //}
            if (HttpContext.Session.GetString("DACustMOBNo") != null)
            {
                if (!clsCustAuthorize.IsAuthCustomerDetails(Convert.ToInt64(ObjTripleDes.Decrypt(HttpContext.Session.GetString("EncryPersonalId").ToString())), HttpContext.Session.GetString("SessionOrgKey").ToString()))
                {
                    return RedirectToAction("CustomerRegistration", "SignIn");
                }
            }
            else if (HttpContext.Session.GetString("UserId") != null)
            {
                if (!clsAdminAuthorize.IsAuthorizeCustDetail((ObjTripleDes.Decrypt(HttpContext.Session.GetString("UserId").ToString())), HttpContext.Session.GetString("SessionKey").ToString()))
                {
                    return RedirectToAction("UserDetails", "AdminLogin");
                }

            }
            try
            {
                if (QEVType == "P1")
                {
                    objQuickEnroll.QEpanNo = QEVTypeTextbox;
                }
                else
                {
                    objQuickEnroll.QEpanNo = "";
                }
                if (QEVType == "A")
                {
                    objQuickEnroll.QEaadhaarNo = QEVTypeTextbox;
                }
                else
                {
                    objQuickEnroll.QEaadhaarNo = "";
                }
                if (QEVType == "DL")
                {
                    objQuickEnroll.QEdrivingLicenceNo = QEVTypeTextbox;
                }
                else
                {
                    objQuickEnroll.QEdrivingLicenceNo = "";
                }
                if (QEVType == "P")
                {
                    objQuickEnroll.QEpassportNo = QEVTypeTextbox;
                }
                else
                {
                    objQuickEnroll.QEpassportNo = "";
                }
                if (QEVType == "V")
                {
                    objQuickEnroll.QEvoterId = QEVTypeTextbox;
                }
                else
                {
                    objQuickEnroll.QEvoterId = "";
                }

                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_Dropout_KYC_QE", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Cust_FirstName", QEFirstName);
                    cmd.Parameters.AddWithValue("@Cust_LastName", QELastName);
                    cmd.Parameters.AddWithValue("@MobileNo", QEMobileNo);
                    HttpContext.Session.SetString("MobileNoVKYC", QEMobileNo);
                    cmd.Parameters.AddWithValue("@panNo", objQuickEnroll.QEpanNo);
                    cmd.Parameters.AddWithValue("@AadhaarNo", objQuickEnroll.QEaadhaarNo);
                    cmd.Parameters.AddWithValue("@voterId", objQuickEnroll.QEvoterId);
                    cmd.Parameters.AddWithValue("@passportNo", objQuickEnroll.QEpassportNo);
                    cmd.Parameters.AddWithValue("@DrivingLicenceNo", objQuickEnroll.QEdrivingLicenceNo);

                    TempData["CustMob"] = QEMobileNo;
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var MobResp = reader["RESULT"].ToString();
                        if (MobResp == "0")
                        {
                            HttpContext.Session.SetString("MobileNo", QEMobileNo);
                            HttpContext.Session.SetString("EmailId", QEEmailId);
                            TempData["CustMob"] = QEMobileNo;
                            string SelectedValue = QEVType;
                            if (SelectedValue == "P1")
                            {
                                objQuickEnroll.Cust_VerificationType = "Pan";
                                objQuickEnroll.QEpanNo = QEVTypeTextbox;
                            }
                            if (SelectedValue == "DL")
                            {
                                objQuickEnroll.Cust_VerificationType = "Driving Licence";
                                objQuickEnroll.QEdrivingLicenceNo = QEVTypeTextbox;
                            }
                            if (SelectedValue == "A")
                            {
                                objQuickEnroll.Cust_VerificationType = "Aadhaar";
                                objQuickEnroll.QEaadhaarNo = QEVTypeTextbox;
                            }
                            if (SelectedValue == "P")
                            {
                                objQuickEnroll.Cust_VerificationType = "Passport";
                                objQuickEnroll.QEpassportNo = QEVTypeTextbox;
                            }
                            if (SelectedValue == "V")
                            {
                                objQuickEnroll.Cust_VerificationType = "Voter ID";
                                objQuickEnroll.QEvoterId = QEVTypeTextbox;
                            }


                            string strEncryptSessionkey;
                            strEncryptSessionkey = ObjTripleDes.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + QEMobileNo.ToString()).Trim();

                            using (SqlConnection connection2 = new SqlConnection(conn))
                            {
                                SqlCommand cmd2 = new SqlCommand("USP_InsertSignupOfCutomerNew", connection2);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@firstName", QEFirstName);
                                cmd2.Parameters.AddWithValue("@lastName", QELastName);
                                cmd2.Parameters.AddWithValue("@Emailid", QEEmailId);
                                cmd2.Parameters.AddWithValue("@mobileNo", QEMobileNo);
                                cmd2.Parameters.AddWithValue("@panNo", objQuickEnroll.QEpanNo);
                                cmd2.Parameters.AddWithValue("@AadhaarNo", objQuickEnroll.QEaadhaarNo);
                                cmd2.Parameters.AddWithValue("@voterId", objQuickEnroll.QEvoterId);
                                cmd2.Parameters.AddWithValue("@passportNo", objQuickEnroll.QEpassportNo);
                                cmd2.Parameters.AddWithValue("@DrivingLicenceNo", objQuickEnroll.QEdrivingLicenceNo);
                                cmd2.Parameters.AddWithValue("@Cust_SessionToken", strEncryptSessionkey);
                                cmd2.Parameters.AddWithValue("@FacilitatorCode", "1");
                                cmd2.Parameters.AddWithValue("@IsAssistedCustFlag", false);
                                cmd2.Parameters.AddWithValue("@Cust_VerificationType", objQuickEnroll.Cust_VerificationType);

                                connection2.Open();
                                int result = cmd2.ExecuteNonQuery();
                                connection2.Close();

                                var results = "";
                                if (result > 0)
                                {
                                    SqlCommand cmd3 = new SqlCommand("getcustomerdetailid", connection2);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    DataTable dt = new DataTable();
                                    SqlDataAdapter dp = new SqlDataAdapter(cmd3);
                                    connection2.Open();
                                    dp.Fill(dt);
                                    SqlDataReader reader3 = cmd3.ExecuteReader();

                                    if (reader3.Read())
                                    {
                                        results = reader3["CustDetailsId"].ToString();
                                    }
                                    connection2.Close();

                                }
                                
                                if (results != null)
                                {
                                    HttpContext.Session.SetString("QEPersonalId", Convert.ToString(results));

                                    HttpContext.Session.SetString("PersonalId", results);

                                    string custid = HttpContext.Session.GetString("PersonalId");
                                    ViewBag.QECustId = HttpContext.Session.GetString("PersonalId");
                                    objQuickEnroll.QECustDetailsId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                                    ViewBag.QECustDetailsId = objQuickEnroll.QECustDetailsId;

                                    string message = "Customer  Enrolled Successfully";
                                    //string message = "Customer  Enrolled Successfully Refrence No is " + "" + custid;

                                    return Json(message);
                                }
                                else
                                {
                                    string message = "unSuccess";

                                    return Json(message);
                                }
                            }
                            if (MobResp != null)

                            {
                                if (objQuickEnroll.LivePhoto != null && objQuickEnroll.LivePhoto != "" && objQuickEnroll.LivePhoto != "undefined")
                                {
                                    objQuickEnroll.livecameraphoto = Convert.FromBase64String(objQuickEnroll.LivePhoto);
                                }
                                else
                                {
                                    objQuickEnroll.livecameraphoto = null;
                                }

                                if (HttpContext.Session.GetString("NSDL_PANNumber") != null && HttpContext.Session.GetString("PersonalId") != null)
                                {
                                    long? PResult = objDetails.Database.ExecuteSqlRaw($"USP_InsertSignupOfCutomerNew {(Convert.ToString(HttpContext.Session.GetString("NSDL_PANNumber")))},{(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}");

                                    long? PP = objDetails.Database.ExecuteSqlRaw($"USP_UpdateCustPanFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}, {true}");
                                }

                                string DOB = objQuickEnroll.DateOfBirth;

                                var result = objDetails.AdmCkycCustomerDetails.FromSqlRaw($"USP_CustomerExistsCheck {(objQuickEnroll.FirstName)},{(objQuickEnroll.LastName)},{(objQuickEnroll.DateOfBirth)},{(objQuickEnroll.PANNumber)},{(objQuickEnroll.AadharNo)},{(objQuickEnroll.VoterId)},{(objQuickEnroll.Passport)},{(objQuickEnroll.DrivingLicence)},{""}");

                                HttpContext.Session.SetString("KYCverificationType", Convert.ToString(objQuickEnroll.KYCverificationType));
                                HttpContext.Session.SetString("PanVerificationType", Convert.ToString(objQuickEnroll.PanVerificationType));
                                HttpContext.Session.SetString("CKYCVerificationType", Convert.ToString(objQuickEnroll.CKYCVerificationType));
                                HttpContext.Session.SetString("AadharVerificationType", Convert.ToString(objQuickEnroll.AadharVerificationType));
                                HttpContext.Session.SetString("Otherverification", Convert.ToString(objQuickEnroll.Otherverification));
                                TempData["msg"] = "Saved Successfully";

                                return Json(MobResp);
                            }

                        }
                        else if (MobResp == "1")
                        {
                            TempData["CustMob"] = QEMobileNo;
                            HttpContext.Session.SetString("MobileNo", QEMobileNo);
                            HttpContext.Session.SetString("EmailId", QEEmailId);

                            string CustDetailsId = reader["CustDetailsId"].ToString();
                            string QUICK = reader["IsQuickEnrollSubmit"].ToString();
                            string CAF = reader["IsCAFSubmit"].ToString();
                            string DOC = reader["IsDocumentSubmit"].ToString();
                            string SUMMARY = reader["IssummarysheetSubmit"].ToString();
                            string SAVEACC = reader["isSavingAcc"].ToString();
                            string digilocker = reader["IsDigilocker"].ToString();

                            string cus = HttpContext.Session.GetString("PersonalId12");


                            if (cus != null)
                            {
                                HttpContext.Session.SetString("PersonalId", cus);
                            }
                            else
                            {
                                HttpContext.Session.SetString("PersonalId", CustDetailsId);

                                if (QUICK == "True" && digilocker == "True" && CAF == "True" && DOC == "True" && SUMMARY == "True" && SAVEACC == "False")
                                {
                                    //return RedirectToAction("CustomerAccForm", "CustomerAccount");
                                    return Json("please click on the Saving Acc");
                                }
                                else if (QUICK == "True" && digilocker == "True" && CAF == "True" && DOC == "True" && SUMMARY == "False")
                                {
                                    //return RedirectToAction("SummerySheetDetails", "DataVerify");
                                    return Json("please click on the SummerySheet");
                                }
                                else if (QUICK == "True" && digilocker == "True" && CAF == "True" && DOC == "False" && SUMMARY == "False")
                                {
                                    //return RedirectToAction("CustomerDocumentDetails", "DocumentDetails");
                                    return Json("please click on the document details");
                                }
                                else if (QUICK == "True" && digilocker == "True" && CAF == "" && DOC == "False" && SUMMARY == "False")
                                {
                                    //return RedirectToAction("DigitalQuickEnrollment", "KYCQuickEnroll");
                                    return Json("please click on the CAF");
                                }
                                else if (QUICK == "True" && digilocker == "" && CAF == "" && DOC == "False" && SUMMARY == "False")
                                {
                                    //return RedirectToAction("DigitalQuickEnrollment", "KYCQuickEnroll");
                                    return Json("please click on the digilocker");
                                }
                                else if (QUICK == "True" && digilocker == "True" && CAF == "True" && DOC == "True" && SUMMARY == "True" && SAVEACC == "True")
                                {
                                    HttpContext.Session.SetString("MobileNo", QEMobileNo);
                                    HttpContext.Session.SetString("EmailId", QEEmailId);
                                    TempData["CustMob"] = QEMobileNo;
                                    string SelectedValue = QEVType;
                                    if (SelectedValue == "P1")
                                    {
                                        objQuickEnroll.Cust_VerificationType = "Pan";
                                        objQuickEnroll.QEpanNo = QEVTypeTextbox;
                                    }
                                    if (SelectedValue == "DL")
                                    {
                                        objQuickEnroll.Cust_VerificationType = "Driving Licence";
                                        objQuickEnroll.QEdrivingLicenceNo = QEVTypeTextbox;
                                    }
                                    if (SelectedValue == "A")
                                    {
                                        objQuickEnroll.Cust_VerificationType = "Aadhaar";
                                        objQuickEnroll.QEaadhaarNo = QEVTypeTextbox;
                                    }
                                    if (SelectedValue == "P")
                                    {
                                        objQuickEnroll.Cust_VerificationType = "Passport";
                                        objQuickEnroll.QEpassportNo = QEVTypeTextbox;
                                    }
                                    if (SelectedValue == "V")
                                    {
                                        objQuickEnroll.Cust_VerificationType = "Voter ID";
                                        objQuickEnroll.QEvoterId = QEVTypeTextbox;
                                    }


                                    string strEncryptSessionkey;
                                    strEncryptSessionkey = ObjTripleDes.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + QEMobileNo.ToString()).Trim();

                                    using (SqlConnection connection2 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd2 = new SqlCommand("USP_InsertSignupOfCutomerNew", connection2);
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@firstName", QEFirstName);
                                        cmd2.Parameters.AddWithValue("@lastName", QELastName);
                                        cmd2.Parameters.AddWithValue("@Emailid", QEEmailId);
                                        cmd2.Parameters.AddWithValue("@mobileNo", QEMobileNo);
                                        cmd2.Parameters.AddWithValue("@panNo", objQuickEnroll.QEpanNo);
                                        cmd2.Parameters.AddWithValue("@AadhaarNo", objQuickEnroll.QEaadhaarNo);
                                        cmd2.Parameters.AddWithValue("@voterId", objQuickEnroll.QEvoterId);
                                        cmd2.Parameters.AddWithValue("@passportNo", objQuickEnroll.QEpassportNo);
                                        cmd2.Parameters.AddWithValue("@DrivingLicenceNo", objQuickEnroll.QEdrivingLicenceNo);
                                        cmd2.Parameters.AddWithValue("@Cust_SessionToken", strEncryptSessionkey);
                                        cmd2.Parameters.AddWithValue("@FacilitatorCode", "1");
                                        cmd2.Parameters.AddWithValue("@IsAssistedCustFlag", false);
                                        cmd2.Parameters.AddWithValue("@Cust_VerificationType", objQuickEnroll.Cust_VerificationType);

                                        connection2.Open();
                                        int result = cmd2.ExecuteNonQuery();
                                        connection2.Close();

                                        var results = "";
                                        if (result > 0)
                                        {
                                            SqlCommand cmd3 = new SqlCommand("getcustomerdetailid", connection2);
                                            cmd3.CommandType = CommandType.StoredProcedure;
                                            DataTable dt = new DataTable();
                                            SqlDataAdapter dp = new SqlDataAdapter(cmd3);
                                            connection2.Open();
                                            dp.Fill(dt);
                                            SqlDataReader reader3 = cmd3.ExecuteReader();

                                            if (reader3.Read())
                                            {
                                                results = reader3["CustDetailsId"].ToString();
                                            }
                                            connection2.Close();

                                        }

                                        if (results != null)
                                        {
                                            HttpContext.Session.SetString("QEPersonalId", Convert.ToString(results));

                                            HttpContext.Session.SetString("PersonalId", results);

                                            string custid = HttpContext.Session.GetString("PersonalId");
                                            ViewBag.QECustId = HttpContext.Session.GetString("PersonalId");
                                            objQuickEnroll.QECustDetailsId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                                            ViewBag.QECustDetailsId = objQuickEnroll.QECustDetailsId;

                                            string message = "Customer  Enrolled Successfully";
                                            //string message = "Customer  Enrolled Successfully Refrence No is " + "" + custid;

                                            return Json(message);
                                        }
                                        else
                                        {
                                            string message = "unSuccess";

                                            return Json(message);
                                        }
                                    }
                                    if (MobResp != null)

                                    {
                                        if (objQuickEnroll.LivePhoto != null && objQuickEnroll.LivePhoto != "" && objQuickEnroll.LivePhoto != "undefined")
                                        {
                                            objQuickEnroll.livecameraphoto = Convert.FromBase64String(objQuickEnroll.LivePhoto);
                                        }
                                        else
                                        {
                                            objQuickEnroll.livecameraphoto = null;
                                        }

                                        if (HttpContext.Session.GetString("NSDL_PANNumber") != null && HttpContext.Session.GetString("PersonalId") != null)
                                        {
                                            long? PResult = objDetails.Database.ExecuteSqlRaw($"USP_InsertSignupOfCutomerNew {(Convert.ToString(HttpContext.Session.GetString("NSDL_PANNumber")))},{(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}");

                                            long? PP = objDetails.Database.ExecuteSqlRaw($"USP_UpdateCustPanFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}, {true}");
                                        }

                                        string DOB = objQuickEnroll.DateOfBirth;

                                        var result = objDetails.AdmCkycCustomerDetails.FromSqlRaw($"USP_CustomerExistsCheck {(objQuickEnroll.FirstName)},{(objQuickEnroll.LastName)},{(objQuickEnroll.DateOfBirth)},{(objQuickEnroll.PANNumber)},{(objQuickEnroll.AadharNo)},{(objQuickEnroll.VoterId)},{(objQuickEnroll.Passport)},{(objQuickEnroll.DrivingLicence)},{""}");

                                        HttpContext.Session.SetString("KYCverificationType", Convert.ToString(objQuickEnroll.KYCverificationType));
                                        HttpContext.Session.SetString("PanVerificationType", Convert.ToString(objQuickEnroll.PanVerificationType));
                                        HttpContext.Session.SetString("CKYCVerificationType", Convert.ToString(objQuickEnroll.CKYCVerificationType));
                                        HttpContext.Session.SetString("AadharVerificationType", Convert.ToString(objQuickEnroll.AadharVerificationType));
                                        HttpContext.Session.SetString("Otherverification", Convert.ToString(objQuickEnroll.Otherverification));
                                        TempData["msg"] = "Saved Successfully";

                                        return Json(MobResp);
                                    }


                                }
                                else
                                {
                                    return View();
                                }

                            }

                            //Dropout(CustDetailsId, QUICK, CAF, DOC, SUMMARY);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
            return View();
            //return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
        }

        public ActionResult GetCBSData(string selectedValue, string appType, ClsCustQuickEnrollment objQuickEnroll)
        {
            ErrorLog error_log = new ErrorLog();

            try
            {
                if (selectedValue != null && appType != null)
                {
                    if (selectedValue == "Pan Number")
                    {
                        var pan = appType;
                        var adhaar = "";
                        var client = new RestClient("https://cbsintegration.azurewebsites.net/api/AadharandPan?PanNo=" + pan + "&AdharNo=" + adhaar);

                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        IRestResponse response = client.Execute(request);
                        string Result = response.Content;

                        Result = Result.Replace(@"\", "");
                        Result = Result.Substring(1, Result.Length - 2);
                        dedupe objroot1 = JsonConvert.DeserializeObject<dedupe>(Result);
                        string dedupe = objroot1.Msg;
                        if (dedupe != "Customer NOT Found ")
                        {
                            dcumentdedupe.Root objroot = JsonConvert.DeserializeObject<dcumentdedupe.Root>(Result);
                            string s = objroot.Name;
                            string s1 = s.Split(' ')[0];
                            string s2 = s.Split(' ')[1];
                            string s3 = s.Split(' ')[2];
                            objQuickEnroll.FirstNameReKyc = s1;
                            objQuickEnroll.MiddleNameReKyc = s2;
                            objQuickEnroll.LastNameReKyc = s3;
                            objQuickEnroll.CustomerNo = objroot.CustNo;
                            string CbsID = objroot.CustNo;
                            HttpContext.Session.SetString("CustIDRekyc", CbsID);
                            objQuickEnroll.MobileReKyc = objroot.Mobile;
                            objQuickEnroll.EmailIdReKyc = objroot.EmailID;
                            objQuickEnroll.DateOfBirthReKyc = objroot.DOB;
                            objQuickEnroll.GenderReKyc = objroot.SexCode;
                            objQuickEnroll.ReKyc_AddressLine1 = objroot.Add1;
                            objQuickEnroll.ReKyc_AddressLine2 = objroot.Add2;
                            objQuickEnroll.ReKyc_AddressLine3 = "";
                            objQuickEnroll.ReKyc_City = objroot.District;
                            objQuickEnroll.ReKyc_PinCode = objroot.PinCode;
                            objQuickEnroll.State = objroot.State;
                            objQuickEnroll.Country = objroot.Nationality;
                            //return Json(objQuickEnroll);

                        }
                    }
                    else if (selectedValue == "Aadhaar Number")
                    {
                        var pan = "";
                        var adhaar = appType;
                        var client = new RestClient("https://cbsintegration.azurewebsites.net/api/AadharandPan?PanNo=" + pan + "&AdharNo=" + adhaar);

                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        IRestResponse response = client.Execute(request);
                        string Result = response.Content;

                        Result = Result.Replace(@"\", "");
                        Result = Result.Substring(1, Result.Length - 2);
                        dedupe objroot1 = JsonConvert.DeserializeObject<dedupe>(Result);
                        string dedupe = objroot1.Msg;
                        if (dedupe != "Customer NOT Found ")
                        {
                            dcumentdedupe.Root objroot = JsonConvert.DeserializeObject<dcumentdedupe.Root>(Result);
                            string s = objroot.Name;
                            string s1 = s.Split(' ')[0];
                            string s2 = s.Split(' ')[1];
                            //string s3 = s.Split(' ')[2];
                            objQuickEnroll.FirstNameReKyc = s1;
                            objQuickEnroll.MiddleNameReKyc = s2;
                            objQuickEnroll.LastNameReKyc = s2;
                            objQuickEnroll.CustomerNo = objroot.CustNo;
                            objQuickEnroll.MobileReKyc = objroot.Mobile;
                            objQuickEnroll.EmailIdReKyc = objroot.EmailID;
                            objQuickEnroll.DateOfBirthReKyc = objroot.DOB;
                            objQuickEnroll.GenderReKyc = objroot.SexCode;
                            objQuickEnroll.ReKyc_AddressLine1 = objroot.Add1;
                            objQuickEnroll.ReKyc_AddressLine2 = objroot.Add2;
                            objQuickEnroll.ReKyc_AddressLine3 = "";
                            objQuickEnroll.ReKyc_City = objroot.District;
                            objQuickEnroll.ReKyc_PinCode = objroot.PinCode;
                            objQuickEnroll.State = objroot.State;
                            objQuickEnroll.Country = objroot.Nationality;
                            // return Json(objQuickEnroll);
                        }
                    }
                    if (objQuickEnroll.CustomerNo != null)
                    {

                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("Usp_toInsertRekycCustomerDetails", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.AddWithValue("@CustomerNo", objQuickEnroll.CustomerNo);
                            cmd2.Parameters.AddWithValue("@CustomerFirstname", objQuickEnroll.FirstNameReKyc);
                            cmd2.Parameters.AddWithValue("@CustomerMiddlename", objQuickEnroll.MiddleNameReKyc);
                            cmd2.Parameters.AddWithValue("@CustomerLastname", objQuickEnroll.LastNameReKyc);
                            cmd2.Parameters.AddWithValue("@Customer_Mobno", objQuickEnroll.MobileReKyc);
                            cmd2.Parameters.AddWithValue("@CustomerEmailID", objQuickEnroll.EmailIdReKyc);
                            cmd2.Parameters.AddWithValue("@customerDOB", objQuickEnroll.DateOfBirthReKyc);
                            cmd2.Parameters.AddWithValue("@CustomerGender", objQuickEnroll.GenderReKyc);
                            cmd2.Parameters.AddWithValue("@CustomerAdd1", objQuickEnroll.ReKyc_AddressLine1);
                            cmd2.Parameters.AddWithValue("@CustomerAdd2", objQuickEnroll.ReKyc_AddressLine2);
                            cmd2.Parameters.AddWithValue("@CustomerAdd3", objQuickEnroll.ReKyc_AddressLine3);
                            cmd2.Parameters.AddWithValue("@CustomerCity", objQuickEnroll.ReKyc_City);
                            cmd2.Parameters.AddWithValue("@CustomerPincode", objQuickEnroll.ReKyc_PinCode);
                            cmd2.Parameters.AddWithValue("@CustomerState", "MH"/*objQuickEnroll.ReKyc_State*/);
                            cmd2.Parameters.AddWithValue("@CustomerCountryID", "101"/* objQuickEnroll.ReKyc_CountryCode*/);
                            cmd2.Parameters.AddWithValue("@CustomerAnualincome", "");
                            cmd2.Parameters.AddWithValue("@CustomerOccupation", "");


                            connection2.Open();
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            if (reader2.Read())
                            {

                                var Result1 = reader2["RESULT"].ToString();
                            }
                        }
                        return Json(objQuickEnroll);

                    }
                }
                else
                {
                    return Json("KYC has been done already for the given accNo/CustomerNo");
                }




            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                // System.IO.File.AppendAllText(file, DateTime.Now + "---" + ex.Message+"--"+ex.Source+"--"+ex.StackTrace);
                return Json(ex.Message);
            }
            return Json("Failed");

            //return View();
        }


        public ActionResult GetCBSData2(string selectedValue, string appType)
        {
            if (selectedValue == "Mobile Number")
            {
                ErrorLog error_log = new ErrorLog();
                DEDUPE_GRID_MAIN objmain = new DEDUPE_GRID_MAIN();
                try
                {
                    List<dedupegridlist> dedupegridlist = new List<dedupegridlist>();

                    if (appType != null)
                    {
                        var client = new RestClient("https://cbs.indofinnet.com/api/MobileNumber_CBS?MobileNo=" + appType);

                        client.Timeout = -1;
                        var request = new RestRequest(Method.GET);
                        IRestResponse response = client.Execute(request);
                        string Result = response.Content;

                        Result = Result.Replace(@"\", "");
                        Result = Result.Substring(1, Result.Length - 2);
                        String msg = Result.Split(":")[2];
                        msg = msg.Replace(@"\", "");
                        var objroot = JsonConvert.DeserializeObject<List<dedupemob.Root>>(Result);
                        if (objroot != null)
                        {
                            if (objroot.Count > 0)
                            {

                                for (int i = 0; i < objroot.Count; i++)
                                {
                                    var data = objroot[i];
                                    if (data.custNo == 0)
                                    {
                                        string msg1 = msg.Split('"')[1];
                                        return Json(msg1);
                                    }
                                    dedupegridlist dedupegridobjmodel = new dedupegridlist();
                                    dedupegridobjmodel.custNo = data.custNo;

                                    dedupegridobjmodel.name = data.name;


                                    string pan = null;

                                    if (data.panNo != null && data.panNo != "" && data.panNo.Length == 10)
                                    {
                                        //aadhaar1 = objEncrypyDecrypt.Encrypt(adharcardNo);
                                        pan = "XXXX" + data.panNo.Substring(4, 6);
                                    }

                                    dedupegridobjmodel.panNo = pan;//data.panNo;

                                    dedupegridobjmodel.DedupeRadioFlag = Convert.ToString(i);
                                    dedupegridobjmodel.DedupeRadioFlag = "DedupeRadioFlag" + i;

                                    dedupegridlist.Add(dedupegridobjmodel);
                                }
                                objmain.dedupegridlists = dedupegridlist;
                            }
                            return PartialView("Views/KYCQuickEnroll/_GetCBSData.cshtml", objmain);
                        }
                        else
                        {
                            return Json("Server Error Occurred");
                        }

                    }
                }
                catch (Exception ex)
                {
                    error_log.WriteErrorLog(ex.ToString());
                    return Json(ex.Message);
                }
            }

            return View();
        }


        public ActionResult GetAccCBSData(string AccValue, ClsCustQuickEnrollment objQuickEnroll)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return PartialView(/*objINDO_FinNet.USP_GetFinacleintegrationData(AccValue).ToList()*/"");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
        }

        [HttpPost]

        public async Task<JsonResult> GetCustomerKYCData(ClsCustQuickEnrollment objQuickEnroll, string zippassword, string uploadfiles, string fileName)
        {
            ErrorLog error_log = new ErrorLog();

            string applicationId = "f9fc8ae7-55fb-4473-98e7-c08a61374aa6";

            string applicationSecret = "nNU95a9UB~BgH05r20yTT_WYYa.1K5F~f0";
            var keyClient = new KeyVaultClient(async (authority, resource, scope) =>
            {
                var adCredential = new ClientCredential(applicationId, applicationSecret);
                var authenticationContext = new AuthenticationContext(authority, null);
                return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
            });
            if (HttpContext.Session.GetString("DACustMOBNo") != null)
            {
                if (!clsCustAuthorize.IsAuthCustomerDetails(Convert.ToInt64(ObjTripleDes.Decrypt(HttpContext.Session.GetString("EncryPersonalId").ToString())), HttpContext.Session.GetString("SessionOrgKey").ToString()))
                {
                    return Json("Cust Session Expired");

                }
            }

            byte[] filexml = null;
            string xmlData = "";
            var path = "";
            try
            {
                string? OrgID = "Alpha01";
                string? ApiKey = "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE";
                string[] filetype = uploadfiles.Split('.');
                string Referencenumber = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 14);

                HttpContext.Session.SetString("Referencenumber", Referencenumber);

                if (filetype[1] == "xml")
                {
                    xmlData = System.Text.Encoding.ASCII.GetString(filexml);
                }
                if (filetype[1] == "zip")
                {
                    IFormFile file = Request.Form.Files[0];
                    fileName = file.FileName;
                    byte[] XmlFile;
                    using (var binaryReader = new BinaryReader(file.OpenReadStream()))
                    {
                        XmlFile = binaryReader.ReadBytes((int)file.Length);
                    }
                    var client = new RestClient("https://apigateway.indofinnet.com/api/GetCustomerOfflineXMLKYCData?OrgID=" + OrgID + "&Password=" + zippassword);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("ApiKey", ApiKey);

                    request.AddFile("ZipFile", XmlFile, file.FileName, file.ContentType);

                    IRestResponse response = client.Execute(request);
                    var IdResp = response.Content;
                   
                    string idresp = IdResp.Trim('\"');
                    if(idresp == "The password did not match.")
                    {
                        return Json("Password Does Not match");
                    }
                    dynamic xmlfiledata = JsonConvert.DeserializeObject(IdResp);
                    dynamic xmlfiledata1 = JsonConvert.DeserializeObject(xmlfiledata);
                    if (xmlfiledata1.statuscode == "200")
                    {
                        string fullname = xmlfiledata1.name;

                        var name = fullname.Split(' ');
                        objQuickEnroll.AadharNo = "";
                        objQuickEnroll.FirstName = name[0];
                        objQuickEnroll.MiddleName = name[1];
                        objQuickEnroll.LastName = name[2];
                        objQuickEnroll.DateOfBirth = xmlfiledata1.dob;
                        objQuickEnroll.Gender = xmlfiledata1.gender;
                        objQuickEnroll.Country = xmlfiledata1.country;
                        objQuickEnroll.State = xmlfiledata1.state;
                        objQuickEnroll.Street = xmlfiledata1.street;
                        objQuickEnroll.Locality = xmlfiledata1.vtc;
                        objQuickEnroll.House = xmlfiledata1.house;
                        objQuickEnroll.District = xmlfiledata1.district;
                        objQuickEnroll.Pincode = xmlfiledata1.postalcode;


                        objQuickEnroll.EnrollMobileNo = HttpContext.Session.GetString("MobileNo");
                        objQuickEnroll.EnrollEmailId = HttpContext.Session.GetString("EmailId");
                        objQuickEnroll.Address = objQuickEnroll.House + "," + objQuickEnroll.Street + "," + objQuickEnroll.Pincode + "," + objQuickEnroll.Locality + "," + objQuickEnroll.District;
                        objQuickEnroll.CustomerPhoto = (xmlfiledata1.photo);
                        objQuickEnroll.XMLStatus = xmlfiledata1.status;
                        objQuickEnroll.XMLReferenceID = xmlfiledata1.referenceid;
                        objQuickEnroll.EmailId = xmlfiledata1.email;
                        objQuickEnroll.TFForVerif = true;

                        string conn = _connectionString;
                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand cmd = new SqlCommand("USP_CustomerAdharExistsCheck", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@AdharName", fullname);
                            cmd.Parameters.AddWithValue("@dateOfBirth", objQuickEnroll.DateOfBirth);

                            connection.Open();
                            SqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                var resultA = reader["RESULT"].ToString();
                                if (resultA != "USEREXISTS")
                                {

                                    using (SqlConnection connection2 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd2 = new SqlCommand("USP_InsertAadharDetailsWithCustId", connection2);
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@AadharNumber", "");
                                        cmd2.Parameters.AddWithValue("@AadharName", fullname);
                                        cmd2.Parameters.AddWithValue("@AadharGender", objQuickEnroll.Gender);
                                        cmd2.Parameters.AddWithValue("@AadharDOB", objQuickEnroll.DateOfBirth);
                                        cmd2.Parameters.AddWithValue("@AadharEmail", objQuickEnroll.EmailId);
                                        cmd2.Parameters.AddWithValue("@AadharMobile", "");
                                        cmd2.Parameters.AddWithValue("@AadharPhoto", objQuickEnroll.CustomerPhoto);
                                        cmd2.Parameters.AddWithValue("@AadharAddress", objQuickEnroll.Address);
                                        cmd2.Parameters.AddWithValue("@CreatedBy", "");
                                        //cmd2.Parameters.AddWithValue("@CreatedBy", ObjTripleDes.Decrypt(Convert.ToString(HttpContext.Session.GetString("OrgUserId"))));
                                        cmd2.Parameters.AddWithValue("@verificationType", "XML");
                                        cmd2.Parameters.AddWithValue("@Vtc", objQuickEnroll.Locality);
                                        cmd2.Parameters.AddWithValue("@Subdist", objQuickEnroll.KYC_SubDistrict);
                                        cmd2.Parameters.AddWithValue("@Street", objQuickEnroll.Street);
                                        cmd2.Parameters.AddWithValue("@state", objQuickEnroll.State);

                                        cmd2.Parameters.AddWithValue("@Po", objQuickEnroll.Pincode);
                                        cmd2.Parameters.AddWithValue("@Pc", objQuickEnroll.Pincode);
                                        cmd2.Parameters.AddWithValue("@Locality", objQuickEnroll.Locality);
                                        cmd2.Parameters.AddWithValue("@House", objQuickEnroll.House);
                                        cmd2.Parameters.AddWithValue("@district", objQuickEnroll.District);
                                        cmd2.Parameters.AddWithValue("@country", objQuickEnroll.Country);
                                        cmd2.Parameters.AddWithValue("@XMLReferenceID", objQuickEnroll.XMLReferenceID);
                                        cmd2.Parameters.AddWithValue("@ReferenceNumber", Referencenumber);
                                        cmd2.Parameters.AddWithValue("@TFForVerif", objQuickEnroll.TFForVerif);
                                        cmd2.Parameters.AddWithValue("@OrganizationID", 1);
                                        cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                        string s = HttpContext.Session.GetString("PersonalId");
                                        HttpContext.Session.SetString("IsAadharXml", s);

                                        connection2.Open();
                                        SqlDataReader reader1 = cmd2.ExecuteReader();
                                        if (reader1.Read())
                                        {
                                            using (SqlConnection connection3 = new SqlConnection(conn))
                                            {
                                                SqlCommand cmd3 = new SqlCommand("USP_UpdateCustAadharFlag", connection3);
                                                cmd3.CommandType = CommandType.StoredProcedure;
                                                cmd3.Parameters.AddWithValue("@CustId", HttpContext.Session.GetString("PersonalId"));
                                                cmd3.Parameters.AddWithValue("@isAadharVerify", true);
                                                connection3.Open();
                                                SqlDataReader reader3 = cmd3.ExecuteReader();
                                                if (reader1.Read())
                                                {
                                                    // var uFlag = reader["RESULT"].ToString();
                                                }
                                            }

                                            using (SqlConnection connection12 = new SqlConnection(conn))
                                            {
                                                SqlCommand cmd4 = new SqlCommand("USP_CustomerAadharXmlFlag", connection12);
                                                cmd4.CommandType = CommandType.StoredProcedure;
                                                cmd4.Parameters.AddWithValue("@CustId", HttpContext.Session.GetString("PersonalId"));
                                                connection12.Open();
                                                SqlDataReader reader3 = cmd4.ExecuteReader();
                                                if (reader1.Read())
                                                {
                                                    // var uFlag = reader["RESULT"].ToString();
                                                }
                                            }

                                            if (reader1 != null)
                                            {
                                                objQuickEnroll.XMLStatus = "Success";
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    var xmlDatadetails = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetCustomerDetailsByReference{(Convert.ToString(HttpContext.Session.GetString("XMLReferenceID")))}");
                                    objQuickEnroll.XMLStatus = "Record Already  Exists";
                                }
                            }
                            connection.Close();
                        }
                    }
                    else if (xmlfiledata1.statuscode == "300")
                    {
                        return Json("300");
                    }

                }

                return Json(objQuickEnroll);
            }
            catch (Exception ex)
            {

                error_log.WriteErrorLog(ex.ToString());
                return Json("Record Already Exists");
            }
        }


        public ActionResult FetchDoc(string Verificationtxt3, string Verificationtxt4, string Verificationtxt6, string Verificationtxt8, bool DrivingLic, bool PanCard, bool Aadharxml,bool Offline_AADHAAR_XML, bool IsPanVerify)
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
                string url = "https://apigateway.indofinnet.com/api/Digilocker?RequestId=123&OrgID=IndoFin007&ApiKey=IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE";
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

        [HttpGet]
        public ActionResult DigitalQuickEnrollment([FromServices] IActiveLogin objLogin, string msg)
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
            string proceedwithOCR = null; long? PersonalId = null; string shareAadharNumber = null;
            string IsREkyctrue = HttpContext.Session.GetString("CustIDRekyc");
            string IsAadharVerify = HttpContext.Session.GetString("IsAadharXml");
            string IsPanVerifyData = HttpContext.Session.GetString("VerifyPan");
            var result11 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();

            if (IsAadharVerify != null || result11.IsAadharXmlDone!=null)
            {
                ViewBag.IsAadharVerify = "Yes";


            }
            else
            {
                ViewBag.IsAadharVerify = "No";
            }
            if (IsPanVerifyData != null || result11.IsPanVerify!=null)
            {
                ViewBag.IsPanVerifyData = "Yes";


            }
            else
            {
                ViewBag.IsPanVerifyData = "No";
            }


            if (IsREkyctrue != null)
            {
                ViewBag.ISrekycTrue = "Yes";
                string ABC = "true";
                HttpContext.Session.SetString("RekycImg", ABC);

            }
            else
            {
                ViewBag.ISrekycTrue = "No";
            }
            string IsCustExist = HttpContext.Session.GetString("ExistCustID");
            if (IsCustExist != null)
            {
                ViewBag.IsCustExist = "Yes";
            }
            else
            {
                ViewBag.IsCustExist = "No";
            }
            var progressbar = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
            if (progressbar.IsQuickEnrollSubmit == true)
            {
                if (progressbar.IsDigiAadharSumbitted == true || progressbar.IsAadharXmlDone == true)
                {
                    ViewBag.DigiAdharDOB = true;
                }
                ViewBag.progressbarq = 1;
            }
            else { }

            if (progressbar.IsCAFSubmit == true)
            {
                var progressbarcaf = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_CAFProgressbardata {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                objDigitalKYCdata.Digi_FirstName = progressbarcaf.FirstName;
                //objDigitalKYCdata.Digi_MiddleName = progressbarcaf.Middlename;

                objDigitalKYCdata.Digi_LastName = progressbarcaf.LastName;
                objDigitalKYCdata.Digi_gender = progressbarcaf.Gender;
                objDigitalKYCdata.Digi_DOB = progressbarcaf.Dob;

                objDigitalKYCdata.CLIENT_ADDRESS_1 = progressbarcaf.ClientAddress1;
                objDigitalKYCdata.CLIENT_PERM_ADDRESS_1 = progressbarcaf.ClientPermAddress1;
                objDigitalKYCdata.CLIENT_ADDRESS_2 = progressbarcaf.ClientAddress2;
                objDigitalKYCdata.CLIENT_PERM_ADDRESS_2 = progressbarcaf.ClientPermAddress2;
                objDigitalKYCdata.CLIENT_ADDRESS_3 = progressbarcaf.ClientAddress3;
                objDigitalKYCdata.CLIENT_PERM_ADDRESS_3 = progressbarcaf.ClientPermAddress3;
                objDigitalKYCdata.CLIENT_CITY = progressbarcaf.CityId;
                objDigitalKYCdata.CLIENT_PERM_CITY = progressbarcaf.CityId;
                objDigitalKYCdata.MobileNo = progressbarcaf.MobileNo;
                objDigitalKYCdata.Digi_PAN =  ObjTripleDes.Decrypt(progressbarcaf.PanNo);
                objDigitalKYCdata.Pincode = progressbarcaf.PinCode;
                objDigitalKYCdata.CLIENT_PERM_Pincode = progressbarcaf.ClientPermCountry;
                //objDigitalKYCdata.CLIENT_COUNTRY = progressbarcaf.CountryId;
                string conn1 = _connectionString;
                using (SqlConnection connection12 = new SqlConnection(conn1))
                {
                    SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection12);
                    cmd12.CommandType = CommandType.StoredProcedure;

                    cmd12.Parameters.AddWithValue("@CountryCode", progressbarcaf.CountryId);
                    connection12.Open();
                    SqlDataReader reader1 = cmd12.ExecuteReader();
                    if (reader1.Read())
                    {


                        var Country = reader1[2].ToString();
                        objDigitalKYCdata.CLIENT_COUNTRY = Country;
                    }
                }
                //objDigitalKYCdata.CLIENT_PERM_COUNTRY = progressbarcaf.CountryId;
                using (SqlConnection connection12 = new SqlConnection(conn1))
                {
                    SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection12);
                    cmd12.CommandType = CommandType.StoredProcedure;

                    cmd12.Parameters.AddWithValue("@CountryCode", progressbarcaf.CountryId);
                    connection12.Open();
                    SqlDataReader reader1 = cmd12.ExecuteReader();
                    if (reader1.Read())
                    {


                        var Country = reader1[2].ToString();
                        objDigitalKYCdata.CLIENT_PERM_COUNTRY = Country;
                    }
                }
                objDigitalKYCdata.EmailId = progressbarcaf.EmailId;
                objDigitalKYCdata.CasteCd = progressbarcaf.CasteCd;
                objDigitalKYCdata.SubTitle = progressbarcaf.SubTitle;
                objDigitalKYCdata.Religion = progressbarcaf.Religion;
                objDigitalKYCdata.maritalstatus = progressbarcaf.maritalstatus;
                objDigitalKYCdata.Residence = progressbarcaf.Residence;
                objDigitalKYCdata.ResidentialStatus = progressbarcaf.ResidentialStatus;
                objDigitalKYCdata.residenceYN = progressbarcaf.residenceYN;
                objDigitalKYCdata.CLIENT_STATE = "Maharashtra";//progressbarcaf.StateId;
                //using (SqlConnection connection12 = new SqlConnection(conn1))
                //{
                //    SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection12);
                //    cmd12.CommandType = CommandType.StoredProcedure;

                //    cmd12.Parameters.AddWithValue("@State_Code", progressbarcaf.StateId);
                //    connection12.Open();
                //    SqlDataReader reader1 = cmd12.ExecuteReader();
                //    if (reader1.Read())
                //    {


                //        var state = reader1[2].ToString();
                //        objDigitalKYCdata.CLIENT_STATE = state;
                //    }
                //}
                //objDigitalKYCdata.CLIENT_PERM_STATE = progressbarcaf.ClientPermState;
                using (SqlConnection connection12 = new SqlConnection(conn1))
                {
                    SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection12);
                    cmd12.CommandType = CommandType.StoredProcedure;

                    cmd12.Parameters.AddWithValue("@State_Code", progressbarcaf.ClientPermState);
                    connection12.Open();
                    SqlDataReader reader1 = cmd12.ExecuteReader();
                    if (reader1.Read())
                    {


                        var state = reader1[2].ToString();
                        objDigitalKYCdata.CLIENT_PERM_STATE = state;
                    }
                }
                objDigitalKYCdata.ResidenceDocument = progressbarcaf.ResidenceDocument;



            }
            else
            {

            }


            try
            {
                if (HttpContext.Session.GetString("UserRole") == "1")
                {
                    var Response2 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetKYCCustomerDetails {(Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId")))}").AsEnumerable().FirstOrDefault();
                    var StateCode = Response2.StateId;

                    var Response3 = objDetails.StateCodes.FromSqlRaw($"USP_IDTOSTATE {StateCode}").AsEnumerable().FirstOrDefault();

                    string conn9 = _connectionString;
                    using (SqlConnection connection12 = new SqlConnection(conn9))
                    {
                        SqlCommand cmd25 = new SqlCommand("USP_IDTOMobileDetails", connection12);
                        cmd25.CommandType = CommandType.StoredProcedure;

                        cmd25.Parameters.AddWithValue("@EmailMobile_code", Response2.MobileDetails);
                        connection12.Open();
                        SqlDataReader reader1 = cmd25.ExecuteReader();
                        if (reader1.Read())
                        {


                            var MobileDetails_description = reader1[0].ToString();
                        }
                        string conn10 = _connectionString;
                        using (SqlConnection connection13 = new SqlConnection(conn10))
                        {
                            SqlCommand cmd26 = new SqlCommand("USP_IDTOPB", connection13);
                            cmd26.CommandType = CommandType.StoredProcedure;

                            cmd26.Parameters.AddWithValue("@PB_code", Response2.PhoneBanking);
                            connection13.Open();
                            SqlDataReader reader2 = cmd26.ExecuteReader();
                            if (reader2.Read())
                            {


                                var PB_description = reader2[0].ToString();
                            }
                            var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (result.IsAadharXmlDone == true)
                            {
                                var AadharData = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetCustomerAdharDetailsByCustId {Convert.ToInt64(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();

                                if (AadharData != null)
                                {
                                    string s = AadharData.AadharName;
                                    string[] Name = s.Split(' ');
                                    objDigitalKYCdata.Digi_FirstName = Name[0];
                                    objDigitalKYCdata.Digi_MiddleName = Name[1];
                                    objDigitalKYCdata.Digi_LastName = Name[2];
                                    objDigitalKYCdata.Digi_gender = AadharData.AadharGender;
                                    objDigitalKYCdata.Digi_DOB = AadharData.AadharDob;

                                    objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress;
                                    objDigitalKYCdata.CLIENT_PERM_ADDRESS_1 = AadharData.AadharAddress;

                                    objDigitalKYCdata.CLIENT_CITY = AadharData.Locality;
                                    objDigitalKYCdata.CLIENT_PERM_CITY = AadharData.Street;
                                    objDigitalKYCdata.MobileNo = AadharData.AadharMobile;
                                    objDigitalKYCdata.CLIENT_STATE = "Maharashtra";//AadharData.State;
                                    string conn1 = _connectionString;
                                    //using (SqlConnection connection125 = new SqlConnection(conn1))
                                    //{
                                    //    SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection125);
                                    //    cmd12.CommandType = CommandType.StoredProcedure;

                                    //    cmd12.Parameters.AddWithValue("@State_Code", AadharData.State);
                                    //    connection125.Open();
                                    //    SqlDataReader reader11 = cmd12.ExecuteReader();
                                    //    if (reader11.Read())
                                    //    {


                                    //        var state = reader11[2].ToString();
                                    //        objDigitalKYCdata.CLIENT_PERM_STATE = state;
                                    //    }
                                    //}
                                    // objDigitalKYCdata.CLIENT_PERM_STATE = AadharData.State;
                                    objDigitalKYCdata.Pincode = AadharData.PinCode;
                                    objDigitalKYCdata.CLIENT_PERM_Pincode = AadharData.PinCode;
                                    //objDigitalKYCdata.CLIENT_COUNTRY = AadharData.Country;
                                    //objDigitalKYCdata.CLIENT_PERM_COUNTRY = AadharData.Country;
                                    using (SqlConnection connection125 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection125);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@CountryCode", AadharData.Country);
                                        connection125.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {


                                            var Country = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_COUNTRY = Country;
                                        }
                                    }
                                    using (SqlConnection connection125 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection125);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@CountryCode", AadharData.Country);
                                        connection125.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {


                                            var Country = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_PERM_COUNTRY = Country;
                                        }
                                    }


                                    objDigitalKYCdata.MobileDetails = reader1[0].ToString();
                                    objDigitalKYCdata.PhoneBanking = reader2[0].ToString();



                                    if (AadharData.AadharPhoto != null)
                                    {
                                        objDigitalKYCdata.LivePhoto = AadharData.AadharPhoto;
                                    }
                                }

                            }
                            else if (result.IsDrivingLicenceDone == true)
                            {

                                if (Response2 != null)
                                {

                                    objDigitalKYCdata.Digi_FirstName = Response2.FirstName;
                                    objDigitalKYCdata.Digi_MiddleName = Response2.MiddleName;
                                    objDigitalKYCdata.Digi_LastName = Response2.LastName;
                                    objDigitalKYCdata.Digi_gender = Response2.Gender;
                                    objDigitalKYCdata.Digi_DOB = Response2.Dob;

                                    objDigitalKYCdata.CLIENT_ADDRESS_1 = Response2.ClientAddress1;
                                    objDigitalKYCdata.CLIENT_PERM_ADDRESS_1 = Response2.ClientPermAddress1;

                                    objDigitalKYCdata.CLIENT_CITY = Response2.ClientPermCity;
                                    objDigitalKYCdata.CLIENT_PERM_CITY = Response2.ClientPermCity;
                                    objDigitalKYCdata.MobileNo = Response2.MobileNo;
                                    objDigitalKYCdata.CLIENT_STATE = "Maharashtra";//Response2.StateId;
                                    objDigitalKYCdata.CLIENT_PERM_STATE = "Maharashtra";//Response2.StateId;
                                    string conn1 = _connectionString;
                                    //using (SqlConnection connection156 = new SqlConnection(conn1))
                                    //{
                                    //    SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection156);
                                    //    cmd12.CommandType = CommandType.StoredProcedure;

                                    //    cmd12.Parameters.AddWithValue("@State_Code", Response2.StateId);
                                    //    connection156.Open();
                                    //    SqlDataReader reader11 = cmd12.ExecuteReader();
                                    //    if (reader11.Read())
                                    //    {


                                    //        var state = reader11[2].ToString();
                                    //        objDigitalKYCdata.CLIENT_STATE = state;
                                    //    }
                                    //}
                                    //using (SqlConnection connection156 = new SqlConnection(conn1))
                                    //{
                                    //    SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection156);
                                    //    cmd12.CommandType = CommandType.StoredProcedure;

                                    //    cmd12.Parameters.AddWithValue("@State_Code", Response2.StateId);
                                    //    connection156.Open();
                                    //    SqlDataReader reader11 = cmd12.ExecuteReader();
                                    //    if (reader11.Read())
                                    //    {


                                    //        var state = reader11[2].ToString();
                                    //        objDigitalKYCdata.CLIENT_PERM_STATE = state;
                                    //    }
                                    //}
                                    objDigitalKYCdata.Pincode = ObjTripleDes.Decrypt(Response2.PinCode);
                                    objDigitalKYCdata.CLIENT_PERM_Pincode = Response2.ClientPermPin;
                                    //objDigitalKYCdata.CLIENT_COUNTRY = Response2.CountryId;
                                    //objDigitalKYCdata.CLIENT_PERM_COUNTRY = Response2.ClientPermCountry;
                                    using (SqlConnection connection125 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection125);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@CountryCode", Response2.CountryId);
                                        connection125.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {


                                            var Country = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_COUNTRY = Country;
                                        }
                                    }
                                    using (SqlConnection connection125 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection125);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@CountryCode", Response2.ClientPermCountry);
                                        connection125.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {


                                            var Country = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_PERM_COUNTRY = Country;
                                        }
                                    }

                                    objDigitalKYCdata.MobileDetails = reader1[0].ToString();
                                    objDigitalKYCdata.PhoneBanking = reader2[0].ToString();
                                    objDigitalKYCdata.AMLRating = Response2.AMLRating;




                                }
                            }
                            else if (result.IsPanVerify == true || result.IsPanDone == true)
                            {

                                var Response12 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetKYCCustomerDetails {Convert.ToInt64(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();

                                if (Response12 != null)
                                {

                                    objDigitalKYCdata.Digi_FirstName = Response12.FirstName;
                                    objDigitalKYCdata.Digi_MiddleName = Response12.MiddleName;
                                    objDigitalKYCdata.Digi_LastName = Response12.LastName;
                                    objDigitalKYCdata.Digi_gender = Response12.Gender;
                                    objDigitalKYCdata.Digi_DOB = Response12.Dob;

                                    objDigitalKYCdata.CLIENT_ADDRESS_1 = Response12.ClientAddress1;
                                    objDigitalKYCdata.CLIENT_PERM_ADDRESS_1 = Response12.ClientPermAddress1;

                                    objDigitalKYCdata.CLIENT_CITY = Response12.ClientPermCity;
                                    objDigitalKYCdata.CLIENT_PERM_CITY = Response12.ClientPermCity;
                                    objDigitalKYCdata.MobileNo = Response12.MobileNo;
                                    //objDigitalKYCdata.CLIENT_STATE = Response12.StateId;
                                    //objDigitalKYCdata.CLIENT_PERM_STATE = Response12.ClientPermState;
                                    string conn1 = _connectionString;
                                    using (SqlConnection connection156 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection156);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@State_Code", Response12.ClientPermState);
                                        connection156.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {
                                            var state = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_STATE = state;
                                        }
                                    }
                                    using (SqlConnection connection156 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection156);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@State_Code", Response12.ClientPermState);
                                        connection156.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {
                                            var state = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_PERM_STATE = state;
                                        }
                                    }
                                    objDigitalKYCdata.Pincode = Response12.PinCode;
                                    objDigitalKYCdata.CLIENT_PERM_Pincode = Response12.ClientPermPin;
                                    //objDigitalKYCdata.CLIENT_COUNTRY = Response12.CountryId;
                                    //objDigitalKYCdata.CLIENT_PERM_COUNTRY = Response12.CountryId;
                                    using (SqlConnection connection125 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection125);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@CountryCode", Response12.CountryId);
                                        connection125.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {


                                            var Country = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_COUNTRY = Country;
                                        }
                                    }
                                    using (SqlConnection connection125 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection125);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@CountryCode", Response12.CountryId);
                                        connection125.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {


                                            var Country = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_PERM_COUNTRY = Country;
                                        }
                                    }

                                    objDigitalKYCdata.MobileDetails = reader1[0].ToString();
                                    objDigitalKYCdata.PhoneBanking = reader2[0].ToString();
                                    objDigitalKYCdata.AMLRating = Response12.AMLRating;
                                }

                            }
                            else
                            {
                                var Response5 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {(Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId")))}").AsEnumerable().FirstOrDefault();
                                if (Response2 != null && HttpContext.Session.GetString("UserRole") == "1")
                                {
                                    objDigitalKYCdata.Digi_FirstName = Response2.FirstName;
                                    objDigitalKYCdata.Digi_MiddleName = Response5.Middlename;

                                    objDigitalKYCdata.Digi_LastName = Response2.LastName;
                                    objDigitalKYCdata.Digi_gender = Response2.Gender;
                                    objDigitalKYCdata.Digi_DOB = Response2.Dob;

                                    objDigitalKYCdata.CLIENT_ADDRESS_1 = Response2.ClientAddress1;
                                    objDigitalKYCdata.CLIENT_PERM_ADDRESS_1 = Response2.ClientPermAddress1;
                                    objDigitalKYCdata.CLIENT_ADDRESS_2 = Response2.ClientAddress2;
                                    objDigitalKYCdata.CLIENT_PERM_ADDRESS_2 = Response2.ClientPermAddress2;
                                    objDigitalKYCdata.CLIENT_ADDRESS_3 = Response2.ClientAddress3;
                                    objDigitalKYCdata.CLIENT_PERM_ADDRESS_3 = Response2.ClientPermAddress3;
                                    objDigitalKYCdata.CLIENT_CITY = Response2.CityId;
                                    objDigitalKYCdata.CLIENT_PERM_CITY = Response2.CityId;
                                    objDigitalKYCdata.MobileNo = Response2.MobileNo;

                                    objDigitalKYCdata.Digi_PAN = ObjTripleDes.Decrypt(Response2.PanNo);
                                    //objDigitalKYCdata.CLIENT_STATE = Response3.StateCode1;
                                    //objDigitalKYCdata.CLIENT_PERM_STATE = Response3.StateCode1;
                                    string conn1 = _connectionString;
                                    using (SqlConnection connection156 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection156);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@State_Code", Response3.StateCode1);
                                        connection156.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {
                                            var state = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_STATE = state;
                                        }
                                    }
                                    using (SqlConnection connection156 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetStateName", connection156);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@State_Code", Response3.StateCode1);
                                        connection156.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {
                                            var state = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_PERM_STATE = state;
                                        }
                                    }
                                    objDigitalKYCdata.Pincode = Response2.PinCode;
                                    objDigitalKYCdata.CLIENT_PERM_Pincode = Response2.PinCode;
                                    //objDigitalKYCdata.CLIENT_COUNTRY = Response2.CountryId;
                                    //objDigitalKYCdata.CLIENT_PERM_COUNTRY = Response2.ClientPermCountry;
                                    using (SqlConnection connection125 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection125);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@CountryCode", Response2.CountryId);
                                        connection125.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {


                                            var Country = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_COUNTRY = Country;
                                        }
                                    }
                                    using (SqlConnection connection125 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd12 = new SqlCommand("USP_GetCountryId", connection125);
                                        cmd12.CommandType = CommandType.StoredProcedure;

                                        cmd12.Parameters.AddWithValue("@CountryCode", Response2.ClientPermCountry);
                                        connection125.Open();
                                        SqlDataReader reader11 = cmd12.ExecuteReader();
                                        if (reader11.Read())
                                        {


                                            var Country = reader11[2].ToString();
                                            objDigitalKYCdata.CLIENT_PERM_COUNTRY = Country;
                                        }
                                    }
                                    objDigitalKYCdata.EmailId = Response2.EmailId;
                                    objDigitalKYCdata.CasteCd = Response2.CasteCd;
                                    objDigitalKYCdata.SubTitle = Response2.SubTitle;
                                    objDigitalKYCdata.Religion = Response2.Religion;
                                    objDigitalKYCdata.maritalstatus = Response2.maritalstatus;
                                    objDigitalKYCdata.Residence = Response2.Residence;
                                    objDigitalKYCdata.ResidentialStatus = Response2.ResidentialStatus;
                                    objDigitalKYCdata.residenceYN = Response2.residenceYN;


                                    objDigitalKYCdata.ResidenceDocument = Response2.ResidenceDocument;


                                    objDigitalKYCdata.AMLRating = Response2.AMLRating;

                                    objDigitalKYCdata.MobileDetails = reader1[0].ToString();
                                    objDigitalKYCdata.PhoneBanking = reader2[0].ToString();
                                }
                            }

                        }




                    }
                }
                string EmailId = Convert.ToString(HttpContext.Session.GetString("EmailId"));
                if (EmailId != "")
                {
                    ViewBag.HideEmailId = "Hide";
                }
                else
                {
                    ViewBag.HideEmailId = "NotHide";

                }
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    var sesspi = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    HttpContext.Session.SetString("PersonalId", sesspi);
                    ViewBag.AdminFlag = "AdminFlag";
                }
                if (HttpContext.Session.GetString("MobileNo") != null)
                //if (TempData["CustMob"] != null)
                {
                    string MoNo = Convert.ToString((HttpContext.Session.GetString("MobileNo")));
                    //ViewBag.custMoNo = MoNo;
                    if (MoNo.Length == 12)
                    {
                        var mm = MoNo.Substring(2);
                        ViewBag.custMoNo = mm;
                    }
                    else
                    {
                        ViewBag.custMoNo = MoNo; ;
                    }
                    HttpContext.Session.SetString("MobileNo", MoNo);

                    var qdata = objDetails.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsByMobNo {MoNo}").AsEnumerable().FirstOrDefault();
                    if (qdata != null)
                    {
                        objDigitalKYCdata.Digi_FirstName = qdata.CustFirstName;
                        objDigitalKYCdata.Digi_LastName = qdata.CustLastName;
                        objDigitalKYCdata.EmailId = qdata.CustEmailId;
                        ViewBag.EmailId = objDigitalKYCdata.EmailId;
                        objDigitalKYCdata.MobileNo = qdata.CustMobileNo;
                        if (qdata.CustMobileNo.Length == 12)
                        {
                            var mm = qdata.CustMobileNo.Substring(2);
                            objDigitalKYCdata.MobileNo = mm;
                        }
                        else
                        {
                            objDigitalKYCdata.MobileNo = qdata.CustMobileNo;
                        }
                        objDigitalKYCdata.Digi_PAN = qdata.PanNo;
                        objDigitalKYCdata.Digi_Voter = qdata.VoterId;
                        objDigitalKYCdata.Digi_passport = qdata.PassportNo;
                        objDigitalKYCdata.Digi_Drivinglicense = qdata.DrivingLicenceNo;
                        objDigitalKYCdata.Digi_Aadhar = qdata.AadhaarNo;
                    }
                }
                if (Convert.ToString(HttpContext.Session.GetString("KYCverificationType")) != "")
                {
                    var Aadhardate = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();

                    //var Aadhardate = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharDetailsByCustId {HttpContext.Session.GetString("PersonalId")}").AsEnumerable().FirstOrDefault();
                }
                objDigitalKYCdata.PanVerificationType = Convert.ToBoolean(HttpContext.Session.GetString("PanVerificationType"));

                var result2 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {Convert.ToInt64(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();

                objDigitalKYCdata.CKYCVerificationType = Convert.ToBoolean(HttpContext.Session.GetString("CKYCVerificationType"));
                objDigitalKYCdata.AadharVerificationType = Convert.ToBoolean(HttpContext.Session.GetString("AadharVerificationType"));
                objDigitalKYCdata.Otherverification = Convert.ToBoolean(HttpContext.Session.GetString("Otherverification"));
                objDigitalKYCdata.DigilockerType = Convert.ToString(HttpContext.Session.GetString("DigilockerType"));
                ViewBag.IsDigiPANSumbitted = result2.IsDigiPansumbitted;
                ViewBag.IsDigilDRLCSumbitted = result2.IsDigilDrlcsumbitted;
                ViewBag.IsDigiAadharSumbitted = result2.IsDigiAadharSumbitted;
                ViewBag.IsAdharXmlSubmitted = result2.IsAadharXmlDone;
                HttpContext.Session.SetString("AccountForm", "");
                if (TempData["msg"] != null)
                {
                    if (msg == "1")
                    {
                        ViewBag.msg = "";
                    }
                    else
                    {
                        ViewBag.msg = TempData["msg"];
                        TempData.Keep();
                    }
                }
                var procc = "";
                var aa = "";
                if (proceedwithOCR != null)
                {
                    procc = proceedwithOCR;
                }
                else
                {
                    procc = "";
                }
                if (shareAadharNumber != null)
                {
                    aa = shareAadharNumber;
                }
                else
                {
                    aa = "";
                }
                HttpContext.Session.SetString("proceedwithOCR", procc);
                HttpContext.Session.SetString("shareAadharNumber", aa);
                objDigitalKYCdata.PersonalId = Convert.ToInt64(HttpContext.Session.GetString(("PersonalId")));
                if (objDigitalKYCdata.PersonalId > 0)
                {

                    var result1 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {objDigitalKYCdata.PersonalId}").AsEnumerable().FirstOrDefault();
                    var Aadhardate1 = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharDetailsByCustId {Convert.ToInt64(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();

                    if (Aadhardate1 != null)
                    {
                        objDigitalKYCdata.KYCverificationType = Aadhardate1.VerificationType;
                    }
                    if (result1 != null)
                    {
                        ViewBag.IsQuickEnrollSubmit = result1.IsQuickEnrollSubmit;
                        ViewBag.isshareAadharNumber = result1.ShareAadharNumber;
                        objDigitalKYCdata.shareAadharNumber = result1.ShareAadharNumber;
                        var SAN = result1.ShareAadharNumber;
                        if (result1.ShareAadharNumber != null)
                        {
                            SAN = result1.ShareAadharNumber;
                        }
                        else
                        {
                            SAN = "";
                        }
                        HttpContext.Session.SetString("shareAadharNumber", SAN);
                    }
                }

                if (objDigitalKYCdata.PersonalId > 0)
                {
                    var panNO = "";
                    if (objDigitalKYCdata.Digi_PAN != null)
                    {
                        panNO = objDigitalKYCdata.Digi_PAN;
                    }
                    else
                    {
                        panNO = null;
                    }
                    var phonebank = "";
                    if (objDigitalKYCdata.PhoneBanking != null)
                    {
                        phonebank = objDigitalKYCdata.PhoneBanking;
                    }
                    else
                    {
                        phonebank = null;
                    }

                    ViewBag.PersonalId = objDigitalKYCdata.PersonalId;

                }
                ViewBag.Gender = new SelectList(new[] { new { ID = "M", Value = "Male" }, new { ID = "F", Value = "Female" }, new { ID = "O", Value = "Others" } }, "ID", "Value");
                ViewBag.AMLRating = new SelectList(new[] { new { ID = "3", Value = "High Risk" }, new { ID = "2", Value = "Mid Risk" }, new { ID = "1", Value = "Low Risk" } }, "ID", "Value");
                ViewBag.Gender_status = new SelectList(new[] { new { ID = "M", Value = "Male" }, new { ID = "F", Value = "Female" } }, "ID", "Value");
                var verificationtype4 = (from details in objDetails.IndoKycEmailMobileDetails.FromSqlRaw($"USP_INDO_KYC_GetEmailMobileDetails").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Description.ToString(),

                                             Value = details.EmailMobileCode,
                                         }).ToList();
                verificationtype4.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.getEmailMobile = verificationtype4;



                var verificationtype = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State").ToList()
                                        select new SelectListItem()
                                        {
                                            Text = details.StateName.ToString(),
                                            Value = details.StateCode1,
                                        }).ToList();
                verificationtype.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.State = verificationtype;
                var verificationtype1 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Country.ToString(),
                                             Value = details.CountryCode,

                                         }).ToList();
                verificationtype1.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.getCountry = verificationtype1;
                var verificationtype2 = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.StateName.ToString(),
                                             Value = details.StateCode1,

                                         }).ToList();
                verificationtype2.Insert(0, new SelectListItem()
                {
                    //Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.per_State = verificationtype2;

                var verificationtype3 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Country.ToString(),
                                             Value = details.CountryCode,

                                         }).ToList();
                verificationtype3.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.per_Country = verificationtype3;

                var verificationtype6 = (from details in objDetails.admnames.FromSqlRaw($"USP_Get_Name").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.NAME_description.ToString(),
                                             Value = details.NAME_description.ToString(),

                                         }).ToList();
                verificationtype6.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.per_name = verificationtype6;

                var verificationtype7 = (from details in objDetails.admCASTDetails.FromSqlRaw($"USP_Get_CASTDetails").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.CAST_description.ToString(),
                                             Value = details.CAST_code.ToString(),

                                         }).ToList();
                verificationtype7.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.cast = verificationtype7;

                var verificationtype8 = (from details in objDetails.adm_ReligionDetails.FromSqlRaw($"USP_Get_ReligionDetails").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Religion_description.ToString(),
                                             Value = details.Religion_code.ToString(),

                                         }).ToList();
                verificationtype8.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.Religion = verificationtype8;

                var verificationtype9 = (from details in objDetails.adm_maritalstatuss.FromSqlRaw($"USP_Get_maritalstatus").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.maritalstatus_description.ToString(),
                                             Value = details.maritalstatus_code.ToString(),

                                         }).ToList();
                verificationtype9.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.maritalstatus = verificationtype9;

                var verificationtype10 = (from details in objDetails.adm_residences.FromSqlRaw($"USP_Get_residence").ToList()
                                          select new SelectListItem()
                                          {
                                              Text = details.residence_description.ToString(),
                                              Value = details.residence_code.ToString(),

                                          }).ToList();
                verificationtype10.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.residence = verificationtype10;

                var verificationtype11 = (from details in objDetails.adm_residencedocuments.FromSqlRaw($"USP_Get_residencedocument").ToList()
                                          select new SelectListItem()
                                          {
                                              Text = details.document_description.ToString(),
                                              Value = details.document_code.ToString(),

                                          }).ToList();
                verificationtype11.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.residencedocument = verificationtype11;
                ViewBag.ResidentialStatusDetail = new SelectList(new[] { new { ID = "Resident Individual", Value = "Resident Individual" }, new { ID = "Non Resident Indian", Value = "Non Resident Indian" }, new { ID = "Foreign National", Value = "Foreign National" }, new { ID = "Person of Indian Origin", Value = "Person of Indian Origin" } }, "ID", "Value");


                var verificationtype12 = (from details in objDetails.adm_residenceYNs.FromSqlRaw($"USP_Get_residenceYN").ToList()
                                          select new SelectListItem()
                                          {
                                              Text = details.residenceYN_description.ToString(),
                                              Value = details.residenceYN_code.ToString(),

                                          }).ToList();
                verificationtype12.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.residenceYN = verificationtype12;

                var verificationtype13 = (from details in objDetails.adm_phonebankings.FromSqlRaw($"USP_Get_phonebanking").ToList()
                                          select new SelectListItem()
                                          {
                                              Text = details.PB_description.ToString(),
                                              Value = details.PB_code.ToString(),

                                          }).ToList();
                verificationtype13.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.phonebanking = verificationtype13;
                return View(objDigitalKYCdata);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                // PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        [HttpPost]
        public JsonResult CheckUser(string digi_pan)
        {

            ClsDocDetails objFinalDoc = new ClsDocDetails();
            string conn = _connectionString;
            using (SqlConnection connection = new SqlConnection(conn))
            {
                SqlCommand cmd6 = new SqlCommand("Usp_AgentExistornot", connection);
                cmd6.CommandType = CommandType.StoredProcedure;

                cmd6.Parameters.AddWithValue("@panNo", digi_pan);

                connection.Open();
                SqlDataReader reader = cmd6.ExecuteReader();

                if (reader.Read())
                {
                    var result = reader["result"].ToString();
                    if (result != "1")
                    {
                        TempData["msg"] = "Submitted Successfully";
                    }
                    else
                    {
                        TempData["msg"] = "ALREADY EXIST";
                    }
                }
            }
            return Json(TempData["msg"]);
        }

        //*******RKYC*****//

        public ActionResult GetREKYCDATA(string msg, long AdminCustId, ClsDigitalKYC1 ClsDigitalKYC1)
        {
            ErrorLog error_log = new ErrorLog();
            string IsREkyctrue = HttpContext.Session.GetString("PersonalId");
            var rekycview = "1";
            HttpContext.Session.SetString("rekycVIEW", rekycview);
            string proceedwithOCR = null; long? PersonalId = null; string shareAadharNumber = null;
            if (IsREkyctrue != null)
            {
                ViewBag.ISrekycTrue = "Yes";
            }
            else
            {
                ViewBag.ISrekycTrue = "No";
            }
            try
            {
                if (HttpContext.Session.GetString("UserRole") == "1")
                {
                    var Response2 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetKYCCustomerDetails {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                    var StateCode = Response2.StateId;
                    ClsDigitalKYC1.DRekycustomerFirstname = Response2.FirstName;
                    ClsDigitalKYC1.DRekycustomerMiddlename = Response2.MiddleName;
                    ClsDigitalKYC1.DRekycustomerLastname = Response2.LastName;
                    ClsDigitalKYC1.DRekycustomerDOB = Response2.Dob;
                    ClsDigitalKYC1.DRekycustomerGEnder = Response2.Gender;
                    ClsDigitalKYC1.DRekycustomerAdd1 = Response2.ClientAddress1;
                    ClsDigitalKYC1.DRekycustomerAdd2 = Response2.ClientAddress2;
                    ClsDigitalKYC1.DRekycustomerAdd3 = Response2.ClientAddress3;
                    ClsDigitalKYC1.DRekycustomerCity = Response2.Vtc;
                    ClsDigitalKYC1.DRekycustomerPincode = Response2.PinCode;
                    ClsDigitalKYC1.DRekycustomerState = Response2.StateId;
                    ClsDigitalKYC1.DRekycustomerCountryID = Response2.CountryId;
                    var Response5 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                    string conn = _connectionString;
                    using (SqlConnection connection3 = new SqlConnection(conn))
                    {
                        SqlCommand cmd3 = new SqlCommand("USP_GetAdminRKYC", connection3);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        connection3.Open();
                        SqlDataReader reader3 = cmd3.ExecuteReader();
                        if (reader3.Read())
                        {
                            var Result = reader3[0].ToString();
                            var Result1 = reader3[1].ToString();

                        }
                        string CustnoFromCBS = reader3[0].ToString();
                        string Ac_No = reader3[1].ToString();
                        var RekyCustomerDetails = objDetails.Set<AdmRekycCustomerDetail>().FromSqlRaw("EXEC usp_TogetRekycCustomerDetails @CustomerNo={0}, @Ac_no={1}", CustnoFromCBS, Ac_No).AsEnumerable().FirstOrDefault(); ClsDigitalKYC1.RekycustomerNO = RekyCustomerDetails.CustomerNo;
                        ClsDigitalKYC1.RekycustomerFirstname = RekyCustomerDetails.CustomerFirstname;
                        ClsDigitalKYC1.RekycustomerMiddlename = RekyCustomerDetails.CustomerMiddlename;
                        ClsDigitalKYC1.RekycustomerLastname = RekyCustomerDetails.CustomerLastname;
                        ClsDigitalKYC1.RekycustomerMobileno = RekyCustomerDetails.Customer_Mobno;
                        ClsDigitalKYC1.RekycustomerEmailID = RekyCustomerDetails.CustomerEmailID;
                        ClsDigitalKYC1.RekycustomerDOB = RekyCustomerDetails.customerDOB;
                        ClsDigitalKYC1.RekycustomerGEnder = RekyCustomerDetails.CustomerGender;
                        ClsDigitalKYC1.RekycustomerAdd1 = RekyCustomerDetails.CustomerAdd1;
                        ClsDigitalKYC1.RekycustomerAdd2 = RekyCustomerDetails.CustomerAdd2;
                        ClsDigitalKYC1.RekycustomerAdd3 = RekyCustomerDetails.CustomerAdd3;
                        ClsDigitalKYC1.RekycustomerCity = RekyCustomerDetails.CustomerCity;
                        ClsDigitalKYC1.RekycustomerPincode = RekyCustomerDetails.CustomerPincode;
                        if (RekyCustomerDetails.CustomerState == "027")
                        {
                            ClsDigitalKYC1.RekycustomerState = "MH";
                        }
                        else
                        {
                            ClsDigitalKYC1.RekycustomerState = "MH";
                        }
                        if (RekyCustomerDetails.CustomerCountryID == "101")
                        {
                            ClsDigitalKYC1.RekycustomerCountryID = "IN";
                        }
                        else
                        {
                            ClsDigitalKYC1.RekycustomerCountryID = "IN";
                        }
                        ClsDigitalKYC1.RekycustomerAnnualIncome = RekyCustomerDetails.CustomerAnualincome;
                        ClsDigitalKYC1.RekycustomerOccupation = RekyCustomerDetails.CustomerOccupation;
                        if (Response2 != null && HttpContext.Session.GetString("UserRole") == "1")
                        {
                            objDigitalKYCdata.Digi_FirstName = Response2.FirstName;
                            objDigitalKYCdata.Digi_MiddleName = Response2.MiddleName;
                            objDigitalKYCdata.Digi_LastName = Response2.LastName;
                            objDigitalKYCdata.Digi_gender = Response2.Gender;
                            objDigitalKYCdata.Digi_DOB = Response2.Dob;
                            objDigitalKYCdata.CLIENT_ADDRESS_1 = Response2.ClientAddress1;
                            objDigitalKYCdata.CLIENT_PERM_ADDRESS_1 = Response2.ClientAddress1;
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = Response2.ClientAddress2;
                            objDigitalKYCdata.CLIENT_PERM_ADDRESS_2 = Response2.ClientAddress2;
                            objDigitalKYCdata.CLIENT_CITY = Response2.CityId;
                            objDigitalKYCdata.CLIENT_PERM_CITY = Response2.CityId;
                            objDigitalKYCdata.MobileNo = Response2.MobileNo;
                            objDigitalKYCdata.Digi_PAN = Response2.PanNo;
                            objDigitalKYCdata.CLIENT_STATE = Response2.ClientPermState;
                            objDigitalKYCdata.CLIENT_PERM_STATE = Response2.ClientPermState;
                            objDigitalKYCdata.Pincode = Response2.PinCode;
                            objDigitalKYCdata.CLIENT_PERM_Pincode = Response2.PinCode;
                            objDigitalKYCdata.CLIENT_COUNTRY = Response2.CountryId;
                            objDigitalKYCdata.CLIENT_PERM_COUNTRY = Response2.CountryId;
                            objDigitalKYCdata.EmailId = Response2.EmailId;
                        }
                        ClsDigitalKYC1.FirstnameStatus = string.Equals(Response2.FirstName.ToLower(), RekyCustomerDetails.CustomerFirstname.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.fnamerekyc = ClsDigitalKYC1.FirstnameStatus;
                        ClsDigitalKYC1.MiddlenameStatus = string.Equals(Response2.MiddleName.ToLower(), RekyCustomerDetails.CustomerMiddlename.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.Mnamerekyc = ClsDigitalKYC1.MiddlenameStatus;
                        ClsDigitalKYC1.LastnameStatus = string.Equals(Response2.LastName.ToLower(), RekyCustomerDetails.CustomerLastname.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.Lnamerekyc = ClsDigitalKYC1.LastnameStatus;
                        ClsDigitalKYC1.DobStatus = string.Equals(Response2.Dob.ToLower(), RekyCustomerDetails.customerDOB.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.DobStatus = ClsDigitalKYC1.DobStatus;
                        ClsDigitalKYC1.GenderStatus = string.Equals(Response2.Gender.ToLower(), RekyCustomerDetails.CustomerGender.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.GenderStatus = ClsDigitalKYC1.GenderStatus;
                        ClsDigitalKYC1.CountryStatus = string.Equals(Response2.Country.ToLower(), /*RekyCustomerDetails.CustomerCountryID*/ClsDigitalKYC1.RekycustomerCountryID.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.CStatus = ClsDigitalKYC1.CountryStatus;
                        ClsDigitalKYC1.Pincode = string.Equals(Response2.PinCode.ToLower(), RekyCustomerDetails.CustomerPincode.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.Pincode = ClsDigitalKYC1.Pincode;
                        ClsDigitalKYC1.CLIENT_STATE = string.Equals(Response2.State.ToLower(), ClsDigitalKYC1.RekycustomerState /*RekyCustomerDetails.CustomerState*/.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.State = ClsDigitalKYC1.CLIENT_STATE;
                        ClsDigitalKYC1.CLIENT_ADDRESS_1 = string.Equals(Response2.ClientAddress1.ToLower(), RekyCustomerDetails.CustomerAdd1.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.CLIENT_ADDRESS_1 = ClsDigitalKYC1.CLIENT_ADDRESS_1;
                        ClsDigitalKYC1.CLIENT_ADDRESS_2 = string.Equals(Response2.ClientAddress2.ToLower(), RekyCustomerDetails.CustomerAdd2.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.CLIENT_ADDRESS_2 = ClsDigitalKYC1.CLIENT_ADDRESS_2;
                        ClsDigitalKYC1.CLIENT_ADDRESS_3 = string.Equals(Response2.ClientAddress3.ToLower(), RekyCustomerDetails.CustomerAdd3.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.CLIENT_ADDRESS_3 = ClsDigitalKYC1.CLIENT_ADDRESS_3;
                        ClsDigitalKYC1.CLIENT_CITY = string.Equals(Response2.Vtc.ToLower(), RekyCustomerDetails.CustomerCity.ToLower()) ? "Match" : "Does Not Match";
                        ViewBag.City = ClsDigitalKYC1.CLIENT_CITY;
                    }
                }
                string EmailId = Convert.ToString(HttpContext.Session.GetString("EmailId"));
                if (EmailId != "")
                {
                    ViewBag.HideEmailId = "Hide";
                }
                else
                {
                    ViewBag.HideEmailId = "NotHide";
                }
                ViewBag.AdminFlag = "AdminFlag";
                if (HttpContext.Session.GetString("MobileNo") != null)
                {
                    string MoNo = Convert.ToString((HttpContext.Session.GetString("MobileNo")));
                    ViewBag.custMoNo = MoNo;
                    HttpContext.Session.SetString("MobileNo", MoNo);
                    var qdata = objDetails.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsByMobNo {MoNo}").AsEnumerable().FirstOrDefault();
                    if (qdata != null)
                    {
                        objDigitalKYCdata.Digi_FirstName = qdata.CustFirstName;
                        objDigitalKYCdata.Digi_LastName = qdata.CustLastName;
                        objDigitalKYCdata.EmailId = qdata.CustEmailId;
                        ViewBag.EmailId = objDigitalKYCdata.EmailId;
                        objDigitalKYCdata.MobileNo = qdata.CustMobileNo;
                        objDigitalKYCdata.Digi_PAN = qdata.PanNo;
                        objDigitalKYCdata.Digi_Voter = qdata.VoterId;
                        objDigitalKYCdata.Digi_passport = qdata.PassportNo;
                        objDigitalKYCdata.Digi_Drivinglicense = qdata.DrivingLicenceNo;
                        objDigitalKYCdata.Digi_Aadhar = qdata.AadhaarNo;
                    }
                }
                if (Convert.ToString(HttpContext.Session.GetString("KYCverificationType")) != "")
                {
                    var Aadhardate = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharByCustId {HttpContext.Session.GetString("PersonalId")}").AsEnumerable().FirstOrDefault();
                }
                if (Convert.ToString(HttpContext.Session.GetString("NSDL_PANNumber")) != "")
                {
                    objDigitalKYCdata.Digi_PAN = Convert.ToString(HttpContext.Session.GetString("NSDL_PANNumber"));
                }
                objDigitalKYCdata.PanVerificationType = Convert.ToBoolean(HttpContext.Session.GetString("PanVerificationType"));
                var result2 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {Convert.ToInt64(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();
                objDigitalKYCdata.CKYCVerificationType = Convert.ToBoolean(HttpContext.Session.GetString("CKYCVerificationType"));
                objDigitalKYCdata.AadharVerificationType = Convert.ToBoolean(HttpContext.Session.GetString("AadharVerificationType"));
                objDigitalKYCdata.Otherverification = Convert.ToBoolean(HttpContext.Session.GetString("Otherverification"));
                objDigitalKYCdata.DigilockerType = Convert.ToString(HttpContext.Session.GetString("DigilockerType"));
                ViewBag.IsDigiPANSumbitted = result2.IsDigiPansumbitted;
                ViewBag.IsDigilDRLCSumbitted = result2.IsDigilDrlcsumbitted;
                ViewBag.IsDigiAadharSumbitted = result2.IsDigiAadharSumbitted;
                HttpContext.Session.SetString("AccountForm", "");
                if (TempData["msg"] != null)
                {
                    if (msg == "1")
                    {
                        ViewBag.msg = "";
                    }
                    else
                    {
                        ViewBag.msg = TempData["msg"];
                        TempData.Keep();
                    }
                }
                var procc = "";
                var aa = "";
                if (proceedwithOCR != null)
                {
                    procc = proceedwithOCR;
                }
                else
                {
                    procc = "";
                }
                if (shareAadharNumber != null)
                {
                    aa = shareAadharNumber;
                }
                else
                {
                    aa = "";
                }
                HttpContext.Session.SetString("proceedwithOCR", procc);
                HttpContext.Session.SetString("shareAadharNumber", aa);
                objDigitalKYCdata.PersonalId = Convert.ToInt64(HttpContext.Session.GetString(("PersonalId")));
                if (PersonalId > 0)
                {

                    objDigitalKYCdata.PersonalId = PersonalId;
                }
                var result1 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {objDigitalKYCdata.PersonalId}").AsEnumerable().FirstOrDefault();
                var Aadhardate1 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharByCustId {HttpContext.Session.GetString("PersonalId")}").AsEnumerable().FirstOrDefault();
                if (result1 != null)
                {
                    ViewBag.IsQuickEnrollSubmit = result1.IsQuickEnrollSubmit;
                    ViewBag.isshareAadharNumber = result1.ShareAadharNumber;
                    objDigitalKYCdata.shareAadharNumber = result1.ShareAadharNumber;
                    var SAN = result1.ShareAadharNumber;
                    if (result1.ShareAadharNumber != null)
                    {
                        SAN = result1.ShareAadharNumber;
                    }
                    else
                    {
                        SAN = "";
                    }
                    HttpContext.Session.SetString("shareAadharNumber", SAN);
                }
                if (objDigitalKYCdata.PersonalId > 0)
                {
                    var panNO = "";
                    if (objDigitalKYCdata.Digilock_PANNo != null)
                    {
                        panNO = objDigitalKYCdata.Digilock_PANNo;
                    }
                    else
                    {
                        panNO = null;
                    }
                    ViewBag.PersonalId = objDigitalKYCdata.PersonalId;
                }
                ViewBag.Gender = new SelectList(new[] { new { ID = "M", Value = "Male" }, new { ID = "F", Value = "Female" }, new { ID = "O", Value = "Others" } }, "ID", "Value");
                ViewBag.Gender_status = new SelectList(new[] { new { ID = "M", Value = "Male" }, new { ID = "F", Value = "Female" } }, "ID", "Value");
                var verificationtype4 = (from details in objDetails.IndoKycEmailMobileDetails.FromSqlRaw($"USP_INDO_KYC_GetEmailMobileDetails").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Description.ToString(),

                                             Value = details.EmailMobileCode,
                                         }).ToList();
                verificationtype4.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.getEmailMobile = verificationtype4;
                var verificationtype = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State").ToList()
                                        select new SelectListItem()
                                        {
                                            Text = details.StateName.ToString(),
                                            Value = details.StateCode1,
                                        }).ToList();
                verificationtype.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.State = verificationtype;
                var verificationtype1 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Country.ToString(),
                                             Value = details.CountryCode,
                                         }).ToList();
                verificationtype1.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.getCountry = verificationtype1;
                var verificationtype2 = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.StateName.ToString(),
                                             Value = details.StateCode1,

                                         }).ToList();
                verificationtype2.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.per_State = verificationtype2;
                var verificationtype3 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Country.ToString(),
                                             Value = details.CountryCode,
                                         }).ToList();
                verificationtype3.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.per_Country = verificationtype3;
                var verificationtype6 = (from details in objDetails.admnames.FromSqlRaw($"USP_Get_Name").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.NAME_description.ToString(),
                                             Value = details.NAME_code.ToString(),
                                         }).ToList();
                verificationtype6.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.per_name = verificationtype6;
                var verificationtype7 = (from details in objDetails.admCASTDetails.FromSqlRaw($"USP_Get_CASTDetails").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.CAST_description.ToString(),
                                             Value = details.CAST_code.ToString(),
                                         }).ToList();
                verificationtype7.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.cast = verificationtype7;
                var verificationtype8 = (from details in objDetails.adm_ReligionDetails.FromSqlRaw($"USP_Get_ReligionDetails").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.Religion_description.ToString(),
                                             Value = details.Religion_code.ToString(),

                                         }).ToList();
                verificationtype8.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.Religion = verificationtype8;
                var verificationtype9 = (from details in objDetails.adm_maritalstatuss.FromSqlRaw($"USP_Get_maritalstatus").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.maritalstatus_description.ToString(),
                                             Value = details.maritalstatus_code.ToString(),
                                         }).ToList();
                verificationtype9.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.maritalstatus = verificationtype9;
                var verificationtype10 = (from details in objDetails.adm_residences.FromSqlRaw($"USP_Get_residence").ToList()
                                          select new SelectListItem()
                                          {
                                              Text = details.residence_description.ToString(),
                                              Value = details.residence_code.ToString(),
                                          }).ToList();
                verificationtype10.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.residence = verificationtype10;
                var verificationtype11 = (from details in objDetails.adm_residencedocuments.FromSqlRaw($"USP_Get_residencedocument").ToList()
                                          select new SelectListItem()
                                          {
                                              Text = details.document_description.ToString(),
                                              Value = details.document_code.ToString(),

                                          }).ToList();
                verificationtype11.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.residencedocument = verificationtype11;
                var verificationtype12 = (from details in objDetails.adm_residenceYNs.FromSqlRaw($"USP_Get_residenceYN").ToList()
                                          select new SelectListItem()
                                          {
                                              Text = details.residenceYN_description.ToString(),
                                              Value = details.residenceYN_code.ToString(),
                                          }).ToList();
                verificationtype12.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.residenceYN = verificationtype12;
                var verificationtype13 = (from details in objDetails.adm_phonebankings.FromSqlRaw($"USP_Get_phonebanking").ToList()
                                          select new SelectListItem()
                                          {
                                              Text = details.PB_description.ToString(),
                                              Value = details.PB_code.ToString(),
                                          }).ToList();
                verificationtype13.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.phonebanking = verificationtype13;
                return View(ClsDigitalKYC1);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }

        [HttpPost]
        public ActionResult TosaveChangedKYCdata(/*long ChangedDetailCustID,*/ string ChangedDetailFirstname, string ChangedDetailMiddlename, string ChangedDetailLastname,/* string ChangedDetailMob, string ChangedDetailEmail,*/ string ChangedDetailDOB, string ChangedDetailGender, string ChangedDetailAdd1, string ChangedDetailAdd2, string ChangedDetailAdd3, string ChangedDetailCity, string ChangedDetailPincode, string ChangedDetailState, string ChangedDetailCountryID)
        {
            string cbsid = HttpContext.Session.GetString("CustIDRekyc");
            var AgentID = HttpContext.Session.GetString("UseID");
            using (SqlConnection cn = new SqlConnection(_connectionString))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("USP_AddCheckRekycData12", cn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustId", (HttpContext.Session.GetString("PersonalId")));
                cmd.Parameters.AddWithValue("@FirstName", ChangedDetailFirstname);
                cmd.Parameters.AddWithValue("@MiddleName", ChangedDetailMiddlename);
                cmd.Parameters.AddWithValue("@Lastname", ChangedDetailLastname);
                cmd.Parameters.AddWithValue("@DOB", ChangedDetailDOB);
                cmd.Parameters.AddWithValue("@Gender", ChangedDetailGender);
                cmd.Parameters.AddWithValue("@CLIENT_ADDRESS_1", ChangedDetailAdd1);
                cmd.Parameters.AddWithValue("@CLIENT_ADDRESS_2", ChangedDetailAdd2);
                cmd.Parameters.AddWithValue("@CLIENT_ADDRESS_3", ChangedDetailAdd3);
                cmd.Parameters.AddWithValue("@Vtc", ChangedDetailCity);
                cmd.Parameters.AddWithValue("@PinCode", ChangedDetailPincode);
                if (ChangedDetailState == "MH" || ChangedDetailCountryID == "IN")
                {
                    cmd.Parameters.AddWithValue("@StateId", "027");
                    cmd.Parameters.AddWithValue("@CountryId", "101");
                }
                cmd.Parameters.AddWithValue("@State", ChangedDetailState);
                cmd.Parameters.AddWithValue("@Country", ChangedDetailCountryID);
                cmd.Parameters.AddWithValue("@Cbsid", cbsid);
                HttpContext.Session.SetString("HideRB", cbsid);
                cmd.Parameters.AddWithValue("@CreatedBy", AgentID);
                cmd.Parameters.AddWithValue("@Acc_No", (HttpContext.Session.GetString("Acc_No")));
                ViewBag.HRButton = "True";
                cmd.ExecuteNonQuery();
                string conn = _connectionString;
                using (SqlConnection connection3 = new SqlConnection(conn))
                {
                    SqlCommand cmd3 = new SqlCommand("RKYCFlag", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                    connection3.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {
                        //var Result = reader2["RESULT"].ToString();
                    }
                }
                return Json("Re-KYC Saved Successfully");
            }
        }

        [HttpPost]
        public IActionResult DigilockerlFlag()
        {
            string conn = _connectionString;
            using (SqlConnection connection3 = new SqlConnection(conn))
            {
                SqlCommand cmd3 = new SqlCommand("USP_DigilockerlFlag", connection3);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                connection3.Open();
                SqlDataReader reader3 = cmd3.ExecuteReader();
                if (reader3.Read())
                {
                    //var Result = reader2["RESULT"].ToString();
                }
            }
            return View();
        }
        [HttpGet]
        public JsonResult GetRekycDatafromCBS(ClsDigitalKYC1 objdetailREkyc)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string CustnoFromCBS = HttpContext.Session.GetString("CustIDRekyc");
                string Ac_No = HttpContext.Session.GetString("Acc_No");
                var RekyCustomerDetails = objDetails.Set<AdmRekycCustomerDetail>().FromSqlRaw("EXEC usp_TogetRekycCustomerDetails @CustomerNo={0}, @Ac_no={1}", CustnoFromCBS, Ac_No).AsEnumerable().FirstOrDefault();
                objdetailREkyc.RekycustomerNO = RekyCustomerDetails.CustomerNo;
                objdetailREkyc.RekycustomerFirstname = RekyCustomerDetails.CustomerFirstname;
                objdetailREkyc.RekycustomerMiddlename = RekyCustomerDetails.CustomerMiddlename;
                objdetailREkyc.RekycustomerLastname = RekyCustomerDetails.CustomerLastname;
                objdetailREkyc.RekycustomerMobileno = RekyCustomerDetails.Customer_Mobno;
                objdetailREkyc.RekycustomerEmailID = RekyCustomerDetails.CustomerEmailID;
                objdetailREkyc.RekycustomerDOB = RekyCustomerDetails.customerDOB;
                objdetailREkyc.RekycustomerGEnder = RekyCustomerDetails.CustomerGender;
                objdetailREkyc.RekycustomerAdd1 = RekyCustomerDetails.CustomerAdd1;
                objdetailREkyc.RekycustomerAdd2 = RekyCustomerDetails.CustomerAdd2;
                objdetailREkyc.RekycustomerAdd3 = RekyCustomerDetails.CustomerAdd3;
                objdetailREkyc.RekycustomerCity = RekyCustomerDetails.CustomerCity;
                objdetailREkyc.RekycustomerPincode = RekyCustomerDetails.CustomerPincode;
                if (RekyCustomerDetails.CustomerState == "027" || RekyCustomerDetails.CustomerState == "MH")
                {
                    string s = "MH";
                    objdetailREkyc.RekycustomerState = s;
                }
                if (RekyCustomerDetails.CustomerCountryID == "101" || RekyCustomerDetails.CustomerCountryID == "IN")
                {
                    string s = "IN";
                    objdetailREkyc.RekycustomerCountryID = s;
                }
                objdetailREkyc.RekycustomerAnnualIncome = RekyCustomerDetails.CustomerAnualincome;
                objdetailREkyc.RekycustomerOccupation = RekyCustomerDetails.CustomerOccupation;
                return Json(objdetailREkyc);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DigitalQuickEnrollment([FromServices] IActiveLogin objLogin, ClsDigitalKYC1 objdigikyc)
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
            ClsDocDetails objFinalDoc = new ClsDocDetails();
            try
            {
                if (HttpContext.Session.GetString("Referencenumber") != null)
                {
                    objdigikyc.DigiReferencenumber = Convert.ToString(HttpContext.Session.GetString("Referencenumber"));
                }
                if (objdigikyc.DigiKYCPhoto != null && objdigikyc.DigiKYCPhoto != "" && objdigikyc.DigiKYCPhoto != "undefined")
                {
                    objdigikyc.livecameraphoto = (objdigikyc.DigiKYCPhoto);
                }
                else
                {
                    objdigikyc.livecameraphoto = null;
                }
                if (objdigikyc.Corresponence_Permanent_Address_same_flag == true)
                {
                    objdigikyc.CLIENT_PERM_ADDRESS_1 = objdigikyc.CLIENT_ADDRESS_1;
                    objdigikyc.CLIENT_PERM_ADDRESS_2 = objdigikyc.CLIENT_ADDRESS_2;
                    objdigikyc.CLIENT_PERM_ADDRESS_3 = objdigikyc.CLIENT_ADDRESS_3;
                    objdigikyc.CLIENT_PERM_CITY = objdigikyc.CLIENT_CITY;
                    objdigikyc.CLIENT_PERM_Pincode = objdigikyc.Pincode.Replace(" ", "");
                    objdigikyc.CLIENT_PERM_STATE = objdigikyc.CLIENT_STATE;
                    objdigikyc.CLIENT_PERM_COUNTRY = objdigikyc.CLIENT_COUNTRY;
                    objdigikyc.CLIENT_PERM_STATE = objdigikyc.CLIENT_STATE;
                }
                string msg = null;
                objdigikyc.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("KYCverificationType"));
                string DOB = objdigikyc.Digi_DOB;
                string pan = "";
                string aadhar = "";
                string pass = "";
                string vot = "";
                string DRI = "";
                if (objdigikyc.Digi_PAN != null)
                {
                    pan = objdigikyc.Digi_PAN;
                }
                else
                {
                    pan = "";
                }
                if (objdigikyc.Digi_Aadhar != null)
                {
                    aadhar = objdigikyc.Digi_Aadhar;
                }
                else
                {
                    aadhar = "";
                }
                if (objdigikyc.Digi_passport != null)
                {
                    pass = objdigikyc.Digi_passport;
                }
                else
                {
                    pass = "";
                }
                if (objdigikyc.Digi_Voter != null)
                {
                    vot = objdigikyc.Digi_Voter;
                }
                else
                {
                    vot = "";
                }
                if (objdigikyc.Digi_Drivinglicense != null)
                {
                    DRI = objdigikyc.Digi_Drivinglicense;
                }
                else
                {
                    DRI = "";
                }
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("Usp_AgentExistornot", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@panNo", pan);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var Result = reader["RESULT"].ToString();
                        if (Result != "1")
                        {
                            if (TempData["Photo"] != null)
                            {
                                byte[] photo = System.Convert.FromBase64String(TempData["Photo"].ToString());
                                objdigikyc.Photo = photo;
                            }
                            long? state = Convert.ToInt64(objdigikyc.CLIENT_PERM_STATE);
                            long? country = Convert.ToInt64(objdigikyc.CLIENT_PERM_COUNTRY);
                            objdigikyc.Digi_FirstName = objdigikyc.Digi_FirstName;
                            objdigikyc.Digi_MiddleName = objdigikyc.Digi_MiddleName;
                            objdigikyc.Digi_LastName = objdigikyc.Digi_LastName;
                            objdigikyc.Digi_gender = objdigikyc.Digi_gender;
                            objdigikyc.Digi_PAN = ObjTripleDes.Encrypt(objdigikyc.Digi_PAN);
                            objdigikyc.MobileNo = objdigikyc.MobileNo;
                            objdigikyc.CasteCd = objdigikyc.CasteCd;
                            objdigikyc.maritalstatus = objdigikyc.maritalstatus;
                            objdigikyc.Religion = objdigikyc.Religion;
                            objdigikyc.Residence = objdigikyc.Residence;
                            objdigikyc.residenceYN = objdigikyc.residenceYN;
                            objdigikyc.ResidenceDocument = objdigikyc.ResidenceDocument;
                            objdigikyc.ResidentialStatus = objdigikyc.ResidentialStatus;
                            objdigikyc.PhoneBanking = objdigikyc.PhoneBanking;
                            objdigikyc.SubTitle = objdigikyc.SubTitle;
                            objdigikyc.MobileDetails = objdigikyc.MobileDetails;
                            objdigikyc.CLIENT_PERM_ADDRESS_1 = objdigikyc.CLIENT_PERM_ADDRESS_1;
                            objdigikyc.CLIENT_PERM_ADDRESS_2 = objdigikyc.CLIENT_PERM_ADDRESS_2;
                            objdigikyc.CLIENT_PERM_ADDRESS_3 = objdigikyc.CLIENT_PERM_ADDRESS_3;

                            objdigikyc.CLIENT_PERM_Pincode = objdigikyc.CLIENT_PERM_Pincode;
                            objdigikyc.EmailId = objdigikyc.EmailId;

                            objdigikyc.Pincode = objdigikyc.Pincode;

                            objdigikyc.CLIENT_ADDRESS_1 = objdigikyc.CLIENT_ADDRESS_1;
                            objdigikyc.CLIENT_ADDRESS_2 = objdigikyc.CLIENT_ADDRESS_2;
                            objdigikyc.CLIENT_ADDRESS_3 = objdigikyc.CLIENT_ADDRESS_3;
                            objdigikyc.AMLRating = objdigikyc.AMLRating;
                            var AgentID = HttpContext.Session.GetString("UseID");
                            objdigikyc.CreatedBy = AgentID;
                            var progressbar = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            if (progressbar.IsPanVerify == true)
                            {
                                string dobStr = objdigikyc.Digi_DOB;
                                DateTime dob = DateTime.ParseExact(dobStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                string formattedDob = dob.ToString("dd-MM-yyyy");
                                objdigikyc.Digi_DOB = formattedDob;
                            }
                            using (SqlConnection connection11 = new SqlConnection(conn))
                            {
                                SqlCommand cmd11 = new SqlCommand("USP_Insert_KYC_CustomerDetails_NEW", connection11);
                                cmd11.CommandType = CommandType.StoredProcedure;
                                cmd11.Parameters.AddWithValue("@FirstName", objdigikyc.Digi_FirstName);
                                cmd11.Parameters.AddWithValue("@MiddleName", objdigikyc.Digi_MiddleName);
                                cmd11.Parameters.AddWithValue("@LastName", objdigikyc.Digi_LastName);
                                cmd11.Parameters.AddWithValue("@FatherName", "");
                                cmd11.Parameters.AddWithValue("@Gender", objdigikyc.Digi_gender);          
                                cmd11.Parameters.AddWithValue("@DOB", objdigikyc.Digi_DOB);
                                cmd11.Parameters.AddWithValue("@PanNo", objdigikyc.Digi_PAN);
                                cmd11.Parameters.AddWithValue("@AadharNo", "");
                                cmd11.Parameters.AddWithValue("@VoterId", "");
                                cmd11.Parameters.AddWithValue("@PassportNo", "");
                                cmd11.Parameters.AddWithValue("@DrivingLicenceNo", "");
                                cmd11.Parameters.AddWithValue("@MobileNo", objdigikyc.MobileNo);
                                cmd11.Parameters.AddWithValue("@CAST_code", objdigikyc.CasteCd);
                                cmd11.Parameters.AddWithValue("@maritalstatus_code", objdigikyc.maritalstatus);
                                cmd11.Parameters.AddWithValue("@Religion_code", objdigikyc.Religion);
                                cmd11.Parameters.AddWithValue("@residence_code", objdigikyc.Residence);
                                cmd11.Parameters.AddWithValue("@residenceYN_code", objdigikyc.residenceYN);
                                cmd11.Parameters.AddWithValue("@document_code", objdigikyc.ResidenceDocument);
                                cmd11.Parameters.AddWithValue("@ResidentialStatus", "Person of Indian Origin");
                                cmd11.Parameters.AddWithValue("@PB_code", objdigikyc.PhoneBanking);
                                cmd11.Parameters.AddWithValue("@NAME_code", objdigikyc.SubTitle);
                                cmd11.Parameters.AddWithValue("@EmailMobile_code", objdigikyc.MobileDetails);
                                cmd11.Parameters.AddWithValue("@PermAddress1", objdigikyc.CLIENT_PERM_ADDRESS_1);
                                cmd11.Parameters.AddWithValue("@PermAddress2", objdigikyc.CLIENT_PERM_ADDRESS_2);
                                cmd11.Parameters.AddWithValue("@PermAddress3", objdigikyc.CLIENT_PERM_ADDRESS_3);
                                cmd11.Parameters.AddWithValue("@PermCity", objdigikyc.CLIENT_PERM_CITY);
                                cmd11.Parameters.AddWithValue("@PermPin", objdigikyc.CLIENT_PERM_Pincode);
                                cmd11.Parameters.AddWithValue("@Permstate", objdigikyc.CLIENT_PERM_STATE);
                                cmd11.Parameters.AddWithValue("@permcountry", objdigikyc.CLIENT_PERM_CITY);
                                cmd11.Parameters.AddWithValue("@EmailId", objdigikyc.EmailId);
                                cmd11.Parameters.AddWithValue("@CountryId", objdigikyc.CLIENT_PERM_COUNTRY);
                                cmd11.Parameters.AddWithValue("@StateId", objdigikyc.CLIENT_PERM_STATE);
                                cmd11.Parameters.AddWithValue("@CityId", objdigikyc.CLIENT_PERM_CITY);
                                cmd11.Parameters.AddWithValue("@PinCode", objdigikyc.Pincode);
                                cmd11.Parameters.AddWithValue("@Photo", "");

                                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                                byte[] data = encoding.GetBytes(objdigikyc.livecameraphoto);
                                MemoryStream cmpStream = new MemoryStream();
                                GZipStream hgs = new GZipStream(cmpStream, CompressionMode.Compress);
                                hgs.Write(data, 0, data.Length);
                                byte[] cmpData = cmpStream.ToArray();
                                cmd11.Parameters.AddWithValue("@livephoto", cmpData);
                                cmd11.Parameters.AddWithValue("@MobileDetails_Code", "");
                                cmd11.Parameters.AddWithValue("@CustomerDetailId", objdigikyc.PersonalId);
                                cmd11.Parameters.AddWithValue("@ReferenceNumber", "");
                                cmd11.Parameters.AddWithValue("@Address1", objdigikyc.CLIENT_ADDRESS_1);
                                cmd11.Parameters.AddWithValue("@Address2", objdigikyc.CLIENT_ADDRESS_2);
                                cmd11.Parameters.AddWithValue("@Address3", objdigikyc.CLIENT_ADDRESS_3);
                                cmd11.Parameters.AddWithValue("@AMLRating", objdigikyc.AMLRating);
                                if (AgentID != null)
                                {
                                    cmd11.Parameters.AddWithValue("@CreatedBy", objdigikyc.CreatedBy);
                                }
                                else
                                {
                                    var MobNo = HttpContext.Session.GetString("CustMobileNo");
                                    cmd11.Parameters.AddWithValue("@CreatedBy", "");
                                }
                                connection11.Open();
                                int response = cmd11.ExecuteNonQuery();
                                objdigikyc.PersonalId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                                string livecameraphoto = null;
                                if (objdigikyc.livecameraphoto != null)
                                {
                                    objFinalDoc.DocName = "livePhoto.jpg";
                                    extension = "jpg";
                                    objFinalDoc.DocMainType = "P";
                                    objFinalDoc.DocCategoryCode = "0";
                                    objFinalDoc.CustomerDetailId = (objdigikyc.PersonalId);
                                    objFinalDoc.Latitude_Longitude = objdigikyc.Latitude_Longitude;
                                    objFinalDoc.Prediction = objdigikyc.Prediction;
                                    objFinalDoc.DocumentId = Convert.ToString(objdigikyc.PersonalId);
                                    objFinalDoc.documentCategory = "LivePhoto";
                                    int documenttypeid = 17;
                                    objFinalDoc.Source = "FromLivePhoto";
                                    string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                                    using (SqlConnection con = new SqlConnection(conn))
                                    {
                                        SqlDataReader reader1 = null;
                                        con.Open();
                                        SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.CustomerDetailId));
                                        cmd1.Parameters.Add(new SqlParameter("@document", Convert.FromBase64String(objdigikyc.livecameraphoto)));
                                        cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                        cmd1.Parameters.Add(new SqlParameter("@docTypeId", documenttypeid));
                                        cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc.DocMainType));
                                        cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                        cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                        cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc.Source));
                                        cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));
                                        using (reader1 = cmd1.ExecuteReader())
                                        {
                                            foreach (var item in reader1)
                                            {
                                                var result = reader[0].ToString();
                                                HttpContext.Session.SetString("liveimage", objdigikyc.DigiKYCPhoto);
                                            }
                                        }
                                    }
                                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                    IMapper mapper = config.CreateMapper();
                                    ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc);
                                    livecameraphoto = "2";
                                    if (isInserted > 0)
                                    {
                                        //isSuccess = true;
                                    }
                                    objdigikyc.LivePhoto = null;
                                }
                                if (response != null)
                                {
                                    objdigikyc.proceedwithOCR = Convert.ToString(HttpContext.Session.GetString("proceedwithOCR"));
                                    objdigikyc.shareAadharNumber = Convert.ToString(HttpContext.Session.GetString("shareAadharNumber"));
                                    bool VerificationTypeForAadhar = false;
                                    if (objdigikyc.KYCverificationType != "" && objdigikyc.KYCverificationType != null)
                                    {
                                        VerificationTypeForAadhar = true;
                                    }
                                    else if (objdigikyc.AadharVerificationType == true)
                                    {
                                        VerificationTypeForAadhar = objdigikyc.AadharVerificationType;
                                    }
                                    using (SqlConnection connection3 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd3 = new SqlCommand("USP_CustomerUpdateFlagNew", connection3);
                                        cmd3.CommandType = CommandType.StoredProcedure;
                                        cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                        cmd3.Parameters.AddWithValue("@proceedwithOCR", objdigikyc.proceedwithOCR);
                                        cmd3.Parameters.AddWithValue("@shareAadharNumber", objdigikyc.shareAadharNumber);
                                        cmd3.Parameters.AddWithValue("@isPanVerify", objdigikyc.PanVerificationType);
                                        cmd3.Parameters.AddWithValue("@isCKYCVerify", objdigikyc.CKYCVerificationType);
                                        cmd3.Parameters.AddWithValue("@isAadharVerify", VerificationTypeForAadhar);
                                        connection3.Open();
                                        SqlDataReader reader3 = cmd3.ExecuteReader();
                                        if (reader3.Read())
                                        {

                                            //var Result = reader2["RESULT"].ToString();
                                        }
                                    }
                                    HttpContext.Session.SetString("shareAadharNumber", objdigikyc.shareAadharNumber);
                                    return RedirectToAction("CustomerDocumentDetails", "DocumentDetails");
                                }
                                else
                                {
                                    TempData["msg"] = "Error" + livecameraphoto;
                                    return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                                }
                            }
                        }
                        else
                        {
                            if (objdigikyc.PersonalId > 0)
                            {
                                TempData["msg"] = "Already Exist";
                                return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                            }
                            else
                            {
                                string updateKycData = JsonConvert.SerializeObject(objdigikyc);
                                string ddDG = updateKycData.Replace("[{", "");
                                string[] linesDG = ddDG.Split(',', ':');
                                long? PersonalId = Convert.ToInt64(linesDG[1].ToString().Trim());
                                var Digi_FirstName = linesDG[3].ToString().Trim();
                                var Digi_MiddleName = linesDG[5].ToString().Trim();
                                var Digi_LastName = linesDG[7].ToString().Trim();
                                var Digi_BarcodeOrAppNumber = linesDG[9].ToString().Trim();
                                var Digi_DOB = linesDG[11].ToString().Trim();
                                var CasteCd = linesDG[12].ToString().Trim();
                                var Digi_gender = linesDG[13].ToString().Trim();
                                var maritalstatus = linesDG[14].ToString().Trim();
                                var Digi_PAN = linesDG[15].ToString().Trim();
                                var Religion = linesDG[16].ToString().Trim();
                                var MobileNo = linesDG[17].ToString().Trim();
                                var Residence = linesDG[18].ToString().Trim();
                                var MobileDetails = linesDG[19].ToString().Trim();
                                var residenceYN = linesDG[20].ToString().Trim();
                                var Digi_Aadhar = linesDG[21].ToString().Trim();
                                var ResidenceDocument = linesDG[22].ToString().Trim();
                                var Digi_Voter = linesDG[23].ToString().Trim();
                                var PhoneBanking = linesDG[24].ToString().Trim();
                                var Digi_passport = linesDG[25].ToString().Trim();
                                var Digi_Drivinglicense = linesDG[27].ToString().Trim();
                                var SubTitle = linesDG[28].ToString().Trim();
                                var Digi_nrega = linesDG[29].ToString().Trim();
                                var Flag = linesDG[31].ToString().Trim();
                                var DigiKYCPhoto = linesDG[33].ToString().Trim();
                                var LiveCameraPhoto = linesDG[35].ToString().Trim();
                                var BCPhoto = linesDG[37].ToString().Trim();
                                var LivePhoto = linesDG[39].ToString().Trim();
                                var Photo = linesDG[41].ToString().Trim();
                                var Address = linesDG[43].ToString().Trim();
                                var Latitude_Longitude = linesDG[45].ToString().Trim();
                                var Prediction = linesDG[49].ToString().Trim();
                                var CLIENT_ADDRESS_1 = linesDG[51].ToString().Trim();
                                var CLIENT_ADDRESS_2 = linesDG[53].ToString().Trim();
                                var CLIENT_ADDRESS_3 = linesDG[55].ToString().Trim();
                                var CLIENT_CITY = linesDG[57].ToString().Trim();
                                var CLIENT_STATE = linesDG[59].ToString().Trim();
                                var CLIENT_COUNTRY = linesDG[61].ToString().Trim();
                                var CLIENT_PERM_ADDRESS_1 = linesDG[63].ToString().Trim();
                                var CLIENT_PERM_ADDRESS_2 = linesDG[65].ToString().Trim();
                                var CLIENT_PERM_ADDRESS_3 = linesDG[67].ToString().Trim();
                                var CLIENT_PERM_CITY = linesDG[69].ToString().Trim();
                                var CLIENT_PERM_STATE = linesDG[71].ToString().Trim();
                                var CLIENT_PERM_COUNTRY = linesDG[73].ToString().Trim();
                                var CLIENT_PERM_Pincode = linesDG[75].ToString().Trim();
                                var Corresponence_Permanent_Address_same_flag = linesDG[77].ToString().Trim();
                                var EmailId = linesDG[79].ToString().Trim();
                                var Pincode = linesDG[81].ToString().Trim();
                                var proceedwithOCR = linesDG[83].ToString().Trim();
                                var shareAadharNumber = linesDG[85].ToString().Trim();
                                var KYCverificationType = linesDG[87].ToString().Trim();
                                var PanVerificationType = linesDG[89].ToString().Trim();
                                var Otherverification = linesDG[91].ToString().Trim();
                                var CKYCVerificationType = linesDG[93].ToString().Trim();
                                var AadharVerificationType = linesDG[95].ToString().Trim();
                                var DigiReferencenumber = linesDG[97].ToString().Trim();
                                var isDigiApproveOrReject = linesDG[99].ToString().Trim();
                                var CustDetailsId = linesDG[101].ToString().Trim();
                                var Digilock_firstname = linesDG[103].ToString().Trim();
                                var Digilock_middlename = linesDG[105].ToString().Trim();
                                var Digilock_lastname = linesDG[107].ToString().Trim();
                                var Digilock_PANNo = linesDG[109].ToString().Trim();
                                var dob1lock = linesDG[111].ToString().Trim();
                                var genderlock = linesDG[115].ToString().Trim();
                                var Genderfemale = linesDG[117].ToString().Trim();
                                var Digi_ORGname = linesDG[119].ToString().Trim();
                                var Digi_Dfirstname = linesDG[121].ToString().Trim();
                                var Digi_Dswd = linesDG[123].ToString().Trim();
                                var Digi_Dlastname = linesDG[125].ToString().Trim();
                                var Digi_DAddress = linesDG[127].ToString().Trim();
                                var Digidob2 = linesDG[129].ToString().Trim();
                                var DigilockerType = linesDG[131].ToString().Trim();
                                var IsDigiPANSumbitted = linesDG[133].ToString().Trim();
                                var IsDigilDRLCSumbitted = linesDG[135].ToString().Trim();
                                var IsDigiAadharSumbitted = linesDG[137].ToString().Trim();
                                var Digilockgender = linesDG[139].ToString().Trim();
                                var Digilock_Afirstname = linesDG[141].ToString().Trim();
                                var Digilock_Amiddlename = linesDG[143].ToString().Trim();
                                var Digilock_Alastname = linesDG[145].ToString().Trim();
                                var FirstNameOther = linesDG[147].ToString().Trim();
                                var MiddleNameOther = linesDG[149].ToString().Trim();
                                var LastNameOther = linesDG[151].ToString().Trim();
                                var Oaddressline1 = linesDG[153].ToString().Trim();
                                var Oaddressline2 = linesDG[155].ToString().Trim();
                                var DateOfBirthOther = linesDG[157].ToString().Trim();
                                var Other_PinCode = linesDG[159].ToString().Trim();
                                var Other_State = linesDG[161].ToString().Trim();
                                var Other_City = linesDG[163].ToString().Trim();
                                var Other_CountryCode = linesDG[165].ToString().Trim();
                                string Referencenumber1 = "";
                                if (DigiReferencenumber != "null")
                                {
                                    Referencenumber1 = DigiReferencenumber;
                                }
                                else
                                {
                                    Referencenumber1 = "";
                                }
                                string name = "";
                                if (Digi_FirstName != "null")
                                {
                                    name = Digi_FirstName;
                                }
                                else if (Digilock_firstname != "null")
                                {
                                    name = Digilock_firstname;
                                }
                                else if (Digilock_Afirstname != "null")
                                {
                                    name = Digilock_Afirstname;
                                }
                                else if (FirstNameOther != "null")
                                {
                                    name = FirstNameOther;
                                }
                                else
                                {
                                    name = "";
                                }
                                string mname = "";
                                if (Digi_MiddleName != "null")
                                {
                                    mname = Digi_MiddleName;
                                }
                                else if (Digilock_middlename != "null")
                                {
                                    mname = Digilock_middlename;
                                }
                                else if (Digilock_Amiddlename != "null")
                                {
                                    mname = Digilock_Amiddlename;
                                }
                                else if (MiddleNameOther != "null")
                                {
                                    mname = MiddleNameOther;
                                }
                                else
                                {
                                    mname = "";
                                }
                                string Lname = "";
                                if (Digi_LastName != "null")
                                {
                                    Lname = Digi_LastName;
                                }
                                else if (Digilock_lastname != "null")
                                {
                                    Lname = Digilock_lastname;
                                }
                                else if (Digi_Dlastname != "null")
                                {
                                    Lname = Digi_Dlastname;
                                }
                                else if (Digilock_Alastname != "null")
                                {
                                    Lname = Digilock_Alastname;
                                }
                                else if (LastNameOther != "null")
                                {
                                    Lname = LastNameOther;
                                }
                                else
                                {
                                    Lname = "";
                                }
                                string Gender = "";
                                if (Digi_gender != "null")
                                {
                                    Gender = Digi_gender;
                                }
                                else if (Digilockgender != "null")
                                {
                                    Gender = Digilockgender;
                                }
                                else if (genderlock != "null")
                                {
                                    Gender = genderlock;
                                }
                                else
                                {
                                    Gender = "";
                                }
                                string DOB1 = "";
                                if (Digidob2 != "null")
                                {
                                    DOB1 = Digidob2;
                                }
                                else if (Digi_DOB != "null")
                                {
                                    DOB1 = Digi_DOB;
                                }
                                else if (DateOfBirthOther != "null")
                                {
                                    DOB1 = DateOfBirthOther;
                                }
                                else
                                {
                                    DOB = "";
                                }
                                var pan1 = "";
                                if (Digilock_PANNo != "null")
                                {
                                    pan1 = Digilock_PANNo;
                                }
                                else if (Digi_PAN != "null")
                                {
                                    pan1 = Digi_PAN;
                                }
                                else
                                {
                                    pan1 = "";
                                }
                                var aadhar1 = "";
                                if (Digilock_PANNo != "null")
                                {
                                    aadhar1 = Digi_Aadhar;
                                }
                                else
                                {
                                    aadhar1 = "";
                                }
                                var VoterId1 = "";
                                if (Digi_Voter != "null")
                                {
                                    VoterId1 = Digi_Voter;
                                }
                                else
                                {
                                    VoterId1 = "";
                                }
                                var PassportNo1 = "";
                                if (Digi_passport != "null")
                                {
                                    PassportNo1 = Digi_passport;
                                }
                                else
                                {
                                    PassportNo1 = "";
                                }
                                var DrivingLicenceNo1 = "";
                                if (Digi_Drivinglicense != "null")
                                {
                                    DrivingLicenceNo1 = Digi_Drivinglicense;
                                }
                                else
                                {
                                    DrivingLicenceNo1 = "";
                                }
                                var MoNo1 = "";
                                if (MobileNo != "null")
                                {
                                    MoNo1 = MobileNo;
                                }
                                else
                                {
                                    MoNo1 = "";
                                }
                                var Email = "";
                                if (EmailId != "null")
                                {
                                    Email = EmailId;
                                }
                                else
                                {
                                    Email = "";
                                }
                                string State1 = "";
                                if (Other_State != null)
                                {
                                    State1 = Other_State;
                                }
                                else if (CLIENT_STATE != null)
                                {
                                    State1 = CLIENT_STATE;
                                }
                                else
                                {
                                    State1 = null;
                                }

                                string City1 = "";
                                if (CLIENT_CITY != null)
                                {
                                    City1 = CLIENT_CITY;
                                }

                                else if (Other_City != null)
                                {
                                    City1 = Other_City;
                                }
                                else
                                {
                                    City1 = null;
                                }

                                var Addre3 = "";
                                if (CLIENT_ADDRESS_2 != "null")
                                {
                                    Addre3 = CLIENT_ADDRESS_3;
                                }
                                else
                                {
                                    Addre3 = "";
                                }

                                var Addre2 = "";
                                if (Oaddressline2 != "null")
                                {
                                    Addre2 = Oaddressline2;
                                }
                                else if (Digi_DAddress != "null")
                                {
                                    Addre2 = Digi_DAddress;
                                }
                                else if (CLIENT_ADDRESS_2 != "null")
                                {
                                    Addre2 = CLIENT_ADDRESS_2;
                                }
                                else
                                {
                                    Addre2 = "";
                                }
                                var Addre1 = "";
                                if (Oaddressline1 != "null")
                                {
                                    Addre1 = Oaddressline1;
                                }
                                else if (Digi_DAddress != "null")
                                {
                                    Addre1 = Digi_DAddress;
                                }
                                else if (CLIENT_ADDRESS_1 != "null")
                                {
                                    Addre1 = CLIENT_ADDRESS_1;
                                }
                                else
                                {
                                    Addre1 = "";
                                }

                                var photo1 = "";
                                if (Photo != "null")
                                {
                                    photo1 = Photo;
                                }
                                else
                                {
                                    photo1 = "";
                                }

                                var Livephoto1 = "";
                                if (LivePhoto != "null")
                                {
                                    Livephoto1 = LivePhoto;
                                }
                                else if (LiveCameraPhoto != "null")
                                {
                                    Livephoto1 = LiveCameraPhoto;
                                }
                                else
                                {
                                    Livephoto1 = LiveCameraPhoto;
                                }


                                var PinCode1 = "";
                                if (Other_PinCode != "null")
                                {
                                    PinCode1 = Other_PinCode;
                                }

                                else if (Pincode != "null")
                                {
                                    PinCode1 = Pincode;
                                }
                                else
                                {
                                    PinCode1 = "";
                                }
                                long? state = Convert.ToInt64(objdigikyc.CLIENT_PERM_STATE);
                                long? country = Convert.ToInt64(objdigikyc.CLIENT_PERM_COUNTRY);
                                using (SqlConnection connection11 = new SqlConnection(conn))
                                {
                                    SqlCommand cmd11 = new SqlCommand("USP_Update_KYC_CustomerDetails_NEW", connection11);
                                    cmd11.CommandType = CommandType.StoredProcedure;
                                    cmd11.Parameters.AddWithValue("@FirstName", name);
                                    cmd11.Parameters.AddWithValue("@MiddleName", Digi_MiddleName);
                                    cmd11.Parameters.AddWithValue("@LastName", Lname);
                                    cmd11.Parameters.AddWithValue("@FatherName", mname);
                                    cmd11.Parameters.AddWithValue("@Gender", Gender);
                                    cmd11.Parameters.AddWithValue("@DOB", DOB1);
                                    cmd11.Parameters.AddWithValue("@CAST_code", CasteCd);
                                    cmd11.Parameters.AddWithValue("@maritalstatus_code", maritalstatus);
                                    cmd11.Parameters.AddWithValue("@Religion_code", Religion);
                                    cmd11.Parameters.AddWithValue("@residence_code", Residence);
                                    cmd11.Parameters.AddWithValue("@residenceYN_code", residenceYN);
                                    cmd11.Parameters.AddWithValue("@document_code", ResidenceDocument);
                                    cmd11.Parameters.AddWithValue("@PB_code", PhoneBanking);
                                    cmd11.Parameters.AddWithValue("@NAME_code", SubTitle);

                                    cmd11.Parameters.AddWithValue("@PanNo", ObjTripleDes.Encrypt(objdigikyc.Digi_PAN));
                                    cmd11.Parameters.AddWithValue("@AadharNo", aadhar1);
                                    cmd11.Parameters.AddWithValue("@VoterId", VoterId1);
                                    cmd11.Parameters.AddWithValue("@PassportNo", PassportNo1);
                                    cmd11.Parameters.AddWithValue("@DrivingLicenceNo", DrivingLicenceNo1);
                                    cmd11.Parameters.AddWithValue("@MobileNo", objdigikyc.MobileNo);
                                    cmd11.Parameters.AddWithValue("@Address", objdigikyc.Address);
                                    cmd11.Parameters.AddWithValue("@EmailId", objdigikyc.EmailId);
                                    cmd11.Parameters.AddWithValue("@CountryId", objdigikyc.CLIENT_COUNTRY);
                                    cmd11.Parameters.AddWithValue("@StateId", objdigikyc.Digi_PAN);
                                    cmd11.Parameters.AddWithValue("@CityId", objdigikyc.CLIENT_PERM_CITY);
                                    cmd11.Parameters.AddWithValue("@PinCode", objdigikyc.Pincode);
                                    cmd11.Parameters.AddWithValue("@EmailMobile_code", MobileDetails);
                                    cmd11.Parameters.AddWithValue("@UCMID", "");
                                    cmd11.Parameters.AddWithValue("@CKYCNO", "");
                                    cmd11.Parameters.AddWithValue("@Photo", photo1);
                                    cmd11.Parameters.AddWithValue("@livephoto", Livephoto1);
                                    cmd11.Parameters.AddWithValue("@CustomerDetailId", objdigikyc.PersonalId);
                                    cmd11.Parameters.AddWithValue("@ReferenceNumber", objdigikyc.DigiReferencenumber);
                                    cmd11.Parameters.AddWithValue("@Address1", objdigikyc.CLIENT_ADDRESS_1);
                                    cmd11.Parameters.AddWithValue("@Address2", objdigikyc.CLIENT_ADDRESS_2);
                                    cmd11.Parameters.AddWithValue("@PermAddress1", CLIENT_PERM_ADDRESS_1);
                                    cmd11.Parameters.AddWithValue("@PermAddress2", CLIENT_PERM_ADDRESS_2);
                                    cmd11.Parameters.AddWithValue("@PermAddress3", CLIENT_PERM_ADDRESS_3);
                                    cmd11.Parameters.AddWithValue("@PermCity", CLIENT_PERM_CITY);
                                    cmd11.Parameters.AddWithValue("@PermPin", CLIENT_PERM_Pincode);
                                    cmd11.Parameters.AddWithValue("@Permstate", CLIENT_PERM_STATE);
                                    cmd11.Parameters.AddWithValue("@permcountry", CLIENT_PERM_CITY);
                                    connection11.Open();
                                    int response = cmd11.ExecuteNonQuery();
                                }
                                var result1 = objDetails.AdmKycCustomerDetails.FromSqlRaw($"USP_GetCustomerDetails {objdigikyc.PersonalId}, {objdigikyc.Digi_PAN}").AsEnumerable().FirstOrDefault();
                                bool VerificationTypeForAadhar = false;
                                if (objdigikyc.KYCverificationType != "" && objdigikyc.KYCverificationType != null)
                                {
                                    VerificationTypeForAadhar = true;
                                }
                                else if (objdigikyc.AadharVerificationType == true)
                                {
                                    VerificationTypeForAadhar = objdigikyc.AadharVerificationType;
                                }
                                if (result1 != null)
                                {
                                    HttpContext.Session.GetString("PersonalId");

                                    return RedirectToAction("CustomerDocumentDetails", "DocumentDetails");
                                }
                                TempData["msg"] = "Record Updated";
                                return RedirectToAction("CustomerDocumentDetails", "DocumentDetails");
                            }

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



        public byte[] watermark(byte[] image)
        {

            string imageName = ts;
            byte[] NewImage1 = null;
            byte[] newimage = null;
            if (image != null)
            {
                string watermarkText = imageName + "\n\n";
                string fileName = imageName + ".png";
                Bitmap bmp1;
                using (var ms = new MemoryStream(image))
                {
                    using (Bitmap bmp = new Bitmap(ms, false))
                    {
                        using (Graphics grp = Graphics.FromImage(bmp))
                        {
                            Brush brush = new SolidBrush(Color.Tomato);
                            System.Drawing.Font font = new System.Drawing.Font("Arial", 10, FontStyle.Bold, GraphicsUnit.Pixel);
                            SizeF textSize = new SizeF();
                            textSize = grp.MeasureString(watermarkText, font);
                            Point position = new Point((bmp.Width - ((int)textSize.Width + 15)), (bmp.Height - ((int)textSize.Height + 25)));
                            grp.DrawString(watermarkText, font, brush, position);
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                bmp.Save(memoryStream, ImageFormat.Jpeg);
                                memoryStream.Position = 0;
                                newimage = memoryStream.ToArray();
                                NewImage1 = AddWaterMark(newimage);
                            }
                        }
                    }
                }
            }
            return NewImage1;
        }
        public byte[] AddWaterMark(byte[] newimage)
        {
            return newimage;
        }
        public JsonResult GetDigilockerPan(long? PersonalId)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var result = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetDigiPan {PersonalId}").AsEnumerable().FirstOrDefault();
                objDigitalKYCdata.Digilock_PANNo = result.Panno;
                objDigitalKYCdata.Digilock_firstname = result.Firstname;
                objDigitalKYCdata.Digilock_middlename = result.Middlename;
                objDigitalKYCdata.Digilock_lastname = result.Lastname;
                objDigitalKYCdata.dob1lock = result.Dob;
                objDigitalKYCdata.Digilockgender = result.Gender;
                return Json(objDigitalKYCdata);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public JsonResult GetDigilockerDriving(long? PersonalId)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var DrivingData = objDetails.AdmDigiDrivingLicences.FromSqlRaw($"USP_GetDigiDriving {PersonalId}").AsEnumerable().FirstOrDefault(); ;
                objDigitalKYCdata.Digi_FirstName = DrivingData.Firstname;
                objDigitalKYCdata.Digi_Dswd = DrivingData.Swd;
                objDigitalKYCdata.Digi_LastName = DrivingData.Lastname;
                objDigitalKYCdata.Digi_DAddress = DrivingData.Address;
                objDigitalKYCdata.Digi_DOB = DrivingData.Dob;
                objDigitalKYCdata.Pincode = (DrivingData.Address).Substring(DrivingData.Address.Length - 6);
                objDigitalKYCdata.CLIENT_COUNTRY = DrivingData.Country;
                objDigitalKYCdata.CLIENT_ADDRESS_1 = DrivingData.Address;
                int length = 45;
                if (DrivingData.Address.Length <= 45)
                {
                    objDigitalKYCdata.CLIENT_ADDRESS_1 = DrivingData.Address;
                }
                else
                {
                    int iNextSpace = DrivingData.Address.LastIndexOf(" ", length, StringComparison.Ordinal);
                    objDigitalKYCdata.CLIENT_ADDRESS_1 = (string.Format("{0}", DrivingData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                    if (DrivingData.Address.Length > 45 && DrivingData.Address.Length <= 90)
                    {
                        DrivingData.Address = DrivingData.Address.Substring(iNextSpace + 1);
                        objDigitalKYCdata.CLIENT_ADDRESS_2 = DrivingData.Address;
                        if (objDigitalKYCdata.CLIENT_ADDRESS_2.Length > 45)
                        {
                            iNextSpace = objDigitalKYCdata.CLIENT_ADDRESS_2.LastIndexOf(" ", length, StringComparison.Ordinal);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", DrivingData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                            objDigitalKYCdata.CLIENT_ADDRESS_3 = DrivingData.Address.Substring(iNextSpace + 1);
                        }
                        else
                        {
                            objDigitalKYCdata.CLIENT_ADDRESS_3 = null;
                        }
                    }
                    if (DrivingData.Address.Length > 45 && DrivingData.Address.Length > 90)
                    {
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = DrivingData.Address.Substring(iNextSpace + 1);
                        iNextSpace = DrivingData.Address.LastIndexOf(" ", length, StringComparison.Ordinal);
                        objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", DrivingData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                        objDigitalKYCdata.CLIENT_ADDRESS_3 = DrivingData.Address.Substring(iNextSpace + 1);
                    }
                }
                if (DrivingData.Country == "IN")
                {
                    objDigitalKYCdata.CLIENT_COUNTRY = "101";
                }
                if (DrivingData.State == "Maharashtra" || DrivingData.State == "maharashtra")
                {
                    objDigitalKYCdata.CLIENT_STATE = "027";
                }
                if (objDigitalKYCdata.CLIENT_STATE == "027")
                {
                    var verificationtype = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State_New {objDigitalKYCdata.CLIENT_STATE}").ToList()
                                            select new SelectListItem()
                                            {
                                                Text = details.StateName.ToString(),
                                                Value = details.StateCode1,
                                            }).ToList();
                    verificationtype.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.State = verificationtype;
                }
                if (objDigitalKYCdata.CLIENT_COUNTRY != null)
                {
                    var verificationtype1 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA_NEW {objDigitalKYCdata.CLIENT_COUNTRY}").ToList()
                                             select new SelectListItem()
                                             {
                                                 Text = details.Country.ToString(),
                                                 Value = details.CountryCode,
                                             }).ToList();
                    verificationtype1.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.getCountry = verificationtype1;
                }
                return Json(objDigitalKYCdata);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        
        public JsonResult GetDigiAadharVerification(long? PersonalId)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string IsREkyctrue = HttpContext.Session.GetString("CustIDRekyc");
                if (IsREkyctrue != null)
                {
                    var CLIENT_COUNTRY1 = "";
                    var AadharData = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {PersonalId}").AsEnumerable().FirstOrDefault();
                    string CustnoFromCBS = HttpContext.Session.GetString("CustIDRekyc");
                    string Ac_No = HttpContext.Session.GetString("Acc_No");
                    var RekyCustomerDetails = objDetails.Set<AdmRekycCustomerDetail>().FromSqlRaw("EXEC usp_TogetRekycCustomerDetails @CustomerNo={0}, @Ac_no={1}", CustnoFromCBS, Ac_No).AsEnumerable().FirstOrDefault();
                    objDigitalKYCdata.Digilock_Afirstname = AadharData.Firstname;
                    objDigitalKYCdata.Digilock_Amiddlename = AadharData.Middlename;
                    objDigitalKYCdata.Digilock_Alastname = AadharData.Lastname;
                    objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.Address;
                    int length = 45;
                    if (AadharData.Address.Length <= 45)
                    {
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.Address;
                    }
                    else
                    {
                        int iNextSpace = AadharData.Address.LastIndexOf(" ", length, StringComparison.Ordinal);
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = (string.Format("{0}", AadharData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                        if (AadharData.Address.Length > 45 && AadharData.Address.Length <= 90)
                        {
                            AadharData.Address = AadharData.Address.Substring(iNextSpace + 1);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = AadharData.Address;
                            if (objDigitalKYCdata.CLIENT_ADDRESS_2.Length > 45)
                            {
                                iNextSpace = objDigitalKYCdata.CLIENT_ADDRESS_2.LastIndexOf(" ", length, StringComparison.Ordinal);
                                objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.Address.Substring(iNextSpace + 1);
                            }
                            else
                            {
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = null;
                            }
                        }
                        if (AadharData.Address.Length > 45 && AadharData.Address.Length > 90)
                        {
                            objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.Address.Substring(iNextSpace + 1);
                            iNextSpace = AadharData.Address.LastIndexOf(" ", length, StringComparison.Ordinal);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                            objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.Address.Substring(iNextSpace + 1);
                        }
                    }
                    string DOB = AadharData.Dob;
                    if (DOB.Contains('-'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.Dob, "dd-MM-yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                    }
                    else if (DOB.Contains('/'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.Dob, "dd/MM/yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                    }
                    objDigitalKYCdata.Digi_gender = AadharData.Gender;
                    objDigitalKYCdata.Pincode = AadharData.Pc;
                    objDigitalKYCdata.CLIENT_CITY = AadharData.Vtc;

                    objDigitalKYCdata.CLIENT_COUNTRY = AadharData.Country;

                    objDigitalKYCdata.CLIENT_STATE = AadharData.State;

                    if (AadharData.Country == "India" || AadharData.Country == "india")
                    {
                        objDigitalKYCdata.CLIENT_COUNTRY = "IN";
                    }
                    if (AadharData.State == "Maharashtra" || AadharData.State == "maharashtra")
                    {
                        objDigitalKYCdata.CLIENT_STATE = "MH";
                    }
                    if (RekyCustomerDetails.CustomerState == "Maharashtra" || RekyCustomerDetails.CustomerState == "maharashtra" || RekyCustomerDetails.CustomerState == "027")
                    {
                        RekyCustomerDetails.CustomerState = "MH";
                    }

                    if (RekyCustomerDetails.CustomerCountryID == "101" || RekyCustomerDetails.CustomerCountryID == "IN")
                    {
                        objDigitalKYCdata.CLIENT_COUNTRY = "IN";
                    }
                    objDigitalKYCdata.FirstnameStatus = string.Equals(AadharData.Firstname.ToLower(), RekyCustomerDetails.CustomerFirstname.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.MiddlenameStatus = string.Equals(AadharData.Middlename.ToLower(), RekyCustomerDetails.CustomerMiddlename.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.LastnameStatus = string.Equals(AadharData.Lastname.ToLower(), RekyCustomerDetails.CustomerLastname.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.DobStatus = string.Equals(AadharData.Dob.ToLower(), RekyCustomerDetails.customerDOB.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.GenderStatus = string.Equals(AadharData.Gender.ToLower(), RekyCustomerDetails.CustomerGender.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.CountryStatus = string.Equals(objDigitalKYCdata.CLIENT_COUNTRY.ToLower(), objDigitalKYCdata.CLIENT_COUNTRY.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.PincodeStatus = string.Equals(AadharData.Pc.ToLower(), RekyCustomerDetails.CustomerPincode.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.STATEStatus = string.Equals(objDigitalKYCdata.CLIENT_STATE.ToLower(), RekyCustomerDetails.CustomerState.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.CITYStatus = string.Equals(AadharData.Vtc.ToLower(), RekyCustomerDetails.CustomerCity.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.ADDRESSStatus1 = string.Equals(AadharData.Address.ToLower(), RekyCustomerDetails.CustomerAdd1.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.ADDRESSStatus2 = string.Equals(AadharData.Address.ToLower(), RekyCustomerDetails.CustomerAdd2.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.ADDRESSStatus3 = string.Equals(AadharData.Address.ToLower(), RekyCustomerDetails.CustomerAdd3.ToLower()) ? "Match" : "Does Not Match";
                    var verificationtype = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State_New {objDigitalKYCdata.CLIENT_STATE}").ToList()
                                            select new SelectListItem()
                                            {
                                                Text = details.StateName.ToString(),
                                                Value = details.StateCode1,
                                            }).ToList();
                    verificationtype.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.State = verificationtype;
                    if (objDigitalKYCdata.CLIENT_COUNTRY == "IN")
                    {
                        CLIENT_COUNTRY1 = "101";
                    }
                    var verificationtype1 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA_NEW {CLIENT_COUNTRY1/*objDigitalKYCdata.CLIENT_COUNTRY*/}").ToList()
                                             select new SelectListItem()

                                             {
                                                 Text = details.Country.ToString(),
                                                 Value = details.CountryCode,
                                             }).ToList();
                    verificationtype1.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.getCountry = verificationtype1;
                }
                else
                {
                    var AadharData = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {PersonalId}").AsEnumerable().FirstOrDefault();
                    objDigitalKYCdata.Digilock_Afirstname = AadharData.Firstname;
                    objDigitalKYCdata.Digilock_Amiddlename = AadharData.Middlename;
                    objDigitalKYCdata.Digilock_Alastname = AadharData.Lastname;
                    objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.Address;
                    int length = 45;
                    if (AadharData.Address.Length <= 45)
                    {
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.Address;
                    }
                    else
                    {
                        int iNextSpace = AadharData.Address.LastIndexOf(" ", length, StringComparison.Ordinal);
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = (string.Format("{0}", AadharData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                        if (AadharData.Address.Length > 45 && AadharData.Address.Length <= 90)
                        {
                            AadharData.Address = AadharData.Address.Substring(iNextSpace + 1);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = AadharData.Address;
                            if (objDigitalKYCdata.CLIENT_ADDRESS_2.Length > 45)
                            {
                                iNextSpace = objDigitalKYCdata.CLIENT_ADDRESS_2.LastIndexOf(" ", length, StringComparison.Ordinal);
                                objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.Address.Substring(iNextSpace + 1);
                            }
                            else
                            {
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = null;
                            }
                        }
                        if (AadharData.Address.Length > 45 && AadharData.Address.Length > 90)
                        {
                            objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.Address.Substring(iNextSpace + 1);
                            iNextSpace = AadharData.Address.LastIndexOf(" ", length, StringComparison.Ordinal);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.Address.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                            objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.Address.Substring(iNextSpace + 1);
                        }
                    }
                    string DOB = AadharData.Dob;
                    if (DOB.Contains('-'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.Dob, "dd-MM-yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                    }
                    else if (DOB.Contains('/'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.Dob, "dd/MM/yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                    }
                    objDigitalKYCdata.Digi_gender = AadharData.Gender;
                    objDigitalKYCdata.Pincode = AadharData.Pc;
                    objDigitalKYCdata.CLIENT_CITY = AadharData.Vtc;

                    objDigitalKYCdata.CLIENT_COUNTRY = AadharData.Country;

                    objDigitalKYCdata.CLIENT_STATE = AadharData.State;

                    if (AadharData.Country == "India")
                    {
                        objDigitalKYCdata.CLIENT_COUNTRY = "101";
                    }
                    if (AadharData.State == "Maharashtra" || AadharData.State == "maharashtra")
                    {
                        objDigitalKYCdata.CLIENT_STATE = "027";
                    }

                    var verificationtype = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State_New {objDigitalKYCdata.CLIENT_STATE}").ToList()
                                            select new SelectListItem()
                                            {
                                                Text = details.StateName.ToString(),
                                                Value = details.StateCode1,
                                            }).ToList();
                    verificationtype.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.State = verificationtype;
                    var verificationtype1 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA_NEW {objDigitalKYCdata.CLIENT_COUNTRY}").ToList()
                                             select new SelectListItem()
                                             {
                                                 Text = details.Country.ToString(),
                                                 Value = details.CountryCode,
                                             }).ToList();
                    verificationtype1.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.getCountry = verificationtype1;

                }



                return Json(objDigitalKYCdata);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }

        }
        public ActionResult Digilocker(string Code)
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
        public ActionResult ViewDoc()
        {

            if (TempData["path"] != null)
            {
                FileStream fs = new FileStream(TempData["path"].ToString(), FileMode.Open, FileAccess.Read);
                return File(fs, "application/pdf");
            }
            else if (TempData["xml"] != null)
            {
                string respxml = TempData["xml"].ToString();
                INDO_FIN_NET.Models.UIDDATA objData = new Models.UIDDATA();

                XDocument AuthXMLResponse = XDocument.Parse(respxml);

                objData.uidtoken = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Attribute("tkn") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Attribute("tkn").Value : "";

                objData.aadhaarNumber = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Attribute("uid") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Attribute("uid").Value : "";


                objData.name = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("name") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("name").Value : "";
                objData.careOfPerson = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("careofperson") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("careofperson").Value : "";


                objData.dateOfBirth = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("dob") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("dob").Value : "";
                objData.gender = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("gender") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poi").Attribute("gender").Value : "";

                objData.Country = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("country") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("country").Value : "";
                objData.district = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("dist") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("dist").Value : "";
                objData.house = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("house") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("house").Value : "";
                objData.locality = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("loc") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("loc").Value : "";
                objData.landmark = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("lm") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("lm").Value : "";

                objData.pincode = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("pc") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("pc").Value : "";
                objData.state = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("state") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("state").Value : "";
                objData.city = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("vtc") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("vtc").Value : "";
                objData.Street = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("street") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Poa").Attribute("street").Value : "";

                objData.photo = AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Pht") != null ? AuthXMLResponse.Root.Element("CertificateData").Element("UidData").Element("Pht").Value : "";

                var prefix = "data:image/gif;base64,";
                ViewBag.Image = prefix + objData.photo;
                TempData["srt"] = objData;

                try
                {
                    System.IO.File.WriteAllText("C:\\Digilocker\\XmlData\\" + "aadharxml" + ".txt", respxml);
                }
                catch
                {
                }
                return View(objData);
            }
            else
            {
                return Content(TempData["Error"].ToString());
            }
        }
        public ActionResult DigiDoc()
        {
            return RedirectToAction("OrganisationDetails", "OrganisationLogin");
            // return Redirect("http://localhost:62667/ServiceProviderMainHome/QuickEnrollDashboard");            
        }


        public async Task<ActionResult> DigiAadharpdf()

        {
            var ABC = HttpContext.Session.GetString("PersonalId");
            ErrorLog error_log = new ErrorLog();
            try
            {
                long personlids = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

                var CustDatadetails = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigiAadharxml {personlids}").AsEnumerable().FirstOrDefault();
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
                objFinalDoc.DocumentId = (Convert.ToString(ABC));
                string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);


                string conn3 = _connectionString;

                using (SqlConnection con = new SqlConnection(conn3))
                {
                    SqlDataReader reader = null;
                    con.Open();

                    SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                    cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc.DocumentId));
                    cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc.DocMainType));
                    cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                    cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc.DocumentId));
                    cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                    cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                    cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc.Source));
                    cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

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
                else if (panadata == "DRIVINGLICENSE")
                {
                    string DrivingDOc = GetDrivingLicenseDoc(DrivingNo, access_token);
                    if (DrivingDOc == "Your Date of birth as per Aadhaar did not match with document details.Please contact concerned department for correction (if needed).")
                    {
                        return Json("Your Date of birth as per Aadhaar did not match with document details.Please contact concerned department for correction (if needed).");
                    }
                    TempData["path"] = path;
                }
                #endregion Driving

                #region Aadhar 
                else if (panadata == "AADHAAR_XML")
                {
                    string xmlEa = GetAadharXml(access_token);
                    TempData["xml"] = xmlEa;
                }

                #endregion Aadhar
                #region Both 
                else if (panadata == "Both")
                {
                    string PanDoc = GetPanDoc(PanNo, PANFullName, access_token);
                    string DrivingDOc = GetDrivingLicenseDoc(DrivingNo, access_token);
                    path = "C:\\Digilocker\\XmlData\\" + PanNo + ".pdf";
                    TempData["path"] = path;
                }

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
                else if (panadata == "Drvlwithxml")
                {
                    string DrivingDOc = GetDrivingLicenseDoc(DrivingNo, access_token);
                    string aadharxml = GetAadharXml(access_token);
                    TempData["path"] = path;
                }

                #endregion Drivingwithxml

                #region allthre
                else if (panadata == "Bothwithxml")
                {
                    string PanDoc = GetPanDoc(PanNo, PANFullName, access_token);
                    string DrivingDOc = GetDrivingLicenseDoc(DrivingNo, access_token);
                    string aadharxml = GetAadharXml(access_token);
                    TempData["path"] = path;
                }
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
                        //string repd = responsePD.Content.Trim('\"');
                        //byte[] dspd = Convert.FromBase64String(repd);
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
                        //var ReqParamUri = JsonConvert.DeserializeObject<FileAPI>(uriresp);
                        //string URI = ReqParamUri.uri;
                        if (URI == string.Empty || (URI == null))
                        {
                            //string Error = ReqParamUri.error;
                            //string ErrorDesc = ReqParamUri.error_description;
                            //TempData["Error"] = uriresp;
                            //HttpContext.Session.SetString("CheckPan", "PanFalse");
                            //return ErrorDesc;
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
                            using (SqlConnection connection3 = new SqlConnection(conn))
                            {
                                SqlCommand cmd3 = new SqlCommand("USP_DigiPanCard", connection3);
                                cmd3.CommandType = CommandType.StoredProcedure;
                                cmd3.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
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

                                connection3.Open();
                                SqlDataReader reader3 = cmd3.ExecuteReader();
                                if (reader3.Read())
                                {
                                    using (SqlConnection connection4 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd4 = new SqlCommand("USP_DigilockerPANFlag", connection4);
                                        cmd4.CommandType = CommandType.StoredProcedure;
                                        cmd4.Parameters.AddWithValue("@CustId", Convert.ToString(HttpContext.Session.GetString("PersonalId")));
                                        connection4.Open();
                                        SqlDataReader reader4 = cmd4.ExecuteReader();
                                        if (reader4.Read())
                                        {
                                            //var resp1 = reader["FLAG"].ToString();
                                        }
                                    }
                                }
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
                                    //SqlDataReader reader = null;
                                    con.Open();

                                    SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                    cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc.DocMainType));
                                    cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                    cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                    cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                    cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc.Source));
                                    cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

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
                                    //SqlDataReader reader = null;
                                    con.Open();

                                    SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                    cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc.DocMainType));
                                    cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                    cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                    cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                    cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc.Source));
                                    cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

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
                                    //SqlDataReader reader = null;
                                    con.Open();

                                    SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                    cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc.DocMainType));
                                    cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                    cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                    cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                    cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc.Source));
                                    cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

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

                                    SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                    cmd1.CommandType = CommandType.StoredProcedure;
                                    cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                    cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                    cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc.DocMainType));
                                    cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                    cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc.DocumentId));
                                    cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                    cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                    cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                    cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc.Source));
                                    cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

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

        public string GetDrivingLicenseDoc(string DrivingLicnse, string access_token)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
        ((sender, certificate, chain, sslPolicyErrors) => true);
                RestClient clientPD = new RestClient();

                string DocNm = "DRVLC";
                clientPD = new RestClient("https://apigateway.indofinnet.com/api/PullDocument?OrgID=IndoFin007&doctype=" + DocNm + "&IdentificationNO=" + DrivingLicnse + "&name=" + " " + "&Token=" + access_token);
                clientPD.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse responsePD = clientPD.Execute(request);

                var serializer = new JavaScriptSerializer();
                dynamic jsonObject = serializer.Deserialize<dynamic>(responsePD.Content);
                dynamic jsonDURI = serializer.Deserialize<dynamic>(jsonObject);
                string DURI = jsonDURI["uri"];
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_DigilockerErrorlog", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerId", Convert.ToInt64((HttpContext.Session.GetString("PersonalId"))));
                    cmd.Parameters.AddWithValue("@Request", Convert.ToString(responsePD));

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        var responce2 = reader["result"].ToString();

                        //string repd = responsePD.Content.Trim('\"');
                        //byte[] dspd = Convert.FromBase64String(repd);
                        //string xmlbase64 = responsePD.Content.Trim('\"');
                        string uriresp = DURI;
                        HttpContext.Session.SetString("checkDrivestatus", uriresp);
                        using (SqlConnection connection4 = new SqlConnection(conn))
                        {
                            SqlCommand cmd4 = new SqlCommand("USP_UpdateDigilockrlog", connection4);
                            cmd4.CommandType = CommandType.StoredProcedure;
                            cmd4.Parameters.AddWithValue("@Identity", Convert.ToInt32(responce2));
                            cmd4.Parameters.AddWithValue("@Response", uriresp);
                            cmd4.Parameters.AddWithValue("@Status", Convert.ToString(responsePD));
                            cmd4.Parameters.AddWithValue("@Digilockertype", "Aadhar");

                            connection4.Open();
                            SqlDataReader reader4 = cmd4.ExecuteReader();
                            if (reader4.Read())
                            {
                                //var resp1 = reader["FLAG"].ToString();
                            }
                        }
                        //var ReqParamUri = JsonConvert.DeserializeObject<FileAPI>(uriresp);
                        //string URI = ReqParamUri.uri;

                        if (DURI == string.Empty || (DURI == null))
                        {
                            //string Error = ReqParamUri.error;
                            //string ErrorDesc = ReqParamUri.error_description;
                            TempData["Error"] = uriresp;
                            HttpContext.Session.SetString("CheckDRLC", "DRLCFalse");
                            //return ErrorDesc;
                        }
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender, certificate, chain, sslPolicyErrors) => true);
                        var clientT = new RestClient("https://apigateway.indofinnet.com/api/FileAPI?OrgID=IndoFin007&URI=" + DURI + "&Token=" + access_token);

                        clientT.Timeout = -1;
                        var requestT = new RestRequest(Method.POST);
                        requestT.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        IRestResponse responseT = clientT.Execute(requestT);

                        var serializer1 = new JavaScriptSerializer();
                        dynamic jsonObject1 = serializer1.Deserialize<dynamic>(responseT.Content);
                        dynamic jsonDURI1 = serializer1.Deserialize<dynamic>(jsonObject1);
                        string pdfresponse = jsonDURI1["doc"];
                        string XMLdata = GetXml(DURI, access_token, DrivingLicnse);
                        {
                            if (XMLdata != "FAIL")
                            {
                                XDocument AuthXMLResponse = XDocument.Parse(XMLdata);
                                string DRVLC = AuthXMLResponse.Root.Attribute("number") != null ? AuthXMLResponse.Root.Attribute("number").Value : "";
                                string name = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("name") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("name").Value : "";
                                string swd = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("swd") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("swd").Value : "";
                                string dob = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("dob") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("dob").Value : "";
                                string gender = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("gender") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Attribute("gender").Value : "";
                                string country = AuthXMLResponse.Root.Element("IssuedBy").Element("Organization").Element("Address").Attribute("country") != null ? AuthXMLResponse.Root.Element("IssuedBy").Element("Organization").Element("Address").Attribute("country").Value : "";
                                string ORGname = AuthXMLResponse.Root.Element("IssuedBy").Element("Organization").Attribute("name") != null ? AuthXMLResponse.Root.Element("IssuedBy").Element("Organization").Attribute("name").Value : "";
                                string Address = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Element("Address").Attribute("line1") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Element("Address").Attribute("line1").Value : "";
                                string photo = AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Element("Photo") != null ? AuthXMLResponse.Root.Element("IssuedTo").Element("Person").Element("Photo").Value : "";
                                byte[] DrivePhoto = Encoding.ASCII.GetBytes(photo);
                                HttpContext.Session.SetString("DrivePhoto", photo);// = photo;

                                string[] Name = (name).Split(' ');

                                string fname = Name[0];
                                string lname = Name[1];
                                using (SqlConnection connection3 = new SqlConnection(conn))
                                {
                                    SqlCommand cmd3 = new SqlCommand("USP_DigiDrivingcard", connection3);
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    cmd3.Parameters.AddWithValue("@name", name);
                                    cmd3.Parameters.AddWithValue("@firstname", fname);
                                    cmd3.Parameters.AddWithValue("@swd", swd);
                                    cmd3.Parameters.AddWithValue("@lastname", lname);
                                    cmd3.Parameters.AddWithValue("@dob", dob);
                                    cmd3.Parameters.AddWithValue("@gender", gender);
                                    cmd3.Parameters.AddWithValue("@country", country);
                                    cmd3.Parameters.AddWithValue("@ORGname", ORGname);
                                    cmd3.Parameters.AddWithValue("@Address", Address);
                                    cmd3.Parameters.AddWithValue("@photo", DrivePhoto);
                                    cmd3.Parameters.AddWithValue("@DRVLC", DRVLC);

                                    connection3.Open();
                                    SqlDataReader reader3 = cmd3.ExecuteReader();
                                    if (reader3.Read())
                                    {
                                        using (SqlConnection connection4 = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd4 = new SqlCommand("USP_DigilockerDRLCFlag", connection4);
                                            cmd4.CommandType = CommandType.StoredProcedure;
                                            cmd4.Parameters.AddWithValue("@CustId", Convert.ToString(HttpContext.Session.GetString("PersonalId")));
                                            connection4.Open();
                                            SqlDataReader reader4 = cmd4.ExecuteReader();
                                            if (reader4.Read())
                                            {
                                                //var resp1 = reader["FLAG"].ToString();
                                            }
                                        }
                                    }
                                }
                                HttpContext.Session.SetString("CheckDRLC", "DRLCTrue");
                                string dataValue = TempData["FlagDoc"].ToString();

                                if (dataValue == "DRIVINGLICENSE")
                                {

                                    ClsDocDetails objFinalDoc = new ClsDocDetails();

                                    byte[] ds = Convert.FromBase64String(pdfresponse);
                                    objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    objFinalDoc.DocDetails = ds;
                                    objFinalDoc.DocName = "DriveDigi.pdf";
                                    extension = objFinalDoc.DocName.Split('.').LastOrDefault();

                                    objFinalDoc.DocMainType = "DL";
                                    objFinalDoc.documentCategory = "Driving Lincence";
                                    objFinalDoc.Source = "FromDigiLocker";
                                    objFinalDoc.documentTypeId = "Driving Lincence";
                                    objFinalDoc.DocCategoryCode = "6";
                                    objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                    string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                                    string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                                    string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic", objFinalDoc.DocName);

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
                                        JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic\\Drvlc.jpg"));
                                    }

                                    byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic\\Drvlc.jpg"));
                                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                    IMapper mapper = config.CreateMapper();
                                    ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc);

                                    string conn3 = _connectionString;

                                    using (SqlConnection con = new SqlConnection(conn3))
                                    {
                                        //SqlDataReader reader = null;
                                        con.Open();

                                        SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                        cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc.DocName));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc.DocCategoryCode));
                                        cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc.DocMainType));
                                        cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                        cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc.documentCategory));
                                        cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc.Source));
                                        cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

                                        cmd1.ExecuteNonQuery();


                                    }

                                }

                                else if (dataValue == "Both")
                                {
                                    ClsDocDetails objFinalDoc1 = new ClsDocDetails();

                                    byte[] ds1 = Convert.FromBase64String(pdfresponse);
                                    objFinalDoc1.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    objFinalDoc1.DocDetails = ds1;
                                    objFinalDoc1.DocName = "DriveDigi.pdf";
                                    extension = objFinalDoc1.DocName.Split('.').LastOrDefault();
                                    objFinalDoc1.DocType = objDetails.Database.ExecuteSqlRaw($"USP_GetDocumentType {("." + extension)}");
                                    objFinalDoc1.DocMainType = "DL";
                                    objFinalDoc1.documentCategory = "Driving Lincence";
                                    objFinalDoc1.Source = "FromDigiLocker";
                                    objFinalDoc1.DocCategoryCode = "6";

                                    objFinalDoc1.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                    string DocumentsDetails1 = JsonConvert.SerializeObject(objFinalDoc1);
                                    string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                                    string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic", objFinalDoc1.DocName);

                                    byte[] bytes = Convert.FromBase64String(pdfresponse);
                                    System.IO.File.WriteAllBytesAsync(FilePath, bytes);
                                    Document pdfDocument = new Document(FilePath);

                                    foreach (var page in pdfDocument.Pages)
                                    {
                                        // Define Resolution
                                        Resolution2 resolution = new Resolution2(1600);
                                        JpegDevice JpegDevice = new JpegDevice(600, 750, resolution);
                                        JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic\\Drvlc.jpg"));
                                    }

                                    byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic\\Drvlc.jpg"));
                                    var config1 = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                    IMapper mapper1 = config1.CreateMapper();
                                    ClsDocumentDetails objResult1 = mapper1.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc1);
                                    string conn3 = _connectionString;

                                    using (SqlConnection con = new SqlConnection(conn3))
                                    {
                                        //SqlDataReader reader = null;
                                        con.Open();

                                        SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                        cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc1.DocName));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc1.DocCategoryCode));
                                        cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc1.DocMainType));
                                        cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                        cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc1.documentCategory));
                                        cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc1.Source));
                                        cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

                                        cmd1.ExecuteNonQuery();


                                    }
                                }
                                else if (dataValue == "Drvlwithxml")
                                {
                                    ClsDocDetails objFinalDoc1 = new ClsDocDetails();

                                    byte[] ds1 = Convert.FromBase64String(pdfresponse);
                                    objFinalDoc1.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    objFinalDoc1.DocDetails = ds1;
                                    objFinalDoc1.DocName = "DriveDigi.pdf";
                                    extension = objFinalDoc1.DocName.Split('.').LastOrDefault();
                                    objFinalDoc1.DocType = objDetails.Database.ExecuteSqlRaw($"USP_GetDocumentType {("." + extension)}");
                                    objFinalDoc1.DocMainType = "DL";
                                    objFinalDoc1.documentCategory = "Driving Lincence";
                                    objFinalDoc1.Source = "FromDigiLocker";
                                    objFinalDoc1.DocCategoryCode = "6";
                                    objFinalDoc1.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                    string DocumentsDetails1 = JsonConvert.SerializeObject(objFinalDoc1);
                                    string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                                    string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic", objFinalDoc1.DocName);

                                    byte[] bytes = Convert.FromBase64String(pdfresponse);
                                    System.IO.File.WriteAllBytesAsync(FilePath, bytes);
                                    Document pdfDocument = new Document(FilePath);

                                    foreach (var page in pdfDocument.Pages)
                                    {
                                        // Define Resolution
                                        Resolution2 resolution = new Resolution2(1600);
                                        JpegDevice JpegDevice = new JpegDevice(600, 750, resolution);
                                        JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic\\Drvlc.jpg"));
                                    }

                                    byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic\\Drvlc.jpg"));
                                    var config1 = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                    IMapper mapper1 = config1.CreateMapper();
                                    ClsDocumentDetails objResult1 = mapper1.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc1);
                                    string conn3 = _connectionString;

                                    using (SqlConnection con = new SqlConnection(conn3))
                                    {
                                        //SqlDataReader reader = null;
                                        con.Open();

                                        SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                        cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc1.DocName));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc1.DocCategoryCode));
                                        cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc1.DocMainType));
                                        cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                        cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc1.documentCategory));
                                        cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc1.Source));
                                        cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

                                        cmd1.ExecuteNonQuery();


                                    }

                                }

                                else if (dataValue == "Bothwithxml")
                                {
                                    ClsDocDetails objFinalDoc1 = new ClsDocDetails();

                                    byte[] ds1 = Convert.FromBase64String(pdfresponse);
                                    objFinalDoc1.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    objFinalDoc1.DocDetails = ds1;
                                    objFinalDoc1.DocName = "DriveDigi.pdf";
                                    extension = objFinalDoc1.DocName.Split('.').LastOrDefault();

                                    objDetails.AdmCustomerDocuments.FromSqlRaw($"USP_GetDocumentType {objFinalDoc1.DocType}");
                                    objFinalDoc1.DocMainType = "DL";
                                    objFinalDoc1.documentCategory = "Driving Lincence";
                                    objFinalDoc1.Source = "FromDigiLocker";
                                    objFinalDoc1.DocCategoryCode = "6";

                                    objFinalDoc1.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                                    string DocumentsDetails1 = JsonConvert.SerializeObject(objFinalDoc1);
                                    string TimeStamp = Convert.ToDateTime(DateTime.Now).ToString();

                                    string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic", objFinalDoc1.DocName);

                                    byte[] bytes = Convert.FromBase64String(pdfresponse);
                                    System.IO.File.WriteAllBytesAsync(FilePath, bytes);
                                    Document pdfDocument = new Document(FilePath);

                                    foreach (var page in pdfDocument.Pages)
                                    {
                                        // Define Resolution
                                        Resolution2 resolution = new Resolution2(1600);
                                        JpegDevice JpegDevice = new JpegDevice(600, 750, resolution);
                                        JpegDevice.Process(pdfDocument.Pages[page.Number], Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic\\Drvlc.jpg"));
                                    }

                                    byte[] imgData = System.IO.File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\DigiDrivinglic\\Drvlc.jpg"));
                                    var config1 = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                                    IMapper mapper1 = config1.CreateMapper();
                                    ClsDocumentDetails objResult1 = mapper1.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc1);
                                    string conn3 = _connectionString;

                                    using (SqlConnection con = new SqlConnection(conn3))
                                    {
                                        //SqlDataReader reader = null;
                                        con.Open();

                                        SqlCommand cmd1 = new SqlCommand("USP_AddDocuments", con);
                                        cmd1.CommandType = CommandType.StoredProcedure;
                                        cmd1.Parameters.Add(new SqlParameter("@CustomerDetailId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@document", imgData));
                                        cmd1.Parameters.Add(new SqlParameter("@docName", objFinalDoc1.DocName));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategoryCode", objFinalDoc1.DocCategoryCode));
                                        cmd1.Parameters.Add(new SqlParameter("@docTypeId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@docMainType", objFinalDoc1.DocMainType));
                                        cmd1.Parameters.Add(new SqlParameter("@createdBy", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentId", objFinalDoc1.DocumentId));
                                        cmd1.Parameters.Add(new SqlParameter("@DocumentIdDate", null));
                                        cmd1.Parameters.Add(new SqlParameter("@Latitude_Longitude", ""));
                                        cmd1.Parameters.Add(new SqlParameter("@documentCategory", objFinalDoc1.documentCategory));
                                        cmd1.Parameters.Add(new SqlParameter("@Source", objFinalDoc1.Source));
                                        cmd1.Parameters.Add(new SqlParameter("@Prediction", ""));

                                        cmd1.ExecuteNonQuery();


                                    }
                                }
                            }
                            else
                            {
                                HttpContext.Session.SetString("CheckDRLC", "DRLCFalse");// = "DRLCFalse";

                            }

                        }

                        return pdfresponse;
                    }
                }
            }


            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                var result1 = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();

                if (result1.IsDigilDrlcsumbitted == false)
                {
                    ViewBag.DRLCFalse = "DRLCFalse";
                    HttpContext.Session.SetString("CheckDRLC", "DRLCFalse");
                }
                string abc = "pdf not generate";
                return abc;
            }

            return DrivingLicnse;
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


                string UserId = Convert.ToString(HttpContext.Session.GetString("UseID"));
                using (SqlConnection connection3 = new SqlConnection(conn))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_DigiAadharxml", connection3);
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
                    //cmd3.Parameters.AddWithValue("@createdBy", UserId);
                    connection3.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {
                        // var  resp = reader["FLAG"].ToString();
                        using (SqlConnection connection4 = new SqlConnection(conn))
                        {
                            SqlCommand cmd4 = new SqlCommand("USP_DigilockerAdharFlag", connection4);
                            cmd4.CommandType = CommandType.StoredProcedure;
                            cmd4.Parameters.AddWithValue("@CustId", Convert.ToString(HttpContext.Session.GetString("PersonalId")));


                            connection4.Open();
                            SqlDataReader reader4 = cmd4.ExecuteReader();
                            if (reader4.Read())
                            {
                                //var resp1 = reader["FLAG"].ToString();

                            }
                        }
                    }
                }
                HttpContext.Session.SetString("CheckAadhar", "AadharTrue");// = "AadharTrue";
                DigiAadharpdf();
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
        public JsonResult GetAadharXmlData(long? PersonalId)

        {
            ErrorLog error_log = new ErrorLog();
            try

            {
                //###########//

                string IsREkyctrue = HttpContext.Session.GetString("CustIDRekyc");
                if (IsREkyctrue != null)
                {
                    var AadharData = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharXmlData {PersonalId}").AsEnumerable().FirstOrDefault();
                    string aadharnm = AadharData.AadharName;
                    string[] name = aadharnm.Split(' ');
                    string CustnoFromCBS = HttpContext.Session.GetString("CustIDRekyc");
                    string Ac_No = HttpContext.Session.GetString("Acc_No");
                    var RekyCustomerDetails = objDetails.Set<AdmRekycCustomerDetail>().FromSqlRaw("EXEC usp_TogetRekycCustomerDetails @CustomerNo={0}, @Ac_no={1}", CustnoFromCBS, Ac_No).AsEnumerable().FirstOrDefault();

                    objDigitalKYCdata.FirstnameStatus = string.Equals(name[0].ToLower(), RekyCustomerDetails.CustomerFirstname.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.fnamerekyc = objDigitalKYCdata.FirstnameStatus;
                    objDigitalKYCdata.MiddlenameStatus = string.Equals(name[1].ToLower(), RekyCustomerDetails.CustomerMiddlename.ToLower()) ? "Match" : "Does Not Match";
                    //.Mnamerekyc = objDigitalKYCdata.MiddlenameStatus;
                    objDigitalKYCdata.LastnameStatus = string.Equals(name[2].ToLower(), RekyCustomerDetails.CustomerLastname.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.Lnamerekyc = objDigitalKYCdata.LastnameStatus;
                    objDigitalKYCdata.DobStatus = string.Equals(AadharData.AadharDob.ToLower(), RekyCustomerDetails.customerDOB.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.DobStatus = objDigitalKYCdata.DobStatus;
                    objDigitalKYCdata.GenderStatus = string.Equals(AadharData.AadharGender.ToLower(), RekyCustomerDetails.CustomerGender.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.GenderStatus = objDigitalKYCdata.GenderStatus;
                    objDigitalKYCdata.CountryStatus = string.Equals(AadharData.Country.ToLower(), RekyCustomerDetails.CustomerCountryID.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.CountryStatus = objDigitalKYCdata.CountryStatus;
                    objDigitalKYCdata.PincodeStatus = string.Equals(AadharData.PinCode.ToLower(), RekyCustomerDetails.CustomerPincode.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.Pincode = objDigitalKYCdata.Pincode;
                    objDigitalKYCdata.STATEStatus = string.Equals(AadharData.State.ToLower(), RekyCustomerDetails.CustomerState.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.State = objDigitalKYCdata.CLIENT_STATE;
                    objDigitalKYCdata.CITYStatus = string.Equals(AadharData.Vtc.ToLower(), RekyCustomerDetails.CustomerCity.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.CityRkyc = objDigitalKYCdata.CITYStatus;

                    objDigitalKYCdata.ADDRESSStatus1 = string.Equals(AadharData.AadharAddress.ToLower(), RekyCustomerDetails.CustomerAdd1.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.CLIENT_ADDRESS_1 = objDigitalKYCdata.CLIENT_ADDRESS_1;
                    objDigitalKYCdata.ADDRESSStatus2 = string.Equals(AadharData.AadharAddress.ToLower(), RekyCustomerDetails.CustomerAdd2.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.ADDRESSStatus3 = string.Equals(AadharData.AadharAddress.ToLower(), RekyCustomerDetails.CustomerAdd3.ToLower()) ? "Match" : "Does Not Match";

                    //###########//
                    objDigitalKYCdata.Digilock_Afirstname = name[0];
                    objDigitalKYCdata.Digilock_Amiddlename = name[1];
                    objDigitalKYCdata.Digilock_Alastname = name[2];
                    objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress;

                    int length = 45;
                    if (AadharData.AadharAddress.Length <= 45)
                    {
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress;
                    }
                    else
                    {
                        int iNextSpace = AadharData.AadharAddress.LastIndexOf(" ", length, StringComparison.Ordinal);
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                        if (AadharData.AadharAddress.Length > 45 && AadharData.AadharAddress.Length <= 90)
                        {
                            AadharData.AadharAddress = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = AadharData.AadharAddress;
                            if (objDigitalKYCdata.CLIENT_ADDRESS_2.Length > 45)
                            {
                                iNextSpace = objDigitalKYCdata.CLIENT_ADDRESS_2.LastIndexOf(" ", length, StringComparison.Ordinal);
                                objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            }
                            else
                            {
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = null;
                            }
                        }
                        if (AadharData.AadharAddress.Length > 45 && AadharData.AadharAddress.Length > 90)
                        {
                            objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            iNextSpace = AadharData.AadharAddress.LastIndexOf(" ", length, StringComparison.Ordinal);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                            objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                        }
                    }

                    string DOB = AadharData.AadharDob;
                    if (DOB.Contains('-'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.AadharDob, "dd-MM-yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                    }
                    else if (DOB.Contains('/'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.AadharDob, "dd/MM/yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                    }
                    objDigitalKYCdata.Digi_gender = AadharData.AadharGender;
                    objDigitalKYCdata.Pincode = AadharData.PinCode;
                    objDigitalKYCdata.CLIENT_CITY = AadharData.Vtc;

                    objDigitalKYCdata.CLIENT_COUNTRY = AadharData.Country;

                    objDigitalKYCdata.CLIENT_STATE = AadharData.State;

                    if (AadharData.Country == "India")
                    {
                        objDigitalKYCdata.CLIENT_COUNTRY = "101";
                    }
                    if (AadharData.State == "Maharashtra" || AadharData.State == "maharashtra")
                    {
                        objDigitalKYCdata.CLIENT_STATE = "027";
                    }

                    var verificationtype = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State_New {objDigitalKYCdata.CLIENT_STATE}").ToList()
                                            select new SelectListItem()
                                            {
                                                Text = details.StateName.ToString(),
                                                Value = details.StateCode1,
                                            }).ToList();
                    verificationtype.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.State = verificationtype;
                    var verificationtype1 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA_NEW {objDigitalKYCdata.CLIENT_COUNTRY}").ToList()
                                             select new SelectListItem()
                                             {
                                                 Text = details.Country.ToString(),
                                                 Value = details.CountryCode,
                                             }).ToList();
                    verificationtype1.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.getCountry = verificationtype1;


                }
                else
                {
                    var AadharData = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharXmlData {PersonalId}").AsEnumerable().FirstOrDefault();
                    string aadharnm = AadharData.AadharName;
                    string[] name = aadharnm.Split(' ');

                    objDigitalKYCdata.Digilock_Afirstname = name[0];

                    objDigitalKYCdata.Digilock_Amiddlename = name[1];
                    objDigitalKYCdata.Digilock_Alastname = name[2];
                    objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress;

                    int length = 45;
                    if (AadharData.AadharAddress.Length <= 45)
                    {
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress;
                    }
                    else
                    {
                        int iNextSpace = AadharData.AadharAddress.LastIndexOf(" ", length, StringComparison.Ordinal);
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                        if (AadharData.AadharAddress.Length > 45 && AadharData.AadharAddress.Length <= 90)
                        {
                            AadharData.AadharAddress = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = AadharData.AadharAddress;
                            if (objDigitalKYCdata.CLIENT_ADDRESS_2.Length > 45)
                            {
                                iNextSpace = objDigitalKYCdata.CLIENT_ADDRESS_2.LastIndexOf(" ", length, StringComparison.Ordinal);
                                objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            }
                            else
                            {
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = null;
                            }
                        }
                        if (AadharData.AadharAddress.Length > 45 && AadharData.AadharAddress.Length > 90)
                        {
                            objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            iNextSpace = AadharData.AadharAddress.LastIndexOf(" ", length, StringComparison.Ordinal);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                            objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                        }
                    }

                    string DOB = AadharData.AadharDob;
                    if (DOB.Contains('-'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.AadharDob, "dd-MM-yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                    }
                    else if (DOB.Contains('/'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.AadharDob, "dd/MM/yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                    }
                    objDigitalKYCdata.Digi_gender = AadharData.AadharGender;
                    objDigitalKYCdata.Pincode = AadharData.PinCode;
                    objDigitalKYCdata.CLIENT_CITY = AadharData.Vtc;

                    objDigitalKYCdata.CLIENT_COUNTRY = AadharData.Country;

                    objDigitalKYCdata.CLIENT_STATE = AadharData.State;

                    if (AadharData.Country == "India")
                    {
                        objDigitalKYCdata.CLIENT_COUNTRY = "101";
                    }
                    if (AadharData.State == "Maharashtra" || AadharData.State == "maharashtra")
                    {
                        objDigitalKYCdata.CLIENT_STATE = "027";
                    }

                    var verificationtype = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State_New {objDigitalKYCdata.CLIENT_STATE}").ToList()
                                            select new SelectListItem()
                                            {
                                                Text = details.StateName.ToString(),
                                                Value = details.StateCode1,
                                            }).ToList();
                    verificationtype.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.State = verificationtype;
                    var verificationtype1 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA_NEW {objDigitalKYCdata.CLIENT_COUNTRY}").ToList()
                                             select new SelectListItem()
                                             {
                                                 Text = details.Country.ToString(),
                                                 Value = details.CountryCode,
                                             }).ToList();
                    verificationtype1.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.getCountry = verificationtype1;

                }



                return Json(objDigitalKYCdata);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }

        }





        public ActionResult QEMoblieOTPSend(string MobileNumber)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var mob = "";
                var mob1 = "";
                if (MobileNumber != null)
                {
                    mob1 = MobileNumber;
                    mob = ObjTripleDes.Decrypt(mob1);
                }
                else
                {
                    mob = HttpContext.Session.GetString("MOBNO");
                }

                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
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

        public ActionResult Dedupemob(string MobileNumber)

        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                var client = new RestClient("https://cbs.indofinnet.com/api/MobileNumber_CBS?MobileNo=" + MobileNumber);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                string res = response.Content;
                res = res.Replace(@"\", "");
                var serializer = new JavaScriptSerializer();
                dynamic res1 = serializer.Deserialize<dynamic>(res);
                string[] s = res1.Split(',');
                return Json(s);

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
            return View();
        }

     public ActionResult QEMobileOTPAuthenticate(string OTP, string MobileNumber)

        {
            ErrorLog error_log = new ErrorLog();
            string DecryptedMobNo = ObjTripleDes.Decrypt(MobileNumber);
            HttpContext.Session.SetString("Mob", DecryptedMobNo);
            try
            {

                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                //var client = new RestClient("https://apigateway.indofinnet.com/api/VerifyOTP?OrgID=IndoFin007&mobileno=" + MobileNumber + "&otp=" + OTP);
                var client = new RestClient("https://cbs.indofinnet.com/api/VerifyOTPForRSSB?mobileno=" + DecryptedMobNo + "&otp=" + OTP);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;

                res = res.Replace(@"\", "");
                string s = res.Split('"')[1];

                return Json(s);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        public ActionResult RejectedCustDropout(string RejectCustId)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_RejectedCustDropOut", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustID", RejectCustId);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string CustDetailsId = reader["CustDetailsId"].ToString();
                        string QUICK = reader["IsQuickEnrollSubmit"].ToString();
                        string CAF = reader["IsCAFSubmit"].ToString();
                        string DOC = reader["IsDocumentSubmit"].ToString();
                        string SUMMARY = reader["IssummarysheetSubmit"].ToString();
                        string SAVEACC = reader["isSavingAcc"].ToString();
                        string digilocker = reader["IsDigilocker"].ToString();


                        string cus = HttpContext.Session.GetString("PersonalId12");


                        if (cus != null)
                        {
                            HttpContext.Session.SetString("PersonalId", cus);
                        }
                        else
                        {
                            HttpContext.Session.SetString("PersonalId", CustDetailsId);

                            if (SAVEACC == "False")
                            {
                                return Json("please click on the Saving Acc");
                            }
                            else if (SUMMARY == "False")
                            {
                                return Json("please click on the SummerySheet");
                            }
                            else if (DOC == "False")
                            {
                                return Json("please click on the document details");
                            }
                            else if (CAF == "False")
                            {
                                return Json("please click on the CAF");
                            }
                            else
                            {
                                return Json("User Not Found");
                            }


                        }

                    }
                    else
                    {
                        return Json("User Not Found");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }
        public ActionResult ReKycRejectedCustDropout(string RejectCustId)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_ReKycRejectedCustDropOut", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustID", RejectCustId);

                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        //string CustDetailsId = reader["CustDetailsId"].ToString();
                        string QUICK = reader["IsQuickEnrollSubmit"].ToString();
                        //string IsREkyctrue = HttpContext.Session.GetString("CustIDRekyc");
                        string DOC = reader["IsDocumentSubmit"].ToString();

                        string digilocker = reader["IsDigilocker"].ToString();
                        string RekycCaf = reader["IsReKycCAFSubmitted"].ToString();
                        string CbsId = reader["CbsId"].ToString();
                        string cus = HttpContext.Session.GetString("PersonalId12");


                        if (cus != null)
                        {
                            HttpContext.Session.SetString("CustIDRekyc", CbsId);
                            HttpContext.Session.SetString("PersonalId", RejectCustId);

                        }
                        else
                        {
                            HttpContext.Session.SetString("CustIDRekyc", CbsId);
                            HttpContext.Session.SetString("PersonalId", RejectCustId);

                            if (DOC == "False")
                            {
                                return Json("please click on the document details");
                            }
                            else if (RekycCaf == "False")
                            {
                                return Json("please click on the ReKycCAF");
                            }
                            else
                            {
                                return Json("User Not Found");
                            }


                        }

                    }
                    else
                    {
                        return Json("User Not Found");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }
        public JsonResult GetPanVerification(string PanNo)
        {
            ErrorLog error_log = new ErrorLog();
            try

            {
                //###########//

                string IsREkyctrue = HttpContext.Session.GetString("CustIDRekyc");
                if (IsREkyctrue != null)
                {
                    var AadharData = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharXmlData {IsREkyctrue}").AsEnumerable().FirstOrDefault();
                    string aadharnm = AadharData.AadharName;
                    string[] name = aadharnm.Split(' ');
                    string CustnoFromCBS = HttpContext.Session.GetString("CustIDRekyc");
                    string Ac_No = HttpContext.Session.GetString("Acc_No");
                    var RekyCustomerDetails = objDetails.Set<AdmRekycCustomerDetail>().FromSqlRaw("EXEC usp_TogetRekycCustomerDetails @CustomerNo={0}, @Ac_no={1}", CustnoFromCBS, Ac_No).AsEnumerable().FirstOrDefault();

                    objDigitalKYCdata.FirstnameStatus = string.Equals(name[0].ToLower(), RekyCustomerDetails.CustomerFirstname.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.fnamerekyc = objDigitalKYCdata.FirstnameStatus;
                    objDigitalKYCdata.MiddlenameStatus = string.Equals(name[1].ToLower(), RekyCustomerDetails.CustomerMiddlename.ToLower()) ? "Match" : "Does Not Match";
                    //.Mnamerekyc = objDigitalKYCdata.MiddlenameStatus;
                    objDigitalKYCdata.LastnameStatus = string.Equals(name[2].ToLower(), RekyCustomerDetails.CustomerLastname.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.Lnamerekyc = objDigitalKYCdata.LastnameStatus;
                    objDigitalKYCdata.DobStatus = string.Equals(AadharData.AadharDob.ToLower(), RekyCustomerDetails.customerDOB.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.DobStatus = objDigitalKYCdata.DobStatus;
                    objDigitalKYCdata.GenderStatus = string.Equals(AadharData.AadharGender.ToLower(), RekyCustomerDetails.CustomerGender.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.GenderStatus = objDigitalKYCdata.GenderStatus;
                    objDigitalKYCdata.CountryStatus = string.Equals(AadharData.Country.ToLower(), RekyCustomerDetails.CustomerCountryID.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.CountryStatus = objDigitalKYCdata.CountryStatus;
                    objDigitalKYCdata.PincodeStatus = string.Equals(AadharData.PinCode.ToLower(), RekyCustomerDetails.CustomerPincode.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.Pincode = objDigitalKYCdata.Pincode;
                    objDigitalKYCdata.STATEStatus = string.Equals(AadharData.State.ToLower(), RekyCustomerDetails.CustomerState.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.State = objDigitalKYCdata.CLIENT_STATE;
                    objDigitalKYCdata.CITYStatus = string.Equals(AadharData.Vtc.ToLower(), RekyCustomerDetails.CustomerCity.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.CityRkyc = objDigitalKYCdata.CITYStatus;

                    objDigitalKYCdata.ADDRESSStatus1 = string.Equals(AadharData.AadharAddress.ToLower(), RekyCustomerDetails.CustomerAdd1.ToLower()) ? "Match" : "Does Not Match";
                    //ViewBag.CLIENT_ADDRESS_1 = objDigitalKYCdata.CLIENT_ADDRESS_1;
                    objDigitalKYCdata.ADDRESSStatus2 = string.Equals(AadharData.AadharAddress.ToLower(), RekyCustomerDetails.CustomerAdd2.ToLower()) ? "Match" : "Does Not Match";
                    objDigitalKYCdata.ADDRESSStatus3 = string.Equals(AadharData.AadharAddress.ToLower(), RekyCustomerDetails.CustomerAdd3.ToLower()) ? "Match" : "Does Not Match";

                    //###########//
                    objDigitalKYCdata.Digilock_Afirstname = name[0];
                    objDigitalKYCdata.Digilock_Amiddlename = name[1];
                    objDigitalKYCdata.Digilock_Alastname = name[2];
                    objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress;

                    int length = 45;
                    if (AadharData.AadharAddress.Length <= 45)
                    {
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress;
                    }
                    else
                    {
                        int iNextSpace = AadharData.AadharAddress.LastIndexOf(" ", length, StringComparison.Ordinal);
                        objDigitalKYCdata.CLIENT_ADDRESS_1 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                        if (AadharData.AadharAddress.Length > 45 && AadharData.AadharAddress.Length <= 90)
                        {
                            AadharData.AadharAddress = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = AadharData.AadharAddress;
                            if (objDigitalKYCdata.CLIENT_ADDRESS_2.Length > 45)
                            {
                                iNextSpace = objDigitalKYCdata.CLIENT_ADDRESS_2.LastIndexOf(" ", length, StringComparison.Ordinal);
                                objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            }
                            else
                            {
                                objDigitalKYCdata.CLIENT_ADDRESS_3 = null;
                            }
                        }
                        if (AadharData.AadharAddress.Length > 45 && AadharData.AadharAddress.Length > 90)
                        {
                            objDigitalKYCdata.CLIENT_ADDRESS_1 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                            iNextSpace = AadharData.AadharAddress.LastIndexOf(" ", length, StringComparison.Ordinal);
                            objDigitalKYCdata.CLIENT_ADDRESS_2 = (string.Format("{0}", AadharData.AadharAddress.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim()));
                            objDigitalKYCdata.CLIENT_ADDRESS_3 = AadharData.AadharAddress.Substring(iNextSpace + 1);
                        }
                    }

                    string DOB = AadharData.AadharDob;
                    if (DOB.Contains('-'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.AadharDob, "dd-MM-yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                    }
                    else if (DOB.Contains('/'))
                    {
                        DateTime date = DateTime.ParseExact(AadharData.AadharDob, "dd/MM/yyyy", null);
                        objDigitalKYCdata.Digi_DOB = Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                    }
                    objDigitalKYCdata.Digi_gender = AadharData.AadharGender;
                    objDigitalKYCdata.Pincode = AadharData.PinCode;
                    objDigitalKYCdata.CLIENT_CITY = AadharData.Vtc;

                    objDigitalKYCdata.CLIENT_COUNTRY = AadharData.Country;

                    objDigitalKYCdata.CLIENT_STATE = AadharData.State;

                    if (AadharData.Country == "India")
                    {
                        objDigitalKYCdata.CLIENT_COUNTRY = "101";
                    }
                    if (AadharData.State == "Maharashtra" || AadharData.State == "maharashtra")
                    {
                        objDigitalKYCdata.CLIENT_STATE = "027";
                    }

                    var verificationtype = (from details in objDetails.StateCodes.FromSqlRaw($"USP_Get_State_New {objDigitalKYCdata.CLIENT_STATE}").ToList()
                                            select new SelectListItem()
                                            {
                                                Text = details.StateName.ToString(),
                                                Value = details.StateCode1,
                                            }).ToList();
                    verificationtype.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.State = verificationtype;
                    var verificationtype1 = (from details in objDetails.MasterCountries.FromSqlRaw($"USP_GET_COUNTRY_DATA_NEW {objDigitalKYCdata.CLIENT_COUNTRY}").ToList()
                                             select new SelectListItem()
                                             {
                                                 Text = details.Country.ToString(),
                                                 Value = details.CountryCode,
                                             }).ToList();
                    verificationtype1.Insert(0, new SelectListItem()
                    {
                        Text = "----Select----",
                        Value = string.Empty
                    });
                    ViewBag.getCountry = verificationtype1;


                }
                else
                {
                    var PANData = objDetails.AdmDigiPanCards.FromSqlRaw($"USP_GetVerifyPanData {(HttpContext.Session.GetString("PersonalId"))}").AsEnumerable().FirstOrDefault();

                    objDigitalKYCdata.Digi_FirstName = PANData.Firstname;

                    objDigitalKYCdata.Digi_MiddleName = PANData.Middlename;
                    objDigitalKYCdata.Digi_LastName = PANData.Lastname;
                    objDigitalKYCdata.Digi_PAN = PANData.Panno;

                }

                return Json(objDigitalKYCdata);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }


        }

        [HttpPost]
        public JsonResult NSDLPanVerification(string PanNo, ClsCustQuickEnrollment objQuickEnroll)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var client = new RestClient("https://apigateway.indofinnet.com/api/PanService?OrgID=IndoFin007&PanNo=" + PanNo);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                string res = response.Content;
                string res1 = res.Replace(@"\", "");
                string res12 = res1.Replace(@"\", "");
                string res2 = res12.Replace("{", ",");
                string res3 = res2.Replace("{", "");
                string[] ress = res3.Split('"');
                string objName = res3.Split(',')[2];
                objQuickEnroll.NSDL_FirstName = ress[12].ToString().Trim();
                objQuickEnroll.NSDL_MiddleName = ress[16].ToString().Trim();
                objQuickEnroll.NSDL_LastName = ress[20].ToString().Trim();
                objQuickEnroll.NSDL_PanTitle = ress[8].ToString().Trim();
                var aadharseeding = ress[36].ToString().Trim();
                //ViewBag.aadharseeding = aadharseeding;
                objQuickEnroll.NSDL_Result = aadharseeding;
                var agentid = HttpContext.Session.GetString("UseID");
                string UserId = Convert.ToString(HttpContext.Session.GetString("UseID"));
                using (SqlConnection cn = new SqlConnection(_connectionString))
                {
                    string s = (HttpContext.Session.GetString("PersonalId"));
                    cn.Open();
                    SqlCommand cmd = new SqlCommand("USP_InsertVerifyPanData", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustId", s);
                    HttpContext.Session.SetString("VerifyPan", s);
                    cmd.Parameters.AddWithValue("@PANNo", ObjTripleDes.Encrypt(ress[4].ToString().Trim()));
                    //cmd.Parameters.AddWithValue("@name", ChangedDetailMiddlename);
                    cmd.Parameters.AddWithValue("@firstname ", ress[12].ToString().Trim());
                    cmd.Parameters.AddWithValue("@middlename", ress[16].ToString().Trim());
                    cmd.Parameters.AddWithValue("@lastname", ress[20].ToString().Trim());
                    cmd.Parameters.AddWithValue("@title", ress[8].ToString().Trim());
                    cmd.Parameters.AddWithValue("@CreatedBy", agentid);

                    cmd.ExecuteNonQuery();
                    using (SqlConnection cn1 = new SqlConnection(_connectionString))

                    {
                        SqlCommand cmd4 = new SqlCommand("USP_CustomerPanVerifyFlag", cn1);
                        cmd4.CommandType = CommandType.StoredProcedure;
                        cmd4.Parameters.AddWithValue("@CustId", HttpContext.Session.GetString("PersonalId"));
                        cn1.Open();
                        SqlDataReader reader3 = cmd4.ExecuteReader();
                        if (reader3.Read())
                        {
                            // var uFlag = reader["RESULT"].ToString();
                        }
                    }
                    return Json(objQuickEnroll);
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        public ActionResult GetApprovedFlag(ServiceProvider1.Models.UserDetails.ClsSummeryDetails clsSummeryDetails, string selectedValue, string appType)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                if (appType == "PAN")
                {
                    HttpContext.Session.SetString("isPANApprove", "PAN");
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    TempData["PANRejectReasonTxt"] = "";
                    string conn = _connectionString;
                    using (SqlConnection connection10 = new SqlConnection(conn))
                    {
                        SqlCommand cmd10 = new SqlCommand("USP_PanApprove", connection10);
                        cmd10.CommandType = CommandType.StoredProcedure;

                        cmd10.Parameters.AddWithValue("@CustId", CustomerId);

                        connection10.Open();
                        cmd10.ExecuteNonQuery();
                        connection10.Close();
                    }
                    return Json("Success");
                }
                if (appType == "RPAN")
                {
                    HttpContext.Session.SetString("isPANApprove", "RPAN");
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    TempData["PANRejectReasonTxt"] = selectedValue;
                    string conn = _connectionString;
                    using (SqlConnection connection11 = new SqlConnection(conn))
                    {
                        SqlCommand cmd11 = new SqlCommand("USP_PanReject", connection11);
                        cmd11.CommandType = CommandType.StoredProcedure;

                        cmd11.Parameters.AddWithValue("@CustId", CustomerId);
                        cmd11.Parameters.AddWithValue("@PanRejectedReason", selectedValue);
                        connection11.Open();
                        cmd11.ExecuteNonQuery();
                        connection11.Close();
                    }
                    return Json("Success");
                }
                if (appType == "Aadhaar")
                {
                    HttpContext.Session.SetString("isAadhaarApprove", "Aadhaar");
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    TempData["AadhaarRejectReasonTxt"] = "";
                    string con1 = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(con1))
                    {
                        SqlCommand cmd10 = new SqlCommand("USP_AadharApprove", connection2);
                        cmd10.CommandType = CommandType.StoredProcedure;

                        cmd10.Parameters.AddWithValue("@CustId", CustomerId);

                        connection2.Open();
                        cmd10.ExecuteNonQuery();
                        connection2.Close();
                    }
                    return Json("Success");
                }
                if (appType == "RAadhaar")
                {
                    HttpContext.Session.SetString("isAadhaarApprove", "RAadhaar");
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    TempData["AadhaarRejectReasonTxt"] = selectedValue;
                    string conn = _connectionString;
                    using (SqlConnection connection11 = new SqlConnection(conn))
                    {
                        SqlCommand cmd11 = new SqlCommand("USP_AadharReject", connection11);
                        cmd11.CommandType = CommandType.StoredProcedure;

                        cmd11.Parameters.AddWithValue("@CustId", CustomerId);
                        cmd11.Parameters.AddWithValue("@AadharRejectedReason", selectedValue);
                        connection11.Open();
                        cmd11.ExecuteNonQuery();
                        connection11.Close();
                    }
                    return Json("Success");
                }
                if (appType == "Quick")
                {
                    HttpContext.Session.SetString("isQuickApprove", "Quick");
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");

                    TempData["QuickRejectReasonTxt"] = "";
                    string conn = _connectionString;
                    using (SqlConnection connection10 = new SqlConnection(conn))
                    {
                        SqlCommand cmd10 = new SqlCommand("USP_QuickEnrollApproved", connection10);
                        cmd10.CommandType = CommandType.StoredProcedure;

                        cmd10.Parameters.AddWithValue("@CustId", CustomerId);

                        connection10.Open();
                        cmd10.ExecuteNonQuery();
                        connection10.Close();
                    }
                    string conn50 = _connectionString;

                    using (SqlConnection connection50 = new SqlConnection(conn50))
                    {
                        SqlCommand cmd50 = new SqlCommand("USP_FlagMaintainData", connection50);
                        cmd50.CommandType = CommandType.StoredProcedure;
                        cmd50.Parameters.AddWithValue("@CustomerId", CustomerId);
                        connection50.Open();
                        SqlDataReader reader50 = cmd50.ExecuteReader();
                        if (reader50.Read())
                        {
                            var QuickEnrollRejectReason = reader50[21].ToString();
                            var CAFRejectReason = reader50[23].ToString();
                            var DocumentRejectReason = reader50[25].ToString();
                            var IpvRejectReason = reader50[27].ToString();
                            var SavingRejectedReason = reader50[60].ToString();

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

                }
                if (appType == "RQuick")
                {
                    HttpContext.Session.SetString("isQuickApprove", "RQuick"); //, "isQuickApprove" "CustomerId",

                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    HttpContext.Session.SetString("isQuickApprove", "RQuick");

                    TempData["QuickRejectReasonTxt"] = selectedValue;
                    //var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_QuickEnrollReject {CustomerId},{selectedValue}").AsEnumerable().FirstOrDefault();
                    string conn = _connectionString;
                    using (SqlConnection connection11 = new SqlConnection(conn))
                    {
                        SqlCommand cmd11 = new SqlCommand("USP_QuickEnrollReject", connection11);
                        cmd11.CommandType = CommandType.StoredProcedure;

                        cmd11.Parameters.AddWithValue("@CustId", CustomerId);
                        cmd11.Parameters.AddWithValue("@QuickEnrollrejectReason", selectedValue);
                        connection11.Open();
                        cmd11.ExecuteNonQuery();
                        connection11.Close();
                    }
                    return Json("Success");

                }
                if (appType == "Digi")
                {
                    //CAF page APPROVE 
                    HttpContext.Session.SetString("isDigiApprove", "Digi"); //(Convert.ToInt64(selectedValue))
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    TempData["DigiRejectReasonTxt"] = "";
                    string conn = _connectionString;
                    using (SqlConnection connection12 = new SqlConnection(conn))
                    {
                        SqlCommand cmd12 = new SqlCommand("USP_CAFpproved", connection12);
                        cmd12.CommandType = CommandType.StoredProcedure;

                        cmd12.Parameters.AddWithValue("@CustId", CustomerId);

                        connection12.Open();
                        cmd12.ExecuteNonQuery();
                        connection12.Close();
                    }
                    string conn51 = _connectionString;

                    using (SqlConnection connection51 = new SqlConnection(conn51))
                    {
                        SqlCommand cmd51 = new SqlCommand("USP_FlagMaintainData", connection51);
                        cmd51.CommandType = CommandType.StoredProcedure;
                        cmd51.Parameters.AddWithValue("@CustomerId", CustomerId);
                        connection51.Open();
                        SqlDataReader reader51 = cmd51.ExecuteReader();
                        if (reader51.Read())
                        {
                            var QuickEnrollRejectReason = reader51[21].ToString();
                            var CAFRejectReason = reader51[23].ToString();
                            var DocumentRejectReason = reader51[25].ToString();
                            var IpvRejectReason = reader51[27].ToString();
                            var SavingRejectedReason = reader51[60].ToString();

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
                }
                if (appType == "RDigi")
                {
                    //CAF PAGE REJECT
                    HttpContext.Session.SetString("isDigiApprove", "RDigi");
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    TempData["DigiRejectReasonTxt"] = selectedValue;
                    string conn = _connectionString;
                    using (SqlConnection connection13 = new SqlConnection(conn))
                    {
                        SqlCommand cmd13 = new SqlCommand("USP_CAFReject", connection13);
                        cmd13.CommandType = CommandType.StoredProcedure;

                        cmd13.Parameters.AddWithValue("@CustId", CustomerId);
                        cmd13.Parameters.AddWithValue("@CAFRejectReason", selectedValue);

                        connection13.Open();
                        cmd13.ExecuteNonQuery();
                        connection13.Close();
                    }
                    return Json("Success");
                }
                if (appType == "Doc")
                {
                    HttpContext.Session.SetString("isDocApprove", "Doc");
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    TempData["DocRejectReasonTxt"] = "";
                    string conn = _connectionString;
                    using (SqlConnection connection16 = new SqlConnection(conn))
                    {
                        SqlCommand cmd16 = new SqlCommand("USP_DocumentApproved", connection16);
                        cmd16.CommandType = CommandType.StoredProcedure;

                        cmd16.Parameters.AddWithValue("@CustId", CustomerId);


                        connection16.Open();
                        cmd16.ExecuteNonQuery();
                        connection16.Close();
                    }
                    return Json("Success");
                }
                if (appType == "RDoc")
                {
                    string CustomerId ;
                    TempData["DocRejectReasonTxt"] = selectedValue;
                    HttpContext.Session.SetString("isDocApprove", "RDoc");
                    if (HttpContext.Session.GetString("PersonalId") == null)
                    {
                    CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                        string conn = _connectionString;
                        using (SqlConnection connection17 = new SqlConnection(conn))
                        {
                            SqlCommand cmd17 = new SqlCommand("USP_DocumentReject", connection17);
                            cmd17.CommandType = CommandType.StoredProcedure;

                            cmd17.Parameters.AddWithValue("@CustId", CustomerId);
                            cmd17.Parameters.AddWithValue("@DocumentRejectReason", selectedValue);

                            connection17.Open();
                            cmd17.ExecuteNonQuery();
                            connection17.Close();
                        }
                    }
                    else
                    {
                      CustomerId = HttpContext.Session.GetString("PersonalId");
                        string con1 = _connectionString;
                        using (SqlConnection connection15 = new SqlConnection(con1))
                        {
                            SqlCommand cmd15 = new SqlCommand("USP_RekycDOCRejected", connection15);
                            cmd15.CommandType = CommandType.StoredProcedure;
                            cmd15.Parameters.AddWithValue("@CustId", CustomerId);
                            cmd15.Parameters.AddWithValue("@ReKycDOCRejectReason", selectedValue);
                            connection15.Open();
                            cmd15.ExecuteNonQuery();
                            connection15.Close();
                        }
                    }
                   
                    return Json("Success");
                }
                if (appType == "Saving")
                {
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    string conn22 = _connectionString;

                    //HttpContext.Session.SetString("isDocApprove", "RDoc");

                    TempData["SavingRejectedReason"] = "";
                    string conn = _connectionString;
                    using (SqlConnection connection14 = new SqlConnection(conn))
                    {
                        SqlCommand cmd14 = new SqlCommand("USP_SavingApproved", connection14);
                        cmd14.CommandType = CommandType.StoredProcedure;

                        cmd14.Parameters.AddWithValue("@CustId", CustomerId);


                        connection14.Open();
                        cmd14.ExecuteNonQuery();
                        connection14.Close();
                    }
                    using (SqlConnection connection22 = new SqlConnection(conn22))
                    {
                        SqlCommand cmd22 = new SqlCommand("USP_FlagMaintainData", connection22);
                        cmd22.CommandType = CommandType.StoredProcedure;
                        cmd22.Parameters.AddWithValue("@CustomerId", CustomerId);
                        connection22.Open();
                        SqlDataReader reader22 = cmd22.ExecuteReader();
                        if (reader22.Read())
                        {
                            var QuickEnrollRejectReason = reader22[21].ToString();
                            var CAFRejectReason = reader22[23].ToString();
                            var DocumentRejectReason = reader22[25].ToString();
                            var IpvRejectReason = reader22[27].ToString();
                            var SavingRejectedReason = reader22[60].ToString();

                            if (QuickEnrollRejectReason == "Not reject" && CAFRejectReason == "Not reject" && DocumentRejectReason == "Not reject" && IpvRejectReason == "Not reject" || SavingRejectedReason == "Not reject")
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

                }
                if (appType == "RSaving")//USP_SavingReject
                {
                    var CustomerId = HttpContext.Session.GetString("DAEditCustomerdetailId");
                    string conn23 = _connectionString;
                    using (SqlConnection connection23 = new SqlConnection(conn23))
                    {
                        SqlCommand cmd23 = new SqlCommand("USP_FlagMaintainData", connection23);
                        cmd23.CommandType = CommandType.StoredProcedure;

                        cmd23.Parameters.AddWithValue("@CustomerId", CustomerId);


                        connection23.Open();


                        SqlDataReader reader23 = cmd23.ExecuteReader();
                        if (reader23.Read())
                        {

                            var QuickEnrollRejectReason = reader23[21].ToString();
                            var CAFRejectReason = reader23[23].ToString();
                            var DocumentRejectReason = reader23[25].ToString();
                            var IpvRejectReason = reader23[27].ToString();
                            var SavingRejectedReason = reader23[60].ToString();

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
                    //HttpContext.Session.SetString("isIPVApprove", "RSaving");


                    TempData["SavingRejectedReason"] = selectedValue;
                    string conn = _connectionString;
                    using (SqlConnection connection15 = new SqlConnection(conn))
                    {
                        SqlCommand cmd15 = new SqlCommand("USP_SavingReject", connection15);
                        cmd15.CommandType = CommandType.StoredProcedure;

                        cmd15.Parameters.AddWithValue("@CustId", CustomerId);
                        cmd15.Parameters.AddWithValue("@SavingRejectedReason", selectedValue);

                        connection15.Open();
                        cmd15.ExecuteNonQuery();
                        connection15.Close();
                    }

                    return Json("Success");
                }
                if (appType == "IPV")
                {
                    HttpContext.Session.SetString("isIPVApprove", "IPV");
                    TempData["IPVRejectReasonTxt"] = "";
                    return Json("Success");
                }
                if (appType == "RIPV")
                {
                    HttpContext.Session.SetString("isIPVApprove", "RIPV");
                    TempData["IPVRejectReasonTxt"] = selectedValue;
                    return Json("Success");
                }
                if (appType == "ReKyc")
                {
                    var CustomerId = HttpContext.Session.GetString("PersonalId");
                    if (CustomerId != null)
                    {
                        HttpContext.Session.SetString("REKYCQ", "1");
                    }
                    TempData["SavingRejectedReason"] = "";
                    string conn = _connectionString;
                    using (SqlConnection connection14 = new SqlConnection(conn))
                    {
                        //SqlCommand cmd14 = new SqlCommand("USP_ReKycApproved", connection14);
                        SqlCommand cmd14 = new SqlCommand("USP_RKYCApproveCAFFlag", connection14);
                        cmd14.CommandType = CommandType.StoredProcedure;
                        cmd14.Parameters.AddWithValue("@CustId", CustomerId);
                        connection14.Open();
                        cmd14.ExecuteNonQuery();
                        connection14.Close();
                    }
                    //string conn22 = _connectionString;
                    //using (SqlConnection connection22 = new SqlConnection(conn22))
                    //{
                    //    SqlCommand cmd22 = new SqlCommand("USP_UpdateAddressSubmit", connection22);
                    //    cmd22.CommandType = CommandType.StoredProcedure;
                    //    cmd22.Parameters.AddWithValue("@CustomerId", CustomerId);
                    //    connection22.Open();
                    //    SqlDataReader reader22 = cmd22.ExecuteReader();
                    //    if (reader22.Read())
                    //    {
                    //        bool IsReKycRejected = (bool)reader22[0];


                    //        if (IsReKycRejected == true)
                    //        {
                    //            string result = "1";
                    //            return Json(result);
                    //        }
                    //        else
                    //        {
                    //            string result = "0";
                    //            return Json(result);
                    //        }

                    //    }
                    //}
                    return Json("Success");
                }
                //--------//
                if (appType == "ReKycDoc")
                {
                    var CustomerId = HttpContext.Session.GetString("PersonalId");
                    if (CustomerId != null)
                    {
                        HttpContext.Session.SetString("REKYCQ", "1");
                    }
                    TempData["SavingRejectedReason"] = "";
                    string conn = _connectionString;
                    using (SqlConnection connection14 = new SqlConnection(conn))
                    {
                        //SqlCommand cmd14 = new SqlCommand("USP_ReKycApproved", connection14);
                        SqlCommand cmd14 = new SqlCommand("USP_RKYCApproveDocFlag", connection14);
                        cmd14.CommandType = CommandType.StoredProcedure;
                        cmd14.Parameters.AddWithValue("@CustId", CustomerId);
                        connection14.Open();
                        cmd14.ExecuteNonQuery();
                        connection14.Close();
                    }


                    string conn22 = _connectionString;
                    using (SqlConnection connection22 = new SqlConnection(conn22))
                    {
                        SqlCommand cmd22 = new SqlCommand("USP_TocheckAddressSubmit", connection22);
                        cmd22.CommandType = CommandType.StoredProcedure;
                        cmd22.Parameters.AddWithValue("@CustId", CustomerId);
                        connection22.Open();
                        SqlDataReader reader22 = cmd22.ExecuteReader();
                        if (reader22.Read())
                        {
                            bool IsReKycCAF = (bool)reader22[0];
                            bool IsReKycDOC = (bool)reader22[1];


                            if (IsReKycCAF == true && IsReKycDOC == true)
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
                    return Json("Success");
                }
                //--------//
                if (appType == "RReKycCAF")
                {
                    var CustomerId = HttpContext.Session.GetString("PersonalId");

                    TempData["SavingRejectedReason"] = selectedValue;
                    string conn = _connectionString;
                    using (SqlConnection connection15 = new SqlConnection(conn))
                    {
                        SqlCommand cmd15 = new SqlCommand("USP_RekycCAFRejected", connection15);
                        cmd15.CommandType = CommandType.StoredProcedure;
                        cmd15.Parameters.AddWithValue("@CustId", CustomerId);
                        cmd15.Parameters.AddWithValue("@ReKycCAFRejectReason", selectedValue);
                        connection15.Open();
                        cmd15.ExecuteNonQuery();
                        connection15.Close();
                    }
                    string conn23 = _connectionString;
                    using (SqlConnection connection23 = new SqlConnection(conn23))
                    {
                        SqlCommand cmd23 = new SqlCommand("USP_TocheckRejectRekyc", connection23);
                        cmd23.CommandType = CommandType.StoredProcedure;
                        cmd23.Parameters.AddWithValue("@CustomerId", CustomerId);
                        connection23.Open();
                        SqlDataReader reader23 = cmd23.ExecuteReader();
                        if (reader23.Read())
                        {
                            bool IsReKycRejected = (bool)reader23[0];
                            if (IsReKycRejected == true)
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
                    return Json("Success");
                }
                //if (appType == "RReKyc")
                //{
                //    var CustomerId = HttpContext.Session.GetString("PersonalId");

                //    TempData["SavingRejectedReason"] = selectedValue;
                //    string conn = _connectionString;
                //    using (SqlConnection connection15 = new SqlConnection(conn))
                //    {
                //        SqlCommand cmd15 = new SqlCommand("USP_ReKycReject", connection15);
                //        cmd15.CommandType = CommandType.StoredProcedure;

                //        cmd15.Parameters.AddWithValue("@CustId", CustomerId);
                //        cmd15.Parameters.AddWithValue("@ReKycRejectReason ", selectedValue);

                //        connection15.Open();
                //        cmd15.ExecuteNonQuery();
                //        connection15.Close();
                //    }
                //    string conn23 = _connectionString;
                //    using (SqlConnection connection23 = new SqlConnection(conn23))
                //    {
                //        SqlCommand cmd23 = new SqlCommand("USP_UpdateAddressSubmit", connection23);
                //        cmd23.CommandType = CommandType.StoredProcedure;

                //        cmd23.Parameters.AddWithValue("@CustomerId", CustomerId);


                //        connection23.Open();


                //        SqlDataReader reader23 = cmd23.ExecuteReader();
                //        if (reader23.Read())
                //        {

                //            bool IsReKycRejected = (bool)reader23[0];


                //            if (IsReKycRejected == true)
                //            {
                //                string result = "1";
                //                return Json(result);
                //            }
                //            else
                //            {
                //                string result = "0";
                //                return Json(result);
                //            }
                //        }
                //    }

                //    return Json("Success");
                //}
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }



        public ActionResult FinalApproveCheck(string selectedValue)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                if (HttpContext.Session.GetString("isQuickApprove").ToString() == "Quick" && HttpContext.Session.GetString("isDigiApprove").ToString() == "Digi" && HttpContext.Session.GetString("isDocApprove").ToString() == "Doc" && selectedValue == "" && HttpContext.Session.GetString("isPANApprove").ToString() == "PAN" && HttpContext.Session.GetString("isIPVApprove").ToString() == "IPV")
                {
                    TempData["msg"] = "Approve";
                    return Json(TempData["msg"].ToString());
                }
                else
                {
                    string CafPdfReason = "";
                    string QuickReason = TempData["QuickRejectReasonTxt"].ToString();
                    string DigiReason = TempData["PANRejectReasonTxt"].ToString() + " " + TempData["AadhaarRejectReasonTxt"].ToString() + " " + TempData["DigiRejectReasonTxt"].ToString();
                    string DocReason = TempData["DocRejectReasonTxt"].ToString();
                    string IPVReason = TempData["IPVRejectReasonTxt"].ToString();
                    if (selectedValue != null)
                    {
                        CafPdfReason = selectedValue;
                    }

                    string reject = "Reject";

                    TempData["msg"] = "Reject";
                    return Json(new { reject, QuickReason, DigiReason, DocReason, CafPdfReason, IPVReason });

                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }
        public ActionResult EmailOTPServices(string Email, string OTP, string OTPforCheck)
        {
            if (Email != null && OTP != null)
            {
                HttpContext.Session.SetString("OTPforemail", OTP);
                string fromsms = "support@alphafinsoft.in";
                string tosms = Email;
                string strpass = "N3F3jShHbxT0";
                MailMessage message = new MailMessage();
                message.Subject = "One Time password for Email Verification.";
                message.Body = "Dear Sir / Madam,<br />" + "<br />" + "<br />" + "Your One Time Password(OTP) is :<br/>" + "<br/>" + "<b>" + OTP + "</b>" + "<br/>" + "<br/>" + "<br/>" + "Your OTP will expire in 5 min.<br/>" + "<br/>" + "Do not share your OTP with anyone.<br/>" + "<br/>" + "For any OTP related query please email us at support@alphafinsoft.in<br/>" + "<br/>" + "<br/>" + "Warm Regards,<br/>" + "Indofinnet Team.";
                message.From = new MailAddress(fromsms);
                message.To.Add(new MailAddress(tosms));
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.zoho.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromsms, strpass),
                    EnableSsl = true,
                };
                smtpClient.Send(message);


                return Json("Successfully Sent");
            }
            else if (OTPforCheck != null)
            {
                var emailOTPAuth = HttpContext.Session.GetString("OTPforemail");
                if (OTPforCheck == emailOTPAuth)
                {
                    return Json("success");

                }
                else
                {
                    return Json("Failed");
                }
            }
            else
            {

            }
            return Json(OTP);
            
           
        }




        public ActionResult Dedupe(string MobileNumber)
        {
            ErrorLog error_log = new ErrorLog();
            DEDUPE_GRID_MAIN objmain = new DEDUPE_GRID_MAIN();
            try
            {
                string Result = "";
                List<dedupegridlist> dedupegridlist = new List<dedupegridlist>();

                if (MobileNumber != null)
                {
                    var client = new RestClient("https://cbs.indofinnet.com/api/MobileNumber_CBS?MobileNo=" + MobileNumber);

                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                     Result = response.Content;
                    if (Result.Contains("Mobile No does Not Exists."))
                    {
                        return View();
                    }
                    else
                    {
                        Result = Result.Replace(@"\", "");
                        Result = Result.Substring(1, Result.Length - 2);

                        //dedupe objroot1 = JsonConvert.DeserializeObject<dedupe>(Result);
                        //if (objroot1.error != "Internal Server Error")
                        //{
                           var objroot = JsonConvert.DeserializeObject<List<dedupemob.Root>>(Result);
                            if (objroot != null)
                            {
                                if (objroot.Count > 0)
                                {

                                    for (int i = 0; i < objroot.Count; i++)
                                    {
                                        var data = objroot[i];
                                        if (data.custNo == 0)
                                        {
                                            return View();
                                        }
                                        dedupegridlist dedupegridobjmodel = new dedupegridlist();
                                        dedupegridobjmodel.custNo = data.custNo;

                                        dedupegridobjmodel.name = data.name;
                                        dedupegridobjmodel.panNo = data.panNo;

                                        dedupegridobjmodel.DedupeRadioFlag = Convert.ToString(i);
                                        dedupegridobjmodel.DedupeRadioFlag = "DedupeRadioFlag" + i;

                                        dedupegridlist.Add(dedupegridobjmodel);
                                    }
                                    objmain.dedupegridlists = dedupegridlist;
                                }
                                return PartialView("Views/KYCQuickEnroll/_Dedupe.cshtml", objmain);
                            }
                       // }
                        else { return View(); }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
            return View();
        }

        public ActionResult Dedupeacc(string CBSValue)

        {
            ErrorLog error_log = new ErrorLog();
            string data2 = CBSValue;
            string authorsList = data2.Split(',')[1];
            string data1 = authorsList;
            var serializer = new JavaScriptSerializer();
            dynamic Cont = serializer.Deserialize<dynamic>(data1);
            DEDUPE_GRID_MAIN1 objmain1 = new DEDUPE_GRID_MAIN1();
            try
            {
                List<dedupegridlists> dedupegridlist = new List<dedupegridlists>();

                if (data1 != null)
                {
                    var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSDataCustNo?custNo=" + Cont);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    string Result1 = response.Content;

                    Result1 = Result1.Replace(@"\", "");
                    //Result1 = Result1.Substring(1, Result1.Length - 2);
                    Dedupeaccno.Root1 objrooot1 = JsonConvert.DeserializeObject<Dedupeaccno.Root1>(Result1);

                    if (objrooot1.AccountNo.Count > 0)
                    {

                        for (int i = 0; i < objrooot1.AccountNo.Count; i++)
                        {
                            var data = objrooot1.AccountNo[i];
                            dedupegridlists dedupegridobjmodels = new dedupegridlists();

                            dedupegridobjmodels.AccountNo = objrooot1.AccountNo[i];

                            dedupegridobjmodels.DedupeAccFlags = Convert.ToString(i);
                            dedupegridobjmodels.DedupeAccFlags = "DedupeAccFlag" + i;

                            //dedupegridlists.a
                            dedupegridlist.Add(dedupegridobjmodels);
                        }
                        objmain1.dedupegridlistss = dedupegridlist;
                    }
                }

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
            return PartialView("Views/KYCQuickEnroll/_dedupeacc.cshtml", objmain1);
        }

        public ActionResult ReKycAcc1(string CBSValue)

        {
            ErrorLog error_log = new ErrorLog();
            string data2 = CBSValue;

            Rekyc_GRID_MAIN1 objmain1 = new Rekyc_GRID_MAIN1();
            try
            {
                List<Rekycgridlists> Rekycgridlist = new List<Rekycgridlists>();

                if (data2 != null)
                {
                    var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSDataCustNo?custNo=" + data2);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    string Result1 = response.Content;

                    Result1 = Result1.Replace(@"\", "");
                    //Result1 = Result1.Substring(1, Result1.Length - 2);
                    Dedupeaccno.Root1 objrooot1 = JsonConvert.DeserializeObject<Dedupeaccno.Root1>(Result1);

                    if (objrooot1.AccountNo.Count > 0)
                    {

                        for (int i = 0; i < objrooot1.AccountNo.Count; i++)
                        {
                            var data = objrooot1.AccountNo[i];
                            Rekycgridlists Rkycgridobjmodels = new Rekycgridlists();

                            Rkycgridobjmodels.AccountNo = objrooot1.AccountNo[i];

                            Rkycgridobjmodels.RekycAccFlags = Convert.ToString(i);
                            Rkycgridobjmodels.RekycAccFlags = "RekycAccFlags" + i;

                            //dedupegridlists.a
                            Rekycgridlist.Add(Rkycgridobjmodels);
                        }
                        objmain1.Rekycgridlistss = Rekycgridlist;
                    }
                }

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
            return PartialView("Views/KYCQuickEnroll/_ReKycAccPan.cshtml", objmain1);
        }
        public ActionResult ReKycAcc(string CBSValue)

        {
            ErrorLog error_log = new ErrorLog();
            string data2 = CBSValue;
            string authorsList = data2.Split(',')[1];
            string data1 = authorsList;
            var serializer = new JavaScriptSerializer();
            dynamic Cont = serializer.Deserialize<dynamic>(data1);

            Rekyc_GRID_MAIN1 objmain1 = new Rekyc_GRID_MAIN1();
            try
            {
                List<Rekycgridlists> Rekycgridlist = new List<Rekycgridlists>();

                if (data1 != null)
                {
                    var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSDataCustNo?custNo=" + Cont);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    string Result1 = response.Content;

                    Result1 = Result1.Replace(@"\", "");
                    //Result1 = Result1.Substring(1, Result1.Length - 2);
                    Dedupeaccno.Root1 objrooot1 = JsonConvert.DeserializeObject<Dedupeaccno.Root1>(Result1);

                    if (objrooot1.AccountNo.Count > 0)
                    {

                        for (int i = 0; i < objrooot1.AccountNo.Count; i++)
                        {
                            var data = objrooot1.AccountNo[i];
                            Rekycgridlists Rkycgridobjmodels = new Rekycgridlists();

                            Rkycgridobjmodels.AccountNo = objrooot1.AccountNo[i];

                            Rkycgridobjmodels.RekycAccFlags = Convert.ToString(i);
                            Rkycgridobjmodels.RekycAccFlags = "RekycAccFlags" + i;

                            //dedupegridlists.a
                            Rekycgridlist.Add(Rkycgridobjmodels);
                        }
                        objmain1.Rekycgridlistss = Rekycgridlist;
                    }
                }

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
            return PartialView("Views/KYCQuickEnroll/_ReKycAcc.cshtml", objmain1);
        }

        public ActionResult ReKycCust(string CBSValue)

        {
            ErrorLog error_log = new ErrorLog();
            string data2 = CBSValue;

            Rekyc_GRID_MAIN1 objmain1 = new Rekyc_GRID_MAIN1();
            try
            {
                List<Rekycgridlists> Rekycgridlist = new List<Rekycgridlists>();

                var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSDataCustNo?custNo=" + data2);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                string Result1 = response.Content;

                Result1 = Result1.Replace(@"\", "");

                string res = Result1.Split(':')[1];
                var msg = res.Split('"')[1];
                if (msg == "Account Not Found ")
                {
                    return Json(msg);
                }
                Dedupeaccno.Root1 objrooot1 = JsonConvert.DeserializeObject<Dedupeaccno.Root1>(Result1);
                if (objrooot1 != null)
                {
                    if (objrooot1.AccountNo.Count > 0)
                    {

                        for (int i = 0; i < objrooot1.AccountNo.Count; i++)
                        {
                            var data = objrooot1.AccountNo[i];
                            Rekycgridlists Rkycgridobjmodels = new Rekycgridlists();

                            Rkycgridobjmodels.AccountNo = objrooot1.AccountNo[i];

                            Rkycgridobjmodels.RekycAccFlags = Convert.ToString(i);
                            Rkycgridobjmodels.RekycAccFlags = "RekycAccFlags" + i;

                            //dedupegridlists.a
                            Rekycgridlist.Add(Rkycgridobjmodels);
                        }
                        objmain1.Rekycgridlistss = Rekycgridlist;
                    }
                    return PartialView("Views/KYCQuickEnroll/_ReKycCust.cshtml", objmain1);

                }
                else
                {
                    return Json("Server Error Occurred");
                }

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }
        }
        public ActionResult GETDOCUMENTDEDUPE(string QEVType, string type, ClsCustQuickEnrollment objExist)
        {
            if (QEVType != null && type != null)
            {
                if (QEVType == "P1")
                {
                    var pan = type.ToUpper();
                    var a = "";
                    var client = new RestClient("https://cbs.indofinnet.com/api/AadharandPan?PanNo=" + pan + "&AdharNo=" + a);

                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    string Result = response.Content;

                    Result = Result.Replace(@"\", "");
                    Result = Result.Substring(1, Result.Length - 2);
                    if (Result != null && Result != "")
                    {
                        dedupe objroot1 = JsonConvert.DeserializeObject<dedupe>(Result);
                        if (objroot1.error != "Internal Server Error")
                        {
                            string dedupe = objroot1.Msg;
                            if (dedupe != "Customer NOT Found ")
                            {
                                dcumentdedupe.Root objroot = JsonConvert.DeserializeObject<dcumentdedupe.Root>(Result);
                                string s = objroot.Name;
                                string s1 = s.Split(' ')[0];
                                string s2 = s.Split(' ')[1];
                                string s3 = s.Split(' ')[2];
                                objExist.existFirstName = s1;
                                objExist.ExistMiddleName = s2;
                                objExist.ExistLastName = s3;
                                objExist.ExistMobileNo = objroot.Mobile;
                                objExist.ExistEmail = objroot.EmailID;
                                objExist.ExistDOB = objroot.DOB;
                                objExist.ExistGender = objroot.SexCode;
                                objExist.ExistAddress1 = objroot.Add1;
                                objExist.ExistAddress2 = objroot.Add2;
                                objExist.ExistAddress3 = "";
                                objExist.ExistCity = objroot.District;
                                objExist.ExistPincode = objroot.PinCode;
                                objExist.State = objroot.State;
                                objExist.Country = objroot.Nationality;
                                return Json(objExist);
                            }
                            else
                            {
                                return View();
                            }
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
                else if (QEVType == "A")
                {
                    var pan = "";
                    var a = type;
                    var client = new RestClient("https://cbs.indofinnet.com/api/AadharandPan?PanNo=" + pan + "&AdharNo=" + a);

                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    string Result = response.Content;

                    Result = Result.Replace(@"\", "");
                    Result = Result.Substring(1, Result.Length - 2);
                    dedupe objroot1 = JsonConvert.DeserializeObject<dedupe>(Result);
                    string dedupe = objroot1.Msg;
                    if (dedupe != "Customer NOT Found ")
                    {
                        dcumentdedupe.Root objroot = JsonConvert.DeserializeObject<dcumentdedupe.Root>(Result);
                        string s = objroot.Name;
                        string s1 = s.Split(' ')[0];
                        string s2 = s.Split(' ')[1];
                        string s3 = s.Split(' ')[2];
                        objExist.existFirstName = s1;
                        objExist.ExistMiddleName = s2;
                        objExist.ExistLastName = s3;
                        objExist.ExistMobileNo = objroot.Mobile;
                        objExist.ExistEmail = objroot.EmailID;
                        objExist.ExistDOB = objroot.DOB;
                        objExist.ExistGender = objroot.SexCode;
                        objExist.ExistAddress1 = objroot.Add1;
                        objExist.ExistAddress2 = objroot.Add2;
                        objExist.ExistAddress3 = "";
                        objExist.ExistCity = objroot.District;
                        objExist.ExistPincode = objroot.PinCode;
                        objExist.State = objroot.State;
                        objExist.Country = objroot.Nationality;
                        return Json(objExist);
                    }


                }
            }
            else
            {
                return View();
            }
            return View();

        }

        public ActionResult GetCBSDataPan(string selectedValue, string appType)
        {
            if (selectedValue == "Pan Number")
            {
                ErrorLog error_log = new ErrorLog();
                DEDUPE_GRID_PAN objmain1 = new DEDUPE_GRID_PAN();
                try
                {
                    List<RekycDedupepan> RekycDedupepan = new List<RekycDedupepan>();

                    var pan = appType;
                    var adhaar = "";
                    var client = new RestClient("https://cbsintegration.azurewebsites.net/api/Pan_CBS?PAN=" + pan);// + "&AdharNo=" + adhaar);

                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    IRestResponse response = client.Execute(request);
                    string Result = response.Content;
                    Result = Result.Replace(@"\", "");
                    Result = Result.Substring(1, Result.Length - 2);
                    String msg = Result.Split(":")[2];
                    msg = msg.Replace(@"\", "");
                    var myDeserializedClass = JsonConvert.DeserializeObject<List<RekycDedupepan>>(Result);
                    if (myDeserializedClass != null)
                    {
                        if (myDeserializedClass.Count > 0)
                        {

                            for (int i = 0; i < myDeserializedClass.Count; i++)
                            {
                                var data = myDeserializedClass[i];
                                if (data.custNo == 0)
                                {
                                    string msg1 = msg.Split('"')[1];
                                    return Json(msg1);
                                }
                                RekycDedupepan dedupegridobjpan = new RekycDedupepan();
                                dedupegridobjpan.custNo = data.custNo;


                                RekycDedupepan.Add(dedupegridobjpan);
                            }
                            objmain1.RekycDedupepans = RekycDedupepan;
                            //return PartialView("Views/KYCQuickEnroll/_GetCBSData.cshtml", objmain);

                            return PartialView("Views/KYCQuickEnroll/_GetCBSDataPan.cshtml", objmain1);

                        }
                    }
                    else
                    {
                        return Json("Server Error Occurred");

                    }
                }
                catch (Exception ex)
                {
                    error_log.WriteErrorLog(ex.ToString());
                    return Json(ex.Message);
                }
            }

            return View();
        }
        public ActionResult GetCBSData12(string CBSValue1, ClsCustQuickEnrollment objQuickEnroll)
        {

            ErrorLog error_log = new ErrorLog();
            try
            {

                string data2 = CBSValue1;
                //string data2 = CBSValue;
                string authorsList = data2.Split(',')[1];
                string data = authorsList;
                var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSData?accountNo=" + data);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("BankCode", "RSSB");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                string Result = response.Content;
                var serializer = new JavaScriptSerializer();
                dynamic Cont = serializer.Deserialize<dynamic>(Result);

                string joint = Cont["JointHolderYN"];
                if (joint == "Y")
                {
                    return RedirectToAction("JointRekyc", "KYCQuickEnroll", new { Ac_No = data });

                }
                else
                {
                    string Ac_No = Cont["AccountNo"];
                    HttpContext.Session.SetString("Acc_No", Ac_No);
                    var test = Cont["AcctDetails"];
                    string iskycDone = test[0].Kyc;
                    long c = test[0].CustNo;
                    string s = c.ToString();
                    //var re = test.Remove('[');
                    //string iskycDone =  Cont["Kyc"];
                    if (iskycDone == "Y")
                    {

                        string Custid = c.ToString();
                        HttpContext.Session.SetString("CustIDRekyc", s);

                        if (Custid != null | Custid != "")
                        {
                            Custid = Cont["AccountNo"];
                        }
                        else
                        {
                            Custid = null;
                        }


                        string Kyc = "N";
                        string FirstName = test[0].FirstName;// Cont["FirstName"];
                        string MiddleName = test[0].MiddleName; //Cont["MiddleName"];
                        string LastName = test[0].LastName;// Cont["LastName"];
                        string MobileNo = test[0].MobileNo; //Cont["MobileNo"];
                        string EmailId = test[0].EmailId; //Cont["EmailId"];
                        string Dob = test[0].Dob; //Cont["Dob"];
                                                  //string Gendertest = test[0].Gender; //Cont["Gender"];
                        string Gender = test[0].Gender; //Gendertest.Trim();
                        string Add1 = test[0].Add1; //Cont["Add1"];
                        string Add2 = test[0].Add2; //Cont["Add2"];
                        string Add3 = test[0].Add3; //Cont["Add3"];
                        string City = test[0].City; //Cont["City"];
                        string Pincode = test[0].Pincode; //Cont["Pincode"];
                        string State = test[0].State; //Cont["State"];
                        string Countrycd = test[0].Countrycd; //Cont["Countrycd"];
                        string AnnualIncome = test[0].AnnualIncome; //Cont["AnnualIncome"];
                        string Occupation = test[0].Occupation;// Cont["Occupation"];



                        objQuickEnroll.QECustDetailsId = c;//Convert.ToInt64(c);
                        objQuickEnroll.FirstNameReKyc = FirstName;
                        objQuickEnroll.MiddleNameReKyc = MiddleName;
                        objQuickEnroll.LastNameReKyc = LastName;
                        objQuickEnroll.MobileReKyc = MobileNo;
                        objQuickEnroll.EmailIdReKyc = EmailId;
                        objQuickEnroll.DateOfBirthReKyc = Dob;
                        objQuickEnroll.GenderReKyc = Gender;
                        objQuickEnroll.ReKyc_AddressLine1 = Add1;
                        objQuickEnroll.ReKyc_AddressLine2 = Add2;
                        objQuickEnroll.ReKyc_AddressLine3 = Add3;
                        objQuickEnroll.ReKyc_City = City;
                        objQuickEnroll.ReKyc_PinCode = Pincode;
                        objQuickEnroll.State = State;
                        objQuickEnroll.Country = Countrycd;
                        objQuickEnroll.ReKyc_Occupation = Occupation;
                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("Usp_toInsertRekycCustomerDetails12", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.AddWithValue("@CustomerNo", c);
                            cmd2.Parameters.AddWithValue("@CustomerFirstname", FirstName);
                            cmd2.Parameters.AddWithValue("@CustomerMiddlename", MiddleName);
                            cmd2.Parameters.AddWithValue("@CustomerLastname", LastName);
                            cmd2.Parameters.AddWithValue("@Customer_Mobno", MobileNo);
                            cmd2.Parameters.AddWithValue("@CustomerEmailID", EmailId);
                            cmd2.Parameters.AddWithValue("@customerDOB", Dob);
                            cmd2.Parameters.AddWithValue("@CustomerGender", Gender);
                            cmd2.Parameters.AddWithValue("@CustomerAdd1", Add1);
                            cmd2.Parameters.AddWithValue("@CustomerAdd2", Add2);
                            cmd2.Parameters.AddWithValue("@CustomerAdd3", Add3);
                            cmd2.Parameters.AddWithValue("@CustomerCity", City);
                            cmd2.Parameters.AddWithValue("@CustomerPincode", Pincode);
                            cmd2.Parameters.AddWithValue("@CustomerState", State);
                            cmd2.Parameters.AddWithValue("@CustomerCountryID", Countrycd);
                            cmd2.Parameters.AddWithValue("@CustomerAnualincome", "");
                            cmd2.Parameters.AddWithValue("@CustomerOccupation", "");
                            cmd2.Parameters.AddWithValue("@Acc_No", Ac_No);
                            connection2.Open();
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            if (reader2.Read())
                            {

                                var Result1 = reader2["RESULT"].ToString();
                            }
                        }

                        return Json(objQuickEnroll);
                    }
                    else
                    {
                        return Json("KYC has been done already for the given accNo/CustomerNo");
                    }
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                // System.IO.File.AppendAllText(file, DateTime.Now + "---" + ex.Message+"--"+ex.Source+"--"+ex.StackTrace);
                return Json(ex.Message);
            }
        }

        public ActionResult JointRekycCustNo(string CBSValue, ClsCustQuickEnrollment objQuickEnroll)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string data2 = CBSValue;
                //string data2 = CBSValue;
                string CustNo1 = data2.Split(',')[1];
                string CustNo = CustNo1;

                string data = HttpContext.Session.GetString("Ac_No");
                var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSData?accountNo=" + data);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("BankCode", "RSSB");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                string Result = response.Content;
                var serializer = new JavaScriptSerializer();
                dynamic Cont = serializer.Deserialize<dynamic>(Result);

                string joint = Cont["JointHolderYN"];
                if (joint == "Y")
                {

                    var test1 = Cont["AcctDetails"];
                    if (test1.Count > 0)
                    {

                        for (int i = 0; i < test1.Count; i++)
                        {
                            var data1 = test1[i];
                            if (data1.CustNo == Convert.ToInt64(CustNo))
                            {
                                HttpContext.Session.SetString("CustIDRekyc", CustNo);
                                HttpContext.Session.SetString("Acc_No", data);

                                string Kyc = "N";
                                string FirstName = data1.FirstName;
                                string MiddleName = data1.MiddleName;
                                string LastName = data1.LastName;
                                string MobileNo = data1.MobileNo;
                                string EmailId = data1.EmailId;
                                string Dob = data1.Dob;
                                string Gender = data1.Gender;
                                string Add1 = data1.Add1;
                                string Add2 = data1.Add2;
                                string Add3 = data1.Add3;
                                string City = data1.City;
                                string Pincode = data1.Pincode;
                                string State = data1.State;
                                string Countrycd = data1.Countrycd;
                                string AnnualIncome = data1.AnnualIncome;
                                string Occupation = data1.Occupation;
                                //----------------//
                                objQuickEnroll.QECustDetailsId = Convert.ToInt64(CustNo);
                                objQuickEnroll.FirstNameReKyc = FirstName;
                                objQuickEnroll.MiddleNameReKyc = MiddleName;
                                objQuickEnroll.LastNameReKyc = LastName;
                                objQuickEnroll.MobileReKyc = MobileNo;
                                objQuickEnroll.EmailIdReKyc = EmailId;
                                objQuickEnroll.DateOfBirthReKyc = Dob;
                                objQuickEnroll.GenderReKyc = Gender;
                                objQuickEnroll.ReKyc_AddressLine1 = Add1;
                                objQuickEnroll.ReKyc_AddressLine2 = Add2;
                                objQuickEnroll.ReKyc_AddressLine3 = Add3;
                                objQuickEnroll.ReKyc_City = City;
                                objQuickEnroll.ReKyc_PinCode = Pincode;
                                objQuickEnroll.State = State;
                                objQuickEnroll.Country = Countrycd;
                                objQuickEnroll.ReKyc_Occupation = Occupation;
                                string conn = _connectionString;
                                using (SqlConnection connection2 = new SqlConnection(conn))
                                {
                                    SqlCommand cmd2 = new SqlCommand("Usp_toInsertRekycCustomerDetails12", connection2);
                                    cmd2.CommandType = CommandType.StoredProcedure;

                                    cmd2.Parameters.AddWithValue("@CustomerNo", CustNo);
                                    cmd2.Parameters.AddWithValue("@CustomerFirstname", FirstName);
                                    cmd2.Parameters.AddWithValue("@CustomerMiddlename", MiddleName);
                                    cmd2.Parameters.AddWithValue("@CustomerLastname", LastName);
                                    cmd2.Parameters.AddWithValue("@Customer_Mobno", MobileNo);
                                    cmd2.Parameters.AddWithValue("@CustomerEmailID", EmailId);
                                    cmd2.Parameters.AddWithValue("@customerDOB", Dob);
                                    cmd2.Parameters.AddWithValue("@CustomerGender", Gender);
                                    cmd2.Parameters.AddWithValue("@CustomerAdd1", Add1);
                                    cmd2.Parameters.AddWithValue("@CustomerAdd2", Add2);
                                    cmd2.Parameters.AddWithValue("@CustomerAdd3", Add3);
                                    cmd2.Parameters.AddWithValue("@CustomerCity", City);
                                    cmd2.Parameters.AddWithValue("@CustomerPincode", Pincode);
                                    cmd2.Parameters.AddWithValue("@CustomerState", State);
                                    cmd2.Parameters.AddWithValue("@CustomerCountryID", Countrycd);
                                    cmd2.Parameters.AddWithValue("@CustomerAnualincome", "");
                                    cmd2.Parameters.AddWithValue("@CustomerOccupation", "");
                                    cmd2.Parameters.AddWithValue("@Acc_No", data);
                                    connection2.Open();
                                    SqlDataReader reader2 = cmd2.ExecuteReader();
                                    if (reader2.Read())
                                    {

                                        var Result1 = reader2["RESULT"].ToString();
                                    }
                                }

                                return Json(objQuickEnroll);

                            }
                            
                        }

                    }

                }
                return Json(objQuickEnroll);

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                // System.IO.File.AppendAllText(file, DateTime.Now + "---" + ex.Message+"--"+ex.Source+"--"+ex.StackTrace);
                return Json(ex.Message);
            }
        }

        public ActionResult JointRekyc(string Ac_No)
        {
            HttpContext.Session.SetString("Ac_No", Ac_No);
            var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSData?accountNo=" + Ac_No);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("BankCode", "RSSB");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            string Result = response.Content;
            var serializer = new JavaScriptSerializer();
            dynamic Cont = serializer.Deserialize<dynamic>(Result);

            string joint = Cont["JointHolderYN"];

            RekycJoint_GRID_MAIN objmain12 = new RekycJoint_GRID_MAIN();
            List<RekycJointgridlist> RekycJointgridlist = new List<RekycJointgridlist>();
            var test = Cont["AcctDetails"];
            if (test.Count > 0)
            {

                for (int i = 0; i < test.Count; i++)
                {
                    var data1 = test[i];
                    if (data1.CustNo == 0)
                    {
                        return View();
                    }
                    RekycJointgridlist obj = new RekycJointgridlist();
                    obj.custNo = data1.CustNo;
                    var Firstnm = data1.FirstName;
                    var Laststnm = data1.LastName;
                    obj.name = string.Concat(Firstnm, " ", Laststnm);

                    obj.RekycJointRadioFlag = Convert.ToString(i);
                    obj.RekycJointRadioFlag = "RekycJointRadioFlag" + i;

                    RekycJointgridlist.Add(obj);
                }
                objmain12.RekycJointgridlists = RekycJointgridlist;
            }
            return PartialView("Views/KYCQuickEnroll/_RekycJoint.cshtml", objmain12);

        }
        public ActionResult GetCBSData1(string CBSValue1, ClsCustQuickEnrollment objExist)

        {

            ErrorLog error_log = new ErrorLog();
            try
            {

                string data2 = CBSValue1;
                //string data2 = CBSValue;
                string authorsList = data2.Split(',')[1];
                string data = authorsList;

                var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSData?accountNo=" + data);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Content-Type", "application/json");
                request.AddHeader("BankCode", "RSSB");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                string Result = response.Content;
                var serializer = new JavaScriptSerializer();
                dynamic Cont = serializer.Deserialize<dynamic>(Result);

                string joint = Cont["JointHolderYN"];
                if (joint == "Y")
                {
                    return Json("error");
                }
                else
                {


                    var test = Cont["AcctDetails"];
                    string iskycDone = test[0].Kyc;
                    long c = test[0].CustNo;
                    string s = c.ToString();
                    //var re = test.Remove('[');
                    //string iskycDone =  Cont["Kyc"];
                    if (iskycDone == "Y")
                    {

                        string Custid = c.ToString();
                        HttpContext.Session.SetString("ExistCustID", s);

                        if (Custid != null | Custid != "")
                        {
                            Custid = Cont["AccountNo"];
                        }
                        else
                        {
                            Custid = null;
                        }


                        string Kyc = "N";
                        string FirstName = test[0].FirstName;
                        // Cont["FirstName"];

                        string MiddleName = test[0].MiddleName; //Cont["MiddleName"];
                        string LastName = test[0].LastName;// Cont["LastName"];
                        string MobileNo = test[0].MobileNo; //Cont["MobileNo"];
                        string EmailId = test[0].EmailId; //Cont["EmailId"];
                        string Dob = test[0].Dob; //Cont["Dob"];
                        //string Gendertest = test[0].Gender; //Cont["Gender"];
                        string Gender = test[0].Gender; //Gendertest.Trim();
                        string Add1 = test[0].Add1; //Cont["Add1"];
                        string Add2 = test[0].Add2; //Cont["Add2"];
                        string Add3 = test[0].Add3; //Cont["Add3"];
                        string City = test[0].City; //Cont["City"];
                        string Pincode = test[0].Pincode; //Cont["Pincode"];
                        string State = test[0].State; //Cont["State"];
                        string Countrycd = test[0].Countrycd; //Cont["Countrycd"];
                        string AnnualIncome = test[0].AnnualIncome; //Cont["AnnualIncome"];
                        string Occupation = test[0].Occupation;// Cont["Occupation"];



                        objExist.QECustDetailsId = c;//Convert.ToInt64(c);
                        ViewBag.Cid = c;
                        objExist.existFirstName = FirstName;
                        ViewBag.EXFirst = FirstName;
                        objExist.ExistMiddleName = MiddleName;
                        ViewBag.ExmName = MiddleName;
                        objExist.ExistLastName = LastName;
                        objExist.ExistMobileNo = MobileNo;
                        objExist.ExistEmail = EmailId;
                        objExist.ExistDOB = Dob;
                        objExist.ExistGender = Gender;
                        objExist.ExistAddress1 = Add1;
                        objExist.ExistAddress2 = Add2;
                        objExist.ExistAddress3 = Add3;
                        objExist.ExistCity = City;
                        objExist.ExistPincode = Pincode;
                        objExist.State = State;
                        objExist.Country = Countrycd;
                        objExist.ExistAnnalIncome = AnnualIncome;
                        objExist.ExistOccupation = Occupation;

                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("Usp_toInsertExistingCustomerDetails", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;

                            cmd2.Parameters.AddWithValue("@CustomerNo", c);
                            cmd2.Parameters.AddWithValue("@CustomerFirstname", FirstName);
                            cmd2.Parameters.AddWithValue("@CustomerMiddlename", MiddleName);
                            cmd2.Parameters.AddWithValue("@CustomerLastname", LastName);
                            cmd2.Parameters.AddWithValue("@Customer_Mobno", MobileNo);
                            cmd2.Parameters.AddWithValue("@CustomerEmailID", EmailId);
                            cmd2.Parameters.AddWithValue("@customerDOB", Dob);
                            cmd2.Parameters.AddWithValue("@CustomerGender", Gender);
                            cmd2.Parameters.AddWithValue("@CustomerAdd1", Add1);
                            cmd2.Parameters.AddWithValue("@CustomerAdd2", Add2);
                            cmd2.Parameters.AddWithValue("@CustomerAdd3", Add3);
                            cmd2.Parameters.AddWithValue("@CustomerCity", City);
                            cmd2.Parameters.AddWithValue("@CustomerPincode", Pincode);
                            cmd2.Parameters.AddWithValue("@CustomerState", State);
                            cmd2.Parameters.AddWithValue("@CustomerCountryID", Countrycd);
                            cmd2.Parameters.AddWithValue("@CustomerAnualincome", "");
                            cmd2.Parameters.AddWithValue("@CustomerOccupation", "");


                            connection2.Open();
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            if (reader2.Read())
                            {

                                var Result1 = reader2["RESULT"].ToString();
                            }
                        }
                        return Json(objExist);
                    }
                    else
                    {
                        return Json("KYC has been done already for the given accNo/CustomerNo");
                    }



                }

            }


            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.Message);
            }

        }



        [HttpGet]
        public ActionResult jointAccountCustomer(string AccountNO, string CustomerId)

        {

            ErrorLog error_log = new ErrorLog();
            try
            {
                if (AccountNO != null)
                {

                    var client = new RestClient("https://cbs.indofinnet.com/api/GetCBSData?accountNo=" + AccountNO);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Content-Type", "application/json");
                    request.AddHeader("BankCode", "RSSB");
                    IRestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);
                    string Result = response.Content;
                    if (Result == "")
                    {
                        return Json("API is not Working");
                    }
                    var serializer = new JavaScriptSerializer();
                    dynamic Cont = serializer.Deserialize<dynamic>(Result);
                    if (Result.Contains("status"))
                    {
                        long status = Cont["status"];
                        if (status == 500)
                        {
                            return Json("API is not Working");
                        }
                    }
                    string joint = Cont["JointHolderYN"];
                    if (joint == "Y")
                    {
                        return Json("error");
                    }
                    var test = Cont["AcctDetails"];

                    string iskycDone = test[0].Kyc;
                    long c = test[0].CustNo;
                    var CustID = c.ToString();
                    if (CustID != null)
                    {
                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_GetsilCustomer", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerNO", CustID);
                            connection2.Open();
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            if (reader2.Read())
                            {

                                CustomerId = reader2[0].ToString();
                                HttpContext.Session.SetString("PersonalId", CustomerId);
                                //HttpContext.Session.SetString("DAEditCustomerdetailId", CustomerId);
                                //  HttpContext.Session.GetString("DAEditCustomerdetailId")
                                HttpContext.Session.SetString("JointACCdata", "true");

                            }
                        }
                    }



                }
                else
                {
                    return Json("Please Enter Account No");
                }
                return Json(CustomerId);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                // System.IO.File.AppendAllText(file, DateTime.Now + "---" + ex.Message+"--"+ex.Source+"--"+ex.StackTrace);
                return Json(ex.Message);
            }
         }

        [HttpPost]
        public ActionResult Live(string image)
        {
            byte[] images = Convert.FromBase64String(image.Split(',')[1]);
            var client = new RestClient("https://apigateway.indofinnet.com/api/Headpose?OrgID=" + "IndoFin007");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
            request.AddHeader("Content-Type", "image/jpg");
            request.AddParameter("image/jpg", images, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var abc = response.Content;
            dynamic output = JsonConvert.DeserializeObject(abc);
            dynamic output2 = JsonConvert.DeserializeObject(output);
            JArray jsonArrayToken = JArray.Parse(output);
            if (jsonArrayToken.Count > 1)
            {
                return Json("Multiple faces are detected");
            }
            return Json("Success");
        }
    }
}

