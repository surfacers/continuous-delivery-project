using System;

namespace Hurace.RaceControl.ViewModels.Controls
{
    public class ComboBoxItemViewModel<T>
    {
        public string Name { get; private set; }

        public T Value { get; private set; }

        public ComboBoxItemViewModel(string name, T value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }

        public override string ToString() => Name;
    }
}
