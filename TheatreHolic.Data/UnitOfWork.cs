using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Repository;

namespace TheatreHolic.Data;

public class UnitOfWork : IDisposable
{
    protected DbContext _context;

    public IShowRepository ShowRepository { get; }
    public ITicketRepository TicketRepository { get; }
    public IAuthorRepository AuthorRepository { get; }
    public IGenreRepository GenreRepository { get; }

    private bool _isDisposed = false;

    public UnitOfWork(DbContext context, IShowRepository showRepository, ITicketRepository ticketRepository,
        IAuthorRepository authorRepository, IGenreRepository genreRepository)
    {
        _context = context;

        ShowRepository = showRepository;
        TicketRepository = ticketRepository;
        AuthorRepository = authorRepository;
        GenreRepository = genreRepository;
    }

    public virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}