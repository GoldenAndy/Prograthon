using Gestion_de_Labs.Data;
using Gestion_de_Labs.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_Labs.Service
{
    public class ReservaService
    {
        private readonly AppDbContext _context;

        public ReservaService(AppDbContext context)
        {
            _context = context;
        }

        public List<Reservacion> ListarTodos()
        {
            return _context.PROGRATHON_Reservacion
                .Include(r => r.Usuario)
                .Include(r => r.Laboratorio)
                .ToList();
        }

        public Reservacion? ObtenerPorId(int id)
        {
            return _context.PROGRATHON_Reservacion
                .Include(r => r.Usuario)
                .Include(r => r.Laboratorio)
                .FirstOrDefault(r => r.Reserva_Id == id);
        }

        public void Crear(Reservacion r)
        {
            _context.PROGRATHON_Reservacion.Add(r);
            _context.SaveChanges();
        }

        public void Editar(Reservacion r)
        {
            _context.PROGRATHON_Reservacion.Update(r);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var r = _context.PROGRATHON_Reservacion.Find(id);

            if (r == null)
                throw new InvalidOperationException("La reserva no existe.");

            _context.PROGRATHON_Reservacion.Remove(r);
            _context.SaveChanges();
        }
    }
}
