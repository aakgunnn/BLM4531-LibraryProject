using Library.Net2.Models.Domain;
using Library.Net2.Models.DTOs.Categories;
using Library.Net2.Repositories;

namespace Library.Net2.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        var categoryDtos = new List<CategoryDto>();

        foreach (var category in categories)
        {
            var bookCount = (await _unitOfWork.Books.FindAsync(b => b.CategoryId == category.Id)).Count();
            categoryDtos.Add(MapToDto(category, bookCount));
        }

        return categoryDtos;
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.FindAsync(c => c.IsActive);
        var categoryDtos = new List<CategoryDto>();

        foreach (var category in categories)
        {
            var bookCount = (await _unitOfWork.Books.FindAsync(b => b.CategoryId == category.Id)).Count();
            categoryDtos.Add(MapToDto(category, bookCount));
        }

        return categoryDtos;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return null;

        var bookCount = (await _unitOfWork.Books.FindAsync(b => b.CategoryId == category.Id)).Count();
        return MapToDto(category, bookCount);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto dto)
    {
        // Check if category name already exists
        var existingCategory = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Name == dto.Name);
        if (existingCategory != null)
        {
            throw new InvalidOperationException("Bu isimde bir kategori zaten mevcut");
        }

        var category = new Category
        {
            Name = dto.Name,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(category, 0);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(int id, CreateCategoryDto dto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
        {
            throw new InvalidOperationException("Kategori bulunamadı");
        }

        // Check if new name conflicts with another category
        var existingCategory = await _unitOfWork.Categories.FirstOrDefaultAsync(c => c.Name == dto.Name && c.Id != id);
        if (existingCategory != null)
        {
            throw new InvalidOperationException("Bu isimde bir kategori zaten mevcut");
        }

        category.Name = dto.Name;
        category.IsActive = dto.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();

        var bookCount = (await _unitOfWork.Books.FindAsync(b => b.CategoryId == category.Id)).Count();
        return MapToDto(category, bookCount);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return false;

        // Check if category has books
        var hasBooks = await _unitOfWork.Books.AnyAsync(b => b.CategoryId == id);
        if (hasBooks)
        {
            throw new InvalidOperationException("Bu kategoriye ait kitaplar var. Önce kitapları silmelisiniz.");
        }

        _unitOfWork.Categories.Remove(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private static CategoryDto MapToDto(Category category, int bookCount)
    {
        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            IsActive = category.IsActive,
            BookCount = bookCount
        };
    }
}

