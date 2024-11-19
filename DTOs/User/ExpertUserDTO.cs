using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class ExpertUserDTO : BaseUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NumarTelefon { get; set; }
        public ExpertType ExpertType { get; set; }
        public string Adresa { get; set; }
        public Judete? Judet { get; set; }
        public Zone? Zona { get; set; }
        public DateTime? DataNasterii { get; set; }
        public string? Gen { get; set; }
    }
}
