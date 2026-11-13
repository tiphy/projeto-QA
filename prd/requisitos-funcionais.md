# Requisitos funcionais

Este documento descreve os requisitos obrigatórios e o estado observado no código-fonte. Os critérios BDD definem o comportamento de aceite; quando a implementação é parcial ou inexistente, isso está indicado explicitamente e o critério representa uma lacuna a implementar, não uma funcionalidade já entregue.

**Escala de risco para a RTM:** Alto para falhas que comprometem autorização, integridade ou concorrência; Médio para fluxos operacionais relevantes; Baixo para documentação e apresentação.

## RF01 — Autenticação e autorização por perfil

- **Descrição e implementação atual:** A aplicação autentica por cookie e protege as pastas Dashboard, Salas, Agendamentos, Relatórios e Configurações. O login compara credenciais de demonstração fixas (`admin@gmail.com` / `1234`) e cria uma identidade de Administrador. Não há políticas para perfis distintos; os endpoints `/api/alocacoes` não são protegidos por autorização.
- **Risco para a RTM:** Alto.
- **Critérios de aceite (BDD):**
  - **Dado** um utilizador sem sessão, **Quando** tentar aceder a uma página protegida, **Então** deverá ser encaminhado para o login e não poderá consultar nem alterar dados protegidos.
  - **Dado** um utilizador autenticado com determinado perfil, **Quando** executar uma ação autorizada para outro perfil, **Então** a aplicação deverá negar a operação sem alterar os dados.

## RF02 — Cadastro e consulta de salas, professores e materiais

- **Descrição e implementação atual:** A página Salas apresenta ambientes de exemplo e formulários de cadastro/edição. Os handlers `OnPost` de Salas e Editar não persistem alterações; detalhes e disponibilidade também contêm dados fixos. Não foi identificada gestão de professores ou materiais, nem API de catálogo. O `AlocacoesStore` mantém apenas alocações em memória.
- **Risco para a RTM:** Alto.
- **Critérios de aceite (BDD):**
  - **Dado** um utilizador autorizado e os dados obrigatórios de uma sala, professor ou material, **Quando** cadastrar ou atualizar o registo, **Então** o catálogo deverá persistir os dados e apresentá-los na consulta seguinte.
  - **Dado** um registo existente, **Quando** consultar pelo identificador, **Então** deverão ser apresentados os dados atuais, ou uma resposta de não encontrado se o registo não existir.

## RF03 — Pesquisa por tipo, capacidade, localização, competência e disponibilidade

- **Descrição e implementação atual:** A página Salas exibe filtros de texto, bloco, andar e tipo, e apresenta salas de exemplo. Os filtros não são recebidos nem aplicados pelo PageModel; não há pesquisa implementada por capacidade, competência ou disponibilidade.
- **Risco para a RTM:** Médio.
- **Critérios de aceite (BDD):**
  - **Dado** um catálogo de salas, professores e materiais, **Quando** o utilizador combinar filtros por tipo, capacidade, localização, competência ou intervalo de disponibilidade, **Então** deverá receber apenas os recursos que satisfaçam todos os critérios informados.
  - **Dado** que nenhum recurso satisfaça os filtros, **Quando** a pesquisa for concluída, **Então** a interface deverá informar que não foram encontrados resultados.

## RF04 — Criação, alteração e cancelamento de reservas

- **Descrição e implementação atual:** `POST /api/alocacoes` cria uma alocação após validar sala, professor e intervalo; `DELETE /api/alocacoes/{id}` remove uma alocação. Não há operação de alteração nem cancelamento com estado/histórico. O formulário e os botões da página de Agendamentos não implementam estes handlers.
- **Risco para a RTM:** Alto.
- **Critérios de aceite (BDD):**
  - **Dado** um utilizador autorizado e um intervalo válido, **Quando** submeter uma nova reserva sem conflito, **Então** a reserva deverá ser criada e identificada na resposta.
  - **Dado** uma reserva ainda não iniciada, **Quando** o seu responsável a alterar ou cancelar, **Então** a alteração deverá ser validada e o cancelamento deverá preservar o registo e o respetivo estado, sem apagamento irreversível.

## RF05 — Deteção de sobreposição de horários, inclusive na agenda do professor

- **Descrição e implementação atual:** A criação pela API rejeita sobreposição da mesma sala com HTTP 409, usando intervalos semiabertos (horários adjacentes são permitidos). Não verifica conflitos de professor ou material. A agenda do professor não é implementada.
- **Risco para a RTM:** Alto.
- **Critérios de aceite (BDD):**
  - **Dado** uma reserva existente de sala, professor ou material, **Quando** for submetida outra reserva que se sobreponha no tempo ao mesmo recurso, **Então** a nova reserva deverá ser rejeitada e o conflito identificado.
  - **Dado** duas reservas em que o término da primeira coincide com o início da segunda, **Quando** forem submetidas para o mesmo recurso, **Então** ambas poderão ser aceites por não haver sobreposição.

## RF06 — Proteção contra dupla reserva em solicitações simultâneas

- **Descrição e implementação atual:** `AlocacoesStore.TentarCriar` sincroniza a verificação de conflito de sala e a inclusão com `lock`, impedindo duas criações conflitantes dentro do mesmo processo. O armazenamento é em memória e não fornece garantia transacional entre instâncias ou processos.
- **Risco para a RTM:** Alto.
- **Critérios de aceite (BDD):**
  - **Dado** duas solicitações simultâneas para o mesmo recurso e intervalo conflitante, **Quando** ambas forem processadas, **Então** exatamente uma deverá ser aceite e a outra deverá receber indicação de conflito.
  - **Dado** que a aplicação opere com mais de uma instância, **Quando** as mesmas solicitações concorrentes forem encaminhadas a instâncias diferentes, **Então** a garantia de unicidade deverá continuar válida.

## RF07 — Aprovação obrigatória para recursos restritos

- **Descrição e implementação atual:** Não há estado de aprovação nem identificação operacional de recurso restrito. A página de Agendamentos apresenta botões Aprovar e Recusar sem handlers funcionais; as criações pela API são aceites diretamente quando não há conflito.
- **Risco para a RTM:** Alto.
- **Critérios de aceite (BDD):**
  - **Dado** um pedido de reserva de recurso restrito, **Quando** for submetido, **Então** deverá ficar pendente e não poderá ser confirmado sem aprovação.
  - **Dado** um utilizador que não seja o Responsável pelo recurso, **Quando** tentar aprovar o pedido, **Então** a operação deverá ser negada e o estado permanecer inalterado.

## RF08 — Bloqueio de recursos em manutenção

- **Descrição e implementação atual:** Não há estado de manutenção nem bloqueio de recursos na API ou no armazenamento atual.
- **Risco para a RTM:** Alto.
- **Critérios de aceite (BDD):**
  - **Dado** um recurso marcado como em manutenção durante determinado intervalo, **Quando** for solicitada uma reserva que intersete esse intervalo, **Então** a reserva deverá ser rejeitada e o motivo informado.
  - **Dado** um intervalo fora da manutenção, **Quando** for submetida uma reserva válida, **Então** o bloqueio de manutenção não deverá impedir a operação.

## RF09 — Registo de retirada e devolução de materiais

- **Descrição e implementação atual:** Não há cadastro operacional de materiais nem registo de retirada, devolução, responsável ou condição do item.
- **Risco para a RTM:** Médio.
- **Critérios de aceite (BDD):**
  - **Dado** um material reservado e aprovado, **Quando** for entregue ao requisitante, **Então** deverão ser registados material, reserva, responsável e data/hora da retirada.
  - **Dado** um material retirado, **Quando** for devolvido, **Então** deverão ser registados data/hora e condição da devolução, mantendo o histórico consultável.

## RF10 — Histórico auditável de mudanças

- **Descrição e implementação atual:** Não há registo de auditoria. As alocações ficam numa lista em memória e podem ser removidas sem histórico de autor, data ou estado anterior.
- **Risco para a RTM:** Alto.
- **Critérios de aceite (BDD):**
  - **Dado** qualquer alteração de estado de uma reserva ou recurso, **Quando** a alteração for efetivada, **Então** deverá ser gravado um evento imutável com entidade, estado anterior e novo, autor e data/hora.
  - **Dado** um utilizador autorizado a consultar auditoria, **Quando** pesquisar uma entidade, **Então** deverá conseguir consultar os eventos ordenados cronologicamente sem os alterar.

## RF11 — Notificação simulada ou integração com API externa

- **Descrição e implementação atual:** Não há envio ou simulação de notificações. A página Configurações apresenta opções de notificações, mas o seu handler `OnPost` é vazio e não guarda preferências.
- **Risco para a RTM:** Médio.
- **Critérios de aceite (BDD):**
  - **Dado** uma alteração relevante de reserva, **Quando** ocorrer criação, aprovação, recusa, alteração ou cancelamento, **Então** o sistema deverá gerar uma notificação simulada registada ou enviá-la pela integração externa configurada.
  - **Dado** uma falha da API externa, **Quando** a notificação for tentada, **Então** a falha deverá ser registada e não deverá desfazer a alteração já confirmada.

## RF12 — Relatório de utilização por recurso, carga horária e conflitos evitados

- **Descrição e implementação atual:** A página Relatórios calcula apenas a contagem de alocações para `TotalAgendamentos`. Taxa de ocupação e horas reservadas permanecem nos valores padrão; gráficos por tipo e dia são marcadores de conteúdo. Não há métrica de conflitos evitados.
- **Risco para a RTM:** Médio.
- **Critérios de aceite (BDD):**
  - **Dado** um período e reservas confirmadas, **Quando** um utilizador autorizado consultar o relatório, **Então** deverá ver utilização por recurso, horas reservadas e total de reservas no período, calculados a partir dos registos.
  - **Dado** conflitos de reserva rejeitados no período, **Quando** o relatório for consultado, **Então** deverá apresentar a quantidade de conflitos evitados, sem os contabilizar como reservas confirmadas.

## RF13 — Interface responsiva e mensagens de erro compreensíveis

- **Descrição e implementação atual:** As páginas usam rótulos, regiões de alerta e contêineres de tabela (`table-wrap`); o login apresenta erro compreensível e a API retorna mensagens para entrada inválida e conflito. A responsividade não está demonstrada por teste automatizado, e os formulários de catálogo/agendamento não apresentam validação funcional nos handlers.
- **Risco para a RTM:** Médio.
- **Critérios de aceite (BDD):**
  - **Dado** um ecrã estreito ou largo, **Quando** o utilizador abrir qualquer fluxo principal, **Então** conteúdo, ações e tabelas deverão permanecer legíveis e utilizáveis sem perda de informação.
  - **Dado** uma entrada inválida ou uma operação não permitida, **Quando** a aplicação a rejeitar, **Então** deverá explicar o problema e a ação corretiva sem expor detalhes internos.

## RF14 — Documentação da API

- **Descrição e implementação atual:** `Program.cs` mapeia OpenAPI em `/openapi/v1.json`. O contrato documenta `GET /api/alocacoes`, `GET /api/alocacoes/{id}`, `POST /api/alocacoes` e `DELETE /api/alocacoes/{id}`, com sumários e respostas declaradas. Não foram identificados endpoints de alteração, aprovação, catálogo ou relatórios.
- **Risco para a RTM:** Baixo.
- **Critérios de aceite (BDD):**
  - **Dado** que a aplicação esteja em execução, **Quando** um consumidor aceder a `/openapi/v1.json`, **Então** deverá receber o contrato OpenAPI válido com métodos, rotas, formatos e respostas dos endpoints publicados.
  - **Dado** uma alteração nos endpoints ou nos seus códigos de resposta, **Quando** a API for atualizada, **Então** o contrato deverá refletir a alteração.

## Regras de negócio críticas

| ID | Regra | Implementação observada |
|---|---|---|
| RN01 | O término da reserva deve ser estritamente posterior ao início. | **Implementada na API:** `Inicio >= Fim` retorna HTTP 400. |
| RN02 | Não pode haver sobreposição de sala, material ou professor. | **Parcial:** a API verifica apenas sala; material e professor não são verificados. |
| RN03 | Dupla submissão simultânea conflitante deve gerar apenas uma reserva aceite. | **Parcial:** `lock` protege uma instância/processo; não garante unicidade entre múltiplas instâncias. |
| RN04 | Recurso em manutenção não pode ser reservado. | **Não implementada:** não há estado ou intervalo de manutenção. |
| RN05 | Apenas o Responsável pode aprovar recurso restrito. | **Não implementada:** não há autorização por perfil nem aprovação funcional. |
| RN06 | Reserva iniciada não pode ser apagada/excluída. | **Não implementada:** `DELETE /api/alocacoes/{id}` remove sem verificar início ou preservar estado. |
| RN07 | Toda alteração de estado gera registo de auditoria. | **Não implementada:** não há histórico de auditoria. |
