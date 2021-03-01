using System;

namespace Hurace.Core.Exceptions
{
    public class CaseNotImplementedException<T> : Exception
    {
        public CaseNotImplementedException(T @case)
            : base($"Enum case '{@case}' cannot be handled for type '{typeof(T).Name}'.")
        {
        }
    }
}
