
namespace EDUHUNT_BE.Model
{
   
    public class Profile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ContentURL { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ContactNumber { get; set; }
        public string Address {  get; set; }
        public string Description { get; set; }
        public string UrlAvatar { get; set; }
        public bool IsVIP { get; set; }
        public bool IsAllow { get; set; } = false;

    }

    
}
