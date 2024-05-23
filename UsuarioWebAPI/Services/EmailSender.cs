using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UsuarioWebAPI.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string destinatario, string assunto, string mensagem)
        {
            var remetente = "healthmeconsultas@outlook.com";
            var password = "HealthMe@159";

            var client = new SmtpClient("smtp-mail.outlook.com", 587){
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(remetente, password),
                EnableSsl = true
            };

            return client.SendMailAsync(new MailMessage(
                from: remetente,
                to: destinatario,
                assunto,
                mensagem
            ));

        }
    }
}