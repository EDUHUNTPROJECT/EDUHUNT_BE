namespace EDUHUNT_BE.Model
{
    public class ScholarshipInfo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Budget { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string SchoolName { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public bool IsInSite { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
