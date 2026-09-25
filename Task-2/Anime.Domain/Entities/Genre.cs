using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudApi.Models
{
    public class Genre
    {
        public int Id {get; set; }

        public  required string Name {get; set;} 

        public  string Description  {get; set;} = string.Empty;


    } 
}