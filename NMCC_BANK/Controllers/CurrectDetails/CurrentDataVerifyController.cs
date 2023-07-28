using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using INDO_FIN_NET.Models.CurrentModels;
using INDO_FIN_NET.Models;
using System;
using Newtonsoft.Json;
using INDO_FIN_NET.Controllers.Organisation;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Specialized;
using Microsoft.Data.SqlClient;
using System.Data;

namespace INDO_FIN_NET.Controllers.CurrectDetails
{
    public class CurrentDataVerifyController : Controller
    {
        private readonly INDO_FinNetDbCotext objData;
        private readonly RSSBPRODDbCotext objDetails;

        public CurrentDataVerifyController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_)
        {
            objDetails = Context;
            objData = iNDO_;
        }
        public ActionResult CurrentSummarySheetDetails(CIN_Verification obj, int userId)
        {
            ErrorLog error_log = new ErrorLog();
            var username = HttpContext.Session.GetString("UserName");
            if (username != null)
            {
                ViewBag.UserName = username;
            }
            else
            {
                ViewBag.UserName = "Prafull";
            }
            var mobileno = HttpContext.Session.GetString("MOBNO");
            if (mobileno != null)
            {
                ViewBag.MOBNO = mobileno;
            }
            try
            {
                //var dataforCIN = objDetails.CIN_Verifications.FromSqlRaw($"USP_GetCINDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault();
                //var CustomerId = 3;
                //var dataforCIN = objDetails.CIN_Verifications.FromSqlRaw($"USP_GetCINDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault();
                var CustomerId = HttpContext.Session.GetString("PersonalId");
                var dataforCIN = objDetails.CIN_Verifications.FromSqlRaw($"USP_GetCINDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault();
                if (dataforCIN != null)
                {
                    obj.CompanyName = dataforCIN.CompanyName;
                    obj.CompanyType = dataforCIN.CompanyType;
                    obj.AuthorizedCapital = dataforCIN.AuthorizedCapital;
                    obj.Category = dataforCIN.Category;
                    obj.DateofIncorporation = dataforCIN.DateofIncorporation;
                    obj.LastAnnualGeneralMeetingDate = dataforCIN.LastAnnualGeneralMeetingDate;
                    obj.NumberofDirectors = dataforCIN.NumberofDirectors;
                    obj.RegisteredAddress = dataforCIN.RegisteredAddress;
                    obj.RegisteredEmailId = dataforCIN.RegisteredEmailId;
                    obj.RegistrationNumber = dataforCIN.RegistrationNumber;
                    obj.StatusForEfiling = dataforCIN.StatusForEfiling;
                    obj.Name = dataforCIN.Name;
                    obj.DateofAppointment = dataforCIN.DateofAppointment;
                    obj.Name2 = dataforCIN.Name2;
                    obj.DateofAppointment2 = dataforCIN.DateofAppointment2;
                }

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }


            return View(obj);

            //return View();
        }
        [AllowAnonymous]
        [HttpGet]
        [Obsolete]
        public async void CurrentGeneratepdf()
        {
            CurrentPdfCreation genpdf = new CurrentPdfCreation();

            //long CustomerId = Convert.ToInt64(HttpContext.Session.GetString(("PersonalId")));
            var CustomerId = HttpContext.Session.GetString("PersonalId");
            ViewBag.refId = CustomerId.ToString();
            var cindata = objDetails.CIN_Verifications.FromSqlRaw($"USP_GetCINDataForCAF {CustomerId}").AsEnumerable().FirstOrDefault();
            var result = objDetails.CAFCustomerDetails.FromSqlRaw($"USP_GetCAFCustomerDetails {CustomerId}").AsEnumerable().FirstOrDefault();

            if ((cindata != null) && (result != null))
            {
                genpdf.CcompanyName = cindata.CompanyName;
                genpdf.CDateofIncorporation = cindata.DateofIncorporation;
                genpdf.CcompanyType = cindata.CompanyType;
                genpdf.CRegisteredAddress = cindata.RegisteredAddress;
                genpdf.CRegisteredEmailId = cindata.RegisteredEmailId;
                genpdf.CRegistrationNumber = cindata.RegistrationNumber;
                genpdf.CAuthorizedCapital = cindata.AuthorizedCapital;
                genpdf.CDIN = cindata.Name;
                genpdf.CDIN2 = cindata.Name2;
                genpdf.CompanyName = result.Comapnyname;
                genpdf.IndustryType = result.Industrytype;
                genpdf.BusinessTL = result.BusinessTL;
                genpdf.DOE = result.DOE;
                genpdf.POE = result.POE;
                genpdf.NOE = result.NOE;
                genpdf.Branches = result.Branches;
                genpdf.Turnover = result.Turnover;
                genpdf.Email = result.Email;
                genpdf.Mobileno = result.Mobile;
                genpdf.LandLine = result.Landline;
                genpdf.PAddress1 = result.PAddress1;
                genpdf.PAddress2 = result.PAddress2;
                genpdf.PAddress3 = result.PAddress3;
                genpdf.PCity = result.PCity;
                genpdf.Pincode = result.PPincode;
                genpdf.PSate = result.PState;
                genpdf.PCountry = result.PCountry;
            }


            var pdf = new Rotativa.AspNetCore.ViewAsPdf("CurrentGeneratepdf", genpdf)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
            };

            byte[] b = await pdf.BuildFile(ControllerContext);
            string stringpdf = Convert.ToBase64String(b);
            //CurrenteSignResponce(stringpdf);
            string refId = ViewBag.refId;// "a12";
            //string Cname = ViewBag.firstname;//= result.FirstName;
            string Cname = "ashish";
            NameValueCollection collections = new NameValueCollection();
            collections.Add("custname", Cname);
            collections.Add("APIKey", "");
            //collections.Add("OrgID", "Indofin01");//Local
            //collections.Add("OrgID", "RSSB01");//Publish
            collections.Add("OrgID", "NMCC01");//testing
            collections.Add("Doc", stringpdf);


            string remoteUrl = "https://esign.indofinnet.com/api/AadharEsign/AadhaarEsign?refID=" + refId;

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


            //return View(genpdf);
        }

        [HttpPost]
        public ActionResult CurrenteSignResponce(String refID)
        {
            long s = Convert.ToInt64(refID);
            string doc = Request.Form["resp"].ToString();

            byte[] data = Convert.FromBase64String(doc);
            //using (SqlConnection cn = new SqlConnection(_connectionString))
            //{
            //    cn.Open();
            //    SqlCommand cmd = new SqlCommand("USP_AddEsignpdf", cn);
            //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@CustId", s);
            //    cmd.Parameters.AddWithValue("@data", data);
            //    cmd.ExecuteNonQuery();

            //    string conn = _connectionString;

            //    using (SqlConnection connection3 = new SqlConnection(conn))
            //    {
            //        SqlCommand cmd3 = new SqlCommand("USP_EsignFlag", connection3);
            //        cmd3.CommandType = CommandType.StoredProcedure;
            //        cmd3.Parameters.AddWithValue("@CustId", s);// Convert.ToInt64(HttpContext.Session.GetString("Ecid")));
            //        connection3.Open();
            //        SqlDataReader reader3 = cmd3.ExecuteReader();
            //        if (reader3.Read())
            //        {
            //            //var Result = reader2["RESULT"].ToString();
            //        }
            //    }
            //}
            ViewBag.DocPdf = "data:application/pdf;base64," + Convert.ToBase64String(data, 0, data.Length);

            return View();


        }


        public ActionResult GOTOCURRENTACCOUNT()
        {
            var companyname = TempData["CompanyName"] as string;
            var industrytype = TempData["IndustryType"] as string;
            var businesstype = TempData["BusinessType"] as string;
            var dateestd = TempData["DateESTD"] as string;
            var placeestd = TempData["PlaceESTD"] as string;
            var noofbranches = TempData["NoOfBranches"] as string;
            var noofemp = TempData["NoOfEmp"] as string;
            var turnover = TempData["Turnover"] as string;
            var address1 = TempData["Address1"] as string;
            var address2 = TempData["Address2"] as string;
            var city = TempData["City"] as string;
            var pincode = TempData["Pincode"] as string;
            var state = TempData["State"] as string;
            var country = TempData["Country"] as string;
            var email = TempData["Email"] as string;
            var mobileno = TempData["MobileNo"] as string;
            var landlineno = TempData["LandlineNo"] as string;
            var cinno = TempData["CinNo"] as string;
            var companypan = TempData["CompanyPan"] as string;
            var panNo = TempData["PanNo"] as string;
            var gstNo = TempData["GSTNo"] as string;
            var udyogAdhaarNo = TempData["UdyogAdhaarNo"] as string;



            ViewData["CompanyName"] = companyname;
            ViewData["IndustryType"] = industrytype;
            ViewData["BusinessType"] = businesstype;
            ViewData["DateESTD"] = dateestd;
            ViewData["PlaceESTD"] = placeestd;
            ViewData["NoOfBranches"] = noofbranches;
            ViewData["NoOfEmp"] = noofemp;
            ViewData["Turnover"] = turnover;
            ViewData["Address1"] = address1;
            ViewData["Address2"] = address2;
            ViewData["City"] = city;
            ViewData["Pincode"] = pincode;
            ViewData["State"] = state;
            ViewData["Country"] = country;
            ViewData["Email"] = email;
            ViewData["MobileNo"] = mobileno;
            ViewData["LandlineNo"] = landlineno;
            ViewData["CinNo"] = cinno;
            ViewData["CompanyPan"] = companypan;
            ViewData["PanNo"] = panNo;
            ViewData["GSTNo"] = gstNo;
            ViewData["UdyogAdhaarNo"] = udyogAdhaarNo;


            TempData.Keep();


            return View();
        }


    }
}
