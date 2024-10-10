using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class CreatePasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}
