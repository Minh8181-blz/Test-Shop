using System;

namespace Domain.Base.SeedWork
{
    public class DomainException : Exception
    {
        public DomainException()
        { }

        public DomainException(string message)
            : base(message)
        { }

        public string Code => GetType().Name;
    }
}
