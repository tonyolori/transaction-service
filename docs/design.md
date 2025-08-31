# Transaction Service - Design Documentation

## Purpose
The Transaction Service acts as a query and reporting layer over the Account Service ledger, ensuring customers and admins have accurate visibility into all money movements.

## Responsibilities
- Record debit and credit transactions
- Ensure consistency with Ledger Service
- Trigger notifications after transaction completion
- Enforce transaction limits (via Transaction Limit Service)

## Design Patterns Used
- **CQRS + MediatR**: Separating queries and commands
- **Repository Pattern**: Database access abstraction
- **Factory Pattern**: Transaction object creation
- **Observer Pattern**: Publish/subscribe to transaction events

## Error Handling Strategy
- Retry failed external calls with exponential backoff
- Persist failed transactions in a dead-letter queue
- Ensure idempotency for transaction processing

## Logging & Observability
- Structured logging via Serilog
- Distributed tracing with OpenTelemetry
- Metrics exposed via Prometheus
