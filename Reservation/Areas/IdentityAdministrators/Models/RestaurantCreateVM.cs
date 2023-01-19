using Reservation.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Areas.IdentityAdministrators.Models
{
    public class RestaurantCreateVM
    {
 
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public UserLikeVM Administrator { get; set; } = new();

        [Required]
        public List<TableLikeVM> Tables { get; set; }

        [Required]
        [Display(Name = "File")]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile SchemaOfRestaurant { get; set; }

    }
    public class UserLikeVM
    {
      

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
    }
    public class TableLikeVM
    {

        [Required]
        public int Capacity { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
