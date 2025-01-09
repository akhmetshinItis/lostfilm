using my_http.Models.Entities;

namespace my_http.Models.UpdateObjectsVms;

public class UpdateMovieDetailsVm
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string RatingIMDb { get; set; }
    public int Duration { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public string Rating { get; set; }
    public string Website { get; set; }


    public UpdateMovieDetailsVm(MovieDetail movieDetail)
    {
        Id = movieDetail.Id;
        MovieId = movieDetail.MovieId;
        RatingIMDb = movieDetail.RatingIMDb.ToString().Replace(',', '.');
        Duration = movieDetail.Duration;
        Type = movieDetail.Type;
        Description = movieDetail.Description;
        ImageUrl = movieDetail.ImageUrl;
        Rating = movieDetail.Rating.ToString().Replace(',', '.');
        Website = movieDetail.Website;
    }
}