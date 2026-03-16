param(
    [string]$Target
)

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$config = & (Join-Path $root "scripts\Get-ReleaseConfig.ps1")

if ([string]::IsNullOrWhiteSpace($Target))
{
    $Target = $config.PortableZipPath
}

if (-not (Test-Path $Target))
{
    throw "Arquivo não encontrado: $Target"
}

$hashOutput = Join-Path $config.Root "dist\SHA256SUMS.txt"

Get-FileHash -Algorithm SHA256 -Path $Target |
    ForEach-Object { "{0}  {1}" -f $_.Hash.ToLowerInvariant(), [System.IO.Path]::GetFileName($_.Path) } |
    Set-Content $hashOutput

if (Test-Path $config.InstallerPath)
{
    Get-FileHash -Algorithm SHA256 -Path $config.InstallerPath |
        ForEach-Object { "{0}  installer\{1}" -f $_.Hash.ToLowerInvariant(), [System.IO.Path]::GetFileName($_.Path) } |
        Add-Content $hashOutput
}

Get-Content $hashOutput
