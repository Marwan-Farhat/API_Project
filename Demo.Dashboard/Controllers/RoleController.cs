using Demo.Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Dashboard.Controllers
{
	public class RoleController(RoleManager<IdentityRole> _roleManager) : Controller
	{
		public async Task<IActionResult> Index()
		{
			// Get all roles
			var roles = await _roleManager.Roles.ToListAsync();
			return View(roles);
		}

		[HttpPost]
		public async Task<IActionResult> Create(RoleFormViewModel model)
		{
			if (ModelState.IsValid)
			{
				// To check if the role already exist
				var roleExists = await _roleManager.RoleExistsAsync(model.Name);

				if (!roleExists)
				{
					await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
					return RedirectToAction(nameof(Index));
				}
				else
				{
					ModelState.AddModelError("Name", "Role is already exist");
					return View("Index", await _roleManager.Roles.ToListAsync());
				}
			}
			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Edit(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			var mappedrole = new RoleViewModel()
			{
				Name = role.Name
			}; 

			return View(mappedrole);
		}

        [HttpPost]
        public async Task<IActionResult> Edit(string id,RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                // To check if the role already exist
                var roleExists = await _roleManager.RoleExistsAsync(model.Name);

                if (!roleExists)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);
					role.Name =model.Name;
					await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Name", "Role is already exist");
                    return View("Index", await _roleManager.Roles.ToListAsync());
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
		{
			var role = await _roleManager.FindByIdAsync(id);
			await _roleManager.DeleteAsync(role);
			return RedirectToAction(nameof(Index));
		}
	
	}
}
