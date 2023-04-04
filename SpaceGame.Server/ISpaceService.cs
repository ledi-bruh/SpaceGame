using System.Net;
using CoreWCF;
using CoreWCF.Web;
using CoreWCF.OpenApi.Attributes;

namespace WebHttp
{
    [ServiceContract]
    [OpenApiBasePath("/space")]
    internal interface ISpaceService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/order")]
        [OpenApiTag("space")]
        [OpenApiResponse(ContentTypes = new[] { "application/json" }, Description = "Success", StatusCode = HttpStatusCode.OK, Type = typeof(SpaceContract))]
        SpaceContract Order(
            [OpenApiParameter(ContentTypes = new[] { "application/json" }, Description = "Space Request")]
            SpaceContract request
        );
    }
}
