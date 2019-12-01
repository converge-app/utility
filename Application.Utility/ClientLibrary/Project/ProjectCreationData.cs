using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Utility.ClientLibrary.Project
{
    public class ProjectCreationData
    {
        [Required]
        public string OwnerId { get; set; }
        public string FreelancerId { get; set; }

        [Required]
        public ProjectContentData ProjectContent { get; set; }
    }
}
