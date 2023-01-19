using Core.Interfaces;
using Core.ResultLibrary;
using Core.Services;
using Infrastructure.IdentityContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reservation.Areas.IdentityAdministrators.Models;
using Reservation.Attributes;
using Reservation.consts;
using Reservation.Controllers;
using Reservation.Core.Models;
using System.Drawing.Imaging;
using System.Security.Claims;

namespace Reservation.Areas.IdentityAdministrators.Controllers
{
    [Area("IdentityAdministrators")]
    [Route("/Identity/Administrator/System/{action}/{id?}")]

    public class SystemAdministratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IRestaurantService _restaurantService;
        private readonly IFileService _fileService;


        public SystemAdministratorController(
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
            IRestaurantService restaurantService,
            IFileService fileService
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _restaurantService = restaurantService;
            _fileService = fileService;
        }
        // GET: Identity
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
                if (role.Value != (UserTypeEnum.System_Administrator.ToString()))
                {
                    ModelState.AddModelError("Wrong_Uri", "This is route for only system administrators");
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

        [HttpGet]
        [UserTypeRoute(UserTypeEnum.System_Administrator)]
        public async Task<IActionResult> Account()
        {
            var restaurants = await _restaurantService.GetAllRestaurants();
            if (restaurants.State == ResultState.Success)
                return View("RestaurantsList", new AllRestaurantsVM() { Restaurants = restaurants.Value });
            ModelState.AddModelError("Internal", "Internal error occured");
            return View("RestaurantsList");

        }

        [HttpGet]
        [UserTypeRoute(UserTypeEnum.System_Administrator)]
        public IActionResult AddNewRestaurant()
        {

            return View("Restaurant");
        }

        [HttpPost]
        [UserTypeRoute(UserTypeEnum.System_Administrator)]
        public async Task<IActionResult> AddNewRestaurantAsync(RestaurantCreateVM restaurantVm)
        {
            if (!ModelState.IsValid)
            {
                return View("Restaurant", restaurantVm);

            }
            var isThereSameEmail = (await _userManager.FindByEmailAsync(restaurantVm.Administrator.Email)) != null;

            if (isThereSameEmail)
            {
                ModelState.AddModelError("", "This Email is used Already. Try another email for the administrator");
                return View("Restaurant", restaurantVm);
            }



            var pathForSchema =  restaurantVm.Name.ToString() + Path.GetExtension(restaurantVm.SchemaOfRestaurant.Name);
            var restaurant = new Core.Models.Restaurant() { Name = restaurantVm.Name, Description = restaurantVm.Description, AddressOfSchemaImage = "/static/" + pathForSchema, Tables = restaurantVm.Tables.Select(t => new Core.Models.Table() { Capacity = t.Capacity , NameOfTable=t.Name}).ToList() };
            var createdRestaurant = await _restaurantService.CreateRestaurant(restaurant);
            if (createdRestaurant.State != ResultState.Success)
            {
                ModelState.AddModelError("", createdRestaurant.ErrorMessage);
                return View("Restaurant", restaurantVm);

            }
            await _fileService.Save(pathForSchema, restaurantVm.SchemaOfRestaurant.OpenReadStream());

            var manager = new ApplicationUser() { Email = restaurantVm.Administrator.Email, UserName = restaurantVm.Administrator.Email };
            var isCreatedManager = await _userManager.CreateAsync(manager, restaurantVm.Administrator.Password);
            if (isCreatedManager.Succeeded)
            {
                var isAddedRole = await _userManager.AddClaimAsync(manager, new Claim(ClaimTypes.Role, UserTypeEnum.Restaraunt_Administrator.ToString()));
                var isRestaurantIdAdded = await _userManager.AddClaimAsync(manager, new Claim(MyClaimTypes.RestaurantId, createdRestaurant.Value.Id.ToString()));

                if (isAddedRole.Succeeded && isRestaurantIdAdded.Succeeded)
                {
                    return RedirectToAction(nameof(Account));

                }
            }
            ModelState.AddModelError("", "An error occured");

            return View("Restaurant", restaurantVm);

        }
//
//        [HttpGet]
//        [UserTypeRoute(UserTypeEnum.System_Administrator)]
//        public async Task<IActionResult> EditRestaurant(int id)
//        {
//            var restaurant = await _restaurantService.GetRestaurant(id);
//            if (restaurant.State == ResultState.Success)
//            {
//                return View("Restaurant", new RestaurantCreateVM() { Description = restaurant.Value.Description, Name = restaurant.Value.Name, Id = restaurant.Value.Id.ToString(), Tables = restaurant.Value.Tables.Select(t => new TableLike() { Capacity = t.Capacity }).ToList() });
//            }
//            else
//                return View("Restaurant");
//        }

        [HttpPost]
        [UserTypeRoute(UserTypeEnum.System_Administrator)]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            var restaurant = await _restaurantService.GetRestaurant(id);
            if (restaurant != null)
                await _restaurantService.DeleteRestaurant(id);
            var restaurants = await _restaurantService.GetAllRestaurants();
            if (restaurants.State == ResultState.Success)
                return View("RestaurantsList", new AllRestaurantsVM() { Restaurants = restaurants.Value });
            ModelState.AddModelError("Internal", "Internal error occured");
            return View("RestaurantsList");
        }

        [HttpPost]
        [UserTypeRoute(UserTypeEnum.System_Administrator)]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/home/index");
            // return RedirectToAction(nameof(HomeController.Index), nameof(HomeController), new { area = "" });
        }
    }
}
