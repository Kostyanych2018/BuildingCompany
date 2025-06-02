using System.Globalization;
using BuildingCompany.Domain.Entities;
using BuildingCompany.Serialization;

namespace BuildingCompany.Domain.Tests.DomainTests;

public class ManualJsonSerializerTests
{
    private readonly ISerializer _serializer;

    public ManualJsonSerializerTests()
    {
        _serializer = new ManualJsonSerializer();
    }

    [Fact]
    public void Serialize_ShouldHandleInteger()
    {
        int value = 42;
        string actualJson = _serializer.Serialize(value);
        Assert.Equal("42", actualJson);
    }

    [Fact]
    public void Serialize_ShouldHandleFloatingPoint()
    {
        double value = 3.14;
        string actualJson = _serializer.Serialize(value);
        Assert.Equal("3.14", actualJson);
    }

    [Fact]
    public void Serialize_ShouldHandleDecimal()
    {
        decimal value = 123.45m;
        string actualJson = _serializer.Serialize(value);
        Assert.Equal("123.45", actualJson);
    }

    [Fact]
    public void Serialize_ShouldHandleBooleanTrue()
    {
        bool value = true;
        string actualJson = _serializer.Serialize(value);
        Assert.Equal("true", actualJson);
    }

    [Fact]
    public void Serialize_ShouldHandleString()
    {
        string value = "Hello, world!";
        string actualJson = _serializer.Serialize(value);
        Assert.Equal("\"Hello, world!\"", actualJson);
    }

    [Fact]
    public void Serialize_ShouldHandleEmptyString()
    {
        string value = "";
        string actualJson = _serializer.Serialize(value);
        Assert.Equal("\"\"", actualJson);
    }

    [Fact]
    public void Serialize_ShouldHandleListOfStrings()
    {
        var list = new List<string> { "apple", "banana", "cherry" };
        string actualJson = _serializer.Serialize(list);
        Assert.Equal("[\"apple\",\"banana\",\"cherry\"]", actualJson);
    }


    [Fact]
    public void Serialize_ShouldHandleNull()
    {
        Project? nullProject = null;
        string actualJson = _serializer.Serialize(nullProject);
        Assert.Equal("null", actualJson);
    }

    [Fact]
    public void Serialize_ShouldHandleList()
    {
        var list = new List<int> { 1, 2, 3 };
        string actualJson = _serializer.Serialize(list);
        Assert.Equal("[1,2,3]", actualJson);
    }

    public abstract class BaseClass
    {
        public int BaseId { get; set; }
        public string? BaseName { get; set; }

        protected BaseClass(int baseId, string? baseName)
        {
            BaseId = baseId;
            BaseName = baseName;
        }
    }

    public class DerivedClass : BaseClass
    {
        public decimal DerivedValue { get; set; }
        public bool IsDerived { get; set; }
        public List<string> Items { get; set; } = [];

        public DerivedClass(int baseId,
            string? baseName,
            decimal derivedValue,
            bool isDerived,
            List<string>? items = null) : base(baseId, baseName)
        {
            DerivedValue = derivedValue;
            BaseName = baseName;
            IsDerived = isDerived;
            if (items is not null) {
                Items = new List<string>(items);
            }
        }
    }

    [Fact]
    public void Serialize_ShouldHandleClassHierarchy()
    {
        var derivedObject = new DerivedClass(1,
            "Base Name",
            11.3m,
            true,
            ["item1", "item2"]);
        string json = _serializer.Serialize(derivedObject);
        Assert.Contains("\"BaseId\":1", json);
        Assert.Contains("\"BaseName\":\"Base Name\"", json);
        Assert.Contains("\"DerivedValue\":11.3", json);
        Assert.Contains("\"IsDerived\":true", json);
        Assert.Contains("\"Items\":[\"item1\",\"item2\"]", json);
    }

    public class SimpleObject
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public double Ratio { get; set; }
    }

    [Fact]
    public void Deserialize_SimpleObject_ShouldReturnCorrectObject()
    {
        string json = @"{
            ""Id"": 123,
            ""Name"": ""Test Item"",
            ""IsActive"": true,
            ""Price"": 99.99,
            ""Ratio"": 0.5,
        }";
        SimpleObject? obj = _serializer.Deserialize<SimpleObject>(json);
        Assert.NotNull(obj);
        Assert.Equal(123, obj.Id);
        Assert.Equal("Test Item", obj.Name);
        Assert.True(obj.IsActive);
        Assert.Equal(99.99m, obj.Price);
        Assert.Equal(0.5, obj.Ratio);
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Customer? CustomerInfo { get; set; }
        public List<Item>? Items { get; set; }
    }

    public class Item
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
    }

    [Fact]
    public void Deserialize_ComplexNestedStructure_ShouldReturnCorrectObject()
    {
        string json = @"{
            ""OrderId"": 1001,
            ""OrderDate"": ""2023-10-28"",
            ""CustomerInfo"": {
                ""CustomerId"": 55,
                ""Name"": ""Alice Smith"",
                ""Email"": ""alice@example.com""
            },
            ""Items"": [
                {""Name"": ""Book"", ""Price"": 35.99},
                {""Name"": ""Pen"", ""Price"": 2.50}
            ],
        }";
        Order? result = _serializer.Deserialize<Order>(json);
        Assert.NotNull(result);
        Assert.Equal(1001, result.OrderId);
        Assert.Equal(DateTime.Parse("2023-10-28", CultureInfo.InvariantCulture), result.OrderDate);

        Assert.NotNull(result.CustomerInfo);
        Assert.Equal(55, result.CustomerInfo.CustomerId);
        Assert.Equal("Alice Smith", result.CustomerInfo.Name);
        Assert.Equal("alice@example.com", result.CustomerInfo.Email);

        Assert.NotNull(result.Items);
        Assert.Equal(2, result.Items.Count);
        Assert.NotNull(result.Items[0]);
        Assert.Equal("Book", result.Items[0].Name);
        Assert.Equal(35.99m, result.Items[0].Price);
        Assert.NotNull(result.Items[1]);
        Assert.Equal("Pen", result.Items[1].Name);
        Assert.Equal(2.50m, result.Items[1].Price);
    }
}