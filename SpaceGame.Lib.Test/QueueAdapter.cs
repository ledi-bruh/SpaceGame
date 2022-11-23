namespace SpaceGame.Lib.Test;

public class QueueAdapter<T> : IQueue<T>
{
    private Queue<T> _queue;

    public QueueAdapter(Queue<T> queue) => _queue = queue;

    public void Push(T t) => _queue.Enqueue(t);

    public T Pop() => _queue.Dequeue();
}
