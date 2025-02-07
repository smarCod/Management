

using Management.Core.Models.DepartmentModels;
using Management.Infrastructure.Department.Data;
using Management.Infrastructure.Department.Repositories.DepartmentRepositories;
using Management.Infrastructure.Department.IntegrationTests.Helpers;

using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Net;

namespace Management.Infrastructure.Department.IntegrationTests.Repositories.Department;

public class DepartmentRepositoryGetAllTests : IClassFixture<DepartmentRepositoryDatabase>
{
    private readonly DepartmentRepositoryDatabase _database;

    public DepartmentRepositoryGetAllTests(DepartmentRepositoryDatabase database)
    {
        _database = database;
        _database.SeedDatabase();
    }

    [Fact]
    public async Task GetAllDepartmentsAsyncRep_ReturnsAllDepartments()
    {
        _database.SeedDatabase();
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            var repository = new DepartmentRepository(context);

            // Act
            var departments = await repository.GetAllDepartmentsAsyncRep();

            // Assert
            if (departments.Data != null)
                Assert.Equal(5, departments.Data.Count);
        }
    }
    
    [Fact]
    public async Task GetAllDepartmentsAsyncRep_ReturnsEmptyList_WhenNoDepartmentsExist()
    {
        // Arrange
        using (var context = new AppDbContextDepartment(_database.Options))
        {
            // Lösche alle bestehenden Daten, um sicherzustellen, dass die Liste leer ist
            context.PostDepartment.RemoveRange(context.PostDepartment);
            await context.SaveChangesAsync();

            var repository = new DepartmentRepository(context);

            // Act
            var departments = await repository.GetAllDepartmentsAsyncRep();

            // Assert
            Assert.False(departments.Success);
            Assert.Equal("Keine Departments gefunden.", departments.Message);
            if(departments.Data != null)
                Assert.Empty(departments.Data);
            Assert.Equal((int)HttpStatusCode.NotFound, departments.StatusCode);
        }
    }
}
