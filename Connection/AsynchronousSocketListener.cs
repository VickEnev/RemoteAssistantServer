using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using Mediator;
using Mediator.ControllerInterfaces;


namespace Connection
{
    public enum ConnectionState
    {
        Disconnected,
        Connected,
        Waiting
    }

    public delegate void ConnectionStateChanged(ConnectionStatusEventArgs args);
    public delegate void OnDataRecieved(string data);

    internal class AsynchronousSocketListener
    {
        // State object for reading client data asynchronously  
        private class StateObject
        {
            // Size of receive buffer.  
            public const int BufferSize = 1024;

            // Receive buffer.  
            public byte[] buffer = new byte[BufferSize];

            // Received data string.  
            public StringBuilder sb = new StringBuilder();

            public void ClearBuffer()
            {
                Array.Clear(buffer, 0, buffer.Length);
                sb.Clear();
            }
        }

        // Thread signal.  
        private ManualResetEvent allDone = new ManualResetEvent(false);
        private readonly int Backlog;
        private Socket mHandler;
       
        public event ConnectionStateChanged onConnectionStateChanged;
        public event OnDataRecieved OnDataRecieved;

        public AsynchronousSocketListener(int backlog)
        {
            Backlog = backlog;       
        }

        private void OnConnectionStateChanged(ConnectionState newState, string additionalData = "")
        {
            onConnectionStateChanged?.Invoke(new ConnectionStatusEventArgs(newState, additionalData));
        }

        public void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[1];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8888);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  

            listener.Bind(localEndPoint);
            listener.Listen(Backlog);

            while (true)
            {
                try
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    Debug.WriteLine($"Listening for connection on ip address { ipAddress?.MapToIPv4() }");
                    OnConnectionStateChanged(ConnectionState.Waiting, ipAddress?.MapToIPv4().ToString());

                    // Start an asynchronous socket to listen for connections.              
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }
                catch(Exception)
                {
                    listener?.Close();
                    return;
                }             
            }
        }

        public void Send(string data)
        {
            Debug.WriteLine($"Data sent: {data}");

            data += "\n";

            if (mHandler == null)
                return;

            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            mHandler.BeginSend(byteData, 0, byteData.Length, SocketFlags.None, null, mHandler);
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            Debug.WriteLine("Connection Accepted");

            OnConnectionStateChanged(ConnectionState.Connected);

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            mHandler = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            if (mHandler == null)
                return;

            string content = string.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;

            // Read data from the client socket.
            int bytesRead = mHandler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read
                // more data.  
                content = state.sb.ToString();

                Debug.WriteLine(content);

                if (content.IndexOf(GlobalRepository._EndMessageSymbol) > -1)
                {
                    OnDataRecieved?.Invoke(content);
                    state.ClearBuffer();
                }

                // Not all data received. Get more.  
                mHandler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                
            }
        }
    }
}
