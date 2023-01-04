using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Reservation.consts;
using System.Security.Claims;

namespace Reservation.Attributes.ActionFilters
{
    public class UserTypeAuthFilter: ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
          var user=  actionContext.HttpContext.User;
            if (user==null || !user.Identity.IsAuthenticated)
            {
               
            }

            var endpointMetadata = actionContext.ActionDescriptor.EndpointMetadata;
            
            Console.WriteLine("dasd");
        }
    }
}
