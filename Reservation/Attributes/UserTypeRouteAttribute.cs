using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Reservation.consts;
using System.Security.Claims;

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
            
            var userType = context.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;

            if (userType == null)
            {
                context.Result=new ForbidResult();

            }
            else if (Enum.TryParse(userType, out UserTypeEnum userTypeAsEnum))
            {
                if (userTypeAsEnum != this.UserType)
                    context.Result = new ForbidResult();

            }
            else
            {
                //TODO: ADD logic for catching
                throw new ArgumentException("USER TYPE IS CORRUPTED");
            }
        }
    }
}
