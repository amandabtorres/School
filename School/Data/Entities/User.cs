﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace School.Data.Entities
{
    public class User :IdentityUser
    {
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters.")]
        public string LastName { get; set; }


        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime DateBirth { get; set; }

        public string Address { get; set; }


        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        public string Nif { get; set; }

        public Guid ImageId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://schoolprojectweb.azurewebsites.net/images/users/noimage.png"
            : $"https://schoolprojectweb.blob.core.windows.net/users/{ImageId}";
        

    }
}
