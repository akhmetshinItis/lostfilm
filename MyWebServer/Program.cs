﻿using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using HttpServerLibrary;
using HttpServerLibrary.Models;

namespace my_http;
class Program
{
    //TODO перенести настройки в json
   /// <summary>
   /// Точка входа в приложение
   /// </summary>
   /// <param name="args">Аргументы при запуске приложения</param>
    static void Main(string[] args)
    {
        
        var config = AppConfig.GetInstance(); // инициализируем AppConfig
        var prefixes = new[] { $"http://{config.Domain}:{config.Port}/" }; // Собираем префиксы в список
        var server = new HttpServerWithTypes(prefixes, config.StaticDirectoryPath); // создаем сервер с префиксами
        server.Start(); // запускаем сервер
    }

}

