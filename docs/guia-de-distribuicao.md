# Guia de distribuição

## Fluxo recomendado

1. Rode a suíte completa de testes localmente.
2. Compile a aplicação WPF em `Release`.
3. Gere o pacote portátil.
4. Gere o instalador, se o Inno Setup estiver disponível.
5. Gere os hashes SHA-256.
6. Crie uma tag de versão, como `v1.0.0`.
7. Anexe o pacote portátil, o instalador e o arquivo de hashes à versão pública.

## Comando único

```powershell
powershell -ExecutionPolicy Bypass -File .\scripts\stage-release.ps1
```

## Fluxo manual

```powershell
dotnet build .\CalculadoraCientifica.Wpf\CalculadoraCientifica.Wpf.csproj -c Release
dotnet test .\CalculadoraCientifica.Core.Tests\CalculadoraCientifica.Core.Tests.csproj -c Release
dotnet test .\CalculadoraCientifica.Wpf.UITests\CalculadoraCientifica.Wpf.UITests.csproj -c Release
powershell -ExecutionPolicy Bypass -File .\publish-wpf.ps1
powershell -ExecutionPolicy Bypass -File .\scripts\build-installer.ps1
powershell -ExecutionPolicy Bypass -File .\scripts\generate-release-hash.ps1
```

## Artefatos esperados

- pacote portátil autossuficiente
- pacote portátil compactado em `.zip`
- executável do instalador
- `SHA256SUMS.txt`

## Observações

- mantenha a integração contínua verde antes de gerar uma versão
- prefira tags limpas, como `v1.0.0`, `v1.1.0` ou `v1.2.0`
- se houver assinatura de código disponível, aplique-a antes da distribuição pública
