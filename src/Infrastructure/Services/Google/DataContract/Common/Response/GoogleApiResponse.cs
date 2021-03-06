﻿using System.Runtime.Serialization;

namespace Infrastructure.Services.Google.DataContract.Common.Response
{
    [DataContract]
    public class GoogleApiResponse
    {
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }

        [DataMember(Name = "error_message")]
        public string ErrorMessage { get; set; }
    }
}