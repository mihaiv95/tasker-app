using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class ApproveGrantDTO
    {
        public int CompanyUserId { get; set; }
        public string DataInregistrare { get; set; }
        public string NumarInregistrare { get; set; }
        public int GrantApprovedByExpertUserId { get; set; }
    }
}
