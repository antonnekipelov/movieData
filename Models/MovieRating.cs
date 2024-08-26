namespace RecomendMovie.Models
{
    public class MovieRating
    {
        public User User { get; set; }          // Данные пользователя
        public Movie Movie { get; set; }        // Данные о фильме
        public bool Rate { get; set; }          // true - Лайк, false - Дизлайк
    }
}
