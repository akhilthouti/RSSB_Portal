using Amazon.SimpleSystemsManagement;
using AutoMapper;
using Grpc.Core;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
//using RestSharp;
using ServiceProvider1.Models;
using ServiceProvider1.Models.OrgExceptionLogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using tessnet2;
using Microsoft.IdentityModel.Clients.ActiveDirectory; //ADAL client library for getting the access token
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Win32;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Org.BouncyCastle.Asn1.Ocsp;
using ParameterType = RestSharp.ParameterType;
using System.IO.Compression;
using Microsoft.Extensions.Configuration;

namespace INDO_FIN_NET.Controllers.OrgnisationDetails
{

    public class DocumentDetailsController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        private readonly string _connectionString;

        public DocumentDetailsController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        string extension = null;
        public long? isInserted = null;
        Random random = new Random();
        string msg;
        byte[] bytesImage = null;
        string imagePath = "";
        string msgs = "";
        byte[] faceextract = null;
        byte[] signatureexract = null;
        byte[] img = null;


        #region CameraCapture

        public ActionResult DocOpenCamera1(string DocType)
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

        public ActionResult DocOpenCamera2(string DocType)
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

        public ActionResult DocOpenCamera3(string DocType)
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
            ClsDocumentDetails objdoc = new ClsDocumentDetails();
            ClsDocDetails objFinalDoc = new ClsDocDetails();
            objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
            byte[] def1 = null;
            byte[] ExtrctSign = null;
            byte[] eibytes1 = null;
            byte[] POAImage = null;
            string Category = null;
            string filetype = null;
            string Photo = null;


            if (DocType == "DOCPOI")
            {
                Photo = base64DOC_POI.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
            }
            if (DocType == "DOCPOA")
            {
                Photo = base64DOC_POA.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,   
            }

            byte[] bytes = Convert.FromBase64String(Photo);
            var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentClassificationData?OrgID=IndoFin007");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
            request.AddHeader("Content-Type", "image/jpeg");
            request.AddParameter("image/jpeg", bytes, RestSharp.ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            dynamic res21 = JsonConvert.DeserializeObject(response.Content);
            var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(res21);
            if (DocType == "DOCCamSign")
            {
                if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "4" || obj.DocumentType == "5")
                {
                    msg = "Sorry...! This Doccument cannot be used as Signature.";
                    return Json("Success" + "," + msg);
                }
                Photo = base64DOC_Sign.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                byte[] eibytes2 = Convert.FromBase64String(Photo);

                string conn1 = _connectionString;
                using (SqlConnection connection2 = new SqlConnection(conn1))
                {
                    SqlCommand cmd2 = new SqlCommand("USP_AddDocumentsFace", connection2);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@document", eibytes2);
                    cmd2.Parameters.AddWithValue("@docName", "Sign.jpg");
                    cmd2.Parameters.AddWithValue("@documentCategoryCode", "8");
                    cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@docMainType", "SI");
                    cmd2.Parameters.AddWithValue("@createdBy", "");
                    cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@DocumentIdDate", null);
                    cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                    cmd2.Parameters.AddWithValue("@documentCategory", "Signature");
                    cmd2.Parameters.AddWithValue("@Source", "Captured");
                    cmd2.Parameters.AddWithValue("@Faceext", null);
                    cmd2.Parameters.AddWithValue("@Signature", null);
                    connection2.Open();
                    isInserted = cmd2.ExecuteNonQuery();
                    connection2.Close();
                    //msg = "Signature Captured Successfully";
                    //return Json("Success" + "," + msg);
                    return Json("Success");

                }
            }
            if (IdProof == "Aadhaar/UID No")
            {
                if (obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "4" || obj.DocumentType == "5")
                {
                    msg = "Sorry...! This Doccument cannot be used as Aadhaar/UID No. Please select the appropriate document";
                    return Json("Success" + "," + msg);
                }
            }
            if (IdProof == "Pan Card")
            {
                if (obj.DocumentType == "1" || obj.DocumentType == "3" || obj.DocumentType == "4" || obj.DocumentType == "5")
                {
                    msg = "Sorry...! This Doccument cannot be used as Pan Card. Please select the appropriate document";
                    return Json("Success" + "," + msg);
                }
            }
            if (IdProof == "Driving License " || IdProof == "Driving License")
            {
                if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "5")
                {
                    msg = "Sorry...! This Doccument cannot be used as Driving License. Please select the appropriate document";
                    return Json("Success" + "," + msg);
                }
            }
            if (IdProof == "Passport " || IdProof == "Passport")
            {
                if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "4")
                {
                    msg = "Sorry...! This Doccument cannot be used as Passport. Please select the appropriate document";
                    return Json("Success" + "," + msg);
                }
            }
            if (IdProof == "Voter Identity Card " || IdProof == "Voter Identity Card")
            {
                if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "4" || obj.DocumentType == "5")
                {
                    msg = "Sorry...! This Doccument cannot be used as Leave and License Agreement. Please select the appropriate document";
                    return Json("Success" + "," + msg);
                }
            }
            if (IdProof == "Leave and License Agreement")
            {
                if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "4" || obj.DocumentType == "5")
                {
                    msg = "Sorry...! This Doccument cannot be used as Leave and License Agreement. Please select the appropriate document";
                    return Json("Success" + "," + msg);
                }

                string fname1 = "EL/LL.jpg";
                string conn1 = _connectionString;
                using (SqlConnection connection2 = new SqlConnection(conn1))
                {
                    SqlCommand cmd2 = new SqlCommand("USP_AddDocuments", connection2);

                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@document", bytes);
                    cmd2.Parameters.AddWithValue("@docName", fname1);
                    cmd2.Parameters.AddWithValue("@documentCategoryCode", "6");
                    cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@docMainType", "EL/LL");
                    cmd2.Parameters.AddWithValue("@createdBy", "");
                    cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                    cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                    cmd2.Parameters.AddWithValue("@documentCategory", IdProof);
                    cmd2.Parameters.AddWithValue("@Source", "Captured");
                    cmd2.Parameters.AddWithValue("@Prediction", "");
                    if (DocType == "DOCPOI")
                    {
                        cmd2.Parameters.AddWithValue("@doctypecode", DocType);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@doctypecodeforadd", DocType);
                    }

                    connection2.Open();
                    cmd2.ExecuteNonQuery();
                    connection2.Close();
                    msg = "Doccument Captured Successfully";
                    return Json("Success" + "," + msg);

                }
            }


            Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
            if (DocType == "DOCPOA")
            {
                if (obj.DocumentType == "2")
                {
                    msg = "Sorry...! PAN card cannot be used as an address document. Please select the appropriate document";
                    return Json("Success" + "," + msg);
                }
            }

            switch (obj.DocumentType)
            {
                case "1":
                    AadharMaskDetails.Add("DocumentType", "aadhar card");
                    msg = "Aadhaar Card Identified & Captured Successfully ";

                    break;
                case "2":
                    AadharMaskDetails.Add("DocumentType", "pan card");
                    msg = "Pan Card Identified & Captured Successfully ";
                    break;
                case "3":
                    AadharMaskDetails.Add("DocumentType", "Voter id");
                    msg = "Voter id Identified & Captured Successfully";
                    break;
                case "4":
                    AadharMaskDetails.Add("DocumentType", "Driving licenses");
                    msg = "Driving license Identified & Captured Successfully";
                    break;
                case "5":
                    AadharMaskDetails.Add("DocumentType", "Pass port");
                    msg = "Pass port Identified & Captured Successfully";
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
                var client1 = new RestClient("https://apigateway.indofinnet.com/api/DocumentMaskingData?OrgID=IndoFin007");
                client1.Timeout = -1;
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                request1.AddHeader("Content-Type", "image/jpeg");
                request1.AddParameter("image/jpeg", bytes, RestSharp.ParameterType.RequestBody);
                IRestResponse response1 = client1.Execute(request1);
                Console.WriteLine(response1.Content);
                string Response2 = response1.Content.Replace("{", "").Replace(@"""", "");
                string Response3 = Response2.Replace("}", "");

                var jsonSendResponse = Response3.Split(',');
                var Dtype = jsonSendResponse[1].Split(':', '\\')[4];
                var Response5 = jsonSendResponse[2];
                var jsonSendResponse1 = Response5.Split(':');
                var Image1 = jsonSendResponse1[1];
                Image1 = Image1.Replace(@"\", "");
                if (Dtype == "AADHAR CARD")
                {
                    objFinalDoc.DocCategoryCode = "1";
                    objFinalDoc.DocMainType = "CA";
                    Category = "Aadhar Card";
                    filetype = (Category + "." + "jpg");

                }

                POAImage = Convert.FromBase64String(Image1);

            }
            if (obj.DocumentType == "4")
            {
                objFinalDoc.DocCategoryCode = "4";
                objFinalDoc.DocMainType = "DL";
                Category = "Driving License";
                filetype = (Category + "." + "jpg");
                POAImage = Convert.FromBase64String(Photo);

            }
            if (obj.DocumentType == "2")
            {
                objFinalDoc.DocCategoryCode = "2";
                objFinalDoc.DocMainType = "I";
                Category = "Pan Card";
                filetype = (Category + "." + "jpg");
                POAImage = Convert.FromBase64String(Photo);
            }
            if (obj.DocumentType == "3")
            {
                objFinalDoc.DocCategoryCode = "3";
                objFinalDoc.DocMainType = "VI";
                Category = "Voter id";
                filetype = (Category + "." + "jpg");
                POAImage = Convert.FromBase64String(Photo);

            }
            if (obj.DocumentType == "5")
            {
                objFinalDoc.DocCategoryCode = "5";
                objFinalDoc.DocMainType = "PP";
                Category = "Passport";
                filetype = (Category + "." + "jpg");
                POAImage = Convert.FromBase64String(Photo);

            }
            if (obj.DocumentType == null && DocType == "DOCPOA")
            {
                objFinalDoc.DocCategoryCode = "6";
                objFinalDoc.DocMainType = "PP";
                Category = "EL/LL";
                filetype = (Category + "." + "jpg");
                POAImage = Convert.FromBase64String(Photo);
                msg = "Doccument Captured Successfully";
            }
            if (obj.DocumentType == null && DocType == "DOCPOI")
            {
                return Json("Success" + "," + msg);

            }
            objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

            //Face Extraction//
            if (msg == "Aadhaar Card Identified & Captured Successfully " || msg == "Pan Card Identified & Captured Successfully " || msg == "Voter id Identified & Captured Successfully" || msg == "Driving license Identified & Captured Successfully" || msg == "Pass port Identified & Captured Successfully")

            {
                //*****Data Extraction*******//

                var client12 = new RestClient("https://apigateway.indofinnet.com/api/DataExtraction?OrgID=IndoFin007");
                client12.Timeout = -1;
                var request12 = new RestRequest(Method.POST);
                request12.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                request12.AddHeader("Content-Type", "image/jpg");
                request12.AddParameter("image/jpg", POAImage, ParameterType.RequestBody);
                IRestResponse response12 = client12.Execute(request12);
                var res = response12.Content;
                var result = JsonConvert.DeserializeObject<DocumentExtractionAPI>(JsonConvert.DeserializeObject<string>(response12.Content));

                var knor = Convert.ToString(result.StatusCode);
                string conn1 = _connectionString;
                using (SqlConnection connection12 = new SqlConnection(conn1))
                {
                    SqlCommand cmd12 = new SqlCommand("USP_InsertAIDocumentExtraction1", connection12);
                    cmd12.CommandType = CommandType.StoredProcedure;
                    cmd12.Parameters.AddWithValue("@Customerid", objFinalDoc.CustomerDetailId);
                    cmd12.Parameters.AddWithValue("@StatusCode", result.StatusCode);
                    cmd12.Parameters.AddWithValue("@CardName", result.Card_Name);
                    cmd12.Parameters.AddWithValue("@Documentid", result.customer_document_id);
                    cmd12.Parameters.AddWithValue("@fullname", result.customer_full_name);
                    cmd12.Parameters.AddWithValue("@dob", result.customer_dob);
                    cmd12.Parameters.AddWithValue("@gender", result.customer_gender);
                    cmd12.Parameters.AddWithValue("@relationtype", result.customer_relation_type);
                    cmd12.Parameters.AddWithValue("@initialname", result.customer_name_initial);
                    cmd12.Parameters.AddWithValue("@fname", result.customer_fname);
                    cmd12.Parameters.AddWithValue("@mname", result.customer_mname);
                    cmd12.Parameters.AddWithValue("@lname", result.customer_lname);
                    connection12.Open();
                    cmd12.ExecuteNonQuery();
                    connection12.Close();
                    var msg12 = "Data Extracted";
                    ViewBag.Message = msg12;


                }

                //###########################//
                //***Face Extract ***//
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

                    if (imagePath == null)
                    {
                        //bytesImage = System.IO.File.ReadAllBytes((imagePath));
                        bytesImage = eibytes1;
                    }
                    else
                    {
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
                }
                //*****signature*******//
                if (msg == "Pan Card Identified & Captured Successfully " || msg == "Driving license Identified & Captured Successfully" || msg == "Pass port Identified & Captured Successfully")
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

                        //if (imagePath != "")
                        if (imagePath == null)
                        {
                            // bytesImage = System.IO.File.ReadAllBytes(Path.GetFullPath(imagePath));
                            bytesImage = eibytes1;
                        }
                        else
                        {
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
                }
                //*********************//
            }
            //##############//

            objFinalDoc.DocDetails = POAImage;
            objFinalDoc.documentCategory = POItype;
            objFinalDoc.DocName = "POACamImage";
            extension = objFinalDoc.DocName.Split('.').LastOrDefault();
            //objFinalDoc.DocCategoryCode = IdProof;
            objFinalDoc.Source = "Captured";

            objFinalDoc.Faceext = def1;
            objFinalDoc.Signature = ExtrctSign;
            ;
            string conn = _connectionString;
            using (SqlConnection connection2 = new SqlConnection(conn))
            {
                SqlCommand cmd2 = new SqlCommand("USP_AddDocuments", connection2);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                MemoryStream cmpStream = new MemoryStream();
                GZipStream hgs = new GZipStream(cmpStream, CompressionMode.Compress);
                hgs.Write(POAImage, 0, POAImage.Length);
                byte[] DocImg = cmpStream.ToArray();
                cmd2.Parameters.AddWithValue("@document", DocImg);
                //cmd2.Parameters.AddWithValue("@document", POAImage);
                cmd2.Parameters.AddWithValue("@docName", filetype);
                cmd2.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocCategoryCode);
                cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.CustomerDetailId);
                cmd2.Parameters.AddWithValue("@docMainType", objFinalDoc.DocMainType);
                cmd2.Parameters.AddWithValue("@createdBy", "");
                cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                cmd2.Parameters.AddWithValue("@documentCategory", Category);
                cmd2.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                cmd2.Parameters.AddWithValue("@Prediction", "");
                if (DocType == "DOCPOI")
                {
                    cmd2.Parameters.AddWithValue("@doctypecode", DocType);
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@doctypecodeforadd", DocType);
                }
                connection2.Open();
                cmd2.ExecuteNonQuery();
                connection2.Close();

            }
            using (SqlConnection connection2 = new SqlConnection(conn))
            {
                SqlCommand cmd2 = new SqlCommand("USP_AddDocumentsFace", connection2);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                if (img != null)
                {
                    cmd2.Parameters.AddWithValue("@document", img);
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@document", POAImage);
                }
                cmd2.Parameters.AddWithValue("@docName", filetype);
                cmd2.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocCategoryCode);
                cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.CustomerDetailId);
                cmd2.Parameters.AddWithValue("@docMainType", objFinalDoc.DocMainType);
                cmd2.Parameters.AddWithValue("@createdBy", "");
                cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                cmd2.Parameters.AddWithValue("@DocumentIdDate", null);
                cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                cmd2.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                cmd2.Parameters.AddWithValue("@Faceext", faceextract);
                cmd2.Parameters.AddWithValue("@documentCategory", Category);
                if (Category == "Pan Card" || Category == "Passport")
                {
                    cmd2.Parameters.AddWithValue("@Signature", signatureexract);
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@Signature", null);
                }
                connection2.Open();
                isInserted = cmd2.ExecuteNonQuery();
                connection2.Close();
            }
            return Json("Success" + "," + msg);
            //return Json("Success");
        }

        public ActionResult DOCImageCapture(string DocType, string base64DOC_POI, string base64DOC_POA, string base64DOC_Sign, string POItype, string IdProof)
        {
            try
            {
                string imagebase64 = base64DOC_Sign;

                long? isInserted;
                ClsDocumentDetails objdoc = new ClsDocumentDetails();
                ClsDocDetails objFinalDoc = new ClsDocDetails();
                //objFinalDoc.CustomerDetailId = (Convert.ToInt64(Session["PersonalId"]));
                objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));


                byte[] POIImage = null;
                byte[] POAImage = null;
                byte[] SignImage = null;
                byte[] CamSign = null;
                byte[] abc = null;
                int Unique = random.Next(1, 20);
                string ImgResult = "";
                //long perosnalid = Convert.ToInt64(Session["PersonalId"]);
                long perosnalid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                if (DocType == "DOCPOI")
                {
                    var Photo = base64DOC_POI.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                    byte[] bytes = Convert.FromBase64String(Photo);
                    var client = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentClassificationData");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/octet-stream");
                    request.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                    request.AddHeader("OrganisationCode", "ALP18980121");
                    request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                    request.AddParameter("application/octet-stream", bytes, RestSharp.ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    Console.WriteLine(response.Content);
                    //long? Result = objINDO_FinNet.USP_AIErrorDocumentType(Convert.ToInt64(Session["PersonalId"]), Convert.ToString(response.StatusCode), Convert.ToString(response.ResponseStatus));
                    string conn1 = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentType", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                        cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));


                        connection2.Open();
                        SqlDataReader reader2 = cmd2.ExecuteReader();
                        if (reader2.Read())
                        {
                            var ivar = reader2["result"].ToString();
                        }
                    }
                    Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                    if (response.StatusCode.ToString() == "OK")
                    {
                        var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(response.Content);

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
                                msg = "Voter id uploaded";
                                break;
                            case "4":
                                AadharMaskDetails.Add("DocumentType", "Driving licenses");
                                msg = "Driving licenses uploaded";
                                break;
                            case "5":
                                AadharMaskDetails.Add("DocumentType", "Pass port");
                                msg = "Pass port uploaded";
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
                        AadharMaskDetails.Add("StatusCode", obj.StatusCode);


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

                        var Photo1 = base64DOC_POI.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                        POIImage = Convert.FromBase64String(Photo1);
                        objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                        objFinalDoc.DocDetails = POIImage;
                        objFinalDoc.documentCategory = POItype;
                        objFinalDoc.DocName = "POICamImage";
                        extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                        objFinalDoc.DocType = 17;
                        objFinalDoc.DocMainType = "I";
                        objFinalDoc.DocCategoryCode = IdProof;
                        objFinalDoc.Source = "Upload";
                        
                        objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                        string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                        IMapper mapper = config.CreateMapper();
                        //Indo_FinNet_orgService.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, Indo_FinNet_orgService.ClsDocumentDetails>(objFinalDoc);
                        INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                        //isInserted = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments {(objResult)}");
                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_AddDocuments", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                            cmd2.Parameters.AddWithValue("@document", POIImage);
                            cmd2.Parameters.AddWithValue("@docName", "POICam");
                            cmd2.Parameters.AddWithValue("@documentCategoryCode", "");
                            cmd2.Parameters.AddWithValue("@docTypeId", "17");
                            cmd2.Parameters.AddWithValue("@docMainType", "I");
                            cmd2.Parameters.AddWithValue("@createdBy", "");
                            cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                            cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                            cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                            cmd2.Parameters.AddWithValue("@documentCategory", "CameraPhoto1");
                            cmd2.Parameters.AddWithValue("@Source", "Upload");

                            cmd2.Parameters.AddWithValue("@Prediction", "");

                            connection2.Open();
                            isInserted = cmd2.ExecuteNonQuery();
                            connection2.Close();
                        }
                        TempData["DocPOAByte"] = POIImage;
                        ImgResult = "Success";
                    }
                }
                else if (DocType == "DOCCamSign")
                {
                    var Photo = base64DOC_Sign.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                    byte[] bytes = Convert.FromBase64String(Photo);
                    var client = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentClassificationData");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/octet-stream");
                    request.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                    request.AddHeader("OrganisationCode", "ALP18980121");
                    request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                    request.AddParameter("application/octet-stream", bytes, RestSharp.ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

                    Console.WriteLine(response.Content);
                    //long? Result = objINDO_FinNet.USP_AIErrorDocumentType(Convert.ToInt64(Session["PersonalId"]), Convert.ToString(response.StatusCode), Convert.ToString(response.ResponseStatus));
                    string conn1 = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentType", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                        cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));


                        connection2.Open();
                        SqlDataReader reader2 = cmd2.ExecuteReader();
                        if (reader2.Read())
                        {
                            var ivar = reader2["result"].ToString();
                        }
                    }
                    Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                    if (response.StatusCode.ToString() == "OK")
                    {
                        var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(response.Content);

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
                                msg = "Voter id uploaded";
                                break;
                            case "4":
                                AadharMaskDetails.Add("DocumentType", "Driving licenses");
                                msg = "Driving licenses uploaded";
                                break;
                            case "5":
                                AadharMaskDetails.Add("DocumentType", "Pass port");
                                msg = "Pass port uploaded";
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
                        AadharMaskDetails.Add("StatusCode", obj.StatusCode);


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
                        CamSign = Convert.FromBase64String(Photo);
                        objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                        objFinalDoc.DocDetails = CamSign;
                        objFinalDoc.documentCategory = POItype;
                        objFinalDoc.DocName = "POACamImage";
                        extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                        objFinalDoc.DocType = 17;
                        objFinalDoc.DocMainType = "CA";
                        objFinalDoc.DocCategoryCode = IdProof;
                        objFinalDoc.Source = "Upload";

                        // objFinalDoc.DocumentId = objdoc.DocumentIdPOI;
                        objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                        string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                        var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                        IMapper mapper = config.CreateMapper();
                        //Indo_FinNet_orgService.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, Indo_FinNet_orgService.ClsDocumentDetails>(objFinalDoc);
                        INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                        //isInserted = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments {(objResult)}");
                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_AddDocuments", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                            cmd2.Parameters.AddWithValue("@document", CamSign);
                            cmd2.Parameters.AddWithValue("@docName", "CamSign");
                            cmd2.Parameters.AddWithValue("@documentCategoryCode", "");
                            cmd2.Parameters.AddWithValue("@docTypeId", "17");
                            cmd2.Parameters.AddWithValue("@docMainType", "SI");
                            cmd2.Parameters.AddWithValue("@createdBy", "");
                            cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                            cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                            cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                            cmd2.Parameters.AddWithValue("@documentCategory", "CameraPhoto3");
                            cmd2.Parameters.AddWithValue("@Source", "Upload");

                            cmd2.Parameters.AddWithValue("@Prediction", "");

                            connection2.Open();
                            isInserted = cmd2.ExecuteNonQuery();
                            connection2.Close();
                        }
                        //System.IO.File.WriteAllBytes(@"E:\YashodaPhoto.jpg", CustImg);
                        TempData["DocPOAByte"] = POIImage;
                        ImgResult = "Success";
                    }
                }
                else if (DocType == "DOCSign")
                {
                    var Photo = base64DOC_Sign.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                    SignImage = Convert.FromBase64String(Photo);
                    objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                    objFinalDoc.DocDetails = SignImage;
                    // objFinalDoc.documentCategory = POItype;
                    objFinalDoc.DocName = "SignCamImage";
                    extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                    objFinalDoc.DocType = 17;
                    objFinalDoc.DocMainType = "SI";
                    objFinalDoc.DocCategoryCode = IdProof;
                    objFinalDoc.documentCategory = "Signature";
                    objFinalDoc.Source = "Upload";

                    objFinalDoc.Faceext = abc;
                    objFinalDoc.Signature = SignImage;
                    // objFinalDoc.DocumentId = objdoc.DocumentIdPOI;
                    objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                    string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                    IMapper mapper = config.CreateMapper();
                    //Indo_FinNet_orgService.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, Indo_FinNet_orgService.ClsDocumentDetails>(objFinalDoc);
                    INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
        
                    string conn = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(conn))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_AddDocuments", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@document", SignImage);
                        cmd2.Parameters.AddWithValue("@docName", "SignCamImage");
                        cmd2.Parameters.AddWithValue("@documentCategoryCode", "");
                        cmd2.Parameters.AddWithValue("@docTypeId", "17");
                        cmd2.Parameters.AddWithValue("@docMainType", "SI");
                        cmd2.Parameters.AddWithValue("@createdBy", "");
                        cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                        cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                        cmd2.Parameters.AddWithValue("@documentCategory", "Signature");
                        cmd2.Parameters.AddWithValue("@Source", "Upload");

                        cmd2.Parameters.AddWithValue("@Prediction", "");

                        connection2.Open();
                        isInserted = cmd2.ExecuteNonQuery();
                        connection2.Close();
                    }
                    TempData["DocPOAByte"] = POIImage;
                    ImgResult = "Success";
                }
                return Json("Success");
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return Json("Exception");
            }

        }
        #endregion

        [HttpGet]
        public ActionResult CustomerDocumentDetails([FromServices] IActiveLogin objLogin, string proceedwithOCR)
        {
            ClsDocDetails objFinalDoc = new ClsDocDetails();
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
            var AdminID = HttpContext.Session.GetString("UserID");
            if (AdminID != null)
            {
                ViewBag.AdminFlag = "AdminFlag";
            }
            else
            { }
            string s=HttpContext.Session.GetString("HideRB");
            if(s!=null)
            {
                ViewBag.HRButton = "True";
            }          
            try
            {
                var rekycview = HttpContext.Session.GetString("rekycVIEW");
                if (rekycview != null)
                {
                    ViewBag.rekycview = rekycview;
                }
                var fs = HttpContext.Session.GetString("PersonalId");
                long CustId = Convert.ToInt64(HttpContext.Session.GetString(("PersonalId")));
                var userid = HttpContext.Session.GetString("UserID");
                var REKYCS = HttpContext.Session.GetString("REKYCQ");
                var progressbar = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))}").AsEnumerable().FirstOrDefault();
                if (progressbar.IsQuickEnrollSubmit == true && progressbar.IsCAFSubmit == true)
                {
                    ViewBag.progressbarqc = 1;
                }
                else { }
                if (userid != null)
                {
                    ViewBag.userid=userid;
                }
                if (REKYCS != null)
                {
                    ViewBag.REKYCQ = REKYCS;
                }
                var result = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetDocName {CustId}").AsEnumerable().FirstOrDefault();
                if (result != null)
                {
                    ViewBag.DocPOI = result.DocumentCategoryCode;
                }
                var result1 = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetDocAddType {CustId}").AsEnumerable().FirstOrDefault();
                if (result1 != null)
                {
                    ViewBag.DocPOA = result1.DocumentCategoryCode;
                }
                var resul1 = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetCustomerDetails1 {CustId}").AsEnumerable().FirstOrDefault();
                if (resul1 != null)
                {
                    string imgs = Convert.ToBase64String(resul1.DocumentHistory);
                    ViewBag.liveimage = (string.Format("data:image/jpg;base64,{0}", imgs));
                    ViewBag.Admin = "DocumentFlag";
                }
                if (HttpContext.Session.GetString("RekycImg") == "true")
                {
                    var result3 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_RekycAimage {CustId}").AsEnumerable().FirstOrDefault();
                    var result12 = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_RekycAxmlimage {CustId}").AsEnumerable().FirstOrDefault();
                    if (result3 != null)
                    {
                        string imgs1 = Convert.ToBase64String(result3.Photo);
                        ViewBag.liveimage = (string.Format("data:image/jpg;base64,{0}", imgs1));
                        ViewBag.rekycrekyc = "DocumentFlag";
                    }
                    else if (result12 != null)
                    {
                        string imgs2 = (result12.AadharPhoto);
                        ViewBag.liveimage = imgs2;// (string.Format("data:image/jpg;base64,{0}", imgs2));
                        ViewBag.rekycrekyc = "DocumentFlag";
                    }
                }
                string conn = _connectionString;
                using (SqlConnection connection3 = new SqlConnection(conn))
                {
                    SqlCommand cmd3 = new SqlCommand("USP_AdminRKYCDetails", connection3);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@CustId", CustId);
                    connection3.Open();
                    SqlDataReader reader3 = cmd3.ExecuteReader();
                    if (reader3.Read())
                    {
                        var Result = reader3[56].ToString();
                        HttpContext.Session.SetString("rekycforDoc", Result);
                    }
                    var Rekyc = HttpContext.Session.GetString("rekycforDoc");
                    if (Rekyc == "True")
                    {
                        var result12 = objDetails.AdmCustomerAadharDetails.FromSqlRaw($"USP_RekycAxmlimage {CustId}").AsEnumerable().FirstOrDefault();
                        var result4 = objDetails.AdmDigiAadharData.FromSqlRaw($"USP_RekycAimage {CustId}").AsEnumerable().FirstOrDefault();
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
                    }
                }
                ViewBag.idDoc = null;
                ViewBag.corAdd = null;
                ViewBag.perAdd = null;
                ViewBag.photo = null;
                ViewBag.poi = "Y";
                ViewBag.popa = "Y";
                ViewBag.poca = "Y";
                ViewBag.sa = null;
                ViewBag.isEdited = false;
                ViewBag.service = null;
                ClsDocumentDetails objDoc = new ClsDocumentDetails();
                objDoc.proceedwithOCR = HttpContext.Session.GetString("proceedwithOCR");
                objDoc.shareAadharNumber = HttpContext.Session.GetString("shareAadharNumber");
                ViewBag.shareAadharNumber = objDoc.shareAadharNumber;
                objDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                TempData["CustomerDetailId"] = objDoc.CustomerDetailId;
                string CustDetails = TempData["CustomerDetailId"].ToString();
                ViewBag.CustomerDetailId = objDoc.CustomerDetailId;
                ViewBag.personalid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string POI_flag="I";
                var verificationtype = (from details in objDetails.AdmCustomerDocumentCategories.FromSqlRaw($"USP_GetPOIDocumentByCategory_NEW").ToList()
                                        select new SelectListItem()
                                        {
                                            Text = details.DocumentCategoryDescription.ToString(),
                                            Value = details.DocumentCategoryDescription,
                                        }).ToList();
                verificationtype.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.proofOfId = verificationtype;
                var verificationtype2 = (from details in objDetails.AdmCustomerDocumentCategories.FromSqlRaw($"USP_GetPOADocumentByCategory_NEW").ToList()
                                         select new SelectListItem()
                                         {
                                             Text = details.DocumentCategory.ToString(),
                                             Value = details.DocumentCategoryDescription,
                                         }).ToList();
                verificationtype2.Insert(0, new SelectListItem()
                {
                    Text = "----Select----",
                    Value = string.Empty
                });
                ViewBag.proofOfCorrAdd = verificationtype2;
                return View(objDoc);
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
                string URL = System.Configuration.ConfigurationSettings.AppSettings["https://apigateway.indofinnet.com/api/GeoLocation?latitude=latcode&Longitude=loncode&OrgID=Alpha01"];
                URL += "?latitude=" + latitude + "&Longitude=" + Longitude + "&OrgID=" + OrgID;
                var client = new RestClient("https://apigateway.indofinnet.com/api/GeoLocation?latitude=18.4633363&Longitude=73.8247495&OrgID=Alpha01");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                return Json(response.Content);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("NEW Errors");
            }
        }

        [HttpGet]
        public ActionResult deleteDocuments(long? docId)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_DeleteDocuments", connection);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerDocumentDetails([FromServices] IActiveLogin objLogin, ClsDocumentDetails objdoc)
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
                if (HttpContext.Session.GetString("RekycImg") == "true")
                {
                    string conn = _connectionString;
                    using (SqlConnection connection3 = new SqlConnection(conn))
                    {
                        SqlCommand cmd3 = new SqlCommand("USP_RKYCDocFlag", connection3);
                        cmd3.CommandType = CommandType.StoredProcedure;
                        cmd3.Parameters.AddWithValue("@CustId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                        connection3.Open();
                        SqlDataReader reader3 = cmd3.ExecuteReader();
                        if (reader3.Read())
                        {
                            //var Result = reader2["RESULT"].ToString();
                        }
                    }
                    return RedirectToAction("SummerySheetDetails", "DataVerify");
                }
                else
                {
                    long? isInserted;
                    ClsDocDetails objFinalDoc = new ClsDocDetails();
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
                        var Res = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))},{("IAPVD")}", conn).ToList();
                        ViewBag.POI = Res;
                    }
                    conn = _connectionString;
                    var Ress = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))},{("CADL")}", conn).ToList();
                    ViewBag.POA = Ress;
                    var Sign = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(Convert.ToInt64(HttpContext.Session.GetString("PersonalId")))},{("SI")}", conn).ToList();
                    ViewBag.Signature = Sign;
                    if (ViewBag.POI.Count == 0)
                    {
                        ViewBag.msg = "Please Upload POI";
                        return RedirectToAction("CustomerDocumentDetails", "DocumentDetails");
                    }
                    else if (ViewBag.POA.Count == 0)
                    {
                        return RedirectToAction("CustomerDocumentDetails", "DocumentDetails");
                    }
                    else if (ViewBag.Signature.Count == 0)
                    {
                        return RedirectToAction("CustomerDocumentDetails", "DocumentDetails");
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
                    return RedirectToAction("CustIPVdetails", "NEWIPVDetails");
                }
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return Json("Exception");
            }
        }

        public ActionResult GetDocumentsForGridView(string DocMainType, int p = 1)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                long perosnalid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string conn = _connectionString;
                if (DocMainType == "SI")
                {
                    var Res12 = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(perosnalid)},{DocMainType}").ToList();
                    ViewBag.Message = Res12;

                }
                else
                {

                    var Res = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(perosnalid)},{DocMainType}").ToList();
                    foreach (var products in Res)
                    {
                        if (products.doctypecode == "DOCPOI")
                        {
                            var Res1 = Res.Where(products => products.doctypecode == "DOCPOI").ToList();

                            ViewBag.Message = Res1;
                            break;
                        }
                        

                    }

                }

                //string idcust = "";
                //foreach(var products in Res)
                //    {
                //    idcust= products.DocumentId;
                //    }
                //ViewBag.idcust = idcust;
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.ToString());
            }
        }
        public ActionResult GetDocumentsForGridView1(string DocMainType, int p = 1)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                long perosnalid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string conn = _connectionString;
                var Res = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(perosnalid)},{DocMainType}").ToList();

                foreach (var products in Res)
                {
                    if (products.doctypecodeforadd == "DOCPOA")
                    {
                        var Res1 = Res.Where(products => products.doctypecodeforadd == "DOCPOA").ToList();

                        ViewBag.Message = Res1;

                        break;
                    }

                }

                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex.ToString());
            }
        }

        public ActionResult CropDocument()
        {
            long personlids = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
            string docmaintype = HttpContext.Session.GetString("doc");
            SqlConnection con = new SqlConnection(_connectionString);
            SqlCommand cmd2 = new SqlCommand("getimagedata", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@docid", personlids);
            cmd2.Parameters.AddWithValue("@documenttype", docmaintype);
            con.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();
            byte[] ivar = null;
            if (reader2.Read())
            {
                ivar = (byte[])reader2["documentHistory"];
                String Dim = Convert.ToBase64String(ivar);
                String imgDataURL = String.Format("data:image/png;base64,{0}", Dim);
                ViewBag.ImageData = imgDataURL;
                HttpContext.Session.SetString("image", Convert.ToString(imgDataURL));
            }
            return View();
        }
        [HttpPost]
        public ActionResult CropDocument(IFormFile file)
        {
            ErrorLog error_log = new ErrorLog();
            long personlid1 = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
            string docmaintype1 = HttpContext.Session.GetString("doc");
            string upload = "";
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                string base64 = Request.Form["imgCropped"];
                byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);
                SqlCommand cmd2 = new SqlCommand("USP_cropImage2", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@document", bytes);
                cmd2.Parameters.AddWithValue("@docid", personlid1);
                cmd2.Parameters.AddWithValue("@documenttype", docmaintype1);
                con.Open();
                cmd2.CommandTimeout = 3600;
                int result = cmd2.ExecuteNonQuery();
                con.Close();
                if (result > 0)
                {
                    upload = "Document Cropped successfully ";
                }
                return RedirectToAction("CustomerDocumentDetails");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(new { Message = "ERROR" });
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
                Bitmap image = (Bitmap)tc.ConvertFrom(bytes);                 
                var ocr = new Tesseract();
                ocr.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz/"); 
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
                                                var result12 = objData.AdmIndoErrorLogs.FromSqlRaw ($"USP_IndoErrorLogs({"D:imagesforaadhar"}, {"blackening_POI"}, {"DocumentDetails"}");
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
                                                            isInserted = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments{(objResult)}");
                                                            if (isInserted > 0)
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
                                                            objDetails.AdmCustomerDocuments.FromSqlRaw ($"USP_GetDocumentType {(objFinalDoc.DocType)}");
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
                                                            isInserted = objDetails.Database.ExecuteSqlRaw ($"USP_AddDocuments{ (objResult1)}");
                                                            if (isInserted > 0)
                                                            {
                                                                var result1 = objData.AdmIndoErrorLogs.FromSqlRaw ($"USP_IndoErrorLogs ({"68"}, {"blackening_POA"}, {"DocumentDetails"}");
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
        #region CROPPING AND MASKING
        [HttpPost]

        public ActionResult ToSavePdf(string CustomerDetailsId)
        {
            IFormFile files = (IFormFile)Request.Form.Files;

            string ImagePath = null;
            ViewBag.CustomerDetailsId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

            for (int i = 0; i < files.Length; i++)
            {
                IFormFile file = files;
                string fname1 = System.IO.Path.GetFileName(file.FileName);
                if (fname1.Contains(".jpg"))
                {
                    string fileName = fname1;
                    int fileExtPos = fileName.LastIndexOf(".");
                    if (fileExtPos >= 0)
                        fileName = fileName.Substring(0, fileExtPos);
                    string updatedFname = fileName + ViewBag.CustomerDetailsId + "_Uploaded.jpg";

                    //file.SaveAs((System.IO.Path.Combine("~/CustDocumentIndoFin/", updatedFname)));
                    //   string path = ("\\") + "CustDocumentIndoFin\\" + CustomerDetailsId + "_Uploaded";
                    ImagePath = "/CustDocumentIndoFin/" + updatedFname;

                    // Image image1 = Image.FromFile(ImagePath);
                    System.Drawing.Image image1 = System.Drawing.Image.FromFile((ImagePath));
                    Size size = image1.Size;
                    FileInfo fi = new FileInfo((ImagePath));
                    float docsize = (float)(fi.Length) / 1024;

                    if (docsize > 150)
                    {
                        CompressImage(image1, ImagePath);
                    }
                    else
                        if ((docsize < 150 & size.Width > 400) || (docsize < 150 & size.Height > 500))
                    {
                        System.Drawing.Image image = resizeImage(image1, new Size(400, 500));
                        image.Save(("/CustDocumentIndoFin/" + "resize.jpeg"));
                        image1.Dispose();
                        image.Dispose();
                        System.IO.File.Delete((ImagePath));
                        System.IO.File.Move(("/CustDocumentIndoFin/" + "resize.jpeg"), (ImagePath));
                    }

                }
                if (fname1.Contains(".jpeg"))
                {
                    string fileName = fname1;
                    int fileExtPos = fileName.LastIndexOf(".");
                    if (fileExtPos >= 0)
                        fileName = fileName.Substring(0, fileExtPos);
                    string updatedFname = fileName + ViewBag.CustomerDetailsId + "_Uploaded.jpeg";

                    //file.SaveAs((System.IO.Path.Combine("~/CustDocumentIndoFin/", updatedFname)));
                    //   string path = ("\\") + "CustDocumentIndoFin\\" + CustomerDetailsId + "_Uploaded";
                    ImagePath = "/CustDocumentIndoFin/" + updatedFname;

                    // Image image1 = Image.FromFile(ImagePath);
                    System.Drawing.Image image1 = System.Drawing.Image.FromFile((ImagePath));
                    Size size = image1.Size;
                    FileInfo fi = new FileInfo((ImagePath));
                    float docsize = (float)(fi.Length) / 1024;

                    if (docsize > 150)
                    {
                        CompressImage(image1, ImagePath);
                    }
                    else
                        if ((docsize < 150 & size.Width > 400) || (docsize < 150 & size.Height > 500))
                    {
                        System.Drawing.Image image = resizeImage(image1, new Size(400, 500));
                        image.Save(("/CustDocumentIndoFin/" + "resize.jpeg"));
                        image1.Dispose();
                        image.Dispose();
                        System.IO.File.Delete((ImagePath));
                        System.IO.File.Move(("/CustDocumentIndoFin/" + "resize.jpeg"), (ImagePath));
                    }

                }
                if (fname1.Contains(".pdf"))
                {
                    try
                    {
                        string fileName = fname1;
                        int fileExtPos = fileName.LastIndexOf(".");
                        if (fileExtPos >= 0)
                            fileName = fileName.Substring(0, fileExtPos);
                        string updatedFname = fileName + ViewBag.CustomerDetailsId + "_Uploaded.pdf";

                        //file.SaveAs((System.IO.Path.Combine("~/CustDocumentIndoFin/", updatedFname)));
                        // string path = ("\\") + "CustDocumentIndoFin\\" + CustomerDetailsId + "_Uploaded";
                        string pathhh = AppDomain.CurrentDomain.BaseDirectory + "CustDocumentIndoFin/" + updatedFname;
                        string aa = System.IO.Path.Combine("~/CustDocumentIndoFin/", updatedFname);
                        ImagePath = "/CustDocumentIndoFin/" + updatedFname;
                        PdfReader reader = new PdfReader(pathhh);

                        // PdfReader reader = new PdfReader(@"D:\AratiWorking2020\INDOFinNet_Project\INDOFinNet_Project\ServiceProvider1\CustDocumentIndoFin\" + updatedFname);
                        reader.Close();              //Clear
                        if (reader.GetPageSize(1).Height != 900 || reader.GetPageSize(1).Width != 750)
                        {
                            ResizepdfusingItext(updatedFname);

                        }
                    }
                    catch (Exception Ex)
                    {

                        PortalException.InsertPortalException(Ex);
                        return Json("Exception");//, JsonRequestBehavior.AllowGet
                    }
                }

            }
            // return Json(ImagePath);
            return Json(ImagePath + "?" + DateTime.Now.Ticks.ToString());//, JsonRequestBehavior.AllowGet


        }
        // after click particular save poi
        [HttpPost]
        public JsonResult SaveDoc(string IMGDATA, string ddl_idProof, long? customerdetailid, string imagePath, string DocMainType, string DocType)
        {
            try
            {
                long? isInserted;
                ClsDocumentDetails objdoc = new ClsDocumentDetails();
                ClsDocDetails objFinalDoc = new ClsDocDetails();
                objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));

                byte[] bytesImage = null;
                string ProofOfIdCode = ddl_idProof;

                if (imagePath != "")
                {
                    bytesImage = System.IO.File.ReadAllBytes((imagePath));
                }
                else
                {
                    var t = IMGDATA.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                    byte[] bytesImage1 = Convert.FromBase64String(t);
                    System.Drawing.Image image;
                    using (MemoryStream ms = new MemoryStream(bytesImage1))
                    {
                        image = System.Drawing.Image.FromStream(ms);
                    }
                    bytesImage1 = ImageToByteArray(image);
                    objFinalDoc.DocDetails = bytesImage1;
                    string documentName;
                    documentName = objFinalDoc.CustomerDetailId + "_" + ddl_idProof + ".jpeg";
                    if (objdoc.DocumentIdDatePOA != null)
                    {
                        objFinalDoc.DocumentIdDate1 = DateTime.ParseExact(objdoc.DocumentIdDatePOA, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        objFinalDoc.DocumentIdDate1 = null;
                    }
                    objFinalDoc.DocName = documentName;


                    extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                    //objFinalDoc.DocType =
                     objDetails.SysDocumentTypes.FromSqlRaw ($"USP_GetDocumentType {("." + extension)}");
                    objFinalDoc.DocMainType = DocMainType;
                    objFinalDoc.documentCategory = DocType;
                    objFinalDoc.DocCategoryCode = ProofOfIdCode;

                    // objFinalDoc.DocumentId = objdoc.DocumentIdPOI;
                    objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                    string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                    IMapper mapper = config.CreateMapper();
                    INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                    isInserted = objDetails.Database.ExecuteSqlRaw ($"USP_AddDocuments {(objResult)}");
                }

                return Json("Successfully saved");//, JsonRequestBehavior.AllowGet
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }
        }
        public byte[] ImageToByteArray(System.Drawing.Image image)
        {

            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }


        }
        [HttpPost]
        public JsonResult SaveCroppedDoc(string IMGDATA, string ddl_idProof, long? CustomerDetailId, string imagePath, string DocMainType, string id, ClsDocumentDetails objdoc)
        {
            try
            {
                long? isInserted;
                ClsDocDetails objFinalDoc = new ClsDocDetails();
                objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));

                byte[] bytesImage = null;
                string ProofOfIdCode = ddl_idProof;
                if (imagePath != "")
                {
                    bytesImage = System.IO.File.ReadAllBytes((imagePath));
                }
                else
                {
                    var t = IMGDATA.Split(new string[] { "," }, StringSplitOptions.None)[1];  // remove data:image/png;base64,
                    byte[] bytesImage1 = Convert.FromBase64String(t);

                    //string ImgPath = Convert.ToString(Request.Form[0]);
                    string ImgPath = Convert.ToString(HttpContext.Session.GetString("ImgPath"));
                    //string path3 = ImgPath.Substring(33);
                    string ImagePathNew = "D://IndoFinCroppedImages/" + objFinalDoc.CustomerDetailId + "_uploadedimage.jpg";
                    //System.IO.File.WriteAllBytes(ImagePathNew, bytesImage1);
                    ///for saving cropped image in DB 
                    MemoryStream ms = new MemoryStream(bytesImage1, 0, bytesImage1.Length);
                    ms.Write(bytesImage1, 0, bytesImage1.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    string pathSaveimg = "/Uploads/" + objFinalDoc.CustomerDetailId + "_uploadedimage.jpg";
                    //image.Save((pathSaveimg), ImageFormat.Jpeg);
                    //string path4 = "/Uploads/" + objFinalDoc.CustomerDetailId+ "/" + path3;
                    image.Save((pathSaveimg), ImageFormat.Jpeg);
                    FileInfo fi = new FileInfo((pathSaveimg));
                    bytesImage = System.IO.File.ReadAllBytes((pathSaveimg));
                    // image.Save(("~/CustDocumentIndoFin/temp.jpeg"), ImageFormat.Jpeg);
                    // FileInfo fi = new FileInfo(("~/CustDocumentIndoFin/temp.jpeg"));
                    //bytesImage = System.IO.File.ReadAllBytes(("~/CustDocumentIndoFin/temp.jpeg"));
                    float docsize = (float)(fi.Length) / 1024;
                    objFinalDoc.DocDetails = bytesImage;
                    string documentName;
                    documentName = objFinalDoc.CustomerDetailId + "_" + ddl_idProof + ".jpeg";
                    //}
                    if (objdoc.DocumentIdDatePOA != null)
                    {
                        //objFinalDoc.DocumentIdDate1 = Convert.ToDateTime(objdoc.DocumentIdDatePOA);
                        objFinalDoc.DocumentIdDate1 = DateTime.ParseExact(objdoc.DocumentIdDatePOA, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        objFinalDoc.DocumentIdDate1 = null;
                    }
                    objFinalDoc.DocName = documentName;
                    extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                    objDetails.SysDocumentTypes.FromSqlRaw($"USP_GetDocumentType {("." + extension)}");
                    objFinalDoc.DocMainType = DocMainType;
                    objFinalDoc.DocCategoryCode = ProofOfIdCode;
                    // objFinalDoc.DocumentId = objdoc.DocumentIdPOI;
                    objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                    string DocumentsDetails = JsonConvert.SerializeObject(objFinalDoc);
                    long? documentid = Convert.ToInt64(id);
                    var config = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(); });
                    IMapper mapper = config.CreateMapper();
                    INDO_FIN_NET.Models.ClsDocumentDetails objResult = mapper.Map<ClsDocDetails, INDO_FIN_NET.Models.ClsDocumentDetails>(objFinalDoc);
                    isInserted = objDetails.Database.ExecuteSqlRaw($"USP_UpdateDocumentById {(Convert.ToInt64(objFinalDoc.DocumentId))}, {objFinalDoc.DocMainType}, {documentid}, {bytesImage1}");   //USP_UpdateDocumentById(objFinalDoc.DocumentId,objFinalDoc.DocMainType, documentid,bytesImage1);
                }


                return Json("Cropped Successfully");//, JsonRequestBehavior.AllowGet
            }
            catch (Exception ex)
            {
                return Json(ex.Message);//, JsonRequestBehavior.AllowGet
            }
        }

        #region Manual Aadhaar Masking
        public ActionResult Tomask(string CordinateX, string CordinateY, string CordinateW, string CordinateH, string CustomerDetailsId, string ddl_idProof)
        {
            // ClsDocDetails objFinalDoc = new ClsDocDetails();
            string customerDetailsId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
            string Doctype = null;
            byte[] bytes;
            //  bytes = System.IO.File.ReadAllBytes(("\\") + "IndoFinCroppedImages\\"+ CustomerDetailsId + "_uploadedimage.jpg");
            bytes = System.IO.File.ReadAllBytes("D://IndoFinCroppedImages/" + customerDetailsId + "_uploadedimage.jpg");
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes))
            {
                Bitmap bmp = new Bitmap(ms);


                Graphics gr = Graphics.FromImage(bmp);
                float top = Convert.ToInt64(CordinateX);
                float left = Convert.ToInt64(CordinateY);
                float height = Convert.ToInt64(CordinateH);
                float width = Convert.ToInt64(CordinateW);
                //int count = (width * 2) + (width / 4);
                System.Drawing.Rectangle rect = new System.Drawing.Rectangle(Convert.ToInt32(top), Convert.ToInt32(left), Convert.ToInt32(width), Convert.ToInt32(height));

                gr.DrawImage(Bitmap.FromFile(("\\") + "BlackImage\\" + "black.jpg"), rect);
                bmp.Clone();

                bmp.Save(("~/CustDocumentIndoFin/temp.jpeg"), ImageFormat.Jpeg);
                FileInfo fi = new FileInfo(("~/CustDocumentIndoFin/temp.jpeg"));
                byte[] bytesImageNew = System.IO.File.ReadAllBytes(("~/CustDocumentIndoFin/temp.jpeg"));
                string maskedbyteImage = Convert.ToBase64String(bytesImageNew);


                string imagedata = string.Format("data:image/jpg;base64,{0}", maskedbyteImage);

                string msg = "Image Masked successfully";
                //  System.IO.File.Delete(("\\") + "CustDocumentIndoFin\\" + CustomerDetailsId + "_uploadedimage");
                SaveDoc(imagedata, ddl_idProof, Convert.ToInt64(customerDetailsId), "", "", Doctype);
                return Json(msg);//, JsonRequestBehavior.AllowGet

            }
            return View();
        }
        #endregion
        public void ResizepdfusingItext(String pdfFilename)
        {
            try
            {
                string original = ("~/Uploads/" + pdfFilename);
                string outPDF = ("~/Uploads/ResizePdf" + pdfFilename);
                PdfReader pdfr = new PdfReader(original);

                Document doc = new Document(PageSize.A3, 0, 0, 0, 0);

                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(outPDF, FileMode.Create));
                doc.Open();

                PdfContentByte cb = writer.DirectContent;

                PdfImportedPage page;

                for (int i = 1; i < pdfr.NumberOfPages + 1; i++)
                {
                    page = writer.GetImportedPage(pdfr, i);
                    iTextSharp.text.Image instance = iTextSharp.text.Image.GetInstance(page);
                    doc.Add(instance);
                    // cb.Add(page);
                    // cb.AddTemplate(page, PageSize.LETTER.Width / pdfr.GetPageSize(i).Width, 0, 0, PageSize.LETTER.Height / pdfr.GetPageSize(i).Height, 0, 0);
                    doc.NewPage();
                }
                //pdfr.Close();
                //pdfr.Dispose();
                doc.Close();
                doc.Dispose();
                pdfr.Dispose();
                pdfr.Close();
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                //writer.Close();

                //  System.IO.File.Delete(("~/CustDocumentIndoFin/" + pdfFilename ));

                //just renaming, conversion / resize process ends at doc.close() above
                System.IO.File.Delete(original);
                System.IO.File.Copy(outPDF, original);
                System.IO.File.Delete(outPDF);

            }
            catch (Exception Ex)
            {

                PortalException.InsertPortalException(Ex);
                // return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        private void CompressImage(System.Drawing.Image image1, string ImagePath)
        {
            System.Drawing.Image image = resizeImage(image1, new Size(400, 500));

            image1.Dispose();

            string file_name = (ImagePath);
            //Image image1 = Image.FromFile(image);
            long compression = 100;
            try
            {
                EncoderParameters encoder_params = new EncoderParameters(1);
                encoder_params.Param[0] = new EncoderParameter(
                    System.Drawing.Imaging.Encoder.Quality, compression);

                ImageCodecInfo image_codec_info = GetEncoderInfo("image/jpeg");
                // 
                //System.IO.File.Delete(file_name);
                string foldername = "/CustDocumentIndoFin/" + "111.jpeg";
                //image.Save(@"E:\AdityaBirlaFinance_WebPortal\CNSBWeb\CNSBWebPortal\CustDocument\1112.jpg", image_codec_info, encoder_params);
                image.Save((foldername), image_codec_info, encoder_params);
                image.Dispose();
                System.IO.File.Delete(file_name);
                System.IO.File.Move((foldername), file_name);

            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error saving file '" + file_name +
                //    "'\nTry a different file name.\n" + ex.Message,
                //    "Save Error", MessageBoxButtons.OK,
                //    MessageBoxIcon.Error);
            }
        }
        public System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
        {

            //Get the image current width

            int sourceWidth = imgToResize.Width;

            //Get the image current height

            int sourceHeight = imgToResize.Height;



            float nPercent = 0;

            float nPercentW = 0;

            float nPercentH = 0;

            //Calulate  width with new desired size

            nPercentW = ((float)size.Width / (float)sourceWidth);

            //Calculate height with new desired size

            nPercentH = ((float)size.Height / (float)sourceHeight);



            if (nPercentH < nPercentW)

                nPercent = nPercentH;

            else

                nPercent = nPercentW;

            //New Width

            int destWidth = (int)(sourceWidth * nPercent);

            //New Height

            int destHeight = (int)(sourceHeight * nPercent);



            Bitmap b = new Bitmap(destWidth, destHeight);

            Graphics g = Graphics.FromImage((System.Drawing.Image)b);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw image with new width and height

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);

            g.Dispose();

            return (System.Drawing.Image)b;


        }
        private ImageCodecInfo GetEncoderInfo(string mime_type)
        {

            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i <= encoders.Length; i++)
            {
                if (encoders[i].MimeType == mime_type) return encoders[i];
            }
            return null;


        }


        #endregion
        #region ViewDoc
        public ActionResult ViewUploadedDocument(string Id)
        {
            try
            {
                var objDocResult = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw ($"USP_Indo_GetSingleDocumentBydocId {(Convert.ToInt64(Id))}").AsEnumerable().FirstOrDefault();
                if (objDocResult != null)
                {
                    if (objDocResult.DocumentHistory != null)
                    {
                        byte[] contentresult = objDocResult.DocumentHistory;
                        string[] fileDetails = objDocResult.DocumentName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                        //string extn = objDocResult.documentType;
                        // Response.AppendHeader("Content-Disposition", "Inline; filename=" + fileDetails[0] + "." + extn + ";");
                        return File(contentresult, objDocResult.DocumentName);
                    }
                    else
                    {
                        TempData["msg"] = "Invalid Data Found";
                    }
                }
                else
                {
                    TempData["msg"] = "Invalid Data Found";
                }
                return RedirectToAction("TabsPage", "CustomerTabPage");
            }
            catch (Exception ex)
            {
                var result = objData.Database.ExecuteSqlRaw ($"USP_IndoErrorLogs {(ex.StackTrace)}, {"CustomerDocumentDetails"}, {"DocumentDetails"}");
                TempData["msg"] = ex.Message;
                return RedirectToAction("TabsPage", "CustomerTabPage");
            }
        }
        #endregion

        #region Delete Document
        //public ActionResult deleteDocument(long? docId)
        //{
        //    //if (!clsOrgUserAuthorize.IsAuthorizeOrgCustDetail(Convert.ToInt64(ObjTripleDes.Decrypt(Session["OrgUserId"].ToString())), Session["SessionOrgKey"].ToString()))
        //    //    return RedirectToAction("OrganisationDetails", "OrganisationLogin");
        //    try
        //    {
        //        //string str = "s";
        //        var str = objData.Database.ExecuteSqlRaw ($"USP_DeleteDocument {(docId)}");
        //        return Json(str);//, JsonRequestBehavior.AllowGet
        //    }
        //    catch (Exception ex)
        //    {
        //        //var spobj = ObjDmatService.USP_DEMAT_Error(0,ex.Message, "deleteDocument", "DematDocumentsDetailsController").ToList();
        //        return Json(ex.ToString());//, JsonRequestBehavior.AllowGet
        //    }

        //}
        #endregion

        public ActionResult DocExtratcion(string ddl_idProof, string DocMainType)
        {
            ErrorLog error_log = new ErrorLog();
            string msg1 = "";
            try
            {
                IFormFile files = Request.Form.Files[0];
                ClsDocDetails objFinalDoc = new ClsDocDetails();
                objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                ClsDocumentDetails objdoc = new ClsDocumentDetails();
                string POItype = Convert.ToString(Request.Form.Files[0]);
                byte[] abcd = null;
                byte[] xyz = null;
                byte[] def = null;
                byte[] def1 = null;
                byte[] mno = null;
                byte[] eibytes = null;
                string ProofOfIdCode = ddl_idProof;
                string fnamee;
                long perosnalid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                string documentName;
                documentName = objFinalDoc.CustomerDetailId + "_" + ddl_idProof + ".jpeg";
                if (ddl_idProof == "69" || ddl_idProof == "70")
                {
                    ViewBag.personalid = perosnalid;
                    IFormFile file = Request.Form.Files[0];
                    string pathString = Path.Combine(@"/Uploads/");
                    fnamee = perosnalid + ".pdf";
                    string str = "CropPdf-" + fnamee;
                    ViewBag.Filename = fnamee;
                    fnamee = System.IO.Path.Combine(pathString, fnamee);
                    return Json(str);
                }
                else
                {
                    IFormFile file = Request.Form.Files[0];
                    string fname;
                    int j = 0;/*= i + 1;*/
                    ViewBag.personalid = perosnalid;
                    //string pathString = Path.GetFullPath("/Uploads/");
                    var pathString = Path.Combine(Directory.GetCurrentDirectory(), "Uploads\\");
                    string pathcombine = Path.Combine(pathString, perosnalid.ToString());
                    if (file.FileName.Contains(".jpg"))
                    {
                        //var client1 = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DataExtraction");
                        BinaryReader reader = new BinaryReader(file.OpenReadStream());
                        eibytes = reader.ReadBytes((int)file.Length);
                        var client = new RestClient("https://apigateway.indofinnet.com/api/DataExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        request.AddHeader("Content-Type", "image/jpeg");
                        request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentExtarction", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                            cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));
                            connection2.Open();
                        }
                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                        if (response.StatusCode.ToString() == "OK")
                        {
                            var obj = JsonConvert.DeserializeObject<DocumentExtractionAPI>(JsonConvert.DeserializeObject<string>(response.Content));
                            AadharMaskDetails1.Add("StatusCode", obj.StatusCode);
                            AadharMaskDetails1.Add("Card_Name", obj.Card_Name);
                            AadharMaskDetails1.Add("customer_document_id", obj.customer_document_id);
                            AadharMaskDetails1.Add("customer_full_name", obj.customer_full_name);
                            AadharMaskDetails1.Add("customer_dob", obj.customer_dob);
                            AadharMaskDetails1.Add("customer_gender", obj.customer_gender);
                            AadharMaskDetails1.Add("customer_relation_type", obj.customer_relation_type);
                            AadharMaskDetails1.Add("customer_name_initial", obj.customer_name_initial);
                            AadharMaskDetails1.Add("customer_fname", obj.customer_fname);
                            AadharMaskDetails1.Add("customer_mname", obj.customer_mname);
                            AadharMaskDetails1.Add("customer_lname", obj.customer_lname);
                            switch (obj.StatusCode)
                            {
                                case "200":
                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                    string conn1 = _connectionString;
                                    using (SqlConnection connection2 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd2 = new SqlCommand("USP_InsertAIDocumentExtraction", connection2);
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@Customerid", perosnalid);
                                        cmd2.Parameters.AddWithValue("@StatusCode", obj.StatusCode);
                                        cmd2.Parameters.AddWithValue("@CardName", obj.Card_Name);
                                        cmd2.Parameters.AddWithValue("@Documentid", obj.customer_document_id);
                                        cmd2.Parameters.AddWithValue("@fullname", obj.customer_full_name);
                                        cmd2.Parameters.AddWithValue("@dob", obj.customer_dob);
                                        cmd2.Parameters.AddWithValue("@gender", obj.customer_gender);
                                        cmd2.Parameters.AddWithValue("@relationtype", obj.customer_relation_type);
                                        cmd2.Parameters.AddWithValue("@initialname", obj.customer_name_initial);
                                        cmd2.Parameters.AddWithValue("@fname", obj.customer_fname);
                                        cmd2.Parameters.AddWithValue("@mname", obj.customer_mname);
                                        cmd2.Parameters.AddWithValue("@lname", obj.customer_lname);
                                        connection2.Open();
                                    }
                                    msg1 = " Data Extracted Sucessfully";
                                    break;
                                    case "300":
                                    AadharMaskDetails1.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                    msg1 = "Missing AUTH-Headertoken";
                                    break;
                                    case "301":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid Content-Type");
                                    msg1 = "Invalid Content-Type";
                                    break;
                                    case "400":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid Image/Document");
                                    msg1 = "Invalid Image/Document";
                                    break;
                                    case "401":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid image file for Masking");
                                    msg1 = "Invalid image file for Masking";
                                    break;
                                    case "500":
                                    AadharMaskDetails1.Add("ErrorMgs", "Unsupported media type");
                                    msg1 = "Unsupported media type";
                                    break;
                                    case "501":
                                    AadharMaskDetails1.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                    msg1 = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                    break;
                                    case "502":
                                    AadharMaskDetails1.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                    msg1 = "The input image is too large. It should not be larger than 4MB.";
                                    break;
                                    case "503":
                                    AadharMaskDetails1.Add("ErrorMgs", "Bad request image format");
                                    msg1 = "Bad request image format";
                                    break;
                                    case "504":
                                    AadharMaskDetails1.Add("ErrorMgs", "Internal server error");
                                    msg1 = "Internal server error";
                                    break;
                                    case "505":
                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                    msg1 = "Exception message";
                                    break;
                                    default:
                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                    msg1 = "Some exception occured";
                                    break;
                            }
                        }
                    }
                    else if (file.FileName.Contains(".png"))
                    {
                        //var client1 = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DataExtraction");
                        var client = new RestClient("https://apigateway.indofinnet.com/api/DataExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        request.AddHeader("Content-Type", "image/jpeg");
                        request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentExtarction", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                            cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));
                            connection2.Open();
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            if (reader2.Read())
                            {
                                var ivar = reader2["result"].ToString();
                            }
                        }
                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                        if (response.StatusCode.ToString() == "OK")
                        {
                            var obj = JsonConvert.DeserializeObject<DocumentExtractionAPI>(JsonConvert.DeserializeObject<string>(response.Content));
                            AadharMaskDetails1.Add("StatusCode", obj.StatusCode);
                            AadharMaskDetails1.Add("Card_Name", obj.Card_Name);
                            AadharMaskDetails1.Add("customer_document_id", obj.customer_document_id);
                            AadharMaskDetails1.Add("customer_full_name", obj.customer_full_name);
                            AadharMaskDetails1.Add("customer_dob", obj.customer_dob);
                            AadharMaskDetails1.Add("customer_gender", obj.customer_gender);
                            AadharMaskDetails1.Add("customer_relation_type", obj.customer_relation_type);
                            AadharMaskDetails1.Add("customer_name_initial", obj.customer_name_initial);
                            AadharMaskDetails1.Add("customer_fname", obj.customer_fname);
                            AadharMaskDetails1.Add("customer_mname", obj.customer_mname);
                            AadharMaskDetails1.Add("customer_lname", obj.customer_lname);
                            switch (obj.StatusCode)
                            {
                                case "200":
                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                    string conn1 = _connectionString;
                                    using (SqlConnection connection2 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd2 = new SqlCommand("USP_InsertAIDocumentExtraction", connection2);
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@Customerid", Convert.ToInt64("PersonalId"));
                                        cmd2.Parameters.AddWithValue("@StatusCode", obj.StatusCode);
                                        cmd2.Parameters.AddWithValue("@CardName", obj.Card_Name);
                                        cmd2.Parameters.AddWithValue("@Documentid", obj.customer_document_id);
                                        cmd2.Parameters.AddWithValue("@fullname", obj.customer_full_name);
                                        cmd2.Parameters.AddWithValue("@dob", obj.customer_dob);
                                        cmd2.Parameters.AddWithValue("@gender", obj.customer_gender);
                                        cmd2.Parameters.AddWithValue("@relationtype", obj.customer_relation_type);
                                        cmd2.Parameters.AddWithValue("@initialname", obj.customer_name_initial);
                                        cmd2.Parameters.AddWithValue("@fname", obj.customer_fname);
                                        cmd2.Parameters.AddWithValue("@mname", obj.customer_mname);
                                        cmd2.Parameters.AddWithValue("@lname", obj.customer_lname);
                                        connection2.Open();
                                        SqlDataReader reader2 = cmd2.ExecuteReader();
                                        if (reader2.Read())
                                        {
                                            var ivar = reader2["result"].ToString();
                                        }
                                    }
                                    msg1 = " Data Extracted Sucessfully";
                                    break;
                                case "300":
                                    AadharMaskDetails1.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                    msg1 = "Missing AUTH-Headertoken";
                                    break;
                                case "301":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid Content-Type");
                                    msg1 = "Invalid Content-Type";
                                    break;
                                case "400":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid Image/Document");
                                    msg1 = "Invalid Image/Document";
                                    break;
                                case "401":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid image file for Masking");
                                    msg1 = "Invalid image file for Masking";
                                    break;
                                case "500":
                                    AadharMaskDetails1.Add("ErrorMgs", "Unsupported media type");
                                    msg1 = "Unsupported media type";
                                    break;
                                case "501":
                                    AadharMaskDetails1.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                    msg1 = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                    break;
                                case "502":
                                    AadharMaskDetails1.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                    msg1 = "The input image is too large. It should not be larger than 4MB.";
                                    break;
                                case "503":
                                    AadharMaskDetails1.Add("ErrorMgs", "Bad request image format");
                                    msg1 = "Bad request image format";
                                    break;
                                case "504":
                                    AadharMaskDetails1.Add("ErrorMgs", "Internal server error");
                                    msg1 = "Internal server error";
                                    break;
                                case "505":
                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                    msg1 = "Exception message";
                                    break;
                                default:
                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                    msg1 = "Some exception occured";
                                    break;
                            }
                        }
                    }
                    else if (file.FileName.Contains(".pdf"))
                    {
                        var customer_relation_type = "";
                        BinaryReader reader = new BinaryReader(file.OpenReadStream());
                        eibytes = reader.ReadBytes((int)file.Length);
                        var client2 = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentExtractionPdf");
                        client2.Timeout = -1;
                        var request2 = new RestRequest(Method.POST);
                        request2.AddHeader("Content-Type", "application/octet-stream");
                        request2.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                        request2.AddHeader("OrganisationCode", "COS75431521");
                        //request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                        request2.AddParameter("application/octet-stream", eibytes, RestSharp.ParameterType.RequestBody);
                        IRestResponse response3 = client2.Execute(request2);                       
                        string Response1 = response3.Content.Replace("[", "");
                        string Response2 = Response1.Replace("]", "");
                        var jObject = JObject.Parse(Response2);
                        string someType2 = jObject.GetValue("documentdata").ToString();
                        string someType3 = jObject.GetValue("status").ToString();
                        Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                        if (response3.StatusCode.ToString() == "OK")
                        {
                            var obj = JsonConvert.DeserializeObject<DocumentExtractionPdf>(someType2);
                            var obj1 = JsonConvert.DeserializeObject<DocumentExtractionPdf>(someType3);
                            AadharMaskDetails.Add("StatusCode", obj1.StatusCode);
                            AadharMaskDetails.Add("Message", obj1.Message);
                            AadharMaskDetails.Add("Card_Name", obj1.Card_Name);
                            AadharMaskDetails.Add("Page_Numbere", obj1.Page_Number);
                            AadharMaskDetails.Add("Masked_Image", obj.Masked_Image);
                            AadharMaskDetails.Add("customer_full_name", obj.customer_full_name);
                            AadharMaskDetails.Add("customer_name_initial", obj.customer_name_initial);
                            AadharMaskDetails.Add("customer_fname", obj.customer_fname);
                            AadharMaskDetails.Add("customer_mname ", obj.customer_mname);
                            AadharMaskDetails.Add("customer_lname", obj.customer_lname);
                            AadharMaskDetails.Add("customer_father_name", obj.customer_father_name);
                            AadharMaskDetails.Add("customer_relation_type", customer_relation_type);
                            AadharMaskDetails.Add("customer_gender", obj.customer_gender);
                            AadharMaskDetails.Add("customer_dob", obj.customer_dob);
                            AadharMaskDetails.Add("customer_document_id", obj.customer_document_id);
                            AadharMaskDetails.Add("customer_document_address", obj.customer_document_address);
                            AadharMaskDetails.Add("customer_document_image", obj.customer_document_image);
                            switch (obj1.StatusCode)
                            {
                                case "200":
                                    AadharMaskDetails.Add("ErrorMgs", "Success");

                                    string conn = _connectionString;
                                    using (SqlConnection connection25 = new SqlConnection(conn))
                                    {
                                        SqlCommand cmd25 = new SqlCommand("USP_InsertAIDocumentExtraction", connection25);
                                        cmd25.CommandType = CommandType.StoredProcedure;
                                        cmd25.Parameters.AddWithValue("@Customerid", perosnalid);
                                        cmd25.Parameters.AddWithValue("@StatusCode", obj1.StatusCode);
                                        cmd25.Parameters.AddWithValue("@CardName", obj1.Card_Name);
                                        cmd25.Parameters.AddWithValue("@Documentid", obj.customer_document_id);
                                        cmd25.Parameters.AddWithValue("@fullname", obj.customer_full_name);
                                        cmd25.Parameters.AddWithValue("@dob", obj.customer_dob);
                                        cmd25.Parameters.AddWithValue("@gender", obj.customer_gender);
                                        cmd25.Parameters.AddWithValue("@relationtype", customer_relation_type);
                                        cmd25.Parameters.AddWithValue("@initialname", obj.customer_name_initial);
                                        cmd25.Parameters.AddWithValue("@fname", obj.customer_fname);
                                        cmd25.Parameters.AddWithValue("@mname", obj.customer_mname);
                                        cmd25.Parameters.AddWithValue("@lname", obj.customer_lname);
                                        connection25.Open();
                                        SqlDataReader reader25 = cmd25.ExecuteReader();
                                        if (reader25.Read())
                                        {
                                            var ivar = reader25[0].ToString();
                                        }
                                    }
                                    msg1 = " Data Extracted Sucessfully";
                                    break;
                                case "300":
                                    AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                    msg1 = "Missing AUTH-Headertoken";
                                    break;
                                case "301":
                                    AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                    msg1 = "Invalid Content-Type";
                                    break;
                                case "400":
                                    AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                    msg1 = "Invalid Image/Document";
                                    break;
                                case "401":
                                    AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                    msg1 = "Invalid image file for Masking";
                                    break;
                                case "500":
                                    AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                    msg1 = "Unsupported media type";
                                    break;
                                case "501":
                                    AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                    msg1 = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                    break;
                                case "502":
                                    AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                    msg1 = "The input image is too large. It should not be larger than 4MB.";
                                    break;
                                case "503":
                                    AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                    msg1 = "Bad request image format";
                                    break;
                                case "504":
                                    AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                    msg1 = "Internal server error";
                                    break;
                                case "505":
                                    AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                    msg1 = "Exception message";
                                    break;
                                default:
                                    AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                    msg1 = "Some exception occured";
                                    break;
                            }
                        }
                    }
                    else if (file.FileName.Contains(".tiff"))
                    {
                        //var client1 = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DataExtraction");
                        var client = new RestClient("https://apigateway.indofinnet.com/api/DataExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        request.AddHeader("Content-Type", "image/jpeg");
                        request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentExtarction", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                            cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));
                            connection2.Open();
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            if (reader2.Read())
                            {
                                var ivar = reader2["result"].ToString();
                            }
                        }
                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                        if (response.StatusCode.ToString() == "OK")
                        {
                            var obj = JsonConvert.DeserializeObject<DocumentExtractionAPI>(JsonConvert.DeserializeObject<string>(response.Content));
                            AadharMaskDetails1.Add("StatusCode", obj.StatusCode);
                            AadharMaskDetails1.Add("Card_Name", obj.Card_Name);
                            AadharMaskDetails1.Add("customer_document_id", obj.customer_document_id);
                            AadharMaskDetails1.Add("customer_full_name", obj.customer_full_name);
                            AadharMaskDetails1.Add("customer_dob", obj.customer_dob);
                            AadharMaskDetails1.Add("customer_gender", obj.customer_gender);
                            AadharMaskDetails1.Add("customer_relation_type", obj.customer_relation_type);
                            AadharMaskDetails1.Add("customer_name_initial", obj.customer_name_initial);
                            AadharMaskDetails1.Add("customer_fname", obj.customer_fname);
                            AadharMaskDetails1.Add("customer_mname", obj.customer_mname);
                            AadharMaskDetails1.Add("customer_lname", obj.customer_lname);
                            switch (obj.StatusCode)
                            {
                                case "200":
                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                    string conn1 = _connectionString;
                                    using (SqlConnection connection2 = new SqlConnection(conn1))
                                    {
                                        SqlCommand cmd2 = new SqlCommand("USP_InsertAIDocumentExtraction", connection2);
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@Customerid", perosnalid);
                                        cmd2.Parameters.AddWithValue("@StatusCode", obj.StatusCode);
                                        cmd2.Parameters.AddWithValue("@CardName", obj.Card_Name);
                                        cmd2.Parameters.AddWithValue("@Documentid", obj.customer_document_id);
                                        cmd2.Parameters.AddWithValue("@fullname", obj.customer_full_name);
                                        cmd2.Parameters.AddWithValue("@dob", obj.customer_dob);
                                        cmd2.Parameters.AddWithValue("@gender", obj.customer_gender);
                                        cmd2.Parameters.AddWithValue("@relationtype", obj.customer_relation_type);
                                        cmd2.Parameters.AddWithValue("@initialname", obj.customer_name_initial);
                                        cmd2.Parameters.AddWithValue("@fname", obj.customer_fname);
                                        cmd2.Parameters.AddWithValue("@mname", obj.customer_mname);
                                        cmd2.Parameters.AddWithValue("@lname", obj.customer_lname);
                                        connection2.Open();
                                        SqlDataReader reader2 = cmd2.ExecuteReader();
                                        if (reader2.Read())
                                        {
                                            var ivar = reader2["result"].ToString();
                                        }
                                    }
                                    msg1 = " Data Extracted Sucessfully";
                                    break;
                                case "300":
                                    AadharMaskDetails1.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                    msg1 = "Missing AUTH-Headertoken";
                                    break;
                                case "301":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid Content-Type");
                                    msg1 = "Invalid Content-Type";
                                    break;
                                case "400":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid Image/Document");
                                    msg1 = "Invalid Image/Document";
                                    break;
                                case "401":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid image file for Masking");
                                    msg1 = "Invalid image file for Masking";
                                    break;
                                case "500":
                                    AadharMaskDetails1.Add("ErrorMgs", "Unsupported media type");
                                    msg1 = "Unsupported media type";
                                    break;
                                case "501":
                                    AadharMaskDetails1.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                    msg1 = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                    break;
                                case "502":
                                    AadharMaskDetails1.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                    msg1 = "The input image is too large. It should not be larger than 4MB.";
                                    break;
                                case "503":
                                    AadharMaskDetails1.Add("ErrorMgs", "Bad request image format");
                                    msg1 = "Bad request image format";
                                    break;
                                case "504":
                                    AadharMaskDetails1.Add("ErrorMgs", "Internal server error");
                                    msg1 = "Internal server error";
                                    break;
                                case "505":
                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                    msg1 = "Exception message";
                                    break;
                                default:
                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                    msg1 = "Some exception occured";
                                    break;
                            }
                        }
                    }
                    else if (file.FileName.Contains(".TIF"))
                    {
                        //var client1 = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DataExtraction");
                        var client = new RestClient("https://apigateway.indofinnet.com/api/DataExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        request.AddHeader("Content-Type", "image/jpeg");
                        request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        string conn = _connectionString;
                        using (SqlConnection connection2 = new SqlConnection(conn))
                        {
                            SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentExtarction", connection2);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                            cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));
                            connection2.Open();
                            SqlDataReader reader2 = cmd2.ExecuteReader();
                            if (reader2.Read())
                            {
                                var ivar = reader2["result"].ToString();
                            }
                        }
                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                        if (response.StatusCode.ToString() == "OK")
                        {
                            var obj = JsonConvert.DeserializeObject<DocumentExtractionAPI>(JsonConvert.DeserializeObject<string>(response.Content));
                            AadharMaskDetails1.Add("StatusCode", obj.StatusCode);
                            AadharMaskDetails1.Add("Card_Name", obj.Card_Name);
                            AadharMaskDetails1.Add("customer_document_id", obj.customer_document_id);
                            AadharMaskDetails1.Add("customer_full_name", obj.customer_full_name);
                            AadharMaskDetails1.Add("customer_dob", obj.customer_dob);
                            AadharMaskDetails1.Add("customer_gender", obj.customer_gender);
                            AadharMaskDetails1.Add("customer_relation_type", obj.customer_relation_type);
                            AadharMaskDetails1.Add("customer_name_initial", obj.customer_name_initial);
                            AadharMaskDetails1.Add("customer_fname", obj.customer_fname);
                            AadharMaskDetails1.Add("customer_mname", obj.customer_mname);
                            AadharMaskDetails1.Add("customer_lname", obj.customer_lname);
                            switch (obj.StatusCode)
                            {
                                case "200":
                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                    string conn2 = _connectionString;
                                    using (SqlConnection connection2 = new SqlConnection(conn2))
                                    {
                                        SqlCommand cmd2 = new SqlCommand("USP_InsertAIDocumentExtraction", connection2);
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.CommandType = CommandType.StoredProcedure;
                                        cmd2.Parameters.AddWithValue("@Customerid", Convert.ToInt64("PersonalId"));
                                        cmd2.Parameters.AddWithValue("@StatusCode", obj.StatusCode);
                                        cmd2.Parameters.AddWithValue("@CardName", obj.Card_Name);
                                        cmd2.Parameters.AddWithValue("@Documentid", obj.customer_document_id);
                                        cmd2.Parameters.AddWithValue("@fullname", obj.customer_full_name);
                                        cmd2.Parameters.AddWithValue("@dob", obj.customer_dob);
                                        cmd2.Parameters.AddWithValue("@gender", obj.customer_gender);
                                        cmd2.Parameters.AddWithValue("@relationtype", obj.customer_relation_type);
                                        cmd2.Parameters.AddWithValue("@initialname", obj.customer_name_initial);
                                        cmd2.Parameters.AddWithValue("@fname", obj.customer_fname);
                                        cmd2.Parameters.AddWithValue("@mname", obj.customer_mname);
                                        cmd2.Parameters.AddWithValue("@lname", obj.customer_lname);
                                        connection2.Open();
                                        SqlDataReader reader2 = cmd2.ExecuteReader();
                                        if (reader2.Read())
                                        {
                                            var ivar = reader2["result"].ToString();
                                        }
                                    }
                                    msg1 = " Data Extracted Sucessfully";
                                    break;
                                case "300":
                                    AadharMaskDetails1.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                    msg1 = "Missing AUTH-Headertoken";
                                    break;
                                case "301":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid Content-Type");
                                    msg1 = "Invalid Content-Type";
                                    break;
                                case "400":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid Image/Document");
                                    msg1 = "Invalid Image/Document";
                                    break;
                                case "401":
                                    AadharMaskDetails1.Add("ErrorMgs", "Invalid image file for Masking");
                                    msg1 = "Invalid image file for Masking";
                                    break;
                                case "500":
                                    AadharMaskDetails1.Add("ErrorMgs", "Unsupported media type");
                                    msg1 = "Unsupported media type";
                                    break;
                                case "501":
                                    AadharMaskDetails1.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                    msg1 = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                    break;
                                case "502":
                                    AadharMaskDetails1.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                    msg1 = "The input image is too large. It should not be larger than 4MB.";
                                    break;
                                case "503":
                                    AadharMaskDetails1.Add("ErrorMgs", "Bad request image format");
                                    msg1 = "Bad request image format";
                                    break;
                                case "504":
                                    AadharMaskDetails1.Add("ErrorMgs", "Internal server error");
                                    msg1 = "Internal server error";
                                    break;
                                case "505":
                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                    msg1 = "Exception message";
                                    break;
                                default:
                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                    msg1 = "Some exception occured";
                                    break;
                            }
                        }
                    }
                }             
                return Json(msg1);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                //PortalException.InsertPortalException(ex);
                return Json(ex);
            }
        }
        public async Task<ActionResult> UploadFiles(string ddl_idProof, string DocMainType, string imagePath)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {              
                IFormFile files = Request.Form.Files[0];
                HttpContext.Session.SetString("doc", DocMainType);
                long? isInserted;
                ClsDocumentDetails objdoc = new ClsDocumentDetails();
                ClsDocDetails objFinalDoc = new ClsDocDetails();
                objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                byte[] bytesImage = null;
                string ProofOfIdCode = ddl_idProof;
                if (ddl_idProof == "Signature")
                {
                    string fname;
                    IFormFile file = Request.Form.Files[0];
                    fname = file.FileName;
                    byte[] eibytes1 = null;
                    BinaryReader reader = new BinaryReader(file.OpenReadStream());
                    eibytes1 = reader.ReadBytes((int)file.Length);
                    string conn = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(conn))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_AddDocumentsFace", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@document", eibytes1);
                        cmd2.Parameters.AddWithValue("@docName", fname);
                        cmd2.Parameters.AddWithValue("@documentCategoryCode", "");
                        cmd2.Parameters.AddWithValue("@docTypeId", "");
                        cmd2.Parameters.AddWithValue("@docMainType", "SI");
                        cmd2.Parameters.AddWithValue("@createdBy", "");
                        cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                        cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                        cmd2.Parameters.AddWithValue("@documentCategory", "");
                        cmd2.Parameters.AddWithValue("@Source", "Upload");
                        cmd2.Parameters.AddWithValue("@Faceext", eibytes1);
                        cmd2.Parameters.AddWithValue("@Signature", eibytes1);
                        connection2.Open();
                        isInserted = cmd2.ExecuteNonQuery();
                        connection2.Close();
                        //TempData[msg] = "Signature Uploaded Successfully";
                        return Json("Signature Uploaded Successfully");//RedirectToAction("CustomerDocumentDetails", "DocumentDetails");
                    }
                }
                    string POItype = Convert.ToString(Request.Form.Files[0]);
                byte[] eibytes = null;
                byte[] abcd = null;
                byte[] xyz = null;
                byte[] def = null;
                byte[] mno = null;
                objFinalDoc.DocCategoryCode = ProofOfIdCode;
                string fnamee;
                long perosnalid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                if (ddl_idProof == "69" || ddl_idProof == "70")
                {
                    ViewBag.personalid = perosnalid;
                    IFormFile file = Request.Form.Files[0];
                    string pathString = Path.Combine(@"C:\Users\Dell\Documents\INDOFINNET_PORTAL\Uploads");
                    fnamee = perosnalid + ".pdf";
                    string str = "CropPdf-" + fnamee;
                    ViewBag.Filename = fnamee;
                    fnamee = System.IO.Path.Combine(pathString, fnamee);
                    return Json(str);
                }
                else
                {
                    IFormFile file = Request.Form.Files[0];
                    {
                        string fname;
                        int j = 0;                     
                        if (file.FileName.Contains(".jpg"))
                        {
                            fname = file.FileName;
                            var pathString = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                            BinaryReader reader = new BinaryReader(file.OpenReadStream());
                            eibytes = reader.ReadBytes((int)file.Length);
                            HttpContext.Session.SetString("eibyte", "eibytes");
                            byte[] newbyte = eibytes;
                            try
                            {
                                //var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentClassificationData? eibytes");
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentClassificationData?OrgID=Alpha01");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string conn1 = _connectionString;
                                using (SqlConnection connection2 = new SqlConnection(conn1))
                                {
                                    SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentType", connection2);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                                    cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));
                                    connection2.Open();
                                    SqlDataReader reader2 = cmd2.ExecuteReader();
                                    if (reader2.Read())
                                    {
                                        var ivar = reader2["result"].ToString();
                                    }
                                }
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(JsonConvert.DeserializeObject<string>(response.Content));
                                    switch (obj.DocumentType)
                                    {
                                        case "1":
                                            AadharMaskDetails.Add("DocumentType", "aadhar card");
                                            msgs = "Aadhaar Card Identified & Uploaded Successfully ";

                                            break;
                                        case "2":
                                            AadharMaskDetails.Add("DocumentType", "pan card");
                                            msgs = "Pan Card Identified & Uploaded Successfully ";
                                            break;
                                        case "3":
                                            AadharMaskDetails.Add("DocumentType", "Voter id");
                                            msgs = "Voter id uploaded";
                                            break;
                                        case "4":
                                            AadharMaskDetails.Add("DocumentType", "Driving licenses");
                                            msgs = "Driving licenses uploaded";
                                            break;
                                        case "5":
                                            AadharMaskDetails.Add("DocumentType", "Pass port");
                                            msgs = "Pass port uploaded";
                                            break;
                                        case "6":
                                            AadharMaskDetails.Add("DocumentType", "Invalid Document");
                                            msgs = "Invalid Document ";
                                            break;
                                        default:
                                            AadharMaskDetails.Add("DocumentType", "Document Not Detected");
                                            msgs = "Document Not Detected ";
                                            break;
                                    }
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);//obj.StatusCode
                                    switch (obj.StatusCode)
                                    {
                                        case "200":
                                            AadharMaskDetails.Add("ErrorMgs", "Success");
                                            break;
                                        case "300":
                                            AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                            msgs = "Missing AUTH-Headertoken";
                                            break;
                                        case "301":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                            msgs = "Invalid Content-Type";
                                            break;
                                        case "400":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                            msgs = "Invalid Image/Document";
                                            break;
                                        case "401":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                            msgs = "Invalid image file for Masking";
                                            break;
                                        case "500":
                                            AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                            msgs = "Unsupported media type";
                                            break;
                                        case "501":
                                            AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                            msgs = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                            break;
                                        case "502":
                                            AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                            msgs = "The input image is too large. It should not be larger than 4MB.";
                                            break;
                                        case "503":
                                            AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                            msgs = "Bad request image forma";
                                            break;
                                        case "504":
                                            AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                            msgs = "Internal server erro";
                                            break;
                                        case "505":
                                            AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                            msgs = "Exception message";
                                            break;
                                        default:
                                            AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                            msgs = "Some exception occured";
                                            break;
                                    }

                                    if (obj.DocumentType == "1")
                                    {
                                        abcd = eibytes;
                                        //var client1 = new RestClient("https://alphafinsoftwebserviceapidevelopment.azurewebsites.net/AlphaFinsoftUser/DocumentMaskingData");azureapi url
                                        //var client1 = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentMaskingData");//alphaazureapiurl
                                        //var client1 = new RestClient("https://apigateway.indofinnet.com/api/https://apigateway.indofinnet.com/api/DocumentMaskingData?OrgID=Alpha01");
                                        var client1 = new RestClient("https://apigateway.indofinnet.com/api/DocumentMaskingData?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                        client1.Timeout = -1;
                                        var request1 = new RestRequest(Method.POST);
                                        request1.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                        request1.AddHeader("Content-Type", "image/jpeg");
                                        request1.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                        IRestResponse response1 = client1.Execute(request1);
                                        Console.WriteLine(response1.Content);
                                        string Response2 = response1.Content.Replace("{", "").Replace(@"""", "");
                                        string Response3 = Response2.Replace("}", "");
                                        string Response4 = Response3.Replace("/", "");
                                        var jsonSendResponse = Response3.Split(',');
                                        var Response5 = jsonSendResponse[2];
                                        var jsonSendResponse1 = Response5.Split(':');
                                        var Image1 = jsonSendResponse1[1];
                                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                                        if (response.StatusCode.ToString() == "OK")
                                        {
                                            var obj1 = JsonConvert.DeserializeObject<AadharMaskResult>(JsonConvert.DeserializeObject<string>(response1.Content));
                                            AadharMaskDetails1.Add("imagefile", obj1.imagefile);
                                            AadharMaskDetails1.Add("DocumentType", obj1.DocumentType);
                                            AadharMaskDetails1.Add("StatusCode", obj1.StatusCode);
                                            string abc = obj1.imagefile;
                                            img = Convert.FromBase64String(abc);
                                            if (imagePath == null)
                                            {
                                                bytesImage = (eibytes);
                                            }
                                            else
                                            {
                                                byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                                xyz = bytesImage1;
                                                string ImgPath = Convert.ToString(HttpContext.Session.GetString("imagepath"));
                                                string ImagePathNew = "E://IndoFinCroppedImages/" + objFinalDoc.CustomerDetailId + "_uploadedimage1.jpg";
                                                MemoryStream ms = new MemoryStream(bytesImage1, 0, bytesImage1.Length);
                                                ms.Write(bytesImage1, 0, bytesImage1.Length);
                                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                                                string pathSaveimg = @"C:/Users/keerti/Desktop/update indo_portal/INDOFINNET_PORTAL/Uploads/" + objFinalDoc.CustomerDetailId + "_uploadedimage1.jpg";
                                                image.Save(Path.GetFullPath(pathSaveimg), ImageFormat.Jpeg);
                                                FileInfo fi = new FileInfo(Path.GetFullPath(pathSaveimg));
                                                bytesImage1 = System.IO.File.ReadAllBytes(Path.GetFullPath(pathSaveimg));
                                            }
                                            switch (obj.StatusCode)
                                            {
                                                case "200":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                                    break;
                                                case "300":
                                                    AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
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
                                                    AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                    break;
                                                case "505":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                                    break;
                                                default:
                                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                                    break;
                                            }
                                        }
                                        Console.WriteLine(response1.Content);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error_log.WriteErrorLog(ex.ToString());
                                //PortalException.InsertPortalException(ex);
                                return Json("Exception");
                            }
                            objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            if (msgs == "Aadhaar Card Identified & Uploaded Successfully ")
                            {
                                objFinalDoc.DocDetails = xyz;
                            }
                            else
                            {
                                objFinalDoc.DocDetails = eibytes;
                            }
                            if (files.Length > 1)
                            {
                                objFinalDoc.documentCategory = POItype + " Page" + j;
                            }
                            else
                            {
                                if (ddl_idProof == "Signature")
                                {
                                    objFinalDoc.documentCategory = ddl_idProof;
                                }
                                else
                                {
                                    objFinalDoc.documentCategory = POItype;
                                }
                            }
                            if (msgs == "Aadhaar Card Identified & Uploaded Successfully " || msgs == "Pan Card Identified & Uploaded Successfully " || msgs == "Voter id uploaded" || msgs == "Driving licenses uploaded" || msgs == "Pass port uploaded")

                            {
                                //var client = new RestClient(" https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentFaceExtraction");
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentFaceExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    // var obj = JsonConvert.DeserializeObject<FaceExtractionAPI>(response.Content);
                                    var obj = JsonConvert.DeserializeObject<FaceExtractionAPI>(JsonConvert.DeserializeObject<string>(response.Content));
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_face);
                                    string abc = obj.Extacted_face;
                                    faceextract = Convert.FromBase64String(abc);
                                    if (imagePath == null)
                                    {                                      
                                        bytesImage = eibytes;
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);

                                        def = bytesImage1;

                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");

                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            if (msgs == "Pan Card Identified & Uploaded Successfully " || msgs == "Driving licenses uploaded" || msgs == "Pass port uploaded")
                            {
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentSignatureExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<SignatureExtractApi>(JsonConvert.DeserializeObject<string>(response.Content));
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_Signature);
                                    string abc = obj.Extacted_Signature;
                                    signatureexract = Convert.FromBase64String(abc);
                                    if (imagePath == null)
                                    {                       
                                        bytesImage = eibytes;
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                        mno = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            objFinalDoc.DocName = fname;
                            extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                            objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            var iva = "";
                            string conn = _connectionString;
                            using (SqlConnection connection2 = new SqlConnection(conn))
                            {
                                SqlCommand cmd2 = new SqlCommand("USP_GetDocumentType", connection2);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@docType", ("." + extension));
                                connection2.Open();
                                SqlDataReader reader2 = cmd2.ExecuteReader();
                                if (reader2.Read())
                                {
                                    iva = reader2["documentTypeId"].ToString();
                                }
                            }                           
                            objFinalDoc.DocType1 = iva;
                            objFinalDoc.DocMainType = DocMainType;
                            objFinalDoc.DocCategoryCode = ProofOfIdCode;
                            objFinalDoc.Source = "Upload";
                            objFinalDoc.Faceext = def;
                            objFinalDoc.Signature = mno;                           
                            objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));                       
                            string conn2 = _connectionString;
                            using (SqlConnection connection2 = new SqlConnection(conn2))
                            {
                                SqlCommand cmd2 = new SqlCommand("USP_AddDocumentsFace", connection2);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                                if (img != null)
                                {
                                    cmd2.Parameters.AddWithValue("@document", img);
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@document", eibytes);
                                }
                                cmd2.Parameters.AddWithValue("@docName", fname);
                                cmd2.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocCategoryCode);
                                cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.DocType1);
                                cmd2.Parameters.AddWithValue("@docMainType", objFinalDoc.DocMainType);
                                cmd2.Parameters.AddWithValue("@createdBy", "");
                                cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                                cmd2.Parameters.AddWithValue("@DocumentIdDate", null);
                                cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                                if (objFinalDoc.DocMainType == "I")
                                {
                                    cmd2.Parameters.AddWithValue("@documentCategory", "Aadhaar Card");
                                }
                                else if (objFinalDoc.DocMainType == "CA")
                                {
                                    cmd2.Parameters.AddWithValue("@documentCategory", "Pan Card");
                                }
                                else
                                {
                                    cmd2.Parameters.AddWithValue("@documentCategory", "");
                                }
                                cmd2.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                                cmd2.Parameters.AddWithValue("@Faceext", faceextract);
                                cmd2.Parameters.AddWithValue("@Signature", signatureexract);
                                connection2.Open();
                                isInserted = cmd2.ExecuteNonQuery();
                                connection2.Close();
                            }
                        }
                        if (file.FileName.Contains(".png"))
                        {
                            fname = file.FileName;
                            BinaryReader reader = new BinaryReader(file.OpenReadStream());
                            eibytes = reader.ReadBytes((int)file.Length);
                            HttpContext.Session.SetString("eibyte", "eibytes");
                            byte[] newbyte = eibytes;                         
                            try
                            {
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentClassificationData?OrgID=Alpha01");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string conn1 = _connectionString;
                                using (SqlConnection connection2 = new SqlConnection(conn1))
                                {
                                    SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentType", connection2);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                                    cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));
                                    connection2.Open();
                                    SqlDataReader reader2 = cmd2.ExecuteReader();
                                    if (reader2.Read())
                                    {
                                        var ivar = reader2["result"].ToString();
                                    }
                                }
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(JsonConvert.DeserializeObject<string>(response.Content));
                                    switch (obj.DocumentType)
                                    {
                                        case "1":
                                            AadharMaskDetails.Add("DocumentType", "aadhar card");
                                            msgs = "Aadhaar Card Identified & Uploaded Successfully ";
                                            break;
                                        case "2":
                                            AadharMaskDetails.Add("DocumentType", "pan card");
                                            msgs = "Pan Card Identified & Uploaded Successfully ";
                                            break;
                                        case "3":
                                            AadharMaskDetails.Add("DocumentType", "Voter id");
                                            msgs = "Voter id uploaded";
                                            break;
                                        case "4":
                                            AadharMaskDetails.Add("DocumentType", "Driving licenses");
                                            msgs = "Driving licenses uploaded";
                                            break;
                                        case "5":
                                            AadharMaskDetails.Add("DocumentType", "Pass port");
                                            msgs = "Pass port uploaded";
                                            break;
                                        case "6":
                                            AadharMaskDetails.Add("DocumentType", "Invalid Document");
                                            msgs = "Invalid Document ";
                                            break;
                                        default:
                                            AadharMaskDetails.Add("DocumentType", "Document Not Detected");
                                            msgs = "Document Not Detected ";
                                            break;
                                    }
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);//obj.StatusCode
                                    switch (obj.StatusCode)
                                    {
                                        case "200":
                                            AadharMaskDetails.Add("ErrorMgs", "Success");
                                            break;
                                        case "300":
                                            AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                            msgs = "Missing AUTH-Headertoken";
                                            break;
                                        case "301":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                            msgs = "Invalid Content-Type";
                                            break;
                                        case "400":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                            msgs = "Invalid Image/Document";
                                            break;
                                        case "401":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                            msgs = "Invalid image file for Masking";
                                            break;
                                        case "500":
                                            AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                            msgs = "Unsupported media type";
                                            break;
                                        case "501":
                                            AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                            msgs = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                            break;
                                        case "502":
                                            AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                            msgs = "The input image is too large. It should not be larger than 4MB.";
                                            break;
                                        case "503":
                                            AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                            msgs = "Bad request image forma";
                                            break;
                                        case "504":
                                            AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                            msgs = "Internal server erro";
                                            break;
                                        case "505":
                                            AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                            msgs = "Exception message";
                                            break;
                                        default:
                                            AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                            msgs = "Some exception occured";
                                            break;
                                    }
                                    if (obj.DocumentType == "1")
                                    {
                                        abcd = eibytes;                                      
                                        var client1 = new RestClient("https://apigateway.indofinnet.com/api/DocumentMaskingData?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                        client1.Timeout = -1;
                                        var request1 = new RestRequest(Method.POST);
                                        request1.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                        request1.AddHeader("Content-Type", "image/jpeg");
                                        request1.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                        IRestResponse response1 = client1.Execute(request1);
                                        Console.WriteLine(response1.Content);
                                        string Response2 = response1.Content.Replace("{", "").Replace(@"""", "");
                                        string Response3 = Response2.Replace("}", "");
                                        string Response4 = Response3.Replace("/", "");
                                        var jsonSendResponse = Response3.Split(',');
                                        var Response5 = jsonSendResponse[2];
                                        var jsonSendResponse1 = Response5.Split(':');
                                        var Image1 = jsonSendResponse1[1];                                      
                                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                                        if (response.StatusCode.ToString() == "OK")
                                        {
                                            var obj1 = JsonConvert.DeserializeObject<AadharMaskResult>(JsonConvert.DeserializeObject<string>(response1.Content));
                                            AadharMaskDetails1.Add("imagefile", obj1.imagefile);
                                            AadharMaskDetails1.Add("DocumentType", obj1.DocumentType);
                                            AadharMaskDetails1.Add("StatusCode", obj1.StatusCode);
                                            byte[] barr = Encoding.ASCII.GetBytes(obj1.imagefile);
                                            if (imagePath == null)
                                            {                                         
                                                bytesImage = (eibytes);
                                            }
                                            else
                                            {
                                                byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                                xyz = bytesImage1;
                                                string ImgPath = Convert.ToString(HttpContext.Session.GetString("imagepath"));
                                                string ImagePathNew = "E://IndoFinCroppedImages/" + objFinalDoc.CustomerDetailId + "_uploadedimage1.jpg";                                               
                                                MemoryStream ms = new MemoryStream(bytesImage1, 0, bytesImage1.Length);
                                                ms.Write(bytesImage1, 0, bytesImage1.Length);
                                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);              
                                                string pathSaveimg = @"C:/Users/keerti/Desktop/update indo_portal/INDOFINNET_PORTAL/Uploads/" + objFinalDoc.CustomerDetailId + "_uploadedimage1.jpg";
                                                image.Save(Path.GetFullPath(pathSaveimg), ImageFormat.Jpeg);
                                                FileInfo fi = new FileInfo(Path.GetFullPath(pathSaveimg));
                                                bytesImage1 = System.IO.File.ReadAllBytes(Path.GetFullPath(pathSaveimg));
                                            }
                                            switch (obj.StatusCode)
                                            {
                                                case "200":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                                    break;
                                                case "300":
                                                    AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
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
                                                    AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                    break;
                                                case "505":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                                    break;
                                                default:
                                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                                    break;
                                            }
                                        }
                                        Console.WriteLine(response1.Content);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error_log.WriteErrorLog(ex.ToString());
                                return Json("Exception");
                            }
                            objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            if (msgs == "Aadhaar Card Identified & Uploaded Successfully ")
                            {
                                objFinalDoc.DocDetails = xyz;
                            }
                            else
                            {
                                objFinalDoc.DocDetails = eibytes;
                            }
                            if (files.Length > 1)
                            {
                                objFinalDoc.documentCategory = POItype + " Page" + j;
                            }
                            else
                            {
                                if (ddl_idProof == "Signature")
                                {
                                    objFinalDoc.documentCategory = ddl_idProof;
                                }
                                else
                                {
                                    objFinalDoc.documentCategory = POItype;
                                }
                            }
                            if (msgs == "Aadhaar Card Identified & Uploaded Successfully " || msgs == "Pan Card Identified & Uploaded Successfully " || msgs == "Voter id uploaded" || msgs == "Driving licenses uploaded" || msgs == "Pass port uploaded")

                            {
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentFaceExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {                            
                                    var obj = JsonConvert.DeserializeObject<FaceExtractionAPI>(JsonConvert.DeserializeObject<string>(response.Content));
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_face);
                                    if (imagePath == null)
                                    {
                                        bytesImage = eibytes;
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                        def = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            if (msgs == "Pan Card Identified & Uploaded Successfully " || msgs == "Driving licenses uploaded" || msgs == "Pass port uploaded")
                            {
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentSignatureExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<SignatureExtractApi>(JsonConvert.DeserializeObject<string>(response.Content));
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_Signature);
                                    string abc = obj.Extacted_Signature;
                                    signatureexract = Convert.FromBase64String(abc);
                                    if (imagePath == null)
                                    {
                                        bytesImage = eibytes;
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                        mno = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            objFinalDoc.DocName = fname;
                            extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                            objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            var iva = "";
                            string conn = _connectionString;
                            using (SqlConnection connection2 = new SqlConnection(conn))
                            {
                                SqlCommand cmd2 = new SqlCommand("USP_GetDocumentType", connection2);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@docType", ("." + extension));
                                connection2.Open();
                                SqlDataReader reader2 = cmd2.ExecuteReader();
                                if (reader2.Read())
                                {
                                    iva = reader2["documentTypeId"].ToString();
                                }
                            }
                            objFinalDoc.DocType1 = iva;
                            objFinalDoc.DocMainType = DocMainType;
                            objFinalDoc.DocCategoryCode = ProofOfIdCode;
                            objFinalDoc.Source = "Upload";
                            objFinalDoc.Faceext = def;
                            objFinalDoc.Signature = mno;
                            objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));                          
                            string conn2 = _connectionString;
                            using (SqlConnection connection2 = new SqlConnection(conn2))
                            {
                                SqlCommand cmd2 = new SqlCommand("USP_AddDocumentsFace", connection2);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                                cmd2.Parameters.AddWithValue("@document", eibytes);
                                cmd2.Parameters.AddWithValue("@docName", fname);
                                cmd2.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocCategoryCode);
                                cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.DocType1);
                                cmd2.Parameters.AddWithValue("@docMainType", objFinalDoc.DocMainType);
                                cmd2.Parameters.AddWithValue("@createdBy", "");
                                cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.DocumentId);
                                cmd2.Parameters.AddWithValue("@DocumentIdDate", null);
                                cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                                cmd2.Parameters.AddWithValue("@documentCategory", "");
                                cmd2.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                                cmd2.Parameters.AddWithValue("@Faceext", eibytes);
                                cmd2.Parameters.AddWithValue("@Signature", eibytes);
                                connection2.Open();
                                isInserted = cmd2.ExecuteNonQuery();
                                connection2.Close();
                            }
                        }
                        else if (file.FileName.Contains(".tiff"))
                        {
                            fname = file.FileName;
                            BinaryReader reader = new BinaryReader(file.OpenReadStream());
                            eibytes = reader.ReadBytes((int)file.Length);
                            string adc = Path.GetFileName(fname);                         
                            string fname2 = Path.GetFileNameWithoutExtension(adc) + ".jpeg";
                            byte[] newbyte = eibytes;
                            try
                            {                             
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentClassificationData?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/tiff");
                                request.AddParameter("image/tiff", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string conn1 = _connectionString;
                                using (SqlConnection connection2 = new SqlConnection(conn1))
                                {
                                    SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentType", connection2);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                                    cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));
                                    connection2.Open();
                                    SqlDataReader reader2 = cmd2.ExecuteReader();
                                    if (reader2.Read())
                                    {
                                        var ivar = reader2["result"].ToString();
                                    }
                                }
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(JsonConvert.DeserializeObject<string>(response.Content));
                                    string msg = "";
                                    switch (obj.DocumentType)
                                    {
                                        case "1":
                                            AadharMaskDetails.Add("DocumentType", "aadhar card");
                                            msgs = "Aadhaar Card Identified & Uploaded Successfully ";

                                            break;
                                        case "2":
                                            AadharMaskDetails.Add("DocumentType", "pan card");
                                            msgs = "Pan Card Identified & Uploaded Successfully ";
                                            break;
                                        case "3":
                                            AadharMaskDetails.Add("DocumentType", "Voter id");
                                            msgs = "Voter id uploaded";
                                            break;
                                        case "4":
                                            AadharMaskDetails.Add("DocumentType", "Driving licenses");
                                            msgs = "Driving licenses uploaded";
                                            break;
                                        case "5":
                                            AadharMaskDetails.Add("DocumentType", "Pass port");
                                            msgs = "Pass port uploaded";
                                            break;
                                        case "6":
                                            AadharMaskDetails.Add("DocumentType", "Invalid Document");
                                            msgs = "Invalid Document ";
                                            break;
                                        default:
                                            AadharMaskDetails.Add("DocumentType", "Document Not Detected");
                                            msgs = "Document Not Detected ";
                                            break;
                                    }
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    switch (obj.StatusCode)
                                    {
                                        case "200":
                                            AadharMaskDetails.Add("ErrorMgs", "Success");
                                            break;
                                        case "300":
                                            AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                            msgs = "Missing AUTH-Headertoken";
                                            break;
                                        case "301":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                            msgs = "Invalid Content-Type";
                                            break;
                                        case "400":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                            msgs = "Invalid Image/Document";
                                            break;
                                        case "401":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                            msgs = "Invalid image file for Masking";
                                            break;
                                        case "500":
                                            AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                            msgs = "Unsupported media type";
                                            break;
                                        case "501":
                                            AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                            msgs = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                            break;
                                        case "502":
                                            AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                            msgs = "The input image is too large. It should not be larger than 4MB.";
                                            break;
                                        case "503":
                                            AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                            msgs = "Bad request image forma";
                                            break;
                                        case "504":
                                            AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                            msgs = "Internal server erro";
                                            break;
                                        case "505":
                                            AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                            msgs = "Exception message";
                                            break;
                                        default:
                                            AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                            msgs = "Some exception occured";
                                            break;
                                    }
                                    if (obj.DocumentType == "1")
                                    {
                                        var client1 = new RestClient("https://apigateway.indofinnet.com/api/DocumentMaskingData?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                        client.Timeout = -1;
                                        var request1 = new RestRequest(Method.POST);
                                        request1.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                        request1.AddHeader("Content-Type", "image/tiff");
                                        request1.AddParameter("image/tiff", eibytes, RestSharp.ParameterType.RequestBody);
                                        IRestResponse response1 = client.Execute(request1);
                                        Console.WriteLine(response1.Content);
                                        string Response2 = response1.Content.Replace("{", "").Replace(@"""", "");
                                        string Response3 = Response2.Replace("}", "");
                                        string Response4 = Response3.Replace("/", "");
                                        var jsonSendResponse = Response3.Split(',');
                                        var Response5 = jsonSendResponse[2];
                                        var jsonSendResponse1 = Response5.Split(':');
                                        var Image1 = jsonSendResponse1[1];                                     
                                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                                        if (response.StatusCode.ToString() == "OK")
                                        {
                                            var obj1 = JsonConvert.DeserializeObject<AadharMaskResult>(JsonConvert.DeserializeObject<string>(response.Content));
                                            AadharMaskDetails1.Add("imagefile", obj1.imagefile);
                                            AadharMaskDetails1.Add("DocumentType", obj1.DocumentType);
                                            AadharMaskDetails1.Add("StatusCode", obj1.StatusCode);
                                            if (imagePath == null)
                                            {
                                                bytesImage = eibytes;
                                            }
                                            else
                                            {
                                                byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                                xyz = bytesImage1;
                                                string ImgPath = Convert.ToString(HttpContext.Session.GetString("ImgPath"));
                                                string ImagePathNew = "E://IndoFinCroppedImages/" + objFinalDoc.CustomerDetailId + "_uploadedimage1.jpg";                                          
                                                MemoryStream ms = new MemoryStream(bytesImage1, 0, bytesImage1.Length);
                                                ms.Write(bytesImage1, 0, bytesImage1.Length);
                                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                                                string pathSaveimg = @"C:/Users/keerti/Desktop/update indo_portal/INDOFINNET_PORTAL/Uploads/" + objFinalDoc.CustomerDetailId + "_uploadedimage1.jpg"; 
                                                image.Save(Path.GetFullPath(pathSaveimg), ImageFormat.Jpeg);
                                                FileInfo fi = new FileInfo(Path.GetFullPath(pathSaveimg));
                                                bytesImage1 = System.IO.File.ReadAllBytes(Path.GetFullPath(pathSaveimg));
                                            }
                                            switch (obj.StatusCode)
                                            {
                                                case "200":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                                    break;
                                                case "300":
                                                    AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
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
                                                    AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                    break;
                                                case "505":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                                    break;
                                                default:
                                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                                    break;
                                            }
                                        }
                                        Console.WriteLine(response1.Content);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error_log.WriteErrorLog(ex.ToString());
                                return Json("Exception");
                            }
                            objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                            if (msgs == "Aadhaar Card Identified & Uploaded Successfully ")
                            {
                                objFinalDoc.DocDetails = xyz;
                            }
                            else
                            {
                            }
                            if (files.Length > 1)
                            {
                                objFinalDoc.documentCategory = POItype + " Page" + j;
                            }
                            else
                            {
                                if (ddl_idProof == "Signature")
                                {
                                    objFinalDoc.documentCategory = ddl_idProof;
                                }
                                else
                                {
                                    objFinalDoc.documentCategory = POItype;
                                }
                            }
                            if (msgs == "Aadhaar Card Identified & Uploaded Successfully " || msgs == "Pan Card Identified & Uploaded Successfully " || msgs == "Voter id uploaded" || msgs == "Driving licenses uploaded" || msgs == "Pass port uploaded")

                            {
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentFaceExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<FaceExtractionAPI>(JsonConvert.DeserializeObject<string>(response.Content));
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_face);
                                    if (imagePath == null)
                                    {
                                        bytesImage = eibytes;
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);

                                        def = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            if (msgs == "Pan Card Identified & Uploaded Successfully " || msgs == "Driving licenses uploaded" || msgs == "Pass port uploaded")
                            {
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentSignatureExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<SignatureExtractApi>(JsonConvert.DeserializeObject<string>(response.Content));
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_Signature);
                                    if (imagePath == null)
                                    {
                                        bytesImage = eibytes;
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                        mno = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            var iva = "";
                            string conn = _connectionString;
                            using (SqlConnection connection2 = new SqlConnection(conn))
                            {
                                SqlCommand cmd2 = new SqlCommand("USP_GetDocumentType", connection2);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@docType", ("." + extension));
                                connection2.Open();
                                SqlDataReader reader2 = cmd2.ExecuteReader();
                                if (reader2.Read())
                                {
                                    iva = reader2["documentTypeId"].ToString();
                                }
                            }                         
                            objFinalDoc.DocType1 = iva;
                            objFinalDoc.DocMainType = DocMainType;
                            objFinalDoc.DocCategoryCode = ProofOfIdCode;
                            objFinalDoc.Source = "Upload";
                            objFinalDoc.Faceext = def;
                            objFinalDoc.Signature = mno;
                            objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));                        
                            string conn2 = _connectionString;
                            using (SqlConnection connection2 = new SqlConnection(conn2))
                            {
                                SqlCommand cmd2 = new SqlCommand("USP_AddDocumentsFace", connection2);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                                cmd2.Parameters.AddWithValue("@document", eibytes);
                                cmd2.Parameters.AddWithValue("@docName", fname);
                                cmd2.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocCategoryCode);
                                cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.DocType1);
                                cmd2.Parameters.AddWithValue("@docMainType", objFinalDoc.DocMainType);
                                cmd2.Parameters.AddWithValue("@createdBy", "");
                                cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.DocumentId);
                                cmd2.Parameters.AddWithValue("@DocumentIdDate", null);
                                cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                                cmd2.Parameters.AddWithValue("@documentCategory", "");
                                cmd2.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                                cmd2.Parameters.AddWithValue("@Faceext", eibytes);
                                cmd2.Parameters.AddWithValue("@Signature", eibytes);
                                connection2.Open();
                                isInserted = cmd2.ExecuteNonQuery();
                                connection2.Close();
                            }
                        }
                        else if (file.FileName.Contains(".TIF"))
                        {
                            fname = file.FileName;
                            var pathString = Path.Combine(Directory.GetCurrentDirectory(), "Uploads\\");                         
                            BinaryReader reader = new BinaryReader(file.OpenReadStream());
                            eibytes = reader.ReadBytes((int)file.Length);
                            using (var stream = new FileStream(pathString, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }
                            string adc = pathString + Path.GetFileName(fname);
                            using (System.Drawing.Image imageFile = System.Drawing.Image.FromFile(adc))
                            {
                                string imanewithoutpath = Path.GetFileNameWithoutExtension(adc);
                                FrameDimension frameDimensions = new FrameDimension(
                                   imageFile.FrameDimensionsList[0]);
                                int frameNum = imageFile.GetFrameCount(frameDimensions);
                                string[] jpegPaths = new string[frameNum];
                                for (int frame = 0; frame < frameNum; frame++)
                                {
                                    imageFile.SelectActiveFrame(frameDimensions, frame);
                                    using (Bitmap bmp = new Bitmap(imageFile))
                                    {
                                        jpegPaths[frame] = String.Format(pathString + Path.GetFileNameWithoutExtension(adc) + ".jpeg", frame);
                                        bmp.Save(jpegPaths[frame], ImageFormat.Jpeg);
                                    }
                                }
                            }
                            string fname2 = Path.GetFileNameWithoutExtension(adc) + ".jpeg";
                            string omnb = Path.Combine(pathString, fname2);
                            byte[] fileContent = System.IO.File.ReadAllBytes(omnb);
                            HttpContext.Session.SetString("fileContent", Convert.ToBase64String(fileContent));
                            byte[] newbyte = eibytes;
                            try
                            {
                                var client = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentClassificationData");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("Content-Type", "application/octet-stream");
                                request.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                                request.AddHeader("OrganisationCode", "ALP18980121");
                                request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                                request.AddParameter("application/octet-stream", fileContent, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string conn = _connectionString;
                                using (SqlConnection connection2 = new SqlConnection(conn))
                                {
                                    SqlCommand cmd2 = new SqlCommand("USP_AIErrorDocumentType", connection2);
                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@CustomerId", Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                                    cmd2.Parameters.AddWithValue("@Status", Convert.ToString(response.StatusCode));
                                    cmd2.Parameters.AddWithValue("@Response", Convert.ToString(response.ResponseStatus));
                                    connection2.Open();
                                    SqlDataReader reader2 = cmd2.ExecuteReader();
                                    if (reader2.Read())
                                    {
                                        var ivar = reader2["result"].ToString();
                                    }
                                }
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(response.Content);
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
                                            msg = "Voter id uploaded";
                                            break;
                                        case "4":
                                            AadharMaskDetails.Add("DocumentType", "Driving licenses");
                                            msg = "Driving licenses uploaded";
                                            break;
                                        case "5":
                                            AadharMaskDetails.Add("DocumentType", "Pass port");
                                            msg = "Pass port uploaded";
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
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
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
                                        abcd = fileContent;
                                        //var client1 = new RestClient("https://alphafinsoftwebserviceapidevelopment.azurewebsites.net/AlphaFinsoftUser/DocumentMaskingData");azureapi url
                                        var client1 = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentMaskingData");//alphaazureapiurl
                                        client1.Timeout = -1;
                                        var request1 = new RestRequest(Method.POST);
                                        request1.AddHeader("Content-Type", "application/octet-stream");
                                        request1.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                                        request1.AddHeader("OrganisationCode", "ALP18980121");
                                        request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                                        request1.AddParameter("application/octet-stream", abcd, RestSharp.ParameterType.RequestBody);
                                        IRestResponse response1 = client1.Execute(request1);
                                        string Response2 = response1.Content.Replace("{", "").Replace(@"""", "");
                                        string Response3 = Response2.Replace("}", "");
                                        string Response4 = Response3.Replace("/", "");
                                        var jsonSendResponse = Response3.Split(',');
                                        var Response5 = jsonSendResponse[2];
                                        var jsonSendResponse1 = Response5.Split(':');
                                        var Image1 = jsonSendResponse1[1];
                                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                                        if (response.StatusCode.ToString() == "OK")
                                        {
                                            var obj1 = JsonConvert.DeserializeObject<AadharMaskResult>(response1.Content);
                                            AadharMaskDetails1.Add("imagefile", obj1.imagefile);
                                            AadharMaskDetails1.Add("DocumentType", obj1.DocumentType);
                                            AadharMaskDetails1.Add("StatusCode", obj1.StatusCode);
                                            if (imagePath != "")
                                            {
                                                bytesImage = System.IO.File.ReadAllBytes(Path.GetFullPath(imagePath));
                                            }
                                            else
                                            {
                                                byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                                xyz = bytesImage1;
                                                string ImgPath = Convert.ToString(HttpContext.Session.GetString("ImgPath"));
                                                string ImagePathNew = "E://IndoFinCroppedImages/" + objFinalDoc.CustomerDetailId + "_uploadedimage1.jpg";
                                                MemoryStream ms = new MemoryStream(bytesImage1, 0, bytesImage1.Length);
                                                ms.Write(bytesImage1, 0, bytesImage1.Length);
                                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                                                string pathSaveimg = @"C:/Users/keerti/Desktop/update indo_portal/INDOFINNET_PORTAL/Uploads/" + objFinalDoc.CustomerDetailId + "_uploadedimage1.jpg";
                                                image.Save(Path.GetFullPath(pathSaveimg), ImageFormat.Jpeg);
                                                FileInfo fi = new FileInfo(Path.GetFullPath(pathSaveimg));
                                                bytesImage1 = System.IO.File.ReadAllBytes(Path.GetFullPath(pathSaveimg));
                                            }
                                            switch (obj.StatusCode)
                                            {
                                                case "200":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                                    break;
                                                case "300":
                                                    AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
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
                                                    AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                    break;
                                                case "505":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                                    break;
                                                default:
                                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                                    break;
                                            }
                                        }
                                        Console.WriteLine(response1.Content);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error_log.WriteErrorLog(ex.ToString());
                                return Json("Exception");
                            }
                            objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                            if (msg == "Aadhaar Card Identified & Uploaded Successfully ")
                            {
                                objFinalDoc.DocDetails = xyz;
                            }
                            else
                            {
                                objFinalDoc.DocDetails = fileContent;
                            }
                            if (files.Length > 1)
                            {
                                objFinalDoc.documentCategory = POItype + " Page" + j;
                            }
                            else
                            {
                                if (ddl_idProof == "Signature")
                                {
                                    objFinalDoc.documentCategory = ddl_idProof;
                                }
                                else
                                {
                                    objFinalDoc.documentCategory = POItype;
                                }
                            }
                            if (msg == "Aadhaar Card Identified & Uploaded Successfully " || msg == "Pan Card Identified & Uploaded Successfully " || msg == "Voter id uploaded" || msg == "Driving licenses uploaded" || msg == "Pass port uploaded")

                            {
                                var client = new RestClient(" https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentFaceExtraction");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("Content-Type", "application/octet-stream");
                                request.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                                request.AddHeader("OrganisationCode", "ALP18980121");
                                //request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                                request.AddParameter("application/octet-stream", HttpContext.Session.GetString(" fileContent"), RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<FaceExtractionAPI>(response.Content);
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_face);
                                    if (imagePath != "")
                                    {
                                        bytesImage = System.IO.File.ReadAllBytes(Path.GetFullPath(imagePath));
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                        def = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            if (msg == "Pan Card Identified & Uploaded Successfully " || msg == "Driving licenses uploaded" || msg == "Pass port uploaded")
                            {
                                var client = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentSignatureExtraction");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("Content-Type", "application/octet-stream");
                                request.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                                request.AddHeader("OrganisationCode", "ALP18980121");
                                //request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                                request.AddParameter("application/octet-stream", HttpContext.Session.GetString("fileContent"), RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<SignatureExtractApi>(response.Content);
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_Signature);
                                    if (imagePath != "")
                                    {
                                        bytesImage = System.IO.File.ReadAllBytes(Path.GetFullPath(imagePath));
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                        mno = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            objFinalDoc.DocName = fname;
                            objFinalDoc.DocMainType = DocMainType;
                            objFinalDoc.DocCategoryCode = ProofOfIdCode;
                            objFinalDoc.Source = "Upload";
                            objFinalDoc.Faceext = def;
                            objFinalDoc.Signature = mno;
                            objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));
                            string DocumentsDetails1 = JsonConvert.SerializeObject(objFinalDoc);
                            var config1 = new MapperConfiguration(cfg => { cfg.CreateMap<ClsDocDetails, ClsDocumentDetails>(); });
                            IMapper mapper1 = config1.CreateMapper();
                            ClsDocumentDetails objResult1 = mapper1.Map<ClsDocDetails, ClsDocumentDetails>(objFinalDoc);
                            isInserted = objDetails.Database.ExecuteSqlRaw($"USP_AddDocuments {objResult1}");                        
                        }
                        else if (file.FileName.Contains(".pdf"))
                        {
                            string fname1;
                            fname1 = file.FileName;
                            var pathString = Path.Combine(Directory.GetCurrentDirectory(), "Uploads\\");
                            BinaryReader reader = new BinaryReader(file.OpenReadStream());
                            eibytes = reader.ReadBytes((int)file.Length);
                            string pat1 = Convert.ToBase64String(eibytes);
                            byte[] pat2 = System.Convert.FromBase64String(pat1);
                            Spire.Pdf.PdfDocument document = new Spire.Pdf.PdfDocument(eibytes);
                            for (int k = 0; k < document.Pages.Count; k++)
                            {
                                System.Drawing.Image image1 = document.SaveAsImage(k, 96, 96);
                                image1.Save(string.Format(pathString + Path.GetFileNameWithoutExtension(fname1) + ".jpg", k), System.Drawing.Imaging.ImageFormat.Png);
                                string jd = image1.ToString();
                            }
                            string fname2 = Path.GetFileNameWithoutExtension(fname1) + ".jpg";
                            string mmm = Path.Combine(pathString, fname2);
                            byte[] fileContent = System.IO.File.ReadAllBytes(mmm);
                            HttpContext.Session.SetString("eibyte", Convert.ToBase64String(eibytes));
                            byte[] newbyte = eibytes;
                            try
                            {                             
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentClassificationpdf?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "application/pdf");
                                request.AddParameter("application/pdf", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);                              
                                var aaa = response.Content;
                                dynamic output = JsonConvert.DeserializeObject(aaa);
                                dynamic output2 = JsonConvert.DeserializeObject(output);
                                string output3 = (output2.StatusCode);
                                string output4 = (output2.Message);
                                string output5 = (output2.DocumentName);
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    AadharMaskDetails.Add("StatusCode", output3);
                                    AadharMaskDetails.Add("Message", output4);
                                    AadharMaskDetails.Add("DocumentName", output5);
                                    switch (output5)
                                    {
                                        case "1":
                                            AadharMaskDetails.Add("DocumentType", "aadhar card");
                                            msgs = "Aadhaar Card Identified & Uploaded Successfully ";

                                            break;
                                        case "2":
                                            AadharMaskDetails.Add("DocumentType", "pan card");
                                            msgs = "Pan Card Identified & Uploaded Successfully ";
                                            break;
                                        case "3":
                                            AadharMaskDetails.Add("DocumentType", "Voter id");
                                            msgs = "Voter id uploaded";
                                            break;
                                        case "4":
                                            AadharMaskDetails.Add("DocumentType", "Driving licenses");
                                            msgs = "Driving licenses uploaded";
                                            break;
                                        case "5":
                                            AadharMaskDetails.Add("DocumentType", "Pass port");
                                            msgs = "Pass port uploaded";
                                            break;
                                        case "6":
                                            AadharMaskDetails.Add("DocumentType", "Invalid Document");
                                            msgs = "Invalid Document ";
                                            break;
                                        default:
                                            AadharMaskDetails.Add("DocumentType", "Document Not Detected");
                                            msgs = "Document Not Detected ";
                                            break;
                                    }
                                    switch (output3)
                                    {
                                        case "200":
                                            AadharMaskDetails.Add("ErrorMgs", "Success");
                                            break;
                                        case "300":
                                            AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                            msgs = "Missing AUTH-Headertoken";
                                            break;
                                        case "301":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                            msgs = "Invalid Content-Type";
                                            break;
                                        case "400":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                            msgs = "Invalid Image/Document";
                                            break;
                                        case "401":
                                            AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                            msgs = "Invalid image file for Masking";
                                            break;
                                        case "500":
                                            AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                            msgs = "Unsupported media type";
                                            break;
                                        case "501":
                                            AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                            msgs = "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.";
                                            break;
                                        case "502":
                                            AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                            msgs = "The input image is too large. It should not be larger than 4MB.";
                                            break;
                                        case "503":
                                            AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                            msgs = "Bad request image forma";
                                            break;
                                        case "504":
                                            AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                            msgs = "Internal server erro";
                                            break;
                                        case "505":
                                            AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                            msgs = "Exception message";
                                            break;
                                        default:
                                            AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                            msgs = "Some exception occured";
                                            break;
                                    }
                                    if (output5 == "1")
                                    {
                                        abcd = eibytes;
                                        // var client1 = new RestClient("https://alphafinsoftwebserviceapidevelopment.azurewebsites.net/AlphaFinsoftUser/DocumentMaskingData"); azureapi url
                                        var client1 = new RestClient("https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentExtractionPdf");
                                        client1.Timeout = -1;
                                        var request1 = new RestRequest(Method.POST);
                                        request1.AddHeader("Content-Type", "application/octet-stream");
                                        request1.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                                        request1.AddHeader("OrganisationCode", "ALP18980121");
                                        request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                                        request1.AddParameter("application/octet-stream", abcd, RestSharp.ParameterType.RequestBody);
                                        IRestResponse response1 = client1.Execute(request1);
                                        string Response3 = response1.Content.Replace("[", "");
                                        string Response4 = Response3.Replace("]", "");
                                        var jObject = JObject.Parse(Response4);
                                        string someType2 = jObject.GetValue("documentdata").ToString();
                                        string Response5 = someType2.Replace("{", "").Replace(@"""", "");
                                        string Response6 = Response5.Replace("}", "");
                                        var jsonSendResponse = Response6.Split(',');
                                        var Response8 = jsonSendResponse[227];
                                        var jsonSendResponse1 = Response8.Split(':');
                                        var Image1 = jsonSendResponse1[1];
                                        Dictionary<string, string> AadharMaskDetails1 = new Dictionary<string, string>();
                                        if (response.StatusCode.ToString() == "OK")
                                        {
                                            var obj1 = JsonConvert.DeserializeObject<DocumentExtractionPdf>(someType2);
                                            AadharMaskDetails1.Add("Masked_Image", obj1.Masked_Image);
                                            AadharMaskDetails1.Add("customer_full_name", obj1.customer_full_name);
                                            AadharMaskDetails1.Add("customer_name_initial", obj1.customer_name_initial);
                                            AadharMaskDetails1.Add("customer_fname", obj1.customer_fname);
                                            AadharMaskDetails1.Add("customer_mname ", obj1.customer_mname);
                                            AadharMaskDetails1.Add("customer_lname", obj1.customer_lname);
                                            AadharMaskDetails1.Add("customer_father_name", obj1.customer_father_name);
                                            AadharMaskDetails1.Add("customer_relation_type", obj1.customer_relation_type);
                                            AadharMaskDetails1.Add("customer_gender", obj1.customer_gender);
                                            AadharMaskDetails1.Add("customer_dob", obj1.customer_dob);
                                            AadharMaskDetails.Add("customer_document_id", obj1.customer_document_id);
                                            AadharMaskDetails.Add("customer_document_address", obj1.customer_document_address);
                                            AadharMaskDetails.Add("customer_document_image", obj1.customer_document_image);
                                            if (imagePath != null)
                                            {
                                                bytesImage = eibytes;
                                            }
                                            else
                                            {
                                                byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                                xyz = bytesImage1;
                                            }
                                            switch (output3)
                                            {
                                                case "200":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Success");
                                                    break;
                                                case "300":
                                                    AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
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
                                                    AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                    break;
                                                case "505":
                                                    AadharMaskDetails1.Add("ErrorMgs", "Exception message");
                                                    break;
                                                default:
                                                    AadharMaskDetails1.Add("ErrorMgs", "Some exception occured");
                                                    break;
                                            }
                                        }
                                        Console.WriteLine(response1.Content);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                error_log.WriteErrorLog(ex.ToString());
                                return Json("Exception");
                            }
                            objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                            if (msgs == "Aadhaar Card Identified & Uploaded Successfully ")
                            {
                                objFinalDoc.DocDetails = xyz;
                            }
                            else
                            {
                                objFinalDoc.DocDetails = fileContent;
                            }
                            if (files.Length == 1)
                            {
                                objFinalDoc.documentCategory = POItype + " Page" + j;
                            }
                            else
                            {
                                if (ddl_idProof == "Signature")
                                {
                                    objFinalDoc.documentCategory = ddl_idProof;
                                }
                                else
                                {
                                    objFinalDoc.documentCategory = POItype;
                                }
                            }
                            if (msgs == "Aadhaar Card Identified & Uploaded Successfully " || msgs == "Pan Card Identified & Uploaded Successfully " || msgs == "Voter id uploaded" || msgs == "Driving licenses uploaded" || msgs == "Pass port uploaded")

                            {                              
                                var client = new RestClient(" https://api.indofinnet.com/APIGatway/AlphaFinsoftUser/DocumentFaceExtraction");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("Content-Type", "application/octet-stream");
                                request.AddHeader("GUID", "e2e5f02b-a67d-416d-a4ab-091172ee3207");
                                request.AddHeader("OrganisationCode", "COS75431521");
                                //request.AddHeader("Cookie", "ARRAffinity=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f; ARRAffinitySameSite=92ca53ad8db4fbb93d4d3b7d8ab54dcf8ffecb2d731f25b0e91ad575d7534c3f");
                                request.AddParameter("application/octet-stream", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<FaceExtractionAPI>(response.Content);
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_face);
                                    if (imagePath != null)
                                    {
                                        bytesImage = eibytes;
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                        def = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            if (msgs == "Pan Card Identified & Uploaded Successfully " || msgs == "Driving licenses uploaded" || msgs == "Pass port uploaded")
                            {
                                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentSignatureExtraction?OrgID=Alpha01&ApiKey=AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("ApiKey", "AIphayARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                request.AddHeader("Content-Type", "image/jpeg");
                                request.AddParameter("image/jpeg", eibytes, RestSharp.ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                                Console.WriteLine(response.Content);
                                string Response1 = response.Content.Replace("{", "").Replace(@"""", "");
                                string Response2 = Response1.Replace("}", "");
                                var jsonSendResponse = Response2.Split(',');
                                var Response3 = jsonSendResponse[3];
                                var jsonSendResponse1 = Response3.Split(':');
                                var Image1 = jsonSendResponse1[1];
                                Dictionary<string, string> AadharMaskDetails = new Dictionary<string, string>();
                                if (response.StatusCode.ToString() == "OK")
                                {
                                    var obj = JsonConvert.DeserializeObject<SignatureExtractApi>(JsonConvert.DeserializeObject<string>(response.Content));
                                    AadharMaskDetails.Add("StatusCode", obj.StatusCode);
                                    AadharMaskDetails.Add("Message", obj.Message);
                                    AadharMaskDetails.Add("probability", obj.probability);
                                    AadharMaskDetails.Add("Extacted_Signature", obj.Extacted_Signature);
                                    if (imagePath == null)
                                    {
                                        bytesImage = eibytes;
                                    }
                                    else
                                    {
                                        byte[] bytesImage1 = Convert.FromBase64String(Image1);
                                        mno = bytesImage1;
                                        switch (obj.StatusCode)
                                        {
                                            case "200":
                                                AadharMaskDetails.Add("ErrorMgs", "Success");
                                                break;
                                            case "300":
                                                AadharMaskDetails.Add("ErrorMgs", "Missing AUTH-Headertoken");
                                                break;
                                            case "301":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Content-Type");
                                                break;
                                            case "400":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid Image/Document");
                                                break;
                                            case "401":
                                                AadharMaskDetails.Add("ErrorMgs", "Invalid image file for Masking");
                                                break;
                                            case "500":
                                                AadharMaskDetails.Add("ErrorMgs", "Unsupported media type");
                                                break;
                                            case "501":
                                                AadharMaskDetails.Add("ErrorMgs", "Image format unsupported. Supported formats include JPEG, PNG,TIFF and JPG.");
                                                break;
                                            case "502":
                                                AadharMaskDetails.Add("ErrorMgs", "The input image is too large. It should not be larger than 4MB.");
                                                break;
                                            case "503":
                                                AadharMaskDetails.Add("ErrorMgs", "Bad request image format");
                                                break;
                                            case "504":
                                                AadharMaskDetails.Add("ErrorMgs", "Internal server error");
                                                break;
                                            case "505":
                                                AadharMaskDetails.Add("ErrorMgs", "Exception message");
                                                break;
                                            default:
                                                AadharMaskDetails.Add("ErrorMgs", "Some exception occured");
                                                break;
                                        }
                                    }
                                }
                            }
                            objFinalDoc.DocName = fname1;
                            extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                            objFinalDoc.CustomerDetailId = (Convert.ToInt64(HttpContext.Session.GetString("PersonalId")));
                            var iva = "";
                            string conn21 = _connectionString;
                            using (SqlConnection connection21 = new SqlConnection(conn21))
                            {
                                SqlCommand cmd21 = new SqlCommand("USP_GetDocumentType", connection21);
                                cmd21.CommandType = CommandType.StoredProcedure;
                                cmd21.Parameters.AddWithValue("@docType", ("." + extension));
                                connection21.Open();
                                SqlDataReader reader21 = cmd21.ExecuteReader();
                                if (reader21.Read())
                                {
                                    iva = reader21["documentTypeId"].ToString();
                                }
                            }
                            objFinalDoc.DocType1 = iva;
                            objFinalDoc.DocDetails = xyz;
                            objFinalDoc.DocMainType = DocMainType;
                            objFinalDoc.DocCategoryCode = ProofOfIdCode;
                            objFinalDoc.Source = "Upload";
                            objFinalDoc.Faceext = def;
                            objFinalDoc.Signature = mno;
                            objFinalDoc.DocumentId = Convert.ToString(HttpContext.Session.GetString("PersonalId"));  
                            string conn22 = _connectionString;
                            using (SqlConnection connection22 = new SqlConnection(conn22))
                            {
                                SqlCommand cmd22 = new SqlCommand("USP_AddDocumentsFace", connection22);
                                cmd22.CommandType = CommandType.StoredProcedure;
                                cmd22.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                                if (img != null)
                                {
                                    cmd22.Parameters.AddWithValue("@document", img);
                                }
                                else
                                {
                                    cmd22.Parameters.AddWithValue("@document", objFinalDoc.DocDetails);//eibytes
                                }
                                cmd22.Parameters.AddWithValue("@docName", fname1);
                                cmd22.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocCategoryCode);
                                cmd22.Parameters.AddWithValue("@docTypeId", objFinalDoc.DocType1);
                                cmd22.Parameters.AddWithValue("@docMainType", objFinalDoc.DocMainType);
                                cmd22.Parameters.AddWithValue("@createdBy", "");
                                cmd22.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                                cmd22.Parameters.AddWithValue("@DocumentIdDate", null);
                                cmd22.Parameters.AddWithValue("@Latitude_Longitude", "");
                                cmd22.Parameters.AddWithValue("@documentCategory", "");
                                cmd22.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                                cmd22.Parameters.AddWithValue("@Faceext", objFinalDoc.Faceext);
                                cmd22.Parameters.AddWithValue("@Signature", signatureexract);
                                connection22.Open();
                                isInserted = cmd22.ExecuteNonQuery();
                                connection22.Close();
                            }
                        }
                    }
                    return Json(msgs);
                }
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex);
            }
        }
        public async Task<ActionResult> UploadFiles1(string ddl_idProof, string DocMainType, string imagePath)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ClsDocumentDetails objdoc = new ClsDocumentDetails();
                ClsDocDetails objFinalDoc = new ClsDocDetails();
                objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                byte[] def1 = null;
                byte[] ExtrctSign = null;
                byte[] bytes = null;
                byte[] POAImage = null;
                string Category = null;
                string filetype = null;
                string Photo = null;
                IFormFile file = Request.Form.Files[0];
                string fname;
                int j = 0;
                if (file.FileName.Contains(".jpg"))
                {
                    fname = file.FileName;
                    var pathString = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                    BinaryReader reader = new BinaryReader(file.OpenReadStream());
                    bytes = reader.ReadBytes((int)file.Length);
                    //HttpContext.Session.SetString("eibyte", "eibytes");
                    byte[] newbyte = bytes;
                }
                var client = new RestClient("https://apigateway.indofinnet.com/api/DocumentClassificationData?OrgID=IndoFin007");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                request.AddHeader("Content-Type", "image/jpeg");
                request.AddParameter("image/jpeg", bytes, RestSharp.ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);
                dynamic res21 = JsonConvert.DeserializeObject(response.Content);
                var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(res21);
                if (ddl_idProof == "Aadhaar/UID No")
                {
                    if (obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "4" || obj.DocumentType == "5")
                    {
                        msg = "Sorry...! This Document cannot be used as Aadhaar/UID No. Please select the appropriate document";
                        return Json(msg);
                    }
                }
                if (ddl_idProof == "Pan Card ")
                {
                    if (obj.DocumentType == "1" || obj.DocumentType == "3" || obj.DocumentType == "4" || obj.DocumentType == "5")
                    {
                        msg = "Sorry...! This Document cannot be used as Pan Card. Please select the appropriate document";
                        return Json(msg);
                    }
                }
                if (ddl_idProof == "Driving License ")
                {
                    if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "5")
                    {
                        msg = "Sorry...! This Document cannot be used as Driving License. Please select the appropriate document";
                        return Json(msg);
                    }
                }
                if (ddl_idProof == "Passport ")
                {
                    if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "4")
                    {
                        msg = "Sorry...! This Document cannot be used as Passport. Please select the appropriate document";
                        return Json(msg);
                    }
                }
                if (ddl_idProof == "Voter Identity Card ")
                {
                    if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "4" || obj.DocumentType == "5")
                    {
                        msg = "Sorry...! This Document cannot be used as Voter Identity Card. Please select the appropriate document";
                        return Json(msg);
                    }
                }
                if (ddl_idProof == "Leave and License Agreement")
                {
                    if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "4" || obj.DocumentType == "5")
                    {
                        msg = "Sorry...! This Document cannot be used as Leave and License Agreement. Please select the appropriate document";
                        return Json(msg);
                    }
                    IFormFile file1 = Request.Form.Files[0];
                    byte[] eibytes1 = null;
                    BinaryReader reader = new BinaryReader(file1.OpenReadStream());
                    eibytes1 = reader.ReadBytes((int)file1.Length);
                    string conn1 = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(conn1))                    {

                        SqlCommand cmd2 = new SqlCommand("USP_AddDocuments", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                        MemoryStream cmpStream = new MemoryStream();
                        GZipStream hgs = new GZipStream(cmpStream, CompressionMode.Compress);
                        hgs.Write(eibytes1, 0, eibytes1.Length);
                        byte[] DocImg = cmpStream.ToArray();
                        cmd2.Parameters.AddWithValue("@document", eibytes1);

                        cmd2.Parameters.AddWithValue("@docName", "EL/LL.jpg");
                        cmd2.Parameters.AddWithValue("@documentCategoryCode", "6");
                        cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@docMainType", "EL/LL");
                        cmd2.Parameters.AddWithValue("@createdBy", "");
                        cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                        cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                        cmd2.Parameters.AddWithValue("@documentCategory", ddl_idProof);
                        cmd2.Parameters.AddWithValue("@Source", "Upload");
                        cmd2.Parameters.AddWithValue("@Prediction", "");
                        if (DocMainType == "DOCPOI")
                        {
                            cmd2.Parameters.AddWithValue("@doctypecode", DocMainType);
                        }
                        else
                        {
                            cmd2.Parameters.AddWithValue("@doctypecodeforadd", DocMainType);
                        }
                        connection2.Open();
                        cmd2.ExecuteNonQuery();
                        connection2.Close();
                        msg = "Doccument Uploaded Successfully";
                        return Json(msg);
                    }

                }
                if (ddl_idProof == "Signature")
                {
                    if (obj.DocumentType == "1" || obj.DocumentType == "2" || obj.DocumentType == "3" || obj.DocumentType == "4" || obj.DocumentType == "5")
                    {
                        msg = "Sorry...! This Document cannot be used as Signature.";
                        return Json(msg);
                    }
                    string fname1;
                    IFormFile file1 = Request.Form.Files[0];
                    fname1 = file1.FileName;
                    byte[] eibytes1 = null;
                    BinaryReader reader = new BinaryReader(file1.OpenReadStream());
                    eibytes1 = reader.ReadBytes((int)file1.Length);
                    string conn1 = _connectionString;
                    using (SqlConnection connection2 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd2 = new SqlCommand("USP_AddDocumentsFace", connection2);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@document", eibytes1);
                        cmd2.Parameters.AddWithValue("@docName", "Sign.jpg");
                        cmd2.Parameters.AddWithValue("@documentCategoryCode", "8");
                        cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@docMainType", "SI");
                        cmd2.Parameters.AddWithValue("@createdBy", "");
                        cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                        cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                        cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                        cmd2.Parameters.AddWithValue("@documentCategory", ddl_idProof);
                        cmd2.Parameters.AddWithValue("@Source", "Upload");
                        cmd2.Parameters.AddWithValue("@Faceext", null);
                        cmd2.Parameters.AddWithValue("@Signature", null);
                        connection2.Open();
                        isInserted = cmd2.ExecuteNonQuery();
                        connection2.Close();
                        msg = "Signature Uploaded Successfully";
                        return Json(msg);
                    }
                }
                if (DocMainType == "DOCPOA")
                {
                    if (obj.DocumentType == "2")
                    {
                        msg = "Sorry...! PAN card cannot be used as an address document. Please select the appropriate document";
                        return Json(msg);
                    }


                }
                // var obj = JsonConvert.DeserializeObject<DocumentClassificationApi>(response.Content);
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
                    var client1 = new RestClient("https://apigateway.indofinnet.com/api/DocumentMaskingData?OrgID=IndoFin007");
                    client1.Timeout = -1;
                    var request1 = new RestRequest(Method.POST);
                    request1.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                    request1.AddHeader("Content-Type", "image/jpeg");
                    request1.AddParameter("image/jpeg", bytes, RestSharp.ParameterType.RequestBody);
                    IRestResponse response1 = client1.Execute(request1);
                    Console.WriteLine(response1.Content);
                    string Response2 = response1.Content.Replace("{", "").Replace(@"""", "");
                    string Response3 = Response2.Replace("}", "");

                    var jsonSendResponse = Response3.Split(',');
                    var Dtype = jsonSendResponse[1].Split(':', '\\')[4];
                    var Response5 = jsonSendResponse[2];
                    var jsonSendResponse1 = Response5.Split(':');
                    var Image1 = jsonSendResponse1[1];
                    Image1 = Image1.Replace(@"\", "");
                    if (Dtype == "AADHAR CARD")
                    {
                        objFinalDoc.DocCategoryCode = "1";
                        objFinalDoc.DocMainType = "CA";
                        Category = "Aadhar Card";
                        filetype = (Category + "." + "jpg");

                    }

                    POAImage = Convert.FromBase64String(Image1);

                }
                if (obj.DocumentType == "4")
                {
                    objFinalDoc.DocCategoryCode = "4";
                    objFinalDoc.DocMainType = "DL";
                    Category = "Driving License";
                    filetype = (Category + "." + "jpg");
                    POAImage = bytes;// Convert.FromBase64String(Photo);

                }
                if (obj.DocumentType == "2")
                {
                    objFinalDoc.DocCategoryCode = "2";
                    objFinalDoc.DocMainType = "I";
                    Category = "Pan Card";
                    filetype = (Category + "." + "jpg");
                    POAImage = bytes;// Convert.FromBase64String(Photo);
                }
                if (obj.DocumentType == "3")
                {
                    objFinalDoc.DocCategoryCode = "3";
                    objFinalDoc.DocMainType = "VI";
                    Category = "Voter id";
                    filetype = (Category + "." + "jpg");
                    POAImage = bytes;// Convert.FromBase64String(Photo);

                }
                if (obj.DocumentType == "5")
                {
                    objFinalDoc.DocCategoryCode = "5";
                    objFinalDoc.DocMainType = "PP";
                    Category = "Passport";
                    filetype = (Category + "." + "jpg");
                    POAImage = bytes;// Convert.FromBase64String(Photo);

                }
                objFinalDoc.CustomerDetailId = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));
                if (msg == "Aadhaar Card Identified & Uploaded Successfully " || msg == "Pan Card Identified & Uploaded Successfully " || msg == "Voter id Identified & Uploaded Successfully" || msg == "Driving license Identified & Uploaded Successfully" || msg == "Pass port Identified & Uploaded Successfully")
                {
                    //*****Data Extraction*******//

                    var client12 = new RestClient("https://apigateway.indofinnet.com/api/DataExtraction?OrgID=IndoFin007");
                    client12.Timeout = -1;
                    var request12 = new RestRequest(Method.POST);
                    request12.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                    request12.AddHeader("Content-Type", "image/jpg");
                    request12.AddParameter("image/jpg", POAImage, ParameterType.RequestBody);
                    IRestResponse response12 = client12.Execute(request12);
                    var res = response12.Content;
                    var result = JsonConvert.DeserializeObject<DocumentExtractionAPI>(JsonConvert.DeserializeObject<string>(response12.Content));

                    var knor = Convert.ToString(result.StatusCode);
                    string conn1 = _connectionString;
                    using (SqlConnection connection12 = new SqlConnection(conn1))
                    {
                        SqlCommand cmd12 = new SqlCommand("USP_InsertAIDocumentExtraction1", connection12);
                        cmd12.CommandType = CommandType.StoredProcedure;
                        cmd12.Parameters.AddWithValue("@Customerid", objFinalDoc.CustomerDetailId);
                        cmd12.Parameters.AddWithValue("@StatusCode", result.StatusCode);
                        cmd12.Parameters.AddWithValue("@CardName", result.Card_Name);
                        cmd12.Parameters.AddWithValue("@Documentid", result.customer_document_id);
                        cmd12.Parameters.AddWithValue("@fullname", result.customer_full_name);
                        cmd12.Parameters.AddWithValue("@dob", result.customer_dob);
                        cmd12.Parameters.AddWithValue("@gender", result.customer_gender);
                        cmd12.Parameters.AddWithValue("@relationtype", result.customer_relation_type);
                        cmd12.Parameters.AddWithValue("@initialname", result.customer_name_initial);
                        cmd12.Parameters.AddWithValue("@fname", result.customer_fname);
                        cmd12.Parameters.AddWithValue("@mname", result.customer_mname);
                        cmd12.Parameters.AddWithValue("@lname", result.customer_lname);
                        connection12.Open();
                        cmd12.ExecuteNonQuery();
                        connection12.Close();
                        var msg12 = "Data Extracted";
                        ViewBag.msg12 = msg12;


                    }

                    //###########################//
                    //***Face Extract ***//
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

                        if (imagePath == null)
                        {
                            //bytesImage = System.IO.File.ReadAllBytes((imagePath));
                            bytesImage = bytes;
                        }
                        else
                        {
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

                            //if (imagePath != "")
                            if (imagePath == null)
                            {
                                // bytesImage = System.IO.File.ReadAllBytes(Path.GetFullPath(imagePath));
                                bytesImage = bytes;
                            }
                            else
                            {
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
                    }
                    //*********************//
                }
                //##############//
                objFinalDoc.DocDetails = POAImage;
                //objFinalDoc.documentCategory = POItype;
                objFinalDoc.DocName = "POACamImage";
                extension = objFinalDoc.DocName.Split('.').LastOrDefault();
                //objFinalDoc.DocCategoryCode = ddl_idProof;
                objFinalDoc.Source = "Upload";
                objFinalDoc.Faceext = def1;
                objFinalDoc.Signature = ExtrctSign;
                string conn = _connectionString;
                using (SqlConnection connection2 = new SqlConnection(conn))
                {
                    SqlCommand cmd2 = new SqlCommand("USP_AddDocuments", connection2);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                    MemoryStream cmpStream = new MemoryStream();
                    GZipStream hgs = new GZipStream(cmpStream, CompressionMode.Compress);
                    hgs.Write(POAImage, 0, POAImage.Length);
                    byte[] DocImg = cmpStream.ToArray();
                    cmd2.Parameters.AddWithValue("@document", DocImg);
                    //cmd2.Parameters.AddWithValue("@document", POAImage);
                    cmd2.Parameters.AddWithValue("@docName", filetype);
                    cmd2.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocCategoryCode);
                    cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@docMainType", objFinalDoc.DocMainType);
                    cmd2.Parameters.AddWithValue("@createdBy", "");
                    cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@DocumentIdDate", "");
                    cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                    cmd2.Parameters.AddWithValue("@documentCategory", Category);
                    cmd2.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                    cmd2.Parameters.AddWithValue("@Prediction", "");
                    if (DocMainType == "DOCPOI")
                    {
                        cmd2.Parameters.AddWithValue("@doctypecode", DocMainType);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@doctypecodeforadd", DocMainType);
                    }
                    connection2.Open();
                    cmd2.ExecuteNonQuery();
                    connection2.Close();

                }
                using (SqlConnection connection2 = new SqlConnection(conn))
                {
                    SqlCommand cmd2 = new SqlCommand("USP_AddDocumentsFace", connection2);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@CustomerDetailId", objFinalDoc.CustomerDetailId);
                    if (img != null)
                    {
                        cmd2.Parameters.AddWithValue("@document", img);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@document", POAImage);
                    }
                    cmd2.Parameters.AddWithValue("@docName", filetype);
                    cmd2.Parameters.AddWithValue("@documentCategoryCode", objFinalDoc.DocCategoryCode);
                    cmd2.Parameters.AddWithValue("@docTypeId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@docMainType", objFinalDoc.DocMainType);
                    cmd2.Parameters.AddWithValue("@createdBy", "");
                    cmd2.Parameters.AddWithValue("@DocumentId", objFinalDoc.CustomerDetailId);
                    cmd2.Parameters.AddWithValue("@DocumentIdDate", null);
                    cmd2.Parameters.AddWithValue("@Latitude_Longitude", "");
                    cmd2.Parameters.AddWithValue("@Source", objFinalDoc.Source);
                    cmd2.Parameters.AddWithValue("@Faceext", faceextract);
                    cmd2.Parameters.AddWithValue("@documentCategory", Category);
                    if (Category == "Pan Card" || Category == "Passport")
                    {
                        cmd2.Parameters.AddWithValue("@Signature", signatureexract);
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@Signature", null);
                    }
                    connection2.Open();
                    isInserted = cmd2.ExecuteNonQuery();
                    connection2.Close();
                }
                return Json(msg);
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json(ex);
            }
        }   
        public ActionResult Masking()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return PartialView("_AadharManualMasking");
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Cropdoc()
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

        public IActionResult GetDocumentsForValidation()
        {
            ErrorLog errorLog = new ErrorLog();
            try
            {
                string proceedwithOCR = Convert.ToString(HttpContext.Session.GetString("proceedwithOCR"));
                string shareAadharNumber = Convert.ToString(HttpContext.Session.GetString("shareAadharNumber"));
                long perosnalid = Convert.ToInt64(HttpContext.Session.GetString("PersonalId"));

                string conn = _connectionString;
                bool sign = true; bool POA = true; bool POI = true;

                var ResforSign = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_GetPOIDocuments {(perosnalid)},{"SI"}").ToList();

                var ResforPOI = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_ValGetPOIDocuments {(perosnalid)},{"IAPVD"},{"DOCPOI"}").ToList();

                var ResforPOA = objDetails.AdmCustomerDocumentsDetails.FromSqlRaw($"USP_ValGetPOIDocuments {(perosnalid)},{"CADL"},{""}{"DOCPOA"}").ToList();

                if (ResforSign.Count == 0)
                {
                    sign = false;
                }
                if (ResforPOI.Count == 0)
                {
                    POI = false;

                }
                if (ResforPOA.Count == 0)
                {
                    POA = false;
                }
                bool[] result = { (POI), (POA), (sign) };

                if (POI == true && POA == true && sign == true)
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


                return Json(result);

            }
            catch (Exception ex)
            {
                errorLog.WriteErrorLog(ex.ToString());
                return Json("Exception");
            }
        }
    }
}
