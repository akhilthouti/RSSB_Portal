using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceProvider1.Models;
using System;

namespace INDO_FIN_NET.Controllers
{
    public class LoanController : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        TripleDESImplementation ObjTripleDes = new TripleDESImplementation();
        private readonly string _connectionString;
        public ActionResult EmiCalculator()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }
        }
        public ActionResult PersonalLoanQuickEnroll()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                ViewBag.VTypeData = new SelectList(new[] {
                      new { ID = "P1", Value = "Pan" }, new { ID = "A", Value = "Aadhaar" } }, "ID", "Value");
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }
        }
        public ActionResult PersonalLoanDocumentDetails()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ViewBag.proofOfId = new SelectList(new[] {
                      new { ID = "A", Value = "Last 6 Months Bank Statements" },
                    new { ID = "B", Value = "If Any Previous Loan From Other Banks ,then Loan A/C statement For last 1 Year" } }, "ID", "Value");
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }
        }
        public ActionResult HouseLoanQuickEnroll()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {

                ViewBag.VTypeData = new SelectList(new[] {
                      new { ID = "P1", Value = "Pan" }, new { ID = "A", Value = "Aadhaar" } }, "ID", "Value");
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }
        }
        public ActionResult HouseLoanDocumentDetails()
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                ViewBag.proofOfId = new SelectList(new[] {
                      new { ID = "A", Value = "Last 6 Months Bank Statements" },
                    new { ID = "B", Value = "If Any Previous Loan From Other Banks ,then Loan A/C statement For last 1 Year" } }, "ID", "Value");
                return View();
            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return Json("Exception");//, JsonRequestBehavior.AllowGet
            }
        }
    }
}