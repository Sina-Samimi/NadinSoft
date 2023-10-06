﻿using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class UserLogin
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
