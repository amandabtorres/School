﻿using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class RecoverPasswordViewModel
    {

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
