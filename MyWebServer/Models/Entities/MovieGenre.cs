namespace my_http.Models.Entities;

public class MovieGenre
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int GenreId { get; set; }
    public DateTime AddedDate { get; set; }
}