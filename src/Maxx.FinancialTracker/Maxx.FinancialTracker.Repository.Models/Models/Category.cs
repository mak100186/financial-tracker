namespace Maxx.FinancialTracker.Repository.Models.Models;

using System.ComponentModel.DataAnnotations;

using Contracts;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Category : ICollectionItem
{
    [Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }

    public string Icon { get; set; } = "";

    public string Type { get; set; } = "Expense";

    public string? TitleWithIcon => this.Icon + " " + this.Title;

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}