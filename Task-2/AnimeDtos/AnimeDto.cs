using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApi.DTOs
{
    public class AnimeDto
    {
        public int Id { get; set; }

        public  required string Title { get; set; }
      
        public  int Episodes{ get; set; }
       
        public double Rating {get; set;}

    }
}