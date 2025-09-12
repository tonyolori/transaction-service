using System;
using System.Runtime.Serialization;

namespace TransactionService.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public string Narration { get; set; } = "Payment Transaction";
        public TransactionType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public TransactionCurrency Currency { get; set; } = TransactionCurrency.NGN;
        public TransactionChannel Channel { get; set; } = TransactionChannel.TRANSFER;
        public TransactionStatus Status { get; set; } = TransactionStatus.PENDING;

        private string? _reference;
        public string Reference
        {
            get => _reference ?? $"TR-{DateTime.UtcNow:yyyyMMddHHmmssfff}";
            set => _reference = value;
        }
    }

}
