namespace NaCoDoKina.Api.Entities.Resources
{
    /// <inheritdoc/>
    /// <summary>
    /// Link to resource 
    /// </summary>
    public class ResourceLink : Entity
    {
        public ResourceLink()
        {
        }

        public ResourceLink(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        public override string ToString()
        {
            return $"{nameof(Url)}: {Url}";
        }
    }
}