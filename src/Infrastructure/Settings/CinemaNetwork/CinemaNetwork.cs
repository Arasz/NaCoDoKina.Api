using Infrastructure.Settings.Common;

namespace Infrastructure.Settings.CinemaNetwork
{
    public class CinemaNetwork
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public PropertySelector[] MoviePageMappings { get; set; }

        public void Deconstruct(out string name, out string url)
        {
            name = Name;
            url = Url;
        }
    }
}