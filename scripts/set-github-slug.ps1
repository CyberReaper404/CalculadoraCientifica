param(
    [string]$RepositorySlug
)

$ErrorActionPreference = "Stop"

$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
$readmePath = Join-Path $root "README.md"

if ([string]::IsNullOrWhiteSpace($RepositorySlug))
{
    $originUrl = git -C $root remote get-url origin 2>$null

    if (-not [string]::IsNullOrWhiteSpace($originUrl) -and $originUrl -match "github\.com[:/](?<slug>[^/\s]+/[^/\s\.]+)(\.git)?$")
    {
        $RepositorySlug = $Matches.slug
    }
    else
    {
        throw "Informe -RepositorySlug ou configure o remote origin do GitHub antes de rodar este script."
    }
}

$content = Get-Content $readmePath -Raw -Encoding utf8
$content = $content.Replace("SEU_USUARIO/SEU_REPOSITORIO", $RepositorySlug)
Set-Content $readmePath $content -Encoding utf8

Write-Host "README atualizado com o slug: $RepositorySlug"
