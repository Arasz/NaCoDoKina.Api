using System.Reflection;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Common
{
    /// <summary>
    /// Cinema City data api service response type 
    /// </summary>
    [DataContract]
    public class CinemaCityResponse<TEntityModel>
    {
        public static void SetCollectionDataMemberName(string name)
        {
            var responseType = typeof(CinemaCityResponse<TEntityModel>);

            var dataMemberAttribute = responseType
                .GetProperty(nameof(ContentBody))
                .PropertyType
                .GetProperty(nameof(Body.Collection))
                .GetCustomAttribute(typeof(DataMemberAttribute)) as DataMemberAttribute;

            if (dataMemberAttribute is null)
                return;

            dataMemberAttribute.Name = name;
        }

        [DataMember(Name = "body")]
        public Body ContentBody { get; set; }

        public class Body
        {
            [DataMember]
            public TEntityModel[] Collection { get; set; }
        }
    }
}