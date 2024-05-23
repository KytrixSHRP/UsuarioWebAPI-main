using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuarioWebAPI.Models;

namespace UsuarioWebAPI.Repository
{
    public interface IUsuarioDatabase
    {
        public Task<Usuario> EncontrarUsuario(LoginForm login);
        public Task<bool> ValidarCadastro (CadastroRequest request);
        public Task <List<Perfil>> EncontrarPerfis(int usuarioId);
        public Task<int> EncontrarUsuarioCadastrado(string cpf);
        public Task AtualizarPerfis(int usuarioId);
        public Task<Usuario> EncontrarUsuarioParaReset(ResetRequest resetRequest);

        public Task<bool> InserirToken(string token, Usuario usuario);

        public Task<string> EncontrarToken(string token);

        public Task<string> EncontrarTokenCpf(string cpf, string token);

        public Task<bool> AlterarSenha(string cpf, string senha);
        

    }
}