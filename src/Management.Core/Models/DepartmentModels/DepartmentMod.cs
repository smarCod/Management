using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Management.Core.Models.DepartmentModels;

public class DepartmentMod
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public ICollection<Section> Sections { get; set; } = new List<Section>();
}

