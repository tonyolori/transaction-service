# Transaction Service – Container Diagram

```mermaid
flowchart TB
    Client["Client Apps (Mobile/Web)"] -->|REST /transactions| APIGW["API Gateway"]

    APIGW --> TSREST["Transaction Service REST API"]
    APIGW --> US["User Service (Auth, Onboarding)"]
    APIGW --> ALS["Transaction Limit Service"]

    subgraph TransactionService [Transaction Service]
        TSREST["REST Endpoints"]
        TSgRPC["gRPC APIs"]
        TSPG[("PostgreSQL Database")]
        TSCache[("Redis Cache")]
        TSQPub[["Event Publisher"]]
        TSQSub[["Event Consumer"]]
    end

    %% Internal Links
    TSREST --> TSPG
    TSREST --> TSCache
    TSREST --> TSQPub

    %% External Services
    TSgRPC --> LS["Ledger Service"]
    TSQPub --> NS["Notification Service"]

    %% Transfer & Accounts
    TSREST --> TRS["Transfer Service"]
    TSREST --> AS["Account Service"]

    classDef db fill=#ff99ff,stroke=#333333,stroke-width=1px;
    classDef cache fill=#ffff99,stroke=#333333,stroke-width=1px;
    classDef queue fill=#99ffff,stroke=#333333,stroke-width=1px;
    class TSPG db
    class TSCache cache
    class TSQPub,TSQSub queue
```