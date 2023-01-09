using Infrastructure.IdentityContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Reservation.Areas.IdentityClient.Models;
using Reservation.Attributes;
using Reservation.consts;
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
        public ClientController(
             UserManager<ApplicationUser> userManager,
             SignInManager<ApplicationUser> signInManager
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;

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

        [HttpGet]
        [UserTypeRoute(UserTypeEnum.Client)]
        public IActionResult Account()
        {
            return View();

        }


    }
}
