# Transaction Service

## 📌 Overview

The Transaction Service handles the recording, retrieval, and presentation of all financial transactions across the platform. It provides customers with transparent access to their transaction history, generates digital receipts for individual transactions, and produces account statements for regulatory and personal use.

It acts as a query and reporting layer over the Account Service ledger, ensuring customers and admins have accurate visibility into all money movements.


## 🚀 Service Requirements
- **Language/Framework**: .NET 9, ASP.NET Core
- **Database**: PostgreSQL
- **Cache/Queue**: Redis, Kafka
- **Protocols**: gRPC, REST
- **Dependencies**: Account Service, Ledger Service, Notification Service

## 🛠️ High-level Documentation
- Handles customer bank accounts business logic
- Interacts with Account Service.
- Integrates with 3rd party APIs if any

## 📂 Code Structure
- `TransactionService.Api` → API & gRPC endpoints
- `TransactionService.Application` → Business logic (CQRS, MediatR handlers)
- `TransactionService.Domain` → Core entities & aggregates
- `TransactionService.Infrastructure` → Database (EF Core), repositories, external integrations
- `TransactionService.Tests` → Unit & integration tests

## 📜 Documentation
- [Design Documentation](./docs/design.md)
- [API Spec](./docs/api.md)
- [System & Sequence Diagrams](./docs/diagrams/)
- [Entity Relationship Diagram](./docs/diagrams/erd.md)

## 🔌 API Specification
- gRPC proto definitions → [`/proto/transaction.proto`](./proto/transaction.proto)
- REST endpoints → See [`/docs/api.md`](./docs/api.md)

## 📡 Deployment
- Dockerized and deployed via Kubernetes
- CI/CD with GitHub Actions (`.github/workflows/ci-cd.yml`)
- Config via environment variables (see `.env.example`)

## 🧪 Testing
- Unit tests: `npm test` / `pytest` / `dotnet test`
- Integration tests: details
- CI/CD pipeline: GitHub Actions / GitLab CI

## ▶️ Running Locally
```bash
# Install dependencies
npm install

# Start dev server
npm run dev
```