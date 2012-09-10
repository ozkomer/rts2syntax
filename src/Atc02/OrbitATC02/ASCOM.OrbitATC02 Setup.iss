;
; Script generated by the ASCOM Driver Installer Script Generator 6.0.0.0
; Generated by Eduardo Maureira on 4/10/2012 (UTC)
;
[Setup]
AppName=ASCOM ASCOM.OrbitATC02 Focuser Driver
AppVerName=ASCOM ASCOM.OrbitATC02 Focuser Driver 0.97
AppVersion=0.58
AppPublisher=Eduardo Maureira <emaureir@gmail.com>
AppPublisherURL=mailto:emaureir@gmail.com
AppSupportURL=http://tech.groups.yahoo.com/group/ASCOM-Talk/
AppUpdatesURL=http://ascom-standards.org/
VersionInfoVersion=1.0.0
MinVersion=0,5.0.2195sp4
DefaultDirName="{cf}\ASCOM\Focuser"
DisableDirPage=yes
DisableProgramGroupPage=yes
OutputDir="."
OutputBaseFilename="ASCOM.OrbitATC02 Setup"
Compression=lzma
SolidCompression=yes
; Put there by Platform if Driver Installer Support selected
WizardImageFile="C:\Program Files\ASCOM\Platform 6 Developer Components\Installer Generator\Resources\WizardImage.bmp"
LicenseFile="C:\Program Files\ASCOM\Platform 6 Developer Components\Installer Generator\Resources\CreativeCommons.txt"
; {cf}\ASCOM\Uninstall\Focuser folder created by Platform, always
UninstallFilesDir="{cf}\ASCOM\Uninstall\Focuser\ASCOM.OrbitATC02"

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Dirs]
Name: "{cf}\ASCOM\Uninstall\Focuser\ASCOM.OrbitATC02"
; TODO: Add subfolders below {app} as needed (e.g. Name: "{app}\MyFolder")

[Files]
Source: "C:\Users\chase\Documents\emaureir\src\Atc02\OrbitATC02\bin\Release\ASCOM.OrbitATC02.Focuser.dll"; DestDir: "{app}"
; Require a read-me HTML to appear after installation, maybe driver's Help doc
Source: "C:\Users\chase\Documents\emaureir\src\Atc02\OrbitATC02\OrbitATC02ReadMe.htm"; DestDir: "{app}"; Flags: isreadme
; TODO: Add other files needed by your driver here (add subfolders above)


; Only if driver is .NET
[Run]
; Only for .NET assembly/in-proc drivers
Filename: "{dotnet2032}\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.OrbitATC02.Focuser.dll"""; Flags: runhidden 32bit
Filename: "{dotnet2064}\regasm.exe"; Parameters: "/codebase ""{app}\ASCOM.OrbitATC02.Focuser.dll"""; Flags: runhidden 64bit; Check: IsWin64




; Only if driver is .NET
[UninstallRun]
; Only for .NET assembly/in-proc drivers
Filename: "{dotnet2032}\regasm.exe"; Parameters: "-u ""{app}\ASCOM.OrbitATC02.Focuser.dll"""; Flags: runhidden 32bit
Filename: "{dotnet2064}\regasm.exe"; Parameters: "-u ""{app}\ASCOM.OrbitATC02.Focuser.dll"""; Flags: runhidden 64bit; Check: IsWin64




[CODE]
//
// Before the installer UI appears, verify that the (prerequisite)
// ASCOM Platform 5.5 or greater is installed, including both Helper
// components. Utility is required for all types (COM and .NET)!
//
function InitializeSetup(): Boolean;
var
   U : Variant;
   H : Variant;
begin
   Result := FALSE;  // Assume failure
   // check that the DriverHelper and Utilities objects exist, report errors if they don't
   try
      H := CreateOLEObject('DriverHelper.Util');
   except
      MsgBox('The ASCOM DriverHelper object has failed to load, this indicates a serious problem with the ASCOM installation', mbInformation, MB_OK);
   end;
   try
      U := CreateOLEObject('ASCOM.Utilities.Util');
   except
      MsgBox('The ASCOM Utilities object has failed to load, this indicates that the ASCOM Platform has not been installed correctly', mbInformation, MB_OK);
   end;
   try
      if (U.IsMinimumRequiredVersion(5,5)) then	// this will work in all locales
         Result := TRUE;
   except
   end;
   if(not Result) then
      MsgBox('The ASCOM Platform 5.5 or greater is required for this driver.', mbInformation, MB_OK);
end;

