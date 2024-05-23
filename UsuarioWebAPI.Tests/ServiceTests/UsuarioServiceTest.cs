using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UsuarioWebAPI.Controllers;
using UsuarioWebAPI.Models;
using UsuarioWebAPI.Repository;
using UsuarioWebAPI.Services;
using Xunit;

namespace UsuarioWebAPI.Tests.ServiceTests;

public class UsuarioServiceTest
{
    [Fact]
    public async Task CadastrarRetornaTrue()
    {
        //Arrange
        var usuarioDatabaseMock = new Mock<IUsuarioDatabase>();
        var loggerMock = new Mock<ILogger<UsuarioService>>();
        var emailSenderMock = new Mock<IEmailSender>();
		var cadastroRequestMock = new CadastroRequest(){Nome = "Usuario1", Cpf = "12345678900", DataNascimento = new DateTime(1999, 01, 01), Email = "usuario1@gmail.com", Numero = "11999999999", Senha = "senhaUsuario1"};
		var service = new UsuarioService(loggerMock.Object, usuarioDatabaseMock.Object, emailSenderMock.Object);
		usuarioDatabaseMock.Setup(s => s.ValidarCadastro(cadastroRequestMock)).ReturnsAsync(true);
        //Act
        var result = await service.Cadastrar(cadastroRequestMock);

        // Assert
        Assert.True(result);

    }

    [Fact]
    public async Task CadastrarRetornaFalso()
    {
        //Arrange
        var usuarioDatabaseMock = new Mock<IUsuarioDatabase>();
        var loggerMock = new Mock<ILogger<UsuarioService>>();
        var emailSenderMock = new Mock<IEmailSender>();
		var cadastroRequestMock = new CadastroRequest(){Nome = "Usuario1", Cpf = "12345678900", DataNascimento = new DateTime(1999, 01, 01), Email = "usuario1@gmail.com", Numero = "11999999999", Senha = "senhaUsuario1"};
		var service = new UsuarioService(loggerMock.Object, usuarioDatabaseMock.Object, emailSenderMock.Object);
		usuarioDatabaseMock.Setup(s => s.ValidarCadastro(cadastroRequestMock)).ReturnsAsync(false);
        //Act
        var result = await service.Cadastrar(cadastroRequestMock);

        // Assert
        Assert.False(result);
    }
	
	
	[Fact]
	public async Task LoginRetornaUsuario()
    {
        //Arrange
        var usuarioDatabaseMock = new Mock<IUsuarioDatabase>();
        var loggerMock = new Mock<ILogger<UsuarioService>>();
        var emailSenderMock = new Mock<IEmailSender>();
		var loginFormMock = new LoginForm(){CPF="12345678900", Senha="senhaUsuario1"};
		var perfisMock = new List<Perfil>(){ new Perfil() { IdPerfil = 1, IdPaciente = 1, IdMedico = 0, Descricao = "Paciente"}};
		var response = new Usuario(){ 
			Id = 1, 
			Nome = "Usuario1",
			CPF = "12345678900",
			Email = "usuario1@gmail.com",
			Numero = "11999999999",
			DataNascimento = new DateTime(1999, 01, 01),
			Ativo = true,
			Perfis = perfisMock
		};
		var service = new UsuarioService(loggerMock.Object, usuarioDatabaseMock.Object, emailSenderMock.Object);
		usuarioDatabaseMock.Setup(s => s.EncontrarUsuario(loginFormMock)).ReturnsAsync(response);
        //Act
        var result = await service.Logar(loginFormMock);

        // Assert
        Assert.IsType<Usuario>(result);
		Assert.Equal(1, result.Id);

    }

    [Fact]
    public async Task LoginRetornaNulo()
    {
        //Arrange
        var usuarioDatabaseMock = new Mock<IUsuarioDatabase>();
        var loggerMock = new Mock<ILogger<UsuarioService>>();
        var emailSenderMock = new Mock<IEmailSender>();
		var loginFormMock = new LoginForm(){CPF="12345678900", Senha="senhaUsuario1"};
		var perfisMock = new List<Perfil>(){ new Perfil() { IdPerfil = 1, IdPaciente = 1, IdMedico = 0, Descricao = "Paciente"}};
		Usuario response = null;
		var service = new UsuarioService(loggerMock.Object, usuarioDatabaseMock.Object, emailSenderMock.Object);
		usuarioDatabaseMock.Setup(s => s.EncontrarUsuario(loginFormMock)).ReturnsAsync((Usuario) null);
        //Act
        var result = await service.Logar(loginFormMock);

        // Assert
		Assert.Null(result);
    }
	
	[Fact]
	public async Task BuscarPerfisSucesso()
    {
        //Arrange
        var usuarioDatabaseMock = new Mock<IUsuarioDatabase>();
        var loggerMock = new Mock<ILogger<UsuarioService>>();
        var emailSenderMock = new Mock<IEmailSender>();
		var response = new List<Perfil>(){ new Perfil() { IdPerfil = 1, IdPaciente = 1, IdMedico = 0, Descricao = "Paciente"}};
		var usuarioMock = new Usuario(){ 
			Id = 1, 
			Nome = "Usuario1",
			CPF = "12345678900",
			Email = "usuario1@gmail.com",
			Numero = "11999999999",
			DataNascimento = new DateTime(1999, 01, 01),
			Ativo = true
		};
		var service = new UsuarioService(loggerMock.Object, usuarioDatabaseMock.Object, emailSenderMock.Object);
		usuarioDatabaseMock.Setup(s => s.EncontrarPerfis(usuarioMock.Id)).ReturnsAsync(response);
        //Act
        var result = await service.BuscarPerfis(usuarioMock);

        // Assert
        Assert.IsType<Usuario>(result);
		Assert.Equal(result.Id, usuarioMock.Id);
		Assert.Equal(result.Perfis, response);

    }

    [Fact]
    public async Task AtualizarPerfisSucesso()
    {
        //Arrange
        var usuarioDatabaseMock = new Mock<IUsuarioDatabase>();
        var loggerMock = new Mock<ILogger<UsuarioService>>();
        var emailSenderMock = new Mock<IEmailSender>();
		var usuarioIdMock = 1;
		var cpfMock = "11111111111";
		var service = new UsuarioService(loggerMock.Object, usuarioDatabaseMock.Object, emailSenderMock.Object);
		usuarioDatabaseMock.Setup(s => s.EncontrarUsuarioCadastrado(cpfMock)).ReturnsAsync((usuarioIdMock));
        //Act
        await service.AtualizarPerfis(cpfMock);

        // Assert
        usuarioDatabaseMock.Verify(v => v.AtualizarPerfis(usuarioIdMock), Times.Once);
    }
}