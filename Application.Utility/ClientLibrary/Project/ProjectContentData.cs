using System.Collections.Generic;

namespace Application.Utility.ClientLibrary.Project
{
    public class ProjectContentData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public decimal Amount { get; set; }
        public List<string> Files { get; set; } = new List<string>();
    }
}