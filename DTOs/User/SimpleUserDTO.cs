using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class SimpleUserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string CreatedAt { get; set; }
        public string? Updated { get; set; }
        public UserType UserType { get; set; }
        public bool IsVerified { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CreatedByUserId { get; set; }
        public int UpdatedByUserId { get; set; }
        public string NumarTelefon { get; set; }
    }
}
