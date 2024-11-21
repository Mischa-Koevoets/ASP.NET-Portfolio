using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace aspnetPortfolio.dbContext;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    [RegularExpression(@"^https:\/\/github\.com\/.+", ErrorMessage = "The GitHub link must start with 'https://github.com/'.")]
    public string GithubLink { get; set; }

    public string? FilePath { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
