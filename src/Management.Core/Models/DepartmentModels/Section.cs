

namespace Management.Core.Models.DepartmentModels;

public class Section
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<SectionSingle> SectionSingles { get; set; } = new List<SectionSingle>();
}
