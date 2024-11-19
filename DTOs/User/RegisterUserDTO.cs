using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class RegisterUserDTO
    {
        
        public string CompanyName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public int ExpertUserId { get; set; }

        public string NumarTelefon { get; set; }
        public string? DataRecrutare { get; set; }
        public string CNP { get; set; }
        public string Adresa { get; set; }
        public Judete? Judet { get; set; }
        public Zone? Zona { get; set; }
        public TipuriStudii? Studii { get; set; }
        public StatuturiAngajare? Statut { get; set; }
        public string? DataNasterii { get; set; }
        public string? Gen { get; set; }
        public TipCurs? Curs { get; set; }
        public string GrupaCurs { get; set; }
        public StatusCurs? Finalizat { get; set; }
        public string? DataInformare { get; set; }
        public string Workshop { get; set; }
        public bool DepunerePlanAfaceri { get; set; }
        public decimal PunctajFaza1 { get; set; }
        public decimal PunctajFaza2 { get; set; }
        public decimal PunctajFaza3 { get; set; }
        public bool SelectatPlanAfaceri { get; set; }
        public decimal PunctajTotal { get; set; }
        public TipuriSubventie? TipSubventie { get; set; }
        public bool IsGrantApproved { get; set; }
    }
}
