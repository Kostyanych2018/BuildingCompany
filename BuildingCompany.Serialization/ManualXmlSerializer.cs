using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace BuildingCompany.Serialization;

public class ManualXmlSerializer : ISerializer
{
    public string Serialize<T>(T obj)
    {
        var sb = new StringBuilder();
        sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
        if (obj == null) {
            throw new NotSupportedException("Root object cannot be null in this basic XML serializer.");
        }

        string rootName = GetElementName(typeof(T));
        SerializeValue(sb, obj, rootName);
        return sb.ToString();
    }

    private void SerializeValue(StringBuilder sb, object? obj, string elementName)
    {
        if (string.IsNullOrWhiteSpace(elementName) || elementName.Contains(" ") ||
            elementName.StartsWith("xml", StringComparison.OrdinalIgnoreCase)
            || elementName.Contains("<") || elementName.Contains(">") || elementName.Contains("&")) {
            elementName = "InvalidElementName";
            Console.WriteLine($"Warning: Invalid element name '{elementName}', using '{elementName}'");
        }

        if (obj == null) {
            sb.Append($"<{elementName}/>");
            return;
        }

        var type = obj.GetType();

        if (type.IsPrimitive || type == typeof(string)
                             || type == typeof(decimal) || type == typeof(Guid)
                             || type == typeof(DateTime) || type.IsEnum) {
            sb.Append($"<{elementName}>");
            string stringValue = obj switch
            {
                DateTime dt => dt.ToString("o", CultureInfo.InvariantCulture),
                Guid guid => guid.ToString("D"),
                _ => obj.ToString() ?? ""
            };

            sb.Append(EscapeXml(stringValue));
            sb.Append($"</{elementName}>");
            return;
        }

        if (obj is IEnumerable enumerable) {
            string itemElementName = "Item";
            Type? itemType = null;

            if (type.IsGenericType) {
                itemType = type.GetGenericArguments().FirstOrDefault();
            }

            if (itemType == null) {
                var firstItem = enumerable.Cast<object?>().FirstOrDefault();
                if (firstItem != null) {
                    itemType = firstItem.GetType();
                }
            }

            if (itemType != null) {
                itemElementName = GetElementName(itemType);
            }


            sb.Append($"<{elementName}>"); 
            foreach (var item in enumerable) {
                SerializeValue(sb, item, itemElementName);
            }

            sb.Append($"</{elementName}>"); 
            return;
        }

        if (type.IsClass || type.IsInterface)
        {
            sb.Append($"<{elementName}>"); 

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties) {
                if (!property.CanRead) continue;

                var value = property.GetValue(obj);
                
                SerializeValue(sb, value, property.Name);
            }

            sb.Append($"</{elementName}>");
            return;
        }
        
        throw new NotSupportedException($"Serialization of type {type.FullName} is not supported by this basic XML serializer.");
    }

    private string GetElementName(Type type)
    {
        string name = type.Name;
        if (type.IsGenericType) {
            name = name.Substring(0, name.IndexOf('`')); // Remove <T> part
        }

        name = System.Text.RegularExpressions.Regex.Replace(name, @"[^a-zA-Z0-9_.-]", "");
        if (!char.IsLetter(name.FirstOrDefault()) && name.FirstOrDefault() != '_') {
            name = "_" + name;
        }

        if (string.IsNullOrWhiteSpace(name)) name = "Object";

        return name;
    }

    private string EscapeXml(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        return s.Replace("&", "&")
            .Replace("<", "<")
            .Replace(">", ">")
            .Replace("\"", "\"")
            .Replace("'", "'");
    }

    public T? Deserialize<T>(string data)
    {
        throw new NotImplementedException();
    }
}