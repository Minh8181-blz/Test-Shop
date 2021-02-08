using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Base.RequestManager
{
    public class RequestEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
    }
}
