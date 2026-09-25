using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApi.DTOs
{
    public class CreateAnimeDto
    {

        [Required]
        public required string Title { get; set; }


        [Required]
        [Range(1 , 5000)]
        public int Episodes { get; set; }

        [Required]
        public double Rating { get; set; } 

        [Required]
        public int ReleaseYear { get; set; }

        [Required]
        public int GenreId { get; set; }
    }

}