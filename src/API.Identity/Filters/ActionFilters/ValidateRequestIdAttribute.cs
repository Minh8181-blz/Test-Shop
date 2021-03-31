using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service.CommunicationStandard.Const;
using Service.CommunicationStandard.Models;
using System;
using System.Linq;

namespace API.Identity.Filters.ActionFilters
{
    public class ValidateRequestIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var requestId = context.HttpContext.Request.Headers
                    .FirstOrDefault(x => x.Key == "x-requestid")
                    .Value
                    .FirstOrDefault();

            if (!Guid.TryParse(requestId, out Guid guid) || guid == Guid.Empty)
            {
                var response = new ActionResultModel
                {
                    Code = ActionCode.RequestIdNotPresent,
                    Message = "X-requestid header is missing"
                };

                context.Result = new BadRequestObjectResult(response);
            }
        }
    }
}
