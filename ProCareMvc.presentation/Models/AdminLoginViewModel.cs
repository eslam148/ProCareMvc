using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class AdminLoginViewModel
    {
        [Required]
        public string UserName { get; set; } 

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } 
    }
}
