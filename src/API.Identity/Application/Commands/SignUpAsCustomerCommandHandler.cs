using API.Identity.Application.Dto;
using API.Identity.Domain;
using Application.Base.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Service.CommunicationStandard.Const;
using System.Threading;
using System.Threading.Tasks;

namespace API.Identity.Application.Commands
{
    public class SignUpAsCustomerCommandHandler : IRequestHandler<SignUpAsCustomerCommand, SignUpResultDto>
    {
        private readonly UserManager<AppUser> _userManager;

        public SignUpAsCustomerCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<SignUpResultDto> Handle(SignUpAsCustomerCommand request, CancellationToken cancellationToken)
        {
            var result = new SignUpResultDto();

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                result.Message = "User with this email already exists";
                return result;
            }

            var passHasher = new PasswordHasher<AppUser>();

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            user.PasswordHash = passHasher.HashPassword(user, request.Password);

            var identity = await _userManager.CreateAsync(user);

            result.Succeeded = identity.Succeeded;

            if (identity.Succeeded)
            {
                result.Code = ActionCode.Created;
                result.User = await _userManager.FindByEmailAsync(user.Email);
            }
            else
            {
                result.Errors = identity.Errors;
            }

            return result;
        }
    }

    public class SignUpAsCustomerIdentifiedCommandHandler : IdentifiedCommandHandler<SignUpAsCustomerCommand, SignUpResultDto>
    {
        public SignUpAsCustomerIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
            : base(mediator, requestManager)
        {
        }

        protected override SignUpResultDto CreateResultForDuplicateRequest()
        {
            var result = new SignUpResultDto
            {
                Succeeded = false,
                Code = ActionCode.DuplicateCommand,
                Message = "Request is already handled"
            };

            return result;
        }
    }
}
