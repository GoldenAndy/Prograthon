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
            return _context.PROGRATHON_Laboratorio.ToList();
        }

        public Laboratorio ObtenerPorId(int id)
        {
            return _context.PROGRATHON_Laboratorio.FirstOrDefault(l => l.Laboratorio_Id == id);
        }

        public void Crear(Laboratorio lab)
        {
            _context.PROGRATHON_Laboratorio.Add(lab);
            _context.SaveChanges();
        }

        public void Editar(Laboratorio lab)
        {
            _context.PROGRATHON_Laboratorio.Update(lab);
            _context.SaveChanges();
        }

        public void Eliminar(int id)
        {
            var lab = _context.PROGRATHON_Laboratorio.Find(id);
            if (lab != null)
            {
                _context.PROGRATHON_Laboratorio.Remove(lab);
                _context.SaveChanges();
            }
        }
    }
}