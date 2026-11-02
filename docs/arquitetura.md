# Arquitetura e atributos de qualidade

## Escopo e stack identificada

O sistema executável é uma aplicação web ASP.NET Core no .NET 10. A interface é composta por Razor Pages e a API usa Minimal APIs com contrato OpenAPI em `/openapi/v1.json`. A autenticação de páginas usa cookies. O `AlocacoesStore` é um singleton com dados mantidos apenas na memória do processo.

O Docker Compose constrói e executa a aplicação e provisiona PostgreSQL 16 com volume persistente, mas a aplicação não se conecta a esse banco: não há configuração, driver ou repositório de persistência no fluxo atual. Os projetos `Application`, `Domain` e `Infrastructure` existem como estrutura, mas não contêm os componentes usados pelo fluxo de alocações atual. Não foi identificada integração com serviço externo de notificação.

## C4 — Nível 1: contexto

Os perfis abaixo são os atores apresentados na interface de demonstração, não papéis de autorização efetivos. O login atualmente cria apenas uma identidade de Administrador. A integração de notificação está representada como possibilidade futura, não como dependência executável.

```mermaid
flowchart LR
    Solicitante["Pessoa: Solicitante<br/>Perfil de demonstração"]
    Responsavel["Pessoa: Responsável<br/>Perfil de demonstração"]
    Administrador["Pessoa: Administrador<br/>Login de demonstração"]
    Sistema(("Sistema de software:<br/>AlocaSenac"))
    Notificacao["Sistema externo:<br/>API de notificação<br/>(integração não implementada)"]

    Solicitante -->|"Consulta agenda e solicita alocação"| Sistema
    Responsavel -->|"Consulta agenda<br/>(aprovação ainda não implementada)"| Sistema
    Administrador -->|"Consulta e remove alocações"| Sistema
    Sistema -.->|"Notificações: integração futura"| Notificacao
```

## C4 — Nível 2: containers e infraestrutura de execução

O navegador comunica-se com o único container de aplicação por HTTP. Dentro dele há páginas Razor, endpoints Minimal API, autenticação por cookie e o armazenamento singleton em memória. PostgreSQL está presente no Compose, mas não existe conexão entre ele e a aplicação. O desenho inclui esse container para explicitar a infraestrutura provisionada e o limite atual de persistência.

```mermaid
flowchart LR
    Pessoa["Pessoas:<br/>Solicitante, Responsável,<br/>Administrador"]
    Navegador["Container:<br/>Navegador web"]

    subgraph AlocaSenac["Sistema AlocaSenac"]
        subgraph App["Container: AlocaSenac.Web<br/>ASP.NET Core / .NET 10"]
            Razor["Componente:<br/>Razor Pages<br/>HTML renderizado no servidor"]
            API["Componente:<br/>Minimal APIs<br/>HTTP/JSON"]
            Auth["Serviço auxiliar:<br/>Autenticação por cookie<br/>e autorização de páginas"]
            OpenAPI["Serviço auxiliar:<br/>OpenAPI em /openapi/v1.json"]
            Store["Componente:<br/>AlocacoesStore singleton<br/>lista e lock em memória"]
            Razor --> Store
            API --> Store
            Razor --> Auth
            API --> OpenAPI
        end
        DB[("Container provisionado:<br/>PostgreSQL 16<br/>Compose; não conectado")]
    end

    Pessoa --> Navegador
    Navegador -->|"HTTPS/HTTP: páginas e formulários"| Razor
    Navegador -->|"HTTP/JSON"| API
    DB -.->|"Sem ligação implementada"| Store
```

## C4 — Nível 3: componentes do container web

```mermaid
flowchart TB
    subgraph Web["AlocaSenac.Web — ASP.NET Core"]
        Middleware["Pipeline ASP.NET Core:<br/>autenticação cookie e autorização"]
        Pages["Razor Pages:<br/>Login, Salas, Agendamentos,<br/>Dashboard e Relatórios"]
        Endpoints["Minimal APIs:<br/>GET, POST e DELETE /api/alocacoes"]
        Contracts["Records de API:<br/>CriarAlocacaoRequest<br/>AlocacaoResponse"]
        OpenAPI["Microsoft.AspNetCore.OpenApi:<br/>/openapi/v1.json"]
        Store["AlocacoesStore:<br/>listar, consultar, criar e remover;<br/>sincronização com lock"]

        Middleware --> Pages
        Pages --> Store
        Endpoints --> Contracts
        Endpoints --> Store
        Endpoints --> OpenAPI
    end
    Browser["Navegador"] --> Middleware
    Client["Cliente HTTP"] --> Endpoints
    Store --> Memory[("List<AlocacaoResponse><br/>em memória")]
```

O diagrama representa os caminhos encontrados no código, não garante que todas as telas apresentadas tenham handlers funcionais. Em particular, os formulários de catálogo e os botões de aprovação não realizam as operações que os seus rótulos sugerem.

## Utility Tree — ATAM

As prioridades refletem o impacto para os fluxos de reserva e o estado atual do produto. Os alvos quantitativos são critérios propostos para avaliação, não resultados medidos. A estratégia de medição de desempenho está descrita em `prd/requisitos-nao-funcionais.md`.

```mermaid
flowchart LR
    Raiz["AlocaSenac<br/>Utility Tree"]

    Raiz --> Confiabilidade["Confiabilidade<br/>Prioridade: Alta"]
    Confiabilidade --> C1["Estímulo: pedidos simultâneos<br/>para o mesmo recurso e intervalo"]
    C1 --> C2["Resposta: serializar verificação<br/>e criação no armazenamento"]
    C2 --> C3["Medida: exatamente uma criação<br/>por conflito dentro do processo"]
    C3 --> C4["Limite atual: sem garantia<br/>entre processos ou instâncias"]

    Raiz --> Seguranca["Segurança<br/>Prioridade: Alta"]
    Seguranca --> S1["Estímulo: utilizador anónimo<br/>ou perfil sem permissão"]
    S1 --> S2["Resposta esperada: negar acesso<br/>sem alterar dados"]
    S2 --> S3["Medida: 100% dos testes sem sessão<br/>redirecionados ou HTTP 401/403;<br/>zero alterações não autorizadas"]
    S3 --> S4["Estado atual: páginas protegidas<br/>por cookie; API pública e sem<br/>políticas de perfil"]

    Raiz --> Desempenho["Desempenho<br/>Prioridade: Média"]
    Desempenho --> D1["Estímulo: 50 utilizadores<br/>simultâneos por 5 minutos"]
    D1 --> D2["Resposta: servir consultas<br/>e criações sem saturação"]
    D2 --> D3["Medida proposta: p95 menor que<br/>500 ms com 10.000 reservas"]
    D3 --> D4["Estado: teste exploratório existente<br/>não valida esta meta"]

    Raiz --> Disponibilidade["Disponibilidade<br/>Prioridade: Média"]
    Disponibilidade --> A1["Estímulo: reinício ou falha<br/>do processo da aplicação"]
    A1 --> A2["Resposta esperada: recuperar<br/>serviço e manter dados"]
    A2 --> A3["Medida proposta: pelo menos<br/>99,5% por mês"]
    A3 --> A4["Estado atual: sem SLO ou health check;<br/>alocações em memória são perdidas"]
```

## Limites e pontos de evolução

- A sincronização de `AlocacoesStore` só coordena threads da mesma instância do processo.
- A base PostgreSQL provisionada não é utilizada pela aplicação; logo, volume e reinício de container não persistem as alocações atuais.
- Não há serviço externo de notificação, workflow funcional de aprovação ou gestão operacional de recursos e materiais.
- A API não exige autenticação; a autenticação de cookie é aplicada às pastas Razor configuradas em `Program.cs`.
- Para executar múltiplas instâncias com integridade concorrente, é necessária persistência compartilhada e uma estratégia transacional/constraint adequada ao modelo de intervalos.
