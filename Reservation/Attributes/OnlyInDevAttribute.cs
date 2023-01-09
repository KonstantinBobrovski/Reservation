using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Reservation.consts;
using System.Security.Claims;

namespace Reservation.Attributes
{

    /// <summary>
    /// Page will be visible only when ASPNET_ENVIROMENT is dev
    /// </summary>
    public class OnlyInDevAttribute : TypeFilterAttribute
    {

        public OnlyInDevAttribute() : base(typeof(OnlyInDevFilter))
        {

        }
    }

    public class OnlyInDevFilter : IAuthorizationFilter
    {


        public OnlyInDevFilter()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var isInDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (!isInDev)
            {
                context.Result = new ForbidResult();

            }
        }
    }
}
