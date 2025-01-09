namespace my_http.Models.FilmPage;

public class FilmPageDataVm
{
    public decimal Rating { get; set; }
    public decimal ImdbRating { get; set; }
    public string TitleRu { get; set; }
    public string TitleEng { get; set; }
    public string ReleaseDateWorld { get; set; }
    public string ReleaseDateRu { get; set; }
    public string GenreNames { get; set; }
    public string Duration { get; set; }
    public string Type { get; set; }

    public bool IsAuthorized { get; set; }

    public string Username { get; set; } = "";

    public string Website { get; set; }

    public string ImageUrl { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/6/6e/Hylobates_agilis_2.jpg";
}