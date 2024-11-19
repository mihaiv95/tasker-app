using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class RegisterExpertUserDTO
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string NumarTelefon { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Adresa { get; set; }
        public Judete? Judet { get; set; }
        public Zone? Zona { get; set; }
        public string? DataNasterii { get; set; }
        public string Gen { get; set; }
        public ExpertType ExpertType { get; set; }
    }
}
