﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Infrastructure.Services.Google.DataContract.Geocoding.Response
{
    [DataContract]
    public class AddressComponent
    {
        [DataMember(Name = "long_name")]
        public string LongName { get; set; }

        [DataMember(Name = "short_name")]
        public string ShortName { get; set; }

        [DataMember(Name = "types")]
        public List<string> Types { get; set; }
    }
}