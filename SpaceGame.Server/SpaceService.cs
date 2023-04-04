using CoreWCF;
using Hwdtech;

namespace WebHttp
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    internal class SpaceService : ISpaceService
    {
        public SpaceContract Order(SpaceContract request)
        {
            IoC.Resolve<ICommand>("Http.Order", new MessageDTO(request)).Execute();
            return request;
        }
    }
}
