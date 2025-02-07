


using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;
using Management.Core.Models;
using Management.Core.Models.DepartmentModels;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Command;

using Moq;
using Microsoft.Extensions.Logging;

namespace Management.Application.IntegrationTests.Service.DepartmentCommandHandlerTests;

public class UpdateDepartmentCommandTests
{
    [Fact]
    public async Task UpdateDepartmentCommand_ShouldReturnErrorMessage_WhenIdIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var mockLogger = new Mock<ILogger<DepartmentCommandHandler>>();
        // var service = new DepartmentCommandHandler(mockRepo.Object, mockLogger.Object);
        var service = new DepartmentCommandHandler(mockRepo.Object);

        // Act
        var result = await service.Handle(new UpdateDepartmentCommand(-1, "Valid Name"), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Ungültige ID", result.ErrorMessage);
    }

    [Fact]
    public async Task UpdateDepartmentCommand_ShouldReturnErrorMessage_WhenNameIsEmpty()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var mockLogger = new Mock<ILogger<DepartmentCommandHandler>>();
        var service = new DepartmentCommandHandler(mockRepo.Object);

        // Act
        var result = await service.Handle(new UpdateDepartmentCommand(1, ""), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Leerer Departmentname", result.ErrorMessage);
    }

    [Fact]
    public async Task UpdateDepartmentCommand_ShouldReturnDepartmentCommandResponse_WhenUpdateIsSuccessful()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var department = new DepartmentMod { Id = 1, Name = "Old Name" };

        mockRepo.Setup(repo => repo.GetDepartmentByIdAsyncRep(1))
                .ReturnsAsync(new GetOneResult<DepartmentMod> { Data = department, Success = true });

        var mockLogger = new Mock<ILogger<DepartmentCommandHandler>>();
        var service = new DepartmentCommandHandler(mockRepo.Object);

        // Act
        var result = await service.Handle(new UpdateDepartmentCommand(1, "Neuer Name"), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Neuer Name", result.Name);
        Assert.Null(result.ErrorMessage);
    }
}




