using System;
using TransactionService.Domain.Entities;

namespace TransactionService.Domain.Services
{
    public interface ITransactionValidator
    {
        bool Validate(Transaction transaction);
    }   
}