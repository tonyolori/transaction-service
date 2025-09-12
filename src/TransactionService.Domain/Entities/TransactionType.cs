using System;
using System.Runtime.Serialization;

namespace TransactionService.Domain.Entities
{
    public enum TransactionCurrency
    {
        [EnumMember(Value = "NGN")]
        NGN,

        [EnumMember(Value = "USD")]
        USD
    }

    public enum TransactionType
    {
        [EnumMember(Value = "Credit")]
        CREDIT,

        [EnumMember(Value = "Debit")]
        DEBIT
    }
   
    public enum TransactionStatus
    {
        [EnumMember(Value = "Pending")]
        PENDING,

        [EnumMember(Value = "Success")]
        SUCCESS,

        [EnumMember(Value = "Failed")]
        FAILED,

        [EnumMember(Value = "Reversed")]
        REVERSED,
    }

    public enum TransactionChannel
    {
        [EnumMember(Value = "Transfer")]
        TRANSFER,

        [EnumMember(Value = "Bill Payment")]
        BILL_PAYMENT,

        [EnumMember(Value = "POS")]
        POS,

        [EnumMember(Value = "Virtual Account")]
        VIRTUAL_ACCOUNT,
    }

}