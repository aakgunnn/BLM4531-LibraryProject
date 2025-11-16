using Library.Net2.Models.Domain;
using Library.Net2.Models.DTOs.Loans;
using Library.Net2.Models.Enums;
using Library.Net2.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Net2.Services;

public class LoanService : ILoanService
{
    private readonly IUnitOfWork _unitOfWork;

    public LoanService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // ===== USER OPERATIONS =====

    public async Task<LoanResponseDto> CreateLoanRequestAsync(int userId, CreateLoanDto dto)
    {
        // 1. Kitap var mı ve müsait mi kontrol et
        var book = await _unitOfWork.Books.GetByIdAsync(dto.BookId);
        if (book == null)
            throw new Exception("Kitap bulunamadı.");

        if (!book.IsAvailable)
            throw new Exception("Bu kitap şu anda müsait değil.");

        // 2. Kullanıcının bu kitaptan aktif ödüncü var mı kontrol et
        var activeLoans = await _unitOfWork.Loans
            .GetAllAsync(
                l => l.UserId == userId && 
                     l.BookId == dto.BookId && 
                     (l.Status == LoanStatus.Pending || l.Status == LoanStatus.Borrowed));

        if (activeLoans.Any())
            throw new Exception("Bu kitaptan zaten aktif ödüncünüz var.");

        // 3. Yeni ödünç kaydı oluştur
        var loan = new Loan
        {
            UserId = userId,
            BookId = dto.BookId,
            LoanDate = DateTime.UtcNow,
            Status = LoanStatus.Pending
        };

        await _unitOfWork.Loans.AddAsync(loan);
        await _unitOfWork.SaveChangesAsync();

        // 4. Response oluştur
        return await MapToResponseDto(loan);
    }

    public async Task<IEnumerable<LoanResponseDto>> GetUserLoansAsync(int userId)
    {
        var loans = await _unitOfWork.Loans
            .GetAllAsync(
                l => l.UserId == userId,
                q => q.Include(l => l.Book).ThenInclude(b => b.Category).Include(l => l.User));

        return loans.Select(MapToResponseDtoSync).OrderByDescending(l => l.LoanDate);
    }

    public async Task<LoanResponseDto> ReturnLoanAsync(int loanId, int userId)
    {
        var loan = await _unitOfWork.Loans
            .GetAllAsync(
                l => l.Id == loanId,
                q => q.Include(l => l.Book).Include(l => l.User));

        var loanEntity = loan.FirstOrDefault();
        if (loanEntity == null)
            throw new Exception("Ödünç kaydı bulunamadı.");

        if (loanEntity.UserId != userId)
            throw new Exception("Bu ödünç kaydına erişim yetkiniz yok.");

        if (loanEntity.Status != LoanStatus.Borrowed)
            throw new Exception("Sadece ödünç alınmış kitaplar iade edilebilir.");

        // İade işlemi
        loanEntity.Status = LoanStatus.Returned;
        loanEntity.ReturnDate = DateTime.UtcNow;
        loanEntity.UpdatedAt = DateTime.UtcNow;

        // Kitabı tekrar müsait yap
        loanEntity.Book.IsAvailable = true;
        loanEntity.Book.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Loans.Update(loanEntity);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponseDtoSync(loanEntity);
    }

    // ===== ADMIN OPERATIONS =====

    public async Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync(LoanStatus? status = null, int? userId = null, int? categoryId = null)
    {
        var loans = await _unitOfWork.Loans
            .GetAllAsync(
                l => (!status.HasValue || l.Status == status.Value) &&
                     (!userId.HasValue || l.UserId == userId.Value) &&
                     (!categoryId.HasValue || l.Book.CategoryId == categoryId.Value),
                q => q.Include(l => l.Book).ThenInclude(b => b.Category).Include(l => l.User));

        return loans.Select(MapToResponseDtoSync).OrderByDescending(l => l.LoanDate);
    }

    public async Task<LoanResponseDto> ApproveLoanAsync(int loanId, ApproveLoanDto dto)
    {
        var loans = await _unitOfWork.Loans
            .GetAllAsync(
                l => l.Id == loanId,
                q => q.Include(l => l.Book).ThenInclude(b => b.Category).Include(l => l.User));

        var loan = loans.FirstOrDefault();
        if (loan == null)
            throw new Exception("Ödünç kaydı bulunamadı.");

        if (loan.Status != LoanStatus.Pending)
            throw new Exception("Sadece beklemedeki talepler onaylanabilir.");

        if (!loan.Book.IsAvailable)
            throw new Exception("Kitap artık müsait değil.");

        // Onaylama işlemi
        loan.Status = LoanStatus.Borrowed;
        loan.DueDate = DateTime.UtcNow.AddDays(dto.DurationDays);
        loan.AdminNote = dto.AdminNote;
        loan.UpdatedAt = DateTime.UtcNow;

        // Kitabı müsait olmaktan çıkar
        loan.Book.IsAvailable = false;
        loan.Book.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Loans.Update(loan);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponseDtoSync(loan);
    }

    public async Task<LoanResponseDto> RejectLoanAsync(int loanId, RejectLoanDto dto)
    {
        try
        {
            var loans = await _unitOfWork.Loans
                .GetAllAsync(
                    l => l.Id == loanId,
                    q => q.Include(l => l.Book).ThenInclude(b => b.Category).Include(l => l.User));

            var loan = loans.FirstOrDefault();
            if (loan == null)
                throw new Exception("Ödünç kaydı bulunamadı.");

            if (loan.Status != LoanStatus.Pending)
                throw new Exception("Sadece beklemedeki talepler reddedilebilir.");

            // Reddetme işlemi
            loan.Status = LoanStatus.Cancelled;
            loan.AdminNote = dto.AdminNote ?? string.Empty; // Null check
            loan.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Loans.Update(loan);
            await _unitOfWork.SaveChangesAsync();

            return MapToResponseDtoSync(loan);
        }
        catch (Exception ex)
        {
            // Detaylı hata mesajı
            throw new Exception($"Ödünç reddetme hatası: {ex.Message} | Inner: {ex.InnerException?.Message}");
        }
    }

    public async Task<IEnumerable<LoanResponseDto>> GetLateLoansAsync()
    {
        var loans = await _unitOfWork.Loans
            .GetAllAsync(
                l => l.Status == LoanStatus.Borrowed && 
                     l.DueDate.HasValue && 
                     l.DueDate.Value < DateTime.UtcNow,
                q => q.Include(l => l.Book).ThenInclude(b => b.Category).Include(l => l.User));

        return loans.Select(MapToResponseDtoSync).OrderBy(l => l.DueDate ?? DateTime.MaxValue);
    }

    public async Task<LoanResponseDto?> GetLoanByIdAsync(int loanId)
    {
        var loans = await _unitOfWork.Loans
            .GetAllAsync(
                l => l.Id == loanId,
                q => q.Include(l => l.Book).ThenInclude(b => b.Category).Include(l => l.User));

        var loan = loans.FirstOrDefault();
        return loan != null ? MapToResponseDtoSync(loan) : null;
    }

    // ===== HELPER METHODS =====

    private async Task<LoanResponseDto> MapToResponseDto(Loan loan)
    {
        var loans = await _unitOfWork.Loans
            .GetAllAsync(
                l => l.Id == loan.Id,
                q => q.Include(l => l.Book).ThenInclude(b => b.Category).Include(l => l.User));

        var fullLoan = loans.FirstOrDefault();
        if (fullLoan == null)
            throw new Exception("Loan not found after creation");

        return MapToResponseDtoSync(fullLoan);
    }

    private LoanResponseDto MapToResponseDtoSync(Loan loan)
    {
        return new LoanResponseDto
        {
            Id = loan.Id,
            BookId = loan.BookId,
            BookTitle = loan.Book?.Title ?? "N/A",
            BookAuthor = loan.Book?.Author ?? "N/A",
            CategoryName = loan.Book?.Category?.Name ?? "N/A",
            UserId = loan.UserId,
            UserFullName = loan.User?.FullName ?? "N/A",
            UserEmail = loan.User?.Email ?? "N/A",
            LoanDate = loan.LoanDate,
            DueDate = loan.DueDate,
            ReturnDate = loan.ReturnDate,
            Status = loan.Status,
            AdminNote = loan.AdminNote
        };
    }
}

