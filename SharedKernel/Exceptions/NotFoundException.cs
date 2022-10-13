using System;

namespace SharedKernel.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
