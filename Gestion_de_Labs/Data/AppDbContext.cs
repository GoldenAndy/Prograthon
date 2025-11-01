using Gestion_de_Labs.Models;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;

namespace Gestion_de_Labs.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> PROGRATHON_Usuario { get; set; }

        public DbSet<Laboratorio> PROGRATHON_Laboratorio { get; set; }

        public DbSet<Reservacion> PROGRATHON_Reservacion { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Usuario>()
                .HasKey(c => c.Usuario_Id);

            modelBuilder.Entity<Laboratorio>()
                .HasKey(c => c.Laboratorio_Id);

            modelBuilder.Entity<Reservacion>()
                .HasKey(c => c.Reserva_Id);

            modelBuilder.Entity<Reservacion>()
                .HasOne(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.Usuario_Id);

            modelBuilder.Entity<Reservacion>()
                .HasKey(c => c.Reserva_Id);

            modelBuilder.Entity<Reservacion>()
                .HasOne(c => c.Laboratorio)
                .WithMany()
                .HasForeignKey(c => c.Laboratorio_Id);

            base.OnModelCreating(modelBuilder);
        }
    }

}

