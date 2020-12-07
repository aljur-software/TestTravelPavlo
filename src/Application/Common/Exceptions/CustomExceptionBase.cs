using System;

namespace Application.Common.Exceptions
{
    public class CustomExceptionBase: Exception
    {
        public CustomExceptionBase(string message) : base(message) { }
    }
}