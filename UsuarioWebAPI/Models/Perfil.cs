using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsuarioWebAPI.Models
{
    public class Perfil
    {
        public int IdPerfil { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public string Descricao { get; set; }
    }
}