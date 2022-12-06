using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace IdentityProject.Models
{
    public class AppIdentityDbContext:IdentityDbContext<AppUser,AppRole,string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options)//DbContextOptions 'ı AppIdentityDbContext içindeki base class'a gönderdik ilk olarak|Default Constructor
        {

        }
    }
}
