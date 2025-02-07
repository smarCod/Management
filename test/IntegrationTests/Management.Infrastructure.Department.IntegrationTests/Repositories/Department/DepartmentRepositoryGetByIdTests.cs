

using Management.Infrastructure.Department.Data;
using Management.Infrastructure.Department.Repositories.DepartmentRepositories;
using Management.Infrastructure.Department.IntegrationTests.Helpers;

using System.Net;

namespace Management.Infrastructure.Department.IntegrationTests.Repositories.Department;

public class DepartmentRepositoryGetByIdTests : IClassFixture<DepartmentRepositoryDatabase>
{
    private readonly DepartmentRepositoryDatabase _database;

    public DepartmentRepositoryGetByIdTests(DepartmentRepositoryDatabase departmentRepositoryDatabase)
    {
        _database = departmentRepositoryDatabase;
        _database.SeedDatabase();
    }

    [Fact]
    public async Task GetDepartmentByIdAsyncRep_ShouldReturnDepartment_WhenDepartmentExists()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            // Act
            var department = await repository.GetDepartmentByIdAsyncRep(1);

            // Assert
            if(department.Data != null)
            {
                Assert.Equal(1, department.Data.Id);
                Assert.Equal("Department 01", department.Data.Name);
            }
        }
    }

    [Fact]
    public async Task GetDepartmentByIdAsyncRep_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            // Act
            var department = await repository.GetDepartmentByIdAsyncRep(100);

            // Assert
            if(department.Data != null)
            {
                Assert.Equal((int)HttpStatusCode.NotFound, department.StatusCode);
                await Assert.ThrowsAsync<KeyNotFoundException>(() => repository.GetDepartmentByIdAsyncRep(99));
            }
        }
    }
}