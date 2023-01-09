using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Reservation.Attributes;
using System.Text;

namespace Reservation.Areas.Developer.Controllers
{
    [Area("Developer")]
    [OnlyInDevAttribute]
    public class HomeController : Controller
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public HomeController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        
        public IActionResult Index()
        {
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Where(ad => ad.AttributeRouteInfo != null).OrderBy(x => x.AttributeRouteInfo?.Template).ToList();

            // build response content
            var sb = new StringBuilder();
            sb.Append($@"<html><head><meta charset='utf-8'><title>Routes</title>
                <style>
                p, li {{
                    font-family: 'Verdana', sans-serif;
                    font-weight: 600;
                    margin: 5px;
                }}
                .val {{
                        font-family: 'Courier New', Courier, monospace;
                }}
                </style>
                </style></head><body>");
            sb.Append($"<p>{routes.Count} routes found<p>");
            sb.Append("<ul>");

            foreach (var route in routes)
            {
                if (route is null || route.ActionConstraints is null)
                    continue;

                var allowedMethods = string.Empty;
                foreach (var actionConstraintMetadata in route.ActionConstraints)
                {
                    if (actionConstraintMetadata is HttpMethodActionConstraint acm)
                    {
                        allowedMethods = string.Join(", ", acm.HttpMethods);
                    }
                }
                route.RouteValues.TryGetValue("action", out var action);
                route.RouteValues.TryGetValue("controller", out var controller);
                sb.Append($"<li><span class='val'>{route.AttributeRouteInfo?.Template}</span>" +
                    $" Controller: <span class='val'>{controller}</span>" +
                    $" Action: <span class='val'>{action}</span>" +
                    $" AllowedMethods: <span class='val'>{allowedMethods}</span>"+
                    $" Link: <a href=\"{route.AttributeRouteInfo?.Template?.Replace("{action}",action).Replace("{controller}",controller)}\">{route?.AttributeRouteInfo?.Template?.Replace("{action}", action).Replace("{controller}", controller)} </a> </li>");
            }
            sb.Append("</ul></body></html>");
            var content = sb.ToString();

            return new ContentResult
            {
                Content = content,
                ContentType = "text/html"
            };
        }
    }
}
