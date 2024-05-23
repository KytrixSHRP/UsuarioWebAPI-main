using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using UsuarioWebAPI.Models;
using UsuarioWebAPI.Repository;

namespace UsuarioWebAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUsuarioDatabase _usuarioDatabase;

        public TokenService(IUsuarioDatabase usuarioDatabase)
        {
            _usuarioDatabase = usuarioDatabase;
        }       
        
        public string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuario.Nome)
            };

            AdicionarRoles(claims, usuario.Perfis);

            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private void AdicionarRoles(List<Claim> claims, List<Perfil> perfis)
        {
            foreach (var perfil in perfis)
            {
                claims.Add(new Claim(ClaimTypes.Role, perfil.Descricao));
            }
        }

        public async Task<string> GerarTokenRecuperacao(Usuario usuario){
            Random res = new Random(); 
  
            // String that contain both alphabets and numbers 
            String str = "abcdefghijklmnopqrstuvwxyz0123456789"; 
            int size = 8; 
        
            // Initializing the empty string 
            String randomstring = ""; 
        
            for (int i = 0; i < size; i++) 
            { 
        
                // Selecting a index randomly 
                int x = res.Next(str.Length); 
        
                // Appending the character at the  
                // index to the random alphanumeric string. 
                randomstring = randomstring + str[x]; 
            }
                
                var tokenInserido = await _usuarioDatabase.InserirToken(randomstring, usuario);

                if(!tokenInserido) return string.Empty;
                
                return randomstring;


        }

        public async Task<bool> ValidarToken(string token)
        {
            var tokenExiste = await _usuarioDatabase.EncontrarToken(token);

            if (tokenExiste is null) return false;

            return true;
        }

        public async Task<bool> ValidarEmailToken(NovaSenhaForm novaSenhaForm)
        {
            var tokenCpfExiste = await _usuarioDatabase.EncontrarTokenCpf(novaSenhaForm.Cpf, novaSenhaForm.Token);

            if (tokenCpfExiste is null) return false;

            return true;
        }
    }
}