using Core.Interfaces;
using Infrastructure.IdentityContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reservation.Areas.IdentityRestaurant.Models;
using Reservation.Attributes;
using Reservation.consts;
using System.Security.Claims;

namespace Reservation.Areas.IdentityRestaurant.Controllers
{
    [Area("IdentityRestaurant")]
    [Route("/Identity/Restaurant/")]
    public class RestaurantController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IReservationService _reservationService;
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(
              UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
             IReservationService reservationService,
             IRestaurantService restaurantService)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _reservationService = reservationService;
            _restaurantService= restaurantService;
        }

        [HttpGet("SignIn")]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn(SignInVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                ModelState.AddModelError("Bad_Data", "Wrong email or password");
                return View(vm);
            }


            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, true, false);

            if (result.Succeeded)
            {

                var claims = await _userManager.GetClaimsAsync(user);
                var role = claims.Where(claim => claim.Type == ClaimTypes.Role).FirstOrDefault();
                if (role is null)
                {
                    ModelState.AddModelError("Wrong_Uri", "This is route for only system administrators");

                    return View(vm);
                }
                if (role.Value != (UserTypeEnum.Restaraunt_Administrator.ToString()))
                {
                    ModelState.AddModelError("Wrong_Uri", "This is route for only restaurant administrators");
                    return View(vm);
                }



                return RedirectToAction(nameof(Account));

            }
            else
            {
                ModelState.AddModelError("Bad_Data", "Wrong email or password");

            }

            return View(vm);
        }

        [HttpGet("Account")]
        [UserTypeRoute(UserTypeEnum.Restaraunt_Administrator)]
        public async Task<IActionResult> Account()
        {
            var restaurantId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == MyClaimTypes.RestaurantId)?.Value);
            var restaurant = await _restaurantService.GetRestaurant(restaurantId);
            var reservations = await _reservationService.GetReservationsForRestaurant(restaurantId);
            return View(new AccountVm { Reservations=reservations.Value,Restaurant= restaurant.Value});
        }

        [HttpPost("SignOut")]
        [UserTypeRoute(UserTypeEnum.Restaraunt_Administrator)]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/home/index");
            // return RedirectToAction(nameof(HomeController.Index), nameof(HomeController), new { area = "" });
        }
    }
}
