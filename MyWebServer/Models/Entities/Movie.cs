namespace my_http.Models.Entities;

public class Movie
{
    public int? Id { get; set; }
    public string TitleRu { get; set; }
    public string TitleEng { get; set; }
    public DateTime ReleaseDateWorld { get; set; }
    public DateTime ReleaseDateRu { get; set; }
}