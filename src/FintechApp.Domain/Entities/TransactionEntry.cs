using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FintechApp.Domain.Entities
{
    public class TransactionEntry
    {
        [Key]
        public int EntryId { get; set; }
        public int TransactionId { get; set; }
        public Transaction Transaction { get; set; } = null!;

        public int WalletId { get; set; }
        public UserWallet Wallet { get; set; } = null!;

        public decimal Amount { get; set; }         
        public EntryType EntryType { get; set; }     
    }
    public enum EntryType
    {
        Debit,   
        Credit    
    }

}
