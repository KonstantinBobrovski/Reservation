using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Reservation.consts;
using System.Security.Claims;
using Reservation.Areas.IdentityClient.Controllers;
using Reservation.Areas.IdentityAdministrators.Controllers;
using System.Reflection;
using Reservation.Areas.IdentityRestaurant.Controllers;

namespace Reservation.Attributes
{
   
    public class UserTypeRouteAttribute: TypeFilterAttribute
    {

        public UserTypeRouteAttribute(UserTypeEnum userType):base(typeof(UserTypeRouteFilter))
        {
            Arguments = new object[] { userType };
        }


        /// <summary>
        /// Default uses Client UserType
        /// </summary>
        public UserTypeRouteAttribute() : this(UserTypeEnum.Client)
        { }
    }

    public class UserTypeRouteFilter : IAuthorizationFilter
    {
        private UserTypeEnum UserType { get;  set; }

        public UserTypeRouteFilter(UserTypeEnum userType)
        {
            UserType = userType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //default redirect is to client
            Type controller= typeof(ClientController);
            if (UserType.HasFlag(UserTypeEnum.System_Administrator)){
                controller = typeof(SystemAdministratorController);
            }
            else if (UserType.HasFlag(UserTypeEnum.Restaraunt_Administrator))
            {
                controller = typeof(RestaurantController);
            }
            else if (UserType.HasFlag(UserTypeEnum.Client))
            {
                controller = typeof(ClientController);
            }
          
            var redirectOnUnAuthorizedUrl=new RedirectToActionResult("SignIn", controller.Name.Replace("Controller","",StringComparison.OrdinalIgnoreCase ), new {area=controller.GetCustomAttribute<AreaAttribute>().RouteValue});
            var userType = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

            if (userType == null)
            {
                context.Result= redirectOnUnAuthorizedUrl;

            }
            else if (Enum.TryParse(userType, out UserTypeEnum userTypeAsEnum))
            {
                if (!this.UserType.HasFlag(userTypeAsEnum))
                    context.Result = redirectOnUnAuthorizedUrl;

            }
            else
            {
                //TODO: ADD logic for catching
                throw new ArgumentException("USER TYPE IS CORRUPTED");
            }
        }
    }
}
