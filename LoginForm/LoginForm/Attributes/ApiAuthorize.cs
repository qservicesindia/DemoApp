using LoginForm.Data;
using LoginForm.Model.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace LoginForm.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ApiAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _someFilterParameter;

        public ApiAuthorizeAttribute() : base()
        {
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }

        public ApiAuthorizeAttribute(string someFilterParameter)
        {
            _someFilterParameter = someFilterParameter;
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var User = context.HttpContext.User;

            if (!User.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                return;
            }

            // you can also use registered services
            AppDbContext _context = (AppDbContext)context.HttpContext.RequestServices.GetService(typeof(AppDbContext));

            AppUnitOfWork unitOfWork = new AppUnitOfWork(_context);
            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = unitOfWork.UserRepository.FindOneReadOnly(m => m.Id == Convert.ToInt32(User.Identity.Name) && m.Status == Status.Active);

                if (user == null)
                {
                    context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                    return;
                }
            }
        }
    }
}
