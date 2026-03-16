param(
    [string]$Runtime = "win-x64",
    [switch]$SkipBuild
)

$ErrorActionPreference = "Stop"

function Assert-LastExitCode([string]$StepName)
{
    if ($LASTEXITCODE -ne 0)
    {
        throw "$StepName falhou com codigo de saida $LASTEXITCODE."
    }
}

$root = Split-Path -Parent $MyInvocation.MyCommand.Path
$config = & (Join-Path $root "scripts\Get-ReleaseConfig.ps1")
$project = Join-Path $root "CalculadoraCientifica.Wpf\CalculadoraCientifica.Wpf.csproj"
$releaseRoot = Join-Path $root "dist"
$version = $config.Version
$portableFolderName = "MM-CALC-Cientifica-$version-$Runtime-portable"
$portableOutput = Join-Path $releaseRoot $portableFolderName
$zipPath = Join-Path $releaseRoot "$portableFolderName.zip"

if (Test-Path $portableOutput)
{
    Remove-Item $portableOutput -Recurse -Force
}

if (Test-Path $zipPath)
{
    Remove-Item $zipPath -Force
}

$publishArguments = @(
    "publish",
    $project,
    "-c", "Release",
    "-r", $Runtime,
    "--self-contained", "true",
    "/p:PublishSingleFile=false",
    "/p:PublishTrimmed=false",
    "/p:DebugType=None",
    "/p:DebugSymbols=false",
    "-o", $portableOutput
)

if ($SkipBuild)
{
    $publishArguments += @("--no-build")
}

dotnet @publishArguments
Assert-LastExitCode "dotnet publish"

Compress-Archive -Path (Join-Path $portableOutput "*") -DestinationPath $zipPath -Force

Write-Host "Pacote portátil: $portableOutput"
Write-Host "Pacote ZIP: $zipPath"
Write-Host "Executável: $portableOutput\\$($config.ExecutableName)"
