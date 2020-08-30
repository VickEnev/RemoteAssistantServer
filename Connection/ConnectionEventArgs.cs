using System;
using System.Collections.Generic;
using System.Text;

namespace Connection
{
    public class ConnectionStatusEventArgs
    {
        public ConnectionState ConnectionState { get; set; }
        public string AdditionalData { get; set; }

        public ConnectionStatusEventArgs(ConnectionState connectionState, string additionalData)
        {
            ConnectionState = connectionState;
            AdditionalData = additionalData;
        }

        public ConnectionStatusEventArgs(ConnectionStatusEventArgs copy)
        {
            ConnectionState = copy.ConnectionState;
            AdditionalData = copy.AdditionalData;
        }
    }
}
