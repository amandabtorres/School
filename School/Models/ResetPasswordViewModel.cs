﻿using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
