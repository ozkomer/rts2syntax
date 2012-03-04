//------------------------------------------------------------------------------
// Este archivo debe estar ubicado en el siguiente path\filename: 
// C:\Program Files\ACP Scheduler\StartupObs.js
//------------------------------------------------------------------------------
//
// StartupObs.js - ACP Scheduler Observatory Shutdown Script
//
// WARNING! THIS SCRIPT MUST RUN HARMLESSLY AND SUCCESSFULLY EVEN IF ALL OF
// THE POWER IS ALREADY ON, ALL PROGRAMS ARE ALREADY RUNNING, AND ALL DEVICES
// ARE ALREADY CONNECTED. IN OTHER WORDS, TEST THIS WITH A COMPLETELY ONLINE
// OBSERVATORY!!!
//
// WARNING! DO NOT OPEN THE DOME IN THIS SCRIPT! THE SCHEDULER OPENS IT IF NEEDED.
//
// Bob Denny        19-Jan-2009 Initial edit
// Bob Denny        23-Apr-2009 DO not open dome here
// Bob Denny        13-Jan-2011 Recognize rotator like shutdown, fix typos, MaxIm 5
// Bob Denny        21-Oct-2011 GEM:685 Do not touch tracking on startup, unsafe
//                  if low hanging roof (causes exception).
//------------------------------------------------------------------------------

var FSO;

function main()
{
    var i;
    
    FSO = new ActiveXObject("Scripting.FileSystemObject");

    var buf = Prefs.LocalUser.DefaultLogDir + "\\Scheduler\\LastStartupObs.log";
    try {
        FSO.DeleteFile(buf);
    } catch(ex) { }
    Console.LogFile = buf;
    Console.Logging = true;

    // =====================================================
    // HERE IS WHERE YOU ADD CODE TO TURN POWER ON AS NEEDED
    // =====================================================
        
        
    // ==================================================================
    // CHANGE PATHS TO PROGRAMS AS NEEDED AND ADD OTHER PROGRAMS YOU NEED
    // ==================================================================
    
    Console.PrintLine("Starting support programs as needed");
    if(!StartProgram("C:\\Program Files\\Diffraction Limited\\MaxIm DL V5\\MaxIm_DL.exe", 1)) { 
        Console.PrintLine("**Failed to start MaxIm");
        Console.Logging = false;
        return;
    }
    if(!StartProgram("C:\\Program Files\\FocusMax\\FocusMax.exe", 1)) {
        Console.PrintLine("**Failed to start FocusMax");
        Console.Logging = false;
        return;
    }
    
    if(FSO.FileExists(ACPApp.Path & "\RotatorInfo.txt"))                    // If there is a rotator, get it going
    {
        // Hand start RotControl so it runs all the time
        if(!StartProgram("C:\\Program Files\\ACP Obs Control\\RotControl.exe", 2)) { // 2 = minimized, focus (typ.)
            Console.PrintLine("**Failed to start RotControl");
            Console.Logging = false;
            return;
        }
        try {
            var R = new ActiveXObject("ACP.Rotator");
            R.Connected = true;
            R = null;
        } catch(ex) {
            Console.PrintLine("**Failed to connect to rotator:");
            Console.PrintLine("  " + ex.message);
            Console.Logging = false;
            return;
        }
    }

    // ==========================================
    // UNCOMMENT IF YOU USE BOLTWOOD CLOUD SENSOR
    // ==========================================
    
//     var C;
//     try {
//         if(!Util.Weather.Available) {
//             Console.PrintLine("Start Clarity and wait for data");
//             C = new ActiveXObject("ClarityII.CloudSensorII");           // Start and wait for Clarity data
//             for(i = 0; i < 20; i++)                                     // Wait for up to 5  minutes
//             {
//                 if(C.DataReady()) {
//                     Util.WeatherConnected = true;                       // Connect ACP to Clarity
//                     break;
//                 }
//                 Util.WaitForMilliseconds(15000);
//             }
//             if(i >= 20)
//                 Console.PrintLine("**Clarity didn't connect for 5 minutes");
//         } else {
//             Console.PrintLine("Weather already connected");
//         }
//     } catch(ex) {
//         Console.PrintLine("Clarity trouble:");
//         Console.PrintLine("  " + ex.message);
//     } finally {
//         C = null;                                                       // Assure our Clarity object released
//     }
// 
//     Console.PrintLine("OK");
    
    try {
        if(!Telescope.Connected) {
            Util.WaitForMilliseconds(5000);
            Console.PrintLine("Connect ACP to the telescope, will auto-home if needed");
            Telescope.Connected = true;                                // (Requires driver "home on connect")
            Console.PrintLine("OK");
        } else {
            Console.PrintLine("Telescope already connected");
        }
    } catch(ex) {
        Console.PrintLine("**Failed to connect to the Telescope:");
        Console.PrintLine("  " + ex.description);
        Console.Logging = false;
        return;
    }

    try {                                                               //  Connect the camera
        if(!Util.CameraConnected) {
            Util.WaitForMilliseconds(5000);
            Console.PrintLine("Connect ACP to the camera");
            Util.CameraConnected = true;
            Console.PrintLine("OK");
        } else {
            Console.PrintLine("Camera already connected");
        }
    } catch(ex) {
        Console.PrintLine("**Failed to connect to MaxIm and camera:");
        Console.PrintLine("  " + ex.description);
        Console.Logging = false;
        return;
    }
    
    // DO NOT OPEN THE DOME HERE!
        
    Console.PrintLine("Chill cooler to -20");
    Camera.CoolerOn = true;
    Camera.TemperatureSetpoint = -20;                                   // Chill the cooler

    // ==========================
    // ADD ANYTHING ELSE YOU NEED
    // ==========================
    Dome.OpenShutter();
    
    while(Dome.ShutterStatus!=0)
    {
     Util.WaitForMilliseconds(1000);
     Console.PrintLine("Dome.ShutterStatus ="+Dome.ShutterStatus );     
    }
    
    Console.PrintLine("Dome.ShutterStatus ="+Dome.ShutterStatus );

    Console.PrintLine("Startup complete");
    Console.Logging = false;
}

//
// Start program by executable path, only if not already running.
// Waits 15 sec after starting for prog to initialize.
//
function StartProgram(exePath, windowState)
{
    try {
        var f = FSO.GetFile(exePath);
        if(IsProcessRunning(f.Name))                                    // Proc name is full file name
            return true;                                                // Already running
        if(IsProcessRunning(f.ShortName))                               // COM servers can have proc name of file short name
            return true;
        Console.PrintLine("Starting " + f.Name);
        Util.ShellExec(exePath, "", windowState);
        Util.WaitForMilliseconds(15000);                                // Wait for prog to initialize
        Console.PrintLine("OK");
        return true;
    } catch(ex) {
        Console.PrintLine("Exec: " + ex.message);
        return false;
    }
}

//
// Test if program is running (by exe name)
//
var WMI = null;                                                         // Avoid creating lots of these
function IsProcessRunning(exeName)
{
    try {
        // Magic WMI incantations!
        if(!WMI) WMI = new ActiveXObject("WbemScripting.SWbemLocator").ConnectServer(".","root/CIMV2");
    	var x = new Enumerator(WMI.ExecQuery("Select * From Win32_Process Where Name=\"" + exeName + "\"")).item().Name;
    	return true;
    } catch(e) {
    	return false;
    }
}
