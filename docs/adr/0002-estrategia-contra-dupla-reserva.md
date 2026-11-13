# ADR 0002: Estratégia contra dupla reserva

- **Status:** Aceite para o armazenamento em memória atual; insuficiente para múltiplas instâncias
- **Data:** 2026-11-02

## Contexto

O endpoint `POST /api/alocacoes` delega a criação a `AlocacoesStore.TentarCriar`. O método verifica sobreposição da sala pelo intervalo semiaberto (`inicio < existente.fim` e `fim > existente.inicio`) e acrescenta a nova alocação. Uma operação de verificação seguida de inclusão precisa ser atómica relativamente às demais chamadas. O armazenamento é uma lista em memória num singleton e o PostgreSQL do Compose não está ligado à aplicação.

## Decisão

Adotar, no escopo atual de processo único, a sincronização pessimista com `lock (_sync)` em torno da verificação de conflito e da inclusão em `TentarCriar`. O mesmo objeto de sincronização protege as operações de leitura e remoção. Assim, apenas uma thread por vez percorre a secção crítica; a primeira criação conflitante é acrescentada e as seguintes observam o conflito e são rejeitadas.

Esta decisão não afirma uma garantia transacional entre processos, contentores ou instâncias. Para escalar, a evolução deve mover a decisão de unicidade para armazenamento compartilhado e testar a estratégia com o motor real. Uma transação serializável ou mecanismo de exclusão/constraint que suporte intervalos pode ser avaliado quando o modelo persistente e o banco estiverem definidos.

## Consequências

- No processo atual, evita a condição de corrida entre verificação e inclusão sem dependência de banco.
- A secção crítica serializa chamadas de criação concorrentes, o que pode limitar throughput quando há contenção.
- Reinícios perdem as reservas; instâncias distintas têm listas e locks independentes, podendo aceitar duplicados.
- `lock` em memória não substitui constraint, transação ou coordenação distribuída.
- Ao adotar persistência, é necessário implementar e testar atomicidade no banco, incluindo concorrência entre conexões/instâncias e intervalos adjacentes.
- Conflitos atuais consideram apenas a sala; esta decisão não implementa deteção para professor ou material.
