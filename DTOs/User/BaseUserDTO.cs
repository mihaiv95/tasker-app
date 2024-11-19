using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tasker_app.DataLayer.DTOs.User
{
    public class BaseUserDTO
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string JwtToken { get; set; }
        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string Created { get; set; }
        public string? Updated { get; set; }
        public UserType UserType { get; set; }
        public bool IsVerified { get; set; }
    }
}
