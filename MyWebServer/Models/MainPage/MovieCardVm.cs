namespace my_http.Models.MainPage;

public class MovieCardVm
{
    public int Id { get; set; }
    public string TitleRu { get; set; }
    public string TitleEng { get; set; }
    public string ReleaseYear { get; set; }
    public string Genre { get; set; }
    public string Rating { get; set; }
    public string ImageUrl { get; set; }
}