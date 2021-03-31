using API.Identity.Application;
using API.Identity.Application.Commands;
using API.Identity.Application.Dto;
using API.Identity.Filters.ActionFilters;
using API.Identity.Infrastructure;
using API.Identity.ViewModels;
using Application.Base.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.CommunicationStandard.Const;
using Service.CommunicationStandard.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AuthService _authService;

        public AuthController(IMediator mediator, AuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [HttpPost("signup-customer")]
        [ValidateRequestId]
        [ValidateModel]
        public async Task<ActionResult> SignUpCustomer([FromBody] SignUpViewModel model, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var response = new ActionResultModel();

            Guid.TryParse(requestId, out Guid guid);

            var command = new SignUpAsCustomerCommand(model.Email, model.Password);

            var identifiedCommand = new IdentifiedCommand<SignUpAsCustomerCommand, SignUpResultDto>(command, guid);

            var result = await _mediator.Send(identifiedCommand);

            response.Code = result.Code;
            response.Message = result.Message;

            if (!result.Succeeded)
            {
                response.Data = result.Errors;
                return BadRequest(result);
            }

            response.Data = new SignUpResultViewModel
            {
                Token = await _authService.GenerateJwtAsync(result.User),
                User = new UserViewModel
                {
                    Id = result.User.Id,
                    Email = result.User.Email
                }
            };

            return Ok(response);
        }
    }
}
