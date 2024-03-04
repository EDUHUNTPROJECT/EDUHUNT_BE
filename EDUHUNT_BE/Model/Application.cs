namespace EDUHUNT_BE.Model
{
    public class Application
    {
        public Guid Id { get; set; }
        public Guid StudentID { get; set; }
        public Guid ScholarshipID { get; set; }

        public string StudentCV { get; set; }

        public string Status { get; set; }

        // New fields
        public string MeetingURL { get; set; }
        public DateTime StudentAvailableStartDate { get; set; }
        public DateTime StudentAvailableEndDate { get; set; }
        public DateTime ScholarshipProviderAvailableStartDate { get; set; }
        public DateTime ScholarshipProviderAvailableEndDate { get; set; }
    }
}
