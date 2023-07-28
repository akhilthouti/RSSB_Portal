using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace INDO_FIN_NET.Models.VKYC
{
    public class vkyc
    {
        public class MeetingRes
        {


            public string meetingId { get; set; }
            public string attendees { get; set; }
            public string host { get; set; }
            public string createdAt { get; set; }
        }

    }
}