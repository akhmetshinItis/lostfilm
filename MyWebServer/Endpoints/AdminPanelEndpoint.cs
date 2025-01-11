using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Microsoft.Data.SqlClient;
using my_http.Helpers;
using my_http.Models.AdminPanel;
using my_http.Models.Entities;
using my_http.Repositories.EntityRepositories;
using MyHttp.Repositories;
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
        try
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
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new {Message = e.Message});        }
    }

    [Post("admin/movies/add")]
    public IHttpResponseResult AddMovie(Movie movie)
    {
        try
        {
            var repository = new Repository<Movie>();
            var titleEng = movie.TitleEng;
            var titleRu = movie.TitleRu;
            var releaseDateWorld = movie.ReleaseDateWorld;
            var releaseDateRu = movie.ReleaseDateRu;

            var savedFilm = repository.FirstOrDefault(m =>
                m.TitleEng == titleEng && m.TitleRu == titleRu && m.ReleaseDateWorld == releaseDateWorld &&
                m.ReleaseDateRu == releaseDateRu);
            if (savedFilm is not null)
            {
                var result1 = Json(false);
                return result1;
            }
            
            repository.Create(movie);
            savedFilm = repository.FirstOrDefault(m =>
                m.TitleEng == titleEng && m.TitleRu == titleRu && m.ReleaseDateWorld == releaseDateWorld &&
                m.ReleaseDateRu == releaseDateRu);
            movie.Id = savedFilm.Id;
            var result = Json(movie);
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new {Message = e.Message});        }
    }
    
    [Post("admin/details/add")]
    public IHttpResponseResult AddDetails(MovieDetail detail)
    {
        try
        {
            var repository = new Repository<MovieDetail>();

            var description = detail.Description;
            var movieId = detail.MovieId;
            var rating = detail.RatingIMDb;
            var duration = detail.Duration;
            var type = detail.Type;
            var imageUrl = detail.ImageUrl;

            var savedDetail = repository.FirstOrDefault(m =>
                m.Description == description && m.MovieId == movieId && m.RatingIMDb == rating
                && m.Duration == duration && m.Type == type && m.ImageUrl == imageUrl);
            if (savedDetail is not null)
            {
                return Json(false);
            }

            repository.Create(detail);
            savedDetail = repository.FirstOrDefault(m =>
                m.Description == description && m.MovieId == movieId && m.RatingIMDb == rating
                && m.Duration == duration && m.Type == type && m.ImageUrl == imageUrl);
            detail.Id = savedDetail.Id;
            var result = Json(detail);
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new {Message = e.Message});        }
    }
    
    [Post("admin/genres/add")]
    public IHttpResponseResult AddGenre(Genre genre)
    {
        try
        {
            var repository = new Repository<Genre>();

            var name = genre.Name;
            var description = genre.Description;
            var usages = genre.UsageCount;

            var savedGenre = repository.FirstOrDefault(g =>
                g.Description == description && g.Name == name && g.UsageCount == usages);
            if (savedGenre is not null)
            {
                return Json(false);
            }

            repository.Create(genre);
            savedGenre = repository.FirstOrDefault(g =>
                g.Description == description && g.Name == name && g.UsageCount == usages);
            genre.Id = savedGenre.Id;
            var result = Json(genre);
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new {Message = e.Message});        }
    }
    
    [Post("admin/moviesgenres/add")]
    public IHttpResponseResult AddMovieGenre(MovieGenre movieGenre)
    {
        try
        {
            var repository = new Repository<MovieGenre>();

            var movieId = movieGenre.MovieId;
            var genreId = movieGenre.GenreId;
            var addedDate = movieGenre.AddedDate;

            var savedMovieGenre = repository.FirstOrDefault(mg =>
                mg.MovieId == movieId && mg.GenreId == genreId && mg.AddedDate == addedDate);
            if (savedMovieGenre is not null)
            {
                return Json(false);
            }

            if (!ValidateMovieGenreConstraint(movieGenre.MovieId, movieGenre.GenreId))
            {
                //знаю что так нельзя, но в теории ничего страшного
                return Json(1);
            }

            repository.Create(movieGenre);
            savedMovieGenre = repository.FirstOrDefault(mg =>
                mg.MovieId == movieId && mg.GenreId == genreId && mg.AddedDate == addedDate);
            movieGenre.Id = savedMovieGenre.Id;
            var result = Json(movieGenre);
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new {Message = e.Message});        }
    }
    
    [Post("admin/users/add")]
    public IHttpResponseResult AddUser(User user)
    {
        try
        {
            var repository = new Repository<User>();

            var username = user.Username;
            var login = user.Login;
            var password = user.Password;

            var savedUser = repository.FirstOrDefault(u =>
                u.Login == login);
            if (savedUser is not null)
            {
                return Json(false);
            }

            repository.Create(user);
            savedUser = repository.FirstOrDefault(u =>
                u.Login == login);
            user.Id = savedUser.Id;
            var result = Json(user);
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
            return Json(new {Message = e.Message});        }
    }
    

    // Delete

    [Post("admin/movies/delete")]
    public IHttpResponseResult DeleteMovie(int MovieId)
    {
        var repository = new Repository<Movie>();
        try
        {
            repository.Delete(repository.GetById(MovieId));
        }
        catch (Exception e)
        {
            return Redirect($"/error?error={e.ToString()}");
        }
        return Redirect("/admin");
    }
    private List<T> GetData<T>() where T : class, new()
    {
        var repository = new Repository<T>();
        var users = repository.GetAll();
        return users;
    }

    private bool ValidateMovieGenreConstraint(int movieId, int genreId)
    {
        var movieRepository = new MovieRepository();
        var movie = movieRepository.GetById(movieId);
        var genreRepository = new GenreRepository();
        var genre = genreRepository.GetById(genreId);
        return genre is not null && movie is not null;
    }
}