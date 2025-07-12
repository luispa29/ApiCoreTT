using ApiCore.Controllers;
using Azure;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Request;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApiCore.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task CreateUser_ReturnsOk_WhenUserCreatedSuccessfully()
        {
            // Arrange
            var userRequest = new UserRequest { Email = "test@test.com", Password = "123456" };
            _mockUserService.Setup(s => s.CreateUser(userRequest)).ReturnsAsync(true);

            // Act
            var result = await _controller.CreateUser(userRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

        
            var json = JsonSerializer.Serialize(result.Value);
            var jsonDoc = JsonDocument.Parse(json);
            var response = jsonDoc.RootElement;
            Assert.True(response.GetProperty("status").GetBoolean());
            Assert.Equal("Usuario creado con éxito", response.GetProperty("message").GetString());
        }

        [Fact]
        public async Task CreateUser_ReturnsStatus200_WhenEmailExists()
        {
            // Arrange
            var userRequest = new UserRequest { Email = "duplicate@test.com", Password = "123456" };
            _mockUserService.Setup(s => s.CreateUser(userRequest)).ReturnsAsync(false);

            // Act
            var result = await _controller.CreateUser(userRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            var json = JsonSerializer.Serialize(result.Value);
            var jsonDoc = JsonDocument.Parse(json);
            var response = jsonDoc.RootElement;
            Assert.False(response.GetProperty("status").GetBoolean());
            Assert.Equal("El email ya se encuentra registrado", response.GetProperty("message").GetString());
        }

        [Fact]
        public async Task CreateUser_Returns500_OnException()
        {
            // Arrange
            var userRequest = new UserRequest();
            _mockUserService.Setup(s => s.CreateUser(userRequest)).ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _controller.CreateUser(userRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenLoginSuccessful()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "test@test.com", Password = "123456" };
            var token = "mock-token";
            _mockUserService.Setup(s => s.Login(loginRequest)).ReturnsAsync(token);

            // Act
            var result = await _controller.Login(loginRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            var json = JsonSerializer.Serialize(result.Value);
            var jsonDoc = JsonDocument.Parse(json);
            var response = jsonDoc.RootElement;
            Assert.True(response.GetProperty("status").GetBoolean());
            Assert.Equal(token, response.GetProperty("data").GetString());
        }

        [Fact]
        public async Task Login_ReturnsStatus200_WhenCredentialsInvalid()
        {
            // Arrange
            var loginRequest = new LoginRequest { Email = "wrong@test.com", Password = "wrong" };
            _mockUserService.Setup(s => s.Login(loginRequest)).ReturnsAsync(string.Empty);

            // Act
            var result = await _controller.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);

            var json = JsonSerializer.Serialize(result.Value);
            var jsonDoc = JsonDocument.Parse(json);
            var response = jsonDoc.RootElement;
            Assert.False(response.GetProperty("status").GetBoolean());
            Assert.Equal("Credenciales incorrectas", response.GetProperty("message").GetString());
        }

        [Fact]
        public async Task Login_Returns500_OnException()
        {
            // Arrange
            var loginRequest = new LoginRequest();
            _mockUserService.Setup(s => s.Login(loginRequest)).ThrowsAsync(new Exception("Auth error"));

            // Act
            var result = await _controller.Login(loginRequest) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, result.StatusCode);

        }
    }
}
