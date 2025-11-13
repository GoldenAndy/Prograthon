using Gestion_de_Labs.Data;
using Gestion_de_Labs.Models;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_Labs.Service
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public List<Usuario> ListarTodos()
        {
            return _context.PROGRATHON_Usuario
                .AsNoTracking()
                .ToList();
        }

        public Usuario? ObtenerPorId(int id)
        {
            return _context.PROGRATHON_Usuario
                .AsNoTracking()
                .FirstOrDefault(u => u.Usuario_Id == id);
        }

        public void Crear(Usuario usuario)
        {
            _context.PROGRATHON_Usuario.Add(usuario);
            _context.SaveChanges();
        }

        public void Editar(Usuario usuario)
        {
            _context.PROGRATHON_Usuario.Update(usuario);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var usuario = _context.PROGRATHON_Usuario.Find(id);
            if (usuario == null)
                throw new InvalidOperationException("El usuario no existe.");

            bool tieneReservas = _context.PROGRATHON_Reservacion
                .Any(r => r.Usuario_Id == id);

            if (tieneReservas)
                throw new InvalidOperationException("No se puede eliminar el usuario porque tiene reservaciones asociadas.");

            _context.PROGRATHON_Usuario.Remove(usuario);
            _context.SaveChanges();
        }
    }
}
