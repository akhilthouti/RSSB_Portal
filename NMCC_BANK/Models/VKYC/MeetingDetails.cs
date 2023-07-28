using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VKYCWebAPI
{
    public class MeetingDetails
    {
        public int meetingDetailId { get; set; }

        public Guid meetingId { get; set; }

        public bool isPrivate { get; set; }

        public bool isHost { get; set; }

        public bool isGuest { get; set; }

        public Guid sessionId { get; set; }
    }
}
