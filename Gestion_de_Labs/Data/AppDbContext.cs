using Gestion_de_Labs.Models;

namespace Gestion_de_Labs.Data
{
    public class AppDbContext
    {

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Laboratorio> Usuario { get; set; }

        public DbSet<Reservacion> Usuario { get; set; }


    }
}
