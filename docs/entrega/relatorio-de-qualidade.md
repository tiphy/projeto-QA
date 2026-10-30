# Relatório de qualidade

**Data:** 2026-10-30
**Escopo:** solução .NET AlocaSenac; build, testes automatizados, documentação de entrega e teste exploratório de leitura HTTP.

## Evidências

- Build Release concluído com 0 avisos e 0 erros.
- 10 testes automatizados aprovados: 3 unidade, 6 integração e 1 especificação.
- Cobertura Coverlet em Cobertura e OpenCover; agregado do relatório de integração: 55,02% de linhas (inclui código gerado).
- Teste exploratório local: 500/500 respostas HTTP 200, sem falhas; p95 observado de 17,7 ms. Não é uma medição de capacidade de produção.
- O workflow GitHub Actions executa restore, build, testes e publica cobertura. A execução hospedada do workflow ainda depende de push/PR.

## Estado da análise estática

JaCoCo não se aplica a C#/.NET; Coverlet é a ferramenta configurada. O projeto e o workflow SonarCloud foram preparados, mas nenhum relatório Sonar foi obtido nesta revisão porque organização, chave de projeto e token são configuração externa. Não afirmar aprovação SonarCloud até executar a análise.

## Riscos e limites

Os perfis de demonstração não autenticam usuários; a API não impõe autorização; os dados são voláteis; não existe fluxo de aprovação. O teste de carga é local e exploratório. Uma revisão por pares precisa ser realizada por outro colaborador e registrada em PR; não foi possível comprová-la nesta revisão.
