using System.ComponentModel.DataAnnotations;

namespace SharedClassLibrary.DTOs
{
    public class ScholarshipDTO
    {
        public string? Id { get; set; } = string.Empty;
        [Required]
        public string? Title { get; set; } = string.Empty;

        [Required]
        public int? Budget { get; set; } = 0;

        [Required]
        public string? Location { get; set; } = string.Empty;

        [Required]
        public string? School_name { get; set; } = string.Empty;

        [Required]
        public string? Url { get; set; } = string.Empty; // Add this property for the URL
    }
}
