using Management.Core.Models.DepartmentModels;
using Management.Infrastructure.Department.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Infrastructure.Department.UnitTests.Helpers;

public class DepartmentRepositoryDatabase
{
    public DbContextOptions<AppDbContextDepartment> Options { get; set; }
    public DepartmentRepositoryDatabase()
    {
        Options = new DbContextOptionsBuilder<AppDbContextDepartment>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // Datenbank initialisieren
        SeedDatabase();
    }

    public void SeedDatabase()
    {
        using (var context = new AppDbContextDepartment(Options))
        {
            context.PostDepartment.RemoveRange(context.PostDepartment);
            context.PostDepartment.AddRange(
                new DepartmentMod { Id = 1, Name = "Department 01" },
                new DepartmentMod { Id = 2, Name = "Department 02" },
                new DepartmentMod { Id = 3, Name = "Department 03" },
                new DepartmentMod { Id = 4, Name = "Department 04" },
                new DepartmentMod { Id = 5, Name = "Department 05" }
            );
            context.SaveChanges();
        }
    }

    public void Dispose()
    {
        using (var context = new AppDbContextDepartment(Options))
        {
            context.Database.EnsureDeleted();
        }
    }
} 


