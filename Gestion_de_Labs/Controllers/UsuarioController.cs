using Gestion_de_Labs.Models;
using Gestion_de_Labs.Service;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_Labs.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var usuarios = _usuarioService.ListarTodos();
            return Json(usuarios);
        }

        [HttpGet]
        public IActionResult ObtenerPorId(int id)
        {
            var usuario = _usuarioService.ObtenerPorId(id);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Json(usuario);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Usuario usuario)
        {
            if (usuario == null || !ModelState.IsValid)
            {
                return BadRequest(new { mensaje = "Datos inválidos" });
            }

            _usuarioService.Crear(usuario);
            return Ok(new { mensaje = "Usuario creado correctamente." });
        }

        [HttpPost]
        public IActionResult Editar([FromBody] Usuario usuario)
        {
            if (usuario == null || !ModelState.IsValid)
            {
                return BadRequest(new { mensaje = "Datos inválidos" });
            }

            _usuarioService.Editar(usuario);
            return Ok(new { mensaje = "Usuario editado correctamente." });
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _usuarioService.Eliminar(id);
                return Json(new { exito = true, mensaje = "Usuario eliminado correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { exito = false, mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error al eliminar.", detalle = ex.Message });
            }
        }
    }
}
