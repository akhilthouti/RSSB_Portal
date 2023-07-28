using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET.Models
{
    public class clsAdminAuthorize
    {
       
        public static bool IsAuthorizeCustDetail(string UserId, string sessionKey)
        {

            // INDOFinNetSerRef.Service1Client objservice = new INDOFinNetSerRef.Service1Client();

            RSSBPRODDbCotext objDetails = new RSSBPRODDbCotext();
            ClsUser objuser = new ClsUser();
            try
            {
                if (UserId == "")
                    return false;
                else
                {

                    var Userdetailsid = objDetails.IndoAdminDetails.FromSqlRaw($"USP_IndoFinNet_AuthorizeAdminDetails {objuser.UserId},{sessionKey}");
                    if (Userdetailsid != null)
                    {
                        objDetails.IndoAdminDetails.FromSqlRaw($"USP_IndoUpdateAdmin_LoginDate_OnEachRequest {(objuser.UserId)}");
                       // objDetails.USP_IndoUpdateAdmin_LoginDate_OnEachRequest(UserId);
                        return true;
                    }
                    else
                    {
                       // HttpContext.Session.Clear();
                        //HttpContext.Current.Session.RemoveAll();
                        //HttpContext.Current.Session.Abandon();

                        return false;
                    }


                }
            }
            catch (Exception ex)
            {


                //HttpContext.Session.Clear();
                //HttpContext.Current.Session.RemoveAll();
                //HttpContext.Current.Session.Abandon();
                return false;
            }
        }

    }
}
