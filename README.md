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
  "name": "João Silva",
  "email": "joao@email.com",
  "role": "User",
  "createdAt": "2024-01-01T00:00:00Z"
}
```

#### Project
```json
{
  "id": 1,
  "name": "Projeto Exemplo",
  "description": "Descrição do projeto",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": null,
  "userId": 1,
  "userName": "João Silva",
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

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

---

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.