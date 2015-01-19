; -- exporter.iss --

#ifndef Revision
  #error Called outside build script!
#endif

#define ExporterGetSuffix() GetDateTimeString('yyyy_mm_dd','','');

[Setup]
AppId=BPExporter
AppName=BPExporter 1.0
AppVerName=BPExporter 1.0
AppPublisher=Baseprotect
AppPublisherURL=http://www.baseprotect.com
AppSupportURL=http://www.baseprotect.com/
DefaultDirName=C:\Baseprotect\BPExporter
DefaultGroupName=Baseprotect
UninstallDisplayIcon={app}\BPExporter.exe
OutputDir=bin
Compression=lzma
SolidCompression=yes
OutputBaseFilename=BPExporterSetup_{#ExporterGetSuffix()}_{#Revision}

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked


[Files]
Source: "..\..\trunk\BPExporter\bin\Release\BPExporter.exe";                                DestDir: "{app}" 
Source: "..\..\trunk\BPExporter\bin\Release\Microsoft.Office.Interop.Excel.dll";            DestDir: "{app}"
Source: "..\..\trunk\BPExporter\bin\Release\ObjectListView.dll";                            DestDir: "{app}"
Source: "..\..\trunk\BPExporter\bin\Release\GeoIPISP.dat";                                  DestDir: "{app}"
Source: "..\..\trunk\BPExporter\bin\Release\GeoIPCity.dat";                                 DestDir: "{app}"
Source: "..\..\trunk\BPExporter\bin\Release\System.Data.SQLite.DLL";                        DestDir: "{app}"

Source: "..\..\trunk\BPExporter\db_deploy\data.db";     DestDir: "{app}"; Flags: onlyifdoesntexist;                

;Office Interop, why we need this?
Source: "..\..\trunk\BPExporter\bin\Release\Microsoft.Office.Interop.Excel.dll";          DestDir: "{app}"

Source: "..\..\trunk\BPExporter\bin\Release\schemas\*.*"; Excludes: ".svn"; DestDir: "{app}\schemas"; Flags: onlyifdoesntexist;
Source: "..\..\trunk\BPExporter\bin\Release\tables\*.*"; Excludes: ".svn"; DestDir: "{app}\tables"; Flags: onlyifdoesntexist;


Source: ".\O2003PIA.MSI";                       DestDir: {tmp}; Flags: deleteafterinstall
Source: ".\O2007PIA.MSI";                       DestDir: {tmp}; Flags: deleteafterinstall
Source: ".\O2010PIA.MSI";                       DestDir: {tmp}; Flags: deleteafterinstall

Source: "..\BPExporter\fix.bat";                            DestDir: {app};
Source: ".\sqlite3.exe";                        DestDir: {app};
Source: ".\rbk_to_cmt.exe";                     DestDir: {app};
Source: ".\isxdl.dll";                          DestDir: {app};

[Icons]
Name: "{group}\BPExporter"; Filename: "{app}\BPExporter.exe"
Name: "{commondesktop}\BPExporter"; Filename: "{app}\BPExporter.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\BPExporter"; Filename: "{app}\BPExporter.exe"; Tasks: quicklaunchicon
Name: "{group}\Remove BPExporter"; Filename: "{uninstallexe}"

[Code]
var
	dotnetRedistPath: string;
	downloadNeeded: boolean;
	dotNetNeeded: boolean;
	sqlceRedistPath: string;
	//downloadSqlCeNeeded: boolean;
	sqlCeNeeded: boolean;
	reportviewerRedistPath: string;
	reportviewerNeeded: boolean;
	o2003RedistPath: string;
  bullZipRedistPath : string;
  bullZipNeeded : boolean;
	o2003Needed: boolean;
	memoDependenciesNeeded: string;
	Version: TWindowsVersion;
	//oldVersion: String;
  uninstaller: String;
  ErrorCode: Integer;
  o2007Needed: boolean;
  o2007RedistPath:String;
  o2010Needed: boolean;
  o2010RedistPath:String;
  OfficeVer:String;
  SVNRevision: String;

procedure isxdl_AddFile(URL, Filename: PAnsiChar);
external 'isxdl_AddFile@files:isxdl.dll stdcall';
function isxdl_DownloadFiles(hWnd: Integer): Integer;
external 'isxdl_DownloadFiles@files:isxdl.dll stdcall';
function isxdl_SetOption(Option, Value: PAnsiChar): Integer;
external 'isxdl_SetOption@files:isxdl.dll stdcall';


// Can change this link to remoba instead of direct link to avoid dead links from MS
const
	dotnetRedistURL = 'http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe';
	appName = 'Exporter 1.0';

function GetSVNRev(Dir: String): String;
var 
  SVNEntiresFilePath : String;
  VersionString : Array Of String;
  begin
    SVNEntiresFilePath := Dir + '.svn' + '\entries'; 
    LoadStringsFromFile(SVNEntiresFilePath, VersionString);
    Result := VersionString[3];
  end;

function CreateAppName(param:String) : String;
var 
  SVNRev : String;
  begin
    //SVNRev := GetSVNRev('D:\_BASEPROTECT\Joiner\trunk\bin\Release\');
    //Result := 'Joiner 2.0.' + SVNRev; 
    Result := 'Exporter 1.0';
  end;

//*********************************************************************************
// This is where all starts.
//*********************************************************************************
function InitializeSetup(): Boolean;

begin

	Result := true;
	dotNetNeeded := false;
	sqlCeNeeded := false;
  bullZipNeeded := false;
  o2010Needed := false;
  o2003Needed := false;
  o2007Needed := false;

	GetWindowsVersionEx(Version);
  RegQueryStringValue(HKEY_CLASSES_ROOT, 'Word.Application\CurVer',  '', OfficeVer);

 //*****************************************************************************************
 // Check whether application  already  installed by reading app id in registry
  //****************************************************************************************
	
	if RegKeyExists(HKEY_LOCAL_MACHINE,  'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Exporter_is1') then
  begin
   if MsgBox('Application has detected that Baseprotect Exporter is already installed on your computer. Do you wish to Uninstall your previous setup and continue with fresh installation?',

           mbConfirmation, MB_YESNO) = IDNO then

         begin

           Result := False;

         end

         else

         begin

             RegQueryStringValue(HKEY_LOCAL_MACHINE,

               'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Exporter_is1',

              'UninstallString', uninstaller);

             //ShellExec('runas', uninstaller, '/SILENT', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);
             //use above statement if extra level security is required usually it is not req
             ShellExec('open', uninstaller, '/SILENT', '', SW_HIDE, ewWaitUntilTerminated, ErrorCode);

             if RegKeyExists(HKEY_LOCAL_MACHINE,  'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Exporter_is1') then
             begin
              Result := false;
             end
             else
             begin
             Result := true;
             end

         end;

       end;

  //************************************************************************************
	// Check for the existance of .NET 3.5  on client machine before installing app
	//************************************************************************************
    if (not RegKeyExists(HKLM, 'Software\Microsoft\NET Framework Setup\NDP\v3.5')) then
		begin
			dotNetNeeded := true;
				

			if (not IsAdminLoggedOn()) then
				begin
					MsgBox('Application needs the Microsoft .NET Framework 3.5 to be installed by an Administrator', mbInformation, MB_OK);
					Result := false;
				end
			else
				begin
					memoDependenciesNeeded := memoDependenciesNeeded + '      .NET Framework 3.5' #13;
					dotnetRedistPath := ExpandConstant('{src}\dotnetfx.exe');
					if not FileExists(dotnetRedistPath) then
						begin
							dotnetRedistPath := ExpandConstant('{tmp}\dotnetfx.exe');
							if not FileExists(dotnetRedistPath) then
								begin
									isxdl_AddFile(dotnetRedistURL, dotnetRedistPath);
									downloadNeeded := true;
								end
						end;

					SetIniString('install', 'dotnetRedist', dotnetRedistPath, ExpandConstant('{tmp}\dep.ini'));
				end
		end;
		
   //************************************************************************************
	 // Check for the existance of Office 2010
	 //************************************************************************************
    if (OfficeVer = 'Word.Application.14') then
		begin
      o2010Needed := true;

			if (not IsAdminLoggedOn()) then
				begin
					MsgBox('Application needs the Microsoft Office 2010 Primary Interop Assemblies to be installed by an Administrator', mbInformation, MB_OK);
					Result := false;
				end
			else
				begin
					memoDependenciesNeeded := memoDependenciesNeeded + '      Office 2010 Primary Interop Assemblies' #13;
					o2010RedistPath := ExpandConstant('{tmp}\O2010PIA.MSI');
				end
		end;		
		
	 //************************************************************************************
	 // Check for the existance of Office 2007 
	 //************************************************************************************
    if (OfficeVer = 'Word.Application.12') then
		begin
			o2007Needed := true;
   
			if (not IsAdminLoggedOn()) then
				begin
					MsgBox('Application needs the Microsoft Office 2007 Primary Interop Assemblies to be installed by an Administrator', mbInformation, MB_OK);
					Result := false;
				end
			else
				begin
					memoDependenciesNeeded := memoDependenciesNeeded + '      Office 2007 Primary Interop Assemblies' #13;
					o2007RedistPath := ExpandConstant('{tmp}\O2007PIA.MSI');
				end
		end;

   //************************************************************************************
	 // Check for the existance of Office 2003 
	 //************************************************************************************
    if (OfficeVer = 'Word.Application.11') then
		begin
			o2003Needed := true;
   
			if (not IsAdminLoggedOn()) then
				begin
					MsgBox('Application needs the Microsoft Office 2003 Primary Interop Assemblies to be installed by an Administrator', mbInformation, MB_OK);
					Result := false;
				end
			else
				begin
					memoDependenciesNeeded := memoDependenciesNeeded + '      Office 2003 Primary Interop Assemblies' #13;
					o2003RedistPath := ExpandConstant('{tmp}\O2003PIA.MSI');
				end
		end;
end;

function NextButtonClick(CurPage: Integer): Boolean;

var
  hWnd: Integer;
  ResultCode: Integer;
  ResultXP: boolean;
  Result2003: boolean;

  begin

  Result := true;
  ResultXP := true;
  Result2003 := true;

  //*********************************************************************************
  // Only run this at the "Ready To Install" wizard page.
  //*********************************************************************************
  if CurPage = wpReady then
	begin

		hWnd := StrToInt(ExpandConstant('{wizardhwnd}'));
		// don't try to init isxdl if it's not needed because it will error on < ie 3

		//********************************************************************************************************
		// Download the .NET 3.5 redistribution file. Can change the MS link to application development site to avoid dead link
		//*********************************************************************************************************
		if downloadNeeded and (dotNetNeeded = true) then
			begin
				isxdl_SetOption('label', 'Downloading Microsoft .NET Framework 3.5');
				isxdl_SetOption('description', 'This app needs to install the Microsoft .NET Framework 3.5. Please wait while Setup is downloading extra files to your computer.');
				if isxdl_DownloadFiles(hWnd) = 0 then Result := false;
			end;

		//***********************************************************************************
		// Run the install file for .NET Framework 3.5. This is usually dotnetfx.exe from MS
		//***********************************************************************************
    if (dotNetNeeded = true) then
			begin

				if Exec(ExpandConstant(dotnetRedistPath), '', '', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
					begin

						// handle success if necessary; ResultCode contains the exit code
						if not (ResultCode = 0) then
							begin

								Result := false;

							end
					end
					else
						begin

							// handle failure if necessary; ResultCode contains the error code
							Result := false;

						end
			end;

    //********************************************************************************************************
		// Install the Office 2003 Primary Interop Assemblies redistribution file.
		//*********************************************************************************************************
		//o2003Needed := false;
		if (o2003Needed = true) then
			begin
		  ExtractTemporaryFile('O2003PIA.MSI');
      if Exec('msiexec',ExpandConstant('/i "{tmp}\O2003PIA.MSI" /qb'),'', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
					begin

						// handle success if necessary; ResultCode contains the exit code
						if not (ResultCode = 0) then
							begin

								Result := false;

							end
					end
					else
						begin

							// handle failure if necessary; ResultCode contains the error code
							Result := false;

						end
			end;		

    //********************************************************************************************************
		// Install the Office 2007 Primary Interop Assemblies redistribution file.
		//*********************************************************************************************************
		//o2007Needed := false;
		if (o2007Needed = true) then
			begin
		  ExtractTemporaryFile('O2007PIA.MSI');
      if Exec('msiexec',ExpandConstant('/i "{tmp}\O2007PIA.MSI" /qb'),'', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
					begin
     
						// handle success if necessary; ResultCode contains the exit code
						if not (ResultCode = 0) then
							begin
    
								Result := false;
    
							end
					end
					else
						begin
    
							// handle failure if necessary; ResultCode contains the error code
							Result := false;
    
						end
			end;	
    
    //********************************************************************************************************
		// Install the Office 2010 Primary Interop Assemblies redistribution file.
		//*********************************************************************************************************
		//o2010Needed := false;
		if (o2010Needed = true) then
			begin
		  ExtractTemporaryFile('O2010PIA.MSI');
      
      if Exec('msiexec',ExpandConstant('/i "{tmp}\O2010PIA.MSI" /qb'),'', SW_SHOW, ewWaitUntilTerminated, ResultCode) then
					begin

						// handle success if necessary; ResultCode contains the exit code
						if not (ResultCode = 0) then
							begin

								Result := false;

							end
					end
					else
						begin

							// handle failure if necessary; ResultCode contains the error code
							Result := false;

						end
			end;
      
	end;

end;

//*********************************************************************************
// Updates the memo box shown right before the install actuall starts.
//*********************************************************************************
function UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
var
  s: string;

begin

  if memoDependenciesNeeded <> '' then s := s + 'Dependencies that will be automatically downloaded And/Or installed:' + NewLine + memoDependenciesNeeded + NewLine;
  s := s + MemoDirInfo + NewLine + NewLine;

  Result := s

end;


[Run]
Filename: "{app}\BPExporter.exe"; Description: "{cm:LaunchProgram,Baseprotect Exporter}"; Flags: nowait postinstall skipifsilent
