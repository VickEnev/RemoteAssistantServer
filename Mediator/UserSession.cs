using System;
using System.Collections.Generic;
using System.Text;

namespace Mediator
{
    public class UserSession
    {
        public Guid SessionGuid { get; private set; }
        public DateTime LoginTimestamp { get; private set; }

        public UserSession(Guid sessionGuid)
        {
            SessionGuid = sessionGuid;
            LoginTimestamp = DateTime.Now;
        }
    }

}
