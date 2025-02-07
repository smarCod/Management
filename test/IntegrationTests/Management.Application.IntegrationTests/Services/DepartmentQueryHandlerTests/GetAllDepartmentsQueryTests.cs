




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
public class GetAllDepartmentsQueryTests
{
    [Fact]
    public async Task GetAllDepartmentsQuery_ShouldReturnDepartmentQueryResponses_WhenDepartmentsExist()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var mockLogger = new Mock<ILogger<DepartmentQueryHandler>>();
        var departments = new List<DepartmentMod>
        {
            new DepartmentMod { Id = 1, Name = "Department 01" },
            new DepartmentMod { Id = 2, Name = "Department 02" }
        };

        mockRepo.Setup(repo => repo.GetAllDepartmentsAsyncRep())
                .ReturnsAsync(new GetAllResult<IList<DepartmentMod>> { Data = departments });

        var service = new DepartmentQueryHandler(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.Handle(new GetAllDepartmentsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Department 01", result[0].Name);
        Assert.Equal("Department 02", result[1].Name);
    }

    [Fact]
    public async Task GetAllDepartmentsQuery_ShouldReturnErrorMessage_WhenNoDepartmentsExist()
    {
        // Arrange
        var mockRepo = new Mock<IDepartmentRepository>();
        var mockLogger = new Mock<ILogger<DepartmentQueryHandler>>();

        mockRepo.Setup(repo => repo.GetAllDepartmentsAsyncRep())
                .ReturnsAsync(new GetAllResult<IList<DepartmentMod>> { Data = null });

        var service = new DepartmentQueryHandler(mockRepo.Object, mockLogger.Object);

        // Act
        var result = await service.Handle(new GetAllDepartmentsQuery(), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Keine Departments gefunden.", result[0].ErrorMessage);
    }
}


// public class GetAllDepartmentsQueryTests
// {

//     [Fact]
//     public async Task GetAllDepartments_ShouldReturnDepartmentQueryResponses_WhenDepartmentsExist()
//     {
//         // Arrange
//         var mockRepo = new Mock<IDepartmentRepository>();
//         var mockLogger = new Mock<ILogger<DepartmentQueryHandler>>();
//         var departments = new List<DepartmentMod>
//         {
//             new DepartmentMod { Id = 1, Name = "Department 01" },
//             new DepartmentMod { Id = 2, Name = "Department 02" }
//         };
//         mockRepo.Setup(repo => repo.GetAllDepartmentsAsyncRep())
//                 .ReturnsAsync(new GetAllResult<IList<DepartmentMod>> { Data = departments });
//         var service = new DepartmentQueryHandler(mockRepo.Object, mockLogger.Object);

//         // Act
//         var result = await service.Handle(new GetAllDepartmentsQuery(), CancellationToken.None);

//         // Assert
//         Assert.NotNull(result);
//         Assert.NotEmpty(result);
//         Assert.Equal("Department 01", result[0].Name);
//         Assert.Equal("Department 02", result[1].Name);
//     }
    
//     [Fact]
//     public async Task GetAllDepartments_ShouldThrowInvalidOperationException_WhenNoDepartmentsExist()
//     {
//         // Arrange
//         var mockRepo = new Mock<IDepartmentRepository>();
//         var mockLogger = new Mock<ILogger<DepartmentQueryHandler>>();
//         mockRepo.Setup(repo => repo.GetAllDepartmentsAsyncRep())
//                 .ReturnsAsync(new GetAllResult<IList<DepartmentMod>> { Data = null });
//         var service = new DepartmentQueryHandler(mockRepo.Object, mockLogger.Object);

//         // Act & Assert
//         var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
//             service.Handle(new GetAllDepartmentsQuery(), CancellationToken.None));
//         Assert.Equal("Keine Departments gefunden.", exception.Message);

//         // mockLogger.Verify(logger => logger.Log(
//         //     It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
//         //     It.IsAny<EventId>(),
//         //     It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No departments found")),
//         //     It.IsAny<Exception>(),
//         //     It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
//     }

// }
