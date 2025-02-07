

using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;
using Management.Core.Models;
using Management.Core.Models.DepartmentModels;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Command;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;
using Management.Application.Services.DepartmentMediatorService;

using Moq;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Management.Application.IntegrationTests.Service.DepartmentCommandHandlerTests;

public class DeleteDepartmentCommandTests
{
    [Fact]
    public async Task DeleteDepartmentCommand_ShouldReturnDepartmentCommandResponse_WhenDeleteIsSuccessful()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var department = new DepartmentMod { Id = 1, Name = "Department 01" };

        mockRepo.Setup(repo => repo.DeleteDepartmentAsyncRep(1))
            .ReturnsAsync(new DeleteResult<DepartmentMod> {
                Data = department, Success = true
            });
        var service = new DepartmentCommandHandler(mockRepo.Object);

        // Act
        var result = await service.Handle(new DeleteDepartmentCommand(1), CancellationToken.None);
    
        // Assert
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task DeleteDepartmentCommand_ShouldReturnErrorMessage_WhenIdIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var service = new DepartmentCommandHandler(mockRepo.Object);

        // Act
        var result = await service.Handle(new DeleteDepartmentCommand(-1), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Zu kleine Id beim deleten", result.ErrorMessage);
    }

    [Fact]
    public async Task DeleteDepartmentCommand_ShouldReturnErrorMessage_WhenDeleteFails()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        mockRepo.Setup(repo => repo.DeleteDepartmentAsyncRep(1))
            .ReturnsAsync(new DeleteResult<DepartmentMod> {
                Data = null, Success = false
            });

        var service = new DepartmentCommandHandler(mockRepo.Object);

        // Act
        var result = await service.Handle(new DeleteDepartmentCommand(1), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Fehler beim Löschen des Departments.", result.ErrorMessage);
    }

}