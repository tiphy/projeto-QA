# Fluxos de sequência

## Criar alocação pela interface

```mermaid
sequenceDiagram
    actor Solicitante
    participant UI as Navegador
    participant Page as IndexModel.OnPostCreate
    participant Store as AlocacoesStore
    Solicitante->>UI: Informa sala, responsável e período
    UI->>Page: POST formulário
    Page->>Page: Valida campos e início anterior ao fim
    alt Dados inválidos
        Page-->>UI: Reexibe formulário com erros
    else Dados válidos
        Page->>Store: TentarCriar(request)
        alt Sala ocupada no intervalo
            Store-->>Page: conflito
            Page-->>UI: Reexibe formulário com erro
        else Sala disponível
            Store-->>Page: alocação criada
            Page-->>UI: Redireciona com aviso
        end
    end
```

## Criar alocação pela API

```mermaid
sequenceDiagram
    actor Cliente
    participant API as POST /api/alocacoes
    participant Store as AlocacoesStore
    Cliente->>API: Envia CriarAlocacaoRequest
    API->>API: Valida campos e início anterior ao fim
    alt Dados inválidos
        API-->>Cliente: 400 Bad Request
    else Dados válidos
        API->>Store: TentarCriar(request)
        alt Sala ocupada no intervalo
            Store-->>API: conflito
            API-->>Cliente: 409 Conflict
        else Sala disponível
            Store-->>API: alocação criada
            API-->>Cliente: 201 Created + alocação
        end
    end
```

## Consultar agenda

```mermaid
sequenceDiagram
    actor Usuario
    participant UI as Navegador
    participant Page as IndexModel
    participant Store as AlocacoesStore
    Usuario->>UI: Abre a página ou troca o perfil
    UI->>Page: GET /?persona=...
    Page->>Page: Normaliza perfil solicitado
    Page->>Store: Listar()
    Store-->>Page: Alocações ordenadas por início
    Page-->>UI: Página renderizada com o perfil e agenda
```

## Remover alocação

```mermaid
sequenceDiagram
    actor Administrador
    participant UI as Razor Pages / cliente HTTP
    participant Handler as OnPostDelete ou DELETE /api/alocacoes/{id}
    participant Store as AlocacoesStore
    Administrador->>UI: Solicita remoção
    UI->>Handler: Envia identificador
    Handler->>Store: Remover(id)
    alt Identificador existente
        Store-->>Handler: true
        Handler-->>UI: 204 No Content ou redirecionamento com aviso
    else Identificador ausente
        Store-->>Handler: false
        Handler-->>UI: 404 Not Found ou aviso
    end
```

> Os perfis da interface são demonstrativos e não autenticam o ator. A API não restringe a operação por perfil.