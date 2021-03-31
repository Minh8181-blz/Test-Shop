using API.Identity.Domain;
using Application.Base.SeedWork;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace API.Identity.Application.Dto
{
    public class SignUpResultDto : CommandResultModel
    {
        public AppUser User { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
