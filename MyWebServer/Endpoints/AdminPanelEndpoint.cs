using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using my_http.Models.AdminPanel;
using my_http.Models.Entities;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class AdminPanelEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();

    [Get("admin")]
    public IHttpResponseResult Index()
    {
        var localPath = "Views/Templates/Pages/AdminPanel/index.html";
        var responseText = _responseHelper.GetResponseTextCustomPath(localPath);
        var model = new DataVm
        {
            Genres = GetData<Genre>(),
            MovieDetailsList = GetData<MovieDetail>(),
            MovieGenres = GetData<MovieGenre>(),
            Movies = GetData<Movie>(),
            Users = GetData<User>()
        };
        var page = _htmlTemplateEngine.Render(responseText, model);
        return Html(page);
    }

    [Post("admin/movies/add")]
    public IHttpResponseResult AddMovie(Movie movie)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<Movie>(connection);
        var titleEng = movie.TitleEng;
        var titleRu = movie.TitleRu;
        var releaseDateWorld = movie.ReleaseDateWorld;
        var releaseDateRu = movie.ReleaseDateRu;
        
        var savedFilm = dbContext.FirstOrDefault(m =>
            m.TitleEng == titleEng && m.TitleRu == titleRu && m.ReleaseDateWorld == releaseDateWorld &&
            m.ReleaseDateRu == releaseDateRu);
        if (savedFilm is not null)
        {
            var result1 = Json(false);
            return result1;
        }

        dbContext.Create(movie);
        savedFilm = dbContext.FirstOrDefault(m =>
            m.TitleEng == titleEng && m.TitleRu == titleRu && m.ReleaseDateWorld == releaseDateWorld &&
            m.ReleaseDateRu == releaseDateRu);
        movie.Id = savedFilm.Id;
        var result = Json(movie);
        return result;
    }
    
    [Post("admin/details/add")]
    public IHttpResponseResult AddDetails(MovieDetail detail)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<MovieDetail>(connection);
        
        var description = detail.Description;
        var movieId = detail.MovieId;
        var rating = detail.RatingIMDb;
        var duration = detail.Duration;
        var type = detail.Type;
        var imageUrl = detail.ImageUrl;
        
        var savedDetail = dbContext.FirstOrDefault(m => 
            m.Description == description && m.MovieId == movieId && m.RatingIMDb == rating
            && m.Duration == duration && m.Type == type && m.ImageUrl == imageUrl);
        if (savedDetail is not null)
        {
            return Json(false);
        }
    
        dbContext.Create(detail);
        savedDetail = dbContext.FirstOrDefault(m => 
            m.Description == description && m.MovieId == movieId && m.RatingIMDb == rating
            && m.Duration == duration && m.Type == type && m.ImageUrl == imageUrl);
        detail.Id = savedDetail.Id;
        var result = Json(detail);
        return result;
    }
    
    [Post("admin/genres/add")]
    public IHttpResponseResult AddGenre(Genre genre)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<Genre>(connection);

        var name = genre.Name;
        var description = genre.Description;
        var usages = genre.UsageCount;
        
        var savedGenre = dbContext.FirstOrDefault(g => 
            g.Description == description && g.Name == name && g.UsageCount == usages);
        if (savedGenre is not null)
        {
            return Json(false);
        }
    
        dbContext.Create(genre);
        savedGenre = dbContext.FirstOrDefault(g => 
            g.Description == description && g.Name == name && g.UsageCount == usages);
        genre.Id = savedGenre.Id;
        var result = Json(genre);
        return result;
    }
    
    [Post("admin/moviesgenres/add")]
    public IHttpResponseResult AddMovieGenre(MovieGenre movieGenre)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<MovieGenre>(connection);

        var movieId = movieGenre.MovieId;
        var genreId = movieGenre.GenreId;
        var addedDate = movieGenre.AddedDate;
        
        var savedMovieGenre = dbContext.FirstOrDefault(mg => 
            mg.MovieId == movieId && mg.GenreId == genreId && mg.AddedDate == addedDate);
        if (savedMovieGenre is not null)
        {
            return Json(false);
        }
    
        dbContext.Create(movieGenre);
        savedMovieGenre = dbContext.FirstOrDefault(mg => 
            mg.MovieId == movieId && mg.GenreId == genreId && mg.AddedDate == addedDate);
        movieGenre.Id = savedMovieGenre.Id;
        var result = Json(movieGenre);
        return result;
    }
    
    [Post("admin/users/add")]
    public IHttpResponseResult AddUser(User user)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<User>(connection);

        var username = user.Username;
        var login = user.Login;
        var password = user.Password;
        
        var savedUser = dbContext.FirstOrDefault(u => 
            u.Login == login);
        if (savedUser is not null)
        {
            return Json(false);
        }
    
        dbContext.Create(user);
        savedUser = dbContext.FirstOrDefault(u => 
            u.Login == login);
        user.Id = savedUser.Id;
        var result = Json(user);
        return result;
    }
    

    // Delete

    [Post("admin/movies/delete")]
    public IHttpResponseResult DeleteMovie(int MovieId)
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<Movie>(connection);
        dbContext.Delete(dbContext.GetById(MovieId));
        return Redirect("/admin");
    }
    private List<T> GetData<T>() where T : class, new()
    {
        using var connection = new SqlConnection(_appConfig.ConnectionString);
        var dbContext = new ORMContext<T>(connection);
        var users = dbContext.GetAll();
        return users;
    }
}