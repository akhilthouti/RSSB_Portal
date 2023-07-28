using INDO_FIN_NET.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using Microsoft.EntityFrameworkCore;
using ServiceProvider1.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Text;
using RestSharp;
using OfficeOpenXml;
using LicenseContext = OfficeOpenXml.LicenseContext;
using System.Reflection;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace INDO_FIN_NET.Controllers
{
    public class VerifyBulkPAN : Controller
    {
        private readonly RSSBPRODDbCotext objDetails;
        private readonly INDO_FinNetDbCotext objData;
        private readonly string _connectionString;


        public VerifyBulkPAN(RSSBPRODDbCotext Context, INDO_FinNetDbCotext iNDO_, IConfiguration configuration)
        {
            objDetails = Context;
            objData = iNDO_;
            _connectionString = configuration.GetConnectionString("DefaultConnection2");

        }

        TripleDESImplementation objtriple = new TripleDESImplementation();
        clsAddNewUser objAddUsers = new clsAddNewUser();
        ClsUser objuser = new ClsUser();
        string imgtypePhoto = "";
        string imgtype_POI = "";
        string imgtype_CA = "";
        byte[] dochistory_Photo = null;
        byte[] dochistory_POI = null;
        byte[] dochistory_CA = null;
        long? result;

        [HttpGet]
        public IActionResult CBSUpload()
        {
            //var pass = HttpContext.Session.GetString("Role");
            //ViewBag.Pass = pass;

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CBSUpload(IFormFile formFile)
        {
            if (Request != null)
            {
                try
                {
                    ExcelPackage.LicenseContext = LicenseContext.Commercial;
                    string fname = formFile.FileName;
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    using (var file = formFile.OpenReadStream())
                    {
                        using (var xlPackage = new ExcelPackage(file))
                        {
                            var workSheet = xlPackage.Workbook.Worksheets.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            long count = 1;

                            if (noOfCol >= 13)
                            {
                                for (int i = 2; i <= noOfRow; i++)
                                {
                                    string panno = (workSheet.Cells[i, 3].Value ?? string.Empty).ToString();
                                    string Firstname = (workSheet.Cells[i, 5].Value ?? string.Empty).ToString();
                                    string Middlename = (workSheet.Cells[i, 6].Value ?? string.Empty).ToString();
                                    string Lastname = (workSheet.Cells[i, 7].Value ?? string.Empty).ToString();
                                    if (panno != null)
                                    {
                                        string conn = _connectionString;
                                        using (SqlConnection connection = new SqlConnection(conn))
                                        {
                                            SqlCommand cmd = new SqlCommand("USP_check_pan_existence_for_bulkupload", connection);
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@panno", panno);

                                            connection.Open();
                                            int result = (int)cmd.ExecuteScalar();

                                            if (result == 1)
                                            {
                                                Console.WriteLine("PAN already exists");
                                            }
                                            else
                                            {
                                                var client = new RestClient("https://apigateway.indofinnet.com/api/PanService?OrgID=IndoFin007&PanNo=" + panno);
                                                client.Timeout = -1;
                                                var request = new RestRequest(Method.GET);
                                                request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                                                IRestResponse response = client.Execute(request);
                                                string res = response.Content;
                                                string res1 = res.Replace(@"\", "");
                                                string res12 = res1.Replace(@"\", "");
                                                string res2 = res12.Replace("{", ",");
                                                string res3 = res2.Replace("{", "");
                                                string[] ress = res3.Split('"');
                                                string objName = res3.Split(',')[2];
                                                var NSDL_FirstName = ress[12].ToString().Trim();
                                                var NSDL_MiddleName = ress[16].ToString().Trim();
                                                var NSDL_LastName = ress[20].ToString().Trim();
                                                var FirstNameStatus = string.Equals(NSDL_FirstName.ToLower(), Firstname.ToLower()) ? "Match" : "Does Not Match";
                                                var MiddleNameStatus = string.Equals(NSDL_MiddleName.ToLower(), Middlename.ToLower()) ? "Match" : "Does Not Match";
                                                var LastNameStatus = string.Equals(NSDL_LastName.ToLower(), Lastname.ToLower()) ? "Match" : "Does Not Match";

                                                if (FirstNameStatus == "Match" && MiddleNameStatus == "Match" && LastNameStatus == "Match")
                                                {
                                                    string conn1 = _connectionString;
                                                    using (SqlConnection connection1 = new SqlConnection(conn1))
                                                    {
                                                        SqlCommand cmd1 = new SqlCommand("pan_bulk_upload_data_insert1", connection1);
                                                        cmd1.CommandType = CommandType.StoredProcedure;

                                                        cmd1.Parameters.AddWithValue("@pan_no", panno);
                                                        cmd1.Parameters.AddWithValue("@first_name", NSDL_FirstName);
                                                        cmd1.Parameters.AddWithValue("@middle_name", NSDL_MiddleName);
                                                        cmd1.Parameters.AddWithValue("@last_name", NSDL_LastName);
                                                        cmd1.Parameters.AddWithValue("@status", "verified");
                                                        cmd1.Parameters.AddWithValue("@createdBy", "9987");
                                                        connection1.Open();
                                                        SqlDataReader reader = cmd1.ExecuteReader();
                                                        connection1.Close();
                                                    }
                                                }
                                                else
                                                {
                                                    string conn2 = _connectionString;
                                                    using (SqlConnection connection2 = new SqlConnection(conn2))
                                                    {
                                                        SqlCommand cmd2 = new SqlCommand("pan_bulk_upload_data_insert1", connection2);
                                                        cmd2.CommandType = CommandType.StoredProcedure;

                                                        cmd2.Parameters.AddWithValue("@pan_no", panno);
                                                        cmd2.Parameters.AddWithValue("@first_name", Firstname);
                                                        cmd2.Parameters.AddWithValue("@middle_name", Middlename);
                                                        cmd2.Parameters.AddWithValue("@last_name", Lastname);
                                                        cmd2.Parameters.AddWithValue("@status", "Not-verified");
                                                        cmd2.Parameters.AddWithValue("@createdBy", "9987");
                                                        connection2.Open();
                                                        SqlDataReader reader = cmd2.ExecuteReader();
                                                        connection2.Close();
                                                    }
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
                    string msg = ex.Message;

                }
            }
            return View();
        }

        public async Task<IActionResult> CBSUpload1Async(string all, string Success, string Reject, string FromDate, string ToDate)
        {
            bulkup objmain = new bulkup();
            if (all != "false" && FromDate != null && ToDate != null)
            {
                var result = objDetails.bulkup.FromSqlRaw("pan_bulk_upload_get_all_data @FromDate, @ToDate", new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                DataTable dt = ToDataTable(result);
                string filename = "PANBULKUPLOAD.xlsx";
                string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PANBULKUPLOAD", filename);

                if (System.IO.File.Exists(Filepath))
                {
                    System.IO.File.Delete(Filepath);
                }

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells.LoadFromDataTable(dt, true);

                        var headerRange = worksheet.Cells[1, 1, 1, dt.Columns.Count];
                        headerRange.Style.Font.Bold = true;
                        headerRange.AutoFilter = true;

                        for (var col = 1; col <= dt.Columns.Count; col++)
                        {
                            var excelColumn = worksheet.Column(col);
                            excelColumn.AutoFit();
                        }

                        package.Save();
                    }
                    await System.IO.File.WriteAllBytesAsync(Filepath, stream.ToArray());
                }
                return Json("Success");
            }
            else if (Success != "false" && FromDate != null && ToDate != null)
            {
                var status1 = "verified";
                var result = objDetails.bulkup.FromSqlRaw("pan_bulk_upload_get_data @status1, @FromDate, @ToDate", new SqlParameter("@status1", status1), new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                DataTable dt = ToDataTable(result);

                string filename = "PANBULKUPLOAD.xlsx";
                string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PANBULKUPLOAD", filename);

                if (System.IO.File.Exists(Filepath))
                {
                    System.IO.File.Delete(Filepath);
                }

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells.LoadFromDataTable(dt, true);

                        var headerRange = worksheet.Cells[1, 1, 1, dt.Columns.Count];
                        headerRange.Style.Font.Bold = true;
                        headerRange.AutoFilter = true;

                        for (var col = 1; col <= dt.Columns.Count; col++)
                        {
                            var excelColumn = worksheet.Column(col);
                            excelColumn.AutoFit();
                        }

                        package.Save();
                    }
                    await System.IO.File.WriteAllBytesAsync(Filepath, stream.ToArray());
                }
                return Json("Success");

            }
            else if (Reject != "false" && FromDate != null && ToDate != null)
            {
                var status1 = "Not-verified";
                var result = objDetails.bulkup.FromSqlRaw("pan_bulk_upload_get_data @status1, @FromDate, @ToDate", new SqlParameter("@status1", status1), new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                DataTable dt = ToDataTable(result);
                string filename = "PANBULKUPLOAD.xlsx";
                string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PANBULKUPLOAD", filename);

                if (System.IO.File.Exists(Filepath))
                {
                    System.IO.File.Delete(Filepath);
                }

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells.LoadFromDataTable(dt, true);

                        var headerRange = worksheet.Cells[1, 1, 1, dt.Columns.Count];
                        headerRange.Style.Font.Bold = true;
                        headerRange.AutoFilter = true;

                        for (var col = 1; col <= dt.Columns.Count; col++)
                        {
                            var excelColumn = worksheet.Column(col);
                            excelColumn.AutoFit();
                        }

                        package.Save();
                    }
                    await System.IO.File.WriteAllBytesAsync(Filepath, stream.ToArray());
                }
                return Json("Success");
            }
            else
            {
                return View();
            }
            return View();
        }
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public IActionResult btnDownload_Click()
        {
            string filename = "PANBULKUPLOAD.xlsx";
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\PANBULKUPLOAD", filename);
            byte[] excelfile = System.IO.File.ReadAllBytes(Filepath);
            return File(excelfile, "application/vnd.ms-excel", filename + ".xlsx");
        }

        public object individualpanverification(string PAN, string FNAME, string MNAME, string LNAME)
        {
            var panno = PAN;
            string Firstname = FNAME;
            string Middlename = MNAME;
            string Lastname = LNAME;
            string status = null;
            if (panno != null)
            {
                string conn = _connectionString;
                using (SqlConnection connection = new SqlConnection(conn))
                {
                    SqlCommand cmd = new SqlCommand("USP_check_pan_existence_for_bulkupload", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@panno", panno);

                    connection.Open();
                    int result = (int)cmd.ExecuteScalar();

                    if (result == 1)
                    {
                        status = "PAN already exists";
                    }
                    else
                    {
                        var client = new RestClient("https://apigateway.indofinnet.com/api/PanService?OrgID=IndoFin007&PanNo=" + panno);
                        client.Timeout = -1;
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("ApiKey", "IndoFinyARmQwEtYFzohGwoXp39wZDDaiqds3y6RE");
                        IRestResponse response = client.Execute(request);
                        string res = response.Content;
                        string res1 = res.Replace(@"\", "");
                        string res12 = res1.Replace(@"\", "");
                        string res2 = res12.Replace("{", ",");
                        string res3 = res2.Replace("{", "");
                        string[] ress = res3.Split('"');
                        string objName = res3.Split(',')[2];
                        var NSDL_FirstName = ress[12].ToString().Trim();
                        var NSDL_MiddleName = ress[16].ToString().Trim();
                        var NSDL_LastName = ress[20].ToString().Trim();
                        var FirstNameStatus = string.Equals(NSDL_FirstName.ToLower(), Firstname.ToLower()) ? "Match" : "Does Not Match";
                        var MiddleNameStatus = string.Equals(NSDL_MiddleName.ToLower(), Middlename.ToLower()) ? "Match" : "Does Not Match";
                        var LastNameStatus = string.Equals(NSDL_LastName.ToLower(), Lastname.ToLower()) ? "Match" : "Does Not Match";

                        if (FirstNameStatus == "Match" && MiddleNameStatus == "Match" && LastNameStatus == "Match")
                        {
                            status = "Pan Verified Successfully";
                            string conn1 = _connectionString;
                            using (SqlConnection connection1 = new SqlConnection(conn1))
                            {
                                SqlCommand cmd1 = new SqlCommand("pan_bulk_upload_data_insert1", connection1);
                                //SqlCommand cmd1 = new SqlCommand("pan_bulk_upload_data_insert", connection1);
                                cmd1.CommandType = CommandType.StoredProcedure;

                                cmd1.Parameters.AddWithValue("@pan_no", panno);
                                cmd1.Parameters.AddWithValue("@first_name", NSDL_FirstName);
                                cmd1.Parameters.AddWithValue("@middle_name", NSDL_MiddleName);
                                cmd1.Parameters.AddWithValue("@last_name", NSDL_LastName);
                                cmd1.Parameters.AddWithValue("@status", "verified");
                                cmd1.Parameters.AddWithValue("@createdBy", "9987");
                                connection1.Open();
                                SqlDataReader reader = cmd1.ExecuteReader();
                                connection1.Close();
                            }

                        }
                        else
                        {
                            status = "Pan Not-Verified";
                            string conn2 = _connectionString;
                            using (SqlConnection connection2 = new SqlConnection(conn2))
                            {
                                SqlCommand cmd2 = new SqlCommand("pan_bulk_upload_data_insert1", connection2);
                                //SqlCommand cmd2 = new SqlCommand("pan_bulk_upload_data_insert", connection2);
                                cmd2.CommandType = CommandType.StoredProcedure;

                                cmd2.Parameters.AddWithValue("@pan_no", panno);
                                cmd2.Parameters.AddWithValue("@first_name", Firstname);
                                cmd2.Parameters.AddWithValue("@middle_name", Middlename);
                                cmd2.Parameters.AddWithValue("@last_name", Lastname);
                                cmd2.Parameters.AddWithValue("@status", "Not-verified");
                                cmd2.Parameters.AddWithValue("@createdBy", "9987");
                                connection2.Open();
                                SqlDataReader reader = cmd2.ExecuteReader();
                                connection2.Close();
                            }
                        }
                    }
                }
            }

            return Json(status);

        }

        public async Task<IActionResult> AgentReportExcel(string all, string Pending, string Success, string Reject, string FromDate, string ToDate)
        {
            var CreatedBy = HttpContext.Session.GetString("UseID");

            bulkup objmain = new bulkup();
            if (all != null && FromDate != null && ToDate != null)
            {
                var result = objDetails.bulkup.FromSqlRaw("USP_GetCustomerKycDataEXCEL @CreatedBy @FromDate, @ToDate", new SqlParameter("@CreatedBy", CreatedBy), new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                DataTable dt = ToDataTable(result);
                string filename = "PANBULKUPLOAD.xlsx";
                string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PANBULKUPLOAD", filename);

                if (System.IO.File.Exists(Filepath))
                {
                    System.IO.File.Delete(Filepath);
                }

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells.LoadFromDataTable(dt, true);

                        var headerRange = worksheet.Cells[1, 1, 1, dt.Columns.Count];
                        headerRange.Style.Font.Bold = true;
                        headerRange.AutoFilter = true;

                        for (var col = 1; col <= dt.Columns.Count; col++)
                        {
                            var excelColumn = worksheet.Column(col);
                            excelColumn.AutoFit();
                        }

                        package.Save();
                    }
                    await System.IO.File.WriteAllBytesAsync(Filepath, stream.ToArray());
                }
                return Json("Success");
            }
            else if (Pending != "false" && FromDate != null && ToDate != null)
            {
                //var status1 = "verified";

                //var result = objDetails.AgentPendingExcels.FromSqlRaw("USP_GetCustomerKycDataEXCEL @CreatedBy, @FromDate, @ToDate", new SqlParameter("@CreatedBy", CreatedBy), new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                var result = objDetails.AgentPendingExcels.FromSqlRaw("USP_GetPendingCustomerKycDataEXCEL @CreatedBy, @FromDate, @ToDate", new SqlParameter("@CreatedBy", CreatedBy), new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                DataTable dt = ToDataTable(result);



                string filename = "AgentReport.xlsx";
                string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Report", filename);

                if (System.IO.File.Exists(Filepath))
                {
                    System.IO.File.Delete(Filepath);
                }

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells.LoadFromDataTable(dt, true);

                        var headerRange = worksheet.Cells[1, 1, 1, dt.Columns.Count];
                        headerRange.Style.Font.Bold = true;
                        headerRange.AutoFilter = true;

                        for (var col = 1; col <= dt.Columns.Count; col++)
                        {
                            var excelColumn = worksheet.Column(col);
                            excelColumn.AutoFit();
                        }

                        package.Save();
                    }
                    await System.IO.File.WriteAllBytesAsync(Filepath, stream.ToArray());

                }
                return Json("Success");

            }
            else if (Success != "false" && FromDate != null && ToDate != null)
            {
                //var status1 = "verified";
                var result = objDetails.AgentPendingExcels.FromSqlRaw("USP_GetCustomerKycDataApproveexcel @CreatedBy, @FromDate, @ToDate", new SqlParameter("@CreatedBy", CreatedBy), new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                DataTable dt = ToDataTable(result);

                string filename = "AgentReport.xlsx";
                string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Report", filename);

                if (System.IO.File.Exists(Filepath))
                {
                    System.IO.File.Delete(Filepath);
                }

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells.LoadFromDataTable(dt, true);

                        var headerRange = worksheet.Cells[1, 1, 1, dt.Columns.Count];
                        headerRange.Style.Font.Bold = true;
                        headerRange.AutoFilter = true;

                        for (var col = 1; col <= dt.Columns.Count; col++)
                        {
                            var excelColumn = worksheet.Column(col);
                            excelColumn.AutoFit();
                        }

                        package.Save();
                    }
                    await System.IO.File.WriteAllBytesAsync(Filepath, stream.ToArray());
                }
                return Json("Success");

            }
            else if (Reject != "false" && FromDate != null && ToDate != null)
            {
                //var status1 = "Not-verified";
                var result = objDetails.AgentRejectedExcels.FromSqlRaw("USP_GetRejctedCustomerKycDataEXCEL @CreatedBy, @FromDate, @ToDate", new SqlParameter("@CreatedBy", CreatedBy), new SqlParameter("@FromDate", FromDate), new SqlParameter("@ToDate", ToDate)).ToList();
                DataTable dt = ToDataTable(result);
                string filename = "AgentReport.xlsx";
                string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Report", filename);

                if (System.IO.File.Exists(Filepath))
                {
                    System.IO.File.Delete(Filepath);
                }

                using (var stream = new MemoryStream())
                {
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells.LoadFromDataTable(dt, true);

                        var headerRange = worksheet.Cells[1, 1, 1, dt.Columns.Count];
                        headerRange.Style.Font.Bold = true;
                        headerRange.AutoFilter = true;

                        for (var col = 1; col <= dt.Columns.Count; col++)
                        {
                            var excelColumn = worksheet.Column(col);
                            excelColumn.AutoFit();
                        }

                        package.Save();
                    }
                    await System.IO.File.WriteAllBytesAsync(Filepath, stream.ToArray());
                }
                return Json("Success");
            }
            else
            {
                return View();
            }
            return View();
        }



        public IActionResult DownloadAgentPendingExcel()
        {
            string filename = "AgentReport.xlsx";
            string Filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Report", filename);
            byte[] excelfile = System.IO.File.ReadAllBytes(Filepath);
            return File(excelfile, "application/vnd.ms-excel", filename + ".xlsx");
        }
    }
}
