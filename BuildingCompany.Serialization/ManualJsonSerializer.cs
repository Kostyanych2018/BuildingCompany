using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace BuildingCompany.Serialization;

public class ManualJsonSerializer : ISerializer
{
    public string Serialize<T>(T obj)
    {
        if (obj == null) {
            return "null";
        }

        var type = obj.GetType();

        if (type == typeof(string)) {
            return $"\"{EscapeString((string)(object)obj)}\"";
        }

        if (type == typeof(bool)) {
            return obj.ToString().ToLower();
        }

        if (type.IsPrimitive || type == typeof(decimal)) {
            return obj.ToString().Replace(",", ".");
        }


        if (obj is IEnumerable enumerable && type != typeof(string)) {
            var sb = new StringBuilder();
            sb.Append("[");
            bool first = true;
            foreach (var item in enumerable) {
                if (!first) {
                    sb.Append(",");
                }

                sb.Append(Serialize(item));
                first = false;
            }

            sb.Append("]");
            return sb.ToString();
        }

        if (type.IsClass || type.IsInterface) {
            var sb = new StringBuilder();
            sb.Append("{");
            bool first = true;
            var properties = type.GetProperties(BindingFlags.Public
                                                | BindingFlags.Instance);
            foreach (var property in properties) {
                if (!property.CanRead) continue;
                var value = property.GetValue(obj);
                if (!first) {
                    sb.Append(",");
                }

                sb.Append($"\"{property.Name}\":");
                sb.Append(Serialize(value));
                first = false;
            }

            sb.Append("}");
            return sb.ToString();
        }

        throw new NotSupportedException($"Тип {type.FullName} не поддерживается сериализатором.");
    }

    private string EscapeString(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        return s.Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\n", "\\n")
            .Replace("\r", "\\r")
            .Replace("\t", "\\t");
    }

    public T? Deserialize<T>(string data)
    {
        if (string.IsNullOrWhiteSpace(data))
            return default(T);

        int index = 0;
        object? parsedValue = ParseValue(data, ref index);
        return ConvertToTargetType<T>(parsedValue);
    }

    private T ConvertToTargetType<T>(object? parsedValue)
    {
        if (parsedValue == null) {
            return default(T)!;
        }

        var targetType = typeof(T);
        var parsedType = parsedValue.GetType();
        if (targetType.IsAssignableFrom(parsedType)) {
            return (T)parsedValue;
        }

        if (parsedValue is Dictionary<string, object?> jsonObj) {
            if (targetType.IsClass || targetType.IsInterface) {
                var instance = Activator.CreateInstance<T>();
                var properties = targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanWrite)
                    .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

                foreach (var kvp in jsonObj) {
                    if (properties.TryGetValue(kvp.Key, out var property)) {
                        try {
                            object? propertyValue = ConvertToTargetType(property.PropertyType, kvp.Value);
                            property.SetValue(instance, propertyValue);
                        }
                        catch (Exception ex) {
                            Console.WriteLine($"Warning: Could not convert JSON value for property " +
                                              $"'{property.Name}' of type {property.PropertyType.FullName}. Error: {ex.Message}");
                        }
                    }
                    else {
                        Console.WriteLine($"Warning: JSON key '{kvp.Key}' has no matching writable property in type {targetType.FullName}");
                    }
                }

                return instance;
            }
        }

        if (parsedValue is List<object?> jsonArray) {
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(List<>)) {
                Type itemType = targetType.GetGenericArguments()[0];
                var list = (IList)Activator.CreateInstance(targetType)!;

                foreach (var item in jsonArray) {
                    try {
                        object? convertedItem = ConvertToTargetType(itemType, item);
                        list.Add(convertedItem);
                    }
                    catch (Exception ex) {
                        Console.WriteLine($"Warning: Could not convert item in JSON array to type {itemType.FullName}. Error: {ex.Message}");
                    }
                }

                return (T)list;
            }

            Console.WriteLine($"Warning: Deserialization to collection type {targetType.FullName} is not fully supported. Only List<TItem> is explicitly handled.");
        }

        try {
            // This handles conversions like:
            // JSON string -> C# Guid/DateTime/Enum (needs TypeDescriptor.GetConverter)
            // JSON number (decimal/double) -> C# int/float/double (needs Convert.ChangeType)
            // JSON boolean -> C# bool
            // JSON number -> C# decimal
            // JSON string representing number/bool -> C# int/bool (needs Convert.ChangeType or TryParse)
            
            Type nullableUnderlyingType = Nullable.GetUnderlyingType(targetType)!;
            if (nullableUnderlyingType != null) {
                // If target type is nullable (e.g., int?), try converting to the underlying type (int)
                // If parsedValue is null, will return default (null for nullable) which is correct.
                object? convertedToUnderlying = ConvertToTargetType(nullableUnderlyingType, parsedValue);
                return (T)convertedToUnderlying!; // Cast back to nullable T
            }


            // Handle specific conversions not covered by ChangeType easily, or where ChangeType might be too strict
            if (targetType.IsEnum && parsedValue is string enumString) {
                return (T)Enum.Parse(targetType, enumString, ignoreCase: true); // Case-insensitive enum parsing
            }

            if (targetType == typeof(Guid) && parsedValue is string guidString) {
                return (T)(object)Guid.Parse(guidString); // Parse Guid from string
            }

            if (targetType == typeof(DateTime) && parsedValue is string dateTimeString) {
                // Use DateTime.Parse/TryParse with appropriate style/culture
                return (T)(object)DateTime.Parse(dateTimeString, CultureInfo.InvariantCulture);
            }


            // Use Convert.ChangeType for many primitive conversions
            // It handles int <-> double <-> decimal, bool <-> string ("True"/"False"), etc.
            return (T)Convert.ChangeType(parsedValue, targetType, CultureInfo.InvariantCulture);
        }
        catch (Exception ex) {
            // Handle conversion errors (e.g., trying to convert "abc" to int)
            throw new JsonException($"Не удалось преобразовать значение '{parsedValue}' (тип {parsedType.FullName}) к целевому типу {targetType.FullName}", ex);
        }
    }

    private object? ConvertToTargetType(Type targetType, object? parsedValue)
    {
        try {
            MethodInfo? convertMethod = typeof(ManualJsonSerializer)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .FirstOrDefault(m => m.Name == nameof(ConvertToTargetType) && m.IsGenericMethodDefinition && m.GetParameters().Length == 1);

            if (convertMethod == null) {
                throw new InvalidOperationException("Internal serializer error: ConvertToTargetType generic method not found.");
            }

            MethodInfo? genericConvertMethod = convertMethod.MakeGenericMethod(targetType);

            return genericConvertMethod.Invoke(this, [parsedValue]);
        }
        catch (TargetInvocationException tie) when (tie.InnerException != null) {
            throw tie.InnerException;
        }
        catch (Exception ex) {
            throw new JsonException($"Failed to perform recursive conversion to type {targetType.FullName}", ex);
        }
    }

    private object? ParseValue(string json, ref int index)
    {
        SkipWhitespace(json, ref index);
        if (index >= json.Length) {
            throw new JsonException("Неожиданный конец строки JSON.");
        }

        char currentChar = json[index];
        switch (currentChar) {
            case '{':
                return ParseObject(json, ref index);
            case '[':
                return ParseArray(json, ref index);
            case '"':
                return ParseString(json, ref index);
            case 't':
                return ParseLiteral(json, ref index, "true");
            case 'f':
                return ParseLiteral(json, ref index, "false");
            case 'n':
                return ParseLiteral(json, ref index, "null");
            case '-':
            case var _ when char.IsDigit(currentChar):
                return ParseNumber(json, ref index);
            default:
                throw new JsonException($"Неожиданный символ при парсинге значения '{currentChar}'" +
                                        $"в позиции {index}");
        }
    }

    private object ParseNumber(string json, ref int index)
    {
        int startIndex = index;
        while (index < json.Length) {
            char currentChar = json[index];
            if (char.IsDigit(currentChar) || currentChar == '-' || currentChar == '.') {
                index++;
            }
            else {
                break;
            }
        }

        string numString = json.Substring(startIndex, index - startIndex);
        if (decimal.TryParse(numString, CultureInfo.InvariantCulture, out decimal decimalValue)) {
            return decimalValue;
        }

        if (double.TryParse(numString, CultureInfo.InvariantCulture, out double doubleValue)) {
            return doubleValue;
        }

        if (int.TryParse(numString, CultureInfo.InvariantCulture, out int intValue)) {
            return intValue;
        }

        throw new JsonException($"Некорректный формат числа: '{numString}' в позиции {startIndex}");
    }

    private List<object?> ParseArray(string json, ref int index)
    {
        if (json[index] != '[')
            throw new JsonException($"Ожидался '[' в позиции {index}");
        index++;
        SkipWhitespace(json, ref index);
        var list = new List<object?>();
        if (index < json.Length && json[index] == ']') {
            index++;
            return list;
        }

        while (index < json.Length) {
            SkipWhitespace(json, ref index);
            object? value = ParseValue(json, ref index);
            list.Add(value);
            SkipWhitespace(json, ref index);
            char separator = json[index];
            if (separator == ',') {
                index++;
            }
            else if (separator == ']') {
                index++;
                return list;
            }
            else {
                throw new JsonException($"Ожидалась ',' или ']' после элемента массива в позиции {index}");
            }
        }

        throw new JsonException("Неожиданный конец строки при парсинге массива.");
    }

    private Dictionary<string, object?> ParseObject(string json, ref int index)
    {
        if (json[index] != '{') throw new JsonException($"Ожидался '{{' в позиции {index}");
        index++;
        SkipWhitespace(json, ref index);
        var objDict = new Dictionary<string, object?>();
        if (index < json.Length && json[index] == '}') {
            index++;
            return objDict;
        }

        while (index < json.Length) {
            SkipWhitespace(json, ref index);
            if (index >= json.Length || json[index] != '\"') {
                if (json[index] == '}') {
                    index++;
                    return objDict; 
                }
                throw new JsonException($"Ожидался ключ (строка в двойных кавычках) или '}}' в позиции {index}");
            }

            string key = ParseString(json, ref index);
            SkipWhitespace(json, ref index);
            if (index >= json.Length || json[index] != ':')
                throw new JsonException($"Ожидался ':' после ключа '{key}' в позиции {index}");
            index++;
            object? value = ParseValue(json, ref index);
            objDict[key] = value;
            SkipWhitespace(json, ref index);
            if (json[index] == ',') {
                index++;
            }
            else if (json[index] == '}') {
                index++;
                return objDict;
            }
            else {
                throw new JsonException($"Ожидалась '}}' после значения ключа '{key}' в поизиции '{index}'");
            }
        }

        throw new JsonException("Неожиданный конец строки при парсинге объекта.");
    }

    private string ParseString(string json, ref int index)
    {
        if (json[index] != '\"')
            throw new JsonException($"Ожидалась '\"' в позиции {index}");
        index++;
        var sb = new StringBuilder();
        bool escaped = false;
        while (index < json.Length) {
            char currChar = json[index];
            index++;
            if (escaped) {
                switch (currChar) {
                    case '"': sb.Append('"'); break;
                    case '\\': sb.Append('\\'); break;
                    case '/': sb.Append('/'); break;
                    case 'b': sb.Append('\b'); break;
                    case 'f': sb.Append('\f'); break;
                    case 'n': sb.Append('\n'); break;
                    case 'r': sb.Append('\r'); break;
                    case 't': sb.Append('\t'); break;
                    default:
                        sb.Append(currChar);
                        break;
                }

                escaped = false;
            }
            else if (currChar == '\\') {
                escaped = true;
            }
            else if (currChar == '"') {
                return sb.ToString();
            }
            else {
                sb.Append(currChar);
            }
        }

        throw new JsonException("Неожиданный конец строки при парсинге строки (отсутствует закрывающая кавычка).");
    }

    private object? ParseLiteral(string json, ref int index, string literal)
    {
        if (index + literal.Length > json.Length || json.Substring(index, literal.Length) != literal) {
            throw new JsonException($"Ожидался литерал '{literal}' в позиции {index}");
        }

        index += literal.Length;
        switch (literal) {
            case "true": return true;
            case "false": return false;
            case "null": return null;
            default: throw new InvalidOperationException("Неизвестный литерал");
        }
    }

    private void SkipWhitespace(string json, ref int index)
    {
        while (index < json.Length && char.IsWhiteSpace(json[index])) {
            index++;
        }
    }
}