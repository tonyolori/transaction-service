namespace TransactionService.Application.DTOs
{
    public class TransactionDto
    {
        public Guid? Id { get; set; }
        public required string AccountId { get; set; }
        public required decimal Amount { get; set; }
        public required string Currency { get; set; }
        public required string Status { get; set; }

        public DateTime? CreatedAt;
        

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
