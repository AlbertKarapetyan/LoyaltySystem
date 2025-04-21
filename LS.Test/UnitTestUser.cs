using LS.API.Controllers;
using LS.API.Models;
using LS.Application.Commands;
using LS.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LS.Test
{
    public class UnitTestUser
    {
        [Fact]
        public async Task CreateUser_ShouldReturnOk_WhenUserIsCreated()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var userName = "Some User";
            var request = new CreateUserRequest { Name = userName };

            // Fake user object returned by the command handler
            var expectedUser = new UserDto
            {
                Id = 1,
                Name = userName,
                TotalPoints = 0
            };

            mockMediator.Setup(m => m.Send(
                    It.Is<CreateUserCommand>(cmd => cmd.Name == userName),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedUser);

            var controller = new UserController(mockMediator.Object);

            // Act
            var result = await controller.CreateUser(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, object>>(
                okResult.Value?.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(okResult.Value)));

            Assert.Equal("User created successfully", response["message"]);

            var userDto = Assert.IsType<UserDto>(response["user"]);
            Assert.Equal(expectedUser.Id, userDto.Id);
            Assert.Equal(expectedUser.Name, userDto.Name);
            Assert.Equal(expectedUser.TotalPoints, userDto.TotalPoints);
        }

    }
}
