using GerenciadorConteudo.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorConteudo
{
    public class DatabaseContext : DbContext
    {        
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Clientes> Clientes { get; set;}
        public DbSet<Usuarios> Usuarios { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Usuarios>().HasKey(t => t.UserId);        
            modelBuilder.Entity<Usuarios>().HasIndex(t => t.UserEmail).IsUnique();        

        }
    }
}
