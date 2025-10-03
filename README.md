# Task Management API (.NET C#)

API RESTful para gerenciamento de projetos e tarefas, desenvolvida em **.NET 8 (C#)**.  
O objetivo é permitir que usuários organizem e monitorem suas tarefas diárias dentro de projetos, além de colaborarem com colegas de equipe.

---

## 📌 Funcionalidades Implementadas

- **Projetos**
  - Listagem de projetos do usuário
  - Criação de novos projetos
  - Restrições de remoção (não pode excluir se houver tarefas pendentes)

- **Tarefas**
  - Visualização de tarefas de um projeto
  - Criação de tarefas (máx. 20 por projeto)
  - Atualização de status ou detalhes
  - Remoção de tarefas
  - Definição de prioridade (baixa, média, alta) **imutável após criação**
  - Histórico de atualizações com data, usuário e alterações
  - Comentários em tarefas (registrados no histórico)

- **Relatórios**
  - Número médio de tarefas concluídas por usuário nos últimos 30 dias
  - Acesso restrito a usuários com perfil **gerente**

---

## ⚙️ Tecnologias Utilizadas

- **.NET 8 (C#)**
- **Entity Framework Core** (ORM)
- **SQL Server** (configurável via `appsettings.json`)
- **xUnit** para testes unitários
- **Docker** e **Docker Compose** para containerização
- **Swagger** para documentação dos endpoints

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (opcional)

### Opção 1: Executar com Docker (Recomendado)
```bash
# Navegar para o diretório do projeto
cd TaskManagementAPI

# Executar com Docker Compose
docker-compose up --build

# A API estará disponível em:
# - HTTP: http://localhost:5000
# - HTTPS: https://localhost:5001
# - Swagger: https://localhost:5001/swagger
```

### Opção 2: Executar Localmente
```bash
# Navegar para o diretório do projeto
cd TaskManagementAPI

# Restaurar dependências
dotnet restore

# Executar migrations
dotnet ef database update

# Executar a aplicação
dotnet run

# A API estará disponível em:
# - HTTP: http://localhost:5000
# - HTTPS: https://localhost:5001
# - Swagger: https://localhost:5001/swagger
```

### Executar Testes
```bash
# Executar todos os testes
dotnet test

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"
```

---

## 📚 Documentação da API

### Endpoints Principais

#### Projetos
- `GET /api/projects/user/{userId}` - Listar projetos do usuário
- `GET /api/projects/{projectId}/user/{userId}` - Obter projeto específico
- `POST /api/projects` - Criar novo projeto
- `PUT /api/projects/{projectId}/user/{userId}` - Atualizar projeto
- `DELETE /api/projects/{projectId}/user/{userId}` - Excluir projeto

#### Tarefas
- `GET /api/tasks/project/{projectId}/user/{userId}` - Listar tarefas do projeto
- `GET /api/tasks/{taskId}/user/{userId}` - Obter tarefa específica
- `POST /api/tasks` - Criar nova tarefa
- `PUT /api/tasks/{taskId}/user/{userId}` - Atualizar tarefa
- `DELETE /api/tasks/{taskId}/user/{userId}` - Excluir tarefa
- `POST /api/tasks/{taskId}/comments` - Adicionar comentário à tarefa

#### Relatórios
- `GET /api/reports/user-tasks/{managerUserId}` - Relatório de tarefas por usuário (apenas gerentes)

### Modelos de Dados

#### User
```json
{
  "id": 1,
  "name": "Felipe de Melo Souza",
  "email": "felip-nho@hotmail.com",
  "role": "User",
  "createdAt": "2025-10-02T00:00:00Z"
}
```

#### Project
```json
{
  "id": 1,
  "name": "Projeto Exemplo",
  "description": "Descrição do projeto",
  "createdAt": "2025-10-02T00:00:00Z",
  "updatedAt": null,
  "userId": 1,
  "userName": "Felipe de Melo Souza",
  "taskCount": 5
}
```

#### Task
```json
{
  "id": 1,
  "title": "Tarefa Exemplo",
  "description": "Descrição da tarefa",
  "status": "Pending",
  "priority": "Medium",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": null,
  "dueDate": "2024-01-15T00:00:00Z",
  "projectId": 1,
  "projectName": "Projeto Exemplo",
  "userId": 1,
  "userName": "João Silva",
  "taskHistories": []
}
```

---

## 🏗️ Estrutura do Projeto

```
TaskManagementAPI/
├── Controllers/          # Controllers da API
├── Data/                # DbContext e configurações do EF
├── DTOs/                # Data Transfer Objects
├── Extensions/          # Extensões de serviços
├── Models/              # Modelos de dados
├── Services/            # Lógica de negócio
├── TaskManagementAPI.Tests/  # Testes unitários
├── Dockerfile           # Configuração Docker
├── docker-compose.yml   # Orquestração de containers
└── README.md           # Este arquivo
```

---

## 🔧 Configurações

### Connection Strings
- **Desenvolvimento**: LocalDB
- **Produção**: SQL Server (Docker)

### Variáveis de Ambiente
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ConnectionStrings__DefaultConnection`: String de conexão do banco

---

## 🧪 Testes

O projeto inclui testes unitários para os principais serviços:
- `ProjectServiceTests`: Testes para operações de projetos
- Cobertura de código configurada
- Execução via `dotnet test`

---

## 📝 Notas de Desenvolvimento

### Regras de Negócio Implementadas
1. **Projetos**: Não podem ser excluídos se houver tarefas pendentes
2. **Tarefas**: Máximo de 20 tarefas por projeto
3. **Prioridade**: Imutável após criação da tarefa
4. **Relatórios**: Acesso restrito a usuários com perfil "Manager"
5. **Histórico**: Todas as alterações são registradas com timestamp e usuário

### Validações
- Validação de entrada em todos os endpoints
- Verificação de permissões para relatórios
- Controle de limites (máximo de tarefas por projeto)
- Validação de integridade referencial

---

## 📌 Fase 2 – Refinamento (Perguntas ao PO)

### Questões sobre Funcionalidades Futuras

1. **Deseja suporte a subtarefas dentro de uma tarefa?**
   - Implementação de hierarquia de tarefas
   - Controle de dependências entre subtarefas
   - Relatórios considerando subtarefas

2. **As prioridades devem ser apenas baixa/média/alta ou podemos parametrizar?**
   - Sistema de prioridades customizáveis
   - Cores e níveis configuráveis por organização
   - Impacto nos relatórios e dashboards

3. **Precisamos implementar notificações (e-mail, push, etc.) quando uma tarefa for atualizada?**
   - Sistema de notificações em tempo real
   - Integração com email corporativo
   - Notificações push para mobile/web

4. **Haverá necessidade de exportação de relatórios (PDF, Excel)?**
   - Geração automática de relatórios
   - Agendamento de relatórios periódicos
   - Templates customizáveis

5. **O controle de usuários/roles (ex: gerente) será feito por outro serviço externo ou devemos evoluir para autenticação interna?**
   - Integração com Active Directory
   - Sistema de autenticação próprio
   - Controle de permissões granular

6. **Deseja integração com calendário (Google/Outlook) para sincronizar prazos?**
   - Sincronização bidirecional
   - Lembretes automáticos
   - Bloqueio de horários

---

## 📌 Fase 3 – Melhorias Futuras

### Autenticação e Autorização
- **Implementar JWT + Identity** para controle de usuários
- Sistema de roles e permissões granular
- Integração com Active Directory/LDAP
- Multi-factor authentication (MFA)

### Padrões de Projeto
- **Uso de CQRS + MediatR** para melhor organização de casos de uso
- Separação clara entre comandos e queries
- Implementação de Domain Events
- Clean Architecture com camadas bem definidas

### Mensageria
- **Uso de RabbitMQ/Kafka** para notificação de alterações em tarefas
- Processamento assíncrono de operações pesadas
- Event-driven architecture
- Dead letter queues para tratamento de erros

### Cache
- **Redis para melhorar performance** em relatórios
- Cache distribuído para sessões
- Cache de consultas frequentes
- Invalidação inteligente de cache

### Escalabilidade/Cloud
- **Deploy em Kubernetes** ou Azure App Service
- Banco em Azure SQL/Postgres
- Auto-scaling baseado em métricas
- Load balancing e health checks

### Observabilidade
- **Logging estruturado com Serilog** + métricas Prometheus/Grafana
- APM (Application Performance Monitoring)
- Alertas proativos para problemas
- Dashboards de monitoramento em tempo real

### Funcionalidades Avançadas
- **Real-time Updates** com SignalR
- **Busca Avançada** com Elasticsearch
- **Machine Learning** para predição de prazos
- **Integração com calendários** (Google/Outlook)
- **Exportação de relatórios** (PDF, Excel)
- **Subtarefas e dependências**
- **Notificações push** e email

---

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.