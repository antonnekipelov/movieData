namespace RecomendMovie.Models
{
    public class MovieRating
    {
        public User User { get; set; }          // Данные пользователя
        public int MovieId { get; set; }       // ID фильма
        public bool Rate { get; set; }          // true - Лайк, false - Дизлайк
    }
}
