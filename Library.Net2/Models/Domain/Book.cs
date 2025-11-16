namespace Library.Net2.Models.Domain;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string? ISBN { get; set; }
    public int? PublishYear { get; set; }
    public string? ImageUrl { get; set; }  // Kitap kapağı resmi
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public Category Category { get; set; } = null!;
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}

