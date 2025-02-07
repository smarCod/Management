



using Management.Infrastructure.Department.Data;
using Management.Infrastructure.Department.Repositories.DepartmentRepositories;
using Management.Infrastructure.Department.IntegrationTests.Helpers;
using Management.Core.Models.DepartmentModels;

using System.Net;

namespace Management.Infrastructure.Department.IntegrationTests.Repositories.Department;

public class DepartmentRepositoryUpdateTests : IClassFixture<DepartmentRepositoryDatabase>
{
    private readonly DepartmentRepositoryDatabase _database;
    public DepartmentRepositoryUpdateTests(DepartmentRepositoryDatabase departmentRepositoryDatabase)
    {
        _database = departmentRepositoryDatabase;
        _database.SeedDatabase();
    }

    [Fact]
    public async Task UpdateDepartmentAsyncRep_ShouldReturnOk_WhenDepartmentIsUpdated()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            // Act
            var department1 = await repository.GetDepartmentByIdAsyncRep(1);
            var departmentUpdate = new DepartmentMod { Id = 1, Name = "Department 01 Updated" };
            var result = await repository.UpdateDepartmentAsyncRep(departmentUpdate);

            // Assert
            if(result.Data != null)
            {
                Assert.Equal(departmentUpdate.Name, result.Data.Name);
                Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("Erfolgreiches Updaten", result.Message);
            }
        }
    }

    [Fact]
    public async Task UpdateDepartmentAsyncRep_ShouldReturnBadRequest_WhenDepartmentIsNull()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            // Act
            var departmentUpdate = new DepartmentMod { };
            var result = await repository.UpdateDepartmentAsyncRep(departmentUpdate);

            // Assert
            if(result.Data != null)
            {
                Assert.Null(result.Data);
                Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
                Assert.Equal("DepartmentMod cannot be null", result.Message);
            }
        }
    }

    [Fact]
    public async Task UpdateDepartmentAsyncRep_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            // Act
            var departmentUpdate = new DepartmentMod { Id = 12, Name = "" };
            var result = await repository.UpdateDepartmentAsyncRep(departmentUpdate);

            // Assert
            if(result.Data != null)
            {
                Assert.Null(result.Data);
                Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
                Assert.Equal("DepartmentMod not found", result.Message);
            }
        }
    }
}