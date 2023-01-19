using Core.Models;
using Core.ResultLibrary;
using Reservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IReservationService
    {
        Task<Result<Reservation.Core.Models.Reservation>> CreateReservation(int tableId, string clientId, DateTime startDate, DateTime endDate);

        Task<Result<List<Reservation.Core.Models.Reservation>>> GetReservationsFor(string clientId);

        Task<Result<List<Reservation.Core.Models.Reservation>>> GetReservationsForRestaurant(int restaurantId);


        Task<Result<Reservation.Core.Models.Reservation>> GetReservationById(int id);

        Task<Result<Reservation.Core.Models.Reservation>> UpdateReservationState(int id, ReservationState newState);


    }
}
