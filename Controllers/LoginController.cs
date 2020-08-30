using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Mediator;
using Mediator.ControllerInterfaces;
using RemoteAssistantStorage;

namespace Controllers
{
   
    public delegate void AuthenticationSucessfull(MobileDeviceInfo mobileDeviceInfo);

    public class LoginController : IController
    {
        public string OneTimePassword { get; } = RandomPasswordGenerator.Generate(6);

        public event SendActionEvent OnSendActionEvent;
        public event EventHandler OnIncorrectCommandRecieved;

        public event AuthenticationSucessfull OnAuthenticationSucessfull;

        private bool CheckIsPasswordCorrect(string password)
        {
            return OneTimePassword.Equals(password);
        }

        public bool RecieveCommand(string command)
        {
            DatabaseController database = new DatabaseController();

            XmlConfigurationController xmlConfigurationController =
                new XmlConfigurationController(GlobalRepository.XmlConfigurationFilePath);

            bool hasDefaultConfig = false;
            XmlConfiguration config = null;

            if (!xmlConfigurationController.ReadXmlConfigurationFile())
            {
                database.WriteToLog("Can't read from xml configuration");
                config = new XmlConfiguration();
                hasDefaultConfig = true;
            }

            bool isCommandSwallowed = false;

            var commandArray = command.Split(GlobalRepository._Delimiter);
            // Логин с парола
            if (commandArray[0] == GlobalRepository.Actions._Login.ToString())
            {
                if (CheckIsPasswordCorrect(commandArray[1]))
                {
                    Guid userGUIDSession = Guid.NewGuid();                 
                    var deviceInfo = new MobileDeviceInfo()
                    {
                        DeviceName = commandArray[2],
                        DeviceModel = commandArray[3]
                    };

                    database.WriteUserDeviceData(deviceInfo, new UserSession(userGUIDSession));

                    OnSendActionEvent?.Invoke(GetPasswordLoginAuthenticationSuccessMessage(userGUIDSession));
                    OnAuthenticationSucessfull?.Invoke(deviceInfo);
                }
                else
                    OnSendActionEvent?.Invoke(GetLoginAuthenticationFailedMessage());

                isCommandSwallowed = true;
            }
            // Логин с уникален идентификатор
            else if (commandArray[0] == GlobalRepository.Actions._LoginWithUUID.ToString())
            {
                var base64GUID = commandArray[1];

                var userSession = database.GetUserDeviceByUUID(base64GUID);
                if(userSession == null)
                    OnSendActionEvent?.Invoke(GetLoginAuthenticationFailedMessage());

                int UUIDValidityMinutes = hasDefaultConfig ? config.ExpirationTimeInMinutes 
                    : xmlConfigurationController.XmlConfiguration.ExpirationTimeInMinutes;

                // Правим проверка дали времето за валидност на сесията НЕ е минало, ако НЕ е минало ще авторизираме потребителя
                // Ако е минало ще върнем неуспешен логин и ще чакаме потребителя да се автентицира с парола

                var validTimeframeForUUID = userSession.LoginGuidCreationTimestamp.AddMinutes(UUIDValidityMinutes);

                if ( DateTime.Now <= validTimeframeForUUID )
                {
                    database.UpdateUserDeviceTimestampByUUID(base64GUID, DateTime.Now);

                    OnSendActionEvent?.Invoke(GetPasswordLoginAuthenticationSuccessMessage());
                    OnAuthenticationSucessfull?.Invoke(new MobileDeviceInfo() 
                    {
                        DeviceName = userSession.DeviceName,
                        DeviceModel = userSession.DeviceModel
                    });
                }
                else
                    OnSendActionEvent?.Invoke(GetLoginAuthenticationFailedMessage());

                isCommandSwallowed = true;
            }

            return isCommandSwallowed;
        }

        private string GetPasswordLoginAuthenticationSuccessMessage(Guid guid)
        {
            return "" + GlobalRepository.Actions._LoginAuthernticationSuccess
                + GlobalRepository._Delimiter
                + Convert.ToBase64String(guid.ToByteArray())
                + GlobalRepository._Delimiter
                + GlobalRepository._EndMessageSymbol;
        }

        private string GetPasswordLoginAuthenticationSuccessMessage()
        {
            return "" + GlobalRepository.Actions._LoginAuthernticationSuccess
                + GlobalRepository._Delimiter
                + GlobalRepository._EndMessageSymbol;
        }

        private string GetLoginAuthenticationFailedMessage()
        {
            return "" + GlobalRepository.Actions._LoginAuthernticationFailed
                + GlobalRepository._Delimiter
                + GlobalRepository._EndMessageSymbol;
        }
    }
}
