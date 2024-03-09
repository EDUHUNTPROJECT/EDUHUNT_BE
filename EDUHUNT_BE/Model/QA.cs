namespace EDUHUNT_BE.Model
{
    public class QA
    {

        public Guid Id { get; set; }
        public Guid AskerId { get; set; }
        public Guid? AnswerId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string AskerFile { get; set; }
        public string AnswerFile { get; set; }
    }
}
