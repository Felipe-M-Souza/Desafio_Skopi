# Task Management Frontend

Frontend moderno para o sistema de gerenciamento de tarefas, desenvolvido com React + TypeScript + Vite.

## 🚀 Tecnologias Utilizadas

- **React 18** - Biblioteca para interfaces de usuário
- **TypeScript** - Superset do JavaScript com tipagem estática
- **Vite** - Build tool moderna e rápida
- **Tailwind CSS** - Framework CSS utilitário
- **React Router** - Roteamento para aplicações React
- **Axios** - Cliente HTTP para requisições à API
- **Lucide React** - Ícones modernos e consistentes

## 📁 Estrutura do Projeto

```
src/
├── components/          # Componentes reutilizáveis
├── pages/              # Páginas da aplicação
├── services/           # Serviços de API
├── types/              # Definições de tipos TypeScript
├── hooks/              # Custom hooks
├── utils/              # Funções utilitárias
└── assets/             # Recursos estáticos
```

## 🎨 Funcionalidades

### ✅ **Interface Moderna**
- Design responsivo e intuitivo
- Navegação lateral com sidebar
- Componentes reutilizáveis
- Tema consistente com Tailwind CSS

### 📊 **Dashboard**
- Visão geral dos projetos e tarefas
- Estatísticas em tempo real
- Acesso rápido às funcionalidades principais

### 📁 **Gerenciamento de Projetos**
- Listagem de projetos
- Criação e edição de projetos
- Exclusão com validações
- Contador de tarefas por projeto

### ✅ **Gerenciamento de Tarefas**
- Listagem de tarefas por projeto
- Criação e edição de tarefas
- Sistema de prioridades (Baixa, Média, Alta)
- Status de tarefas (Pendente, Em Progresso, Concluída, Cancelada)
- Histórico de comentários
- Data de vencimento

### 📈 **Relatórios**
- Relatório de produtividade por usuário
- Estatísticas dos últimos 30 dias
- Visualização de dados em tabelas
- Acesso restrito para gerentes

## 🛠️ Como Executar

### Pré-requisitos
- Node.js 18+
- npm ou yarn

### Desenvolvimento Local
```bash
# Instalar dependências
npm install

# Executar em modo desenvolvimento
npm run dev

# A aplicação estará disponível em http://localhost:3000
```

### Build para Produção
```bash
# Gerar build de produção
npm run build

# Preview do build
npm run preview
```

### Com Docker
```bash
# Build da imagem
docker build -t taskmanagement-frontend .

# Executar container
docker run -p 3000:80 taskmanagement-frontend
```

## 🔧 Configuração

### Variáveis de Ambiente
Crie um arquivo `.env` na raiz do projeto:

```env
VITE_API_URL=http://localhost:5000
```

### Integração com API
O frontend se conecta automaticamente com a API backend através da variável `VITE_API_URL`.

## 📱 Responsividade

A aplicação é totalmente responsiva e funciona em:
- 📱 Dispositivos móveis
- 💻 Tablets
- 🖥️ Desktops

## 🎯 Funcionalidades por Página

### Dashboard
- Estatísticas gerais
- Projetos recentes
- Tarefas recentes
- Acesso rápido às funcionalidades

### Projetos
- Listagem em grid responsivo
- Modal para criação/edição
- Validações de exclusão
- Informações detalhadas

### Tarefas
- Listagem com filtros
- Sistema de prioridades
- Histórico de comentários
- Status visual
- Data de vencimento

### Relatórios
- Tabela de produtividade
- Estatísticas por usuário
- Gráficos (placeholder)
- Acesso restrito

## 🚀 Deploy

### Docker Compose
```bash
# Executar toda a stack
docker-compose up --build

# Frontend: http://localhost:3000
# API: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

### Build Manual
```bash
# Build do frontend
cd TaskManagementFrontend
npm run build

# Build da API
cd TaskManagementAPI
dotnet publish -c Release
```

## 🎨 Design System

### Cores
- **Primary**: Azul (#3b82f6)
- **Success**: Verde (#10b981)
- **Warning**: Amarelo (#f59e0b)
- **Danger**: Vermelho (#ef4444)
- **Gray**: Escala de cinzas

### Componentes
- Botões com estados hover/focus
- Cards com sombras sutis
- Inputs com validação visual
- Modais responsivos
- Tabelas com hover states

## 📝 Notas de Desenvolvimento

### Estado da Aplicação
- Estado local com React hooks
- Gerenciamento de formulários
- Validações client-side
- Feedback visual para ações

### Performance
- Lazy loading de componentes
- Otimização de bundle
- Cache de requisições
- Imagens otimizadas

### Acessibilidade
- Navegação por teclado
- Contraste adequado
- Labels descritivos
- ARIA attributes

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT.
