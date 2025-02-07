



using Management.Core.Models.DepartmentModels.DepartmentMediator.Command;
using Management.Core.Models.DepartmentModels.DepartmentMediator.Responses;

using Moq;  // Mock
using MediatR;  // IMediator
using Microsoft.AspNetCore.Mvc.Testing; // WebApplicationFactory
using System.Net.Http.Json; // ReadFromJsonAsync
using System.Text;  // Encoding.UTF8
using Xunit.Abstractions; //    ITestOutputHelper
using Microsoft.Extensions.DependencyInjection; //  AddSingleton

namespace Management.Api.IntegrationTests.Groups;

public class GroupDepartmentPostTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ITestOutputHelper _output;

    public GroupDepartmentPostTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
        _output = output;
    }

    [Fact]
    public async Task PostDepartment_ReturnsOkResult_WhenDepartmentPost()
    {
        // Arrange
        var departmentCommandResponse = new DepartmentCommandResponse { Id = 1, Name = "Test Department 01" };
        _mediatorMock.Setup(m => m.Send(It.IsAny<PostDepartmentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(departmentCommandResponse);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var departmentName = "Test Department 01";
        var content = new StringContent($"{{ \"Name\": \"{departmentName}\" }}", Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/v1/department/postdepartment/" + departmentName, content);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine("---------------------------------");
        _output.WriteLine($"--> Raw Response Content: {responseContent}");
        _output.WriteLine($"--> Response Headers: {response.Headers}");

        // Assert
        var result = await response.Content.ReadFromJsonAsync<DepartmentCommandResponse>();

        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Department 01", result.Name);
    }

    [Fact]
    public async Task PostDepartment_ReturnsNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<PostDepartmentCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((DepartmentCommandResponse)null!); // Null indicates not found

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var departmentName = "NonExistentDepartment";
        var content = new StringContent($"{{ \"Name\": \"{departmentName}\" }}", Encoding.UTF8, "application/json");
        var response = await client.PostAsync("/v1/department/postdepartment/" + departmentName, content);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine("---------------------------------");
        _output.WriteLine($"--> Raw Response Content: {responseContent}");
        _output.WriteLine($"--> Response Headers: {response.Headers}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

}