using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using OneOf;

namespace Hurace.Core.Logic.Models
{
    public class SaveResult : OneOfBase<SaveResult.Success, SaveResult.ValidationError, SaveResult.Error>
    {
        public bool IsSuccess => this.IsT0;
        public bool IsError => !this.IsSuccess;

        public class Success : SaveResult 
        {
            public Success(int id)
            {
                this.Id = id;
            }

            public int Id { get; private set; }            
        }

        public class ValidationError : SaveResult 
        {
            public ValidationError(IList<ValidationFailure> errors)
            {
                this.Errors = errors.Select(e => (e.PropertyName, e.ErrorCode)).ToList().AsReadOnly();
            }

            public IReadOnlyList<(string PropertyName, string ErrorCode)> Errors { get; private set; } 
        }

        public class Error : SaveResult 
        {
            public Error(string errorCode)
            {
                this.ErrorCode = errorCode;
            }

            public string ErrorCode { get; private set; } 
        }
    }
}
