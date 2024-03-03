using System;
using System.ComponentModel.DataAnnotations;

namespace EDUHUNT_BE.Model
{
    public class CodeVerify
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime ExpirationTime { get; set; }
    }
}