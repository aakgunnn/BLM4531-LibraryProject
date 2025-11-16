using Library.Net2.Models.Domain;

namespace Library.Net2.Repositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Book> Books { get; }
    IRepository<Category> Categories { get; }
    IRepository<Loan> Loans { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}

