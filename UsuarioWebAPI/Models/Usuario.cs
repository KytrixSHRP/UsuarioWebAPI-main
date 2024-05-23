using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsuarioWebAPI.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Email {get; set;}
        public string Numero { get; set; } 
        public DateTime DataNascimento { get; set; }
        public bool Ativo { get; set; }
        public List<Perfil> Perfis { get; set; } = new List<Perfil>();

    }
}