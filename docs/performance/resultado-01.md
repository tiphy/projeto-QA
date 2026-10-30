# Resultado de desempenho 01 — leitura concorrente da API

**Data:** 2026-10-30
**Ambiente:** aplicação Release e cliente no mesmo ambiente de desenvolvimento, via loopback HTTP.
**Comando:** `python3 tests/load/smoke.py --url http://127.0.0.1:5206/api/alocacoes --requests 500 --concurrency 10`

| Métrica | Resultado |
|---|---:|
| Requisições | 500 |
| Concorrência | 10 |
| Respostas HTTP 200 | 500 |
| Falhas HTTP/transporte | 0 |
| Duração observada | 0,767 s |
| Taxa observada | 652,0 req/s |
| Latência p50 | 6,5 ms |
| Latência p95 | 17,7 ms |
| Latência máxima | 376,9 ms |

Este é um teste exploratório de baixa intensidade, em loopback e somente de leitura. Não mede concorrência de escrita, múltiplas instâncias ou condições de produção; não deve ser interpretado como capacidade garantida ou SLA.
