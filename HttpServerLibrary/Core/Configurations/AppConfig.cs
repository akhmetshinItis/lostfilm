using System.ComponentModel.Design;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HttpServerLibrary.Models;

/// <summary>
/// Задает конфигурации для юнитов программы
/// Получает конфигурации из файла и предоставляет к ним доступ
/// Реализует Singleton
/// </summary>
public sealed class AppConfig
{
    /// <summary>
    /// Путь к JSON файлу конфигураций
    /// </summary>
    public static string FILE_NAME = "config.json"; //static
    /// <summary>
    /// Домен на котором работает сервер
    /// </summary>
    public string Domain {get; set; } = "localhost";
    /// <summary>
    /// Порт на котором работает сервер
    /// </summary>
    public string Port { get; set; } = "3232";
    /// <summary>
    /// Путь до статических файлов (контент)
    /// </summary>
    public string StaticDirectoryPath { get; set; } = "public/";
    
    /// <summary>
    /// Настройки EmailService
    /// </summary>
    public EmailServiceConfiguration EmailServiceConfiguration { get; set; } = new ();

    public string ConnectionString { get; set; } = string.Empty;

    
    /// <summary>
    /// Единственный экземпляр класса <see cref="AppConfig"/>
    /// </summary>
    private static AppConfig _instance;
    
    /// <summary>
    /// Приватный конструктор без параметров для реализации Singleton
    /// имеет атрибут [JsonConstructor] для правильной работы <see cref="JsonSerializer"/>
    /// </summary>
    [JsonConstructor]
    private AppConfig(){}
    
    /// <summary>
    /// Проверяет наличие экземпляра класса <see cref="AppConfig"/>
    /// Инициализирует его, если экземпляра раннее не существовало
    /// </summary>
    /// <returns>Instance of <see cref="AppConfig"/></returns>
    public static AppConfig GetInstance()
    {
        if (_instance is null)
        {
            _instance = new AppConfig();
            _instance.Initialize();
        }

        return _instance;
    }

    /// <summary>
    /// Инициализирует instance с данными из файла
    /// если файл не существует выводит сообщение в консоль
    /// </summary>
    private void Initialize()
    {
        if (File.Exists(AppConfig.FILE_NAME))
        {
            var configFile = File.ReadAllText(AppConfig.FILE_NAME);
            _instance = JsonSerializer.Deserialize<AppConfig>(configFile);
        }
        else
        {
            Console.WriteLine($"Файл настроек {AppConfig.FILE_NAME} не найден");
        }
    }
}