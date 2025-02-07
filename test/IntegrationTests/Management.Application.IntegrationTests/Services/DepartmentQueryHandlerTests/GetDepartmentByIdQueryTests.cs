


using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;
using Management.Core.Models;
using Management.Core.Models.DepartmentModels;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;
using Management.Application.Services.DepartmentMediatorService;

using Moq;
using System.Net;
using Microsoft.Extensions.Logging;

namespace Management.Application.IntegrationTests.Service.DepartmentQueryHandlerTests;

public class GetDepartmentByIdQueryTests
{
[Fact]
    public async Task GetDepartmentByIdQuery_ShouldReturnDepartmentQueryResponse_WhenDepartmentExist()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var mockLogger = new Mock<ILogger<DepartmentQueryHandler>>();
        var department = new DepartmentMod { Id = 1, Name = "Department 01" };

        mockRepo.Setup(repo => repo.GetDepartmentByIdAsyncRep(1))
                .ReturnsAsync(new GetOneResult<DepartmentMod> { Data = department, Success = true });

        var service = new DepartmentQueryHandler(mockRepo.Object, mockLogger.Object);

        // Act
        var query = new GetDepartmentByIdQuery(1);
        var result = await service.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Department 01", result.Name);
        Assert.Equal("", result.ErrorMessage);
    }

    [Fact]
    public async Task GetDepartmentByIdQuery_ShouldReturnErrorMessage_WhenDepartmentDoesNotExist()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var mockLogger = new Mock<ILogger<DepartmentQueryHandler>>();

        mockRepo.Setup(repo => repo.GetDepartmentByIdAsyncRep(It.IsAny<int>()))
                .ReturnsAsync(new GetOneResult<DepartmentMod> { Data = null, Success = false });

        var service = new DepartmentQueryHandler(mockRepo.Object, mockLogger.Object);

        // Act
        var query = new GetDepartmentByIdQuery(999);
        var result = await service.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.Id); // Hier wird der Standardwert von 0 verwendet
        Assert.Equal("", result.Name); // Hier wird der Standardwert von "" verwendet
        Assert.Equal("Department nicht gefunden.", result.ErrorMessage);
    }
}
