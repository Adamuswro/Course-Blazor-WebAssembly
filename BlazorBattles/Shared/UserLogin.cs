using System.ComponentModel.DataAnnotations;

namespace BlazorBattles.Shared
{
    public class UserLogin
    {
        [Required(ErrorMessage ="Please, enter an email adress.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please, enter password.")]
        public string Password { get; set; }
    }
}
