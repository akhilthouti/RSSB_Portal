namespace INDO_FIN_NET.Models
{
    public class UIDDATA
    {


        public string sha { get; set; }
        public string dt { get; set; }

        public string aadhaarNumber { get; set; }
        public string name { get; set; }
        public string uidtoken { get; set; }
        public string photo { get; set; }

        public string dateOfBirth { get; set; }
        public string gender { get; set; }
        public string phone { get; set; }
        public string careOfPerson { get; set; }
        public string landmark { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string subDistrict { get; set; }
        public string house { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string postOfficeName { get; set; }
        public string error { get; set; }
        public string emailId { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        public string Doneby { get; set; }
        public string TXN { get; set; }
        public string TimeStamp { get; set; }



        public string ResXML { get; set; }
        public string UIDAITransactionID { get; set; }
        public string ApplicationID { get; set; }

        public string SECURITY_ALERT { get; set; }






    }
    public class DigiClass
    {

        public string obj { get; set; }

        public string msg { get; set; }

        public string PanNo { get; set; }
    }

    public class GetToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string digilockerid { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string eaadhaar { get; set; }




    }


    public class FileAPI
    {

        public string uri { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }


    }
}
