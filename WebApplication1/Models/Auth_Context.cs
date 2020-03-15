using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthCoreApp.Models
{
    public class Auth_Context : IdentityDbContext
    {
        public Auth_Context(DbContextOptions<Auth_Context> options) : base(options)
        {

        }

   
    }
}
