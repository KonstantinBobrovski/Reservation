using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Areas.IdentityRestaurant.Models
{
    public class SignInVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
    }
}
