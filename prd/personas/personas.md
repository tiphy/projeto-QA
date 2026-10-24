# Personas do AlocaSenac

> **Nota de discovery:** personas hipotéticas para orientar requisitos e testes; devem ser validadas com entrevistas e observação de usuários do campus. Nomes e dados são fictícios. “Senac” é tratado aqui como contexto de ensino técnico e superior, sem pressupor políticas específicas da instituição.

## 1. Marina Costa — Administradora acadêmica

**Perfil e papel**
- **Idade:** 38 anos
- **Cargo:** Assistente de secretaria acadêmica
- **Rotina no campus:** Organiza solicitações de salas, ajusta reservas, atende docentes e resolve mudanças de última hora. Trabalha principalmente em horário comercial e lida com vários cursos e turmas.

**Objetivos no AlocaSenac**
- Consultar disponibilidade por data, horário, capacidade e recursos.
- Criar, alterar e cancelar reservas sem duplicidade.
- Identificar conflitos de sala, professor ou recurso antes de confirmar.
- Comunicar mudanças aos envolvidos e manter um histórico das alterações.

**Dores e frustrações**
- Planilhas paralelas ficam desatualizadas e podem divergir.
- Descobre conflitos depois que a reserva foi comunicada.
- Não sabe com facilidade quais salas estão bloqueadas para manutenção.
- Recebe pedidos incompletos e precisa buscar informações por e-mail ou mensagem.

**Comportamento tecnológico**
- Usa desktop durante a jornada, com várias abas e sistemas internos abertos.
- Tem familiaridade com planilhas e sistemas administrativos; prefere fluxos diretos e campos claramente identificados.
- Consulta o celular para notificações e demandas urgentes, mas evita cadastrar reservas pelo aparelho.

**Cenário de uso chave**
No início do dia, Marina precisa reservar uma sala para uma turma e descobre que a sala solicitada está em manutenção. Ela pesquisa alternativas com capacidade e projetor, verifica a disponibilidade do professor e confirma uma nova reserva.

**Critério de sucesso**
A reserva só é confirmada quando sala e horário estão disponíveis; Marina consegue visualizar uma alternativa compatível, registrar a mudança e notificar os envolvidos sem recorrer a outra planilha.

**BDD**
```gherkin
Cenário: Encontrar alternativa para uma sala indisponível
  Dado que Marina precisa reservar uma sala para uma turma
  E a sala solicitada está bloqueada para manutenção no horário desejado
  Quando ela buscar salas disponíveis com capacidade e recursos equivalentes
  Então o sistema deve exibir apenas opções compatíveis e disponíveis
  E deve impedir a confirmação de uma reserva conflitante
  E deve permitir registrar e comunicar a alternativa escolhida
```

## 2. Rafael Lima — Professor

**Perfil e papel**
- **Idade:** 34 anos
- **Cargo:** Docente de cursos técnicos e de graduação
- **Rotina no campus:** Alterna entre aulas, preparação de atividades e reuniões. Em algumas disciplinas, precisa de laboratório de informática e equipamentos específicos.

**Objetivos no AlocaSenac**
- Solicitar uma sala adequada à atividade, indicando capacidade e recursos necessários.
- Reservar laboratório e materiais ou equipamentos vinculados à aula.
- Consultar o status da solicitação e receber confirmação ou aviso de alteração.
- Ver rapidamente suas reservas da semana.

**Dores e frustrações**
- Recebe uma sala comum quando a aula exige computadores ou outro recurso.
- Descobre tarde que um equipamento está indisponível.
- Precisa repetir informações em formulários, mensagens e planilhas.
- Não sabe se o pedido foi recebido, aprovado ou alterado.

**Comportamento tecnológico**
- Usa notebook para preparar aulas e acessar sistemas acadêmicos.
- Consulta agenda e notificações pelo celular entre aulas.
- Prefere formulários curtos, salvamento claro e confirmação imediata do status.

**Cenário de uso chave**
Rafael planeja uma aula prática e solicita um laboratório para sua turma, com computadores e projetor. Antes da aula, consulta a reserva e recebe uma notificação caso haja alteração.

**Critério de sucesso**
A solicitação registra todos os requisitos da aula; Rafael consegue acompanhar o status e recebe confirmação com sala, horário e recursos, ou uma alternativa quando algo não está disponível.

**BDD**
```gherkin
Cenário: Solicitar laboratório com recursos para uma aula
  Dado que Rafael está planejando uma aula prática
  Quando ele informar turma, data, horário, capacidade e recursos necessários
  E enviar a solicitação de laboratório
  Então o sistema deve validar a disponibilidade da sala e dos recursos
  E deve mostrar o status da solicitação
  E deve notificar Rafael quando houver confirmação, recusa ou alteração
```

## 3. Patrícia Nunes — Coordenadora de curso

**Perfil e papel**
- **Idade:** 45 anos
- **Cargo:** Coordenadora de curso
- **Rotina no campus:** Planeja a grade semestral, alinha horários com docentes e acompanha necessidades de salas e laboratórios. Atua em reuniões de planejamento e revisões da oferta acadêmica.

**Objetivos no AlocaSenac**
- Planejar horários considerando salas, docentes e recursos disponíveis.
- Visualizar a ocupação por curso, período e espaço.
- Detectar sobreposições antes de publicar a grade.
- Avaliar alternativas quando há conflito ou demanda acima da capacidade.

**Dores e frustrações**
- Consolida informações recebidas em formatos diferentes.
- Conflitos entre horários de docentes aparecem tarde no planejamento.
- Não tem uma visão simples da ocupação total dos espaços.
- Alterações em uma turma podem exigir ajustes manuais em várias reservas.

**Comportamento tecnológico**
- Usa desktop para planejamento e análise, geralmente com planilhas e calendário.
- Consulta informações no celular, mas realiza alterações estruturais no computador.
- Valoriza filtros, visão consolidada e exportação ou compartilhamento dos dados.

**Cenário de uso chave**
Durante o planejamento do semestre, Patrícia monta a grade de um curso. Ao incluir uma turma, consulta os horários do docente e a ocupação de salas; o sistema sinaliza uma sobreposição e apresenta opções compatíveis.

**Critério de sucesso**
Patrícia consegue revisar a grade antes de publicá-la e identificar conflitos de sala, docente e horário sem comparar manualmente várias planilhas.

**BDD**
```gherkin
Cenário: Revisar a grade semestral antes da publicação
  Dado que Patrícia está montando a grade de um curso
  E já existem horários planejados para salas e docentes
  Quando ela incluir ou alterar uma turma
  Então o sistema deve verificar conflitos de horário, sala e docente
  E deve destacar cada conflito na visão da grade
  E deve permitir consultar alternativas disponíveis antes da publicação
```

## 4. Eduardo Alves — Operador de suporte predial

**Perfil e papel**
- **Idade:** 29 anos
- **Cargo:** Técnico de apoio operacional
- **Rotina no campus:** Prepara espaços antes das aulas, controla a entrega e devolução de chaves e equipamentos e encaminha ocorrências de manutenção.

**Objetivos no AlocaSenac**
- Consultar a agenda diária por sala e horário.
- Ver quais materiais e equipamentos devem estar em cada espaço.
- Registrar entrega, devolução, falta ou dano de itens.
- Bloquear salas ou recursos temporariamente quando houver manutenção.

**Dores e frustrações**
- A lista de reservas nem sempre reflete mudanças recentes.
- Não sabe com antecedência quais salas exigem preparação especial.
- Equipamentos podem constar como disponíveis, mas estar em manutenção ou já terem sido retirados.
- Registros em papel dificultam saber quem retirou um item e quando.

**Comportamento tecnológico**
- Usa celular durante rondas e verificações no campus.
- Acessa desktop no posto de apoio para atualizar ocorrências e consultar relatórios.
- Precisa de telas simples, legíveis e rápidas, com poucas etapas para registrar uma ação.

**Cenário de uso chave**
Antes do início das aulas, Eduardo consulta a agenda do dia pelo celular e prepara uma sala com projetor. Ao verificar que o equipamento apresenta defeito, registra a ocorrência e sinaliza sua indisponibilidade para novas reservas.

**Critério de sucesso**
A equipe consegue consultar as necessidades de preparação por sala e registrar uma indisponibilidade; o recurso deixa de aparecer como reservável no período informado e os responsáveis são avisados.

**BDD**
```gherkin
Cenário: Registrar equipamento com defeito
  Dado que Eduardo identificou um projetor com defeito
  Quando ele registrar a ocorrência e informar o período de indisponibilidade
  Então o sistema deve atualizar o status do projetor
  E não deve oferecê-lo em novas reservas durante esse período
  E deve disponibilizar a informação para os responsáveis pelas reservas
```
