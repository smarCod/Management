


using Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;
using Management.Api.Groups;

using Moq;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit.Abstractions; //    ITestOutputHelper
using Microsoft.Extensions.DependencyInjection; //  AddSingleton

namespace Management.Api.IntegrationTests.Groups.GroupDepartmentTests;

public class GroupDepartmentGetByIdTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ITestOutputHelper _output;

    public GroupDepartmentGetByIdTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
        _output = output;
    }

    [Fact]
    public async Task GetDepartmentById_ReturnsOkResult_WhenDepartmentExists()
    {
        // Arrange
        var departmentId = 1;
        var department = new DepartmentQueryResponse { Id = departmentId, Name = "Test Department" };
        // var department = new DepartmentQueryResponse(1, "Test Department");

        // It.IsAny<GetDepartmentByIdQuery>(): Akzeptiert jede Query
        // It.IsAny<CancellationToken>(): Akzeptiert jeden Cancel-Token
        // ReturnsAsync(department): Gibt unser Test-Department zurück
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetDepartmentByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(department);

        // var client = _factory.CreateClient();        
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync($"/v1/department/getdepartmentbyid/{departmentId}");
        
        var content = await response.Content.ReadAsStringAsync();
        _output.WriteLine($"--> Raw Response Content: {content}");
        _output.WriteLine($"--> Response Headers: {response.Headers}");
        
        // Assert
        // EnsureSuccessStatusCode(): Wirft eine Exception bei Fehlern
        // ReadFromJsonAsync: Wandelt die JSON-Antwort in ein C#-Objekt um
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<DepartmentQueryResponse>();

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(departmentId, result.Id);
        Assert.Equal("Test Department", result.Name);
    }

    [Fact]
    public async Task GetDepartmentById_ReturnsNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        var departmentId = 999; // Nicht existierende ID
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetDepartmentByIdQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync((DepartmentQueryResponse)null!);

        // var client = _factory.CreateClient();
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync($"/v1/department/getbyid/{departmentId}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }
}

