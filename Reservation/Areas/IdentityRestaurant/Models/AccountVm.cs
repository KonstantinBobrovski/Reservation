using Reservation.Core.Models;

namespace Reservation.Areas.IdentityRestaurant.Models
{
    public class AccountVm
    {
        public Restaurant Restaurant { get; set; }
        public List<Reservation.Core.Models.Reservation> Reservations { get; set; }
    }
}
