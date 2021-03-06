﻿using Newtonsoft.Json;

namespace Infrastructure.Services
{
    public class JsonSerializationService : ISerializationService
    {
        public string Serialize<TSerialized>(TSerialized serialized)
        {
            return JsonConvert.SerializeObject(serialized);
        }

        public TDeserialized Deserialize<TDeserialized>(string serialized)
        {
            return JsonConvert.DeserializeObject<TDeserialized>(serialized);
        }
    }
}