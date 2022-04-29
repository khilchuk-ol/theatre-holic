using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Repository;

namespace TheatreHolic.Data.UnitOfWork.Impl;

public class UnitOfWork : Data.UnitOfWork.UnitOfWork, IDisposable
{
    protected DbContext _context;

    public override IShowRepository ShowRepository { get; }
    public override ITicketRepository TicketRepository { get; }
    public override IAuthorRepository AuthorRepository { get; }
    public override IGenreRepository GenreRepository { get; }

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

    public override void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public override void Save()
    {
        _context.SaveChanges();
    }
}