


namespace Management.Core.Models.DepartmentModels;

public class Port
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<SectionSinglePort> SectionSinglePorts { get; set; } = new List<SectionSinglePort>();
}
