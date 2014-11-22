; -- ams.iss --

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
OutputBaseFilename=BPExporterUpdate

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\..\trunk\BPExporter\bin\Release\BPExporter.exe";                                DestDir: "{app}" 
;Source: "..\..\trunk\BPExporter\bin\Release\Microsoft.Office.Interop.Excel.dll";            DestDir: "{app}"
Source: "..\..\trunk\BPExporter\bin\Release\ObjectListView.dll";                            DestDir: "{app}"
;Source: "..\..\trunk\BPExporter\bin\Release\GeoIPISP.dat";                                  DestDir: "{app}"
;Source: "..\..\trunk\BPExporter\bin\Release\GeoIPCity.dat";                                 DestDir: "{app}"
;Source: "..\..\trunk\BPExporter\bin\Release\System.Data.SQLite.DLL";                        DestDir: "{app}"

;Source: "..\..\trunk\BPExporter\db_deploy\data.db";     DestDir: "{app}"

Source: "..\..\trunk\BPExporter\bin\Release\schemas\*.*"; Excludes: ".svn"; DestDir: "{app}\schemas"


[Run]
Filename: "{app}\BPExporter.exe"; Description: "{cm:LaunchProgram,Baseprotect Exporter}"; Flags: nowait postinstall skipifsilent
