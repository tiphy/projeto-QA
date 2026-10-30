# Relatório de qualidade — candidato v1.0.0-rc1

**Data da validação local:** 2026-10-30
**SDK:** .NET 10.0.401
**Commit/tag:** conferir a tag anotada `v1.0.0-rc1`; este relatório deve ser consultado no conteúdo versionado pela tag.

## Resultados

- `dotnet restore AlocaSenac.slnx`: concluído.
- `dotnet build AlocaSenac.slnx --no-restore --configuration Release`: sucesso, 0 avisos e 0 erros.
- `dotnet test AlocaSenac.slnx --no-build --no-restore --configuration Release --settings coverlet.runsettings --collect:"XPlat Code Coverage" --logger "trx;LogFileName=tests.trx"`: 10 testes aprovados, 0 falhas e 0 ignorados (3 unidade, 6 integração e 1 especificação).
- Coverlet produziu relatórios Cobertura e OpenCover. Na execução de integração local registrada, a cobertura agregada foi 197/358 linhas (55,02%), incluindo código gerado pelo OpenAPI/Razor. Não foi definida meta de cobertura.
- Teste local de leitura concorrente: 500/500 respostas HTTP 200; p95 observado 17,7 ms. Resultado exploratório, não representa SLA ou capacidade de produção.

## Qualidade hospedada

- O workflow CI foi corrigido na branch de trabalho, mas a execução remota no commit de `main` ainda não foi aprovada. A publicação do relatório de cobertura como artifact só pode ser confirmada após execução no commit candidato.
- JaCoCo é ferramenta Java e não se aplica a este projeto C#/.NET. A cobertura apropriada é Coverlet (Cobertura/OpenCover), configurada no workflow.
- SonarCloud não foi executado nesta revisão. O job depende de `SONAR_ORGANIZATION`, `SONAR_PROJECT_KEY` e `SONAR_TOKEN` configurados no GitHub; não há Quality Gate aprovado para declarar.

## Riscos e limitações conhecidos

Os perfis de demonstração não autenticam usuários; a API não impõe autorização; os dados são voláteis; o fluxo de aprovação está ausente. A medição de carga é local. Uma revisão por pares precisa ser realizada por outro colaborador e registrada em PR.

Ver `docs/evidencias/README.md` para a evidência hospedada associada ao commit da tag após a publicação.
