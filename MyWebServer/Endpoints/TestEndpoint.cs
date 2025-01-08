using System.Net;
using HttpServerLibrary;
using HttpServerLibrary.Core.Attributes;

namespace my_http.Endpoints;
/// <summary>
/// Тестовый Enpoint использовался для проверки возможности получения параметров из строки запроса
/// </summary>
internal class TestEndpoint : EndpointBase
{
    /// <summary>
    /// Метод обработки Get запроса по роутингу "wow"
    /// Выводит в консоль строку полученную и строки запроса
    /// </summary>
    /// <param name="hello">Строка для вывода в консоль</param>
    [Get("wow")]
    public void GetWow(string hello)
    {
        Console.WriteLine(hello);
    }
}