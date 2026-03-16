# De projeto de faculdade a aplicativo desktop testado

## Contexto

Este projeto começou como uma calculadora científica em terminal, criada durante a faculdade para praticar estruturas de repetição, condicionais, funções e módulos matemáticos.

A versão desktop surgiu como uma reinterpretação orientada a produto dessa primeira ideia, com o objetivo de mostrar evolução técnica em vez de apenas preservar o código antigo.

## Problema

A calculadora original funcionava, mas tinha os limites típicos de um projeto acadêmico inicial:

- interação apenas por terminal
- lógica misturada com entrada e saída
- ausência de testes automatizados
- ausência de empacotamento e processo de distribuição
- ausência de identidade visual

## Decisões

A reescrita desktop priorizou escolhas que aproximassem o projeto de um aplicativo real:

- `C# + WPF` para uma experiência desktop nativa e estável no Windows
- projeto de lógica de domínio separado da interface
- testes automatizados para o núcleo matemático
- testes automatizados de interface para fluxos críticos
- fluxos de distribuição, lock files e geração de hash
- persistência local para histórico e estado da sessão
- linguagem visual própria em preto e branco, evitando o visual padrão do framework

## O que mudou tecnicamente

A versão atual passou a incluir:

- lógica matemática modular
- validação de domínio para entradas inválidas
- interface WPF com identificadores de automação para testes de interface
- pacote portátil autossuficiente
- instalador via Inno Setup
- GitHub Actions para integração contínua e preparação de distribuição

## O que este projeto demonstra

Para uma pessoa recrutadora, este projeto foi pensado para demonstrar:

- visão de produto
- refatoração e modernização de código legado pessoal
- implementação de interface desktop
- automação de testes
- noções de distribuição e versionamento
- atenção a acabamento e experiência de desenvolvimento

## O que ele não tenta provar sozinho

Este projeto não foi posicionado como prova isolada de experiência com sistemas distribuídos de grande porte, autenticação complexa ou plataformas de API.

Ele funciona melhor no portfólio como um projeto polido e orientado a produto, complementando outros trabalhos com APIs, persistência ou arquitetura full stack.
