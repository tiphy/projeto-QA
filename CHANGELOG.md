# Changelog

## [1.0.0-rc1] — candidato a release

Esta versão candidata reúne a interface e API demonstrativas para gestão de alocações de espaços acadêmicos.

### Incluído

- Interface Razor Pages com navegação entre os perfis demonstrativos Solicitante, Responsável e Administrador.
- Consulta da agenda, criação de alocações e remoção pelo perfil Administrador.
- API HTTP para listar, consultar, criar e remover alocações, com validação de intervalo e conflito de sala/horário.
- Contrato OpenAPI em `/openapi/v1.json`.
- Testes de unidade, integração e especificação; geração de cobertura .NET pelo Coverlet.
- Workflow GitHub Actions para build, testes e publicação de resultados de cobertura.
- Documentação de execução, rastreabilidade, arquitetura, sequências, plano de testes e resultados exploratórios.

### Limitações conhecidas

- Os perfis são demonstrações de interface, não autenticação ou autorização.
- Os dados ficam em memória e são perdidos ao reiniciar a aplicação.
- Não há fluxo de aprovação/recusa, gestão de usuários, salas ou recursos, nem notificações.
- A medição de carga é exploratória, local e não estabelece capacidade ou SLA de produção.
- JaCoCo não é aplicável ao projeto C#/.NET; a cobertura é fornecida pelo Coverlet.

> A tag `v1.0.0-rc1` só deve ser publicada depois que as condições do checklist de release forem atendidas.
