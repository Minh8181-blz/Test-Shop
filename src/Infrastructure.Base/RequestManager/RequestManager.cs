using Application.Base.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Base.RequestManager
{
    public class RequestManager<T> : IRequestManager where T : IRequestManagerDbContext
    {
        protected readonly IRequestManagerDbContext _context;

        public RequestManager(T context)
        {
            _context = context;
        }

        public async Task CreateRequestForCommandAsync<TOC>(Guid id)
        {
            var request = new RequestEntry()
            {
                Id = id,
                Name = typeof(TOC).Name,
                Time = DateTime.UtcNow
            };

            _context.RequestEntries.Add(request);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(Guid id)
        {
            var request = await _context.RequestEntries.FirstOrDefaultAsync(x => x.Id == id);

            return request != null;
        }
    }
}
