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
            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("PROGRATHON_Usuario");
                e.HasKey(x => x.Usuario_Id);
            });

            modelBuilder.Entity<Laboratorio>(e =>
            {
                e.ToTable("PROGRATHON_Laboratorio");
                e.HasKey(x => x.Laboratorio_Id);
            });

            modelBuilder.Entity<Reservacion>(e =>
            {
                e.ToTable("PROGRATHON_Reserva");
                e.HasKey(x => x.Reserva_Id);

                e.HasOne(r => r.Usuario)
                 .WithMany()
                 .HasForeignKey(r => r.Usuario_Id)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Laboratorio)
                 .WithMany()
                 .HasForeignKey(r => r.Laboratorio_Id)
                 .OnDelete(DeleteBehavior.Restrict);

                e.Property(r => r.Fecha).HasColumnType("date");
                e.Property(r => r.Hora).HasColumnType("time");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}