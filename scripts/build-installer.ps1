param(
    [string]$InnoSetupCompiler = "iscc"
)

$ErrorActionPreference = "Stop"

function Assert-LastExitCode([string]$StepName)
{
    if ($LASTEXITCODE -ne 0)
    {
        throw "$StepName falhou com codigo de saida $LASTEXITCODE."
    }
}

$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$config = & (Join-Path $root "scripts\Get-ReleaseConfig.ps1")
$script = Join-Path $root "installer\MM-CALC-Cientifica.iss"

if (-not (Test-Path $config.PortableDirectory))
{
    throw "Pacote portátil não encontrado em '$($config.PortableDirectory)'. Rode publish-wpf.ps1 antes de gerar o instalador."
}

if (-not (Get-Command $InnoSetupCompiler -ErrorAction SilentlyContinue))
{
    $profileLocalAppData = [Environment]::GetFolderPath([Environment+SpecialFolder]::LocalApplicationData)
    $candidates = @(
        "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
        "${env:ProgramFiles}\Inno Setup 6\ISCC.exe",
        "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe",
        (Join-Path $profileLocalAppData "Programs\Inno Setup 6\ISCC.exe")
    )

    $candidate = $candidates | Where-Object { Test-Path $_ } | Select-Object -First 1

    if (-not $candidate)
    {
        $registryCandidate = Get-ChildItem @(
            "HKCU:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            "HKLM:\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            "HKLM:\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
        ) -ErrorAction SilentlyContinue |
            Get-ItemProperty -ErrorAction SilentlyContinue |
            Where-Object { $_.DisplayName -like "Inno Setup*" -and -not [string]::IsNullOrWhiteSpace($_.InstallLocation) } |
            ForEach-Object { Join-Path $_.InstallLocation "ISCC.exe" } |
            Where-Object { Test-Path $_ } |
            Select-Object -First 1

        if ($registryCandidate)
        {
            $candidate = $registryCandidate
        }
    }

    if ($candidate)
    {
        $InnoSetupCompiler = $candidate
    }
    else
    {
        throw "Compilador do Inno Setup não encontrado. Instale o Inno Setup 6 ou informe -InnoSetupCompiler."
    }
}

& $InnoSetupCompiler `
    "/DMyAppName=$($config.Product)" `
    "/DMyAppVersion=$($config.Version)" `
    "/DMyAppPublisher=$($config.Publisher)" `
    "/DMyAppURL=$($config.RepositoryUrl)" `
    "/DMyAppExeName=$($config.ExecutableName)" `
    "/DMyReleaseDir=$($config.PortableDirectory)" `
    "/DMyOutputBaseFilename=MM-CALC-Cientifica-Setup-$($config.Version)" `
    $script
Assert-LastExitCode "iscc"
