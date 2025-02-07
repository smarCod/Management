

using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;
using Management.Core.Models;
using Management.Core.Models.DepartmentModels;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Command;

using Moq;

namespace Management.Application.IntegrationTests.Service.DepartmentCommandHandlerTests;

public class PostDepartmentCommandTests
{
    [Fact]
    public async Task PostDepartmentCommand_ShouldReturnDepartmentCommandResponse_WhenDepartmentNameIsValid()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        mockRepo.Setup(repo => repo.PostDepartmentAsyncRep(It.IsAny<DepartmentMod>()))
            .ReturnsAsync(new PostResult<DepartmentMod>
            {
                Data = new DepartmentMod { Id = 1, Name = "Valid Department" },
                Success = true
            });

        var service = new DepartmentCommandHandler(mockRepo.Object);

        // Act
        var result = await service.Handle(new PostDepartmentCommand("Valid Department"), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Valid Department", result.Name);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public async Task PostDepartmentCommand_ShouldReturnErrorMessage_WhenDepartmentNameIsEmpty()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var service = new DepartmentCommandHandler(mockRepo.Object);

        // Act
        var result = await service.Handle(new PostDepartmentCommand(""), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Leerer Departmentname", result.ErrorMessage);
    }
}