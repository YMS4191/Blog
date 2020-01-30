using System;
using System.ComponentModel.DataAnnotations;

namespace UI.Areas.Admin.Models
{
    public class LoginVM
    {
        public LoginVM()
        {
        }

        [Required]
        [UIHint("email")]
        public string Email { get; set; }
         
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
