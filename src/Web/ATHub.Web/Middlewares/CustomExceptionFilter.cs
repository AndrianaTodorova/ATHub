//using Microsoft.AspNetCore.Mvc.Filters;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ATHub.Web.Middlewares
//{
//    public class CustomExceptionFilter : ExceptionFilterAttribute
//    {
//        public override void OnException(ExceptionContext context)
//        {
//            context.HttpContext.Response.StatusCode = 500;

//            base.OnException(context);
//        }

//        public override Task OnExceptionAsync(ExceptionContext context)
//        {
//            context.HttpContext.Response.StatusCode = 500;
//            this.OnException(context);
//            return base.OnExceptionAsync(context);
//        }
//    }
//}
