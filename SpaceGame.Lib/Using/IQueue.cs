namespace SpaceGame.Lib;

public interface IQueue<T>
{
    public void Push(T t);
    public T Pop();
}
