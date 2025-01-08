using my_http.Models.Entities;

namespace my_http.Models.AdminPanel;

public class DataVm
{
    public List<Entities.User> Users { get; set; }
    public List<Genre>  Genres { get; set; }
    public List<Movie> Movies { get; set; }
    public List<MovieDetail> MovieDetailsList { get; set; }
    public List<MovieGenre> MovieGenres { get; set; }
}