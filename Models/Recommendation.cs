using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecomendMovie.Models
{
    public class Recommendation
    {
        public Movie Movie { get; set; }
        public string Reason { get; set; }
    }
}