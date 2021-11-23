using System.ComponentModel.DataAnnotations;

namespace BlazorBattles.Shared
{
    public class UserLogin
    {
        [Required(ErrorMessage ="Please, enter user name.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please, enter password.")]
        public string Password { get; set; }
    }
}
