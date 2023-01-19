using Reservation.Core.Models;

namespace Reservation.Areas.Reservations.Models
{
    public class RestaurantVM
    {
        public Restaurant Restaurant { get; private set; }

        public List<Table> Tables { get; private set; }

        public DateTime MaxReservationCheckDate { get; private set; } = DateTime.Now.AddDays(2);
        public RestaurantVM(Restaurant restaurant, List<Table> tables, DateTime? maxReservaitonDate)
        {
            Restaurant = restaurant;
            Tables = tables;
            if(maxReservaitonDate!=null)
            MaxReservationCheckDate = maxReservaitonDate.Value;
        }
    }
}
