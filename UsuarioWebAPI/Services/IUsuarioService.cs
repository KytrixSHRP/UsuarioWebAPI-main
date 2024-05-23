using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuarioWebAPI.Models;

namespace UsuarioWebAPI.Services
{
    public interface IUsuarioService
    {
        public Task<bool> Cadastrar(CadastroRequest request);
        public Task<Usuario> Logar(LoginForm login);

        public Task<Usuario> BuscarPerfis(Usuario usuario);
        public Task<Usuario> EncontrarUsuarioParaReset(ResetRequest resetRequest);
        public Task EnviarRequisicaoReset(Usuario usuarioExistente, string token);
        public Task<bool> AlterarSenha(NovaSenhaForm novaSenhaForm);
        Task AtualizarPerfis(string cpf);
    }
}