using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class UpdateUserDTO
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EnumDataType(typeof(UserType))]
        public string UserType { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string CompanyName { get; set; }
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
        public ExpertType ExpertType { get; set; }
        public bool IsGrantApproved { get; set; }
    }
}
