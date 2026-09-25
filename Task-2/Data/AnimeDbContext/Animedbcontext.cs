using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CrudApi.Models;


namespace CrudApi.Data
{
    public class AnimeDbContext: DbContext
    {
      public AnimeDbContext(DbContextOptions<AnimeDbContext> options)
           : base(options)
        {
            
        }
      public DbSet<Anime>Anime { get; set; }
      
      public DbSet<Genre>Genres { get; set; }

    }
}