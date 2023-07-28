using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VKYCWebAPI
{
    public class Meeting
    {
        public Guid MeetingId { get; set; }

        public string Attendees { get; set; }

        public string Host { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public string meetingTitle { get; set; }

        public string meetingDescription { get; set; }

        public Boolean IsPrivate { get; set; }

        public string PassCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public string hostPassCode { get; set; }
    }
}
