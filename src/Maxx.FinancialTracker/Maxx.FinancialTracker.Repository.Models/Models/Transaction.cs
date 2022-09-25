namespace Maxx.FinancialTracker.Repository.Models.Models;

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

using Contracts;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Transaction : ICollectionItem
{
    [Required]
    public string? CategoryId { get; set; }

    public Category? Category { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Amount should be greater than 0.")]
    public int Amount { get; set; }

    public string? Note { get; set; }

    public DateTime Date { get; set; } = DateTime.Now;

    public string CategoryTitleWithIcon => this.Category == null ? "" : this.Category.Icon + " " + this.Category.Title;

    public string FormattedAmount => (this.Category == null || this.Category.Type == "Expense" ? "- " : "+ ") + this.Amount.ToString("C0");

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}