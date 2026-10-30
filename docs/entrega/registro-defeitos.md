# Registro de defeitos e limitações

Atualizado em 2026-10-30 após execução dos testes automatizados e da verificação exploratória descritas em `plano-e-relatorio-de-testes.md`.

| ID | Estado | Descrição | Evidência / impacto |
|---|---|---|---|
| DEF-001 | Limitação aberta | Os perfis de demonstração não autenticam nem autorizam usuários; a API também não impõe permissões. | Não disponibilizar como serviço multiusuário/produção sem implementar identidade e autorização. |
| DEF-002 | Limitação aberta | Os dados do `AlocacoesStore` existem apenas em memória. | Reiniciar o processo perde as alocações e cada instância mantém seu próprio estado. |
| DEF-003 | Limitação aberta | O responsável apenas consulta a agenda; não há aprovação nem recusa. | Solicitações são criadas diretamente como alocações. |

Nenhum defeito funcional adicional foi confirmado pelos testes automatizados incluídos nesta entrega. Isso não equivale a uma auditoria de segurança, teste de produção ou garantia de ausência de defeitos.
