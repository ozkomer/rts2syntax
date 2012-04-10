//*** CHECK THIS ProgID ***
var X = new ActiveXObject("ASCOM.ASCOM.OrbitATC02.Focuser");
WScript.Echo("This is " + X.Name + ")");
// You may want to uncomment this...
// X.Connected = true;
X.SetupDialog();
