# ADR 0001: Escolha da stack

- **Status:** Aceite (stack presente na implementação atual)
- **Data:** 2026-11-02

## Contexto

O repositório implementa uma aplicação web para consulta e criação de alocações. A solução e os projetos usam .NET 10. O projeto web usa ASP.NET Core, Razor Pages e Minimal APIs; o pacote `Microsoft.AspNetCore.OpenApi` publica o contrato OpenAPI. O pipeline de páginas usa autenticação por cookie. O Dockerfile cria imagens de build/runtime .NET, e o Compose provisiona PostgreSQL 16.

Os projetos `Application`, `Domain` e `Infrastructure` existem, mas estão vazios além das referências de projeto. O armazenamento funcional está em `AlocacoesStore`, uma lista em memória. Apesar do PostgreSQL ser iniciado pelo Compose, não há conexão da aplicação com o banco. Também não foi identificada integração com API externa de notificações.

## Decisão

Registar a stack existente como a decisão tecnológica do protótipo:

- **Linguagem e runtime:** C# e .NET 10.
- **Servidor e interface:** ASP.NET Core com Razor Pages e renderização HTML no servidor.
- **API:** Minimal APIs HTTP/JSON, documentadas por OpenAPI.
- **Autenticação da interface:** middleware de cookie do ASP.NET Core.
- **Empacotamento:** Docker multi-stage e Docker Compose.
- **Persistência atual:** armazenamento singleton em memória. O PostgreSQL do Compose é infraestrutura provisionada, mas não é uma dependência funcional da aplicação.
- **Testes automatizados:** xUnit e `Microsoft.NET.Test.Sdk`; teste exploratório de carga em Python com biblioteca padrão.

Esta decisão documenta o que foi selecionado no código, não presume que a persistência ou os módulos em camadas estejam concluídos.

## Consequências

- Razor Pages e Minimal APIs permitem disponibilizar interface e endpoints no mesmo host ASP.NET Core.
- OpenAPI expõe um contrato consultável sem introduzir um servidor de documentação separado.
- Docker oferece uma imagem de execução e um ambiente local reproduzível; Compose pode iniciar o PostgreSQL para evolução futura.
- A lista em memória simplifica o protótipo, mas perde dados em reinícios e não suporta coordenação concorrente entre instâncias.
- O PostgreSQL não deve ser considerado persistência disponível até a aplicação implementar conexão, migrações e repositórios.
- Credenciais fixas e a ausência de autorização na API impedem considerar esta configuração adequada a produção.
