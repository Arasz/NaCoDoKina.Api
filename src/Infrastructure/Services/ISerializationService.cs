namespace Infrastructure.Services
{
    public interface ISerializationService
    {
        string Serialize<TSerialized>(TSerialized serialized);

        TDeserialized Deserialize<TDeserialized>(string serialized);
    }
}