using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }
        public DbSet<Client>? Clients{ get; set; }
    }
}
