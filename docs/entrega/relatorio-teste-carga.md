# Relatório de teste de carga exploratório

**Executado em:** 2026-10-30
**Ambiente:** aplicação Release e cliente no mesmo ambiente de desenvolvimento, via loopback HTTP
**Comando:** `python3 tests/load/smoke.py --url http://127.0.0.1:5206/api/alocacoes --requests 500 --concurrency 10`

| Métrica | Resultado |
|---|---:|
| Requisições | 500 |
| Concorrência | 10 |
| HTTP 200 | 500 |
| Falhas HTTP/transporte | 0 |
| Duração observada | 0,767 s |
| Taxa observada | 652,0 req/s |
| Latência p50 | 6,5 ms |
| Latência p95 | 17,7 ms |
| Latência máxima | 376,9 ms |

O resultado é uma caracterização exploratória de baixa intensidade, em loopback e sem concorrência de escrita. Não representa capacidade de produção nem estabelece um SLA; os valores dependem do ambiente compartilhado e não devem ser comparados sem repetir o mesmo procedimento em ambiente controlado.
