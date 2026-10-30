# Passo a passo: usando a API do AlocaSenac

Este tutorial mostra como consultar, criar e remover alocações de salas pela API. Os exemplos usam `curl` e consideram a aplicação local em `http://localhost:5206`.

## 1. Inicie a aplicação

Na raiz do repositório, execute:

```bash
dotnet run --project src/AlocaSenac.Web/AlocaSenac.Web.csproj
```

Mantenha esse terminal aberto enquanto usa a API. Se estiver executando com Docker Compose, substitua `http://localhost:5206` por `http://localhost:8080`.

## 2. Consulte a documentação OpenAPI

O contrato da API pode ser consultado no navegador ou via `curl`:

```bash
curl http://localhost:5206/openapi/v1.json
```

## 3. Liste as alocações

Faça uma requisição `GET` para obter todas as alocações:

```bash
curl -i http://localhost:5206/api/alocacoes
```

Uma resposta bem-sucedida tem status `200 OK` e retorna uma lista de objetos com `id`, `sala`, `professor`, `inicio` e `fim`.

## 4. Crie uma alocação

Envie uma requisição `POST` com os dados em JSON:

```bash
curl -i -X POST http://localhost:5206/api/alocacoes \
  -H 'Content-Type: application/json' \
  -d '{
    "sala": "Sala 202",
    "professor": "Ana Souza",
    "inicio": "2027-05-10T09:00:00",
    "fim": "2027-05-10T10:00:00"
  }'
```

Se a alocação for criada, a API responde `201 Created` e retorna o objeto criado. Guarde o valor de `id` da resposta para consultar ou remover essa alocação.

## 5. Consulte uma alocação pelo ID

Substitua `<id>` pelo identificador retornado na etapa anterior:

```bash
curl -i http://localhost:5206/api/alocacoes/<id>
```

A resposta será `200 OK` se o ID existir ou `404 Not Found` se não houver uma alocação com esse ID.

## 6. Remova uma alocação

Use o ID da alocação criada:

```bash
curl -i -X DELETE http://localhost:5206/api/alocacoes/<id>
```

O status `204 No Content` confirma a remoção. Se o ID não existir, a API responde `404 Not Found`.

## Validações e conflitos

- `Sala` e `Professor` são obrigatórios. Se algum deles estiver vazio, a criação retorna `400 Bad Request`.
- `Inicio` deve ser anterior a `Fim`; caso contrário, a API retorna `400 Bad Request`.
- Não é permitido reservar a mesma sala em horários sobrepostos. Uma tentativa conflitante retorna `409 Conflict`.
- A mesma sala pode ser reservada em horários que não se sobreponham.
- As alocações são mantidas em memória e são perdidas quando a aplicação é reiniciada.
