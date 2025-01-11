using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using my_http.Helpers;
using my_http.Helpers.AuthHelpers;
using my_http.Models.Entities;
using my_http.Models.FilmPage;
using my_http.Repositories;
using MyHttp.Repositories;
using TemlateEngine;

namespace my_http.Endpoints;

public class FilmPageEndpoint : EndpointBase
{
    
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();
    private AuthorizationHelper _authorizationHelper = new AuthorizationHelper();
    
    [Get("film")]
    public IHttpResponseResult Index(int id)
    {
        try
        {
            if (id == 0)
            {
                return Redirect("index");
            }

            var localPath = "Views/Templates/Pages/FilmPage/film-page.html";
            var responseText = _responseHelper.GetResponseTextCustomPath(localPath);
            var model = new FilmPageDataRepository().GetMovieById(id);
            CheckAuthorization(model);
            var page = _htmlTemplateEngine.Render(responseText, model);
            return Html(page);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Error(e.Message);
        }
    }
    
    private void CheckAuthorization(FilmPageDataVm model)
    {
        
        model.IsAuthorized = _authorizationHelper.IsAuthorized(Context); // Используем метод проверки авторизации
        if (model.IsAuthorized){
        
            var userId = Int32.Parse(SessionStorage.GetUserId(Context.Request.Cookies["session-token"].Value));
            if (userId != 0)
            {
                var repository = new Repository<User>();
                model.Username = repository.FirstOrDefault(u => u.Id == userId).Username;
            }
        }
    }
}