using INDO_FIN_NET.Models.Organisation;
using INDO_FIN_NET.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using ServiceProvider1.Models.OrgExceptionLogs;
using ServiceProvider1.Models;
using System.IO;
using System;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace INDO_FIN_NET.Controllers
{
    public class NEWIPVDetailsController : Controller
    {
        ClsCustIPVDetails objcustIpv = new ClsCustIPVDetails();
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData1;
        public NEWIPVDetailsController(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData1 = iNDO_;
        }
        // GET: IPVDetails
        public ActionResult CustIPVdetails()

        {
            try
            {
                var agentid = HttpContext.Session.GetString("UseID");
                if (agentid == null)
                {
                    ViewBag.CustomerFlow = true;
                }
                else
                {
                    ViewBag.CustomerFlow = false;
                }
                objcustIpv.PersonalId = Convert.ToInt64(HttpContext.Session.GetString("DAEditCustomerdetailId"));
                if (HttpContext.Session.GetString("DAEditCustomerdetailId") != null)
                {
                    ViewBag.AdminFlag = "AdminFlag";
                }
                else
                {
                    ViewBag.PersonalId = HttpContext.Session.GetString("PersonalId");
                }

                ViewBag.MOB = HttpContext.Session.GetString("MobileNo");
                var result = objDetails.AdmFlagMainTains.FromSqlRaw($"USP_GetCust_FormFlag {(objcustIpv.PersonalId)}").AsEnumerable().FirstOrDefault();

                if (result != null)
                {
                    ViewBag.ipvrecordflag = true;//result.IsIpvrecord;

                }
               
                return View();
            }
            catch (Exception ex)
            {
                PortalException.InsertPortalException(ex);
                return View();
            }
        }        
    }
}
