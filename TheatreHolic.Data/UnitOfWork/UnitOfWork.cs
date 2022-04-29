using TheatreHolic.Data.Repository;

namespace TheatreHolic.Data.UnitOfWork;

public abstract class UnitOfWork : IDisposable
{
    public abstract IShowRepository ShowRepository { get; }
    public abstract ITicketRepository TicketRepository { get; }
    public abstract IAuthorRepository AuthorRepository { get; }
    public abstract IGenreRepository GenreRepository { get; }

    public abstract void Save();
    public abstract void Dispose();
}