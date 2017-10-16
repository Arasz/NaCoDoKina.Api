using Infrastructure.Settings;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Settings.Common;

namespace Infrastructure.DataProviders.Bindings
{
    public class BindingErrors<TBinded>
    {
        private readonly PropertySelector[] _selectors;
        private readonly Dictionary<string, string> _bindingErrors;

        public void AddError(string propertyName, string error)
        {
            _bindingErrors[propertyName] = error;
        }

        public BindingErrors(PropertySelector[] selectors)
        {
            _selectors = selectors;
            _bindingErrors = new Dictionary<string, string>();
        }

        public bool HasErrors() => _bindingErrors.Any();

        public override string ToString()
        {
            var errorBuilder = new StringBuilder($"Binding errors for type {typeof(TBinded)}");
            errorBuilder.AppendLine();

            foreach (var propertySelector in _selectors)
            {
                var propertyName = propertySelector.PropertyName;
                if (_bindingErrors.ContainsKey(propertyName))
                {
                    var errorLine = $"{propertyName} - {_bindingErrors[propertyName]}";
                    errorBuilder.AppendLine(errorLine);
                }
            }

            return errorBuilder.ToString();
        }
    }
}