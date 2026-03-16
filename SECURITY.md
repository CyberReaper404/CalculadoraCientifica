# Notas de segurança

## Escopo prático

Este projeto é uma calculadora desktop, não um produto de segurança isolado. Nenhum repositório de código aberto consegue impedir totalmente que terceiros:

- façam fork do código
- alterem o código localmente
- redistribuam o aplicativo com outro nome

O que este repositório consegue fazer é tornar a versão oficial mais fácil de verificar e mais difícil de adulterar silenciosamente.

## O que já está em prática

- `NuGet.Config` limpa fontes herdadas e usa apenas `https://api.nuget.org/v3/index.json`
- a restauração de pacotes usa lock files por meio de `Directory.Build.props`
- o aplicativo não usa rede em tempo de execução, shell, carregamento dinâmico de plugins ou avaliação de scripts
- as entradas da calculadora são validadas e rejeitam domínios inválidos e valores não finitos, como `NaN` e `Infinity`
- os fluxos do GitHub Actions compilam, testam, empacotam e geram hash SHA-256 para versões oficiais

## Prática recomendada para releases

Para releases no GitHub:

1. Gere a build a partir de uma máquina limpa ou do CI.
2. Rode a suíte completa de testes.
3. Publique o aplicativo.
4. Gere os hashes SHA-256 dos artefatos.
5. Publique os hashes junto com a release.
6. Se possível, assine digitalmente o executável do Windows.

## No que a pessoa usuária deve confiar

Prefira:

- o código-fonte do repositório principal
- artefatos de release com hashes publicados
- binários anexados às GitHub Releases, e não arquivos espalhados em espelhos aleatórios

## Limites

Sem assinatura de código, não é possível garantir que um binário baixado de uma fonte não oficial não tenha sido modificado.
