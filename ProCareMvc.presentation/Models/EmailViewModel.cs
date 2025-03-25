using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class EmailViewModel
    {
        [Required, EmailAddress]
        public string To { get; set; }

       

        [Required]
        public string Body { get; set; }
    }
}
