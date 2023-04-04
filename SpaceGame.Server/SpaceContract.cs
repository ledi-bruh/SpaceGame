using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

namespace WebHttp
{
    [DataContract(Name = "SpaceContract")]
    internal class SpaceContract
    {
        [DataMember(Name = "OrderType", Order = 0)]
        [OpenApiProperty(Description = "Тип приказа")]
        public string OrderType { get; set; }

        [DataMember(Name = "GameID", Order = 1)]
        [OpenApiProperty(Description = "ID игры")]
        public int GameID { get; set; }

        [DataMember(Name = "GameItemID", Order = 2)]
        [OpenApiProperty(Description = "ID объекта")]
        public int ObjectID { get; set; }

        [DataMember(Name = "Parameters", Order = 3)]
        [OpenApiProperty(Description = "Параметры приказа")]
        public IDictionary<string, object> Parameters { get; set; }
    }
}
