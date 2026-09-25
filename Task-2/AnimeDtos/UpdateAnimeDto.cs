using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CrudApi.DTOs
{
    public class UpdateAnimeDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        [Range(1 , 5000 )]
        public int Episodes { get; set; }

        public double Rating { get; set; }
    }
}