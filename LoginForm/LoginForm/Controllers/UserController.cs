using LoginForm.Data;
using LoginForm.Helpers;
using LoginForm.Model.Enums;
using LoginForm.Model.Parties;
using LoginForm.ViewModel.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using BC = BCrypt.Net.BCrypt;

namespace LoginForm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : BaseApiController
    {
        #region fields

        private readonly AppSettings _appSettings;


        #endregion

        #region constructor

        public UserController(AppDbContext context, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _appSettings = appSettings.Value;

        }

        #endregion

        #region Register
        [HttpPost("register")]
        public IActionResult Register(RegisterInput model)
        {
            try
            {
                using (AppUnitOfWork unitOfWork = new AppUnitOfWork(_context))
                {
                    model.Password = BC.HashPassword(model.Password);

                    UserEntity user = unitOfWork.UserRepository.FindOneReadOnly(m => m.EmailAddress.ToLower() == model.EmailAddress.ToLower() && m.Status == Status.Active);

                    if (user == null)
                    {
                        RoleEntity userRole = unitOfWork.RoleRepository.FindOneReadOnly(m => m.Name == DataConstants.Roles.User);

                        user = new UserEntity()
                        {
                            EmailAddress = model.EmailAddress,
                            Name = model.Username,

                            PasswordHash = model.Password,
                            Picture = "",
                            Roles = new List<UserRoleEntity>()
                            {
                                new UserRoleEntity() { RoleId = userRole.Id }
                            },
                            Location = model.Location,
                            DOB = model.DOB

                        };
                        unitOfWork.UserRepository.InsertOrUpdate(user);
                        unitOfWork.Commit();

                        var token = TokenHelper.Generate(user.Id, _appSettings.Secret, userRole.Name);

                        UserTokenEntity userToken = new UserTokenEntity()
                        {
                            UserId = user.Id,
                            Token = token,
                            ExpiryDate = DateTime.UtcNow.AddYears(1).ToLongDateString(),
                        };

                        unitOfWork.UserTokenRepository.InsertOrUpdate(userToken);
                        unitOfWork.Commit();

                        return ApiResponse(true, GlobalConstants.MESSAGE_SUCCESS, new RegisterOutput()
                        {
                            EmailAddress = user.EmailAddress,
                            Name = user.Name,
                            Token = token,
                            Location = model.Location,
                            DOB = model.DOB,
                        });
                    }

                    return ApiResponse(false, "This email already exists!");
                }
            }
            catch (Exception ex)
            {
                LogError(typeof(UserController), ex);
                return ApiResponse(false, GlobalConstants.MESSAGE_EXCEPTION, ex);
            }
        }
        #endregion

        #region Login

        [HttpPost("login")]
        public IActionResult Login(LoginInput model)
        {
            try
            {
                using (AppUnitOfWork unitOfWork = new AppUnitOfWork(_context))
                {
                    UserEntity user = unitOfWork.UserRepository.FindOneReadOnly(m => m.EmailAddress.ToLower() == model.Username.ToLower() && m.Status == Status.Active, i => i.Roles, i => i.Roles.Select(m => m.Role), x => x.Roles);

                    if (user != null && BC.Verify(model.Password, user.PasswordHash))

                    {
                        var token = TokenHelper.Generate(user.Id, _appSettings.Secret, user.Roles.Select(m => m.Role.Name).ToArray());

                        UserTokenEntity userToken = new UserTokenEntity()
                        {
                            UserId = user.Id,
                            Token = token,
                            ExpiryDate = DateTime.UtcNow.AddYears(1).ToLongDateString(),
                        };

                        unitOfWork.UserTokenRepository.InsertOrUpdate(userToken);


                        var roleName = "";
                        if (user.Roles.FirstOrDefault() != null)
                        {
                            roleName = user.Roles.FirstOrDefault().Role.Name;
                        }
                        else
                        {
                            roleName = DataConstants.Roles.User;
                        }

                        unitOfWork.Commit();

                        return ApiResponse(true, GlobalConstants.MESSAGE_SUCCESS, new RegisterOutput()
                        {
                            Id = user.Id,
                            EmailAddress = user.EmailAddress,
                            Name = user.Name,
                            Token = token,
                            Role = roleName,
                            Location = user.Location,
                            DOB = user.DOB
                        });
                    }

                    return ApiResponse(false, "Invalid Credentials");
                }
            }
            catch (Exception ex)
            {
                LogError(typeof(UserController), ex);
                return ApiResponse(false, GlobalConstants.MESSAGE_EXCEPTION);
            }
        }

        #endregion
    }
}




