

using Management.Infrastructure.Department.Repositories.DepartmentRepositories;
using Management.Infrastructure.Department.Data;


using System.Net;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Management.Core.Models.DepartmentModels;
using System.Security.Cryptography.X509Certificates;
using Management.Infrastructure.Department.UnitTests.Helpers;
using Management.Core.Interfaces.DepartmentInterfaces.RepositoryInterfaces;

namespace Management.Infrastructure.Department.UnitTests.Repositories.Department;

// [Collection("Mock Collection")]
[Trait("Category", "Unit")]
public class DepartmentRepositoryGetAllUnitTests : IClassFixture<DepartmentRepositoryDatabase>
{
    private readonly AppDbContextDepartment _context;
    private readonly DepartmentRepository _repository;

    public DepartmentRepositoryGetAllUnitTests(DepartmentRepositoryDatabase fixture)
    {
        _context = new AppDbContextDepartment(fixture.Options);
        _repository = new DepartmentRepository(_context);
    }

    [Fact]
    public async Task GetAllDepartmentsAsyncRep_Returns_CorrectData_WhenDataExists()
    {
        // Act
        var result = await _repository.GetAllDepartmentsAsyncRep();

        // Assert
        Assert.True(result.Success);
        Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        if(result.Data != null)
        {
            Assert.Equal(5, result.Data.Count);
            Assert.Contains(result.Data, d => d.Name == "Department 01");
        }
    }
}
