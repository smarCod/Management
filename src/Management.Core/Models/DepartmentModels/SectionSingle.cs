

namespace Management.Core.Models.DepartmentModels;

public class SectionTyp
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SectionSingle
{
    public int Id { get; set; }

    public int? SectionTypId { get; set; }
    public SectionTyp SectionTyp { get; set; } = new SectionTyp();
    
    public string Name { get; set; } = string.Empty;
    public decimal Size { get; set; }
    public string Description { get; set; } = string.Empty;

    public int? SectionId { get; set; }
    public Section Section { get; set; } = new Section();

    public ICollection<Device> Devices { get; set; } = new List<Device>();

    public ICollection<SectionSinglePort> SectionSinglePorts { get; set; } = new List<SectionSinglePort>();
}
