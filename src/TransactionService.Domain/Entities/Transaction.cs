public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "NGN";
    public string Status { get; set; } = "pending";
    public DateTimeOffset CreatedAt { get; set; }
}
