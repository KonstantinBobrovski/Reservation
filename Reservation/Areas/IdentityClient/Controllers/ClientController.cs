using Core.Interfaces;
using Infrastructure.IdentityContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reservation.Areas.IdentityClient.Models;
using Reservation.Attributes;
using Reservation.consts;
using Reservation.Controllers;
using Reservation.Core.Interfaces;
using System.Security.Claims;

namespace Reservation.Areas.IdentityClient.Controllers
{
    [Area("IdentityClient")]
    [Route("/Identity/Client/{action}")]
    public class ClientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IReservationService _reservationService;
        public ClientController(
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager,
            IReservationService reservationService
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _reservationService = reservationService;

        }
        // GET: Identity
        [HttpGet]
        public ActionResult SignUp()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpVM vm)
        {
            var user = new ApplicationUser { UserName = vm.Email, Email = vm.Email };
            
            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
            
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, UserTypeEnum.Client.ToString()));
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction(nameof(Account));

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
               
            }
            return View(vm);
        }


        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignUpVM vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if(user is null)
            {
                ModelState.AddModelError("", "No such user");
                return View(vm);
            }
            var result = await _signInManager.PasswordSignInAsync(user, vm.Password,true, false);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Account));

            }
            else
            {
                ModelState.AddModelError("", "Wrong Password");


            }
            return View(vm);
        }

        [HttpGet]
        [UserTypeRoute(UserTypeEnum.Client)]
        public IActionResult Account()
        {
            var reseravations = _reservationService.GetReservationsFor(User.Identity.Name)?.Result?.Value ?? new ();

            return View(new AccountVM() { Reservations=reseravations});

        }

        [HttpPost]
        [UserTypeRoute(UserTypeEnum.Client)]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/index");
           // return RedirectToAction(nameof(HomeController.Index), nameof(HomeController), new { area = });
        }


    }
}
