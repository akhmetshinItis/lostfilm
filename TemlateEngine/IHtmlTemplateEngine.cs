namespace TemlateEngine;

public interface IHtmlTemplateEngine
{
    public string Render(string template, string data);
    public string Render(string template, object obj);
}