using System.ComponentModel.DataAnnotations;

namespace ProCareMvc.presentation.Models
{
    public class ErrorViewModel
    {
        [Required]
        [MaxLength(255)]
        [MinLength(100)]
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
