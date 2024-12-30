using Microsoft.EntityFrameworkCore;

namespace UserAPI.DB
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=RAFAEL;Database=DB_USERS;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=30;");
        //}

    }
}
