using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Channels;
using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;
using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using my_http.Helpers;


namespace my_http.Endpoints;
/// <summary>
/// Endpoint для работы с запросами отправляющими сообщения на почту
/// и запросами для генерации связанных с отправкой на почту страниц
/// Для тестов был включен метод <see cref="GetResponseText"/>
/// Позднее будет удален
/// </summary>
//[Endpoint("route")]
public class SendEmailEndpoint : EndpointBase
{
    public virtual IResponseHelper ResponseHelper { get; set; } = new ResponseHelper();
    /// <summary>
    /// Instance of <see cref="EmailService"/>
    /// Изначально для тестирования задал, но вроде как нормально пока
    /// хотя потом будут проблемы при асинхронной реализации
    /// </summary>
    public EmailService _emailService { get; set; } = new EmailService(AppConfig.GetInstance().EmailServiceConfiguration);
    /// <summary>
    /// Метод обрабатываюзий Post запрос по роуту "anime"
    /// получает из тела запроса имя и емейл пользователя
    /// создает юзера и возвразает <see cref="JsonResult"/> с его данными
    /// не знаю что тут делать, не понятный метод какой-то
    /// </summary>
    /// <param name="name">Имя пользователя</param>
    /// <param name="email">Емейл пользователя</param>
    /// <returns><see cref="JsonResult"/> Json файл с данными пользователя</returns>
    [Post("anime")]
    public IHttpResponseResult SendEmailAnime(string name, string email)
    {
        var user = new { Name = name, Email = email };
        return Json(user);
    }
    
    /// <summary>
    /// Метод обрабатывающий Post запрос по роутингу "login"
    /// получает из тела запроса емейл и пароль
    /// отправляет на введенный емейл письио об успешном входе
    /// </summary>
    /// <param name="email">Емейл пользователя</param>
    /// <param name="password">Пароль пользователя</param>
    [Post("login")]
    public void SendEmailLogin(string email, string password)
    {
       //var emailService = new EmailService(AppConfig.GetInstance().EmailServiceConfiguration);
        _emailService.SendEmail(email, "ENTER", "Вы вошли в систему!");
    }
    
    // Get страниц

    /// <summary>
    /// Метод обрабатывающий Get запрос по роутингу "anime"
    /// Получает и обрабатывает параметры из строки запроса
    /// Вызывает отправку html страницы аниме сайта
    /// </summary>
    /// <param name="h1">h1 параметр создан в тестовых целях</param>
    /// <param name="h2">h2 параметр создан в тестовых целях</param>
    /// <returns>объект <see cref="IHttpResponseResult"/> html страницу сайта с аниме</returns>
    [Get("anime")]
    public IHttpResponseResult GetAnimePage(string h1, string h2)
    {
        Console.WriteLine("GET: anime");
        Console.WriteLine(h1 + h2);
        var localPath = "anime/index.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        return Html(responseText);
    }

    /// <summary>
    /// Метод обработки Get запроса по роутингу "login"
    /// Вызывает отправку html страницы авторизации
    /// </summary>
    /// <returns>объект <see cref="IHttpResponseResult"/> html страницу авторизации</returns>
    [Get("login")]
    public IHttpResponseResult GetLoginPage()
    {
        Console.WriteLine("GET: login");
        var localPath = "login.html";
        var responseText = ResponseHelper.GetResponseText(localPath);
        return Html(responseText);
    }
}
