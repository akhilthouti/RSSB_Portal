using INDO_FIN_NET.Models;
using INDO_FIN_NET.Models.QuickEnroll;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceProvider1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Configuration;
using System.Text;

using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;

using System.ComponentModel;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs.Specialized;

namespace INDO_FIN_NET.Controllers
{
    public class ServiceProviderMainHomeController : Controller
    {
        ClsFormFlag objForm = new ClsFormFlag();
        TripleDESImplementation objtriple = new TripleDESImplementation();
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        private readonly IConfiguration configuration;
        private readonly string _connectionString;

        public ServiceProviderMainHomeController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_,  IConfiguration configuration_)
        {
            objDetails = Context;
            objData1 = iNDO_;
            configuration = configuration_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }

        public ActionResult QuickEnrollDashboard(string DigiLocker, string ShareAadharOrNot, string AdminCustId, string RejectCustId, string NewCJourney, string XMLReferenceId, long? CustomerId)
        {
            ErrorLog error_log = new ErrorLog();
            ViewBag.jointTab = HttpContext.Session.GetString("jointGrid");
            if (HttpContext.Session.GetString("JointACCdata") == "true")
            {
                HttpContext.Session.SetString("JointAdmin", "true");
                HttpContext.Session.SetString("JointACCdata", "true");
            }
            //HttpContext.Session.SetString("JointACCdata", "true");
            if (HttpContext.Session.GetString("DACustMOBNo") != null)
            {
                if (!clsAdminAuthorize.IsAuthorizeCustDetail((objtriple.Decrypt(HttpContext.Session.GetString("EncryPersonalId").ToString())), HttpContext.Session.GetString("SessionKey").ToString()))                
                {
                    return RedirectToAction("CustomerRegistration", "SignIn");
                }
            }
            else if (HttpContext.Session.GetString("UserId") != null)
            {
                if (!clsAdminAuthorize.IsAuthorizeCustDetail((objtriple.Decrypt(HttpContext.Session.GetString("UserId").ToString())), HttpContext.Session.GetString("SessionKey").ToString()))
                {
                    return RedirectToAction("UserDetails", "AdminLogin");
                }
            }
            else if (HttpContext.Session.GetString("OrgUserId") != null && (HttpContext.Session.GetString("SessionOrgKey") != null))
            {
                if (!clsCustAuthorize.IsAuthCustomerDetails(Convert.ToInt64(objtriple.Decrypt(HttpContext.Session.GetString("OrgUserId").ToString())), HttpContext.Session.GetString("SessionOrgKey").ToString()))
                {
                    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
                }
            }
                try
                {

                if (AdminCustId != null)
                {
                    ViewBag.AdminFlag = "AdminFlag";
                    HttpContext.Session.SetString("DAEditCustomerdetailId",AdminCustId) ;
                    var ViewResult = objDetails.AdmFlagMainTains1.FromSqlRaw($"USP_ViewForApproveRejectFlag {Convert.ToInt64(AdminCustId)}, {"Verify"},{Convert.ToInt64(AdminCustId)}").AsEnumerable().FirstOrDefault();
                    if (ViewResult != null)
                    {
                        {
                            return Json("This Customer is used for verification by another user ");
                        }
                    }
                    var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId")))}").AsEnumerable().FirstOrDefault();
                    if (result != null)
                    {
                        objForm.IsQuickEnrollSubmit = result.IsQuickEnrollSubmit;
                        objForm.IsDocumentSubmit = result.IsDocumentSubmit;
                        objForm.IsIPVSubmit = result.IsIpvsubmit;
                        objForm.IssummarysheetSubmit = result.IssummarysheetSubmit;
                        objForm.proceedwithOCR = result.ProceedwithOcr;
                        objForm.isIPVSkip = result.IsIpvSkip;
                        objForm.isCAFPDF = result.IsCafpdf;
                        objForm.isSavingAcc = result.IsSavingAcc;
                        objForm.IsSignUpDone = result.IsSignUpDone;
                        objForm.shareAadharNumber = result.ProceedwithOcr;
                        objForm.PersonalId = Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId"));
                        if (AdminCustId != null)
                        {
                            objForm.AccountType = "Account";
                        }
                        else
                        {
                            objForm.AccountType = HttpContext.Session.GetString("AccountForm").ToString();
                        }
                    }
                }
                else if (RejectCustId != null)
                {
                    var PersonalId = RejectCustId;
                    String DAEditCustomerdetailId = null;
                    var RejectId = RejectCustId;
                    var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();                    
                    if (result != null)
                    {
                        objForm.IsQuickEnrollSubmit = result.IsQuickEnrollSubmit;
                        objForm.IsDocumentSubmit = result.IsDocumentSubmit;
                        objForm.IsIPVSubmit = result.IsIpvsubmit;
                        objForm.IssummarysheetSubmit = result.IssummarysheetSubmit;
                        objForm.proceedwithOCR = result.ProceedwithOcr;
                        objForm.isIPVSkip = result.IsIpvSkip;
                        objForm.isCAFPDF = result.IsCafpdf;
                        objForm.isSavingAcc = result.IsSavingAcc;
                        objForm.IsSignUpDone = result.IsSignUpDone;
                        objForm.shareAadharNumber = result.ShareAadharNumber;
                        objForm.PersonalId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                        objForm.KYCverificationType = "";
                        if (RejectCustId != null)
                        {
                            objForm.AccountType = null;
                        }
                        else
                        {
                            objForm.AccountType = HttpContext.Session.GetString("AccountForm").ToString();
                        }
                    }

                }
                else if (NewCJourney == "NewCJ")
                {
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("KYCverificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("PanVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("CKYCVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("AadharVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("Otherverification"));

                    if (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")) > 0)
                    {
                        var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                        if (result != null)
                        {
                            objForm.IsQuickEnrollSubmit = result.IsQuickEnrollSubmit;
                            objForm.IsDocumentSubmit = result.IsDocumentSubmit;
                            objForm.IsIPVSubmit = result.IsIpvsubmit;
                            objForm.IssummarysheetSubmit = result.IssummarysheetSubmit;
                            objForm.proceedwithOCR = result.ProceedwithOcr;
                            objForm.shareAadharNumber = result.ShareAadharNumber;
                            objForm.PersonalId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                            if (result.IsDocumentSubmit == true)
                            {
                                objForm.AccountType = "Account";
                            }
                            else
                            {
                                objForm.AccountType = null;
                            }
                            if ((result.IsPanVerify) == true)
                            {
                                var CustQE = objDetails.AdmPanVerificationDetails.FromSqlRaw($"USP_GetPanDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                                string PanNo = CustQE.PanNo;
                                ViewBag.NSDL_PanNumber = PanNo;
                                var NSDL_PANNumber = PanNo;
                                ViewBag.IsPanVerify = true;
                            }
                            if (result.IsAadharVerify == true)
                            {
                                var resp = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                                var XMLReferenceID = resp.XmlreferenceId;
                                var KYCverificationType = resp.VerificationType;
                                objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("KYCverificationType"));
                            }
                            if (result.IsCkycVerify == true)
                            {
                                objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("CKYCVerificationType"));
                            }
                            if (result.IsSignUpDone == true)    
                            {
                                ClsCustQuickEnrollment obj = new ClsCustQuickEnrollment();
                                string MoNo = Convert.ToString(TempData["CustMob"]);
                                var qdata = objDetails.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsByMobNo {(Convert.ToInt64(HttpContext.Session.GetString("MoNo")))}").AsEnumerable().FirstOrDefault();
                                obj.QEFirstName = qdata.CustFirstName;
                                obj.QELastName = qdata.CustLastName;
                                obj.QEEmailId = qdata.CustEmailId;
                                obj.QEMobileNo = qdata.CustMobileNo;



                                if (qdata.CustVerificationType == "Aadhaar")
                                {
                                    obj.QEVTypeTextbox = qdata.AadhaarNo;
                                    obj.QEVType = "A";
                                }
                                if (qdata.CustVerificationType == "Pan")
                                {
                                    obj.QEVTypeTextbox = qdata.PanNo;
                                    obj.QEVType = "P1";
                                }
                                if (qdata.CustVerificationType == "Driving Licence")
                                {
                                    obj.QEVTypeTextbox = qdata.DrivingLicenceNo;
                                    obj.QEVType = "DL";
                                }
                                if (qdata.CustVerificationType == "Passport")
                                {
                                    obj.QEVTypeTextbox = qdata.PassportNo;
                                    obj.QEVType = "P";
                                }
                                if (qdata.CustVerificationType == "Voter ID")
                                {
                                    obj.QEVTypeTextbox = qdata.VoterId;
                                    obj.QEVType = "V";
                                }
                                var PersonalId = qdata.CustDetailsId;
                                var verificationtype1 = (from details in objDetails.TblVerificationTypes.FromSqlRaw($"USP_Get_VerificationDetails {qdata.CustVerificationType}").ToList()
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
                                TempData["UpdateQE"] = true;
                                ViewBag.UpdateQE = TempData["UpdateQE"];
                            }
                        }
                    }
                    ViewBag.Cust = "NewCustomer";
                }
                else if (NewCJourney == "" || NewCJourney == null)
                {
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("KYCverificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("PanVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("CKYCVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("AadharVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("Otherverification"));
                    if (CustomerId != null)
                    {
                        var PersonalId = CustomerId;
                    }
                    if (XMLReferenceId != null)
                    {
                        var Aadhardate = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("XMLReferenceId")))}").AsEnumerable().FirstOrDefault();
                        if (Aadhardate != null)
                        {

                            var AadharVerificationType = true;
                            objForm.KYCverificationType = Aadhardate.VerificationType;
                        }
                        var AadhardateDigi = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_GetDigi_Locker_AadharDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("XMLReferenceId")))}").AsEnumerable().FirstOrDefault();
                        if (AadhardateDigi != null)
                        {
                            objForm.KYCverificationType = "AadharVerificationType";
                        }
                        var PanData = objDetails.AdmPanVerificationDetails.FromSqlRaw($"USP_GetPanDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("XMLReferenceId")))}").AsEnumerable().FirstOrDefault();
                        if (PanData != null)
                        {
                            objForm.KYCverificationType = "PAN";
                        }
                        var PersonalId = XMLReferenceId;
                    }
                    if (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")) > 0)
                    {
                        var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                        if (result != null)
                        {
                            objForm.IsQuickEnrollSubmit = result.IsQuickEnrollSubmit;
                            objForm.IsDocumentSubmit = result.IsDocumentSubmit;
                            objForm.IsIPVSubmit = result.IsIpvsubmit;
                            objForm.IssummarysheetSubmit = result.IssummarysheetSubmit;
                            objForm.proceedwithOCR = result.ProceedwithOcr;
                            objForm.shareAadharNumber = result.ShareAadharNumber;
                            objForm.isIPVSkip = result.IsIpvSkip;
                            objForm.isCAFPDF = result.IsCafpdf;
                            objForm.isSavingAcc = result.IsSavingAcc;
                            objForm.IsSignUpDone = result.IsSignUpDone;
                            objForm.PersonalId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                            if (result.IsDocumentSubmit == true)
                            {
                                objForm.AccountType = "Account";
                            }
                            else
                            {
                                objForm.AccountType = null;
                            }
                        }
                        if (result.IsAadharVerify == true & CustomerId != null)
                        {
                            var resp = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                            var XMLReferenceID = resp.XmlreferenceId;
                            var KYCverificationType = resp.VerificationType;
                            objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("KYCverificationType"));
                        }
                    }
                    if (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")) == 0 & XMLReferenceId != null)
                    {
                        string XMLReferenceID = XMLReferenceId;
                        ViewBag.XMLRefID = XMLReferenceID;
                        var xmlData = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetCustomerDetailsByReference {(Convert.ToInt64(HttpContext.Session.GetString("XMLReferenceID")))}").AsEnumerable().FirstOrDefault();
                        objForm.KYCverificationType = "XML";
                        var AadharVerificationType = true;
                    }
                    ViewBag.Cust = "";
                    ViewBag.AdminFlag = "";
                }
                else if (DigiLocker == "DigiLocker" || NewCJourney == "yes")
                {

                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("KYCverificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("PanVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("CKYCVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("AadharVerificationType"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("Otherverification"));
                    objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("DigiLocker"));

                    if (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")) > 0)
                    {
                        var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                       
                        if (result != null)
                        {
                            objForm.IsQuickEnrollSubmit = result.IsQuickEnrollSubmit;
                            objForm.IsDocumentSubmit = result.IsDocumentSubmit;
                            objForm.IsIPVSubmit = result.IsIpvsubmit;
                            objForm.IssummarysheetSubmit = result.IssummarysheetSubmit;
                            objForm.proceedwithOCR = result.ProceedwithOcr;
                            objForm.shareAadharNumber = result.ShareAadharNumber;
                            objForm.PersonalId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                            if (result.IsDocumentSubmit == true)
                            {
                                objForm.AccountType = "Account";
                            }
                            else
                            {
                                objForm.AccountType = null;
                            }
                            if ((result.IsPanVerify) == true)
                            {
                                var CustQE = objDetails.AdmPanVerificationDetails.FromSqlRaw($"USP_GetPanDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();                                
                                string PanNo = CustQE.PanNo;
                                ViewBag.NSDL_PanNumber = PanNo;
                                var NSDL_PANNumber = PanNo;
                                ViewBag.IsPanVerify = true;
                            }
                            if (result.IsAadharVerify == true)
                            {
                                var resp = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_GetAadharDetailsByCustId {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                                var XMLReferenceID = resp.XmlreferenceId;
                                var KYCverificationType = resp.VerificationType;
                                objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("KYCverificationType"));
                            }
                            if (result.IsCkycVerify == true)
                            {                                
                                objForm.KYCverificationType = Convert.ToString(HttpContext.Session.GetString("CKYCVerificationType"));
                            }
                            if (result.IsSignUpDone == true)
                            {
                                ClsCustQuickEnrollment obj = new ClsCustQuickEnrollment();
                                string MoNo2 = Convert.ToString(TempData["CustMob"]);
                                string MoNo = Convert.ToString(HttpContext.Session.GetString("CustMob1 "));
                                var qdata = objDetails.TblCustomerDetails.FromSqlRaw($"USP_GetQEDetailsByMobNo {(Convert.ToInt64(HttpContext.Session.GetString("MoNo")))}").AsEnumerable().FirstOrDefault();

                                if (qdata != null)
                                {
                                    obj.QEFirstName = qdata.CustFirstName;
                                    obj.QELastName = qdata.CustLastName;
                                    obj.QEEmailId = qdata.CustEmailId;
                                    obj.QEMobileNo = qdata.CustMobileNo;
                                    ViewBag.QEFirstName = qdata.CustFirstName;
                                    ViewBag.QEEmailId = qdata.CustEmailId;
                                    ViewBag.QEMobileNo = qdata.CustMobileNo;
                                    ViewBag.QELastName = qdata.CustLastName;

                                    if (qdata.CustVerificationType == "Aadhaar")
                                    {
                                        obj.QEVTypeTextbox = qdata.AadhaarNo;
                                        ViewBag.QEVTypeTextbox = qdata.AadhaarNo;
                                        ViewBag.VTYPEData1 = qdata.CustVerificationType;

                                        obj.QEVType = "A";
                                    }
                                    if (qdata.CustVerificationType == "Pan")
                                    {
                                        obj.QEVTypeTextbox = qdata.PanNo;
                                        ViewBag.QEVTypeTextbox = qdata.PanNo;
                                        ViewBag.VTYPEData1 = qdata.CustVerificationType;

                                        obj.QEVType = "P1";
                                    }
                                    if (qdata.CustVerificationType == "Driving Licence")
                                    {
                                        obj.QEVTypeTextbox = qdata.DrivingLicenceNo;
                                        ViewBag.QEVTypeTextbox = qdata.DrivingLicenceNo;
                                        ViewBag.VTYPEData1 = qdata.CustVerificationType;

                                        obj.QEVType = "DL";
                                    }
                                    if (qdata.CustVerificationType == "Passport")
                                    {
                                        obj.QEVTypeTextbox = qdata.PassportNo;
                                        ViewBag.QEVTypeTextbox = qdata.PassportNo;
                                        ViewBag.VTYPEData1 = qdata.CustVerificationType;

                                        obj.QEVType = "P";
                                    }
                                    if (qdata.CustVerificationType == "Voter ID")
                                    {
                                        obj.QEVTypeTextbox = qdata.VoterId;
                                        ViewBag.QEVTypeTextbox = qdata.VoterId;
                                        ViewBag.VTYPEData1 = qdata.CustVerificationType;

                                        obj.QEVType = "V";
                                    }
                                }
                                else if (qdata == null)
                                        {
                                    return RedirectToAction("CustomerQuickEnrollment", "KYCQuickEnroll");
                                }
                                else 
                                {
                                }

                                var PersonalId = qdata.CustDetailsId;
                                var verificationtype = (from details in objDetails.TblVerificationTypes.FromSqlRaw($"USP_Get_VerificationDetails {qdata.CustVerificationType}").ToList()
                                                         select new SelectListItem()
                                                         {
                                                             Text = details.CustVerificationType,
                                                             Value = details.VtypeId.ToString(),
                                                         }).ToList();
                                verificationtype.Insert(0, new SelectListItem()
                                {
                                    Text = "----Select----",
                                    Value = string.Empty
                                });
                                ViewBag.VTypeData = verificationtype;
                
                                TempData["UpdateQE"] = true;
                                ViewBag.UpdateQE = TempData["UpdateQE"];
                            }
                        }
                    }

                    ViewBag.Cust = "NewCustomer";
                    
                    ViewBag.Digilocker = true;
                }

                objForm.IsLogin = true;
                if (TempData["msg"] != null)
                {
                    ViewBag.msg = TempData["msg"];
                    TempData.Keep();
                }
                return View(objForm);
                }
                catch (Exception ex)
                {
                error_log.WriteErrorLog(ex.ToString());
                TempData["msg"] = ex.Message;
                string errormsg = Environment.NewLine + DateTime.Now + "--" + ex.Message + "-" + ex.InnerException + "-" + ex.StackTrace + "-" + ex.Source + "-" + ex.Data;

                return View(objForm);
                }
        }        
    }
}
