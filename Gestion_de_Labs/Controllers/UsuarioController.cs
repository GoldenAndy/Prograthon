using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Gestion_de_Labs.Data;
using Gestion_de_Labs.Models;

namespace Gestion_de_Labs.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }

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
                {
                    op.Selected = op.Value == seleccionado.Value.ToString();
                }
            }

            ViewBag.TiposUsuario = opciones;
        }

        //GET:/Usuarios/Crear
        public IActionResult Crear()
        {
            CargarTiposUsuarioDropDown();
            return View();
        }

        //POST:/Usuarios/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Usuario _usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(_usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarTiposUsuarioDropDown(_usuario?.Tipo_Usuario_Id);
            return View(_usuario);
        }

        //GET:/Usuarios/Editar/x
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var itemUsuario = await _context.Usuario.FindAsync(id);
            if (itemUsuario == null) return NotFound();

            CargarTiposUsuarioDropDown(itemUsuario.Tipo_Usuario_Id);
            return View(itemUsuario);
        }

        //POST:/Usuarios/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Usuario _usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Update(_usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            CargarTiposUsuarioDropDown(_usuario?.Tipo_Usuario_Id);
            return View(_usuario);
        }

        //GET:/Usuarios/Eliminar/x
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var itemUsuario = await _context.Usuario.FindAsync(id);
            if (itemUsuario == null) return NotFound();

            return View(itemUsuario);
        }

    }
}
