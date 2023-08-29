using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<Client>? Clients{ get; set; }
        public DbSet<Account>? Accounts { get; set; }
        public DbSet<Card>? Cards { get; set; }
        public DbSet<Transaction>? Transactions { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<PaymentMethod>? PaymentMethods { get; set; }
    }
}
