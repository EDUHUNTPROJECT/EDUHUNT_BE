namespace EDUHUNT_BE.Model
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
