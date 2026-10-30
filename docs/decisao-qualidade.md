# Decisão de qualidade — AlocaSenac

**Data da decisão:** 2026-10-30  
**Relator:** não identificado no pacote de evidências  
**Decisão:** **BLOQUEAR a entrega como versão final/tag avaliada.**  
**Uso permitido enquanto isso:** demonstração local, não produtiva, com as limitações abaixo explicitadas.

## Base e escopo da avaliação

As evidências documentais consultadas incluem `docs/entrega/checklist.md`, `plano-e-relatorio-de-testes.md`, `relatorio-de-qualidade.md`, `registro-defeitos.md`, `relatorio-teste-carga.md`, `docs/relatorio-qualidade.md`, `docs/rtm.md` e `docs/evidencias/README.md`.

O histórico local aponta o commit `1eaa6b1a49061be82a6deb06aa9901ebf0eaac1c` na branch `feature/setup-documentacao-docker`. Os documentos citam `v1.0.0-rc1` como candidata, mas não há tag local nem referência publicada dessa tag no remoto consultado. O worktree também contém alterações não commitadas. Portanto, não existe uma versão imutável que possa ser identificada como o objeto efetivamente avaliado; os resultados locais abaixo são os registrados nos relatórios, não uma validação independente deste relator sobre a árvore atual.

## Critérios e evidências

| Critério | Resultado | Evidência e fundamento |
|---|---|---|
| Build e testes funcionais locais | Atendido no escopo documentado | `plano-e-relatorio-de-testes.md` e `relatorio-de-qualidade.md` registram build Release com 0 avisos/erros e 10 testes aprovados (3 unidade, 6 integração e 1 especificação), executados localmente em 2026-10-30. Não comprovam a árvore atual nem um artefato publicado. |
| Requisitos e documentação rastreáveis | Parcialmente atendido | `docs/rtm.md`, `docs/arquitetura.md`, README e os planos descrevem implementação, verificações e limites. A rastreabilidade não substitui execução externa nem revisão independente. |
| Cobertura | Evidência local disponível; suficiência não estabelecida | O relatório registra 55,02% (197/358 linhas) no agregado de integração, incluindo código gerado. Não há meta acordada; não se interpreta o número como aprovação de cobertura. |
| Desempenho | Evidência exploratória, não critério de capacidade | `relatorio-teste-carga.md` registra 500/500 respostas HTTP 200, p95 de 17,7 ms, em loopback, somente leitura e concorrência 10. Não valida SLA, escrita concorrente ou produção. |
| CI hospedada no commit de entrega | Pendente — bloqueador | Workflow configurado em `.github/workflows/ci.yml`, mas checklist e relatórios dizem que a execução hospedada no SHA candidato e seu artefato ainda precisam ser confirmados. |
| Análise estática/SonarCloud | Pendente; JaCoCo não aplicável | Relatórios e `docs/evidencias/README.md` dizem que SonarCloud não foi executado por depender de configuração externa. O projeto é C#/.NET; Coverlet, não JaCoCo, é a ferramenta pertinente. |
| Revisão por pares | Pendente — bloqueador | O checklist e o relatório de qualidade não apresentam PR revisado/aprovado por outro colaborador. |
| Identidade e imutabilidade da versão | Não atendido — bloqueador | A candidata `v1.0.0-rc1` aparece nos documentos, mas não foi encontrada como tag local ou remota. Há mudanças não commitadas e o branch atual é de trabalho; não é possível ligar os resultados a uma versão final imutável. |
| Segurança para exposição/produção | Não atendido para produção | `registro-defeitos.md`, `docs/relatorio-qualidade.md` e `docs/rtm.md` registram ausência de autenticação/autorização real na API e na interface. Não há auditoria de segurança documentada. Não liberar como serviço multiusuário ou exposto. |
| Declaração nominal da equipe | Pendente — bloqueador da documentação de entrega | Não foi encontrado roster nem declaração individual de atividades. O histórico Git permite identificar autoria de commits, mas não comprova a divisão de trabalho nem substitui declaração dos integrantes. |

## Riscos aceitos e limitações de uso

Os riscos abaixo são considerados toleráveis **somente para demonstração local controlada**, sem dados sensíveis e sem alegação de prontidão para produção. A evidência não contém aceite nominal dos responsáveis; logo, este registro define o escopo recomendado de aceitação, não atribui aprovação a pessoas não identificadas.

| Risco | Impacto | Justificativa do aceite restrito | Responsável pelo aceite/mitigação |
|---|---|---|---|
| Sem autenticação/autorização real; API permite operações sem controle de identidade | Acesso não autorizado pode consultar, criar ou remover alocações; exposição indevida se publicada | Aceitável apenas em demonstração isolada, sem usuários reais ou dados sensíveis. Impede uso público/multiusuário | Responsável de produto e responsável técnico/segurança — nomes e aceite não informados |
| Dados mantidos em memória | Perda de alocações ao reiniciar e estado divergente entre instâncias | Coerente com demonstração efêmera; não atende operação com continuidade ou integridade de agenda | Responsável de produto e responsável técnico — nomes e aceite não informados |
| Não existe aprovação/recusa pelo responsável | Solicitações são confirmadas diretamente, podendo contrariar o processo esperado | Limitação documentada e explícita; aceitável somente se a demonstração tratar o cadastro direto como comportamento pretendido | Responsável de produto — nome e aceite não informados |
| Teste de carga local, de baixa intensidade e só de leitura | Capacidade, latência sob rede real e comportamento de escrita não conhecidos | Resultado serve apenas como smoke exploratório, não como compromisso de desempenho | Responsável de operação/desempenho — nome e aceite não informados |
| CI hospedada, SonarCloud e revisão por pares não comprovados | Regressões ou problemas não detectados podem chegar à entrega; confiança independente insuficiente | Não aceito como evidência de versão final; requer fechamento antes de remover o bloqueio | Responsável de release/CI e revisor independente — nomes e evidências pendentes |
| Sem tag verificável e com alterações no worktree | Não é possível reproduzir exatamente nem associar resultados à entrega avaliada | Não aceito para publicação/entrega final; criar commit/tag imutável e repetir as verificações pertinentes | Responsável de release — nome não informado |

## Justificativa e condições para reavaliar

Os resultados locais registrados indicam funcionamento básico do escopo de demonstração e justificam manter o projeto disponível para avaliação local controlada. Não sustentam uma liberação final: falta uma tag imutável, a execução remota de CI e seus artefatos não foram confirmados, não há revisão independente registrada e a equipe não está nominalmente declarada. As limitações de segurança, persistência e fluxo de aprovação também excluem qualquer interpretação de prontidão para produção.

Para reavaliar a entrega final: (1) integrar e registrar as mudanças num commit limpo; (2) publicar uma tag que aponte para esse SHA e executar CI nesse mesmo SHA, guardando os artefatos; (3) registrar revisão por pares; (4) registrar o resultado real do SonarCloud, se configurado ou exigido; (5) obter as declarações nominais de todos os integrantes e os aceites dos riscos de demonstração. Qualquer liberação para produção exige ainda tratar autenticação/autorização, persistência e os requisitos operacionais e de aprovação aplicáveis, além de validação de segurança e desempenho apropriada.

## Integrantes e declaração de atividades

Não há declarações individuais nem lista completa da equipe nos arquivos de evidência consultados. Para não inventar nomes ou atribuições, registra-se somente o dado verificável do histórico Git:

| Nome identificado | Atividade que a evidência permite declarar | Situação |
|---|---|---|
| tiphany | O histórico local atribui a autoria do commit `1eaa6b1` (`chore: prepare v1.0.0-rc1 release package`) e de commits anteriores. Isso comprova autoria desses commits, mas não detalha quais tarefas a integrante declara ter realizado. | Solicitar declaração individual e confirmar a lista completa de integrantes antes da entrega. |
| Demais integrantes | Não identificados nominalmente no pacote de evidências. | Declarações pendentes; não presumir que não existam integrantes adicionais. |

## Texto para o formulário do Hub

> Repositório público: https://github.com/tiphy/projeto-QA — tag avaliada: nenhuma tag publicada/verificável até 2026-10-30. `v1.0.0-rc1` é a candidata citada nos documentos, mas não foi localizada no remoto; não informar essa candidata como tag avaliada até publicá-la e confirmar o SHA.
