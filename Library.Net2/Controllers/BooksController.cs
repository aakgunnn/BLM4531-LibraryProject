using Library.Net2.Models.DTOs.Books;
using Library.Net2.Models.DTOs.Common;
using Library.Net2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Net2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    /// <summary>
    /// Tüm kitapları listele
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookDto>>>> GetAllBooks()
    {
        try
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(ApiResponse<IEnumerable<BookDto>>.SuccessResponse(books));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<BookDto>>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }

    /// <summary>
    /// Kitap ara (başlık, yazar, kategori)
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookDto>>>> SearchBooks(
        [FromQuery] string? q,
        [FromQuery] int? categoryId,
        [FromQuery] string? author)
    {
        try
        {
            var books = await _bookService.SearchBooksAsync(q, categoryId, author);
            return Ok(ApiResponse<IEnumerable<BookDto>>.SuccessResponse(books));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<IEnumerable<BookDto>>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }

    /// <summary>
    /// ID'ye göre kitap getir
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BookDto>>> GetBookById(int id)
    {
        try
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound(ApiResponse<BookDto>.ErrorResponse("Kitap bulunamadı"));
            }

            return Ok(ApiResponse<BookDto>.SuccessResponse(book));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<BookDto>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }

    /// <summary>
    /// Yeni kitap ekle (Admin)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<BookDto>>> CreateBook([FromBody] CreateBookDto dto)
    {
        try
        {
            var book = await _bookService.CreateBookAsync(dto);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, 
                ApiResponse<BookDto>.SuccessResponse(book, "Kitap başarıyla eklendi"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<BookDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<BookDto>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }

    /// <summary>
    /// Kitap güncelle (Admin)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<BookDto>>> UpdateBook(int id, [FromBody] UpdateBookDto dto)
    {
        try
        {
            var book = await _bookService.UpdateBookAsync(id, dto);
            return Ok(ApiResponse<BookDto>.SuccessResponse(book, "Kitap başarıyla güncellendi"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<BookDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<BookDto>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }

    /// <summary>
    /// Kitap sil (Admin)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteBook(int id)
    {
        try
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse("Kitap bulunamadı"));
            }

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Kitap başarıyla silindi"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("Bir hata oluştu: " + ex.Message));
        }
    }
}

