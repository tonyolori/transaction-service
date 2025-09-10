using TransactionService.Application.DTOs;
using Xunit;

namespace TransactionService.Application.Tests.DTOs
{
    public class TransactionDtoTests
    {
        [Fact]
        public void Can_Create_TransactionDto_With_Required_Properties()
        {
            // Arrange
            var dto = new TransactionDto
            {
                AccountId = "12345",
                Currency = "USD",
                Status = "Pending",
                Amount = 20000m
            };

            // Act & Assert
            Assert.Equal("12345", dto.AccountId);
            Assert.Equal("USD", dto.Currency);
            Assert.Equal("Pending", dto.Status);
            Assert.Equal(20000m, dto.Amount);
        }
    }
}
