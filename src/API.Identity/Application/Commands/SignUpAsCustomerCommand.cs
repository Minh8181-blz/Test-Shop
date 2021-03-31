using API.Identity.Application.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Identity.Application.Commands
{
    public class SignUpAsCustomerCommand : IRequest<SignUpResultDto>
    {
        public SignUpAsCustomerCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }
}
