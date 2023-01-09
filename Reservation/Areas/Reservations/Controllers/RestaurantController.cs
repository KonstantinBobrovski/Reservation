using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation.Attributes;
using Reservation.consts;

namespace Reservation.Areas.Reservations.Controllers
{
    [UserTypeRoute(UserTypeEnum.Client)]
    [Area("Reservations")]
    [Route("[area]/[controller]")]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService) {
            _restaurantService = restaurantService;
        }

        // GET: RestaurantController
        [HttpGet]
        [Route("")]
        [Route("Index")]
        public async Task<ActionResult> All()
        {
            var restaurants =await _restaurantService.GetAllRestaurants();
            return View("Index", new Models.AllRestaurantsVm(restaurants.Value));
        }

       
    }
}
