using Microsoft.EntityFrameworkCore;
using TheatreHolic.Data.Models;

namespace TheatreHolic.Data.Repository.Impl;

public class AuthorRepository : Repository<int, Author>, IAuthorRepository
{
    public AuthorRepository(DbContext context) : base(context)
    {
    }
}