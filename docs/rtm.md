# Matriz de rastreabilidade

| ID | Requisito verificável | Implementação | Verificação |
|---|---|---|---|
| RF-01 | Listar as alocações em ordem de início | `AlocacoesStore.Listar`; `GET /api/alocacoes`; agenda em `Pages/Index.cshtml` | `AlocacoesApiTests.DeveListarAlocacoesEExporOpenApi` |
| RF-02 | Consultar uma alocação por identificador e responder não encontrado quando ausente | `GET /api/alocacoes/{id}` | `AlocacoesApiTests.DeveCriarRejeitarConflitoEExcluirAlocacao` |
| RF-03 | Criar alocação com sala, responsável e intervalo válido | `POST /api/alocacoes`; handler `OnPostCreate` | `AlocacoesApiTests.DeveRetornarBadRequestParaIntervaloInvalido` |
| RF-04 | Rejeitar alocação que sobrepõe outra na mesma sala, sem diferenciar caixa | `AlocacoesStore.TentarCriar` | `AlocacoesStoreTests.DeveImpedirSobreposicaoSemDiferenciarMaiusculas`; `AlocacoesApiTests.DeveCriarRejeitarConflitoEExcluirAlocacao` |
| RF-05 | Permitir horários adjacentes que não se sobrepõem | `AlocacoesStore.TentarCriar` | `AlocacoesStoreTests.DevePermitirHorariosAdjacentes` |
| RF-06 | Permitir remoção de alocação existente pela interface do administrador e API | `OnPostDelete`; `DELETE /api/alocacoes/{id}` | `AlocacoesStoreTests.DeveRemoverUmaAlocacaoExistenteUmaUnicaVez`; `AlocacoesApiTests.DeveCriarRejeitarConflitoEExcluirAlocacao` |
| RF-07 | Navegar entre modos Solicitante, Responsável e Administrador | Links de perfil em `Pages/Index.cshtml`; normalização em `IndexModel` | `AlocacoesApiTests` verifica a API; smoke manual da interface descrito em `docs/entrega/plano-e-relatorio-de-testes.md` |
| RF-08 | Disponibilizar contrato OpenAPI | `Program.cs` (`MapOpenApi`) | `AlocacoesApiTests.DeveListarAlocacoesEExporOpenApi` |

## Fora do escopo implementado

Aprovação/recusa, autenticação/autorização real, persistência após reinício, gestão de espaços e recursos, usuários e notificações não estão implementados. A seleção de perfil é demonstrativa; não deve ser tratada como mecanismo de segurança.
