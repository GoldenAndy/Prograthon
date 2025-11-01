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
            var reservas = await _context.Reservaciones
                .Include(r => r.Usuario_Id)
                .Include(r => r.Laboratorio_Id)
                .ToListAsync();

            return View(reservas);
        }




        // ================== CREAR ==================
        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Usuarios = _context.Usuario.ToList();
            ViewBag.Laboratorios = _context.Laboratorios.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Crear(Reservacion reserva)
        {
            if (ModelState.IsValid)
            {
                _context.Reservaciones.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Usuarios = _context.Usuario.ToList();
            ViewBag.Laboratorios = _context.Laboratorios.ToList();
            return View(reserva);
        }




        // ================== EDITAR ==================
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var reserva = await _context.Reservaciones.FindAsync(id);
            if (reserva == null)
                return NotFound();

            ViewBag.Usuarios = _context.Usuario.ToList();
            ViewBag.Laboratorios = _context.Laboratorios.ToList();
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

            ViewBag.Usuarios = _context.Usuario.ToList();
            ViewBag.Laboratorios = _context.Laboratorios.ToList();
            return View(reserva);
        }




        // ================== ELIMINAR ==================
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var reserva = await _context.Reservaciones
                .Include(r => r.Usuario)
                .Include(r => r.Laboratorio)
                .FirstOrDefaultAsync(r => r.Reserva_Id == id);

            if (reserva == null)
                return NotFound();

            return View(reserva);
        }

        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservaciones.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservaciones.Remove(reserva);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
