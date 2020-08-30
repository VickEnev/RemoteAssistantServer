using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controllers;
using System.Threading;
using Connection;
using Mediator.ControllerInterfaces;
using Mediator;
using RemoteAssistantStorage;

namespace RemoteAssistantUI
{
    public partial class RemoteAssistantUI : Form
    {
        private Thread mConnectionThread;

        private ConnectionHandler mConnectionHandler;
        private IOController mIOController;
        private LoginController mLoginController;

        private string mGeneratedPassword;

        public RemoteAssistantUI()
        {
            InitializeComponent();
            InitConnectionHandlerAndControllers();
            InitConfigIfNotExists();
        }

        private void InitConnectionHandlerAndControllers()
        {
            var controllers = InitControllers();
            mConnectionHandler = new ConnectionHandler(controllers);

            mConnectionHandler.OnConnectionStateChanged += ConnectionHandler_OnConnectionStateChanged;
            mConnectionHandler.OnAuthenticationSucessfull += ConnectionHandler_OnAuthenticationSucessfull;

            mConnectionThread = new Thread(() => mConnectionHandler.StartListening());
            mConnectionThread.Start();
        }

        private List<IController> InitControllers()
        {
            mLoginController = new LoginController();
            mIOController = new IOController();            

            mGeneratedPassword = mLoginController.OneTimePassword;

            return new List<IController>() { mLoginController, mIOController };
        }

        private void InitConfigIfNotExists()
        {
            XmlConfigurationController xmlConfigurationController =
                new XmlConfigurationController(GlobalRepository.XmlConfigurationFilePath);

            xmlConfigurationController.CreateFile();
        }

        //////////////////////////////
        // Event Callbacks
        ////////////////////////////// 

        private void RemoteAssistantUI_Load(object sender, EventArgs e)
        {
            edb_OneTimePassword.Text = mGeneratedPassword;
        }

        private void ConnectionHandler_OnAuthenticationSucessfull(MobileDeviceInfo mobileDeviceInfo)
        {
            if (mobileDeviceInfo == null)
                return;

            this.Invoke(new Action(()
                => edb_DeviceInfo.Text = $"{mobileDeviceInfo.DeviceName} - {mobileDeviceInfo.DeviceModel}"));
     
        }

        private void ConnectionHandler_OnConnectionStateChanged(ConnectionStatusEventArgs args)
        {
            this?.Invoke(new Action(() =>
            {
                switch (args.ConnectionState)
                {
                    case Connection.ConnectionState.Disconnected:

                        this.Close();
                        break;

                    case Connection.ConnectionState.Connected:

                        edb_ConnectionStatus.Text = "Connected";
                        edb_ConnectionStatus.ForeColor = Color.Green;

                        break;

                    case Connection.ConnectionState.Waiting:

                        edb_ConnectionStatus.Text = $"Waiting for connection on: {args.AdditionalData}";
                        edb_ConnectionStatus.ForeColor = Color.Black;

                        break;

                    default:
                        break;
                }
            }));
        }

        private void RemoteAssistantUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(mConnectionThread.ThreadState != ThreadState.Stopped)
            {
                mConnectionThread?.Interrupt();
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!XmlConfigurationController.CheckIfConfigFileExists(Mediator.GlobalRepository.XmlConfigurationFilePath))
                return;

            System.Diagnostics.Process.Start("notepad.exe", Mediator.GlobalRepository.XmlConfigurationFilePath);
        }
    }
}
