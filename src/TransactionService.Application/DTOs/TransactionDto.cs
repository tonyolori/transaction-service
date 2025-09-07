namespace TransactionService.Application.DTOs
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public TransactionDto() { }

        public TransactionDto(Guid id, string accountId, decimal amount, string currency, string status, DateTime createdAt)
        {
            Id = id;
            AccountId = accountId;
            Amount = amount;
            Currency = currency;
            Status = status;
            CreatedAt = createdAt;
        }
    }
}
