public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Date { get; set; }  // Год релиза
    public string Tagline { get; set; }
    public string Description { get; set; }
    public int Minute { get; set; }  // Длительность в минутах
    public double Rating { get; set; }  // Рейтинг по пятибалльной шкале
    //public string PosterPath { get; set; }
}
