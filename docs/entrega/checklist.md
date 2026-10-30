# Checklist de entrega

Revisão do estado do repositório em 2026-10-30. A validação local usou .NET SDK 10.0.401; resultados completos e limites estão em `plano-e-relatorio-de-testes.md` e `relatorio-de-qualidade.md`.

| Item do checklist original | Estado atual | Evidência / pendência |
|---|---|---|
| Aplicação executável | Resolvido localmente | Build Release, testes HTTP via `WebApplicationFactory` e teste local de API aprovados. Para iniciar, ver `README.md`. |
| README | Resolvido | Instruções de restauração, execução, API, teste, perfis demonstrativos e limitações. |
| Documentação da API | Resolvido | OpenAPI em `/openapi/v1.json`; endpoint coberto por teste de integração. |
| RTM | Resolvido | `docs/rtm.md` liga requisitos a código e testes. |
| Diagrama de contexto | Resolvido | `docs/arquitetura.md`. |
| Diagrama de componentes | Resolvido | `docs/arquitetura.md`. |
| Diagramas de sequência | Resolvido | `prd/fluxos-sequencia.md` cobre criação na interface/API, consulta e remoção. |
| Plano e relatório de testes | Resolvido para validação local | `docs/entrega/plano-e-relatorio-de-testes.md`; 10 testes aprovados. |
| Pipeline de CI | Configurado; execução remota pendente | `.github/workflows/ci.yml` faz restore, build Release, testes e publica resultados. Requer push/PR para comprovar execução hospedada. |
| SonarCloud | Configurado condicionalmente; relatório pendente | `sonar-project.properties` e job opcional na CI. Requer `SONAR_ORGANIZATION`, `SONAR_PROJECT_KEY` e `SONAR_TOKEN` no GitHub. |
| JaCoCo | Não aplicável | Ferramenta Java; este projeto usa .NET/C# e Coverlet. |
| Relatório de cobertura | Resolvido localmente | Coverlet gera Cobertura/OpenCover; 55,02% de linhas no agregado do projeto de integração, incluindo código gerado. CI publica os relatórios como artefatos. |
| Plano e resultado de teste de carga | Resolvido como teste exploratório | `plano-teste-carga.md` e `relatorio-teste-carga.md`; 500 leituras concorrentes em loopback. Não é teste de capacidade de produção. |
| Registro de defeitos | Resolvido | `registro-defeitos.md` registra limitações conhecidas de segurança, persistência e fluxo de aprovação. |
| Relatório de qualidade | Resolvido para o escopo validado | `relatorio-de-qualidade.md`; registra resultados e restrições não verificadas. |
| Revisão por pares | Pendente externo | Não é possível produzir evidência de revisão independente localmente. Requer PR revisado/aprovado por outro colaborador. |

## Ajustes em relação à avaliação anterior

- O entrypoint já contém Razor Pages e API de alocações; os testes agora verificam listagem, criação, conflito, remoção, OpenAPI e navegação entre os perfis.
- O README documenta comandos compatíveis com o projeto .NET. `./mvnw -B verify` não se aplica: não há Maven nem wrapper Maven neste repositório. A validação equivalente deste projeto é `dotnet build` e `dotnet test`.
- JaCoCo foi substituído por Coverlet, ferramenta compatível com .NET.
- SonarCloud e revisão por pares continuam dependentes de configuração/ação externa e não são declarados como concluídos.
