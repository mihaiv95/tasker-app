using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using tasker_app.DataLayer.DTOs.User;
using tasker_app.DataLayer.Utils;
using tasker_app.DBContexts;
using tasker_app.Models;
using tasker_app.Models.User;
using tasker_app.ServiceLayer.EmailService;
using tasker_app.ServiceLayer.UsersService;
using tasker_app.ServiceLayer.Utils;
using BC = BCrypt.Net.BCrypt;

namespace tasker_app.ServiceLayer
{
    public class UserService : IUserService
    {
        private readonly MainDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public UserService(MainDbContext context, 
            IMapper mapper, 
            IOptions<AppSettings> appSettings, 
            IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public async Task<ServiceResponse<IEnumerable<BaseUserDTO>>> GetAll()
        {
            try
            {
                var dbUsers = await _context.Users.ToListAsync();
                var response = _mapper.Map<List<BaseUserDTO>>(dbUsers);
                return new ServiceResponse<IEnumerable<BaseUserDTO>> { Response = response, Success = true };
            }
            catch (Exception e)
            {
                

                return new ServiceResponse<IEnumerable<BaseUserDTO>> { Response = null, Success = false, Message = Messages.Message_UsersLoadError };
            }
            
        }

        public async Task<ServiceResponse<BaseUserDTO>> Authenticate(AuthenticateRequestDTO model, string ipAddress)
        {
            try
            {
                var success = true;
                var account = await _context.Users.SingleOrDefaultAsync(x => x.Email == model.Email);
                var message = Messages.Message_LoggedInSuccessfully;

                if (account == null || !account.IsVerified || !BC.Verify(model.Password, account.Password))
                    throw new AppException("Email or password is incorrect");

                // authentication successful so generate jwt and refresh tokens
                var jwtToken = generateJwtToken(account);
                var refreshToken = generateRefreshToken(ipAddress);
                account.RefreshTokens.Add(refreshToken);

                // remove old refresh tokens from account
                removeOldRefreshTokens(account);

                // save changes to db
                _context.Update(account);
                _context.SaveChanges();

                var response = new BaseUserDTO();
                switch (account.UserType)
                {
                    case UserType.AdminUser:
                        response = _mapper.Map<AdminUserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = refreshToken.Token;
                        break;
                    case UserType.ExpertUser:
                        response = _mapper.Map<ExpertUserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = refreshToken.Token;
                        break;
                    case UserType.CompanyUser:
                        response = _mapper.Map<CompanyUserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = refreshToken.Token;
                        break;
                    default:
                        success = false;
                        message = Messages.Message_LoggedInError;
                        break;
                }

                return new ServiceResponse<BaseUserDTO> { Response = response, Success = success, Message = message };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<BaseUserDTO> { Response = null, Success = false, Message = Messages.Message_LoggedInError };
            }
        }

        public async Task<ServiceResponse<BaseUserDTO>> RefreshToken(string token, string ipAddress)
        {
            try
            {
                var (refreshToken, account) = await getRefreshToken(token);
                var success = false;
                var message = Messages.Message_RefreshedTokenSuccess;

                // replace old refresh token with a new one and save
                var newRefreshToken = generateRefreshToken(ipAddress);
                refreshToken.Revoked = GlobalFunctions.GetCurrentDateTime();
                refreshToken.RevokedByIp = ipAddress;
                refreshToken.ReplacedByToken = newRefreshToken.Token;
                account.RefreshTokens.Add(newRefreshToken);

                removeOldRefreshTokens(account);

                _context.Update(account);
                _context.SaveChanges();

                // generate new jwt
                var jwtToken = generateJwtToken(account);

                success = true;

                var response = new BaseUserDTO();
                switch (account.UserType)
                {
                    case UserType.AdminUser:
                        response = _mapper.Map<AdminUserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = newRefreshToken.Token;
                        break;
                    case UserType.ExpertUser:
                        response = _mapper.Map<ExpertUserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = newRefreshToken.Token;
                        break;
                    case UserType.CompanyUser:
                        response = _mapper.Map<CompanyUserDTO>(account);
                        response.JwtToken = jwtToken;
                        response.RefreshToken = newRefreshToken.Token;
                        break;
                    default:
                        success = false;
                        message = Messages.Message_RefreshedTokenError;
                        break;
                }

                return new ServiceResponse<BaseUserDTO> { Response = response, Success = success, Message = message };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<BaseUserDTO> { Response = null, Success = false, Message = Messages.Message_RefreshedTokenError };
            }
        }

        public async Task<ServiceResponse<Object>> RevokeToken(string token, string ipAddress)
        {
            try
            {
                var (refreshToken, account) = await getRefreshToken(token);
                var message = Messages.Message_RevokeTokenSuccess;

                // revoke token and save
                refreshToken.Revoked = GlobalFunctions.GetCurrentDateTime();
                refreshToken.RevokedByIp = ipAddress;
                _context.Update(account);
                _context.SaveChanges();

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = message };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_RevokeTokenError };
            }
        }
        public async Task<ServiceResponse<Object>> RegisterAdminUser(RegisterAdminUserDTO model, string origin)
        {
            try
            {
                var message = Messages.Message_UserRegisteredSuccess;

                // validate
                if (await _context.Users.AnyAsync(x => x.Email == model.Email))
                {
                    // send already registered error in email to prevent account enumeration
                    sendAlreadyRegisteredEmail(model.Email, origin);
                    return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_EmailAlreadyUsed };
                }

                // map model to new account object
                var account = _mapper.Map<AdminUser>(model);

                account.UserType = UserType.AdminUser;
                account.CreatedAt = GlobalFunctions.GetCurrentDateTime();
                account.VerificationToken = randomTokenString();

                // hash password
                account.Password = BC.HashPassword(model.Password);

                // save account
                _context.Users.Add(account);
                _context.SaveChanges();

                // send email
                sendVerificationEmail(account, origin);

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = message };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_UserRegisterError };
            }
        }
        public async Task<ServiceResponse<Object>> VerifyEmail(string token)
        {
            try
            {
                var account = await _context.Users.SingleOrDefaultAsync(x => x.VerificationToken == token);

                if (account == null) throw new AppException("Verification failed");

                account.Verified = TimeZoneInfo.ConvertTime(GlobalFunctions.GetCurrentDateTime(), TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time"));
                account.VerificationToken = null;

                _context.Users.Update(account);
                _context.SaveChanges();

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_EmailVerified };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_EmailVerifyError };
            }
        }

        public async Task<ServiceResponse<Object>> ForgotPassword(string email, string origin)
        {
            try
            {
                var account = await _context.Users.SingleOrDefaultAsync(x => x.Email == email);

                // always return ok response to prevent email enumeration
                if (account == null) return new ServiceResponse<Object> { Response = (string)null, Success = false }; ;

                // create reset token that expires after 1 day
                account.ResetToken = randomTokenString();
                account.ResetTokenExpires = GlobalFunctions.GetCurrentDateTime().AddDays(1);

                _context.Users.Update(account);
                _context.SaveChanges();

                // send email
                sendPasswordResetEmail(account, origin);

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_ForgottenPasswordEmailSent };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_ForgottenPasswordEmailNotSent };
            }
        }

        public async Task<ServiceResponse<Object>> ValidateResetToken(string token)
        {
            try
            {
                var account = await _context.Users.SingleOrDefaultAsync(x =>
                        x.ResetToken == token &&
                        x.ResetTokenExpires > GlobalFunctions.GetCurrentDateTime());

                if (account == null)
                    throw new AppException("Invalid token");

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_ValidateResetTokenSuccess };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<object> { Response = null, Success = false, Message = Messages.Message_ValidateResetTokenError };
            }
        }

        public async Task<ServiceResponse<Object>> ResetPassword(ResetPasswordDTO model)
        {
            try
            {
                var account = await _context.Users.SingleOrDefaultAsync(x =>
                        x.ResetToken == model.Token &&
                        x.ResetTokenExpires > GlobalFunctions.GetCurrentDateTime());

                if (account == null)
                    throw new AppException("Invalid token");

                // update password and remove reset token
                account.Password = BC.HashPassword(model.Password);
                account.PasswordReset = GlobalFunctions.GetCurrentDateTime();
                account.ResetToken = null;
                account.ResetTokenExpires = null;

                _context.Users.Update(account);
                _context.SaveChanges();

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_ResetPasswordSuccess };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_ResetPasswordError };
            }
        }

        public async Task<ServiceResponse<Object>> GetById(int id)
        {
            try
            {
                var account = await getAccount(id);

                if (account == null)
                {
                    return new ServiceResponse<object> { Response = null, Success = false, Message = Messages.Message_UserDataLoadError };
                }

                var response = new BaseUserDTO();

                switch (account.UserType)
                {
                    case UserType.AdminUser:
                        response = _mapper.Map<AdminUserDTO>(account);
                        break;
                    case UserType.ExpertUser:
                        response = _mapper.Map<ExpertUserDTO>(account);
                        response.Created = GlobalFunctions.ParseDateTime(account.CreatedAt);
                        response.Updated = GlobalFunctions.ParseNullableDateTime(account.Updated);
                        break;
                    case UserType.CompanyUser:
                        response = _mapper.Map<CompanyUserDTO>(account);
                        response.Created = GlobalFunctions.ParseDateTime(account.CreatedAt);
                        response.Updated = GlobalFunctions.ParseNullableDateTime(account.Updated);
                        break;
                    default:
                        return new ServiceResponse<object> { Response = null, Success = false, Message = Messages.Message_UserDataLoadError };
                        break;
                }

                return new ServiceResponse<object> { Response = response, Success = true };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<object> { Response = null, Success = false, Message = Messages.Message_UserDataLoadError };
            }
        }

        public async Task<ServiceResponse<BaseUserDTO>> Create(ManualUserCreationDTO model)
        {
            try
            {
                // validate
                if (await _context.Users.AnyAsync(x => x.Email == model.Email))
                    throw new AppException($"Email '{model.Email}' is already registered");

                // map model to new account object
                var account = _mapper.Map<BaseUser>(model);
                account.CreatedAt = GlobalFunctions.GetCurrentDateTime();
                account.Verified = GlobalFunctions.GetCurrentDateTime();

                // hash password
                account.Password = BC.HashPassword(model.Password);

                // save account
                _context.Users.Add(account);
                _context.SaveChanges();

                var result = _mapper.Map<BaseUserDTO>(account);

                return new ServiceResponse<BaseUserDTO> { Response = result, Success = true, Message = Messages.Message_UserRegisteredSuccess };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<BaseUserDTO> { Response = null, Success = false, Message = Messages.Message_UserRegisterError };
            }

        }

        public async Task<ServiceResponse<BaseUserDTO>> Update(UpdateUserDTO model, int userLoggedInId)
        {
            try
            {
                var account = await getAccount(model.UserId);

                // validate
                if (account.Email != model.Email && _context.Users.Any(x => x.Email == model.Email))
                    throw new AppException($"Email '{model.Email}' is already taken");

                // copy model to account and save
                _mapper.Map(model, account);
                
                account.Updated = GlobalFunctions.GetCurrentDateTime();
                _context.Users.Update(account);
                _context.SaveChanges();

                var result = _mapper.Map<BaseUserDTO>(account);

                return new ServiceResponse<BaseUserDTO> { Response = result, Success = true, Message = Messages.Message_UserUpdateSuccess };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<BaseUserDTO> { Response = null, Success = false, Message = Messages.Message_UserUpdateError };
            }
        }

        public async Task<ServiceResponse<Object>> Delete(int id)
        {
            try
            {
                var account = await getAccount(id);
                _context.Users.Remove(account);
                _context.SaveChanges();

                return new ServiceResponse<Object> { Response = (string)null, Success = true, Message = Messages.Message_UserDeleteSuccess };
            }
            catch (Exception e)
            {
                
                return new ServiceResponse<Object> { Response = (string)null, Success = false, Message = Messages.Message_UserDeleteError };
            }
        }
        private async Task<BaseUser> getAccount(int id)
        {
            var account = await _context.Users.FindAsync(id);
            if (account == null) throw new KeyNotFoundException("Account not found");
            return account;
        }


        private async Task<(RefreshToken, BaseUser)> getRefreshToken(string token)
        {
            var account = await _context.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (account == null) throw new AppException("Invalid token");
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) throw new AppException("Invalid token");
            return (refreshToken, account);
        }

        private string generateJwtToken(BaseUser account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.UserId.ToString()) }),
                Expires = GlobalFunctions.GetCurrentDateTime().AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken generateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = randomTokenString(),
                Expires = GlobalFunctions.GetCurrentDateTime().AddDays(7),
                Created = GlobalFunctions.GetCurrentDateTime(),
                CreatedByIp = ipAddress
            };
        }

        private void removeOldRefreshTokens(BaseUser account)
        {
            account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= GlobalFunctions.GetCurrentDateTime());
        }

        private string randomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private void sendVerificationEmail(BaseUser account, string origin)
        {
            string message;
            origin = "localhost:4200";
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/users/verifyemail?token={account.VerificationToken}";
                message = $@"<p>Vă rugăm apăsați pe link-ul de mai jos pentru a vă verifica email-ul:</p>
                             <p><a href=""{verifyUrl}"">Verifică email</a></p>";
            }
            else
            {
                message = $@"<p>Vă rugăm contactați un expert cu codul de mai jos pentru a verifica email-ul</p>
                             <p><code>{account.VerificationToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "ProRural: Verifică Email",
                html: $@"<h4>Verifică email-ul</h4>
                         <p>Ați fost înregistrat cu succes pe platforma ProRural</p>
                         {message}"
            );
        }

        private void sendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            message = $@"<p>Dacă ați uitat parola folositi link-ul următor pentru a o reseta <a href=""https://www.proiecteprorural.xerom.ro/users/resetPassword"">Link</a></p>";
           
            _emailService.Send(
                to: email,
                subject: "ProRural: Acest email este deja înregistrat în platformă",
                html: $@"<h4>Cont existent</h4>
                         <p>Email-ul <strong>{email}</strong> este deja înregistrat.</p>
                         {message}"
            );
        }

        private void sendPasswordResetEmail(BaseUser account, string origin)
        {
            string message;
            origin = "https://www.proiecteprorural.xerom.ro";
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/users/ResetPassword?token={account.ResetToken}";
                message = $@"<p>Apăsați pe link-ul de mai jos pentru a reseta parola. Link-ul e valabil 24 de ore de la primire</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Vă rugăm contactați un expert din cadrul proiectului cu token-ul de mai jos, pentru resetarea parolei</p>
                             <p><code>{account.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "ProRural: Verificare resetare parola",
                html: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }
    }
}
