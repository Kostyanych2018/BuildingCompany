using System.Xml;
using System.Xml.Serialization;

namespace BuildingCompany.Serialization;

public class MyXmlSerializer: ISerializer
{
    public string Serialize<T>(T obj)
    {
        if (obj is null) {
            throw new ArgumentNullException(nameof(obj),"Объект для сериализации не может быть null.");
        }
        var xmlSerializer = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        xmlSerializer.Serialize(stringWriter, obj);
        return stringWriter.ToString();
    }
    public T? Deserialize<T>(string data)
    {
        if (string.IsNullOrWhiteSpace(data)) {
            throw new ArgumentException("Строка для десериализации не может быть пустой или null.", nameof(data));
        }
        var xmlSerializer = new XmlSerializer(typeof(T));
        using var stringReader = new StringReader(data);
        var obj = (T?)xmlSerializer.Deserialize(stringReader);
        if (obj is null) {
            throw new XmlException($"Не удалось десериализовать XML данные в тип {typeof(T).FullName}");
        }
        return obj;
    }
}