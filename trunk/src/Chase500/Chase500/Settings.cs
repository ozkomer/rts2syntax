using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace ASCOM.Chase500
{
    [DeviceId("ASCOM.Chase500.Dome", DeviceName = "Chase500 Dome")]
    [System.Configuration.SettingsProvider(typeof(ASCOM.SettingsProvider))]
    internal partial class Settings {}
}
