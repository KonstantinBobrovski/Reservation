using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IClientNotifier
    {
        Task SendReservationStateChange(string clientId, Reservation.Core.Models.Reservation reservation);
    }
}
