using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using my_http.Helpers;
using my_http.Models.Entities;
using my_http.Models.UpdateObjectsVms;
using MyHttp.Repositories;
using MyORMLibrary;
using TemlateEngine;

namespace my_http.Endpoints;

public class UpdateObjectsEndpoint : EndpointBase
{
     private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();

    // Универсальный метод для получения страницы обновления
    private IHttpResponseResult GetUpdatePage<T>(int id, string templatePath) where T : class, new()
    {
        var repository = new Repository<T>();
        var entity = repository.GetById(id);

        if (entity is null)
        {
            return Redirect("/admin");
        }
        
        object model;
        if (typeof(T) == typeof(MovieDetail))
        {
            model = new UpdateMovieDetailsVm((MovieDetail)(object)entity);
        }
        else if (typeof(T) == typeof(Movie))
        {
            model = new UpdateMovieVm((Movie)(object)entity);
        }
        else if (typeof(T) == typeof(MovieGenre))
        {
            model = new UpdateMovieGenreVm((MovieGenre)(object)entity);
        }
        else
        {
            model = entity; // Для всех остальных сущностей используем саму модель
        }

        var text = _responseHelper.GetResponseTextCustomPath(templatePath);
        var page = _htmlTemplateEngine.Render(text, model);
        return Html(page);
    }

    // Универсальный метод для обновления сущности
    private IHttpResponseResult UpdateEntity<T>(T entity) where T : class, new()
    {
        var repository = new Repository<T>();
        repository.Update(entity);
        return Redirect("/admin");
    }

    [Get("admin/movies/update")]
    public IHttpResponseResult GetUpdateMoviePage(int id)
        => GetUpdatePage<Movie>(id, "Views/Templates/Pages/UpdatePages/Update-Movie.html");

    [Post("admin/movies/update")]
    public IHttpResponseResult UpdateMovie(Movie movie)
        => UpdateEntity(movie);
    

    [Get("admin/genres/update")]
    public IHttpResponseResult GetUpdateGenresPage(int id)
        => GetUpdatePage<Genre>(id, "Views/Templates/Pages/UpdatePages/Update-Genre.html");

    [Post("admin/genres/update")]
    public IHttpResponseResult UpdateGenre(Genre genre)
        => UpdateEntity(genre);

    [Get("admin/users/update")]
    public IHttpResponseResult GetUpdateUsersPage(int id)
        => GetUpdatePage<User>(id, "Views/Templates/Pages/UpdatePages/Update-User.html");

    [Post("admin/users/update")]
    public IHttpResponseResult UpdateUser(User user)
        => UpdateEntity(user);
    
    [Get("admin/details/update")]
    public IHttpResponseResult GetUpdateDetailsPage(int id)
        => GetUpdatePage<MovieDetail>(id, "Views/Templates/Pages/UpdatePages/Update-MovieDetails.html");

    [Post("admin/details/update")]
    public IHttpResponseResult UpdateDetail(MovieDetail movieDetail)
        => UpdateEntity(movieDetail);
    
    
    [Get("admin/moviesgenres/update")]
    public IHttpResponseResult GetUpdateMoviesGenresPage(int id)
        => GetUpdatePage<MovieGenre>(id, "Views/Templates/Pages/UpdatePages/Update-MovieGenre.html");

    [Post("admin/moviesgenres/update")]
    public IHttpResponseResult UpdateMoviesGenre(MovieGenre movieGenre)
        => UpdateEntity(movieGenre);
}