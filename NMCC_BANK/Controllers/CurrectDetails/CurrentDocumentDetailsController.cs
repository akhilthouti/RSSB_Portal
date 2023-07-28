using Grpc.Core;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Models.CurrentModels;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestSharp;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
namespace INDO_FIN_NET.Controllers.CurrectDetails
{
    public class CurrentDocumentDetailsController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        private readonly string _connectionString;

        public CurrentDocumentDetailsController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        public ActionResult CurrentDocOpenCamera1(string DocType)
        {
            try
            {
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    HttpContext.Session.GetString("PersonalId");
                    HttpContext.Session.GetString("DAEditCustomerdetailId");
                    ViewBag.AdminFlag = "AdminFlag";
                }
                if (DocType == "DOCPOI")
                {
                    ViewBag.DocPOI = true;
                }
                if (DocType == "DOCPOA")
                {
                    ViewBag.DocPOA = true;
                }
                if (DocType == "DOCSign")
                {
                    ViewBag.DOCSign = true;
                }
                return View();
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }

        public ActionResult CurrentDocOpenCamera2(string DocType)
        {
            try
            {
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    HttpContext.Session.GetString("PersonalId");
                    HttpContext.Session.GetString("DAEditCustomerdetailId");
                    ViewBag.AdminFlag = "AdminFlag";
                }
                if (DocType == "DOCPOI")
                {
                    ViewBag.DocPOI = true;
                }
                if (DocType == "DOCPOA")
                {
                    ViewBag.DocPOA = true;
                }
                if (DocType == "DOCSign")
                {
                    ViewBag.DOCSign = true;
                }
                return View();
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }
        public ActionResult CurrentDocOpenCamera3(string DocType)
        {
            try
            {
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    HttpContext.Session.GetString("PersonalId");
                    HttpContext.Session.GetString("DAEditCustomerdetailId");
                    ViewBag.AdminFlag = "AdminFlag";
                }
                if (DocType == "DOCPOI")
                {
                    ViewBag.DocPOI = true;
                }
                if (DocType == "DOCPOA")
                {
                    ViewBag.DocPOA = true;
                }
                if (DocType == "DOCCamSign")
                {
                    ViewBag.DOCSign = true;
                }
                return View();
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }

        }

        public ActionResult DOCImageCapture12(string DocType, string base64DOC_POI, string base64DOC_POA, string base64DOC_Sign, string POItype, string IdProof)
        {
            //ClsCurrentDocDetails objdoc = new ClsCurrentDocDetails();
            Current_indivitualVerification objdoc= new Current_indivitualVerification();    
            //byte[] POAImage = null;
            //string Category = null;
            string Photo = null;
            var CustomerId = HttpContext.Session.GetString("PersonalId");
            if (DocType == "DOCPOI")
            {
                Photo = base64DOC_POI.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                objdoc.DocType = "POI";
                if (IdProof == "Certificate of Incorporation")
                {
                    objdoc.DocCategory = "CertificteofIncorp";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "I";
                }
                if (IdProof == "Memorandum Association")
                {
                    objdoc.DocCategory = "Memorandum_Assoc";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "I";
                }
                if (IdProof == "Resolution of Board")
                {
                    objdoc.DocCategory = "Resolution_of_Board";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "I";
                }
                if (IdProof == "Partnership Deed")
                {
                    objdoc.DocCategory = "Partnership_Deed";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "I";
                }
                if (IdProof == "Trust Deed")
                {
                    objdoc.DocCategory = "Trust_Deed";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "I";
                }
                if (IdProof == "Power of Attorney")
                {
                    objdoc.DocCategory = "Power_OF_Attorney";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "I";
                }



            }
            if (DocType == "DOCPOA")
            {
                Photo = base64DOC_POA.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                objdoc.DocType = "POA";
                if (IdProof == "Certificate of Incorporation")
                {
                    objdoc.DocCategory = "CertificteofIncorp";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "CA";
                }
                if (IdProof == "Registration Certificate")
                {
                    objdoc.DocCategory = "Registration_Certificate";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "CA";
                }
                if (IdProof == "Other document")
                {
                    objdoc.DocCategory = "Other_Doc";
                    objdoc.DocName = IdProof + ".jpg";
                    objdoc.DocMainCategory = "CA";
                }

            }
            if (DocType == "DOCCamSign")
            {
                Photo = base64DOC_Sign.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                objdoc.DocType = "Sign";
                objdoc.DocCategory = "Signature";
                objdoc.DocName = "Signature.jpg";
                objdoc.DocMainCategory = "SI";
            }

            byte[] POAImage = Convert.FromBase64String(Photo);
            objdoc.DocDetails = POAImage;
            objdoc.Source = "Captured";
            string conn = _connectionString;
            using (SqlConnection connection2 = new SqlConnection(conn))
            {
                SqlCommand cmd2 = new SqlCommand("USP_AddCurrentCapturedDocuments", connection2);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@CustomerDetailId", CustomerId);
                cmd2.Parameters.AddWithValue("@document", POAImage);
                cmd2.Parameters.AddWithValue("@docName", objdoc.DocName);
                cmd2.Parameters.AddWithValue("@documentCategory", objdoc.DocCategory);
                cmd2.Parameters.AddWithValue("@docMainCategory", objdoc.DocMainCategory);
                cmd2.Parameters.AddWithValue("@docType", objdoc.DocType);
                cmd2.Parameters.AddWithValue("@source", objdoc.Source);
                connection2.Open();
                cmd2.ExecuteNonQuery();
                connection2.Close();



            }





            return Json("Success");
        }


        public IActionResult CurrentDocumentDetail()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file,string DocMainType,string DocumentCategory)
        {
            //IFormFile file = Request.Form.Files[0];
            ErrorLog error_log = new ErrorLog();
            try
            {


                var CustomerId = HttpContext.Session.GetString("PersonalId");
                var userid = HttpContext.Session.GetString("UseID");
                var DocName = file.FileName;
                byte[] eibytes = null;
                if (file.FileName.Contains(".jpg"))
                {

                    BinaryReader reader = new BinaryReader(file.OpenReadStream());
                    eibytes = reader.ReadBytes((int)file.Length);
                    string conn = _connectionString;
                    using (SqlConnection connection = new SqlConnection(conn))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand("USP_InsertCurrentDocument", connection);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustId", CustomerId);
                        cmd.Parameters.AddWithValue("@documentHistory", eibytes);
                        cmd.Parameters.AddWithValue("@documentName", DocName);
                        cmd.Parameters.AddWithValue("@documentMainCategory", DocMainType);
                        cmd.Parameters.AddWithValue("@createdBy", userid);
                        if(DocumentCategory== "Signature")
                        {
                            cmd.Parameters.AddWithValue("@documentCategory", "Signature");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@documentCategory", DocumentCategory);
                        }
                        if(DocMainType=="I")
                        {
                            cmd.Parameters.AddWithValue("@documentType", "POI");
                        }
                        else if(DocMainType== "CA")
                        {
                            cmd.Parameters.AddWithValue("@documentType", "POA");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@documentType", "Signature");
                        }
                        
                        cmd.ExecuteReader();
                        connection.Close();
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

        public ActionResult GetCurrentDocumentsForGridView(string DocMainType)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var CustomerId = HttpContext.Session.GetString("PersonalId");
                
                var Res = objDetails.Current_DocumentDetails.FromSqlRaw($"USP_GetCurrentDocument {(CustomerId)},{DocMainType}").ToList();
                ViewBag.Message = Res;

                return View(Res);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.ToString());
            }
        }


        [HttpGet]
        public ActionResult deleteCurrentDocuments(long? docId)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_DeleteCurrentDocuments", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@documentId", docId);
                    connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                }
                return Json("1");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.ToString());
            }
        }

    }
}
