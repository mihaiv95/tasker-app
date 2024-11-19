using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using tasker_app.DataLayer.DTOs.User;
using tasker_app.DataLayer.Utils;
using tasker_app.DBContexts;
using tasker_app.Models;
using tasker_app.ServiceLayer;
using tasker_app.ServiceLayer.UsersService;
using tasker_app.ServiceLayer.Utils;

namespace tasker_app.Controllers
{
    [ApiController]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/Users/Login")]
        public async Task<IActionResult> Authenticate(AuthenticateRequestDTO model)
        {
            var authenticatedUser = await _userService.Authenticate(model, ipAddress());
            if (authenticatedUser.Success)
            {
                setTokenCookie(authenticatedUser.Response.RefreshToken);
                switch (authenticatedUser.Response.UserType)
                {
                    case UserType.AdminUser:
                        return Ok(new { Response = (AdminUserDTO)authenticatedUser.Response, Message = authenticatedUser.Message, Success = authenticatedUser.Success });
                    case UserType.ExpertUser:
                        return Ok(new { Response = (ExpertUserDTO)authenticatedUser.Response, Message = authenticatedUser.Message, Success = authenticatedUser.Success });
                    case UserType.CompanyUser:
                        return Ok(new { Response = (CompanyUserDTO)authenticatedUser.Response, Message = authenticatedUser.Message, Success = authenticatedUser.Success });
                    default:
                        return Ok(new { Response = (string)null, Message = authenticatedUser.Message, Success = authenticatedUser.Success });
                }
            }
            else
            {
                return BadRequest(authenticatedUser);
            }
        }

        [HttpPost("/Users/RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _userService.RefreshToken(refreshToken, ipAddress());
            if (result.Success)
            {
                setTokenCookie(result.Response.RefreshToken);
                switch (result.Response.UserType)
                {
                    case UserType.AdminUser:
                        return Ok(new { Response = (AdminUserDTO)result.Response, Message = result.Message, Success = result.Success });
                    case UserType.ExpertUser:
                        return Ok(new { Response = (ExpertUserDTO)result.Response, Message = result.Message, Success = result.Success });
                    case UserType.CompanyUser:
                        return Ok(new { Response = (CompanyUserDTO)result.Response, Message = result.Message, Success = result.Success });
                    default:
                        return Ok(new { Response = (string)null, Message = result.Message, Success = result.Success });
                }
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/Users/RevokeToken")]
        public async Task<IActionResult> RevokeToken(string tokenToReset)
        {
            // accept token from request body or cookie
            var token = tokenToReset ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            // users can revoke their own tokens and admins can revoke any tokens
            if (!Account.OwnsToken(token) && Account.UserType != UserType.AdminUser)
                return Unauthorized(new { message = "Unauthorized" });

            var result = await _userService.RevokeToken(token, ipAddress());
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterAdminUserDTO model)
        {
            var result = await _userService.RegisterAdminUser(model, Request.Headers["origin"]);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            var result = await _userService.VerifyEmail(token);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _userService.ForgotPassword(email, Request.Headers["origin"]);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/ValidateResetToken")]
        public async Task<IActionResult> ValidateResetToken([FromQuery] string token)
        {
            var result = await _userService.ValidateResetToken(token);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("/Users/ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            var result = await _userService.ResetPassword(model);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize(UserType.AdminUser)]
        [HttpGet("/Users/GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userService.GetAll();
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize]
        [HttpGet("/Users/GetUser")]
        public async Task<IActionResult> GetById(int id)
        {
            // users can get their own account and admins can get any account
            //if (id != Account.UserId && Account.UserType != UserType.AdminUser)
            //    return Unauthorized(new { message = "Unauthorized" });

            var result = await _userService.GetById(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize(UserType.AdminUser)]
        [HttpPost("/Users/CreateUser")]
        public async Task<IActionResult> Create(ManualUserCreationDTO model)
        {
            var result = await _userService.Create(model);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize]
        [HttpPost("/Users/UpdateUser")]
        public async Task<IActionResult> Update(UpdateUserDTO model)
        {
            int userLoggedInId = 0;
            if (Account != null)
            {
                userLoggedInId = Account.UserId; 
            }
            // users can update their own account and admins can update any account
            if (Account!=null && model.UserId != Account.UserId && Account.UserType != UserType.AdminUser && Account.UserType != UserType.ExpertUser)
                return Unauthorized(new { message = "Unauthorized" });

            var result = await _userService.Update(model,userLoggedInId);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [Authorize(UserType.AdminUser)]
        [HttpPost("/Users/DeleteUser")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            // users can delete their own account and admins can delete any account
            if (Account!=null && id != Account.UserId && Account.UserType != UserType.AdminUser)
                return Unauthorized(new { message = "Unauthorized" });

            var result = await _userService.Delete(id);
            if (result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        // helper methods

        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = GlobalFunctions.GetCurrentDateTime().AddDays(7),
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
