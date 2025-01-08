using System.Net.Mail;
using System.Net;
using HttpServerLibrary.Models;

namespace my_http
{
    //был sealed то для тестов убрал 
    //internal
    
    /// <summary>
    /// Класс сервис отправки писем на почту, реализует интерфейс <see cref="IEmailService"/>
    ///
    /// </summary>
    public class EmailService : IEmailService
    {
        
        // TODO: вынести настройки smtp сервера: домен/логин/ пароль  в config.json
        /// <summary>
        /// конфигурации сервиса
        /// </summary>
        private EmailServiceConfiguration _config;
        /// <summary>
        /// вложения при наличии
        /// </summary>
        public Attachment _attachment;
        /// <summary>
        /// инициализирует новый instance сервиса с заданными из конфига настройками
        /// </summary>
        /// <param name="config">Настройки для сервиса</param>
        public EmailService(EmailServiceConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// Отправляет письмо на почту при этом инициализирует новый объект <see cref="SmtpClient"/>
        /// настравивает его из конфига, формирует письмо и отправляет его 
        /// </summary>
        /// <param name="email">Емейл получателя письма</param>
        /// <param name="subject">Тема письма</param>
        /// <param name="message">Сообщение в письме</param>
        //TODO: сделать чтобы метод не был виртуальным
        public virtual void SendEmail(string email, string subject, string message)
        {
            string fromEmail = _config.UserName;

            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress(fromEmail, "Tagir");

            // кому отправляем
            MailAddress to = new MailAddress(email);

            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            if (_attachment is not null)
            {
                m.Attachments.Add(_attachment);
            }
            
            // тема письма
            m.Subject = subject;

            // текст письма
            m.Body = message;

            // письмо представляет код html
            m.IsBodyHtml = true;

            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient();

            //
            smtp.UseDefaultCredentials = false;
            // логин и пароль
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(fromEmail, _config.Password);
            //порт и сервер так как сначала надо аутентификацию заполнить для google и yandex
            smtp.Port = Convert.ToInt32(_config.Port);
            smtp.Host = _config.Host;
            smtp.Send(m);
        }
    }
}
