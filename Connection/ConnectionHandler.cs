using Mediator;
using Mediator.ControllerInterfaces;
using System;
using System.Collections.Generic;
using Controllers;

namespace Connection
{
    public class ConnectionHandler
    {

        // todo: make the connection broadcast the commands to all the controllers
        private List<IController> mControllers;
        private AsynchronousSocketListener mAsynchronousSocketListener;

        public bool HasLoggedInUser { get; set; }

        public event ConnectionStateChanged OnConnectionStateChanged;
        public event AuthenticationSucessfull OnAuthenticationSucessfull;

        public ConnectionHandler(List<IController> controllers)
        {
            mControllers = controllers;

            foreach(var controller in mControllers)
            {
                controller.OnSendActionEvent += ConnectionHandler_OnSendActionEvent;
                if(controller is LoginController)
                {
                    ((LoginController)controller).OnAuthenticationSucessfull += ConnectionHandler_OnAuthenticationSucessfull;
                }
            }

            if (mControllers.Count == 0)
                throw new ArgumentException("mControllers.Count == 0");

            mAsynchronousSocketListener = new AsynchronousSocketListener(GlobalRepository._MaxConnectionsBacklog);
            InitAsynchronousSocketListenerEvents();
        }

        public void SendOutputMessageToClient(string data)
        {
            mAsynchronousSocketListener.Send(data);        
        }

        public void StartListening()
        {
            mAsynchronousSocketListener.StartListening();
        }

        private void InitAsynchronousSocketListenerEvents()
        {
            mAsynchronousSocketListener.onConnectionStateChanged += AsynchronousSocketListener_onConnectionStateChanged;
            mAsynchronousSocketListener.OnDataRecieved += AsynchronousSocketListener_OnDataRecieved; 
        }

        //////////////////////////////
        // Event Callbacks
        ////////////////////////////// 

        private void AsynchronousSocketListener_OnUserDisconnected(object sender, EventArgs e)
        {
            HasLoggedInUser = false;
        }

        private void AsynchronousSocketListener_OnDataRecieved(string data)
        {
            if(mControllers == null)
                throw new ArgumentException("mControllers == null");

            if(mControllers.Count == 0)
                throw new ArgumentException("mControllers.Count == 0");

            foreach (var controllers in mControllers)
                controllers?.RecieveCommand(data);
        }

        private void AsynchronousSocketListener_onConnectionStateChanged(ConnectionStatusEventArgs args)
        {
            OnConnectionStateChanged?.Invoke(new ConnectionStatusEventArgs(args));
        }

        private void ConnectionHandler_OnSendActionEvent(string actionCommand)
        {
            this.mAsynchronousSocketListener.Send(actionCommand);
        }

        private void ConnectionHandler_OnAuthenticationSucessfull(MobileDeviceInfo mobileDeviceInfo)
        {
            HasLoggedInUser = true;
            OnAuthenticationSucessfull?.Invoke(mobileDeviceInfo);
        }
    }
}
