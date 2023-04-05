using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Role
{
    public class RolePageModel:PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly AppDbContext _appDbContext;
        public RolePageModel(RoleManager<IdentityRole> roleManager , AppDbContext appDbContext)
        {
            _roleManager =  roleManager;
            _appDbContext =  appDbContext;
        }
    }
}