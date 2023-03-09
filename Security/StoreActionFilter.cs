using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRMS.Security
{
    public class StoreActionFilter : IActionFilter
    {
        public StoreActionFilter()
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Debug.Write(MethodBase.GetCurrentMethod(), context.HttpContext.Request.Path);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Do something after the action executes.
            Console.WriteLine("TestActionExecuted:  " + context.HttpContext.Request.Path);
        }
    }
}
