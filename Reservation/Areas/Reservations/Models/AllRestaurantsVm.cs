using Reservation.Core.Models;

namespace Reservation.Areas.Reservations.Models
{
    public class AllRestaurantsVm
    {
        public List<Restaurant> Restaurants { get; set; } = new();
        public AllRestaurantsVm(List<Restaurant> _Restaurants)
        {
            Restaurants = _Restaurants;
        }
    }
}
