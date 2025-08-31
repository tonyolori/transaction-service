# Transaction Service – Component Diagram

```mermaid
flowchart TB
    subgraph TransactionService [Transaction Service]
        API[REST Controllers\n(TransactionController)]
        gRPC[gRPC Endpoints\n(TransactionGrpcService)]

        CMD[Command Handlers\n(CreateTransactionHandler, ValidateLimitHandler, ...)]
        REPO[Repositories\n(TransactionRepository, AccountRepository)]
        SVC[Domain Services\n(TransactionValidator, FeeCalculator)]
        EVT[Event Publisher\n(Kafka/Redis PubSub)]

        DB[(PostgreSQL Database)]
        CACHE[(Redis Cache)]
    end

    %% Flow
    API --> CMD
    gRPC --> CMD
    CMD --> SVC
    CMD --> REPO
    CMD --> EVT
    REPO --> DB
    SVC --> CACHE

    %% External dependencies
    REPO --> AS[Account Service (gRPC/REST)]
    SVC --> ALS[Transaction Limit Service (REST)]
    CMD --> LS[Ledger Service (gRPC)]
    EVT --> NS[Notification Service (Event Bus)]
    
    classDef db fill=#f9f,stroke=#333,stroke-width=1px;
    classDef cache fill=#ff9,stroke=#333,stroke-width=1px;
    classDef ext fill=#9cf,stroke=#333,stroke-width=1px;
    class DB db
    class CACHE cache
    class AS,ALS,LS,NS ext
```