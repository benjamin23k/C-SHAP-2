using Microsoft.AspNetCore.Mvc;
using DuesApi.Dtos;
using DuesApi.Models;
using DuesApi.Services;

namespace DuesApi.Controllers
{
    [ApiController]
    [Route("api/miembros")]
    public class MiembrosComunidadController : ControllerBase
    {
        private readonly DataStore _db;

        public MiembrosComunidadController(DataStore db) => _db = db;

        [HttpGet]
        public ActionResult<IEnumerable<MiembroDeLaComunidad>> GetAll() => Ok(_db.Miembros);

        [HttpGet("{id}")]
        public ActionResult<MiembroDeLaComunidad> GetById(int id)
        {
            var miembro = _db.Miembros.FirstOrDefault(x => x.Id == id);
            return miembro is null ? NotFound() : Ok(miembro);
        }

        [HttpPost]
        public ActionResult<MiembroDeLaComunidad> Create(MiembroDto dto)
        {
            var miembro = new MiembroDeLaComunidad
            {
                Id = _db.NextMiembroId(),
                Nombre = dto.Nombre,
                Tipo = dto.Tipo
            };

            _db.Miembros.Add(miembro);
            return CreatedAtAction(nameof(GetById), new { id = miembro.Id }, miembro);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, MiembroDto dto)
        {
            var miembro = _db.Miembros.FirstOrDefault(x => x.Id == id);
            if (miembro is null) return NotFound();

            miembro.Nombre = dto.Nombre;
            miembro.Tipo = dto.Tipo;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var miembro = _db.Miembros.FirstOrDefault(x => x.Id == id);
            if (miembro is null) return NotFound();

            _db.Miembros.Remove(miembro);
            return NoContent();
        }
    }
}
