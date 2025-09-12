using System;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public required Guid AccountId { get; set; }
        public required Guid DestinationAccountId { get; set; }
        public required decimal Amount { get; set; }
        public required decimal OpeningBalance { get; set; }
        public decimal? ClosingBalance { get; set; }
        public string Narration { get; set; } = "Payment Transaction";
        public TransactionType Type { get; set; }
        public required TransactionCurrency Currency { get; set; } = TransactionCurrency.NGN;
        public TransactionChannel Channel { get; set; } = TransactionChannel.TRANSFER;
        public TransactionStatus Status { get; set; } = TransactionStatus.PENDING;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        private string? _reference;

        public string Reference
        {
            get => _reference ??= $"TR-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            set => _reference = value ?? $"TR-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
        }

        public TransactionDto() 
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public TransactionDto(
            Guid id,
            Guid accountId,
            Guid destinationId,
            decimal amount,
            TransactionCurrency currency,
            TransactionStatus status,
            TransactionType type,
            TransactionChannel channel,
            decimal openingBalance,
            string? narration,
            decimal? closingBalance,
            string? reference,
            DateTime? createdAt)
        {
            Id = id;
            AccountId = accountId;
            DestinationAccountId = destinationId;
            Amount = amount;
            Currency = currency;
            Channel = channel;
            Status = status;
            Type = type;
            OpeningBalance = openingBalance;
            ClosingBalance = closingBalance;
            Reference = reference ?? $"TR-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            Narration = narration ?? "Payment Transaction";
            CreatedAt = createdAt;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
