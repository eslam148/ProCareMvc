using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class ChangePasswordViewModel
    {

        [Required(ErrorMessage = "Old Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
      

        [Required(ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "New password must be at least 6 characters long.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }



        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmPassword { get; set; }


    }
}
