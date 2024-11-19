using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class CompanyUsersPaginatedDTO
    {
        public int RowCount { get; set; }
        public List<SimpleUserDTO> Users { get; set; }
    }
}
