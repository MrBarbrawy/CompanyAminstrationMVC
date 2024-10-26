using AutoMapper;
using CompanyAdminstrationMVC.DAL.Models;
using CompanyAdminstrationMVC.PL.Helpers;
using CompanyAdminstrationMVC.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using CompanyAdminstrationMVC.PL.Mapping.User;
using Microsoft.AspNetCore.Authorization;
namespace C42_G01_MVC04.PL.Controllers
{

    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;


        public UserController(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;

        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Index(string searchInput)
        {
            var users = Enumerable.Empty<UserViewModel>();

            if (string.IsNullOrEmpty(searchInput))
            {
                users = _userManager.Users.Select(U => new UserViewModel()
                {
                    Id = U.Id,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).ToList();
            }
            else
            {
                users = _userManager.Users.Where(U => U.Email
                                          .ToLower()
                                          .Contains(searchInput.ToLower()))
                                          .Select(U => new UserViewModel()
                                          {
                                              Id = U.Id,
                                              FirstName = U.FirstName,
                                              LastName = U.LastName,
                                              Email = U.Email,
                                              Roles = _userManager.GetRolesAsync(U).Result

                                          }).ToList();
            }


            return View(users);
        }

        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();

            }
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return NotFound();
            }
            var result = _mapper.Map<UserViewModel>(user);

            return View(viewName, result);
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {

            return await Details(id, "Update");

        }


        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model)
        {
            try
            {
                if (id != model.Id)
                    return BadRequest();


                if (ModelState.IsValid)
                {

                    var user = await _userManager.FindByIdAsync(id);

                    if (user is null)
                        return NotFound();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);

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


        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public Task<IActionResult> Delete(string? id)
        {
            return Details(id, "Delete");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model)
        {
            try
            {
                if (id != model.Id)
                    return BadRequest();
                if (ModelState.IsValid)
                {
                    var user = _mapper.Map<AppUser>(model);

                    user = await _userManager.FindByIdAsync(id);

                    if (user is null)
                        return NotFound();

                    var result = await _userManager.DeleteAsync(user);
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
        public async Task<IActionResult> ShowProfile(string? id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); // Handle if id is not provided
            }

            // Fetch user from UserManager
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // Handle user not found
            }
            var result = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            };


            return View(result);


        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id) 
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); 
            }

            // Fetch user from UserManager
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); 
            }
            var result = new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            };

            return View(result);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserViewModel model) 
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound(); 
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ShowProfile), new { id = model.Id });
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);

        }


    }
}
