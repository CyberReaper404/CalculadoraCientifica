param()

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$propsPath = Join-Path $root "Directory.Build.props"

if (-not (Test-Path $propsPath))
{
    throw "Arquivo não encontrado: $propsPath"
}

[xml]$props = Get-Content $propsPath
$propertyGroup = $props.Project.PropertyGroup

$version = $propertyGroup.Version
$product = $propertyGroup.Product
$publisher = $propertyGroup.Company

if ([string]::IsNullOrWhiteSpace($version) -or [string]::IsNullOrWhiteSpace($product))
{
    throw "Não foi possível ler os metadados de release em Directory.Build.props."
}

$runtime = "win-x64"
$exeName = "MMCalcCientifica.exe"
$portableFolderName = "MM-CALC-Cientifica-$version-$runtime-portable"
$portableDirectory = Join-Path $root "dist\$portableFolderName"
$installerDirectory = Join-Path $root "dist\installer"
$installerName = "MM-CALC-Cientifica-Setup-$version.exe"
$zipName = "$portableFolderName.zip"
$repositorySlug = $env:MM_CALC_REPOSITORY_SLUG
$repositoryUrl = if ([string]::IsNullOrWhiteSpace($repositorySlug))
{
    "https://github.com/SEU_USUARIO/SEU_REPOSITORIO"
}
else
{
    "https://github.com/$repositorySlug"
}

[pscustomobject]@{
    Root = $root
    Version = $version
    Product = $product
    Publisher = $publisher
    Runtime = $runtime
    ExecutableName = $exeName
    PortableFolderName = $portableFolderName
    PortableDirectory = $portableDirectory
    PortableZipPath = Join-Path $root "dist\$zipName"
    InstallerDirectory = $installerDirectory
    InstallerPath = Join-Path $installerDirectory $installerName
    RepositoryUrl = $repositoryUrl
}
