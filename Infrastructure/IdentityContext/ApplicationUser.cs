using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IdentityContext
{
    public class ApplicationUser : IdentityUser
    {
        public int DbId { get; set; }
    }
}
