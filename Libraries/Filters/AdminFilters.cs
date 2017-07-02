using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EverStore.Libraries.Filters
{
    public class AdminFilters:Attribute,IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            
            throw new NotImplementedException();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }
    }
}