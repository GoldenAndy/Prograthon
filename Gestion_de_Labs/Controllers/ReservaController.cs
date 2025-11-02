using Gestion_de_Labs.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_de_Labs.Models;

namespace Gestion_de_Labs.Controllers
{
    public class ReservaController : Controller
    {
        private readonly AppDbContext _context;

        public ReservaController(AppDbContext context)
        {
            _context = context;
        }

        // ================== LISTAR ==================
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.PROGRATHON_Reservacion
                .Include(r => r.Usuario) 
                .Include(r => r.Laboratorio) 
                .ToListAsync();

            return View(reservas);
        }




        // ================== CREAR ==================
        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Usuarios = _context.PROGRATHON_Usuario.ToList();
            ViewBag.Laboratorios = _context.PROGRATHON_Laboratorio.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Reservacion reserva)
        {
            if (ModelState.IsValid)
            {
                _context.PROGRATHON_Reservacion.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Usuarios = _context.PROGRATHON_Usuario.ToList();
            ViewBag.Laboratorios = _context.PROGRATHON_Laboratorio.ToList();
            return View(reserva);
        }




        // ================== EDITAR ==================
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var reserva = await _context.PROGRATHON_Reservacion.FindAsync(id);
            if (reserva == null)
                return NotFound();

            ViewBag.Usuarios = _context.PROGRATHON_Usuario.ToList();
            ViewBag.Laboratorios = _context.PROGRATHON_Laboratorio.ToList();
            return View(reserva);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Reservacion reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Update(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Usuarios = _context.PROGRATHON_Usuario.ToList();
            ViewBag.Laboratorios = _context.PROGRATHON_Laboratorio.ToList();
            return View(reserva);
        }




        // ================== ELIMINAR ==================
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var reserva = await _context.PROGRATHON_Reservacion
                .Include(r => r.Usuario)      // <-- navegación
                .Include(r => r.Laboratorio)  // <-- navegación
                .FirstOrDefaultAsync(r => r.Reserva_Id == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.PROGRATHON_Reservacion.FindAsync(id);
            if (reserva != null)
            {
                _context.PROGRATHON_Reservacion.Remove(reserva);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
