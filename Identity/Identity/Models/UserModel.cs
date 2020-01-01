using Identity.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Identity.Models
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }



    public class Register
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }

    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> members { get; set; }
        public IEnumerable<ApplicationUser> NoNmembers { get; set; }
    }

    public class RoleUpdateModel
    {
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}