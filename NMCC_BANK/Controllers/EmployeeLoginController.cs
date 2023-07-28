using Microsoft.AspNetCore.Mvc;
using Amazon.SimpleSystemsManagement.Model;
using INDO_FIN_NET.Controllers.Organisation;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
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
using Microsoft.AspNetCore.Identity;
using RestSharp;
using Microsoft.Extensions.Configuration;

namespace INDO_FIN_NET.Controllers
    {
        public class EmployeeLoginController : Controller
        {
            private readonly RSSBPRODDbCotext objDetails;
            private readonly INDO_FinNetDbCotext objData;
            TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
           private readonly string _connectionString;

        public EmployeeLoginController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
            {
                objDetails = Context;
                objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        [HttpGet]
            public async Task<ActionResult> EmpOrganisationDetails(string userid, string BankName, string alphaservicename, string ServiceProvider, string CategoryName, string ProductName)

            {
            ErrorLog error_log = new ErrorLog();
            var errorline = "1";
                try
                {
                    ClsOrgLogin objclsorg = new ClsOrgLogin();
                    var UserId = ObjTripleDes.Decrypt(userid);
                    HttpContext.Session.SetInt32("OrgUserId", Convert.ToInt32(UserId));
                    var Bankname = ObjTripleDes.Decrypt(BankName);
                    var AlphaService = ObjTripleDes.Decrypt(alphaservicename);
                    var ServiceProvide = ObjTripleDes.Decrypt(ServiceProvider);
                    var categoryName = ObjTripleDes.Decrypt(CategoryName);
                    var product = ObjTripleDes.Decrypt(ProductName);
                    if (UserId != null && Bankname != null && AlphaService != null && ServiceProvide != null && categoryName != null && product != null)
                    {
                        ErrorLog error_log1 = new ErrorLog(); 
                        try
                        {
                            errorline = "2";
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                            string myURLFund = "https://indodbservice.azurewebsites.net/api/OrganisationLogin/USP_GETIndoBankDetailsById?=" + UserId + "";
                            var client = new HttpClient();
                            var res = client.GetAsync(myURLFund);
                            string response = await res.Result.Content.ReadAsStringAsync();
                            var result = JsonConvert.SerializeObject(response);
                            errorline = "3";
                            if (result != null)
                            {
                                errorline = "4";
                                if (result == Bankname)
                                {
                                    errorline = "5";
                                    System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                                    string myURLFund1 = "https://indodbservice.azurewebsites.net/api/OrganisationLogin/USP_InsertLogDetails" + UserId + "" + "BNAK" + "" + Bankname + "" + AlphaService + "" + ServiceProvide + "" + categoryName + "" + product + "";
                                    var client1 = new HttpClient();
                                    var res1 = client.GetAsync(myURLFund1);
                                    string response1 = await res1.Result.Content.ReadAsStringAsync();
                                    var Insert = JsonConvert.SerializeObject(response);
                                    return Json(Insert);
                                    errorline = "6";
                                    return RedirectToAction("QuickEnrollDashboard", "ServiceProviderMainHome");
                                }
                                else
                                {
                                    errorline = "7";
                                    string error = "User Not Authenticate";
                                    errorline = "8";
                                    return new RedirectResult("https://organization.indofinnet.in/SelectedServicesByBank/SelectedServicesByBank/" + "/?" + "Error=" + error);
                                }
                            }
                            else
                            {
                                errorline = "9";
                                string error = "User Not available";
                                return new RedirectResult("https://organization.indofinnet.in/SelectedServicesByBank/SelectedServicesByBank/" + "/?" + "Error=" + error);
                            }
                        }
                        catch (Exception e)
                        {
                            error_log1.WriteErrorLog(e.ToString());
                            string error = e.Message + "/" + errorline;
                            var result = objDetails.Database.ExecuteSqlRaw($"USP_IndoErrorLogs {e.Message},{"EmployeeLoginController"},{"OrganisationDetails"}");
                      
                            return new RedirectResult("https://organization.indofinnet.in/SelectedServicesByBank/SelectedServicesByBank/" + "/?" + "Error=" + error);
                        }
                    }
                    else
                    {
                        ViewBag.msg = TempData["errormsg"];
                        ViewBag.ip = TempData["Ip"];
                        ViewBag.loginDateTime = TempData["LoginDateTime"];
                        if (ViewBag.loginDateTime != null)
                        {
                            objclsorg.error = "Your previous session is already active, Please Logout.";
                            return View(objclsorg);
                        }
                        return View(objclsorg);
                    }
                }
                catch (Exception e)
                {
                    error_log.WriteErrorLog(e.ToString());
                    TempData["errormsg"] = e.Message + "/" + e.StackTrace;
                    return RedirectToAction("EmpOrganisationDetails", "EmployeeLogin");
                }



            }
            [HttpPost]
        public async Task<ActionResult> EmpOrganisationDetails(ClsOrgLogin objOrg)
        {

            TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
            string encryptedUsername = ObjTripleDes.Encrypt(objOrg.UserId);
            string encryptedPassword = ObjTripleDes.Encrypt(objOrg.Password);
            //strEncryptSessionkey = objtriple.Encrypt(Guid.NewGuid().ToString().Replace("-", "") + objuser.UserId.ToString() + objuser.Password.ToString()).Trim();
            ErrorLog error_log = new ErrorLog();
            var errorline = "1";
            string message = null;
            string mobileno = null;
            try
            {
                var client = new RestClient("https://cbsintegration.azurewebsites.net/api/UserValidate?UserName=" + encryptedUsername + "&Password=" + encryptedPassword);

                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                IRestResponse response = client.Execute(request);
                string Result = response.Content;
                Result = Result.Replace(@"\", "");
                if (Result.Contains("Success"))
                {
                    Result = Result.Substring(1, Result.Length - 2);
                    UserValidate objroot1 = JsonConvert.DeserializeObject<UserValidate>(Result);
                    string success = objroot1.success;
                    message = objroot1.message;
                    mobileno = objroot1.Mobile;
                    HttpContext.Session.SetString("MOBNO", mobileno);
                }
                else if (Result == "\"\"")
                {
                    return Json("No responce from Sil");
                }
                else
                {
                    return Json("failed");
                }

                if (message == "Validation Success.")
                {
                    return Json(message + "," + mobileno);
                }
                else
                {
                    return RedirectToAction("EmpOrganisationDetails", "EmployeeLogin");
                }
            }
            catch (Exception ex)
            {
                return Json(error_log);

            }
        }

        public ActionResult MainHomePage()

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
            public ActionResult touploadFiletoDB()
            {

                return View();
            }



            [HttpGet]
            public ActionResult CustomerRegistration()
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

        }
    }

