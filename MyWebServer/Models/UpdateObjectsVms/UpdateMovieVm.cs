using my_http.Models.Entities;

namespace my_http.Models.UpdateObjectsVms;

public class UpdateMovieVm
{
    public int? Id { get; set; }
    public string TitleRu { get; set; }
    public string TitleEng { get; set; }
    public string ReleaseDateWorld { get; set; }
    public string ReleaseDateRu { get; set; }

    public UpdateMovieVm(Movie movie)
    {
        Id = movie.Id;
        TitleRu = movie.TitleRu ?? string.Empty;
        TitleEng = movie.TitleEng ?? string.Empty;
        ReleaseDateWorld = movie.ReleaseDateWorld.ToString("yyyy-MM-dd");
        ReleaseDateRu = movie.ReleaseDateRu.ToString("yyyy-MM-dd");
    }
}