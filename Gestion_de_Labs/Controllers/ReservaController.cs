using Gestion_de_Labs.Models;
using Gestion_de_Labs.Service;
using Microsoft.AspNetCore.Mvc;

namespace Gestion_de_Labs.Controllers
{
    public class ReservaController : Controller
    {
        private readonly ReservaService _reservaService;
        private readonly UsuarioService _usuarioService;
        private readonly LaboratorioService _laboratorioService;

        public ReservaController(
            ReservaService reservaService,
            UsuarioService usuarioService,
            LaboratorioService laboratorioService)
        {
            _reservaService = reservaService;
            _usuarioService = usuarioService;
            _laboratorioService = laboratorioService;
        }

        // Vista principal
        public IActionResult Index() => View();

        // ======== API ========

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            return Json(_reservaService.ListarTodos());
        }

        [HttpGet]
        public IActionResult ObtenerPorId(int id)
        {
            var r = _reservaService.ObtenerPorId(id);
            if (r == null)
                return NotFound(new { mensaje = "Reserva no encontrada." });

            return Json(r);
        }

        [HttpGet]
        public IActionResult ListarUsuarios()
            => Json(_usuarioService.ListarTodos());

        [HttpGet]
        public IActionResult ListarLaboratorios()
            => Json(_laboratorioService.ListarTodos());


        [HttpPost]
        public IActionResult Crear([FromBody] Reservacion reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos inválidos." });

            _reservaService.Crear(reserva);
            return Ok(new { mensaje = "Reserva creada correctamente." });
        }

        [HttpPost]
        public IActionResult Editar([FromBody] Reservacion reserva)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { mensaje = "Datos inválidos." });

            _reservaService.Editar(reserva);
            return Ok(new { mensaje = "Reserva editada correctamente." });
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _reservaService.Eliminar(id);
                return Json(new { exito = true, mensaje = "Reserva eliminada correctamente." });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { exito = false, mensaje = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error inesperado.", detalle = ex.Message });
            }
        }
    }
}
