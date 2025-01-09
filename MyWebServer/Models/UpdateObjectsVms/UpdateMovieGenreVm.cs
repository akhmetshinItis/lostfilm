using my_http.Models.Entities;

namespace my_http.Models.UpdateObjectsVms;

public class UpdateMovieGenreVm
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int GenreId { get; set; }
    public string AddedDate { get; set; }

    public UpdateMovieGenreVm(MovieGenre movieGenre)
    {
        Id = movieGenre.Id;
        MovieId = movieGenre.MovieId;
        GenreId = movieGenre.GenreId;
        AddedDate = movieGenre.AddedDate.ToString("yyyy-MM-dd");
    }
}