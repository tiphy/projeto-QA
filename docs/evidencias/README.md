# Índice de evidências — `v1.0.0-rc1`

Este índice é versionado junto com a versão candidata. Os links de código/documentação usam a referência imutável `v1.0.0-rc1`; após criar a tag, confirme que ela aponta para o SHA do commit aprovado. Não mova nem reutilize uma tag publicada. Se algum arquivo precisar mudar depois, publique `v1.0.0-rc2`.

| Evidência | Link / localização | Estado |
|---|---|---|
| Commit avaliado pela tag | [Commit `v1.0.0-rc1`](https://github.com/tiphy/projeto-QA/commits/v1.0.0-rc1) | Só válido após publicação da tag. |
| Execução do CI para o commit da tag | [Execuções CI filtradas pelo ref `v1.0.0-rc1`](https://github.com/tiphy/projeto-QA/actions/workflows/ci.yml?query=branch%3Av1.0.0-rc1) | Abra a execução cujo `head SHA` seja o commit da tag. A execução hospedada ainda precisa ocorrer. |
| Artifact de cobertura | Artifact `test-and-coverage-results` na execução CI acima; contém TRX, Cobertura e OpenCover | Coverlet/.NET substitui JaCoCo, que é específico de Java e não é aplicável a este repositório. O artifact só existirá após a execução CI. |
| Quality Gate SonarCloud | [Projeto SonarCloud configurado em `sonar-project.properties`](https://sonarcloud.io/project/overview?id=tiphy_projeto-QA) | Não confirmado. Requer organização, chave válida e `SONAR_TOKEN`; a análise deve incluir o SHA da tag. |
| Resultado de desempenho 01 | [docs/performance/resultado-01.md](https://github.com/tiphy/projeto-QA/blob/v1.0.0-rc1/docs/performance/resultado-01.md) | Teste local exploratório; limites descritos no relatório. |
| Matriz de rastreabilidade | [docs/rtm.md](https://github.com/tiphy/projeto-QA/blob/v1.0.0-rc1/docs/rtm.md) | Artefato versionado pela tag. |
| Defeitos e limitações | [docs/defeitos/](https://github.com/tiphy/projeto-QA/tree/v1.0.0-rc1/docs/defeitos) | Artefato versionado pela tag. |
| Relatório de qualidade | [docs/relatorio-qualidade.md](https://github.com/tiphy/projeto-QA/blob/v1.0.0-rc1/docs/relatorio-qualidade.md) | Artefato versionado pela tag; diferencia validação local de evidência hospedada. |
| Plano e relatório de testes | [docs/entrega/plano-e-relatorio-de-testes.md](https://github.com/tiphy/projeto-QA/blob/v1.0.0-rc1/docs/entrega/plano-e-relatorio-de-testes.md) | Artefato versionado pela tag. |
| Registro de defeitos detalhado | [docs/entrega/registro-defeitos.md](https://github.com/tiphy/projeto-QA/blob/v1.0.0-rc1/docs/entrega/registro-defeitos.md) | Artefato versionado pela tag. |

## Condições para fechar o pacote

- Confirmar CI verde na `main` para o mesmo SHA que será anotado pela tag e confirmar o artifact de cobertura na execução.
- Executar SonarCloud para esse SHA e registrar o estado real do Quality Gate; não declarar aprovação sem resultado.
- Obter a revisão por pares no PR e verificar que o SHA aprovado não mudou.
- JaCoCo não é um bloqueio substituível por um arquivo renomeado: se a avaliação exigir literalmente JaCoCo, esclarecer a incompatibilidade com C#/.NET. O artifact correto do projeto é Coverlet.
