using Microsoft.AspNetCore.Mvc;
using tasker_app.DataLayer.DTOs.User;
using tasker_app.DataLayer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tasker_app.ServiceLayer.UsersService
{
    public interface IUserService
    {
        Task<ServiceResponse<BaseUserDTO>> Authenticate(AuthenticateRequestDTO model, string ipAddress);
        Task<ServiceResponse<BaseUserDTO>> RefreshToken(string token, string ipAddress);
        Task<ServiceResponse<Object>> RevokeToken(string token, string ipAddress);
        Task<ServiceResponse<Object>> RegisterAdminUser(RegisterAdminUserDTO model, string origin);
        
        Task<ServiceResponse<Object>> VerifyEmail(string token);
        Task<ServiceResponse<Object>> ForgotPassword(string email, string origin);
        Task<ServiceResponse<Object>> ValidateResetToken(string token);
        Task<ServiceResponse<Object>> ResetPassword(ResetPasswordDTO model);
        Task<ServiceResponse<IEnumerable<BaseUserDTO>>> GetAll();
        Task<ServiceResponse<Object>> GetById(int id);
        Task<ServiceResponse<BaseUserDTO>> Create(ManualUserCreationDTO model);
        Task<ServiceResponse<BaseUserDTO>> Update( UpdateUserDTO model, int userLoggedInId);
        Task<ServiceResponse<Object>> Delete(int id);

    }
}
