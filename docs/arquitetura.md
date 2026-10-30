# Arquitetura (escopo atual)

Esta demonstração usa ASP.NET Core Razor Pages e Minimal APIs. Os perfis exibidos na interface não representam autenticação nem autorização. O `AlocacoesStore` mantém os dados em memória e os perde ao reiniciar o processo.

## Diagrama de contexto

```mermaid
flowchart LR
    Solicitante[Solicitante]
    Responsavel[Responsável]
    Administrador[Administrador]
    Sistema((AlocaSenac))
    Memoria[(AlocacoesStore<br/>em memória)]

    Solicitante -->|consulta agenda e cria alocação| Sistema
    Responsavel -->|consulta agenda| Sistema
    Administrador -->|consulta e remove alocação| Sistema
    Sistema -->|lê e altera| Memoria
```

## Diagrama de componentes

```mermaid
flowchart TB
    Navegador[Navegador]
    subgraph Web[AlocaSenac.Web]
        Razor[Razor Pages<br/>Pages/Index]
        Api[Minimal API<br/>/api/alocacoes]
        Store[AlocacoesStore<br/>validação e memória]
        Razor --> Store
        Api --> Store
    end
    Dados[(Lista em memória)]

    Navegador -->|HTML e formulários| Razor
    Navegador -->|HTTP/JSON| Api
    Store --> Dados
```

## Limites arquiteturais

O cadastro de alocação é confirmado imediatamente; não há fluxo de aprovação. A API ainda não impõe autenticação. Banco persistente, gestão de espaços/recursos e notificações não fazem parte do comportamento implementado nesta entrega.
