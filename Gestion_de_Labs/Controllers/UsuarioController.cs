using Gestion_de_Labs.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_de_Labs.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gestion_de_Labs.Controllers
{
    [Route("Usuario")]
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        //Listar
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.PROGRATHON_Usuario
            .AsNoTracking()
            .ToListAsync();
            return View(usuarios);
        }


        //Dropdown tipo de usuario
        private void CargarTiposUsuarioDropDown(int? seleccionado = null)
        {
            var opciones = new[]
            {
                new SelectListItem { Value = "1", Text = "Estudiante" },
                new SelectListItem { Value = "2", Text = "Profesor" },
                new SelectListItem { Value = "3", Text = "Otro" }
            };

            if (seleccionado.HasValue)
            {
                foreach (var op in opciones)
                    op.Selected = op.Value == seleccionado.Value.ToString();
            }

            ViewBag.TiposUsuario = opciones;
        }


        //Crear
        [HttpGet]
        public IActionResult Crear()
        {
            CargarTiposUsuarioDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.PROGRATHON_Usuario.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarTiposUsuarioDropDown(usuario?.Tipo_Usuario_Id);
            return View(usuario);
        }


        //Editar
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var usuario = await _context.PROGRATHON_Usuario.FindAsync(id);
            if (usuario == null)
                return NotFound();

            CargarTiposUsuarioDropDown(usuario.Tipo_Usuario_Id);
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Update(usuario);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarTiposUsuarioDropDown(usuario?.Tipo_Usuario_Id);
            return View(usuario);
        }


        //Eliminar
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _context.PROGRATHON_Usuario
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Usuario_Id == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.PROGRATHON_Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.PROGRATHON_Usuario.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
