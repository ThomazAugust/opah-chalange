# CashFlow

Projeto inicial para gestão de fluxo de caixa com arquitetura limpa em .NET 10.

## Pré-requisitos

- .NET SDK 10
- Docker
- PostgreSQL (ou Docker Compose)

## Estrutura

- `src/Core/CashFlow.Domain`: entidades e regras de negócio
- `src/Core/CashFlow.Application`: casos de uso e DTOs
- `src/Infrastructure/CashFlow.Infrastructure`: repositórios Dapper e integração PostgreSQL
- `src/Presentation/CashFlow.API`: API de lançamentos
- `src/Presentation/Consolidation.API`: API de consulta/reprocessamento de consolidação
- `src/Presentation/Consolidation.Worker`: worker de consolidação assíncrona
- `src/Tests`: testes unitários e integração

## Executar localmente

1. Restaurar e compilar:

```Bash ou PowerShell
dotnet restore CashFlow.slnx
dotnet build CashFlow.slnx
```

2. Subir PostgreSQL com Docker:

```Bash ou PowerShell
docker compose up -d postgres
```

3. Criar schema inicial no banco:

```Bash ou PowerShell
docker compose cp "src/Infrastructure/CashFlow.Infrastructure/Migrations/001_initial.sql" postgres:/tmp/001_initial.sql

```

```PowerShell
docker compose exec -e PGPASSWORD=postgres -T postgres psql -U postgres -d cashflow -f /tmp/001_initial.sql

```

```Bash
PGPASSWORD=postgres docker compose exec -T postgres sh -lc "psql -U postgres -d cashflow -f /tmp/001_initial.sql"

```

4. Executar APIs e Worker:

```Bash ou PowerShell

dotnet run --project src/Presentation/CashFlow.API
dotnet run --project src/Presentation/Consolidation.API
dotnet run --project src/Presentation/Consolidation.Worker
```

## Endpoints principais

- `POST /api/lancamentos`: registra crédito/débito
- `GET /api/saldos/{data}`: consulta saldo consolidado por dia
- `GET /api/consolidacao/{data}`: consulta via serviço de consolidação
- `POST /api/consolidacao/reprocessar/{data}`: reprocessa consolidação diária

## Autenticação JWT

As APIs estão preparadas para autenticação JWT com parâmetros em `appsettings.json`.

## Testes

```Bash
dotnet test CashFlow.slnx
```

## Melhorias futuras

- Observabilidade com Serilog
- Cobertura de testes >= 80%
