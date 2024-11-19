using Microsoft.EntityFrameworkCore;
using tasker_app.Models;
using tasker_app.ServiceLayer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.Utils
{
    [Owned]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public BaseUser Account { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => GlobalFunctions.GetCurrentDateTime() >= Expires;
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
    }
}
