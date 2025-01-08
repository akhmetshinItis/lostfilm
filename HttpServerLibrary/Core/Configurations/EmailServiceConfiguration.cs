namespace HttpServerLibrary.Models;
/// <summary>
/// Класс конфигурации <see cref="EmailService"/>
/// </summary>
public sealed class EmailServiceConfiguration
{
    /// <summary>
    /// Адрес сервера для отправки сообщений
    /// </summary>
    public string Host { get; set; } = "smtp.mail.ru";

    /// <summary>
    /// Порт через который работает <see cref="EmailService"/>
    /// </summary>
    public string Port { get; set; } = "25";
    /// <summary>
    /// Почта с которой отправляются сообщения
    /// </summary>
    public string UserName { get; set; } = "akhmetshin.itis@mail.ru";
    
    /// <summary>
    /// Ключ доступа приложения к почте
    /// </summary>
    public string Password { get; set; } = "eJxT3FbqjXHQ6ZYqqGyf";
}