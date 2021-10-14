using Elcom.DataObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Elcom.Core.Service.Helper
{
    public class SubscriptionAttribute : ActionFilterAttribute, IActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepositoryBase<User> _userRepository;
        public SubscriptionAttribute(IHttpContextAccessor httpContextAccessor, IRepositoryBase<User> userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }
     
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
           
        }
        void IActionFilter.OnActionExecuted(ActionExecutedContext filterContext)
        {
            _httpContextAccessor.HttpContext.Session.SetString("IsUserSubscriber", null);
        }
    }
}
