using EDUHUNT_BE.Data;

namespace EDUHUNT_BE.Model
{
    public class RoadMap
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ContentURL { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
