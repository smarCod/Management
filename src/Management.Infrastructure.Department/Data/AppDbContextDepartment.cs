


using Microsoft.EntityFrameworkCore;

using Management.Core.Models.DepartmentModels;

namespace Management.Infrastructure.Department.Data;

public class AppDbContextDepartment : DbContext
{
    public AppDbContextDepartment(DbContextOptions<AppDbContextDepartment> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<SectionSinglePort>()
            .HasKey(ssk => new { ssk.SectionSingleId, ssk.PortId });
    }
    
    public DbSet<DepartmentMod> PostDepartment => Set<DepartmentMod>();
    public DbSet<Device> PostDevice => Set<Device>();
    public DbSet<Port> PostPort => Set<Port>();
    public DbSet<Section> PostSection => Set<Section>();
    public DbSet<SectionTyp> PostSectionTyp => Set<SectionTyp>();
    public DbSet<SectionSingle> PostSectionSingle => Set<SectionSingle>();
    public DbSet<SectionSinglePort> PostSectionSinglePort => Set<SectionSinglePort>();
}

