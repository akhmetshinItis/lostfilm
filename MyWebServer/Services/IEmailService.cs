namespace my_http
{
    /// <summary>
    /// Интерфейс сервиса по отправке писем на почту
    /// </summary>
    internal interface IEmailService
    {
        /// <summary>
        /// Метод для отправки письма
        /// </summary>
        /// <param name="email">Емейл получателя</param>
        /// <param name="title">Тема письма</param>
        /// <param name="message">Сообщение письма</param>
        internal void SendEmail(string email, string title, string message);
    }
}
