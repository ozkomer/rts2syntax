using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ASCOM.OrbitATC02
{
    [DeviceId("ASCOM.OrbitATC02.Focuser", DeviceName = "Chase500 Focuser")]
    [System.Configuration.SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings { }
}
