using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class RegisterVM
    {
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        
    }
}
