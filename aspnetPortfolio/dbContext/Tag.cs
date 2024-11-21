using System;
using System.Collections.Generic;

namespace aspnetPortfolio.dbContext;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
}
