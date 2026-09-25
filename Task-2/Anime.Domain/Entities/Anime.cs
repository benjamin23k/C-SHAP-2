using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApi.Models
{
    public class Anime
    {
        public int Id { get; set; }

        public  required string Title { get; set; } 

        public int Episodes { get; set; }

        public double Rating { get; set; } 
        
        public  int ReleaseYear {get; set;}

        public int GenreId {get; set;}

    }
}