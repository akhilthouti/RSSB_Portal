using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
//using System.Web.Configuration;

namespace ServiceProvider1.Models.OrgExceptionLogs
{
    public static class PortalException
    {
        public static void InsertPortalException(Exception ex)
        {

            string message = "";
            //message += "----------------------------------------------------Start Exception-----------------------------------------------------------------";
            //message += Environment.NewLine;
            message += string.Format("Time:          {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += string.Format("Message:       {0}      ", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source:        {0}       ", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite:    {0}   ", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += string.Format("InnerException:    {0}   ", ex.InnerException);
            message += Environment.NewLine;
            // message += string.Format("InnerExcep:    {0}   ", ex.InnerException.ToString());
            //  message += Environment.NewLine;
            message += "---------------------------------------------------------------------------------------------------------------------------------------";
            message += Environment.NewLine;
            //string path = @"C:\inetpub\wwwroot\NetBanking\LogBackUp";
            //string path =HttpContext.Current.Server.MapPath("~/HSLExceptionLogs/ErrorLog.txt");
            //string path = @"F:\HSLTrading\PortalErrorLog.txt";
            string path = ConfigurationManager.AppSettings["GrandParent_Key:PortalExceptionLogPath:Child_Key"];

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + DateTime.Today.ToString("dd-MM-yy") + ".txt";   //Text File Name  + "___" + i_REFNO + "___" + 
                if (!System.IO.File.Exists(path))
                {
                    System.IO.File.Create(path).Dispose();
                }

                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            

        }
    }
}