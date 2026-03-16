param(
    [switch]$SkipInstaller
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
$wpfProject = Join-Path $root "CalculadoraCientifica.Wpf\CalculadoraCientifica.Wpf.csproj"
$coreTestsProject = Join-Path $root "CalculadoraCientifica.Core.Tests\CalculadoraCientifica.Core.Tests.csproj"
$uiTestsProject = Join-Path $root "CalculadoraCientifica.Wpf.UITests\CalculadoraCientifica.Wpf.UITests.csproj"

Write-Host "==> Compilando aplicação"
dotnet build $wpfProject -c Release -r $config.Runtime
Assert-LastExitCode "dotnet build app"

Write-Host "==> Executando testes de domínio"
dotnet test $coreTestsProject -c Release --no-build --no-restore
Assert-LastExitCode "dotnet test core"

Write-Host "==> Executando testes de interface"
$env:MM_CALC_EXE_PATH = Join-Path $root "CalculadoraCientifica.Wpf\bin\Release\net8.0-windows\$($config.Runtime)\$($config.ExecutableName)"
dotnet test $uiTestsProject -c Release --no-build --no-restore
Assert-LastExitCode "dotnet test ui"

Write-Host "==> Gerando pacote portátil"
& (Join-Path $root "publish-wpf.ps1") -SkipBuild

if (-not $SkipInstaller)
{
    Write-Host "==> Gerando instalador"
    & (Join-Path $root "scripts\build-installer.ps1")
}

Write-Host "==> Gerando hashes da release"
& (Join-Path $root "scripts\generate-release-hash.ps1")

Write-Host "Artefatos da release prontos em: $($config.Root)\dist"
