namespace Library.Net2.Models.DTOs.Books;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public int? PublishYear { get; set; }
    public string? ImageUrl { get; set; }  // Kitap kapağı resmi
    public bool IsAvailable { get; set; }
}

