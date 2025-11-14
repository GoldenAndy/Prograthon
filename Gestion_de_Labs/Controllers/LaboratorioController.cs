using Gestion_de_Labs.Models;
using Gestion_de_Labs.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Gestion_de_Labs.Controllers
{
    public class LaboratorioController : Controller
    {
        private readonly LaboratorioService _laboratorioService;

        public LaboratorioController(LaboratorioService laboratorioService)
        {
            _laboratorioService = laboratorioService;
        }

        public IActionResult Index()
        {
            var laboratorios = _laboratorioService.ListarTodos();
            return View(laboratorios);
        }

        [HttpGet]
        public IActionResult ObtenerTodos()
        {
            var laboratorios = _laboratorioService.ListarTodos();
            return Json(laboratorios);
        }

        [HttpGet]
        public IActionResult ObtenerPorId(int id)
        {
            var lab = _laboratorioService.ObtenerPorId(id);
            if (lab == null)
                return NotFound(new { mensaje = "No existe el laboratorio." });
            return Json(lab);
        }

        [HttpPost]
        public IActionResult Crear([FromBody] Laboratorio lab)
        {
            if (lab == null)
                return BadRequest(new { mensaje = "Datos inválidos (cuerpo vacío)." });

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage);
                return BadRequest(new { mensaje = "Validación fallida", errores });
            }

            try
            {
                _laboratorioService.Crear(lab);
                return Ok(new { mensaje = "Laboratorio creado correctamente" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "DB error al crear",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al crear", detalle = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Editar([FromBody] Laboratorio lab)
        {
            if (lab == null)
                return BadRequest(new { mensaje = "Datos inválidos (cuerpo vacío)." });

            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception?.Message : e.ErrorMessage);
                return BadRequest(new { mensaje = "Validación fallida", errores });
            }

            try
            {
                _laboratorioService.Editar(lab);
                return Ok(new { mensaje = "Laboratorio editado correctamente" });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "DB error al editar",
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al editar", detalle = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _laboratorioService.Eliminar(id);
                return Json(new { exito = true, mensaje = "Laboratorio eliminado correctamente." });
            }
            catch (InvalidOperationException ex)
            {

                return Json(new { exito = false, mensaje = ex.Message });
            }
            catch (DbUpdateException dbEx)
            {
                var inner = dbEx.InnerException;
                var innerType = inner?.GetType().FullName ?? "";
                var innerMsg = inner?.Message ?? dbEx.Message;

                bool esFK =
                    innerMsg.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) ||
                    innerMsg.Contains("constraint fails", StringComparison.OrdinalIgnoreCase) ||
                    innerType.Contains("MySqlConnector.MySqlException") ||
                    innerType.Contains("MySql.Data.MySqlClient.MySqlException");

                if (esFK)
                {
                    return Json(new
                    {
                        exito = false,
                        mensaje = "No se puede eliminar el laboratorio porque tiene reservaciones en curso."
                    });
                }

                return Json(new { exito = false, mensaje = "Error de base de datos al eliminar el laboratorio.", detalle = innerMsg });
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error al intentar eliminar el laboratorio.", detalle = ex.Message });
            }
        }
    }
}
