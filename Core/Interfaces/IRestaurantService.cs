using Reservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRestaurantService
    {
        Reservation.Core.Models.Reservation CreateReservation(Table table,string clientId, DateTime startDate, DateTime? endDate);
    }
}
