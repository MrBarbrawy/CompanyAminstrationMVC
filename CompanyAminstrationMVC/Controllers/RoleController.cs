using AutoMapper;
using CompanyAdminstrationMVC.DAL.Models;
using  CompanyAdminstrationMVC.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace C42_G01_MVC04.PL.Controllers
{
    [Authorize(Roles = "ADMIN")]
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;


        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(string searchInput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(searchInput))
            {
                roles = _roleManager.Roles.Select(R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name,

                }).ToList();
            }
            else
            {
                roles = _roleManager.Roles.Where(U => U.Name
                                          .ToLower()
                                          .Contains(searchInput.ToLower()))
                                          .Select(R => new RoleViewModel()
                                          {
                                              Id = R.Id,
                                              RoleName = R.Name

                                          }).ToList();
            }


            return View(roles);
        }


        public async Task<IActionResult> Create()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var role = new IdentityRole()
                    {
                        Name = model.RoleName,
                    };

                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }


            return View(model);
        }




        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();

            }
            var roles = await _roleManager.FindByIdAsync(id);

            if (roles is null)
            {
                return NotFound();
            }

            var roles0 = new RoleViewModel()
            {
                Id = roles.Id,
                RoleName = roles.Name
            };

            return View(viewName, roles0);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Update");

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model)
        {
            try
            {
                if (id != model.Id)
                    return BadRequest();


                if (ModelState.IsValid)
                {

                    var role = await _roleManager.FindByIdAsync(id);

                    if (role is null)
                        return NotFound();
                    role.Name = model.RoleName;

                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {

                        return RedirectToAction(nameof(Index));

                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }



        [HttpGet]
        public Task<IActionResult> Delete(string? id)
        {
            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model)
        {
            try
            {
                if (id != model.Id)
                    return BadRequest();
                if (ModelState.IsValid)
                {


                    var role = await _roleManager.FindByIdAsync(id);

                    if (role is null)
                        return NotFound();

                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index");

                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleID) 
        {
          var role = await  _roleManager.FindByIdAsync(roleID);
          if (role is null) 
            return BadRequest();

            ViewData["RoleId"] = roleID;
            var usersInRole = new List<UserInRoleViewModel>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users) 
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                if (await _userManager.IsInRoleAsync(user , role.Name)) 
                {
                    userInRole.IsSelected = true;


                }else { userInRole.IsSelected = false; }
                usersInRole.Add(userInRole);
                
            }
            

            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string roleID,List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleID);
            if (role is null)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var user in users)
                    {
                        var userFromDb = await _userManager.FindByIdAsync(user.UserId);
                        if (userFromDb is not null) 
                        { 
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(userFromDb, role.Name))
                        {
                          await  _userManager.AddToRoleAsync(userFromDb ,role.Name );

                        } else if (! user.IsSelected && await _userManager.IsInRoleAsync(userFromDb, role.Name)) 
                        {
                           await _userManager.RemoveFromRoleAsync(userFromDb,role.Name);                        
                        }  
                        }                                                        
                    }
                    return RedirectToAction(nameof(Edit),new {id = roleID });
                }
                catch (Exception ex)
                { 
                    ModelState.AddModelError(string.Empty, ex.Message);
                }   
            }
            return View(users);
        }




    }
}
