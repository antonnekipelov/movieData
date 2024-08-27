using CsvHelper.Configuration.Attributes;
using System.Diagnostics.Metrics;
using System.IO;

public class Movie
{
    [Name("id")]
    public int Id { get; set; }

    [Name("name")]
    public string Name { get; set; }

    [Name("date")]
    public int Date { get; set; }

    [Name("tagline")]
    public string Tagline { get; set; }

    [Name("description")]
    public string Description { get; set; }

    [Name("minute")]
    public int Minute { get; set; }

    [Name("rating")]
    public double Rating { get; set; }
}


