
namespace CompanyAdminstrationMVC.PL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        public string RoleName { get; set; }

        // Constructor to set default value for Id
        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
