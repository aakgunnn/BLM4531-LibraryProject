using System.ComponentModel.DataAnnotations;

namespace Library.Net2.Models.DTOs.Books;

public class UpdateBookDto
{
    [Required(ErrorMessage = "Kitap başlığı zorunludur")]
    [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Yazar adı zorunludur")]
    [StringLength(100, ErrorMessage = "Yazar adı en fazla 100 karakter olabilir")]
    public string Author { get; set; } = string.Empty;

    [Required(ErrorMessage = "Kategori seçimi zorunludur")]
    public int CategoryId { get; set; }

    [StringLength(20, ErrorMessage = "ISBN en fazla 20 karakter olabilir")]
    public string? ISBN { get; set; }

    [Range(1000, 9999, ErrorMessage = "Yayın yılı 1000 ile 9999 arasında olmalıdır")]
    public int? PublishYear { get; set; }

    [StringLength(500, ErrorMessage = "Resim URL'i en fazla 500 karakter olabilir")]
    public string? ImageUrl { get; set; }  // Kitap kapağı resmi

    public bool IsAvailable { get; set; }
}

