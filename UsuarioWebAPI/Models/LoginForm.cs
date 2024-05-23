using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsuarioWebAPI.Models
{
    public class LoginForm
    {
        [JsonPropertyName("cpf")]
        public string  CPF{ get; set; }
        [JsonPropertyName("senha")]
        public string Senha { get; set; }

            public LoginForm()
            {
        
            }
    }
}