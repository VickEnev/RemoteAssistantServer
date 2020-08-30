using Mediator;
using RemoteAssistantStorage.DatabaseContext;
using RemoteAssistantStorage.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace RemoteAssistantStorage
{
    public class DatabaseController
    {
        public void WriteToLog(string message)
        {
            using (var database = new RemoteAssistantDatabaseContext())
            {
                LogSystem logSystemTableData = new LogSystem();
                logSystemTableData.Message = message;
                logSystemTableData.MessageDate = DateTime.Now;

                database.LogSystem.Add(logSystemTableData);
                database.SaveChanges();
            }
        }

        public void WriteUserDeviceData(MobileDeviceInfo mobileDeviceInfo, UserSession userSession)
        {
            using (var database = new RemoteAssistantDatabaseContext())
            {
                UserDevice userDevice = new UserDevice();
                userDevice.DeviceModel = mobileDeviceInfo.DeviceModel;
                userDevice.DeviceName = mobileDeviceInfo.DeviceName;
                userDevice.SetUserSession(userSession);

                database.UserDevices.Add(userDevice);
                database.SaveChanges();
            }
        }

        public UserDevice GetUserDeviceByUUID(string base64UUID)
        {
            using (var database = new RemoteAssistantDatabaseContext())
            {
                return database.UserDevices.FirstOrDefault(u => u.Base64LoginGuid.Equals(base64UUID));              
            }
        }

        public void UpdateUserDeviceTimestampByUUID(string base64UUID, DateTime timestamp)
        {
            using (var database = new RemoteAssistantDatabaseContext())
            {
                var userDevice = database.UserDevices.FirstOrDefault(u => u.Base64LoginGuid.Equals(base64UUID));
                userDevice.LoginGuidCreationTimestamp = timestamp;
                database.SaveChanges();
            }
        }

    }
}
