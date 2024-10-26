using CompanyAdminstrationMVC.DAL.Models;
using CompanyAdminstrationMVC.PL.Helpers;
using CompanyAdminstrationMVC.PL.ViewModels.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyAdminstrationMVC.PL.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}


		#region SignUp
		[HttpGet]
		public IActionResult SignUp()
		{


			return View();


		}
		[HttpPost]
		public async Task<IActionResult> SignUp(SignUpViewModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					// Check if the username exists
					var user = await _userManager.FindByNameAsync(model.UserName);
					if (user != null)
					{
						ModelState.AddModelError(string.Empty, "Username already exists !!");
						return View(model); // Return early if the username is taken
					}

					// Check if the email exists
					user = await _userManager.FindByEmailAsync(model.Email);
					if (user != null)
					{
						ModelState.AddModelError(string.Empty, "Email already exists !!");
						return View(model); // Return early if the email is taken
					}

					// If both username and email are valid, proceed with user creation
					user = new AppUser()
					{
						UserName = model.UserName,
						FirstName = model.FirstName,
						LastName = model.LastName,
						Email = model.Email,
						IsAgree = model.IsAgree,
					};

					var result = await _userManager.CreateAsync(user, model.Password);

					if (result.Succeeded)
					{
						return RedirectToAction(nameof(SignIn));
					}

					// If creation fails, add errors to the ModelState
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
				}
			}

			return View(model);


		}
		#endregion


		#region SignIn
		[HttpGet]
		public IActionResult SignIn()
		{


			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{

			if (ModelState.IsValid)
			{

				try
				{

					var user = await _userManager.FindByEmailAsync(model.Email);
					if (user is not null)
					{
						var Flag = await _userManager.CheckPasswordAsync(user, model.Password);
						if (Flag)
						{
							var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
							if (result.Succeeded)
							{

								return RedirectToAction("Index", "Home");

							}

						}


					}
					ModelState.AddModelError(string.Empty, "Invalid Login !!");


				}
				catch (Exception ex)
				{

					ModelState.AddModelError(string.Empty, ex.Message);

				}
			}





			return View(model);
		}
		#endregion

		#region SignOut
		public async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
		#endregion

		#region ForgetPassword
		[HttpGet]
		public IActionResult ForgetPassword()
		{

			return View();


		}
		[HttpPost]
		public async Task<IActionResult> SendResetPasswordUrl(ForgetPasswordViewModel model)
		{

			if (ModelState.IsValid)
			{
				try
				{
					var user = await _userManager.FindByEmailAsync(model.Email);
					if (user is not null)
					{

						// Create Token 
						var token = await _userManager.GeneratePasswordResetTokenAsync(user);


						// Create reset Password URL
						var URL = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, Request.Scheme);

						var email = new Email()
						{
							To = model.Email,
							Subject = "Reset Password",
							Body = URL

						};
						EmailSettings.SendEmail(email);
						return RedirectToAction("CheckYourEmail");
					}
					ModelState.AddModelError(string.Empty, "Invalid Operation , Please Try Again");

				}
				catch (Exception ex)
				{

					ModelState.AddModelError(string.Empty, ex.Message);

				}



			}
			return View(model);
		}
		[HttpGet]
		public IActionResult CheckYourEmail()
		{

			return View();

		}
		#endregion

		#region ResetPassword
		[HttpGet]
		public IActionResult ResetPassword(string email, string token)
		{
			TempData["email"] = email;
			TempData["token"] = token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				if (user is not null)
				{
					var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
					if (result.Succeeded)
					{

						return RedirectToAction(nameof(SignIn));
					}
				}
			}
			ModelState.AddModelError(string.Empty, "Invalid Operation , Please Try Again");

			return View(model);
		}
		#endregion


		public IActionResult AccessDenied() 
		{
			return View();
		}


	}
}