using Ardalis.Specification;
using Core.Interfaces;
using Core.ResultLibrary;
using Reservation.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Core.Services
{
    public class RestaurantService : IRestaurantService
    {
        

        IRepository<Restaurant> _restaurantsRepository;
        IRepository<Table> _tablesRepository;
        IRepository<Reservation.Core.Models.Reservation> _reservationRepository;

        public RestaurantService(IRepository<Restaurant> restaurantsRepository, IRepository<Table> tablesRepo,IRepository<Reservation.Core.Models.Reservation> resRepo)
        {
            _restaurantsRepository = restaurantsRepository;
            _tablesRepository = tablesRepo;
            _reservationRepository = resRepo;
        }

        public async Task<Result<Restaurant>> CreateRestaurant(Restaurant restaurant)
        {

            return new Result<Restaurant>(await _restaurantsRepository.AddAsync(restaurant));
        }

        public async Task<Result<Restaurant>> DeleteRestaurant(int Id)
        {
            var restaurant = await _restaurantsRepository.GetByIdAsync(Id);
            await _restaurantsRepository.DeleteAsync(restaurant);
            return new Result<Restaurant>(restaurant);
        }

        public async Task<Result<List<Restaurant>>> GetAllRestaurants()
        {
            var res = await _restaurantsRepository.ListAsync();
            return new Result<List<Restaurant>>(res);
        }

        public async Task<Result<Restaurant>> GetRestaurant(int Id)
        {
            var restaurant = await _restaurantsRepository.GetBySpecAsync(new GetRestaurantSpec(Id));
           
            return new Result<Restaurant>(restaurant);
        }

        public async Task<Result<Restaurant>> UpdateRestaurant(int Id, Restaurant r)
        {
           await _restaurantsRepository.UpdateAsync(r);
            return new Result<Restaurant>(r);
        }

       

        public async Task<Result<List<Table>>> GetTablesWithReservations(int restaurantId, DateTime from, DateTime? to)
        {
            try
            {
                var result =await _tablesRepository.ListAsync(new GetTablesWithReservationsSpec(restaurantId, from, to));
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private class GetRestaurantSpec : Specification<Restaurant>, ISingleResultSpecification
        {
            public GetRestaurantSpec(int id)
            {
                Query.Where(company => company.Id == id).Include(x => x.Tables);
            }
        }

        private class GetTablesWithReservationsSpec : Specification<Table>
        {
            public GetTablesWithReservationsSpec(int restaurantId, DateTime from, DateTime? to)
            {
                if(to is null)
                    Query.Where(table => table.RestaurantId == restaurantId).Include(x => x.Reservations.Where(res=> res.ReservationState == Models.ReservationState.Approved &&  res.StartDate>=from));
                else
                    Query.Where(table => table.RestaurantId == restaurantId).Include(x => x.Reservations.Where(res => res.ReservationState==Models.ReservationState.Approved && res.StartDate >= from && res.StartDate<=to));

            }
        }

        
    }
}
