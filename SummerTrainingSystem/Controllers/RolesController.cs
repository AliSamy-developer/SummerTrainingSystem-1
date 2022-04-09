﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SummerTrainingSystem.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SummerTrainingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("roles")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
       
        [HttpGet("new")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost("new")]
        public async Task<IActionResult> CreateRole(CreateRoleVM model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new() { Name = model.Name };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(ListRoles));
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet("")]
        public IActionResult ListRoles()
        {
            return View(_roleManager.Roles.ToList());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _roleManager.DeleteAsync(await _roleManager.FindByIdAsync(id));
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
