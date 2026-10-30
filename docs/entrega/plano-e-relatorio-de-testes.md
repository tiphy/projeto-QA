# Plano e relatório de testes

## Escopo

Verificar build da solução, regras de conflito/remoção, contratos HTTP, OpenAPI, navegação dos três perfis e resposta básica sob concorrência. Os perfis não representam autenticação; persistência e fluxo de aprovação estão fora do comportamento entregue.

## Plano

| Tipo | Cobertura | Execução |
|---|---|---|
| Unidade | Conflito ignorando caixa, horários adjacentes e remoção idempotente | `dotnet test AlocaSenac.slnx` |
| Integração HTTP | Lista, consulta, criação, conflito, intervalo inválido, exclusão e OpenAPI | `AlocacoesApiTests` usando `WebApplicationFactory` |
| Interface | Abrir cada perfil e verificar seu conteúdo | Teoria de integração `DeveAbrirCadaPerfilPeloLinkDeNavegacao` |
| Build | Compilar todos os projetos da solução | `dotnet build AlocaSenac.slnx` |
| Cobertura | Gerar Cobertura e OpenCover com Coverlet | `dotnet test AlocaSenac.slnx --settings coverlet.runsettings --collect:"XPlat Code Coverage"` |
| Carga exploratória | Leituras concorrentes de `GET /api/alocacoes` | `python3 tests/load/smoke.py --url http://localhost:5206/api/alocacoes` |

## Resultado desta revisão

Executado em 2026-10-30 com .NET SDK 10.0.401:

- `dotnet restore AlocaSenac.slnx`: concluído.
- `dotnet build AlocaSenac.slnx --no-restore --configuration Release`: sucesso, 0 avisos e 0 erros.
- `dotnet test AlocaSenac.slnx --no-build --no-restore --configuration Release --settings coverlet.runsettings --collect:"XPlat Code Coverage"`: 10 testes aprovados, 0 falhas e 0 ignorados (3 unidade, 6 integração e 1 especificação).
- Coverlet gerou arquivos Cobertura e OpenCover para os três projetos de teste. No relatório de integração, o agregado registrou 197 de 358 linhas cobertas (55,02%); o agregado inclui código gerado pelo OpenAPI/Razor e não é uma meta de cobertura definida.
- `python3 tests/load/smoke.py --url http://127.0.0.1:5206/api/alocacoes --requests 500 --concurrency 10`: 500 respostas HTTP 200 e nenhuma falha. Métricas e limitações em `relatorio-teste-carga.md`.

Os artefatos XML de cobertura são produzidos em `TestResults/` e publicados como artefatos pela CI; os resultados locais de build não são versionados.
