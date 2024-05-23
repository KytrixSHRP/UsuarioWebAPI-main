using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsuarioWebAPI.Models
{
    public class ResetRequest
    {
        public string Cpf { get; set; }
        public string Email { get; set; }
    }
}