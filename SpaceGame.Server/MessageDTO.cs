using SpaceGame.Lib;

namespace WebHttp
{
    internal class MessageDTO : IInterpretingMessage
    {
        private SpaceContract _spaceContract;

        public MessageDTO(SpaceContract spaceContract) => _spaceContract = spaceContract;

        public int GameID => _spaceContract.GameID;

        public int ObjectID => _spaceContract.ObjectID;

        public string TypeCommand => _spaceContract.OrderType;

        public IDictionary<string, object> Parameters => _spaceContract.Parameters;
    }
}
