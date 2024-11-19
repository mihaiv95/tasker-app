using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class CompanyUsersFiltersDTO
    {
        public int PageNumber { get; set; }
        public DataGridColumnNames ColumnToSortBy { get; set; }
        public bool SortingOrder { get; set; }
    }
}
