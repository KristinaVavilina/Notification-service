namespace Core.Logic.Serialization.Interfaces;

public interface IMessageSerializer
{
    public byte[] Serialize<T>(T obj);

    public T? Deserialize<T>(byte[] data);
}