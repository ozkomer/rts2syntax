//------------------------------------------------------------------------------
// Este archivo debe estar ubicado en el siguiente path\filename: 
// C:\Program Files\ACP Scheduler\ShutdownObs.js
//------------------------------------------------------------------------------
//
// ShutdownObs.js - ACP Scheduler Observatory Shutdown Script
//
// Sample shutdown script for Scheduler 2.2. This script runs at te end of an
// observing night, not in response to a weather alert (which is handled by ACP).
// 
// Bob Denny        19-Jan-2009 Initial edit
// Bob Denny		22-Jan-2009	Scheduler now passes dawn cal frame parameter
// Bob Denny        23-Jan-2009 No more cal frames here! Greatly simplified.
// Bob Denny        26-May-2010 GEM:414 Make cooler warmup more robust per 
//                  AcquireSupport
//------------------------------------------------------------------------------
var FSO;

function main()
{
    var SEQTOK, buf;
    var doDawnCalFrames = false;
    
    FSO = new ActiveXObject("Scripting.FileSystemObject");

    buf = Prefs.LocalUser.DefaultLogDir + "\\Scheduler\\LastShutdownObs.log";
    try {
        FSO.DeleteFile(buf);
    } catch(ex) { }
    Console.LogFile = buf;
    Console.Logging = true;
    
    //
    // Don't allow weather interrupts any more, may play havoc with orderly shutdown here.
    //
    if(Util.WeatherConnected) {
        Console.PrintLine("Disconnecting weather");
        Util.WeatherConnected = false;
    }

    //
    // Similar to shutdown logic in AcquireSupport.wsc
    //
    
    if(Telescope.Connected) {
        Console.PrintLine("Parking scope"); 
        Telescope.Park();                                               // Park the scope if possible, and/or close/park dome
        Console.PrintLine("OK");
        if(Telescope.Connected) {
            Console.PrintLine("Disconnecting Scope");
            Telescope.Connected = false;
        }
    }
            
    if(Util.CameraConnected) {
        Console.PrintLine("Shutting down imager. Please wait...");
        var z = Camera.TemperatureSetpoint;                             // Remember this, as MaxIm remembers
        Console.PrintLine("  (raising temperature to +5.0C... 20 min max)");
        var tPrev = -273.15;
        Console.PrintLine("  (cooler is now at " + Util.FormatVar(Camera.Temperature, "0.0") + "C");
        Camera.TemperatureSetpoint = 6.0;                               // Raise temperature to +6C
        for(var i = 0; i < 20; i++)                                     // Take 20 minutes max...
        {   
            var tNow = Camera.Temperature;
            if(tNow >= 3.0) break;                                      // Warmed, time to shut down cooler
            if((tNow - tPrev) < 3.0) break;                             // Warming rate < 0.05deg/sec, can shut down
            tPrev = tNow;
            Util.WaitForMilliseconds(60000);                            // Wait another minute
            Console.PrintLine("  (cooler is now at " +
                Util.FormatVar(Camera.Temperature, "0.0") + "C)");
        }
        Camera.CoolerOn = false;
        Camera.TemperatureSetpoint = z;                                 // Restore original setpoint for future
        Util.WaitForMilliseconds(2000);                                 // Give MaxIm a chance to shutdown cooler
        Camera.LinkEnabled = false;
        Util.CameraConnected = false;                                   // Disconnect it from ACP
        Console.PrintLine("OK, imager shutdown complete.");
    }
    
    Util.WaitForMilliseconds(1000);
    
    Console.PrintLine("Shutting down programs");
    if(!StopProcess("FocusMax.exe"))
        Console.PrintLine("**Failed to stop FocusMax");
    Util.WaitForMilliseconds(1000);
    if(!StopProcess("MaxIm_DL.exe"))
        Console.PrintLine("**Failed to stop MaxIm");
    if(FSO.FileExists(ACPApp.Path & "\RotatorInfo.txt"))                // If there is a rotator, assure proper shutdown
    {
        Util.WaitForMilliseconds(1000);
        try {
            var R = new ActiveXObject("ACP.Rotator");
            R.Connected = false;                                        // This should cause RCOS-ae to exit nicely
            R = null;
        } catch(ex) {
            Console.PrintLine("**Failed to disconnect rotator:");
            Console.PrintLine("  " + ex.message);
        }
        Util.WaitForMilliseconds(5000);
        if(!StopProcess("RotControl.exe"))
            Console.PrintLine("**Failed to stop ACP rotator applet.");
    }

    // ======================================================
    // HERE IS WHERE YOU ADD CODE TO TURN POWER OFF AS NEEDED
    // ======================================================
        
    Console.PrintLine("Observatory shutdown complete");
    Console.Logging = false;
}

//
// Stop a process by executable NAME. WMI magic. Look in TaskManager
// to see what the executable NAMEs are.
//
var WMI = null;                                                         // Avoid creating lots of these
function StopProcess(exeName)
{
    var x = null;
    Console.PrintLine("Stopping " + exeName);
    try {
        // Magic WMI incantations!
        if(!WMI) WMI = new ActiveXObject("WbemScripting.SWbemLocator").ConnectServer(".","root/CIMV2");
    	x = new Enumerator(WMI.ExecQuery("Select * From Win32_Process Where Name=\"" + exeName + "\"")).item();
        if(!x) {                                                        // May be 'undefined' as well as null
            Console.PrintLine("(" + exeName + " not running)");
        	return true;                                                // This is a success, it's stopped
        }
        x.Terminate();
        Console.PrintLine("OK");
        return true;
    } catch(ex) {
        Console.PrintLine("WMI: " + (ex.message ? ex.message : ex));
        return false;
    }
}

//
// Shell out to an executable. Probably handy for power control.
//  exePath: full path/name of executable to run
//  cmdLine: command line to give to that executable
//
// Waits till executable exits. Throws error if can't start executable,
// if the executable takes longer than 10 seconds to complete, or if 
// the executable exits with error status. The executable is run 
// minimized without getting focus ("background").
//
function RunProgram(exePath, cmdLine)
{
    var exeName = FSO.GetFileName(exePath);                             // Just the file name of the executable
    Console.PrintLine("Running " + exeName + "...");                    // Announce our intentions
    var tid = Util.ShellExec(exePath, cmdLine, 6);                      // Execute command (minimized, no focus, throws on errors)
    var i;
    for(i = 0; i < 10; i++) {                                           // Wait up to 10 sec
        if(!Util.IsTaskActive(tid)) break;
        Util.WaitForMilliseconds(1000);                                 // Wait 1 sec here
    }
    if(i >= 10)                                                         // Wait failed?
        throw "** " + exeName + " failed to exit in 10 sec.";
    if(Util.GetTaskExitStatus(tid) !== 0)                               // Exited with failure status?
        throw "** " + exeName + " exited with error status.";
    Console.PrintLine("...OK");
}
