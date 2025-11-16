using Library.Net2.Data;
using Library.Net2.Models.Domain;
using Microsoft.EntityFrameworkCore.Storage;

namespace Library.Net2.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly LibraryDbContext _context;
    private IDbContextTransaction? _transaction;

    public IRepository<User> Users { get; }
    public IRepository<Book> Books { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Loan> Loans { get; }

    public UnitOfWork(LibraryDbContext context)
    {
        _context = context;
        Users = new Repository<User>(_context);
        Books = new Repository<Book>(_context);
        Categories = new Repository<Category>(_context);
        Loans = new Repository<Loan>(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}

