using System;
using System.ComponentModel.DataAnnotations;

namespace EDUHUNT_BE.Model
{
    public class ScholarshipInfo
    {
        public Guid? Id { get; set; }

        [Required]
        public string Budget { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string SchoolName { get; set; }

        public string Description { get; set; }

        public int? CategoryId { get; set; } = 0;

        [Required]
        public string AuthorId { get; set; }
        public bool? IsInSite { get; set; } = false;
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsApproved { get; set; } = false;
        public string ImageUrl { get; set; }
    }
}