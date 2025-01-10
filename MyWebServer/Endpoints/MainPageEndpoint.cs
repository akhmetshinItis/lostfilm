using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using my_http.Helpers;
using my_http.Helpers.AuthHelpers;
using my_http.Models.Entities;
using my_http.Models.MainPage;
using my_http.Repositories;
using MyHttp.Repositories;
using TemlateEngine;

namespace my_http.Endpoints;

public class MainPageEndpoint : EndpointBase
{
    private readonly ResponseHelper _responseHelper = new ResponseHelper();
    private readonly AppConfig _appConfig = AppConfig.GetInstance();
    private readonly IHtmlTemplateEngine _htmlTemplateEngine = new HtmlTemplateEngine();
    private AuthorizationHelper _authorizationHelper = new AuthorizationHelper();
    
    [Get("index")]
    public IHttpResponseResult Index()
    {
        var localPath = "Views/Templates/Pages/MainPage/index.html";
        try
        {
            var responseText = _responseHelper.GetResponseTextCustomPath(localPath);
            var movieCardsRepository = new MovieCardRepository();
            var movieCards = movieCardsRepository.GetMovieCards();
            var model = new MainPageDataVm
            {
                MovieCards = movieCards,
                Genres = new Repository<Genre>().GetAll(),
                IsAuthorized = _authorizationHelper.IsAuthorized(Context),
            };
            CheckAuthorization(model);
            var page = _htmlTemplateEngine.Render(responseText, model);
            return Html(page);
        }
        catch (Exception e)
        {
            return Redirect($"/error?error={e.ToString()}");
        }
    }
    
    private void CheckAuthorization(MainPageDataVm model)
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