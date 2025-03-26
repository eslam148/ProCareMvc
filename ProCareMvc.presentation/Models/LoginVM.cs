using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class LoginVM
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
