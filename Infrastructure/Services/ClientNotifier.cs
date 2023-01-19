using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ClientNotifier : IClientNotifier
    {
        public Task SendReservationStateChange(string clientId, Reservation.Core.Models.Reservation reservation)
        {
           return Task.CompletedTask;
        }
    }
}
