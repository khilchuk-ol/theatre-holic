using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Models;

namespace TheatreHolic.Data.Repository.Impl;

public class GenreRepository : Repository<int, Genre>, IGenreRepository
{
    public GenreRepository(DbContext context) : base(context)
    {
    }
}