using RecomendMovie.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecomendMovie.Services
{
    public interface IMovieService
    {
        IEnumerable<Movie> GetMovies();
        IEnumerable<Recommendation> GetRecommendations(Movie movie);
    }
}