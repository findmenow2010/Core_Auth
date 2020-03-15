﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthCoreApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthCoreApp.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(ViewModels.CreateRoleVM _model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole {
                    Name = _model.RoleName
                };

                var res = await roleManager.CreateAsync(role);
                if (res.Succeeded) { return RedirectToAction("Rolelist","Administration"); }

                foreach (var err in res.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(_model);
        }

        [HttpGet]
        public IActionResult Rolelist()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> editRole(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var role = await roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    ModelState.AddModelError("",$"Role with ID{id} was not found.");
                }
                var data = new ViewModels.EditRoleVm {
                    Id = role.Id,
                    RoleName = role.Name,
                    Users = new List<string>()
                };

                foreach (var user in userManager.Users)
                {
                    if (await userManager.IsInRoleAsync(user, role.Name))
                    {
                        data.Users.Add(user.UserName);
                    }
                }

                return View(data);
            }
            else
            {
                return RedirectToAction("RoleList");
            }
           
        }
        [HttpPost]
        public async Task<IActionResult> editRole(EditRoleVm role)
        {
            if (ModelState.IsValid)
            {
                var oldrole = await roleManager.FindByIdAsync(role.Id);

                if (oldrole == null) { ModelState.AddModelError("", "Rold Not Found"); return View("NotFound"); }
                else { oldrole.Name = role.RoleName; }

                var res =  await roleManager.UpdateAsync(oldrole);

                if (res.Succeeded) {
                    // Update Role Users

                    return RedirectToAction("RoleList", "Administration");
                }
                else {
                    foreach (var err in res.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                    return View(role);
                }
            }
            return View(role);
        }

        [HttpPost]
        public IActionResult deleteRole()
        { return View(); }


        [HttpGet]
        public IActionResult EditUsersInRole(string RoleID)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole()
        {
            return RedirectToAction("EditRole");
        }


    }
}