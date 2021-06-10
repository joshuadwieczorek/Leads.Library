using Leads.Domain.Contracts.v1;
using System;
using System.Collections.Generic;

namespace Leads.Library.Validation.v1
{
    public sealed class ValidationException : Exception
    {
        public List<ErrorMessage> Errors { get; set; }


        public ValidationException()
        {
            Errors = new List<ErrorMessage>();
        }


        public ValidationException(string field, string message)
        {
            Errors = new List<ErrorMessage>() { new ErrorMessage(field, message) };
        }
    }
}