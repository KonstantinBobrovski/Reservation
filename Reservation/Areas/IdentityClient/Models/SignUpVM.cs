using System.ComponentModel.DataAnnotations;

namespace Reservation.Areas.IdentityClient.Models
{
    public class SignUpVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(6, ErrorMessage ="Min lengtn of password 6")]
        public string Password { get; set; } = null!;
    }
}
