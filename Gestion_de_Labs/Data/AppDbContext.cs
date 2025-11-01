using Gestion_de_Labs.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_Labs.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Laboratorio> Laboratorios { get; set; }

        public DbSet<Reservacion> Reservaciones { get; set; }


    }
}
