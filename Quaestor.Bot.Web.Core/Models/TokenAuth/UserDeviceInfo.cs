using System;
using System.Collections.Generic;
using System.Text;

namespace Quaestor.Bot.Models.TokenAuth
{
    public class UserDeviceInfo
    {
        public string DeviceType { get; set; }
        public string DeviceUUID { get; set; }
        public string DeviceFCMToken { get; set; }
    }
}
