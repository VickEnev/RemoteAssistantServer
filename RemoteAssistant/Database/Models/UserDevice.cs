using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Mediator;

namespace RemoteAssistantStorage.Models
{
    [Table("USER_DEVICE")]
    public class UserDevice
    {
        ///<summary>
        /// Identity колона
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Base64LoginGuid { get; set; } = "";
        public string DeviceName { get; set; } = "";
        public string DeviceModel { get; set; } = "";
        public DateTime LoginGuidCreationTimestamp { get; set; } = new DateTime();

        public void SetUserSession(UserSession userSession)
        {
            Base64LoginGuid = Convert.ToBase64String(userSession.SessionGuid.ToByteArray());
            LoginGuidCreationTimestamp = userSession.LoginTimestamp;
        }
    }
}
