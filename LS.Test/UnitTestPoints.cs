using FluentValidation.Results;
using LS.API.Controllers;
using LS.API.FluentValidations;
using LS.API.Models;
using LS.Application.Commands;
using LS.Application.Exceptions;
using LS.Application.Queries;
using LS.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System;

namespace LS.Test
{
    public class UnitTestPoints
    {
        [Fact]
        public async void EarnPoints_ShouldReturnOk_WhenUserExists()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var userId = 1;
            var points = 10;
            var request = new EarnPointsRequest { Points = points };

            // Mock Mediator to return true (success)
            mockMediator
                .Setup(m => m.Send(It.Is<EarnPointsCommand>(cmd => cmd.UserId == userId && cmd.Points == points), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var controller = new PointsController(mockMediator.Object);

            // Act
            var result = await controller.EarnPoints(userId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            var response = Assert.IsType<Dictionary<string, object>>(
                okResult.Value?.GetType()
                    .GetProperties()
                    .ToDictionary(p => p.Name, p => p.GetValue(okResult.Value)));

            Assert.Equal(userId, response["userId"]);
            Assert.Equal(points, response["points"]);
        }

        [Fact]
        public async Task EarnPoints_ShouldReturnError_WhenUserNotExists()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var userId = 999;
            var points = 10;

            // Simulate a failure (e.g. user not found)
            mockMediator
                .Setup(m => m.Send(It.IsAny<EarnPointsCommand>(), default))
                .ThrowsAsync(new UserNotFoundException(userId));

            var controller = new PointsController(mockMediator.Object);
            var request = new EarnPointsRequest { Points = points };

            // Act
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
                controller.EarnPoints(userId, request));

            // Assert
            Assert.Equal($"User with ID {userId} was not found.", exception.Message);
        }

        [Fact]
        public async Task EarnPoints_ShouldReturnError_WhenPointIs0()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var controller = new PointsController(mockMediator.Object);

            int userId = 1;
            var request = new EarnPointsRequest { Points = 0 };

            // Manually validate the request (since model validation doesn't run in unit tests)
            var validator = new EarnPointsRequestValidator();
            var validationResult = validator.Validate(request);

            foreach (var error in validationResult.Errors)
            {
                controller.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            // Act
            var result = await controller.EarnPoints(userId, request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            var response = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(response.ContainsKey("Points"));
        }

        [Fact]
        public async Task GetUserPoints_ShouldReturnPoints_WhenUserExists()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var userId = 1;
            var expectedPoints = 150;

            mockMediator
                .Setup(m => m.Send(It.Is<GetUserTotalPointsQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedPoints);

            var controller = new PointsController(mockMediator.Object);

            // Act
            var result = await controller.GetUserPoints(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedPoints, okResult.Value);
        }

        [Fact]
        public async Task GetUserPoints_ShouldThrowUserNotFoundException_WhenUserDoesNotExist()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var userId = 999;

            mockMediator
                .Setup(m => m.Send(It.Is<GetUserTotalPointsQuery>(q => q.UserId == userId), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UserNotFoundException(userId));

            var controller = new PointsController(mockMediator.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UserNotFoundException>(() =>
                controller.GetUserPoints(userId));

            Assert.Equal($"User with ID {userId} was not found.", exception.Message);
        }

    }
}