namespace EDUHUNT_BE.Model
{
    public class QA
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AskerId { get; set; }
        public Guid AnswerId { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    }
}
