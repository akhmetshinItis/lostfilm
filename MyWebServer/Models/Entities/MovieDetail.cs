namespace my_http.Models.Entities;

public class MovieDetail
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public decimal RatingIMDb { get; set; }
    public int Duration { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }

    public decimal Rating { get; set; }
}