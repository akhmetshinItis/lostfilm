using my_http.Models.Entities;

namespace my_http.Models.MainPage;

public class MainPageDataVm
{
    public List<MovieCardVm> MovieCards { get; set; } = new();
    public List<Genre> Genres { get; set; } = new();
    public string Username { get; set; } = "";
    public bool IsAuthorized { get; set; }
}