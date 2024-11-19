using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class AdminUserDTO : BaseUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
