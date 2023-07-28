using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDO_FIN_NET.Models.VKYC
{
    public class clsGetToken
    {
        public string resp_code { get; set; }
        public resp_msg resp_msg { get; set; }

    }
    public class resp_msg
    {
        public string rest_token { get; set; }
    }

    public class users_list
    {

        public string name { get; set; }
        public string mobile { get; set; }

        public string email { get; set; }
    }

    public class Generatemetting
    {
        public string title { get; set; }
        public bool is_instant { get; set; }
        public bool send_notification { get; set; }
        public string consent_message { get; set; }
        public int category { get; set; }

        public string notes { get; set; }
        public List<users_list> users_list { get; set; } = new List<users_list>();


    }

    public class MeetingResponse
    {


        public int resp_code { get; set; }
        public string resp_msg { get; set; }
        public string id { get; set; }
        public string channel_id { get; set; }
        public string host_meeting_link { get; set; }

        public Meeting_Participants UserDetails { get; set; } = new Meeting_Participants();
    }
    public class Meeting_Participants
    {
        public string name { get; set; }
        public string mobile { get; set; }
        public string participant_id { get; set; }

        public string url { get; set; }

    }
}