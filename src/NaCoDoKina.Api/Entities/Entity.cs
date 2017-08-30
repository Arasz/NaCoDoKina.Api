namespace NaCoDoKina.Api.Entities
{
    /// <summary>
    /// Base class for all persisted model classes 
    /// </summary>
    public abstract class Entity
    {
        public long Id { get; set; }
    }
}