using System.ComponentModel.DataAnnotations;

namespace CompanyAdminstrationMVC.PL.ViewModels.Auth
{
    public class ForgetPasswordViewModel
    {

        [Required(ErrorMessage = "Email is Required !!")]

        [EmailAddress(ErrorMessage = "Email is invalid")]

        public string Email { get; set; }

    }
}
