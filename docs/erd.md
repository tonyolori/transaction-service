# Entity Relationship Diagram (ERD) — Transaction Service

The Transaction Service records, retrieves, and presents financial transactions.  
It generates **receipts on-demand** (no file persistence) and **statements** on-demand or persisted for compliance.  
All transaction records are **immutable** and reconciled with the Account Service ledger.  
Shareable access is provided via **signed, time-limited links**.

---

## Diagram

```mermaid
erDiagram
    %% External references
    User ||--o{ Account : "owns (via Account Service)"

    %% Core domain
    Account ||--o{ Transaction : "has"
    Transaction ||--o{ Reversal : "optional reversal"
    Transaction ||--o{ SignedLink : "receipt links"

    %% Statements
    Account ||--o{ StatementJob : "requests"
    StatementJob ||--o| StatementArtifact : "produces"
    StatementArtifact ||--o{ SignedLink : "share links"

    %% Ledger reconciliation
    Transaction }o--|| LedgerPointer : "reconciles to"

    User {
        string UserId PK
        string FullName
        string Email
        string PhoneNumber
        datetime CreatedAt
    }

    Account {
        string AccountId PK
        string UserId FK
        string AccountNumber
        string AccountType
        string Status
        datetime CreatedAt
    }

    Transaction {
        string TransactionId PK
        string AccountId FK
        string Type
        string Channel
        decimal Amount
        string Currency
        string Narration
        string Reference
        string Status
        decimal RunningBalance
        json Metadata
        string IntegrityHash
        datetime CreatedAt
        datetime CompletedAt
    }

    Reversal {
        string ReversalId PK
        string TransactionId FK
        string Reason
        string InitiatedBy
        datetime CreatedAt
    }

    LedgerPointer {
        string PointerId PK
        string TransactionId FK
        string LedgerEntryId
        string LedgerVersion
        datetime PostedAt
    }

    StatementJob {
        string JobId PK
        string AccountId FK
        string RequestedBy
        date FromDate
        date ToDate
        string Format
        boolean IsCompliance
        string Status
        string FailureReason
        datetime RequestedAt
        datetime CompletedAt
        datetime RetentionUntil
    }

    StatementArtifact {
        string ArtifactId PK
        string JobId FK
        string StorageProvider
        string StorageKey
        string PresignedUrl
        string MimeType
        bigint SizeBytes
        string Checksum
        boolean IsPermanent
        datetime ExpiresAt
        datetime CreatedAt
    }

    SignedLink {
        string LinkId PK
        string SubjectType
        string SubjectId
        string TokenHash
        integer MaxUses
        integer Uses
        datetime ExpiresAt
        boolean Revoked
        string CreatedBy
        datetime CreatedAt
        datetime RevokedAt
    }
```

# Entity Notes & Rationale

## Transaction
- **Immutable after creation**: status transitions are append-only via `TransactionAudit`.
- `IntegrityHash` ensures tamper detection (hash over `id`, `account`, `amount`, `currency`, `reference`, `createdAt`, etc.).
- `Reference` is unique for reconciliation with rails/partners.

## Reversal
- Captures reversal intent & reason **without mutating** the original `Transaction`.
- Status change is reflected through an audit entry; original record remains intact.

## LedgerPointer
- Cross-service reference to the **Ledger Service entry/version** to guarantee ledger alignment.

## StatementJob
- Tracks asynchronous generation of statements for a date range in a given format.
- `IsCompliance = true` ⇒ artifact must be persisted and retained for regulatory period.

## StatementArtifact
- **Ad-hoc**: may be cached briefly (`ExpiresAt`) or regenerated; `IsPermanent = false`.
- **Compliance**: stored durably; `IsPermanent = true`; governed by `RetentionUntil` policy.
- `Checksum` allows integrity verification on download/stream.

## SignedLink
- Encodes shareable access to receipts (subject = `TransactionId`) or statements (subject = `ArtifactId`).
- Store only a **hash of the token** (JWT/HMAC) for security; actual token is returned to caller.
- **Time-limited** (`ExpiresAt`) and optionally usage-limited (`MaxUses`).

## TransactionAudit
- Provides a complete, **append-only** history of lifecycle events for audit/compliance.
- Useful for tracing, forensics, and regulatory evidence.

---

# Keys, Constraints & Indexing (Suggested)

### Primary Keys
- UUIDs for all entities.

### Uniqueness
- `Transaction.Reference` (unique, nullable when not provided by rail).
- `StatementArtifact.JobId` (1:1 with job).

### Foreign Keys
- `Transaction.AccountId → Account.AccountId` (reference from Account Service).
- `Reversal.TransactionId → Transaction.TransactionId`.
- `TransactionAudit.TransactionId → Transaction.TransactionId`.
- `LedgerPointer.TransactionId → Transaction.TransactionId`.
- `StatementJob.AccountId → Account.AccountId`.
- `StatementArtifact.JobId → StatementJob.JobId`.
- `SignedLink.SubjectId` references `Transaction.TransactionId` or `StatementArtifact.ArtifactId` based on `SubjectType`.

### Indexes
- **Transaction**: `(AccountId, CreatedAt DESC)`, `(Status)`, `(Type)`, `(Channel)`, `(Reference)`.
- **StatementJob**: `(AccountId, RequestedAt DESC)`, `(Status)`.
- **SignedLink**: `(SubjectType, SubjectId)`, `(ExpiresAt)`.
- TTL/expiration index on `SignedLink.ExpiresAt` and ad-hoc `StatementArtifact.ExpiresAt` (if DB supports TTL).

---

# Immutability & Compliance Rules

### Transactions
- Never update monetary fields; changes recorded via `TransactionAudit`.
- Reversals are additive (`Reversal`) and reconciled in ledger via `LedgerPointer`.

### Receipts
- Not persisted as files.
- Regenerated on access using current transaction data.
- Shareable via `SignedLink` (subject = `TRANSACTION_RECEIPT`).

### Statements
- **Ad-hoc**: generated on-demand; artifacts may be cached temporarily or regenerated; share via `SignedLink`.
- **Compliance**: artifact persisted (e.g., S3) with retention policy (`IsPermanent = true`, `RetentionUntil`).

---

# Data Retention & Expiry
- `SignedLink` and ad-hoc `StatementArtifact` are short-lived (TTL cleanup).
- Compliance `StatementArtifact` retained for **X years per regulation**, then purged per policy.
- `Transaction` and `TransactionAudit` retained according to **financial record-keeping laws**.

---

# Mapping to Functional Requirements
- **Transaction History**: `Transaction` + indexes for filtering (date/type/channel/status) and pagination.
- **Receipt Generation**: no file entity; use `SignedLink` on `Transaction`.
- **Statement Generation**: `StatementJob` + optional `StatementArtifact` (persist for compliance).
- **Ledger Consistency**: `LedgerPointer` links each transaction to ledger entries.
- **Shareable Links**: `SignedLink` with hashed token and expiry.
