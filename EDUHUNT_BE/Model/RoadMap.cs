namespace EDUHUNT_BE.Model
{
    public class RoadMap
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string ContentURL { get; set; }
    }
}
