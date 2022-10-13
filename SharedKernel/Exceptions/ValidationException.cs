using System;

namespace SharedKernel.Exceptions;

public abstract class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }

    public ValidationException(string message, Exception innerException)
        : base(message, innerException) { }
}