using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UsuarioWebAPI.Controllers;
using UsuarioWebAPI.Models;
using UsuarioWebAPI.Services;
using Xunit;

namespace UsuarioWebAPI.Tests.ControllerTests
{
    public class UsuarioControllerTest
    {
        [Fact]
		public async Task LoginUnauthorized()
		{
			//Arrange
			var usuarioServiceMock = new Mock<IUsuarioService>();
			var tokenServiceMock = new Mock<ITokenService>();
			var loginFormMock = new LoginForm(){CPF="11111111111", Senha="12345"};
			usuarioServiceMock.Setup(s => s.Logar(loginFormMock)).ReturnsAsync((Usuario)null);
			var loggerMock = new Mock<ILogger<UsuarioController>>();
			var controller = new UsuarioController(loggerMock.Object, usuarioServiceMock.Object, tokenServiceMock.Object);

			//Act
			var result = await controller.Login(loginFormMock);

			// Assert
			Assert.IsType<UnauthorizedObjectResult>(result);

		}

		[Fact]
		public async Task LoginOk()
		{
			//Arrange
			var usuarioServiceMock = new Mock<IUsuarioService>();
			var tokenServiceMock = new Mock<ITokenService>();
			var loginFormMock = new LoginForm(){CPF="12345678900", Senha="senhaUsuario1"};
			var loggerMock = new Mock<ILogger<UsuarioController>>();
			var controller = new UsuarioController(loggerMock.Object, usuarioServiceMock.Object, tokenServiceMock.Object);
			
			var usuarioEsperadoMock = new Usuario(){Id = 1, Ativo = true, CPF = "12345678900", DataNascimento = new DateTime(1999, 10, 14), Email = "usuario1@gmail.com", Nome = "Usuario1", Numero = "11111111111", Perfis = new List<Perfil>()};
			usuarioServiceMock.Setup(s => s.Logar(loginFormMock)).ReturnsAsync(usuarioEsperadoMock);

			
			var perfisMock = new List<Perfil>() { new Perfil() {IdPerfil = 1, Descricao = "Paciente", IdMedico = 0, IdPaciente = 1}};
			usuarioEsperadoMock.Perfis.AddRange(perfisMock);
			var usuarioPerfisEsperadoMock =  usuarioEsperadoMock;
		
			string tokenEsperadoMock = "meu_token";

			usuarioServiceMock.Setup(s => s.BuscarPerfis(usuarioEsperadoMock)).ReturnsAsync(usuarioPerfisEsperadoMock);
			tokenServiceMock.Setup(s => s.GerarToken(usuarioPerfisEsperadoMock)).Returns(tokenEsperadoMock);
		

			//Act
			var result = await controller.Login(loginFormMock);
				
			// Assert
			Assert.IsType<OkObjectResult>(result);
		}
			
		[Fact]
        public async Task CadastroOk ()
        {
            
            //Arrange
            var usuarioServiceMock = new Mock<IUsuarioService>();
            var tokenServiceMock = new Mock<ITokenService>();
            var cadastroRequestMock = new CadastroRequest(){Nome = "Usuario1", Cpf = "12345678900", DataNascimento = new DateTime(1999, 01, 01), Email = "usuario1@gmail.com", Numero = "11999999999", Senha = "senhaUsuario1"};
            var loggerMock = new Mock<ILogger<UsuarioController>>();
            var controller = new UsuarioController(loggerMock.Object, usuarioServiceMock.Object, tokenServiceMock.Object);
            usuarioServiceMock.Setup(s => s.Cadastrar(cadastroRequestMock)).ReturnsAsync(true);

            //Act            
            var result = await controller.Cadastro(cadastroRequestMock);
            
            //Assert
            Assert.IsType<OkObjectResult>(result);
			usuarioServiceMock.Verify(v => v.AtualizarPerfis(cadastroRequestMock.Cpf), Times.Once);

        }

        [Fact]  
          public async Task CadastroBadRequest()
        {
            
            //Arrange
            var usuarioServiceMock = new Mock<IUsuarioService>();
            var tokenServiceMock = new Mock<ITokenService>();
            var cadastroRequestMock = new CadastroRequest(){Nome = "Usuario1", Cpf = "12345678900", DataNascimento = new DateTime(1999, 01, 01), Email = "usuario1@gmail.com", Numero = "11999999999", Senha = "senhaUsuario1"};
            var loggerMock = new Mock<ILogger<UsuarioController>>();
            var controller = new UsuarioController(loggerMock.Object, usuarioServiceMock.Object, tokenServiceMock.Object);
            usuarioServiceMock.Setup(s => s.Cadastrar(cadastroRequestMock)).ReturnsAsync(false);

            //Act
            var result = await controller.Cadastro(cadastroRequestMock);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
    }
}