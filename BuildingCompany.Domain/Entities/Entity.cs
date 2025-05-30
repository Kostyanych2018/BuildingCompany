using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BuildingCompany.Domain.Entities;

public class Entity
{
    [BsonId]
    public ObjectId Id { get; set; }
}