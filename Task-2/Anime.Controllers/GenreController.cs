using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CrudApi.Data;
using Microsoft.EntityFrameworkCore;
using CrudApi.Models;
using CrudApi.DTOs;

namespace CrudApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly AnimeDbContext _db;

        public GenreController(AnimeDbContext db)
        {
            _db = db;
        }

       [HttpGet]

       public async Task<IActionResult>GetAll ()
       {
           var Genres = await _db.Genres.Select(t => new GenreDto
           {
               Id = t.Id,
               Name = t.Name,
              
         
           }).ToListAsync();
           if(Genres is null)
            {
               return NotFound();
            }
            return Ok(Genres);
       }


       [HttpGet("{Id:int}")]
      public async Task<IActionResult>GetById(int Id)
        {
            var Genres = await _db.Genres.FindAsync(Id);
            if(Genres is null)
            {
                return NotFound();
            }
            var dto = new GenreDto
            {
                Id = Genres.Id,
                Name = Genres.Name,
                Description = Genres.Description
           
            };
            return Ok(dto);
        }


      [HttpPost]

       public async Task<IActionResult> Create(CreateGenreDto dto)
        {
            if(dto is null || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Description))
            {
                return BadRequest("Name is required and you subject ");
            }

            var Genres = new Genre
            {
                Name = dto.Name,
               Description = dto.Description,
           
            };
            await _db.Genres.AddAsync(Genres);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{Id:int}")]

        public async Task<IActionResult> Update(int Id , UpdateGenreDto dto)
        {
            var exist =  await _db.Genres.FindAsync(Id);
            if (exist is null)
            {
                return NotFound();
            }

            exist.Name = dto.Name;
            exist.Description = dto.Description!;
         

            await _db.SaveChangesAsync();

            return Ok(exist);
        }


        [HttpDelete("{Id:int}")]

        public async Task<IActionResult> Delete(int Id )
        {
            var Genre = await _db.Genres.FindAsync();

            if (Genre is null)
            {
                return NotFound();
            }
          
            _db.Genres.Remove(Genre);

            await _db.SaveChangesAsync();

            return Ok();
        }


    }
}