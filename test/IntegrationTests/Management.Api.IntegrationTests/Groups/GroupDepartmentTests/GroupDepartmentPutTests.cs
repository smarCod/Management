

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

public class GroupDepartmentPutTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ITestOutputHelper _output;

    public GroupDepartmentPutTests(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
        _output = output;
    }
    [Fact]
    public async Task UpdateDepartment_ReturnsOkResult_WhenDepartmentUpdate()
    {
        // Arrange
        var departmentCommandResponse = new DepartmentCommandResponse { Id = 1, Name = "Test Department 01 Update" };

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateDepartmentCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(departmentCommandResponse);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var departmentId = 1;
        var departmentName = "Test Department 01 Update";
        var content = new StringContent($"{{ \"Name\": \"{departmentName}\" }}", Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"/v1/department/updatedepartment/{departmentId}/{departmentName}", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine("---------------------------------");
        _output.WriteLine($"--> Raw Response Content: {responseContent}");
        _output.WriteLine($"--> Response Headers: {response.Headers}");

        var result = await response.Content.ReadFromJsonAsync<DepartmentCommandResponse>();
        // Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test Department 01 Update", result.Name);
    }

    [Fact]
    public async Task UpdateDepartment_ReturnsBadRequest_WhenDepartmentIsInvalid()
    {
        // Arrange
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateDepartmentCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((DepartmentCommandResponse)null!); // Null indicates invalid request

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var departmentId = 1; // Assuming this ID is invalid or not found
        var departmentName = "InvalidDepartmentName";
        var content = new StringContent($"{{ \"Name\": \"{departmentName}\" }}", Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"/v1/department/updatedepartment/{departmentId}/{departmentName}", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine("---------------------------------");
        _output.WriteLine($"--> Raw Response Content: {responseContent}");
        _output.WriteLine($"--> Response Headers: {response.Headers}");

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        // Assert.Contains("Expected error message", response.ErrorMessage); // Check for specific error message
    }

    [Fact]
    public async Task UpdateDepartment_ReturnsErrorMessageVomHandle_WhenDepartmentIsInvalid()
    {
        // Arrange
        var errorResponse = new DepartmentCommandResponse(ErrorMessage: "Department could not be updated");
        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateDepartmentCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(errorResponse);

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Act
        var departmentId = 1; // Assuming this ID is invalid or not found
        var departmentName = "InvalidDepartmentName";
        var content = new StringContent($"{{ \"Name\": \"{departmentName}\" }}", Encoding.UTF8, "application/json");
        var response = await client.PutAsync($"/v1/department/updatedepartment/{departmentId}/{departmentName}", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        _output.WriteLine("---------------------------------");
        _output.WriteLine($"--> Raw Response Content: {responseContent}");
        _output.WriteLine($"--> Response Headers: {response.Headers}");

        // Assert
        // Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        
        var result = await response.Content.ReadFromJsonAsync<DepartmentCommandResponse>();
        Assert.NotNull(result);
        Assert.Equal("Department could not be updated", result.ErrorMessage);
    }
}
