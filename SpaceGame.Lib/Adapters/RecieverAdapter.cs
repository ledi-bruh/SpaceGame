namespace SpaceGame.Lib;
using System.Collections.Concurrent;

class ReceiverAdapter : IReceiver
{
    BlockingCollection<ICommand> queue;

    public ReceiverAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public ICommand Receive()
    {
        return queue.Take();
    }

    public bool isEmpty()
    {
        return queue.Count() == 0;
    }
}
