using BankAccountAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccountAPI.Data
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
