

using Management.Core.Models.DepartmentModels.DepartmentMediator.Queries;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;
using Management.Api.Groups;

using Moq;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Xunit.Abstractions; //    ITestOutputHelper
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Management.Api.IntegrationTests.Groups.GroupDepartmentTests;

public class DepartmentGetAllApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ITestOutputHelper _output;

    public DepartmentGetAllApiTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
        _output = output;
    }

    [Fact]
    public async Task GroupDepartmentGetAllTests_ShouldReturnsOkResult_WhenDepartmentsExists()
    {
        // Arrange
        var departments = new List<DepartmentQueryResponse>
        {
            new DepartmentQueryResponse { Id = 1, Name = "HR" },
            new DepartmentQueryResponse { Id = 2, Name = "Finance" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllDepartmentsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(departments);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("/v1/department/getalldepartments/");

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<DepartmentQueryResponse>>();

        // _output.WriteLine("--> HTTP Status Code: " + response.StatusCode);
        // _output.WriteLine("--> HTTP Headers: " + response.Headers);
        // _output.WriteLine("--> Raw Response Content: " + await response.Content.ReadAsStringAsync());
        // _output.WriteLine("--> Parsed Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetAllDepartments_ShouldReturnNotFound_WhenNoDepartmentsExist()
    {
        // Arrange
        var departments = new List<DepartmentQueryResponse>();
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllDepartmentsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<DepartmentQueryResponse>());

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var response = await client.GetAsync("/v1/department/getalldepartments/");

        // var result = await response.Content.ReadFromJsonAsync<DepartmentQueryResponse>();

        // _output.WriteLine("--> HTTP Status Code: " + response.StatusCode);
        // _output.WriteLine("--> HTTP Headers: " + response.Headers);
        // _output.WriteLine("--> Raw Response Content: " + await response.Content.ReadAsStringAsync());
        // _output.WriteLine("--> Parsed Response: " + Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented));
        
        Console.WriteLine($"Response Status Code: {response.StatusCode}"); // Debugging-Ausgabe

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
    }

}