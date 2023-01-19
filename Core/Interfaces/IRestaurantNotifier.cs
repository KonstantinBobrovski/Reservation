using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRestaurantNotifier
    {
        public Task SendReservationStateChange(int restaurantId, Reservation.Core.Models.Reservation reservation);
    }
}
