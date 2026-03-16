#ifndef MyAppName
  #define MyAppName "MM CALC Científica"
#endif
#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif
#ifndef MyAppPublisher
  #define MyAppPublisher "Maria Eduarda Miyashiro"
#endif
#ifndef MyAppURL
  #define MyAppURL "https://github.com/SEU_USUARIO/SEU_REPOSITORIO"
#endif
#ifndef MyAppExeName
  #define MyAppExeName "MMCalcCientifica.exe"
#endif
#ifndef MyReleaseDir
  #define MyReleaseDir "..\dist\MM-CALC-Cientifica-1.0.0-win-x64-portable"
#endif
#ifndef MyOutputBaseFilename
  #define MyOutputBaseFilename "MM-CALC-Cientifica-Setup-1.0.0"
#endif

[Setup]
AppId={{3E2F4AE7-0D7F-4F0E-8B3A-3E23B58A7DAB}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\dist\installer
OutputBaseFilename={#MyOutputBaseFilename}
Compression=lzma
SolidCompression=yes
WizardStyle=modern
SetupIconFile=..\CalculadoraCientifica.Wpf\Assets\mm-calc.ico
UninstallDisplayIcon={app}\{#MyAppExeName}
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na Área de Trabalho"; GroupDescription: "Atalhos adicionais:"

[Files]
Source: "{#MyReleaseDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Abrir MM CALC Científica"; Flags: nowait postinstall skipifsilent
