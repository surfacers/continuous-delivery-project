using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;

namespace Hurace.Mvvm.ViewModels
{
    public abstract class EditViewModel<TModel, TValidator> : NotifyPropertyChanged, INotifyDataErrorInfo
        where TModel : new()
        where TValidator : IValidator<TModel>, new()
    {
        private readonly IMapper mapper;
        private readonly TValidator validator;
        private IDictionary<string, string> errors = new Dictionary<string, string>();

        public TModel Original { get; set; }

        public bool HasErrors => errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            return errors.TryGetValue(propertyName, out string value)
                ? new[] { value }
                : null;
        }

        public EditViewModel(
            IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.validator = new TValidator();
        }

        public void Validate([CallerMemberName] string propertyName = "")
        {
            var model = mapper.Map<TModel>(this);
            ValidationResult result = validator.Validate(model);

            errors = result.Errors
                .GroupBy(m => m.PropertyName)
                .ToDictionary(m => m.Key, m => m.First().ErrorCode);

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public override void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(value, field))
            {
                field = value;
                Raise(propertyName);
                Validate(propertyName);
            }
        }
    }
}
