# 💈 Navalha API

API REST para gerenciamento completo de uma barbearia, desenvolvida em **ASP.NET Core (.NET 10)** com **Entity Framework Core** e **SQL Server**.

Projeto desenvolvido como atividade da disciplina de **Programação Web 2**, na **Etec Comendador João Rays**.

## 👥 Autores

- **Gabriel Gomes Ferreira Arildo**
- **Aroldo Mucare Moraes**

**Instituição:** Etec Comendador João Rays
**Disciplina:** Programação Web 2

---

## 📖 Sobre o projeto

A **Navalha** é uma barbearia fictícia, e esta API foi construída para gerenciar todo o fluxo de atendimento do estabelecimento: cadastro de clientes, barbeiros e serviços, além do controle completo de agendamentos (criação, reagendamento, confirmação, conclusão e cancelamento), com cálculo automático de valores e verificação de disponibilidade de horários.

A API também conta com um sistema de **autenticação via JWT**, com senhas protegidas por hash (**BCrypt**).

---

## 🚀 Tecnologias utilizadas

- [.NET 10](https://dotnet.microsoft.com/) (ASP.NET Core Web API)
- [Entity Framework Core 10](https://learn.microsoft.com/ef/core/) (SQL Server)
- [JWT (JSON Web Token)](https://jwt.io/) para autenticação
- [BCrypt.Net-Next](https://github.com/BcryptNet/bcrypt.net) para hash de senha
- [Scalar.AspNetCore](https://github.com/scalar/scalar) — documentação interativa da API (OpenAPI nativo do .NET 10)

---

## 🗂️ Estrutura do projeto

```
TodoApi/
├── Controllers/         # Endpoints da API (Auth, Usuarios, Clientes, Barbeiros, Servicos, Agendamentos)
├── Data/                 # AppDbContext (EF Core) e DbSeeder (dados iniciais)
├── Migrations/           # Migrations do Entity Framework Core
├── Models/
│   ├── Entities/         # Entidades do banco de dados
│   └── DTOs/             # Data Transfer Objects (Create, Update, Response) + Mappers
├── Services/             # Regras de negócio da aplicação
├── Program.cs            # Configuração e inicialização da aplicação
├── appsettings.json       # Configurações (string de conexão, JWT, etc.)
└── TodoApi.http           # Exemplos de requisições HTTP
```

---

## 🧩 Entidades do sistema

| Entidade | Descrição |
|---|---|
| **Usuario** | Conta de acesso ao sistema (login), com papel (`Role`): `Admin`, `Barbeiro` ou `Cliente` |
| **Cliente** | Cliente da barbearia: nome, telefone, e-mail e histórico de atendimentos |
| **Barbeiro** | Profissional da barbearia: nome, telefone, especialidade e status (ativo/inativo) |
| **Servico** | Serviço oferecido: nome, descrição, valor e duração (em minutos) |
| **Agendamento** | Relaciona Cliente + Barbeiro + Serviço em uma data/horário, com status e valor |

### Status possíveis de um agendamento
`Agendado` → `Confirmado` → `Concluido`, ou `Cancelado` em qualquer ponto antes do horário marcado.

### Serviços oferecidos pela Navalha (cadastrados automaticamente na primeira execução)

| Serviço | Valor | Duração |
|---|---|---|
| Corte Masculino | R$ 45,00 | 30 min |
| Barba | R$ 35,00 | 20 min |
| Corte + Barba | R$ 70,00 | 50 min |
| Degradê | R$ 50,00 | 40 min |
| Sobrancelha | R$ 20,00 | 15 min |
| Pigmentação | R$ 60,00 | 45 min |
| Hidratação Capilar | R$ 40,00 | 30 min |

---

## 📏 Regras de negócio

- ❌ **Não é permitido** criar agendamentos em datas/horários **já passados**.
- ❌ **Não é permitido** agendar um barbeiro em um horário que **já está ocupado** por outro agendamento ativo.
- ✅ É possível **consultar os horários disponíveis** de um barbeiro em uma data específica.
- ✅ O **valor do agendamento é calculado automaticamente** a partir do valor cadastrado para o serviço escolhido.
- ✅ O **cancelamento** de um agendamento só é permitido **antes** da data/horário marcado.
- ✅ Cada cliente possui um **histórico de atendimentos** (todos os seus agendamentos, do mais recente ao mais antigo).
- ✅ Barbeiros ou serviços marcados como **inativos** não podem ser usados para novos agendamentos.

---

## 🔑 Autenticação

A API utiliza **JWT**. Para acessar rotas protegidas, é necessário:

1. Registrar um usuário em `POST /api/auth/register`
2. Fazer login em `POST /api/auth/login`
3. Utilizar o token retornado no cabeçalho `Authorization: Bearer {token}` das próximas requisições

As senhas nunca são armazenadas em texto puro — são protegidas com **hash BCrypt**.

---

## 📌 Endpoints principais

### Autenticação
| Método | Rota | Descrição |
|---|---|---|
| POST | `/api/auth/register` | Cadastra um novo usuário |
| POST | `/api/auth/login` | Realiza login e retorna o token JWT |
| GET | `/api/auth/{id}` | Busca um usuário pelo ID (autenticado) |

### Clientes
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/clientes` | Lista todos os clientes |
| GET | `/api/clientes/{id}` | Busca um cliente pelo ID |
| GET | `/api/clientes/{id}/historico` | Histórico de agendamentos do cliente |
| POST | `/api/clientes` | Cadastra um novo cliente |
| PUT | `/api/clientes/{id}` | Atualiza um cliente |
| DELETE | `/api/clientes/{id}` | Remove um cliente |

### Barbeiros
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/barbeiros` | Lista todos os barbeiros |
| GET | `/api/barbeiros/{id}` | Busca um barbeiro pelo ID |
| GET | `/api/barbeiros/{id}/horarios-disponiveis?data=AAAA-MM-DD` | Lista horários livres/ocupados do barbeiro na data |
| POST | `/api/barbeiros` | Cadastra um novo barbeiro |
| PUT | `/api/barbeiros/{id}` | Atualiza um barbeiro |
| DELETE | `/api/barbeiros/{id}` | Remove um barbeiro |

### Serviços
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/servicos` | Lista todos os serviços |
| GET | `/api/servicos/{id}` | Busca um serviço pelo ID |
| POST | `/api/servicos` | Cadastra um novo serviço |
| PUT | `/api/servicos/{id}` | Atualiza um serviço |
| DELETE | `/api/servicos/{id}` | Remove um serviço |

### Agendamentos
| Método | Rota | Descrição |
|---|---|---|
| GET | `/api/agendamentos` | Lista todos os agendamentos |
| GET | `/api/agendamentos/{id}` | Busca um agendamento pelo ID |
| GET | `/api/agendamentos/barbeiro/{barbeiroId}` | Lista agendamentos de um barbeiro |
| POST | `/api/agendamentos` | Cria um novo agendamento (valor calculado automaticamente) |
| PUT | `/api/agendamentos/{id}` | Reagenda (altera serviço/data/observações) |
| PATCH | `/api/agendamentos/{id}/status` | Atualiza o status (`Confirmado`, `Concluido`, `Cancelado`) |
| PATCH | `/api/agendamentos/{id}/cancelar` | Cancela o agendamento (apenas antes do horário) |

Exemplos completos de requisições estão disponíveis no arquivo [`TodoApi.http`](./TodoApi/TodoApi.http).

---

## ⚙️ Como executar o projeto localmente

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (ou LocalDB, já incluso no Visual Studio)

### Passo a passo

```bash
# 1. Clone o repositório
git clone https://github.com/seu-usuario/navalha-api.git
cd navalha-api/TodoApi

# 2. Restaure as dependências
dotnet restore

# 3. Ajuste a string de conexão em appsettings.json, se necessário
#    (por padrão usa LocalDB: Server=(localdb)\mssqllocaldb;Database=NavalhaDbAPI)

# 4. Gere e aplique as migrations
dotnet ef migrations add NavalhaSchema
dotnet ef database update

# 5. Execute a aplicação
dotnet run
```

A API estará disponível em `http://localhost:5089` (ou na porta exibida no terminal).
Em ambiente de desenvolvimento, a documentação interativa (Scalar) fica disponível em `/scalar`.

> 💡 O próprio `Program.cs` já chama `db.Database.Migrate()` e popula automaticamente o catálogo de serviços (`DbSeeder`) na primeira execução, então o cadastro inicial de serviços não precisa ser feito manualmente.

---

## 🧪 Testando a API

Use o arquivo [`TodoApi.http`](./TodoApi/TodoApi.http) diretamente no VS Code (extensão *REST Client*) ou no Visual Studio, ou importe os exemplos em ferramentas como Postman/Insomnia.

---

## 📄 Licença

Projeto acadêmico, desenvolvido para fins de estudo na disciplina de **Programação Web 2** — Etec Comendador João Rays.
