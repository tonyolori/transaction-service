# Transaction Service API Specification

## gRPC Definition
```proto
service TransactionService {
  rpc CreateTransaction (TransactionRequest) returns (TransactionResponse);
  rpc GetTransaction (TransactionId) returns (TransactionResponse);
}
```

### REST Endpoints

POST   /api/transactions     → Create a transaction
GET    /api/transactions/{id} → Retrieve a transaction
