using Gestion_de_Labs.Data;
using Gestion_de_Labs.Models;

namespace Gestion_de_Labs.Service
{
    public class LaboratorioService
    {
        private readonly AppDbContext _context;

        public LaboratorioService(AppDbContext context)
        {
            _context = context;
        }

        public List<Laboratorio> ListarTodos()
        {
            return _context.Laboratorios.ToList();
        }

        public Laboratorio ObtenerPorId(int id)
        {
            return _context.Laboratorios.FirstOrDefault(l => l.Laboratorio_Id == id);
        }

        public void Crear(Laboratorio lab)
        {
            _context.Laboratorios.Add(lab);
            _context.SaveChanges();
        }

        public void Editar(Laboratorio lab)
        {
            _context.Laboratorios.Update(lab);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var lab = _context.Laboratorios.Find(id);
            if (lab != null)
            {
                _context.Laboratorios.Remove(lab);
                _context.SaveChanges();
            }
        }
    }
}