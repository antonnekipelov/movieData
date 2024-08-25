using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecomendMovie.Models
{
    public class MovieRating
    {
        public Movie Movie { get; set; } // Экземпляр модели фильма
        public bool Rate { get; set; }//true - Лайк, false - Диздайк
    }
}
