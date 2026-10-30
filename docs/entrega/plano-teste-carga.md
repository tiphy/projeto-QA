# Plano de teste de carga

## Objetivo e limites

Caracterizar a latência de leitura da API em desenvolvimento e detectar erros HTTP em chamadas concorrentes. Este teste exploratório não estabelece capacidade de produção, não cobre gravações concorrentes, persistência, autenticação, rede externa ou múltiplas instâncias.

## Procedimento reproduzível

1. Iniciar a aplicação localmente com `dotnet run --project src/AlocaSenac.Web/AlocaSenac.Web.csproj`.
2. Executar `python3 tests/load/smoke.py --url http://localhost:5206/api/alocacoes --requests 500 --concurrency 10`.
3. Registrar duração, taxa, p50, p95, latência máxima e quantidade de respostas diferente de HTTP 200 em `relatorio-teste-carga.md`.

O programa usa apenas a biblioteca padrão do Python, tem timeout de 10 segundos por chamada e retorna código diferente de zero se houver falha HTTP/transporte. Limites de aprovação de produção ainda precisam ser acordados com o responsável pelo serviço.
