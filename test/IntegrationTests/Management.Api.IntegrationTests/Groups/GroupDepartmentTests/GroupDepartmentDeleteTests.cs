


using Management.Core.Models.DepartmentModels.DepartmentMediator.Command;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;

using Moq;  // Mock
using MediatR;  // IMediator
using Microsoft.AspNetCore.Mvc.Testing; // WebApplicationFactory
using System.Net.Http.Json; // ReadFromJsonAsync
using System.Text;  // Encoding.UTF8
using Xunit.Abstractions; //    ITestOutputHelper
using Microsoft.Extensions.DependencyInjection; //  AddSingleton

namespace Management.Api.IntegrationTests.Groups.GroupDepartmentDeleteTests;

public class GroupDepartmentDeleteTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ITestOutputHelper _output;

    public GroupDepartmentDeleteTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
        _output = output;
    }

    [Fact]
    public async Task DeleteDepartment_ReturnsOkResult_WhenDepartmentDeleted()
    {
        // Arrange
        var departmentCommandResponse = new DepartmentCommandResponse { Id = 1, Name = "Deleted Department" };

        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteDepartmentCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(departmentCommandResponse);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var departmentId = 1;
        var response = await client.DeleteAsync($"/v1/department/deletedepartment/{departmentId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine("---------------------------------");
        _output.WriteLine($"--> Raw Response Content: {responseContent}");
        _output.WriteLine($"--> Response Headers: {response.Headers}");

        var result = await response.Content.ReadFromJsonAsync<DepartmentCommandResponse>();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Deleted Department", result.Name);
    }

    [Fact]
    public async Task DeleteDepartment_ReturnsNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteDepartmentCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((DepartmentCommandResponse)null!); // Null indicates not found

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var departmentId = 999; // Assuming this ID does not exist
        var response = await client.DeleteAsync($"/v1/department/deletedepartment/{departmentId}");

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine("---------------------------------");
        _output.WriteLine($"--> Raw Response Content: {responseContent}");
        _output.WriteLine($"--> Response Headers: {response.Headers}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}
