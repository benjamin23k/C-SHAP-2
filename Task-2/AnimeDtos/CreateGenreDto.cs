using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApi.DTOs
{
    public class CreateGenreDto
    {
        [Required]
        public required string Name { get; set; }

        public   string? Description {get; set;} 

     

    }
}