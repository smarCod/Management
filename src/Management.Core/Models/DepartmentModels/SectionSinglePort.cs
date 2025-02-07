


namespace Management.Core.Models.DepartmentModels;

public class SectionSinglePort
{
    public int? SectionSingleId { get; set; }
    public SectionSingle SectionSingle { get; set; } = new SectionSingle();

    public int? PortId { get; set; }
    public Port Port { get; set; } = new Port();
}
