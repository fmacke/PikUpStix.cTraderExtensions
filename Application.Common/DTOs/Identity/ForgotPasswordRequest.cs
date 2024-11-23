using System.ComponentModel.DataAnnotations;

namespace Application.Common.DTOs.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}