using System.Text.Json;
using System.Text.Json.Serialization;

namespace BuildingCompany.Serialization;

public class MyJsonSerializer: ISerializer
{
    private readonly JsonSerializerOptions _options;

    public MyJsonSerializer()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        };
    }


    public string Serialize<T>(T obj)
    {
        if (obj is null) {
            throw new ArgumentNullException(nameof(obj),"Объект для сериализации не может быть null.");
        }
        return JsonSerializer.Serialize(obj, _options);
    }
    public T? Deserialize<T>(string data)
    {
        if (string.IsNullOrWhiteSpace(data)) {
            throw new ArgumentException("Строка для десериализации не может быть пустой или null.", nameof(data));
        }
        var obj = JsonSerializer.Deserialize<T>(data, _options);
        if (obj is null) {
            throw new JsonException($"Не удалось десериализовать JSON данные в тип {typeof(T).FullName}");
        }
        return obj;
    }
}