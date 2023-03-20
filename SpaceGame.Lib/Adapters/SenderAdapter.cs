namespace SpaceGame.Lib;
using System.Collections.Concurrent;

class SenderAdapter : ISender
{
    BlockingCollection<ICommand> queue;

    public SenderAdapter(BlockingCollection<ICommand> queue) => this.queue = queue;

    public void Send(ICommand cmd)
    {
        queue.Add(cmd);
    }
}
