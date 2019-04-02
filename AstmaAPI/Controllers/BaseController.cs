using AstmaAPI.EF;
using AstmaAPI.Models.DBO;
using AstmaAPI.ViewModels.Request;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstmaAPI.Controllers
{
    public class BaseController : Controller
    {
        protected readonly DataContext Context;
        protected readonly IHostingEnvironment HostingEnvironment;

        public BaseController(DataContext context, IHostingEnvironment hostingEnvironment)
        {
            Context = context;
            HostingEnvironment = hostingEnvironment;
        }

        protected async Task<IActionResult> MethodWrapper<TParam>(Func<UserToken, Task<IActionResult>> func, TParam param)
            where TParam : BaseRequest
        {
            try
            {
                if (CheckToken(param, out UserToken userToken))
                    return await func(userToken);
                else
                    return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("error", "Ошибка сервера :(");
#if DEBUG
                ModelState.AddModelError("ex", string.IsNullOrEmpty(ex.StackTrace) ? ex.InnerException?.StackTrace ?? string.Empty : ex.StackTrace);
                Console.WriteLine(ex.Message);
#endif
                return BadRequest(ModelState);
            }
        }

        protected bool CheckToken(BaseRequest request, out UserToken userToken)
        {
            userToken = Context.UserTokens.Include(ut => ut.User).FirstOrDefault(token => token.Value == request.Token);

            if (userToken != null)
                return true;
            else
            {
                ModelState.AddModelError("error", "Токен недействителен");
                return false;
            }
        }
    }
}
