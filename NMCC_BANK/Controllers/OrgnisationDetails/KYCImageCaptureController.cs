using INDO_FIN_NET.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace INDO_FIN_NET.Controllers.OrgnisationDetails
{
    public class KYCImageCaptureController : Controller
    {
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

        public ActionResult OpencameraForDigi()
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

        public ActionResult OpencameraForOCR()
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
    }
}
