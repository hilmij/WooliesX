namespace WooliesXFunctionApp.Exception
{
    using System;

    public class CannotGetResourceException : Exception
    {
        public CannotGetResourceException(string message) : base(message)
        {
        }
    }
}
