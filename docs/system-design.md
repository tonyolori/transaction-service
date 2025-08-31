# 🏗️ Fintech Platform – High-Level Architecture

```mermaid
flowchart TB
    %% External Clients
    Client[Client Apps <br/> (Mobile/Web)] -->|REST| APIGW[API Gateway]

    %% API Gateway Responsibilities
    APIGW -->|REST Proxy <br/> Auth, Rate Limit, Validation| TS[Transaction Service]
    APIGW -->|REST Proxy| US[User Service]
    APIGW -->|REST Proxy| KYC[KYC Service]
    APIGW -->|REST Proxy| ALS[Transaction Limit Service]
    APIGW -->|REST Proxy| ABS[Airtime & Bill Payment Service]
    APIGW -->|REST Proxy| AS[Account Service]
    APIGW -->|REST Proxy| TRS[Transfer Service]
    APIGW -->|REST Proxy| NS[Notification Service]

    %% Transaction Service Responsibilities
    subgraph TransactionService [Transaction Service]
        TSREST[/REST Endpoints <br/> (/transactions, /history, /statements)/]
        TSgRPC{{gRPC APIs}}
        TSQPub((Event Publisher))
        TSQSub((Event Consumer))
        TSPG[(PostgreSQL DB)]
        TSCache[(Redis Cache)]
    end

    %% API Gateway -> Transaction Service
    APIGW --> TSREST
    TSREST -->|Writes| TSPG
    TSREST -->|Caches queries, tokens| TSCache

    %% Transaction Service gRPC
    LS[Ledger Service] -->|gRPC Query <br/> (transaction details)| TSgRPC
    TSgRPC --> LS
    TSQSub -->|Consume LedgerEntryCommitted| LS

    %% Transaction Service Events
    TSQPub -->|TransactionCreated| LS
    TSQPub -->|StatementGenerated| NS
    TSQPub -->|ReceiptGenerated| NS

    %% Notification Service
    NS -->|Send Email/SMS <br/> Receipts, Statements| Client

    %% Account & Transfer flows
    TSREST -->|Account Sync| AS
    TRS -->|Transfer Ops| TSREST

    %% Airtime/Bill Payment
    ABS -->|3rd Party Bill Aggregator| EXTTP[External Payment/Bill APIs]

    %% External Payment Integrations
    TRS --> EXTTP
    AS --> EXTTP

    %% Styling
    classDef db fill=#f9f,stroke=#333,stroke-width=1px;
    classDef cache fill=#ff9,stroke=#333,stroke-width=1px;
    classDef queue fill=#9ff,stroke=#333,stroke-width=1px;
    class TSPG db
    class TSCache cache
    class TSQPub,TSQSub queue
```