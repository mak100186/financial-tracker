namespace Maxx.FinancialTracker.Repository.Models.Contracts;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public interface ICollectionItem
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }
}