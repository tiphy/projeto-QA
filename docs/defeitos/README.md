# Registro de defeitos e limitações

Revisado em 2026-10-30. Os itens abaixo são limitações conhecidas desta versão candidata; devem ser resolvidos antes de qualquer implantação que dependa de autenticação, persistência ou aprovação.

| ID | Estado | Descrição | Impacto |
|---|---|---|---|
| DEF-001 | Aberto | Os perfis de demonstração não autenticam nem autorizam usuários; a API também não impõe permissões. | Não disponibilizar como serviço multiusuário/produção sem identidade e autorização. |
| DEF-002 | Aberto | `AlocacoesStore` guarda dados somente em memória. | Reiniciar o processo perde as alocações e instâncias distintas não compartilham estado. |
| DEF-003 | Aberto | O Responsável não aprova nem recusa solicitações. | A criação confirma diretamente uma alocação. |
| DEF-004 | Não aplicável | JaCoCo não suporta instrumentação de C#/.NET neste fluxo. | Usar os relatórios Cobertura/OpenCover do Coverlet, publicados como artifact pela CI. |

Nenhum defeito funcional adicional foi confirmado pelos testes automatizados desta revisão. Isso não constitui auditoria de segurança ou garantia de ausência de defeitos.
