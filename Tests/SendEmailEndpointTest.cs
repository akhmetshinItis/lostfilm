using HttpServerLibrary.Core.HttpResponse;
using HttpServerLibrary.Models;
using Moq;
using my_http;
using my_http.Endpoints;


namespace Tests;
public class Tests
{
    private SendEmailEndpoint _endpoint;
    [SetUp]
    public void Setup()
    {
        _endpoint = new SendEmailEndpoint();
    }

    [Test]
    public void SendEmailAnimeShouldReturnJsonResult()
    {
        var response = _endpoint.SendEmailAnime("testName", "testEmail");
        Assert.AreEqual(response.GetType(), typeof(JsonResult));
    }

    [Test]
    public void SendLoginShouldInvokeSendOnlyOnce()
    {
        var mock = new Mock<EmailService>(AppConfig.GetInstance().EmailServiceConfiguration);
        var email = "test";
        var password = "xxx";
        _endpoint._emailService = mock.Object;
        _endpoint.SendEmailLogin(email, password);
        mock.Verify(es => es.SendEmail(email, "ENTER", "Вы вошли в систему!"), Times.Once);
    }

    [Test]
    public void GetAnimePageShouldSendCorrectHtmlResult()
    {
        var mock = new Mock<SendEmailEndpoint>();
        mock.Setup(c => c.ResponseHelper.GetResponseText("anime/index.html")).Returns("<html>Test text</html>");
        var response = (HtmlResult)mock.Object.GetAnimePage("hello", "hello2");
        Assert.AreEqual(response.GetHtml(), "<html>Test text</html>");
    }

    [Test]
    public void GetLoginPageShouldSendCorrectHtmlResult()
    {
        var mock = new Mock<SendEmailEndpoint>();
        mock.Setup(c => c.ResponseHelper.GetResponseText("login.html")).Returns("<html>Test text</html>");
        var response = (HtmlResult)mock.Object.GetLoginPage();
        Assert.AreEqual(response.GetHtml(), "<html>Test text</html>");
    }

    // [Test]
    // public void GetResponseTextShouldReturnCorrectTextIfFileExits()
    // {
    //     var filePath = "/Users/tagirahmetsin/2nd course/oris/oris 05.11/MyWebServer/Tests/Test.txt";
    //     Assert.IsTrue(File.Exists(filePath));
    //     Assert.AreEqual(File.ReadAllText(filePath), _endpoint.GetResponseText(filePath));
    // }
    
    [Test]
    public void GetResponseTextShouldReturn404htmlFileIfTextFileNotExists()
    {
        var filePath = "";
        Assert.IsFalse(File.Exists(filePath));
        Assert.That(_endpoint.ResponseHelper.GetResponseText(filePath), Is.EqualTo(File.ReadAllText("/Users/tagirahmetsin/2nd course/oris/oris 05.11/MyWebServer/MyWebServer/public/404.html")));
    }
    
}