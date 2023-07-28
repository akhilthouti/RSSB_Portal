using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.IO;
using System.Net;

namespace INDO_FIN_NET.Controllers
{
    public class DataPushToFinacleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult SubmitToFinacle()
        {
            JObject UPIRequest = new JObject
            {
                    { "Code","1180"}
                    
                    };

            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

            HttpWebRequest request = null;
            WebResponse response = null;
            string myURL = "https://indofinpay.com/PROD_UPI/api/UPI/CollectPay"; 
            request = (HttpWebRequest)WebRequest.Create(myURL); // Create a request using a URL that can receive a post. 
            request.Method = "POST";  // Set the Method property of the request to POST.                
            request.ContentType = "application/json";
            // request.Headers.Add("Auth", "AuthRequest");
            StreamWriter writer = new StreamWriter(request.GetRequestStream()); // Wrap the request stream with a text-based writer
            writer.WriteLine(UPIRequest);  // Write the xml text into the stream input xml
            writer.Close();
            Stream dataStream = null;
            response = request.GetResponse(); // Get the response.
            string Response_status = ((HttpWebResponse)response).StatusDescription.ToString(); // Display the status.            
            dataStream = response.GetResponseStream(); // Get the stream containing content returned by the server.            
            StreamReader incomingStreamReader = new StreamReader(dataStream); // Open the stream using a StreamReader for easy access.            
            string responseFromServer = incomingStreamReader.ReadToEnd();// Read the content xml response.
           

            string some = responseFromServer;


            return View();
        }
    }
}
