using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Base.SeedWork
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
