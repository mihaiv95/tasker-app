using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.Models.User
{
    public class AdminUser : BaseUser
    {
        public string SediuSocial { get; set; }
        public string CodFiscal { get; set; }
        public string NumarTelefon { get; set; }
        public string FaxNumber { get; set; }
        public string ContBancar { get; set; }
        public string NumeleBancii { get; set; }
        public string ReprezentantLegal { get; set; }
        public string FunctiaInCompanie { get; set; }
    }
}
