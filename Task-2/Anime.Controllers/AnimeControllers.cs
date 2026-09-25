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

   
    public class AnimeController : ControllerBase
    {
     private readonly AnimeDbContext _db;
       
       public AnimeController(AnimeDbContext db)
        {
            _db = db;
        }

       [HttpGet]

       public async Task<IActionResult> GetAll()
        {
           var Anime = await _db.Anime.Select(a => new AnimeDto
           {
               Id = a.Id,
               Title = a.Title,
               Episodes = a.Episodes,
               Rating = a.Rating,
             

           }).ToListAsync();
           if(Anime is null)
            {
                return NotFound();
            }
            return Ok(Anime);
        }
        
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var Anime = await _db.Anime.FindAsync(Id);
            if(Anime is null)
            {
                return NotFound();
            }
            var dto = new AnimeDto
            {
                Id = Anime.Id,
                Title = Anime.Title,
                Episodes = Anime.Episodes,
                Rating = Anime.Rating
            
            };
            return Ok(Anime);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateAnimeDto dto)
        {
            if(dto is null || string.IsNullOrEmpty(dto.Title))
            {
                return BadRequest("Name is required and Lastname");
                
            }
         
            var Anime = new Anime
            {
                Title = dto.Title,
                Episodes = dto.Episodes,
                Rating= dto.Rating
              
            };
            await   _db.Anime.AddAsync(Anime);
            await   _db.SaveChangesAsync();

            return  NoContent();
        } 

       [HttpPut("{Id:int}")]


        public async Task<IActionResult> Update(int Id , UpdateAnimeDto dto)
        {
            
            var exist = await _db.Anime.FindAsync(Id);

            if(exist is null)
            {
                return NotFound();
            }

            exist.Title = dto.Title;
            exist.Episodes = dto.Episodes;
            exist.Rating= dto.Rating;

            await _db.SaveChangesAsync();
            return Ok(exist);
        }
        
        [HttpDelete("{Id:int}")]

        public async Task<IActionResult>Delete(int Id)
        {
            
            var Anime = await _db.Anime.FindAsync(Id);

            if(Anime is null)
            {
                return NotFound();
            }


            _db.Anime.Remove(Anime);

            await _db.SaveChangesAsync();
            
            return Ok();

        }

    }
}