using Gestion_de_Labs.Models;
using Gestion_de_Labs.Service;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public IActionResult Crear([FromBody] Laboratorio lab)
        {
            if (lab == null)
                return BadRequest("Datos inválidos");

            _laboratorioService.Crear(lab);
            return Ok(new { mensaje = "Laboratorio creado correctamente" });
        }

        [HttpPost]
        public IActionResult Editar([FromBody] Laboratorio lab)
        {
            if (lab == null)
                return BadRequest("Datos inválidos");

            _laboratorioService.Editar(lab);
            return Ok(new { mensaje = "Laboratorio editado correctamente" });
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest("ID inválido");

            _laboratorioService.Eliminar(id);
            return Ok(new { mensaje = "Laboratorio eliminado correctamente" });
        }
    }
}
