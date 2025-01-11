using HttpServerLibrary.Core.HttpResponse;
using TemlateEngine;

namespace TemplateEngine.UnitTests;

[TestFixture]
public class HtmlTemplateEngineTests
{
    [Test]
    public void Render_ValidTemplateAndData_ReturnHtml()
    {
        // Arrange - подготовительная часть
        IHtmlTemplateEngine engine = new HtmlTemplateEngine();
        var template = "Hello, {{Name}}! How are you?";
        var data = "Bob";
        // Act - действие
        var result = engine.Render(template, data);
        // Assert - проверка
        Assert.AreEqual("Hello, Bob! How are you?", result);
    }

    [Test]
    public void Render_ValidObject_ReturnHtml()
    {
        // Arrange
        Student student = new Student {Id = 1, Name = "Bob"};
        IHtmlTemplateEngine engine = new HtmlTemplateEngine();
        var template = "Ура вы поступили {{Name}}! ваш номер студенческого билета {{Id}}";
        // Act
        var result = engine.Render(template, student);
        //Assert
        Assert.AreEqual("Ура вы поступили Bob! ваш номер студенческого билета 1", result);
    }

    [Test]
    public void Render_ValidObjectWithTrueInIf_ReturnHtml()
    {
        //Arrange
        Student student = new Student{Id = 1, Name = "Bob", Gender = true};
        IHtmlTemplateEngine engine = new HtmlTemplateEngine();
        var template = "{%if<{{Name}}='Bob' && {{Id}}=1>%} Man {%else%}{{Name}} {%/if%}"; // && = AND, || == OR, == is =, != is <>
        //Act
        var result = engine.Render(template: template, student);
        //Assert
        Assert.AreEqual("Man", result);
        
    }
    [Test]
    public void Render_ValidObjectWithFalseInIf_ReturnHtml()
    {
        //Arange
        Student student = new Student{Id = 1, Name = "Bob", Gender = false};
        IHtmlTemplateEngine engine = new HtmlTemplateEngine();
        var template = "{%if<{{Gender}}>%} Man {%else%}{{Name}}{%/if%}";
        //Act
        var result = engine.Render(template: template, student);
        //Assert
        Assert.AreEqual("Bob", result);
    }

    [Test]
    public void Render_ValidObjectWithCollection_ReturnHtml()
    {
        //Arrange
        var group = new Group();
        group.Students = new List<Student>();
        group.Name = "11-308";
        group.Students.Add(new Student{Gender = true, Id = 1, Name = "bob"});
        group.Students.Add(new Student{Gender = false, Id = 2, Name = "ANN"});
        group.Students.Add(new Student{Gender = true, Id = 3, Name = "Gay"});
        var template = "{%for%Student%in%Students}{{Student.Name}} {%/for%}"; //"{%for%(.+?)%in%(.+?)}(.*?){%/for%}"
        var engine = new HtmlTemplateEngine();
        //Act
        var result = engine.Render(template: template, group);
        //Assert
        Assert.AreEqual("bob ANN Gay ", result);
    }


    [Test]
    public void Render_ValidUserWithListOfBooks_ReturnHtml()
    {
        //Arrange
        User user = new User{Id = 1, Name = "Bob", Gender = false, Age = 17};
        user.Books = new List<string> { "book", "boob", "bob" };
        var template = "{%for%Book%in%Books}<li>{{Book}}</li>{%/for%}"; //"{%for%(.+?)%in%(.+?)}(.*?){%/for%}"
        var engine = new HtmlTemplateEngine();
        //Act
        var result = engine.Render(template: template, user);
        //Assert
        Assert.AreEqual("<li>book</li><li>boob</li><li>bob</li>", result);
    }
    
    [Test]
    public void Render_ValidObjectByFilePath_ReturnHtml()
    {
        // Arrange
        User user = new User{Id = 1, Name = "Bob", Gender = false, Age = 17, Books = new List<string> { "book", "boob", "bob" }};
        HtmlTemplateEngine engine = new HtmlTemplateEngine();
        var filePath = new FileInfo(
            "/Users/tagirahmetsin/2nd course/oris/Семетровка орис/Repository/lostfilm/TemplateEngine.UnitTests/Templates/UserProfile_template.html");
        // Act
        var result = engine.Render(filePath, user);
        Console.WriteLine(result);
        var expect =
            File.ReadAllText(
                "/Users/tagirahmetsin/2nd course/oris/Семетровка орис/Repository/lostfilm/TemplateEngine.UnitTests/Templates/Result.html");
        // Assert
        Assert.AreEqual(expect, result);
    }
}