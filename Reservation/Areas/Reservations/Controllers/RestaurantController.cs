using Core.Interfaces;
using Core.ResultLibrary;

using Microsoft.AspNetCore.Mvc;
using Reservation.Areas.IdentityClient.Controllers;
using Reservation.Areas.Reservations.Models;
using Reservation.Attributes;
using Reservation.consts;
using System.Security.Claims;

namespace Reservation.Areas.Reservations.Controllers
{

    [Area("Reservations")]
    [Route("/Restaurants/")]
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IReservationService _reservationService;
        public RestaurantController(IRestaurantService restaurantService, IReservationService reservationService)
        {
            _restaurantService = restaurantService;
            _reservationService = reservationService;
        }

        // GET: RestaurantController
        [HttpGet]
        [Route("Home")]
        [Route("Index")]
        [Route("")]
        [UserTypeRoute(UserTypeEnum.Client)]
        public async Task<IActionResult> Home()
        {
            var restaurants = await _restaurantService.GetAllRestaurants();
            if (restaurants.State == ResultState.Success)
                return View("Index", new Models.AllRestaurantsVm(restaurants.Value));
            else
                throw new Exception();
        }

        [HttpGet("{id:int}")]
        [UserTypeRoute(UserTypeEnum.Client)]
        public async Task<IActionResult> Restaurant(int id, DateTime? maxCheckReservation)
        {

            if (maxCheckReservation is null || maxCheckReservation <= DateTime.UtcNow)
            {
                maxCheckReservation = DateTime.UtcNow.AddDays(2);
            }
            else
            {
                maxCheckReservation = maxCheckReservation.Value.ToUniversalTime();

            }
            //Can be parallel
            var restaurant = await _restaurantService.GetRestaurant(id);
            var tables = await _restaurantService.GetTablesWithReservations(id, DateTime.UtcNow, maxCheckReservation);

            return View(new RestaurantVM(restaurant.Value, tables.Value, maxCheckReservation));
        }

        [HttpGet("{id:int}/createReservation")]
        [UserTypeRoute(UserTypeEnum.Client)]
        public async Task<IActionResult> CreateReservation(int id, ReservationCreateVM viewModel)
        {
            var reservationCheckFrom = viewModel?.StartTime;
            var maxCheckReservation = viewModel?.EndTime;
            reservationCheckFrom = reservationCheckFrom < DateTime.UtcNow ? DateTime.UtcNow : reservationCheckFrom?.ToUniversalTime();
            if (maxCheckReservation == null || maxCheckReservation < reservationCheckFrom)
            {
                maxCheckReservation = reservationCheckFrom.Value.AddHours(3);
            }
            else
            {
                maxCheckReservation = maxCheckReservation.Value.ToUniversalTime();
            }
            var restaurant = await _restaurantService.GetRestaurant(id);
            var tables = await _restaurantService.GetTablesWithReservations(id, reservationCheckFrom.Value, maxCheckReservation);

            var vm = new ReservationCreateVM(restaurant.Value.Name, tables.Value) { StartTime = reservationCheckFrom.Value, EndTime = maxCheckReservation.Value };
            return View(vm);
        }

        [HttpPost("{id:int}/createReservation")]
        [UserTypeRoute(UserTypeEnum.Client)]
        public async Task<IActionResult> CreateReservationPost(int id, ReservationCreateVM viewModel)
        {
            var userId = User.Identity.Name;
            var reservationTask = await _reservationService.CreateReservation(viewModel.TableId, userId, viewModel.StartTime.ToUniversalTime(), viewModel.EndTime.ToUniversalTime());
            if (reservationTask.State != ResultState.Success)
            {
                ModelState.AddModelError("ResultState", reservationTask.ErrorMessage);
                return View("CreateReservation", viewModel);
            }

            //TODO: Find way for better types
            return RedirectToAction(nameof(ClientController.Account), nameof(ClientController).Replace("Controller", ""), new { area = "Identity" });
        }

        [HttpGet("{restaurnatId:int}/{reservationId:int}")]
        [UserTypeRoute(UserTypeEnum.Client | UserTypeEnum.Restaraunt_Administrator)]
        public async Task<IActionResult> Reservation(int restaurnatId, int reservationId)
        {
            var userRole = User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;


            var reservation = await _reservationService.GetReservationById(reservationId);

            //it is not his reservation
            if (userRole == UserTypeEnum.Client.ToString() && reservation.Value.ClientId != User.Identity.Name)
            {
                return Forbid();
            }
            else if (userRole == UserTypeEnum.Restaraunt_Administrator.ToString() && reservation.Value.Table.RestaurantId.ToString() != User.Claims.FirstOrDefault(claim => claim.Type == MyClaimTypes.RestaurantId)?.Value)
            {
                //admin not of this restaurant
                return Forbid();

            }

            return View(new ReservationVM() { Reservation = reservation.Value });
        }

        [HttpPost("{restaurnatId:int}/{reservationId:int}")]
        [UserTypeRoute(UserTypeEnum.Client | UserTypeEnum.Restaraunt_Administrator)]
        public async Task<IActionResult> ReservationChange(int restaurnatId, int reservationId, ReservationVM vm)
        {
            var reservation = await _reservationService.UpdateReservationState(reservationId, vm.NewState);
            if (reservation.State != ResultState.Success)
            {
                ModelState.AddModelError("Validation", reservation.ErrorMessage);
                return View("Reservation", vm);

            }
            else
            {
                if (User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value == UserTypeEnum.Client.ToString())
                    return RedirectToAction(nameof(ClientController.Account), nameof(ClientController).Replace("Controller", ""), new { area = "Identity" });
                else
                    return Redirect("/Identity/Restaurant/Account");

            }
        }

    }
}
