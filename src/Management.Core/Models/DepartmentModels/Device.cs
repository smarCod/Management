




namespace Management.Core.Models.DepartmentModels;

public class Device
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int? SectionSingleId { get; set; }
    public SectionSingle SectionSingle { get; set; } = new SectionSingle();
}