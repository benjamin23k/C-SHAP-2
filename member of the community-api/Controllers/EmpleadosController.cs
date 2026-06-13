using Microsoft.AspNetCore.Mvc;
using DuesApi.Dtos;
using DuesApi.Models;
using DuesApi.Services;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/empleados")]
    public class EmpleadosController : ControllerBase
    {
        private readonly DataStore _db;

        public EmpleadosController(DataStore db) => _db = db;

        [HttpGet]
        public ActionResult<IEnumerable<Empleado>> GetAll() => Ok(_db.Empleados);

        [HttpGet("{id}")]
        public ActionResult<Empleado> GetById(int id)
        {
            var empleado = _db.Empleados.FirstOrDefault(x => x.Id == id);
            return empleado is null ? NotFound() : Ok(empleado);
        }

        [HttpPost]
        public ActionResult<Empleado> Create(EmpleadoDto dto)
        {
            var empleado = new Empleado
            {
                Id = _db.NextEmpleadoId(),
                Nombre = dto.Nombre,
                Tipo = "Empleado",
                Departamento = dto.Departamento
            };

            _db.Empleados.Add(empleado);
            return CreatedAtAction(nameof(GetById), new { id = empleado.Id }, empleado);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, EmpleadoDto dto)
        {
            var empleado = _db.Empleados.FirstOrDefault(x => x.Id == id);
            if (empleado is null) return NotFound();

            empleado.Nombre = dto.Nombre;
            empleado.Departamento = dto.Departamento;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var empleado = _db.Empleados.FirstOrDefault(x => x.Id == id);
            if (empleado is null) return NotFound();

            _db.Empleados.Remove(empleado);
            return NoContent();
        }
    }
}
