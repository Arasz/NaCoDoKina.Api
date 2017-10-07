using System.Linq;

namespace NaCoDoKina.Api.Infrastructure.Settings
{
    public abstract class MultiElementSettingsBase<TElement>
    {
        private TElement[] _elements;

        public TElement[] GetAllElements()
        {
            var type = GetType();

            if (_elements is null)
                _elements = type.GetProperties()
                    .Where(info => info.PropertyType == typeof(TElement))
                    .Select(info => info.GetValue(this))
                    .Cast<TElement>()
                    .ToArray();

            return _elements;
        }
    }
}