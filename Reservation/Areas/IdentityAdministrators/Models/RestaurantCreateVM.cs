using Reservation.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Reservation.Areas.IdentityAdministrators.Models
{
    public class RestaurantCreateVM
    {
        
        public string? Id { get; set; }= "";
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        
        public UserLike Administrator { get; set; } = new();

        public List<TableLike> Tables { get; set; }

        [Required]
        [Display(Name = "File")]
        [AllowedExtensions(new string[] { ".jpg", ".png" })]
        public IFormFile SchemaOfRestaurant { get; set; }

    }
    public class UserLike
    {
       
        public string? Id { get; set; } = "";

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; } = null!;
    }
    public class TableLike
    {
        public string? Id { get; set; } = "";

        [Required]
        public int Capacity { get; set; }
    }
}
