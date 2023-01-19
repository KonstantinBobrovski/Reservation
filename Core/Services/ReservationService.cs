using Ardalis.Specification;
using Core.Interfaces;
using Core.Models;
using Core.ResultLibrary;
using Reservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class ReservationService : IReservationService
    {
        //TODO: Move to db
        public const int MAX_HOURS_RESERVATION = 5;

        IRepository<Table> _tablesRepository;
        IRepository<Reservation.Core.Models.Reservation> _reservationRepository;
        IClientNotifier _clientNotifier;
        IRestaurantNotifier _restaurantNotifier;
        public ReservationService(IRepository<Table> tablesRepo, IRepository<Reservation.Core.Models.Reservation> resRepo, IClientNotifier clientNotifier, IRestaurantNotifier restaurantNotifier)
        {
            _clientNotifier=clientNotifier;
            _restaurantNotifier = restaurantNotifier;
            _tablesRepository = tablesRepo;
            _reservationRepository = resRepo;
        }
        public async Task<Result<Reservation.Core.Models.Reservation>> CreateReservation(int tableId, string clientId, DateTime startDate, DateTime endDate)
        {
            var durationInMiliseconds = (endDate - startDate).TotalMilliseconds;
            if (durationInMiliseconds < 0)
            {
                return new Result<Reservation.Core.Models.Reservation>("End date is before start date", ResultState.BadData);

            }
            else if (durationInMiliseconds >= TimeSpan.FromHours(MAX_HOURS_RESERVATION).TotalMilliseconds)
            {
                return new Result<Reservation.Core.Models.Reservation>($"Duration is too big it shouldnt be more that {MAX_HOURS_RESERVATION} hours", ResultState.BadData);
            }

            var reservationsForTable = await _reservationRepository.CountAsync(new GetAcceptedReservationsForRangeSpec(tableId, startDate, endDate));
            if (reservationsForTable != 0)
            {
                return new Result<Reservation.Core.Models.Reservation>("There are already reservation for this period of time", ResultState.BadData);

            }

            var reservation = new Reservation.Core.Models.Reservation();
            reservation.ClientId = clientId;
            reservation.EndDate = endDate;
            reservation.StartDate = startDate;
            reservation.ReservationState = Models.ReservationState.Sended;
            reservation.TableId = tableId;
            await _reservationRepository.AddAsync(reservation);
            var reservationFromDb =await this.GetReservationById(reservation.Id);
#pragma warning disable CS4014 // Dont mind if it wont get an notify
            _restaurantNotifier.SendReservationStateChange(reservationFromDb.Value.Table.RestaurantId, reservationFromDb.Value);
#pragma warning restore CS4014 // 
            return reservation;
        }

        public async Task<Result<List<Reservation.Core.Models.Reservation>>> GetReservationsFor(string clientId)
        {
            var result = await _reservationRepository.ListAsync(new GetReservationsForUserSpec(clientId));
            return result;
        }

        public async Task<Result<Reservation.Core.Models.Reservation>> GetReservationById(int id)
        {
            var result = await _reservationRepository.GetBySpecAsync(new GetReservationByIdSpec(id));
            return result;
        }

        public async Task<Result<Reservation.Core.Models.Reservation>> UpdateReservationState(int id, ReservationState newState)
        {
            var reservation = await this.GetReservationById(id);
            if (reservation.Value.ReservationState == newState)
            {
                return new Result<Reservation.Core.Models.Reservation>("The state is same",ResultState.BadData);
            }
            reservation.Value.ReservationState = newState;
            await _reservationRepository.UpdateAsync(reservation.Value);
            string clientId = reservation.Value.ClientId;

#pragma warning disable CS4014 // Dont mind
            _clientNotifier.SendReservationStateChange(clientId, reservation.Value);
            _restaurantNotifier.SendReservationStateChange(reservation.Value.Table.RestaurantId, reservation.Value);
#pragma warning restore CS4014 // 

            return reservation;
        }

        public async Task<Result<List<Reservation.Core.Models.Reservation>>> GetReservationsForRestaurant(int restaurantId)
        {
            var result = await _reservationRepository.ListAsync(new GetReservationsForRestaurantSpec(restaurantId));
            return result;
        }

        private class GetReservationByIdSpec : Specification<Reservation.Core.Models.Reservation>
        {
            public GetReservationByIdSpec(int id)
            {
                Query.Where(res => res.Id == id).Include(res => res.Table).ThenInclude(t => t.Restaurant);
            }
        }

        private class GetReservationsForRestaurantSpec : Specification<Reservation.Core.Models.Reservation>
        {
            public GetReservationsForRestaurantSpec(int restaurantId)
            {
                Query.Where(res => res.Table.RestaurantId == restaurantId).Include(res => res.Table).ThenInclude(t => t.Restaurant);
            }
        }
        private class GetReservationsForUserSpec : Specification<Reservation.Core.Models.Reservation>
        {
            public GetReservationsForUserSpec(string clientId)
            {
                Query.Where(res => res.ClientId == clientId).Include(res => res.Table).ThenInclude(t => t.Restaurant);
            }
        }
        private class GetAcceptedReservationsForRangeSpec : Specification<Reservation.Core.Models.Reservation>
        {
            public GetAcceptedReservationsForRangeSpec(int tableId, DateTime from, DateTime to)
            {
                Query.Where(res => res.TableId == tableId &&
                res.ReservationState == Models.ReservationState.Approved &&
                (
                // db is full inside of new
                (res.StartDate >= from && res.EndDate <= to) ||
                //End is inside of anotehr reservation
                (res.StartDate < to && res.EndDate >= to) ||
                //Start is inside
                (res.StartDate <= from && res.EndDate >= from))
                );

            }
        }
    }
}
