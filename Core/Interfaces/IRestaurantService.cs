using Core.ResultLibrary;
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

        Task<Result<List<Restaurant>>> GetAllRestaurants();
        Task<Result<Restaurant>> GetRestaurant(int Id);

        Task<Result<Restaurant>> CreateRestaurant(Restaurant restaurant);

        Task<Result<Restaurant>> DeleteRestaurant(int Id);

        Task<Result<Restaurant>> UpdateRestaurant(int Id,Restaurant r);

        Task<Result<List<Reservation.Core.Models.Table>>> GetTablesWithReservations(int restaurantId, DateTime from, DateTime? to);
    }
}
