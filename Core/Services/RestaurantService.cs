using Core.Interfaces;
using Reservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        public Reservation.Core.Models.Reservation CreateReservation(Table table, string clientId, DateTime startDate, DateTime? endDate)
        {
            return new Reservation.Core.Models.Reservation();
        }
    }
}
