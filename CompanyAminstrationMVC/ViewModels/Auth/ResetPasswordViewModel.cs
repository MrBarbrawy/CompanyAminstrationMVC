using System.ComponentModel.DataAnnotations;

namespace CompanyAdminstrationMVC.PL.ViewModels.Auth
{
	public class ResetPasswordViewModel
	{


		[Required(ErrorMessage = "Password is Required !!")]

		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "ConfirmPassword is Required !!")]

		[DataType(DataType.Password)]

		[Compare(nameof(Password), ErrorMessage = "Passwords dont match !!")]
		public string ConfirmPassword { get; set; }


	}
}
