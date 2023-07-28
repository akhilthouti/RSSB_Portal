using System.Configuration;
using System.Text;
using System.IO;
using System;
using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel;
using Azure.Storage.Blobs.Models;
using Microsoft.WindowsAzure.Storage;
using Azure.Storage.Blobs.Specialized;

namespace INDO_FIN_NET.Models
{
    public class ErrorLog
    {
        public async Task<IActionResult> WriteErrorLog(string LogMessage)
        {
            //var blobClient2 = Container.GetBlockBlobClient("test.txt");
            //BlobDownloadInfo download = blobClient2.Download();
            //var content = download.Content;
            //using (var streamReader = new StreamReader(content))
            //{
            //    while (!streamReader.EndOfStream)
            //    {
            //        var line = await streamReader.ReadLineAsync();
            //        Console.WriteLine(line);
            //    }
            //}


            bool Status = false;
            // string LogDirectory = this.Configuration.GetSection("AppSettings")["Site"];
            string LogDirectory = @"C:\LogFiles\"; //ConfigurationManager.AppSettings["LogDirectory"].ToString();
            string Connection = "DefaultEndpointsProtocol=https;AccountName=alphafileupload;AccountKey=gKAw4e0SU9dk0vmWYghlEmI/tFbUL1RdYKMJghhKWaj+0tEZ0KXnKN3GSjTWsB9QDDIuwVtvaCr7+AStGgmbIQ==;EndpointSuffix=core.windows.net"; //Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            string FileUpload = @"fileupload\SMS_JOGI.txt";//Environment.GetEnvironmentVariable("ContainerName");

            //await blob.UploadAsync(myBlob);


            DateTime CurrentDateTime = DateTime.Now;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            //CheckCreateLogDirectory(LogDirectory);
            string logLine = BuildLogLine(CurrentDateTime, LogMessage);

            //other code
            var connectionstring = "DefaultEndpointsProtocol=https;AccountName=alphafileupload;AccountKey=gKAw4e0SU9dk0vmWYghlEmI/tFbUL1RdYKMJghhKWaj+0tEZ0KXnKN3GSjTWsB9QDDIuwVtvaCr7+AStGgmbIQ==;EndpointSuffix=core.windows.net";

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);


            var containerName = "fileupload";
            var container = new BlobContainerClient(connectionstring, containerName);
            var blobClient4 = container.GetBlockBlobClient("ErrorLog_IndofinetPortal" + CurrentDateTime.ToString());

            using (var ms = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(ms);
                writer.Write(logLine);

                writer.Flush();
                ms.Position = 0;
                await blobClient4.UploadAsync(ms);
            }

            LogDirectory = (LogDirectory + "Log_" + LogFileName(DateTime.Now) + ".txt");






            return new OkObjectResult("file uploaded successfully");
            // return Status;
        }


        private bool CheckCreateLogDirectory(string LogPath)
        {
            bool loggingDirectoryExists = false;
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(LogPath);
            if (oDirectoryInfo.Exists)
            {
                loggingDirectoryExists = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(LogPath);
                    loggingDirectoryExists = true;
                }
                catch
                {
                    // Logging failure
                }
            }
            return loggingDirectoryExists;
        }


        private string BuildLogLine(DateTime CurrentDateTime, string LogMessage)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append(LogFileEntryDateTime(CurrentDateTime));
            loglineStringBuilder.Append(" \t");
            loglineStringBuilder.Append(LogMessage);
            return loglineStringBuilder.ToString();
        }


        public string LogFileEntryDateTime(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd-MM-yyyy HH:mm:ss");
        }


        private string LogFileName(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd_MM_yyyy");
        }
    }
}
