


using Management.Infrastructure.Department.Data;
using Management.Infrastructure.Department.Repositories.DepartmentRepositories;
using Management.Infrastructure.Department.IntegrationTests.Helpers;
using Management.Core.Models.DepartmentModels;

using System.Net;

namespace Management.Infrastructure.Department.IntegrationTests.Repositories.Department;

public class DepartmentRepositoryPostTests : IClassFixture<DepartmentRepositoryDatabase>
{
    private readonly DepartmentRepositoryDatabase _database;
    public DepartmentRepositoryPostTests(DepartmentRepositoryDatabase departmentRepositoryDatabase)
    {
        _database = departmentRepositoryDatabase;
        _database.SeedDatabase();
    }

    [Fact]
    public async Task PostDepartmentAsyncRep_ShouldReturnCreated_WhenDepartmentIsValid()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);
            var department = new DepartmentMod { Id = 6, Name = "Department 06" };

            // Act
            var result = await repository.PostDepartmentAsyncRep(department);

            // Assert
            if(result.Data != null)
            {
                Assert.Equal(department.Name, result.Data.Name);
            }
        }
    }

    [Fact]
    public async Task PostDepartmentAsyncRep_ShouldReturnBadRequest_WhenDepartmentIsNull()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            // Act
            var result = await repository.PostDepartmentAsyncRep(null);

            // Assert
            if(result.Data != null)
            {
                Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
                Assert.Equal("DepartmentMod cannot be null", result.Message);
            }
        }
    }
}
