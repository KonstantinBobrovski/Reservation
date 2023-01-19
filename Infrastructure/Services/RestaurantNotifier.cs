using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RestaurantNotifier : IRestaurantNotifier
    {
        public Task SendReservationStateChange(int restaurantId, Reservation.Core.Models.Reservation reservation)
        {
            return Task.CompletedTask;
        }
    }
}
