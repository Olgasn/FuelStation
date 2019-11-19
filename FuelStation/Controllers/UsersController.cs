using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FuelStation.Models;
using FuelStation.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FuelStation.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
     
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;

        public UsersController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.OrderBy(user => user.Id);

            List<UserViewModel> userViewModel = new List<UserViewModel>();

            string urole = "";
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Count() > 0)
                {
                    urole = userRoles[0] ?? "";
                }

                userViewModel.Add(
                    new UserViewModel
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        RegistrationDate = user.RegistrationDate,
                        RoleName = urole

                    });

            }

            return View(userViewModel);
        }

        public IActionResult Create()
        {
            var allRoles = _roleManager.Roles.ToList();
            CreateUserViewModel user = new CreateUserViewModel();

            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name");

            return View(user);

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    RegistrationDate = model.RegistrationDate,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
               
                var role = model.UserRole;
                if (role.Count() > 0)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // �������� ������ ����� ������������
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = _roleManager.Roles.ToList();
            string userRole = "";
            if (userRoles.Count() > 0)
            {
                userRole = userRoles[0] ?? "";
            }

            EditUserViewModel model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                RegistrationDate = user.RegistrationDate,
                UserRole = userRole
            };
            ViewData["UserRole"] = new SelectList(allRoles, "Name", "Name", model.UserRole);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    // �������� � ������� ������� ���� ������������
                    var oldRoles = await _userManager.GetRolesAsync(user);

                    if (oldRoles.Count() > 0)
                    {
                        await _userManager.RemoveFromRolesAsync(user, oldRoles);

                    }
                    // �������� � ������������� ����� ���� ������������
                    var newRole = model.UserRole;
                    if (newRole.Count() > 0)
                    {
                        await _userManager.AddToRoleAsync(user, newRole);
                    }
                    user.Email = model.Email;
                    user.UserName = model.UserName;
                    user.RegistrationDate = model.RegistrationDate;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}