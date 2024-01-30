namespace EDUHUNT_BE.Model
{
    // Message.cs

    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }

}
