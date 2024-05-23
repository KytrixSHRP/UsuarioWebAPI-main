using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsuarioWebAPI.Models
{
    public class NovaSenhaForm
    {
        public string Cpf { get; set; }
        public string Token { get; set;}
        public string Senha { get; set; }
        public string ConfirmarSenha { get; set; }
    }
}