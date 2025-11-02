using Microsoft.EntityFrameworkCore;
using Gestion_de_Labs.Models;

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
            // USUARIO
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("PROGRATHON_Usuario");
                e.HasKey(x => x.Usuario_Id);
            });

            // LABORATORIO
            modelBuilder.Entity<Laboratorio>(e =>
            {
                e.ToTable("PROGRATHON_Laboratorio");
                e.HasKey(x => x.Laboratorio_Id);

            });

            // RESERVA
            modelBuilder.Entity<Reservacion>(e =>
            {
                e.ToTable("PROGRATHON_Reserva");
                e.HasKey(x => x.Reserva_Id);

                // FKs
                e.HasOne(x => x.Usuario)
                 .WithMany()
                 .HasForeignKey(x => x.Usuario_Id)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.Laboratorio)
                 .WithMany()
                 .HasForeignKey(x => x.Laboratorio_Id)
                 .OnDelete(DeleteBehavior.Restrict);

                // Tipos exactos en MySQL
                e.Property(x => x.Fecha).HasColumnType("date");
                e.Property(x => x.Hora).HasColumnType("time");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
