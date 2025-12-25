using Core.Logic.Serialization.Interfaces;
using System.Text;
using System.Text.Json;

namespace Core.Logic.Serialization;

internal class MessageSerializer : IMessageSerializer
{
    public T? Deserialize<T>(byte[] data)
    {
        var json = Encoding.UTF8.GetString(data);
        var obj = JsonSerializer.Deserialize<T>(json);
        return obj;
    }

    public byte[] Serialize<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var bytes = Encoding.UTF8.GetBytes(json);
        return bytes;
    }
}