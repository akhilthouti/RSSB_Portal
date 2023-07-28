
using Aspose.Pdf.Operators;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace INDO_FIN_NET.Models
{

    public interface IActiveLogin
    {
        public String OrganisationDetails(HttpContext httpContext, string _connectionString);

    }
    public class CheckLoginStatus : IActiveLogin
    {
        private readonly RSSBPRODDbCotext objDetails;
        public String OrganisationDetails(HttpContext httpContext, string _connectionString)
        {
            ErrorLog error_log = new ErrorLog();
            try
            {
                var res = "";
                var CustomerMobileNo = httpContext.Session.GetString("CustMobileNo");
                var agentid = httpContext.Session.GetString("UseID");
                if (agentid != null)
                {
                    var userid = httpContext.Session.GetString("UseID");
                    var NewSessionId = httpContext.Session.GetString("SessionId");
                    if (userid == null && NewSessionId == null)
                    {
                        res = null;
                    }
                    else
                    {
                        string conn = _connectionString;
                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand cmd = new SqlCommand("USP_ToCheckLoginStatus", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(userid)));
                            cmd.Parameters.AddWithValue("@SessionId", NewSessionId);
                            connection.Open();

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                var result = reader["FLAG"].ToString();
                                if (result == "1")
                                {
                                    res = "Active";
                                }
                                else
                                {

                                    res = "Inactive";
                                }

                            }
                        }

                    }

                    return res;
                }
                else if (CustomerMobileNo != null)
                {

                    var NewSessionId = httpContext.Session.GetString("NewSessionId");
                    if (CustomerMobileNo == null && NewSessionId == null)
                    {
                        res = null;
                    }
                    else
                    {
                        string conn = _connectionString;
                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand cmd = new SqlCommand("USP_Customer_ToCheckLoginStatus", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@MobileNo", CustomerMobileNo);
                            cmd.Parameters.AddWithValue("@SessionId", NewSessionId);
                            connection.Open();

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                var result = reader["FLAG"].ToString();
                                if (result == "1")
                                {
                                    res = "Active";
                                }
                                else
                                {

                                    res = "Inactive";
                                }

                            }
                        }
                    }
                    return res;
                }
                else
                {
                    var userid = httpContext.Session.GetString("UserID");
                    var NewSessionId = httpContext.Session.GetString("SessionId");
                    if (userid == null && NewSessionId == null)
                    {
                        res = null;
                    }
                    else
                    {
                        var userid1 = userid.Split('"');
                        var UseriD = userid1[1];
                        string conn = _connectionString;
                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            SqlCommand cmd = new SqlCommand("USP_ToCheckLoginStatus", connection);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@UserId", (Convert.ToInt64(UseriD)));
                            cmd.Parameters.AddWithValue("@SessionId", NewSessionId);
                            connection.Open();

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                var result = reader["FLAG"].ToString();
                                if (result == "1")
                                {
                                    res = "Active";
                                }
                                else
                                {

                                    res = "Inactive";
                                }

                            }
                        }

                    }

                    return res;
                }
                return res;

            }
            catch (Exception ex)
            {
                error_log.WriteErrorLog(ex.ToString());
                return ("Exception");
            }
        }
    }

}
