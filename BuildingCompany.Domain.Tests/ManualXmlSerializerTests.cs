using BuildingCompany.Serialization;

namespace BuildingCompany.Domain.Tests;

public class ManualXmlSerializerTests
{
    private readonly ISerializer _serializer;

    public ManualXmlSerializerTests()
    {
        _serializer = new ManualXmlSerializer();
    }
    
    [Fact]
    public void Serialize_SimpleObject_ShouldGenerateCorrectXml()
    {
        var obj = new ManualJsonSerializerTests.SimpleObject()
        {
            Id = 123,
            IsActive = true,
            Price = 99.99m,
            Name = "Test Name",
            Ratio = 0.5
        };
        string xml = _serializer.Serialize(obj);
    }
}