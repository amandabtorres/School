using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class UserViewModel
    {       
        
        [Required]
        [Display(Name = "First Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string LastName { get; set; }


        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime DateBirth { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }


        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Address { get; set; }


        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "The field {0} only can contain {1} characters length.")]
        public string Nif { get; set; }
    }
}
