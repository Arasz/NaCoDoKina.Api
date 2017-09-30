namespace NaCoDoKina.Api.Services
{
    public interface ISerializationService
    {
        string Serialize<TSerialized>(TSerialized serialized);

        TDeserialized Deserialize<TDeserialized>(string serialized);
    }
}