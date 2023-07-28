using INDO_FIN_NET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using INDO_FIN_NET.Models.CurrentModels;
using INDO_FIN_NET.Controllers.OrgnisationDetails;
using INDO_FIN_NET.Repository;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using ServiceProvider1.Models.OrgExceptionLogs;

namespace INDO_FIN_NET.Controllers.CurrectDetails
{
    public class CurrentindividualDocumentDetailsController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        private readonly string _connectionString;
        public CurrentindividualDocumentDetailsController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData1 = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
        }
        public ActionResult CurrentIndividualLivePhoto(string DocType)
        {
            return View();
        }

        public ActionResult CurrentIndividualDocOpenCamera1(string DocType)
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

        public ActionResult CurrentIndividualDocOpenCamera2(string DocType)
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
        public ActionResult CurrentIndividualDocOpenCamera3(string DocType)
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
            ClsCurrentDocDetails objdoc = new ClsCurrentDocDetails();
            //byte[] POAImage = null;
            //string Category = null;
            string Photo = null;
            objdoc.CustomerDetailId = 23;
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
                cmd2.Parameters.AddWithValue("@CustomerDetailId", objdoc.CustomerDetailId);
                MemoryStream cmpStream = new MemoryStream();
                GZipStream hgs = new GZipStream(cmpStream, CompressionMode.Compress);
                hgs.Write(POAImage, 0, POAImage.Length);
                byte[] DocImg = cmpStream.ToArray();
                cmd2.Parameters.AddWithValue("@document", DocImg);
                cmd2.Parameters.AddWithValue("@docName", objdoc.DocName);
                cmd2.Parameters.AddWithValue("@documentCategory", objdoc.DocCategory);
                cmd2.Parameters.AddWithValue("@docMainCategory", objdoc.DocMainCategory);
                cmd2.Parameters.AddWithValue("@docType", objdoc.DocType);
                cmd2.Parameters.AddWithValue("@source", objdoc.Source);
                connection2.Open();
                cmd2.ExecuteNonQuery();
                connection2.Close();



            }





            return Json("Success" + ",");
        }
        public ActionResult CurrentIndividualDocument()
        {
            var Ptype = HttpContext.Session.GetString("Ctype");
            if (Ptype == "PARTNERSHIP")
            {
                ViewBag.Ispartnership = "True";
            }
            //var CustomerId = "111";
            //var DocMainType = "I";
            //var Res = objDetails.Current_DocumentDetails.FromSqlRaw($"USP_GetCurrentDocument {(CustomerId)},{DocMainType}").ToList();
            //byte[] ma = Res[0].DocumentHistory;
            //MemoryStream cmpStream = new MemoryStream();
            //GZipStream hgs = new GZipStream(cmpStream, CompressionMode.Compress);
            //hgs.Write(ma, 0, ma.Length);
            //byte[] DocImg = cmpStream.ToArray();
            //string mmm = Convert.ToBase64String(DocImg);
            return View();
        }
        public ActionResult saveDocs()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string msg = "";
                int maxPartner = 3;
                var Partners = 0;
                var cid = HttpContext.Session.GetString("PersonalId");
                var pid = HttpContext.Session.GetString("PrimaryPartnerId");
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("USP_GetCountOfPartner", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@custId", pid);

                        Partners = (int)command.ExecuteScalar();
                    }
                }
                //Partners = Convert.ToInt32(HttpContext.Session.GetInt32("Addpartner"));
                if (Partners == 0)
                {
                    msg = "Please add a partner.";
                    Partners++;
                }
                else if (Partners == 1 || Partners == 2)
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_CountOfPartner", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@custCId", cid);
                            cmd.Parameters.AddWithValue("@Acount", Partners);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }

                    msg = "Do you want to add another partner?";
                    Partners++;
                }
                else if (Partners == 3)
                {
                    using (SqlConnection conn = new SqlConnection(_connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("USP_CountOfPartner", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@custCId", cid);
                            cmd.Parameters.AddWithValue("@Acount", Partners);
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    msg = "Partner Added Successfully.";
                    //Partners++;
                }
                else
                {
                    msg = "Error";

                }
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {

                    using (SqlCommand cmd = new SqlCommand("USP_UpdateCountOfPartner", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@custId", pid);
                        cmd.Parameters.AddWithValue("@count", Partners);


                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                TempData["addPartner"] = true;
                return Json(msg);
            }

            catch (Exception ex)
            {
                _ = error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
        public async Task<IActionResult> Uploadforindiviual(IFormFile file, string DocMainType, string DocumentCategory)
        {
            //IFormFile file = Request.Form.Files[0];
            Current_DocumentDetails objFinalDoc = new Current_DocumentDetails();
            ErrorLog error_log = new ErrorLog();
            try
            {
                if (file != null)
                {
                    //HttpContext.Session.SetString("PersonalId", "111");

                    var CustomerId = HttpContext.Session.GetString("PersonalId");
                    var userid = HttpContext.Session.GetString("UseID");
                    var DocName = file.FileName;
                    string extension = null;
                    string msg = null;
                    byte[] eibytes = null;
                    byte[] def1 = null;
                    byte[] ExtrctSign = null;
                    byte[] bytes = null;
                    byte[] POAImage = null;
                    byte[] faceextract = null;
                    byte[] signatureexract = null;
                    byte[] img = null;
                    string Category = null;
                    string filetype = null;
                    string Photo = null;

                    if (DocMainType == "SI")
                    {
                        string Source = "Uploaded";
                        BinaryReader reader = new BinaryReader(file.OpenReadStream());
                        eibytes = reader.ReadBytes((int)file.Length);
                        string conn1 = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn1))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_AddCurrentIndivitualDocuments", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerDetailId", CustomerId);
                            cmd2.Parameters.AddWithValue("@document", eibytes);
                            cmd2.Parameters.AddWithValue("@docName", file.FileName);
                            cmd2.Parameters.AddWithValue("@documentCategoryCode", "8");

                            cmd2.Parameters.AddWithValue("@docType", "Signature");

                            cmd2.Parameters.AddWithValue("@docMainType", DocMainType);
                            cmd2.Parameters.AddWithValue("@createdBy", "");
                            cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                            cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                            cmd2.Parameters.AddWithValue("@documentCategory", DocumentCategory);
                            cmd2.Parameters.AddWithValue("@Source", Source);
                            cmd2.Parameters.AddWithValue("@Prediction", "");
                            cmd2.Parameters.AddWithValue("@Faceext", faceextract);
                            cmd2.Parameters.AddWithValue("@Signature", signatureexract);
                            connection2.Open();
                            cmd2.ExecuteNonQuery();
                            connection2.Close();
                            return Json("Success");
                        }
                    }

                    if (file.FileName.Contains(".jpg") || file.FileName.Contains(".png"))
                    {

                        BinaryReader reader = new BinaryReader(file.OpenReadStream());
                        eibytes = reader.ReadBytes((int)file.Length);
                        //FOr Document Classification
                        var client = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentClassificationData");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Content-Type", "application/octet-stream");
                        request.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                        request.AddHeader("OrganisationCode", "ALP18980121");
                        request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                        request.AddParameter("application/octet-stream", eibytes, RestSharp.ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(response.Content);
                        if (DocMainType == "CA")
                        {
                            if (obj.DocumentType == "2")
                            {
                                return Json("Enter Wrong Document");
                            }
                        }


                        Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();

                        switch (obj.DocumentType)
                        {
                            case "1":
                                AadharMaskDetails.Add("DocumentType", "aadhar card");
                                msg = "Aadhaar Card Identified & Uploaded Successfully ";

                                break;
                            case "2":
                                AadharMaskDetails.Add("DocumentType", "pan card");
                                msg = "Pan Card Identified & Uploaded Successfully ";
                                break;
                            case "3":
                                AadharMaskDetails.Add("DocumentType", "Voter id");
                                msg = "Voter id Identified & Uploaded Successfully";
                                break;
                            case "4":
                                AadharMaskDetails.Add("DocumentType", "Driving licenses");
                                msg = "Driving license Identified & Uploaded Successfully";
                                break;
                            case "5":
                                AadharMaskDetails.Add("DocumentType", "Pass port");
                                msg = "Pass port Identified & Uploaded Successfully";
                                break;
                            case "6":
                                AadharMaskDetails.Add("DocumentType", "Invalid Document");
                                msg = "Invalid Document ";
                                break;
                            default:
                                AadharMaskDetails.Add("DocumentType", "Document Not Detected");
                                msg = "Document Not Detected ";
                                break;
                        }
                        switch (obj.StatusCode)
                        {
                            case "200":
                                AadharMaskDetails.Add("ErrorMgs", "Success");

                                break;
                            case "300":
                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                msg = "Missing AUTH-Headertoken";
                                break;
                            case "301":
                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                msg = "Invalid Content-Type";
                                break;
                            case "400":
                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                msg = "Invalid Image/Document";
                                break;
                            case "401":
                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                msg = "Invalid image file for Masking";
                                break;
                            case "500":
                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                msg = "Unsupported media type";
                                break;
                            case "501":
                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                msg = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                break;
                            case "502":
                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                msg = "The input image is too large. It should not be larger than 4MB.";
                                break;
                            case "503":
                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                msg = "Bad request image forma";
                                break;
                            case "504":
                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                msg = "Internal server erro";
                                break;
                            case "505":
                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                msg = "Exception message";
                                break;
                            default:
                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                msg = "Some exception occured";
                                break;
                        }
                        if (obj.DocumentType == "1")
                        {
                            var client1 = new RestClient("https://apigateway.indofinnet.com/api/DocumentMaskingData?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                            client1.Timeout = -1;
                            var request1 = new RestRequest(Method.POST);
                            request1.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                            request1.AddHeader("Content-Type", "image/jpeg");
                            request1.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                            IRestResponse response1 = client1.Execute(request1);
                            string Response2 = response1.Content.Replace("{", "").Replace(@"""", "");
                            string Response3 = Response2.Replace("}", "");

                            var jsonSendResponse = Response3.Split(',');
                            var Dtype = jsonSendResponse[1].Split(':', '\\')[4];
                            var Response5 = jsonSendResponse[2];
                            var jsonSendResponse1 = Response5.Split(':', '\\');
                            var Image1 = jsonSendResponse1[4];
                            if (Dtype == "AADHAR CARD")
                            {

                                if (file.FileName.Contains(".png"))
                                {
                                    objFinalDoc.DocumentCategoryCode = "1";
                                    objFinalDoc.DocMainCategory = "CA";
                                    Category = "Aadhar Card";
                                    filetype = (Category + "." + "png");
                                }
                                else
                                {
                                    objFinalDoc.DocumentCategoryCode = "1";
                                    objFinalDoc.DocMainCategory = "CA";
                                    Category = "Aadhar Card";
                                    filetype = (Category + "." + "jpg");
                                }


                            }

                            POAImage = Convert.FromBase64String(Image1);

                        }
                        if (obj.DocumentType == "4")
                        {
                            objFinalDoc.DocumentCategoryCode = "4";
                            objFinalDoc.DocMainCategory = "DL";
                            Category = "Driving License";
                            filetype = (Category + "." + "jpg");
                            POAImage = eibytes;// Convert.FromBase64String(Photo);

                        }
                        if (obj.DocumentType == "2")
                        {

                            if (file.FileName.Contains(".png"))
                            {
                                objFinalDoc.DocumentCategoryCode = "2";
                                objFinalDoc.DocMainCategory = "I";
                                Category = "Pan Card";
                                filetype = (Category + "." + "png");
                                POAImage = eibytes;
                            }
                            else
                            {
                                objFinalDoc.DocumentCategoryCode = "2";
                                objFinalDoc.DocMainCategory = "I";
                                Category = "Pan Card";
                                filetype = (Category + "." + "jpg");
                                POAImage = eibytes;
                            }
                            // Convert.FromBase64String(Photo);
                        }
                        if (obj.DocumentType == "3")
                        {
                            objFinalDoc.DocumentCategoryCode = "3";
                            objFinalDoc.DocMainCategory = "VI";
                            Category = "Voter id";
                            filetype = (Category + "." + "jpg");
                            POAImage = Convert.FromBase64String(Photo);

                        }
                        if (obj.DocumentType == "5")
                        {
                            objFinalDoc.DocumentCategoryCode = "5";
                            objFinalDoc.DocMainCategory = "PP";
                            Category = "Passport";
                            filetype = (Category + "." + "jpg");
                            POAImage = bytes;// Convert.FromBase64String(Photo);

                        }

                        objFinalDoc.CustomerDocumentId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

                        //Face Extraction//
                        if (msg == "Aadhaar Card Identified & Uploaded Successfully " || msg == "Pan Card Identified & Uploaded Successfully " || msg == "Voter id Identified & Uploaded Successfully" || msg == "Driving license Identified & Uploaded Successfully" || msg == "Pass port Identified & Uploaded Successfully")

                        {

                            var clientF = new RestClient("https://apigateway.indofinnet.com/api/DocumentFaceExtraction?OrgID=IndoFin007");
                            clientF.Timeout = -1;
                            var requestF = new RestRequest(Method.POST);
                            requestF.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                            requestF.AddHeader("Content-Type", "image/jpg");
                            requestF.AddParameter("image/jpg", POAImage, ParameterType.RequestBody);
                            IRestResponse responseF = clientF.Execute(requestF);
                            var aaa = responseF.Content;
                            dynamic output = JsonConvert.DeserializeObject(aaa);
                            dynamic output2 = JsonConvert.DeserializeObject(output);
                            var oikl = Convert.ToString(output2.Extacted_face);
                            faceextract = Convert.FromBase64String(oikl);
                            string Image1 = "data:image/jpg;base64," + oikl;

                            Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();

                            if (responseF.StatusCode.ToString() == "OK")
                            {
                                // var obj = JsonConvert.DeserializeObject<FaceExtractionAPI>(response.Content);
                                var obj1 = JsonConvert.DeserializeObject<FaceExtractionAPI>(JsonConvert.DeserializeObject<string>(responseF.Content));


                                AadharMaskDetails1.Add("StatusCode", obj1.StatusCode);
                                AadharMaskDetails1.Add("Message", obj1.Message);
                                AadharMaskDetails1.Add("probability", obj1.probability);
                                AadharMaskDetails1.Add("Extacted_Signature", obj1.Extacted_face);
                                string abc = obj1.Extacted_face;
                                //     var Photo = abc.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                                faceextract = Convert.FromBase64String(abc);

                                byte[] bytesImage1 = Convert.FromBase64String(abc);

                                def1 = bytesImage1;

                                switch (obj1.StatusCode)
                                {
                                    case "200":
                                        AadharMaskDetails1.Add("ErrorMgs", "Success");
                                        break;
                                    case "300":
                                        AadharMaskDetails1.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                        break;
                                    case "301":
                                        AadharMaskDetails1.Add("ErrorMgs", "Invalid Content-Type");
                                        break;
                                    case "400":
                                        AadharMaskDetails1.Add("ErrorMgs", "Invalid Image/Document");
                                        break;
                                    case "401":
                                        AadharMaskDetails1.Add("ErrorMgs", "Invalid image file for Masking");
                                        break;
                                    case "500":
                                        AadharMaskDetails1.Add("ErrorMgs", "Unsupported media type");
                                        break;
                                    case "501":
                                        AadharMaskDetails1.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                        break;
                                    case "502":
                                        AadharMaskDetails1.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                        break;
                                    case "503":
                                        AadharMaskDetails1.Add("ErrorMgs", "Bad request image format");
                                        break;
                                    case "504":
                                        AadharMaskDetails1.Add("ErrorMgs", "Internal server error");
                                        break;
                                    case "505":
                                        AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                        break;
                                    default:
                                        AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                        break;
                                }

                            }
                            //*****signature*******//
                            if (msg == "Pan Card Identified & Uploaded Successfully " || msg == "Driving license Identified & Uploaded Successfully" || msg == "Pass port Identified & Uploaded Successfully")
                            {
                                var clientS = new RestClient("https://apigateway.indofinnet.com/api/DocumentSignatureExtraction?OrgID=IndoFin007");
                                clientS.Timeout = -1;
                                var requestS = new RestRequest(Method.POST);
                                requestS.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                requestS.AddHeader("Content-Type", "image/jpg");
                                requestS.AddParameter("image/jpg", POAImage, ParameterType.RequestBody);
                                IRestResponse responseS = clientS.Execute(requestS);
                                var s1 = responseS.Content;
                                dynamic ds = JsonConvert.DeserializeObject(s1);
                                dynamic ds1 = JsonConvert.DeserializeObject(ds);
                                var sign = Convert.ToString(ds1.Extacted_Signature);
                                signatureexract = Convert.FromBase64String(sign);
                                string imagedata = "data:image/jpg;base64," + sign;

                                Dictionary<string, string> AadharMaskDetails2 = new Dictionary<string, string>();
                                if (responseS.StatusCode.ToString() == "OK")
                                {
                                    //var obj = JsonConvert.DeserializeObject<SignatureExtractApi>(response.Content);

                                    var obj1 = JsonConvert.DeserializeObject<SignatureExtractApi>(JsonConvert.DeserializeObject<string>(responseS.Content));

                                    AadharMaskDetails2.Add("StatusCode", obj1.StatusCode);
                                    AadharMaskDetails2.Add("Message", obj1.Message);
                                    AadharMaskDetails2.Add("probability", obj1.probability);
                                    AadharMaskDetails2.Add("Extacted_Signature", obj1.Extacted_Signature);
                                    string abc = obj1.Extacted_Signature;

                                    signatureexract = Convert.FromBase64String(abc);



                                    byte[] bytesImage1 = Convert.FromBase64String(abc);

                                    ExtrctSign = bytesImage1;

                                    switch (obj1.StatusCode)
                                    {
                                        case "200":
                                            AadharMaskDetails2.Add("ErrorMgs", "Success");
                                            break;
                                        case "300":
                                            AadharMaskDetails2.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                            break;
                                        case "301":
                                            AadharMaskDetails2.Add("ErrorMgs", "Invalid Content-Type");
                                            break;
                                        case "400":
                                            AadharMaskDetails2.Add("ErrorMgs", "Invalid Image/Document");
                                            break;
                                        case "401":
                                            AadharMaskDetails2.Add("ErrorMgs", "Invalid image file for Masking");
                                            break;
                                        case "500":
                                            AadharMaskDetails2.Add("ErrorMgs", "Unsupported media type");
                                            break;
                                        case "501":
                                            AadharMaskDetails2.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                            break;
                                        case "502":
                                            AadharMaskDetails2.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                            break;
                                        case "503":
                                            AadharMaskDetails2.Add("ErrorMgs", "Bad request image format");
                                            break;
                                        case "504":
                                            AadharMaskDetails2.Add("ErrorMgs", "Internal server error");
                                            break;
                                        case "505":
                                            AadharMaskDetails2.Add("ErrorMgs", "Exception message");
                                            break;
                                        default:
                                            AadharMaskDetails2.Add("ErrorMgs", "Some exception occured");
                                            break;
                                    }


                                }
                            }
                            //*********************//
                        }
                        //##############//

                        //objFinalDoc.DocumentDetails = POAImage;
                        //objFinalDoc.documentCategory = POItype;
                        objFinalDoc.DocumentName = "POACamImage";
                        extension = objFinalDoc.DocumentName.Split('.').LastOrDefault();
                        //objFinalDoc.DocCategoryCode = ddl_idProof;
                        objFinalDoc.Source = "Upload";

                        objFinalDoc.Faceext = def1;
                        objFinalDoc.Signature = ExtrctSign;

                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_AddCurrentIndivitualDocuments", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDocumentId);
                            cmd2.Parameters.AddWithValue("@document", POAImage);
                            cmd2.Parameters.AddWithValue("@docName", filetype);
                            cmd2.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocumentCategoryCode);
                            if (DocMainType == "I")
                            {
                                cmd2.Parameters.AddWithValue("@docType", "POI");
                            }
                            else if (DocMainType == "CA")
                            {
                                cmd2.Parameters.AddWithValue("@docType", "POA");
                            }
                            else
                            {
                                cmd2.Parameters.AddWithValue("@docType", "Signature");
                            }
                            cmd2.Parameters.AddWithValue("@docMainType", DocMainType);
                            cmd2.Parameters.AddWithValue("@createdBy", "");
                            cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                            cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                            cmd2.Parameters.AddWithValue("@documentCategory", Category);
                            cmd2.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                            cmd2.Parameters.AddWithValue("@Prediction", "");
                            cmd2.Parameters.AddWithValue("@Faceext", faceextract);
                            cmd2.Parameters.AddWithValue("@Signature", signatureexract);
                            connection2.Open();
                            cmd2.ExecuteNonQuery();
                            connection2.Close();
                            return Json("Success");

                        }


                    }
                    else
                    {
                        return Json("Valid");
                    }
                }
                else
                {
                    return Json("Uploded Failed");
                }




                return Json("");
            }
            catch (Exception ex)
            {
                _ = error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }


        }

        [HttpPost]
        public ActionResult GetCurrentIndivitualDocumentsForGridView(string DocMainType)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var CustomerId = HttpContext.Session.GetString("PersonalId");

                var Res = objDetails.Current_DocumentDetails.FromSqlRaw($"USP_GetCurrentIndivitualDocument {(CustomerId)},{DocMainType}").ToList();
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
        public ActionResult deleteCurrentIndivitualDocuments(long? docId)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_DeleteCurrentIndiviualDocuments", connection);
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
