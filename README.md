# AlocaSenac

Aplicação de demonstração para consultar e cadastrar alocações de salas, laboratórios e auditórios, com validação de conflito de horário. Os dados ficam em memória e são reiniciados quando a aplicação para.

## Requisitos

- .NET SDK 10.0

## Executar

Na raiz do repositório:

```bash
dotnet restore AlocaSenac.slnx
dotnet run --project src/AlocaSenac.Web/AlocaSenac.Web.csproj
```

Abra `http://localhost:5206`. Para interromper, pressione `Ctrl+C`.

## Interface e perfis de demonstração

Use as abas no topo para alternar entre **Solicitante**, **Responsável** e **Administrador**. A seleção de perfil não é autenticação nem controle de acesso real.

- Solicitante: cria uma alocação e consulta a agenda.
- Responsável: consulta a agenda.
- Administrador: consulta a agenda e pode remover alocações.

## API

O documento OpenAPI fica disponível em `/openapi/v1.json`, inclusive fora do ambiente Development.

| Método | Rota | Resultado |
|---|---|---|
| `GET` | `/api/alocacoes` | Lista as alocações |
| `GET` | `/api/alocacoes/{id}` | Consulta uma alocação |
| `POST` | `/api/alocacoes` | Cria alocação; retorna `409` em conflito |
| `DELETE` | `/api/alocacoes/{id}` | Remove uma alocação |

Exemplo de criação:

```bash
curl -i -X POST http://localhost:5206/api/alocacoes \
  -H 'Content-Type: application/json' \
  -d '{"sala":"Sala 202","professor":"Ana Souza","inicio":"2030-05-10T09:00:00","fim":"2030-05-10T10:00:00"}'
```

## Build e testes

```bash
dotnet build AlocaSenac.slnx
dotnet test AlocaSenac.slnx --settings coverlet.runsettings --collect:"XPlat Code Coverage"
```

O coletor Coverlet gera relatórios Cobertura e OpenCover em `TestResults/`. JaCoCo é específico do ecossistema Java e não é utilizado neste projeto .NET. A análise SonarCloud é opcional e requer `SONAR_TOKEN`, `SONAR_ORGANIZATION` e `SONAR_PROJECT_KEY` configurados no repositório.

Teste de carga local de baixa intensidade:

```bash
python3 tests/load/smoke.py --url http://localhost:5206/api/alocacoes
```

## Limitações conhecidas

- Perfis são apenas modos de demonstração, sem autenticação/autorização.
- Armazenamento em memória não persiste dados após reiniciar.
- O responsável ainda não aprova nem recusa solicitações; as alocações são criadas diretamente.
- Não há gestão de salas, recursos, usuários ou notificações.
- A execução de análise SonarCloud requer projeto e credenciais configurados no GitHub.

Veja `docs/entrega/` para rastreabilidade, plano e resultados de validação, registro de defeitos e estado dos itens de entrega.
