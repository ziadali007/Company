using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using Presentation_Layer.Dtos;
using Presentation_Layer.Helpers;

namespace Presentation_Layer.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index(string? Search)
        {
            IEnumerable<RoleToReturnDto> roles;
            if (Search is not null)
            {
                roles = _roleManager.Roles.Select(R => new RoleToReturnDto()
                {
                    Id = R.Id,
                    Name=R.Name

                }).Where(R => R.Name.ToLower().Contains(Search.ToLower()));

            }
            else
            {
                roles = _roleManager.Roles.Select(R => new RoleToReturnDto()
                {
                    Id = R.Id,
                    Name = R.Name

                });

            }
            return View(roles);
        }

        [HttpGet]

        public async Task<IActionResult> Create()
        {           
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto roleDto)
        {
            if (ModelState.IsValid)
            {
               var role = await _roleManager.FindByNameAsync(roleDto.Name);
                if (role is null)
                {
                    var newRole = new IdentityRole()
                    {
                        Name = roleDto.Name
                    };
                    var result = await _roleManager.CreateAsync(newRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
                ModelState.AddModelError("Name", "Role Name Already Exists");
            }
            return View(roleDto);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var roles = await _roleManager.FindByIdAsync(id);

            if (roles is null) return NotFound(new { StatusCode = 404, Message = $"Role With Id {id} Is Not Found" });

            var dto = new RoleToReturnDto()
            {
                Id = roles.Id,
                Name = roles.Name

            };
            return View(ViewName, dto);

        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto roleDto)
        {
            if (ModelState.IsValid)
            {
                if (id != roleDto.Id) return BadRequest("Invalid Operation");
                var role = await _roleManager.FindByIdAsync(id);

                if (role is null) return BadRequest("Invalid Operation");
                var roleResult=await _roleManager.FindByNameAsync(roleDto.Name);

                if (roleResult is null)
                {
                    role.Name = roleDto.Name;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }

                ModelState.AddModelError("Name", "Role Name Already Exists");


            }
            return View(roleDto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto roleDto)
        {
            if (ModelState.IsValid)
            {

                if (id != roleDto.Id) return BadRequest("Invalid Operation");
                var role = await _roleManager.FindByIdAsync(id);

                if (role is null) return BadRequest("Invalid Operation");
                var roleResult = await _roleManager.FindByNameAsync(roleDto.Name);

                
                role.Name = roleDto.Name;

                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                     return RedirectToAction("Index");
                }

                ModelState.AddModelError("Name", "Invalid Operation");


            }
            return View(roleDto);
        }

        [HttpGet]

        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role= await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound(new { StatusCode = 404, Message = $"Role With Id {roleId} Is Not Found" });

            ViewData["RoleId"] = roleId;
            var userInRole = new List<UsersInRoleViewModel>();
            var users =await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRoleViewModel = new UsersInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRoleViewModel.IsSelected = true;
                }
                else
                {
                    userInRoleViewModel.IsSelected = false;
                }
                userInRole.Add(userInRoleViewModel);
            }

            return View(userInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UsersInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound(new { StatusCode = 404, Message = $"Role With Id {roleId} Is Not Found" });

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser= await _userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser,role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }

                return RedirectToAction("Edit" , new {id= roleId });
            }
            return View(role);
        }

    }
}
