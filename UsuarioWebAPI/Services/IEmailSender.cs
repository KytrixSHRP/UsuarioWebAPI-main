using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsuarioWebAPI.Services
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(string remetente, string assunto, string mensagem);
    }
}