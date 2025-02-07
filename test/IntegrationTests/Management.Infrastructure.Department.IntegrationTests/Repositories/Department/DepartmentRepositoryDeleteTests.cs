

using Management.Infrastructure.Department.Data;
using Management.Infrastructure.Department.Repositories.DepartmentRepositories;
using Management.Infrastructure.Department.IntegrationTests.Helpers;
using Management.Core.Models.DepartmentModels;

using System.Net;

namespace Management.Infrastructure.Department.IntegrationTests.Repositories.Department;

public class DepartmentRepositoryDeleteTests : IClassFixture<DepartmentRepositoryDatabase>
{
    private readonly DepartmentRepositoryDatabase _database;
    public DepartmentRepositoryDeleteTests(DepartmentRepositoryDatabase departmentRepositoryDatabase)
    {
        _database = departmentRepositoryDatabase;
        _database.SeedDatabase();
    }

    [Fact]
    public async Task DeleteDepartmentAsyncRep_ShouldReturnOk_WhenDepartmentIsDeleted()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            //  Act
            var result = await repository.DeleteDepartmentAsyncRep(2);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Id);
            Assert.Equal("Department 02", result.Data.Name);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Erfolgreich deleted", result.Message);
        }
    }

    [Fact]
    public async Task DeleteDepartmentAsyncRep_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            //  Act
            var result = await repository.DeleteDepartmentAsyncRep(23);

            // Assert
            Assert.False(result.Success);
            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("DepartmentMod not found", result.Message);
        }
    }
}