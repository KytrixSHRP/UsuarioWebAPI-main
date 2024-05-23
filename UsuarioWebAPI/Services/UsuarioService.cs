using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsuarioWebAPI.Repository;
using UsuarioWebAPI.Models;
using System.Text.RegularExpressions;

namespace UsuarioWebAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ILogger<UsuarioService> _logger;
        private readonly IUsuarioDatabase _usuarioDatabase;
        private readonly IEmailSender _emailSender;
        public UsuarioService(ILogger<UsuarioService> logger, IUsuarioDatabase usuarioDatabase, IEmailSender emailSender)
        {
            _logger = logger;
            _usuarioDatabase = usuarioDatabase;
            _emailSender = emailSender;
        }
        public async Task<bool> Cadastrar(CadastroRequest request)
        {
            var cadastroValido = await _usuarioDatabase.ValidarCadastro(request);
            return cadastroValido;
        }

        public async Task<Usuario> Logar(LoginForm login)
        {
            var usuario = await _usuarioDatabase.EncontrarUsuario(login);

            return usuario;
        }

        public async Task<Usuario> BuscarPerfis(Usuario usuario)
        {
            var perfis = await _usuarioDatabase.EncontrarPerfis(usuario.Id);

            usuario.Perfis.AddRange(perfis);
            return usuario;

        }

        public async Task AtualizarPerfis(string cpf)
        {
            var usuarioId = await _usuarioDatabase.EncontrarUsuarioCadastrado(cpf);
            await _usuarioDatabase.AtualizarPerfis(usuarioId);
        }

        public async Task<Usuario> EncontrarUsuarioParaReset(ResetRequest resetRequest)
        {
            var usuarioExistente = await _usuarioDatabase.EncontrarUsuarioParaReset(resetRequest);
            return usuarioExistente;
        }

        public async Task EnviarRequisicaoReset(Usuario usuarioExistente, string token)
        {
            var destinatario = usuarioExistente.Email;
            var assunto = "Redefinição de Senha";
            var mensagem = $"Accesse o link a seguir para redefinir a senha: http://localhost:5173/Confirmar_Senha?token={token}. O link expirará em 30 minutos";

            await _emailSender.SendEmailAsync(destinatario, assunto, mensagem);
        }

        public async Task<bool> AlterarSenha(NovaSenhaForm novaSenhaForm)
        {
            if(!novaSenhaForm.Senha.Equals(novaSenhaForm.ConfirmarSenha)) return false;
            
            var tokenEmailValido = await _usuarioDatabase.EncontrarTokenCpf(novaSenhaForm.Cpf, novaSenhaForm.Token);

            if(tokenEmailValido is null) return false;

            var senhaAtualizada = await _usuarioDatabase.AlterarSenha(novaSenhaForm.Cpf, novaSenhaForm.Senha);

            return senhaAtualizada;

        }
    }
}